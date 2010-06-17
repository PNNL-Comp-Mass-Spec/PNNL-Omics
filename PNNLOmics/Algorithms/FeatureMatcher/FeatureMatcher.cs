using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureMatcher.Data;
using PNNLOmics.Utilities;

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
            // Create a list to hold the matches until they are returned.
            List<FeatureMatch<T, U>> matchList = new List<FeatureMatch<T, U>>();

            // Set indices to use when iterating over the lists.
            int observedIndex = 0;
            int targetIndex = 0;
            int lowerBound = 0;

            // Sort both lists by mass.
            shortObservedList.Sort(new Comparison<T>(Feature.MassAlignedComparison));
            shortTargetList.Sort(new Comparison<U>(Feature.MassAlignedComparison));

            // Locally store the mass tolerance.
            double massTolerancePPM = tolerances.MassTolerancePPM;

            // Iterate through the list of observed features.
            while( observedIndex < shortObservedList.Count )
            {
                // Store the current observed feature locally.
                T observedFeature = shortObservedList[observedIndex];
                // Flag variable that gets set to false when the observed mass is greater than the current mass tag by more than the tolerance.
                bool continueLoop = true;
                // Set the target feature iterator to the current lower bound.
                targetIndex = lowerBound;
                // Iterate through the list of target featrues or until the observed feature is too great.
                while( targetIndex < shortTargetList.Count && continueLoop )
                {
                    // Store the current target feature locally.
                    U shiftedTag = shortTargetList[targetIndex];
                    // Add any shift to the mass tag.
                    shiftedTag.MassMonoisotopicAligned += shiftAmount;
                    // Store the masses of both features locally.
                    double shiftedTagMass = shiftedTag.MassMonoisotopicAligned;
                    double observedMass = observedFeature.MassMonoisotopicAligned;
                    // Check to see that the features are within the mass tolearance of one another.
                    if (MathUtilities.MassDifferenceInPPM(shiftedTagMass, observedMass) <= massTolerancePPM)
                    {
                        // Create a temporary matche between the two and check it against all tolerances before adding to the match list.
                        FeatureMatch<T, U> match = new FeatureMatch<T, U>();
                        match.AddFeatures(observedFeature, shiftedTag, m_matchParameters.UseDriftTime, (shiftAmount > 0));
                        if (match.InRegion(tolerances, useEllipsoid))
                        {
                            matchList.Add(match);
                        }
                    }
                    else
                    {
                        // Increase the lower bound if the the MassTag masses are too low or set the continueLoop flag to false if they are too high.
                        if (shiftedTagMass < observedMass)
                        {
                            lowerBound++;
                        }
                        else
                        {
                            continueLoop = false;
                        }
                    }
                    // Increment the target index.
                    targetIndex++;
                }
                // Increment the observed index.
                observedIndex++;
            }
            // Return the list of matches.
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
        /// <summary>
        /// Find the subset of a list which has the same charge state.
        /// </summary>
        /// <typeparam name="T">Any class derived from Feature.</typeparam>
        /// <param name="featureList">Original list of features from which to extract the subset.</param>
        /// <param name="startIndex">Index at which to start search.  Set to the value of the last such item.</param>
        /// <param name="chargeState">The charge state to search for a subset of.</param>
        /// <returns>A list of features with the given charge state.</returns>
        private List<T> ExtractChargeStateList<T>(List<T> featureList, ref int startIndex, int chargeState) where T:Feature, new()
        {
            featureList.Sort(new Comparison<T>(Feature.ChargeStateComparison));
            int currentIndex = startIndex;
            int start = startIndex;
            int endIndex = featureList.Count;
            while (featureList[currentIndex].ChargeState < chargeState)
            {
                currentIndex++;
            }
            start = currentIndex;
            while (featureList[currentIndex].ChargeState == chargeState)
            {
                currentIndex++;
            }
            endIndex = currentIndex;
            startIndex = currentIndex;
            return featureList.GetRange(startIndex, endIndex - startIndex + 1);
        }
        /// <summary>
        /// Sets the false discovery rate by creating a histogram of the mass errors and comparing the proportion above a threshhold to the area below.
        /// </summary>
        private void SetMassErrorHistogramFDR()
        {
            // Populate the mass error list and create the histogram.
            List<XYData> histogramList = new List<XYData>();
            List<double> massErrorList = new List<double>();
            foreach (FeatureMatch<T, U> match in m_matchList)
            {
                massErrorList.Add(match.DifferenceVector[1, 1]);
            }
            histogramList = MathUtilities.GetHistogramValues(massErrorList, m_matchParameters.HistogramBinWidth);

            int peakIndex = 0;
            double peakValue = 0.0;
            double upperBound = 0.0;
            double lowerBound = 0.0;
            double threshold = 0.0;
            double meanValue = 0.0;
            // Find the maximum and average values.
            for( int i=0; i<histogramList.Count; i++ )
            {
                XYData bin = histogramList[i];
                if (bin.Y > peakValue)
                {
                    peakValue = bin.Y;
                    peakIndex = i;
                }
                meanValue += bin.Y;
            }
            meanValue /= histogramList.Count;
            // Set the threshold to a value between the peak and the average value.
            threshold = meanValue + m_matchParameters.HistogramMultiplier * (peakValue - meanValue);
            // Find the upper bound.
            for (int i = peakIndex; i < histogramList.Count; i++)
            {
                XYData bin = histogramList[i];
                XYData lowerBin = histogramList[i - 1];
                if (bin.Y < threshold && lowerBin.Y >= threshold)
                {
                    upperBound = lowerBin.X + (lowerBin.Y - threshold) / (lowerBin.Y - bin.Y) * (bin.X - lowerBin.X);
                }
            }
            // Find the lower bound.
            for (int i = peakIndex; i >= 0; i--)
            {
                XYData bin = histogramList[i];
                XYData upperBin = histogramList[i + 1];
                if (bin.Y < threshold && upperBin.Y >= threshold)
                {
                    lowerBound = bin.X + (threshold-bin.Y) / (upperBin.Y-bin.Y) * (upperBin.X - bin.X);
                }
            }
            // Count the number of matches within the bounds and calculate the FDR.
            int countInBounds = 0;
            foreach (double massDifference in massErrorList)
            {
                if (massDifference >= lowerBound && massDifference <= upperBound)
                {
                    countInBounds++;
                }
            }
            m_errorHistogramFDR = countInBounds/((upperBound-lowerBound)*threshold);
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
                // If no charge states were specified in the parameters, fill in the charge state list with all charge states contained in the observed feature list.
                if (m_matchParameters.ChargeStateList.Count == 0)
                {
                    SetChargeStateList();
                }
                // Resize the STAC and refined tolerances list so that they are the same length as the charge state list.
                int chargeStateCount = m_matchParameters.ChargeStateList.Count;
                m_stacParametersList.Capacity = chargeStateCount;
                m_refinedTolerancesList.Capacity = chargeStateCount;
                // Sort the feature lists and create indices to store where a charge state ends.
                m_observedFeatureList.Sort(new Comparison<T>(Feature.ChargeStateComparison));
                int chargeStateIndexObserved = 0;
                m_targetFeatureList.Sort(new Comparison<U>(Feature.ChargeStateComparison));
                int chargeStateIndexTarget = 0;
                // Iterate through each charge state performing the desired operations.
                for (int i = 0; i < chargeStateCount; i++)
                {
                    // Create sub-lists of the features with the current charge state.
                    int chargeState = m_matchParameters.ChargeStateList[i];
                    List<T> chargeObservedList = ExtractChargeStateList(m_observedFeatureList, ref chargeStateIndexObserved, chargeState);
                    List<U> chargeTargetList = ExtractChargeStateList(m_targetFeatureList, ref chargeStateIndexTarget, chargeState);
                    // Find the matches among the two feature lists.
                    List<FeatureMatch<T, U>> tempMatchList = new List<FeatureMatch<T, U>>();
                    tempMatchList = FindMatches(chargeObservedList, chargeTargetList, m_matchParameters.UserTolerances, false, 0);
                    bool lengthCheck = (tempMatchList.Count < MIN_MATCHES_FOR_NORMAL_ASSUMPTION);
                    // If the STAC score is requested and there are sufficient matches for the assumptions, perform it.
                    if (m_matchParameters.ShouldCalculateSTAC && lengthCheck)
                    {
                        STACParameterList[i].PerformSTAC(tempMatchList, m_matchParameters.UserTolerances, m_matchParameters.UseDriftTime, m_matchParameters.UsePriors);
                    }
                    // If 11 Dalton shift FDR is requested, calculate whether each of the temporary matches is within the bounds.
                    if (m_matchParameters.ShouldCalculateShiftFDR)
                    {
                        m_refinedTolerancesList[i] = FindOptimalTolerances(tempMatchList);
                        for (int j = 0; j < tempMatchList.Count; j++)
                        {
                            tempMatchList[j].InRegion(m_refinedTolerancesList[i], m_matchParameters.UseEllipsoid);
                        }
                        m_shiftedMatchList.AddRange(FindMatches(chargeObservedList, chargeTargetList, m_refinedTolerancesList[i],
                                                                    m_matchParameters.UseEllipsoid, m_matchParameters.ShiftAmount));
                    }
                    // Add the temporary matches to the match list.
                    m_matchList.AddRange(tempMatchList);
                }
                PopulateSTACFDRTable(); // Need to calculate STAC FDR in a smarter way than this.
                int count = 0;
                for (int j = 0; j < m_matchList.Count; j++)
                {
                    if (m_matchList[j].WithinRefinedRegion)
                    {
                        count++;
                    }
                }
                m_shiftFDR = (1.0 * count) / m_shiftedMatchList.Count;
                m_shiftConservativeFDR = (2.0 * count) / m_shiftedMatchList.Count;
            }
            else
            {
                m_matchList.Clear();
                m_matchList = FindMatches(m_observedFeatureList, m_targetFeatureList, m_matchParameters.UserTolerances, false, 0);
                bool lengthCheck = (m_matchList.Count >= MIN_MATCHES_FOR_NORMAL_ASSUMPTION);
                if (m_matchParameters.ShouldCalculateSTAC && lengthCheck)
                {
                    STACParameterList[0].PerformSTAC(m_matchList, m_matchParameters.UserTolerances, m_matchParameters.UseDriftTime, m_matchParameters.UsePriors);
                    PopulateSTACFDRTable();
                }
                if (m_matchParameters.ShouldCalculateHistogramFDR)
                {
                    SetMassErrorHistogramFDR();
                }
                if (m_matchParameters.ShouldCalculateShiftFDR)
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
                if (m_matchParameters.ShouldCalculateSLiC && lengthCheck)
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
