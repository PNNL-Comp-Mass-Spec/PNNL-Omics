using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureMatcher.Data;

namespace PNNLOmics.Algorithms.FeatureMatcher
{
    public class FeatureMatcher<T, U> where T: Feature, new() where U: Feature, new()
    {
        #region Members
        private FeatureMatcherParameters m_matchParameters;

        private List<FeatureMatch<T,U>> m_matchList;
        private List<FeatureMatch<T,U>> m_shiftedMatchList;
        private List<T> m_observedFeatureList;
        private List<U> m_targetFeatureList;

        private double m_stacFDR;
        private double m_shiftFDR;
        private double m_shiftConservativeFDR;
        private double m_errorHistogramFDR;

        private List<STACInformation> m_stacParametersList;
        private List<FeatureMatcherTolerances> m_refinedTolerancesList;
        private SLiCInformation m_slicParameters;

        const int MIN_MATCHES_FOR_NORMAL_ASSUMPTION = 50;
        #endregion

        #region Properties
        public FeatureMatcherParameters MatchParameters
        {
            get { return m_matchParameters; }
            set { m_matchParameters = value; }
        }
        public List<FeatureMatch<T, U>> MatchList
        {
            get { return m_matchList; }
        }
        public List<FeatureMatch<T, U>> ShiftedMatchList
        {
            get { return m_shiftedMatchList; }
        }

        public double STACFDR
        {
            get { return m_stacFDR; }
        }
        public double ShiftFDR
        {
            get { return m_shiftFDR; }
        }
        public double ShiftConservativeFDR
        {
            get { return m_shiftConservativeFDR; }
        }
        public double ErrorHistogramFDR
        {
            get { return m_errorHistogramFDR; }
        }

        public List<STACInformation> STACParameterList
        {
            get { return m_stacParametersList; }
        }
        public List<FeatureMatcherTolerances> RefinedToleranceList
        {
            get { return m_refinedTolerancesList; }
        }
        public SLiCInformation SLiCParameters
        {
            get { return m_slicParameters; }
        }
        #endregion

        #region Constructors
        public FeatureMatcher(List<T> observedFeatureList, List<U> targetFeatureList, FeatureMatcherParameters matchParameters)
        {
            Clear();
            m_observedFeatureList = observedFeatureList;
            m_targetFeatureList = targetFeatureList;
            m_matchParameters = matchParameters;
        }
        #endregion

        #region Private functions
        private void Clear()
		{
            m_matchParameters = new FeatureMatcherParameters();
            m_stacFDR = 0;
            m_shiftFDR = 0;
            m_shiftConservativeFDR = 0;
            m_errorHistogramFDR = 0;
            m_stacParametersList = new List<STACInformation>();
            m_refinedTolerancesList = new List<FeatureMatcherTolerances>();
            m_slicParameters = new SLiCInformation();
            m_matchList = new List<FeatureMatch<T, U>>();
            m_shiftedMatchList = new List<FeatureMatch<T, U>>();
        }

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
                    match.AddFeatures(feature, shiftedTag, m_matchParameters.UseDriftTime,(shiftAmount>0));
                    if (match.InRegion(tolerances, useEllipsoid))
                    {
                        matchList.Add(match);
                    }
                }
            }
            return matchList;
        }

        private FeatureMatcherTolerances FindOptimalTolerances<T,U>(List<FeatureMatch<T,U>> matchList)
        {
            List<Matrix> differenceMatrixList = new List<Matrix>();
            for (int i = 0; i <= matchList.Count; i++)
            {
                differenceMatrixList.Add(matchList[i].ReducedDifferenceVector);
            }

            int rows = differenceMatrixList[0].RowCount;
            Matrix meanVector = new Matrix(rows,1,0.0);
            Matrix covarianceMatrix = new Matrix(rows,1.0);
            double mixtureParameter = 0.5; 
            double logLikelihood = 0;

            Utilities.ExpectationMaximization.NormalUniformMixture(differenceMatrixList, ref meanVector, ref covarianceMatrix,
                                                                    m_matchParameters.UserTolerances.AsVector(m_matchParameters.UseDriftTime),
                                                                    ref mixtureParameter, ref logLikelihood, true);
            
            m_slicParameters.MassPPMStDev = Math.Sqrt(covarianceMatrix[0,0]);
            m_slicParameters.NETStDev = Math.Sqrt(covarianceMatrix[1, 1]);
            if (m_matchParameters.UseDriftTime)
                m_slicParameters.DriftTimeStDev = (float)Math.Sqrt(covarianceMatrix[2, 2]);

            FeatureMatcherTolerances refinedTolerances = new FeatureMatcherTolerances((2.5 * m_slicParameters.MassPPMStDev), (2.5 * m_slicParameters.NETStDev), 
                                                            (float)(2.5 * m_slicParameters.DriftTimeStDev));
            refinedTolerances.Refined = true;
            return refinedTolerances;
        }
        /*
        public void FindSTACParameters<T,U>(List<FeatureMatch<T,U>> featureMatchList, bool usePriorProbabilities) where T: Feature, new() where U: Feature, new()
        {
            List<Matrix> differenceMatrixList = new List<Matrix>();
            List<bool> useDriftTime = new List<bool>();
            List<bool> useDriftTimePredicted = new List<bool>();

            for (int i = 0; i <= featureMatchList.Count; i++)
            {
                differenceMatrixList.Add(featureMatchList[i].DifferenceVector);
                useDriftTime.Add(featureMatchList[i].UseDriftTime);
                useDriftTimePredicted.Add(featureMatchList[i].UseDriftTimePredicted);
            }

            int rows = differenceMatrixList[0].RowCount;

            m_smartParameters.RunSTAC(differenceMatrixList, m_userTolerances, useDriftTime, useDriftTimePredicted, usePriorProbabilities);
        }
        public void FindSTACParameters<T, U>(List<FeatureMatch<T, U>> featureMatchList, bool useDriftTime, bool usePriorProbabilities) where T : Feature, new() where U : Feature, new()
        {

        }*/
        #endregion

        #region Public functions
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
                for (int i = 0; i < chargeStateCount; i++ )
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
                        // Calculate STAC parameters, values, and specificity for each potential match.
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
                // Calculate STAC FDR in a smarter way than is currently done.
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
                    // Calculate STAC parameters, values, specificity, and FDR table for each potential match.
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
