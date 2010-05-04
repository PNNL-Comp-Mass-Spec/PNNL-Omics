using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureMatcher.Utilities;

namespace PNNLOmics.Algorithms.FeatureMatcher.Data
{
	public class FeatureMatcherParameters
    {
        #region Member variables
        private STACInformation m_smartParameters;
        private Tolerances m_userTolerances;
        private Tolerances m_refinedTolerances;
        private SLiCInformation m_slicParameters;
        #endregion

        #region Properties
        public STACInformation SMARTParameters
        {
            get { return m_smartParameters; }
            set { m_smartParameters = value; }
        }
        public Tolerances UserTolerances
        {
            get { return m_userTolerances; }
            set { m_userTolerances = value; }
        }
        public Tolerances RefinedTolerances
        {
            get { return m_refinedTolerances; }
            set { m_refinedTolerances = value; }
        }
        public SLiCInformation SLiCParameters
        {
            get { return m_slicParameters; }
            set { m_slicParameters = value; }
        }
        #endregion

        #region Constructors
        public FeatureMatcherParameters()
		{
			Clear();
		}

        public FeatureMatcherParameters(double massTolerance, double netTolerance, float driftTimeTolerance)
        {
            Clear();
            m_userTolerances.MassTolerancePPM = massTolerance;
            m_userTolerances.NETTolerance = netTolerance;
            m_userTolerances.DriftTimeTolerance = driftTimeTolerance;
        }
        #endregion

        #region Private functions
        private void Clear()
		{
            m_slicParameters = new SLiCInformation();
            m_smartParameters = new STACInformation();
            m_refinedTolerances = new Tolerances();
            m_userTolerances = new Tolerances();
        }
        #endregion

        #region Train parameters
        public void FindOptimalTolerances<T,U>(List<FeatureMatch<T,U>> featureMatchList) where T: Feature, new() where U: Feature, new()
        {
            List<Matrix> differenceMatrixList = new List<Matrix>();

            for (int i = 0; i <= featureMatchList.Count; i++)
            {
                differenceMatrixList.Add(featureMatchList[i].ReducedDifferenceVector);
            }

            int rows = differenceMatrixList[0].RowCount;
            bool useDriftTime = (rows==3);

            Matrix meanVector = new Matrix(rows,1,0.0);
            Matrix covarianceMatrix = new Matrix(rows,1.0);
            double mixtureParameter = 0.5; 
            double logLikelihood = 0;

            ExpectationMaximization.NormalUniformMixture(differenceMatrixList, ref meanVector, ref covarianceMatrix, UserTolerances.AsVector(useDriftTime), ref mixtureParameter, ref logLikelihood, true);
            
            SLiCParameters.MassPPMStDev = Math.Sqrt(covarianceMatrix[0,0]);
            SLiCParameters.NETStDev = Math.Sqrt(covarianceMatrix[1,1]);
            if (useDriftTime)
                SLiCParameters.DriftTimeStDev = (float)Math.Sqrt(covarianceMatrix[2, 2]);

            RefinedTolerances = new Tolerances((2.5 * SLiCParameters.MassPPMStDev), (2.5 * SLiCParameters.NETStDev), (float)(2.5 * SLiCParameters.DriftTimeStDev));
            RefinedTolerances.Refined = true;
        }
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

        }
        #endregion
    }
}
