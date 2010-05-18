using System;
using System.Collections.Generic;
using System.Text;
using PNNLOmics.Data;
using PNNLOmics.Algorithms.FeatureMatcher.Utilities;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureMatcher.Data
{
    public class STACInformation
    {
        # region Members
        private uint m_iterations;

        private double m_mixtureProportion;
        private double m_logLikelihood;

        private Matrix m_meanVectorT;
        private Matrix m_covarianceMatrixT;
        private Matrix m_meanVectorF;
        private Matrix m_covarianceMatrixF;
        #endregion

        #region Properties
        public uint Iterations
        {
            get { return m_iterations; }
            set { m_iterations = value; }
        }

        public double MixtureProportion
        {
            get { return m_mixtureProportion; }
            set { m_mixtureProportion = value; }
        }
        public double LogLikelihood
        {
            get { return m_logLikelihood; }
            set { m_logLikelihood = value; }
        }

        public Matrix MeanVectorT
        {
            get { return m_meanVectorT; }
            set { m_meanVectorT = value; }
        }
        public Matrix CovarianceMatrixT
        {
            get { return m_covarianceMatrixT; }
            set { m_covarianceMatrixT = value; }
        }
        public Matrix MeanVectorF
        {
            get { return m_meanVectorF; }
            set { m_meanVectorF = value; }
        }
        public Matrix CovarianceMatrixF
        {
            get { return m_covarianceMatrixF; }
            set { m_covarianceMatrixF = value; }
        }
        # endregion

        #region Constructors
        public STACInformation(bool driftTime)
        {
            Clear(driftTime);
        }
        #endregion

        #region Variable routines  ---Needs work
        public void Clear(bool driftTime)
        {
            m_iterations = 0;
            m_mixtureProportion = 0.5;
            m_logLikelihood = 0.0;
            if (driftTime)
            {
                m_meanVectorT = new Matrix(3, 1, 0.0);
                m_covarianceMatrixT = new Matrix(3, 3, 0.0);
                m_covarianceMatrixT[0, 0] = 2.0;
                m_covarianceMatrixT[1, 1] = 0.3;
                m_covarianceMatrixT[2, 2] = 1.0;
                m_meanVectorF = new Matrix(3, 1, 0.0);
                m_covarianceMatrixF = new Matrix(3, 3, 0.0);
                m_covarianceMatrixF[0, 0] = 2.0;
                m_covarianceMatrixF[1, 1] = 0.1;
                m_covarianceMatrixF[2, 2] = 1.0;
            }
            else
            {
                m_meanVectorT = new Matrix(2, 1, 0.0);
                m_covarianceMatrixT = new Matrix(2, 2, 0.0);
                m_covarianceMatrixT[1, 1] = 2.0;
                m_covarianceMatrixT[2, 2] = 0.3;
                m_meanVectorF = new Matrix(2, 1, 0.0);
                m_covarianceMatrixF = new Matrix(2, 2, 0.0);
                m_covarianceMatrixF[1, 1] = 2.0;
                m_covarianceMatrixF[2, 2] = 0.1;
            }
        }

        // Redesign to take in matches.
        public bool RunSTAC(List<Matrix> differenceMatrixList, Tolerances uniformTolerances, List<bool> driftTime, List<bool> driftTimePredicted, 
                                    bool usePriorProbabilities)
        {
            ExpectationMaximization.NormalUniformMixture(differenceMatrixList, ref m_meanVectorT, ref m_covarianceMatrixT, uniformTolerances.AsVector(true),
                                                            ref m_mixtureProportion, ref m_logLikelihood, false);
            return false;
        }
        #endregion
    }
}
