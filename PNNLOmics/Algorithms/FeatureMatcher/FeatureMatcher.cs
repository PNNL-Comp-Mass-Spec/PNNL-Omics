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
        #region Member variables
        private FeatureMatcherParameters m_matchParameters;

        private List<FeatureMatch<T,U>> m_matchList;
        private List<FeatureMatch<T,U>> m_shiftedMatchList;
        private List<T> m_observedFeatureList;
        private List<U> m_targetFeatureList;

        private double m_stacFDR;
        private double m_shiftBy11DaltonFDR;
        private double m_shiftBy11DaltonConservativeFDR;
        private double m_errorHistogramFDR;

        private bool m_useDriftTimes;
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
            get { return Math.Round(m_stacFDR,4); }
        }
        public double ShiftBy11DaltonFDR
        {
            get { return Math.Round(m_shiftBy11DaltonFDR,4); }
        }
        public double ShiftBy11DaltonConservativeFDR
        {
            get { return Math.Round(m_shiftBy11DaltonConservativeFDR,4); }
        }
        public double ErrorHistogramFDR
        {
            get { return Math.Round(m_errorHistogramFDR,4); }
        }

        public bool UseDriftTimes
        {
            get { return m_useDriftTimes; }
        }
        #endregion

        #region Constructors
        public FeatureMatcher(List<T> observedFeatureList, List<U> targetFeatureList)
		{
			Clear();
            m_observedFeatureList = observedFeatureList;
            m_targetFeatureList = targetFeatureList;
		}
        public FeatureMatcher(List<T> observedFeatureList, List<U> targetFeatureList, FeatureMatcherParameters matchParameters)
        {
            Clear();
            m_observedFeatureList = observedFeatureList;
            m_targetFeatureList = targetFeatureList;
            m_matchParameters = matchParameters;
        }
        public FeatureMatcher(List<T> observedFeatureList, List<U> targetFeatureList, FeatureMatcherParameters matchParameters, bool useDriftTimes)
        {
            Clear();
            m_observedFeatureList = observedFeatureList;
            m_targetFeatureList = targetFeatureList;
            m_matchParameters = matchParameters;
            m_useDriftTimes = useDriftTimes;
        }
        public FeatureMatcher(List<T> observedFeatureList, List<U> targetFeatureList, bool useDriftTimes)
        {
            Clear();
            m_observedFeatureList = observedFeatureList;
            m_targetFeatureList = targetFeatureList;
            m_useDriftTimes = useDriftTimes;
        }
        public FeatureMatcher(List<T> observedFeatureList, List<U> targetFeatureLIst, double massTolerancePPM, double netTolerance, float driftTimeTolerance)
        {
            Clear();
            m_observedFeatureList = observedFeatureList;
            m_targetFeatureList = targetFeatureLIst;
            m_matchParameters = new FeatureMatcherParameters(massTolerancePPM, netTolerance, driftTimeTolerance);
            m_useDriftTimes = true;
        }
        public FeatureMatcher(List<T> observedFeatureList, List<U> targetFeatureLIst, double massTolerancePPM, double netTolerance)
        {
            Clear();
            m_observedFeatureList = observedFeatureList;
            m_targetFeatureList = targetFeatureLIst;
            m_matchParameters = new FeatureMatcherParameters(massTolerancePPM, netTolerance, 100);
        }
        #endregion

        #region Private functions
        private void Clear()
		{
            m_matchParameters = new FeatureMatcherParameters();
            m_stacFDR = 0;
            m_shiftBy11DaltonFDR = 0;
            m_errorHistogramFDR = 0;
            m_useDriftTimes = false;
        }
        #endregion

        #region Fill match list -- FIX THESE!!!
        /// <summary>
        /// Function to match observedFeatures to targetFeatures within a rectangular (rectangular prism in 3-D) window.
        /// </summary>
        /// <param name="useOptimized">true/false:  Use the refined tolerances as optimized by the EM algorithm (if previously set).</param>
        private void MatchToRectangle(bool useOptimized)
        {
            m_matchList.Clear();
            m_shiftedMatchList.Clear();

            Matrix tolerancesToUse;
            if (useOptimized && m_matchParameters.RefinedTolerances.Refined)
            {
                tolerancesToUse = m_matchParameters.RefinedTolerances.AsVector(true);
            }
            else
            {
                tolerancesToUse = m_matchParameters.UserTolerances.AsVector(true);
            }

            foreach (T feature in m_observedFeatureList)
            {
                foreach (U massTag in m_targetFeatureList)
                {
                    FeatureMatch<T,U> match = new FeatureMatch<T,U>();
                    if (Math.Abs(match.DifferenceVector[1, 0]) < tolerancesToUse[1, 0] && Math.Abs(match.DifferenceVector[2, 0]) < tolerancesToUse[2, 0])
                    {
                        if (Math.Abs(match.DifferenceVector[0, 0]) < tolerancesToUse[0, 0])
                        {
                            m_matchList.Add(match);
                        }
                        else if (Math.Abs(match.DifferenceVector[0, 0] + 11.0) < tolerancesToUse[0, 0])
                        {
                            m_shiftedMatchList.Add(match);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Function to match Features to MassTags within an elliptical (ellipsoid in 3-D) window.
        /// </summary>
        /// <param name="useOptimized">true/false:  Use the refined tolerances as optimized by the EM algorithm (if previously set).</param>
        private void MatchToEllipsoid(bool useOptimized)
        {
            m_matchList.Clear();
            m_shiftedMatchList.Clear();

            Matrix tolerancesToUse;
            if (useOptimized)
            {
                tolerancesToUse = m_matchParameters.RefinedTolerances.AsVector(true);
            }
            else
            {
                tolerancesToUse = m_matchParameters.UserTolerances.AsVector(true);
            }

            foreach (T feature in m_observedFeatureList)
            {
                foreach (U massTag in m_targetFeatureList)
                {
                    FeatureMatch<T,U> match = new FeatureMatch<T,U>();
                    if (match.DifferenceVector[1, 0] <= tolerancesToUse[1, 0])
                    {
                        double distance = Math.Pow(match.DifferenceVector[1, 0] / tolerancesToUse[1, 0], 2) + Math.Pow(match.DifferenceVector[2, 0] / tolerancesToUse[2, 0], 2);
                        double shiftedDistance = Math.Sqrt(Math.Pow((match.DifferenceVector[0, 0] + 11) / tolerancesToUse[0, 0], 2) + distance);
                        distance = Math.Sqrt(Math.Pow((match.DifferenceVector[0, 0]) / tolerancesToUse[0, 0], 2) + distance);
                        if (distance <= 1.0)
                        {
                            m_matchList.Add(match);
                        }
                        else if (shiftedDistance <= 1.0)
                        {
                            m_shiftedMatchList.Add(match);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
