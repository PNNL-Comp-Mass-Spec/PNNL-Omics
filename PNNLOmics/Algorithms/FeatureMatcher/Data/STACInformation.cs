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
        # region Member variables
        private bool m_usePriorProbabilities;

        public bool UsePriorProbabilities
        {
            get { return m_usePriorProbabilities; }
            set { m_usePriorProbabilities = value; }
        }

        private uint m_iterations;

        public uint Iterations
        {
            get { return m_iterations; }
            set { m_iterations = value; }
        }

        private double m_mixtureProportion;

        public double MixtureProportion
        {
            get { return m_mixtureProportion; }
            set { m_mixtureProportion = value; }
        }
        private double m_logLikelihood;

        public double LogLikelihood
        {
            get { return m_logLikelihood; }
            set { m_logLikelihood = value; }
        }

        private Matrix m_meanVectorT;

        public Matrix MeanVectorT
        {
            get { return m_meanVectorT; }
            set { m_meanVectorT = value; }
        }
        private Matrix m_covarianceMatrixT;

        public Matrix CovarianceMatrixT
        {
            get { return m_covarianceMatrixT; }
            set { m_covarianceMatrixT = value; }
        }
        private Matrix m_meanVectorF;

        public Matrix MeanVectorF
        {
            get { return m_meanVectorF; }
            set { m_meanVectorF = value; }
        }
        private Matrix m_covarianceMatrixF;

        public Matrix CovarianceMatrixF
        {
            get { return m_covarianceMatrixF; }
            set { m_covarianceMatrixF = value; }
        }
        # endregion

        #region Initializers
        /// <summary>
        /// Initialize class with option to choose to use drift times or not.
        /// </summary>
        /// <param name="driftTime">true/false:  Whether or not to use drift times in calculations.</param>
        public STACInformation(bool driftTime)
        {
            SetDefaults(driftTime);
        }
        /// <summary>
        /// Initialize class without drift times (default).
        /// </summary>
        public STACInformation()
        {
            SetDefaults(false);
        }
        #endregion

        #region Variable routines
        /// <summary>
        /// Set STAC parameters to defaults.
        /// </summary>
        /// <param name="driftTime">true/false:  Whether or not to use drift times in calculations.</param>
        public void SetDefaults(bool driftTime)
        {
            m_usePriorProbabilities = true;
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

        public bool RunSTAC(List<Matrix> differenceMatrixList, Tolerances uniformTolerances, List<bool> driftTime, List<bool> driftTimePredicted, 
                                    bool usePriorProbabilities)
        {
            ExpectationMaximization.NormalUniformMixture(differenceMatrixList, ref m_meanVectorT, ref m_covarianceMatrixT, uniformTolerances.AsVector(true),
                                                            ref m_mixtureProportion, ref m_logLikelihood, false);
            return false;
        }

        public bool RunSTAC(List<Matrix> differenceMatrixList, Tolerances uniformTolerances, bool usePriorProbabilities)
        {
            ExpectationMaximization.NormalUniformMixture(differenceMatrixList, ref m_meanVectorT, ref m_covarianceMatrixT, uniformTolerances.AsVector(false),
                                                            ref m_mixtureProportion, ref m_logLikelihood, false);

            return false;
        }
        #endregion
    }
}
