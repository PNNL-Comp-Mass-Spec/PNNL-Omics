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
        /// <summary>
        /// Gets the number of iterations needed to reach convergence.
        /// </summary>
        public uint Iterations
        {
            get { return m_iterations; }
            set { m_iterations = value; }
        }

        /// <summary>
        /// Gets or sets the mixture proportion, i.e. the probability of being from the correct distribution.
        /// </summary>
        public double MixtureProportion
        {
            get { return m_mixtureProportion; }
            set { m_mixtureProportion = value; }
        }
        /// <summary>
        /// Gets or sets the value of the loglikelihood function evaluated at the current parameters.
        /// </summary>
        public double LogLikelihood
        {
            get { return m_logLikelihood; }
            set { m_logLikelihood = value; }
        }

        /// <summary>
        /// Gets or sets the estimated means of the true normal distribution.
        /// </summary>
        public Matrix MeanVectorT
        {
            get { return m_meanVectorT; }
            set { m_meanVectorT = value; }
        }
        /// <summary>
        /// Gets or sets the estimated covariance matrix of the true normal distribution.
        /// </summary>
        public Matrix CovarianceMatrixT
        {
            get { return m_covarianceMatrixT; }
            set { m_covarianceMatrixT = value; }
        }
        /// <summary>
        /// Gets or sets the mean vector of the normal distribution used in the case of a low prior probability.
        /// </summary>
        public Matrix MeanVectorF
        {
            get { return m_meanVectorF; }
            set { m_meanVectorF = value; }
        }
        /// <summary>
        /// Gets or sets the covariance matrix of the normal distribution used in the case of a low prior probability.
        /// </summary>
        public Matrix CovarianceMatrixF
        {
            get { return m_covarianceMatrixF; }
            set { m_covarianceMatrixF = value; }
        }
        # endregion

        #region Constructors
        /// <summary>
        /// Default constructor for STAC parameters.
        /// </summary>
        /// <param name="driftTime">Whether drift times will be used in the analysis.</param>
        public STACInformation(bool driftTime)
        {
            Clear(driftTime);
        }
        #endregion

        #region Variable routines  ---Needs work
        /// <summary>
        /// Resets parameters to default values.
        /// </summary>
        /// <param name="driftTime">Whether to use drift times in the analysis.</param>
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
        public bool RunSTAC(List<Matrix> differenceMatrixList, FeatureMatcherTolerances uniformTolerances, List<bool> driftTime, List<bool> driftTimePredicted, 
                                    bool usePriorProbabilities)
        {
            ExpectationMaximization.NormalUniformMixture(differenceMatrixList, ref m_meanVectorT, ref m_covarianceMatrixT, uniformTolerances.AsVector(true),
                                                            ref m_mixtureProportion, ref m_logLikelihood, false);
            return false;
        }
        #endregion
    }
}
