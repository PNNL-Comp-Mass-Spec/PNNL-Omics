using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureMatcher.Data;

namespace PNNLOmics.Algorithms.FeatureMatcher
{
    public class FeatureMatcher<T, U>
        where T : Feature, new()
        where U : Feature, new()
    {
        #region Members
        private FeatureMatcherParameters m_matchParameters;

        private List<FeatureMatch<T, U>> m_matchList;
        private List<FeatureMatch<T, U>> m_shiftedMatchList;
        private List<T> m_observedFeatureList;
        private List<U> m_targetFeatureList;

        private double m_shiftFDR;
        private double m_shiftConservativeFDR;
        private double m_errorHistogramFDR;

        private List<STACInformation> m_stacParametersList;
        private List<STACFDR> m_stacFDRList;
        private List<FeatureMatcherTolerances> m_refinedTolerancesList;
        private SLiCInformation m_slicParameters;

        const int MIN_MATCHES_FOR_NORMAL_ASSUMPTION = 50;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the initial parameters used for matching.
        /// </summary>
        public FeatureMatcherParameters MatchParameters
        {
            get { return m_matchParameters; }
            set { m_matchParameters = value; }
        }
        /// <summary>
        /// Gets the list of feature matches.
        /// </summary>
        public List<FeatureMatch<T, U>> MatchList
        {
            get { return m_matchList; }
        }
        /// <summary>
        /// Gets the list of features matched with a shift.
        /// </summary>
        public List<FeatureMatch<T, U>> ShiftedMatchList
        {
            get { return m_shiftedMatchList; }
        }

        /// <summary>
        /// Gets the FDR calculated by using a fixed shift.  Calculated as (# shifted matches)/(# shifted matches + # non-shifted matches).
        /// </summary>
        public double ShiftFDR
        {
            get { return m_shiftFDR; }
        }
        /// <summary>
        /// Gets the FDR calculated by using a fixed shift.  Calculated as 2*(# shifted matches)/(# shifted matches + # non-shifted matches).
        /// </summary>
        public double ShiftConservativeFDR
        {
            get { return m_shiftConservativeFDR; }
        }
        /// <summary>
        /// Gets the FDR calculated by using a mass error histogram.
        /// </summary>
        public double ErrorHistogramFDR
        {
            get { return m_errorHistogramFDR; }
        }

        /// <summary>
        /// Gets the list of parameters trained by STAC.  Each entry is a different charge state.
        /// </summary>
        public List<STACInformation> STACParameterList
        {
            get { return m_stacParametersList; }
        }
        /// <summary>
        /// Get the STAC FDR table.
        /// </summary>
        public List<STACFDR> STACFDRTable
        {
            get { return m_stacFDRList; }
        }
        /// <summary>
        /// Gets the list of refined tolerances used for SLiC and shift.  Each entry is a different charge state.
        /// </summary>
        public List<FeatureMatcherTolerances> RefinedToleranceList
        {
            get { return m_refinedTolerancesList; }
        }
        /// <summary>
        /// Gets the parameters used in calculating SLiC.
        /// </summary>
        public SLiCInformation SLiCParameters
        {
            get { return m_slicParameters; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor for use in matching features.  Uses passed parameters to calculate desired results.
        /// </summary>
        /// <param name="observedFeatureList">List of features observed in an analysis.  Generally UMC or UMCCluster.</param>
        /// <param name="targetFeatureList">List of features to be matched to.  Generally AMTTags.</param>
        /// <param name="matchParameters">FeatureMatcherParameters object containing initial parameters and algorithm settings.</param>
        public FeatureMatcher(List<T> observedFeatureList, List<U> targetFeatureList, FeatureMatcherParameters matchParameters)
        {
            Clear();
            m_observedFeatureList = observedFeatureList;
            m_targetFeatureList = targetFeatureList;
            m_matchParameters = matchParameters;
            MatchFeatures();
        }
        #endregion

        #region Private functions
        /// <summary>
        /// Reset all algorithm results to default values.
        /// </summary>
        private void Clear()
        {
            m_matchParameters = new FeatureMatcherParameters();
            m_shiftFDR = 0;
            m_shiftConservativeFDR = 0;
            m_errorHistogramFDR = 0;
            m_stacParametersList = new List<STACInformation>();
            m_refinedTolerancesList = new List<FeatureMatcherTolerances>();
            m_slicParameters = new SLiCInformation();
            m_matchList = new List<FeatureMatch<T, U>>();
            m_shiftedMatchList = new List<FeatureMatch<T, U>>();
        }
        /// <summary>
        /// Set the list of charge states to be used in IMS calculations.
        /// </summary>
        private void SetChargeStateList()
        {
            m_matchParameters.ChargeStateList.Clear();
            foreach (T observedFeature in m_observedFeatureList)
            {
                if (!m_matchParameters.ChargeStateList.Contains(observedFeature.ChargeState))
                {
                    m_matchParameters.ChargeStateList.Add(observedFeature.ChargeState);
                }
            }
        }
        /// <summary>
        /// Find a list of matches between two lists.
        /// </summary>
        /// <param name="shortObservedList">List of observed features.  Possibly a subset of the entire list corresponding to a particular charge state.</param>
        /// <param name="shortTargetList">List of target features.  Possibly a subset of the entire list corresponding to a particular charge state.</param>
        /// <param name="tolerances">Tolerances to be used for matching.</param>
        /// <param name="useEllipsoid">Whether or not to use an ellipsoidal matching region.  If false, uses a rectangular match region.</param>
        /// <param name="shiftAmount">A fixed shift amount to use for populating the shifted match list.</param>
        /// <returns>A list of type FeatureMatch containing matches within the defined region.</returns>
        private List<FeatureMatch<T, U>> FindMatches(List<T> shortObservedList, List<U> shortTargetList,
                                                        FeatureMatcherTolerances tolerances, bool useEllipsoid, double shiftAmount)
        {
            List<FeatureMatch<T, U>> matchList = new List<FeatureMatch<T, U>>();

            foreach (T feature in shortObservedList)
            {
                foreach (U massTag in shortTargetList)
                {
                    U shiftedTag = massTag;
                    shiftedTag.MassMonoisotopicAligned += shiftAmount;
                    FeatureMatch<T, U> match = new FeatureMatch<T, U>();
                    match.AddFeatures(feature, shiftedTag, m_matchParameters.UseDriftTime, (shiftAmount > 0));
                    if (match.InRegion(tolerances, useEllipsoid))
                    {
                        matchList.Add(match);
                    }
                }
            }
            return matchList;
        }
        /// <summary>
        /// Refine tolerances for use in SLiC and shift match methods.
        /// </summary>
        /// <param name="matchList">List of matches used to train the refined tolerances.</param>
        /// <returns>Refined tolerances for use in SLiC and shift match methods.</returns>
        private FeatureMatcherTolerances FindOptimalTolerances(List<FeatureMatch<T, U>> matchList)
        {
            List<Matrix> differenceMatrixList = new List<Matrix>();
            for (int i = 0; i <= matchList.Count; i++)
            {
                differenceMatrixList.Add(matchList[i].ReducedDifferenceVector);
            }

            int rows = differenceMatrixList[0].RowCount;
            Matrix meanVector = new Matrix(rows, 1, 0.0);
            Matrix covarianceMatrix = new Matrix(rows, 1.0);
            double mixtureParameter = 0.5;

            Utilities.ExpectationMaximization.NormalUniformMixture(differenceMatrixList, ref meanVector, ref covarianceMatrix, m_matchParameters.UserTolerances.AsVector(m_matchParameters.UseDriftTime), ref mixtureParameter, true);

            m_slicParameters.MassPPMStDev = Math.Sqrt(covarianceMatrix[0, 0]);
            m_slicParameters.NETStDev = Math.Sqrt(covarianceMatrix[1, 1]);
            if (m_matchParameters.UseDriftTime)
                m_slicParameters.DriftTimeStDev = (float)Math.Sqrt(covarianceMatrix[2, 2]);

            FeatureMatcherTolerances refinedTolerances = new FeatureMatcherTolerances((2.5 * m_slicParameters.MassPPMStDev), (2.5 * m_slicParameters.NETStDev),
                                                            (float)(2.5 * m_slicParameters.DriftTimeStDev));
            refinedTolerances.Refined = true;
            return refinedTolerances;
        }
        /// <summary>
        /// Populate the FDR table with default cutoffs.
        /// </summary>
        private void SetSTACCutoffs()
        {
            m_stacFDRList.Clear();
            for (double cutoff = 0.97; cutoff > 0.90; cutoff -= 0.01)
            {
                STACFDR tableLine = new STACFDR(cutoff);
                m_stacFDRList.Add(tableLine);
            }
            for (double cutoff = 0.90; cutoff >= 0; cutoff -= 0.10)
            {
                STACFDR tableLine = new STACFDR(cutoff);
                m_stacFDRList.Add(tableLine);
            }
        }
        /// <summary>
        /// Fills in the values for the STAC FDR table.
        /// </summary>
        private void PopulateSTACFDRTable()
        {
            SetSTACCutoffs();
            m_matchList.Sort(FeatureMatch<T,U>.STACComparison);
            double falseDiscoveries = 0.0;
            int cutoffIndex = 0;
            int matches = 0;
            double fdr = 0.0;
            foreach (FeatureMatch<T, U> match in m_matchList)
            {
                double stac = match.STACScore;
                if (stac > m_stacFDRList[cutoffIndex].Cutoff)
                {
                    falseDiscoveries += 1 - stac;
                    matches++;
                    double newFDR = falseDiscoveries/matches;
                    if (fdr < 0.01 && newFDR >= 0.01)
                    {
                        STACFDR newLine = new STACFDR(Math.Round(stac-0.00005,4));
                        newLine.FillLine(newFDR, matches, (int)falseDiscoveries);
                        m_stacFDRList.Insert(cutoffIndex, newLine);
                        cutoffIndex++;
                    }
                    else if (fdr < 0.05 && newFDR >= 0.05)
                    {
                        STACFDR newLine = new STACFDR(Math.Round(stac - 0.00005, 4));
                        newLine.FillLine(newFDR, matches, (int)falseDiscoveries);
                        m_stacFDRList.Insert(cutoffIndex, newLine);
                        cutoffIndex++;
                    }
                    else if (fdr < 0.10 && newFDR >= 0.10)
                    {
                        STACFDR newLine = new STACFDR(Math.Round(stac - 0.00005, 4));
                        newLine.FillLine(newFDR, matches, (int)falseDiscoveries);
                        m_stacFDRList.Insert(cutoffIndex, newLine);
                        cutoffIndex++;
                    }
                    fdr = falseDiscoveries / matches;
                }
                else
                {
                    fdr = falseDiscoveries / matches;
                    m_stacFDRList[cutoffIndex].FillLine(fdr, matches, (int)Math.Round(falseDiscoveries, 0));
                    cutoffIndex++;
                    falseDiscoveries += 1 - stac;
                    matches++;
                }
            }
        }
        #endregion

        #region Public functions
        /// <summary>
        /// Function to call to re-calculate algorithm results.  Called within constructor by default.
        /// </summary>
        public void MatchFeatures()
        {
            if (m_matchParameters.UseDriftTime)
            {
                if (m_matchParameters.ChargeStateList.Count == 0)
                {
                    SetChargeStateList();
                }
                int chargeStateCount = m_matchParameters.ChargeStateList.Count;
                m_stacParametersList.Capacity = chargeStateCount;
                m_refinedTolerancesList.Capacity = chargeStateCount;
                for (int i = 0; i < chargeStateCount; i++)
                {
                    List<T> chargeObservedList = new List<T>();
                    foreach (T observed in m_observedFeatureList)
                    {
                        if (observed.ChargeState == m_matchParameters.ChargeStateList[i])
                        {
                            chargeObservedList.Add(observed);
                        }
                    }
                    List<U> chargeTargetList = new List<U>();
                    foreach (U target in m_targetFeatureList)
                    {
                        if (target.ChargeState == m_matchParameters.ChargeStateList[i])
                        {
                            chargeTargetList.Add(target);
                        }
                    }
                    List<FeatureMatch<T, U>> tempMatchList = new List<FeatureMatch<T, U>>();
                    tempMatchList = FindMatches(chargeObservedList, chargeTargetList, m_matchParameters.UserTolerances, false, 0);
                    bool lengthCheck = (tempMatchList.Count < MIN_MATCHES_FOR_NORMAL_ASSUMPTION);
                    if (m_matchParameters.CalculateSTAC && lengthCheck)
                    {
                        STACParameterList[i].PerformSTAC(tempMatchList, m_matchParameters.UserTolerances, m_matchParameters.UseDriftTime, m_matchParameters.UsePriors);
                    }
                    if (m_matchParameters.CalculateShiftFDR)
                    {
                        m_refinedTolerancesList[i] = FindOptimalTolerances(tempMatchList);
                        for (int j = 0; j < tempMatchList.Count; j++)
                        {
                            tempMatchList[j].InRegion(m_refinedTolerancesList[i], m_matchParameters.UseEllipsoid);
                        }
                        m_shiftedMatchList.AddRange(FindMatches(chargeObservedList, chargeTargetList, m_refinedTolerancesList[i],
                                                                    m_matchParameters.UseEllipsoid, m_matchParameters.ShiftAmount));
                    }
                    m_matchList.AddRange(tempMatchList);
                }
                PopulateSTACFDRTable(); // Need to calculate STAC FDR in a smarter way than this.
                int count = 0;
                for (int j = 0; j < m_matchList.Count; j++)
                {
                    if (m_matchList[j].WithinRefinedRegion)
                        count++;
                }
                m_shiftFDR = (1.0 * count) / m_shiftedMatchList.Count;
                m_shiftConservativeFDR = (2.0 * count) / m_shiftedMatchList.Count;
            }
            else
            {
                m_matchList.Clear();
                m_matchList = FindMatches(m_observedFeatureList, m_targetFeatureList, m_matchParameters.UserTolerances, false, 0);
                bool lengthCheck = (m_matchList.Count >= MIN_MATCHES_FOR_NORMAL_ASSUMPTION);
                if (m_matchParameters.CalculateSTAC && lengthCheck)
                {
                    STACParameterList[0].PerformSTAC(m_matchList, m_matchParameters.UserTolerances, m_matchParameters.UseDriftTime, m_matchParameters.UsePriors);
                    PopulateSTACFDRTable();
                }
                if (m_matchParameters.CalculateHistogramFDR)
                {
                    // Calculate mass error histogram FDR.
                }
                if (m_matchParameters.CalculateShiftFDR)
                {
                    int count = 0;
                    m_refinedTolerancesList[0] = FindOptimalTolerances(m_matchList);
                    for (int j = 0; j < m_matchList.Count; j++)
                    {
                        m_matchList[j].InRegion(m_refinedTolerancesList[0], m_matchParameters.UseEllipsoid);
                    }
                    for (int j = 0; j < m_matchList.Count; j++)
                    {
                        if (m_matchList[j].WithinRefinedRegion)
                            count++;
                    }
                    m_shiftedMatchList.AddRange(FindMatches(m_observedFeatureList, m_targetFeatureList, m_refinedTolerancesList[0],
                                                                m_matchParameters.UseEllipsoid, m_matchParameters.ShiftAmount));
                    m_shiftFDR = (1.0 * count) / m_shiftedMatchList.Count;
                    m_shiftConservativeFDR = (2.0 * count) / m_shiftedMatchList.Count;
                }
                if (m_matchParameters.CalculateSLiC && lengthCheck)
                {
                    if (!m_refinedTolerancesList[0].Refined)
                    {
                        m_refinedTolerancesList[0] = FindOptimalTolerances(m_matchList);
                    }
                    // Find SLiC scores.
                }
            }
        }
        #endregion
    }
}
