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
        private double m_mixtureProportion;
        private double m_logLikelihood;

        private Matrix m_meanVectorT;
        private Matrix m_covarianceMatrixT;
        private Matrix m_meanVectorF;
        private Matrix m_covarianceMatrixF;

        private uint m_iteration;

        private const double EPSILON = 0.0001;
        private const int MAX_ITERATIONS = 500;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the mixture proportion, i.e. the probability of being from the correct distribution.
        /// </summary>
        public double MixtureProportion
        {
            get { return m_mixtureProportion; }
            set { m_mixtureProportion = value; }
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

        #region Public functions
        /// <summary>
        /// Resets parameters to default values.
        /// </summary>
        /// <param name="driftTime">Whether to use drift times in the analysis.</param>
        public void Clear(bool driftTime)
        {
            m_iteration = 0;
            m_mixtureProportion = 0.5;
            m_logLikelihood = 0.0;
            if (driftTime)
            {
                m_meanVectorT = new Matrix(4, 1, 0.0);
                m_covarianceMatrixT = new Matrix(4, 4, 0.0);
                m_covarianceMatrixT[0, 0] = 2.0;
                m_covarianceMatrixT[1, 1] = 0.3;
                m_covarianceMatrixT[2, 2] = 0.5;
                m_covarianceMatrixT[3, 3] = 1.0;
                m_meanVectorF = m_meanVectorT;
                m_covarianceMatrixF = m_covarianceMatrixT;
            }
            else
            {
                m_meanVectorT = new Matrix(2, 1, 0.0);
                m_covarianceMatrixT = new Matrix(2, 2, 0.0);
                m_covarianceMatrixT[1, 1] = 2.0;
                m_covarianceMatrixT[2, 2] = 0.3;
                m_meanVectorF = m_meanVectorT;
                m_covarianceMatrixF = m_covarianceMatrixT;
            }
        }
        /// <summary>
        /// Trains the STAC parameters using the passed data.
        /// </summary>
        /// <typeparam name="T">Observed feature to be matched.  Derived from Feature.  Usually UMC or UMCCluster.</typeparam>
        /// <typeparam name="U">Feature to be matched to.  Derived from Feature.  Usually AMTTag.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to train parameters on.</param>
        /// <param name="uniformTolerances">User provided tolerances.</param>
        /// <param name="useDriftTime">Whether to train the data on the drift time dimension.</param>
        /// <returns>A boolean flag indicating whether convergence was reached.</returns>
        public bool TrainSTAC<T, U> (List<FeatureMatch<T,U>> featureMatchList, FeatureMatcherTolerances uniformTolerances, bool useDriftTime, bool usePrior) where T: Feature, new() where U: Feature, new()
        {
            Clear();

            // Calculate the density of the uniform density given the tolerances.
            Matrix toleranceMatrix = uniformTolerances.AsVector(useDriftTime);
            double uniformDensity = 1.0;
            for (int i = 0; i < toleranceMatrix.RowCount; i++)
            {
                uniformDensity /= (2 * toleranceMatrix[i, 0]);
            }
            // Train the EM parameters on the data.
            if (usePrior)
            {
                return TrainWithPrior(featureMatchList, uniformDensity, useDriftTime);
            }
            else
            {
                return TrainWithoutPrior(featureMatchList, uniformDensity, useDriftTime);
            }
        }
        /// <summary>
        /// Function to calculate STAC score.
        /// </summary>
        /// <typeparam name="T">Observed feature to be matched.  Derived from Feature.  Usually UMC or UMCCluster.</typeparam>
        /// <typeparam name="U">Feature to be matched to.  Derived from Feature.  Usually AMTTag.</typeparam>
        /// <param name="featureMatchList">FeatureMatch to compute score for.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <param name="useDriftTime">Whether to use drift times in the calculations.</param>
        /// <param name="usePrior">Whether to use prior probabilities in the calculation of the STAC score.</param>
        /// <returns>The STAC score corresponding to featureMatch.</returns>
        public double ComputeSTAC<T, U>(FeatureMatch<T, U> featureMatch, FeatureMatcherTolerances uniformTolerances, bool useDriftTime, bool usePrior) where T : Feature, new() where U : Feature, new()
        {
            // Calculate the density of the uniform density given the tolerances.
            Matrix toleranceMatrix = uniformTolerances.AsVector(useDriftTime);
            double uniformDensity = 1.0;
            for (int i = 0; i < toleranceMatrix.RowCount; i++)
            {
                uniformDensity /= (2 * toleranceMatrix[i, 0]);
            }
            return ComputeSTAC(featureMatch, uniformDensity, useDriftTime, usePrior);
        }
        /// <summary>
        /// Function to calculate STAC score.
        /// </summary>
        /// <typeparam name="T">Observed feature to be matched.  Derived from Feature.  Usually UMC or UMCCluster.</typeparam>
        /// <typeparam name="U">Feature to be matched to.  Derived from Feature.  Usually AMTTag.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to calculate scores for.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <param name="useDriftTime">Whether to use drift times in the calculations.</param>
        /// <param name="usePrior">Whether to use prior probabilities in the calculation of the STAC score.</param>
        /// <returns>The STAC score corresponding to featureMatch.</returns>
        public void SetSTACScores<T, U>(List<FeatureMatch<T, U>> featureMatchList, FeatureMatcherTolerances uniformTolerances, bool useDriftTime, bool usePrior) where T : Feature, new() where U : Feature, new()
        {
            // Calculate the density of the uniform density given the tolerances.
            Matrix toleranceMatrix = uniformTolerances.AsVector(useDriftTime);
            double uniformDensity = 1.0;
            for (int i = 0; i < toleranceMatrix.RowCount; i++)
            {
                uniformDensity /= (2 * toleranceMatrix[i, 0]);
            }
            foreach (FeatureMatch<T, U> match in featureMatchList)
            {
                match.STACScore = ComputeSTAC(match, uniformDensity, useDriftTime, usePrior);
            }
        }
        #endregion

        #region Private functions
        /// <summary>
        /// Overload Clear function to reset number of iterations.
        /// </summary>
        private void Clear()
        {
            m_logLikelihood = 0.0;
            m_iteration = 0;
        }
        /// <summary>
        /// Function to calculate STAC parameters using prior probabilities.
        /// </summary>
        /// <typeparam name="T">The type of the observed feature.  Derived from Feature.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to train data on.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <param name="useDriftTime">Whether to use drift times in the calculations.</param>
        /// <returns>A boolean flag indicating the convergence state of the algorithm.</returns>
        private bool TrainWithPrior<T> (List<FeatureMatch<T, MassTag>> featureMatchList, double uniformDensity, bool useDriftTime) where T : Feature, new()
        {
            Clear();
            bool converges = false;

            List<Matrix> dataList = new List<Matrix>();
            List<double> priorList = new List<double>();
            foreach (FeatureMatch<T, MassTag> match in featureMatchList)
            {
                dataList.Add(match.DifferenceVector);
                priorList.Add(match.TargetFeature.PriorProbability);
            }

            m_logLikelihood = CalculateNNULogLikelihood(featureMatchList, uniformDensity, useDriftTime);

            double nextLogLikelihood = 0.0;
            List<double> alphaList = new List<double>(dataList.Count);

            // Step through the EM algorithm up to m_maxIterations time.
            while (m_iteration <= MAX_ITERATIONS)
            {
                // Update the parameters in the following order: mixture parameters, mean, covariance.
                m_mixtureProportion = UpdateNNUMixtureParameter(featureMatchList, uniformDensity, ref alphaList);
                m_meanVectorT = ExpectationMaximization.UpdateNormalMeanVector(dataList, alphaList, priorList, false);
                m_meanVectorF = ExpectationMaximization.UpdateNormalMeanVector(dataList, alphaList, priorList, true);
                m_covarianceMatrixT = ExpectationMaximization.UpdateNormalCovarianceMatrix(dataList, m_meanVectorT, alphaList, priorList, false, false);
                m_covarianceMatrixF = ExpectationMaximization.UpdateNormalCovarianceMatrix(dataList, m_meanVectorF, alphaList, priorList, false, true);
                // Calculate the loglikelihood based on the new parameters.
                nextLogLikelihood = CalculateNNULogLikelihood(featureMatchList, uniformDensity, useDriftTime);
                // Increment the counter to show that another iteration has been completed.
                m_iteration++;
                // Set the convergence flag and exit the while loop if the convergence criteria is met.
                if (Math.Abs(nextLogLikelihood - m_logLikelihood) < EPSILON)
                {
                    converges = true;
                    break;
                }
                // Update the loglikelihood.
                m_logLikelihood = nextLogLikelihood;
            }
            // Return the convergence flag, which is still false unless changed to true above.
            return converges;
        }
        /// <summary>
        /// Overload function to catch data that does not contain a prior probabilities.
        /// </summary>
        /// <typeparam name="T">The type of the observed feature.  Derived from Feature.</typeparam>
        /// <typeparam name="U">The type of the target feature.  Derived from Feature.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to train data on.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <param name="useDriftTime">Whether to use drift times in the calculations.</param>
        /// <returns>A boolean flag indicating the convergence state of the algorithm.</returns>
        private bool TrainWithPrior<T, U> (List<FeatureMatch<T, U>> featureMatchList, double uniformDensity, bool useDriftTime) where T : Feature, new() where U : Feature, new()
        {
            return TrainWithoutPrior(featureMatchList, uniformDensity, useDriftTime);
        }
        /// <summary>
        /// Train the STAC parameters without using prior probabilities.
        /// </summary>
        /// <typeparam name="T">The type of the observed feature.  Derived from Feature.</typeparam>
        /// <typeparam name="U">The type of the target feature.  Derived from Feature.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to train data on.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <param name="useDriftTime">Whether to use drift times in the calculations.</param>
        /// <returns>A boolean flag indicating the convergence state of the algorithm.</returns>
        private bool TrainWithoutPrior<T, U> (List<FeatureMatch<T, U>> featureMatchList, double uniformDensity, bool useDriftTime) where T : Feature, new() where U : Feature, new()
        {
            Clear();
            bool converges = false;

            List<Matrix> dataList = new List<Matrix>();
            foreach (FeatureMatch<T, U> match in featureMatchList)
            {
                dataList.Add(match.DifferenceVector);
            }

            m_logLikelihood = CalculateNULogLikelihood(featureMatchList, uniformDensity, useDriftTime);

            double nextLogLikelihood = 0.0;
            List<double> alphaList = new List<double>(dataList.Count);
            
            // Step through the EM algorithm up to m_maxIterations time.
            while (m_iteration <= MAX_ITERATIONS)
            {
                // Update the parameters in the following order: mixture parameters, mean, covariance.
                m_mixtureProportion = UpdateNUMixtureParameter(featureMatchList, uniformDensity, ref alphaList);
                m_meanVectorT = ExpectationMaximization.UpdateNormalMeanVector(dataList, alphaList);
                m_covarianceMatrixT = ExpectationMaximization.UpdateNormalCovarianceMatrix(dataList, m_meanVectorT, alphaList, false);
                // Calculate the loglikelihood based on the new parameters.
                nextLogLikelihood = CalculateNULogLikelihood(featureMatchList, uniformDensity, useDriftTime);
                // Increment the counter to show that another iteration has been completed.
                m_iteration++;
                // Set the convergence flag and exit the while loop if the convergence criteria is met.
                if (Math.Abs(nextLogLikelihood - m_logLikelihood) < EPSILON)
                {
                    converges = true;
                    break;
                }
                // Update the loglikelihood.
                m_logLikelihood = nextLogLikelihood;
            }
            // Return the convergence flag, which is still false unless changed to true above.
            return converges;
        }
        /// <summary>
        /// Calculate the loglikelihood for a match with the current parameters.
        /// </summary>
        /// <typeparam name="T">The type of the observed feature.  Derived from Feature.</typeparam>
        /// <typeparam name="U">The type of the target feature.  Derived from Feature.</typeparam>
        /// <param name="featureMatch">FeatureMatch to evaluate at.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <returns>The value of the loglikelihood evaluated at featureMatch.</returns>
        private double CalculateNULogLikelihood<T, U> (FeatureMatch<T,U> featureMatch, double uniformDensity) where T: Feature, new() where U: Feature, new()
        {
            if (featureMatch.UseDriftTimePredicted)
            {
                return ExpectationMaximization.NormalUniformLogLikelihood(featureMatch.ReducedDifferenceVector, MatrixUtilities.RemoveRow(m_meanVectorT, 3), MatrixUtilities.ReduceMatrix(m_covarianceMatrixT, 3), uniformDensity, m_mixtureProportion);
            }
            else
            {
                return ExpectationMaximization.NormalUniformLogLikelihood(featureMatch.ReducedDifferenceVector, MatrixUtilities.RemoveRow(m_meanVectorT, 4), MatrixUtilities.ReduceMatrix(m_covarianceMatrixT, 4), uniformDensity, m_mixtureProportion);
            }
        }
        /// <summary>
        /// Calculate the loglikelihood for matches with the current parameters.
        /// </summary>
        /// <typeparam name="T">The type of the observed feature.  Derived from Feature.</typeparam>
        /// <typeparam name="U">The type of the target feature.  Derived from Feature.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to evaluate at.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <returns>The value of the loglikelihood evaluated over featureMatchList.</returns>
        private double CalculateNULogLikelihood<T, U> (List<FeatureMatch<T, U>> featureMatchList, double uniformDensity) where T: Feature, new() where U: Feature, new()
        {
            double loglikelihood = 0.0;
            foreach (FeatureMatch<T, U> match in featureMatchList)
            {
                loglikelihood += CalculateNULogLikelihood(match, uniformDensity);
            }
            return loglikelihood;
        }
        /// <summary>
        /// Calculate the loglikelihood for matches with the current parameters.
        /// </summary>
        /// <typeparam name="T">The type of the observed feature.  Derived from Feature.</typeparam>
        /// <typeparam name="U">The type of the target feature.  Derived from Feature.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to evaluate at.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <param name="useDriftTime">Whether to use drift times in the calculations.</param>
        /// <returns>The value of the loglikelihood evaluated over featureMatchList.</returns>
        private double CalculateNULogLikelihood<T, U> (List<FeatureMatch<T, U>> featureMatchList, double uniformDensity, bool useDriftTime) where T: Feature, new() where U: Feature, new()
        {
            double logLikelihood = 0.0;
            if (useDriftTime)
            {
                logLikelihood = CalculateNULogLikelihood(featureMatchList, uniformDensity);
            }
            else
            {
                List<Matrix> dataList = new List<Matrix>();
                foreach (FeatureMatch<T, U> match in featureMatchList)
                {
                    dataList.Add(match.DifferenceVector);
                }
                logLikelihood = ExpectationMaximization.NormalUniformLogLikelihood(dataList, m_meanVectorT, m_covarianceMatrixT, uniformDensity, m_mixtureProportion);
            }
            return logLikelihood;
        }
        /// <summary>
        /// Update the mixture proportion for the normal-uniform mixture model.
        /// </summary>
        /// <typeparam name="T">The type of the observed feature.  Derived from Feature.</typeparam>
        /// <typeparam name="U">The type of the target feature.  Derived from Feature.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to evaluate at.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <param name="alphaList">List to be filled with individual mixture values.</param>
        /// <returns>The updated mixture proportion.</returns>
        private double UpdateNUMixtureParameter<T, U> (List<FeatureMatch<T,U>> featureMatchList, double uniformDensity, ref List<double> alphaList) where T: Feature, new() where U: Feature, new()
        {
            double nextMixtureParameter = 0.0;
            alphaList.Clear();

            foreach (FeatureMatch<T,U> match in featureMatchList)
            {
                double alpha = Math.Exp(CalculateNULogLikelihood(match, uniformDensity));
                alphaList.Add(alpha);
                nextMixtureParameter += alpha;
            }

            return (nextMixtureParameter / alphaList.Count);
        }
        /// <summary>
        /// Calculate the loglikelihood for a match with the current parameters.
        /// </summary>
        /// <typeparam name="T">The type of the observed feature.  Derived from Feature.</typeparam>
        /// <param name="featureMatch">FeatureMatch to evaluate at.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <returns>The value of the loglikelihood evaluated at featureMatch.</returns>
        private double CalculateNNULogLikelihood<T> (FeatureMatch<T,MassTag> featureMatch, double uniformDensity) where T: Feature, new()
        {
            if (featureMatch.UseDriftTimePredicted)
            {
                return ExpectationMaximization.NormalNormalUniformLogLikelihood(featureMatch.ReducedDifferenceVector, featureMatch.TargetFeature.PriorProbability, MatrixUtilities.RemoveRow(m_meanVectorT, 3), MatrixUtilities.ReduceMatrix(m_covarianceMatrixT, 3), MatrixUtilities.RemoveRow(m_meanVectorF, 3), MatrixUtilities.ReduceMatrix(m_covarianceMatrixF, 3), uniformDensity, m_mixtureProportion);
            }
            else
            {
                return ExpectationMaximization.NormalNormalUniformLogLikelihood(featureMatch.ReducedDifferenceVector, featureMatch.TargetFeature.PriorProbability, MatrixUtilities.RemoveRow(m_meanVectorT, 4), MatrixUtilities.ReduceMatrix(m_covarianceMatrixT, 4), MatrixUtilities.RemoveRow(m_meanVectorF, 4), MatrixUtilities.ReduceMatrix(m_covarianceMatrixF, 4), uniformDensity, m_mixtureProportion);
            }
        }
        /// <summary>
        /// Calculate the loglikelihood for matches with the current parameters.
        /// </summary>
        /// <typeparam name="T">The type of the observed feature.  Derived from Feature.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to evaluate at.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <returns>The value of the loglikelihood evaluated over featureMatchList.</returns>
        private double CalculateNNULogLikelihood<T> (List<FeatureMatch<T, MassTag>> featureMatchList, double uniformDensity) where T : Feature, new()
        {
            double loglikelihood = 0.0;
            foreach (FeatureMatch<T, MassTag> match in featureMatchList)
            {
                loglikelihood += CalculateNNULogLikelihood(match, uniformDensity);
            }
            return loglikelihood;
        }
        /// <summary>
        /// Calculate the loglikelihood for matches with the current parameters.
        /// </summary>
        /// <typeparam name="T">The type of the observed feature.  Derived from Feature.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to evaluate at.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <param name="useDriftTime">Whether to use drift times in the calculations.</param>
        /// <returns>The value of the loglikelihood evaluated over featureMatchList.</returns>
        private double CalculateNNULogLikelihood<T> (List<FeatureMatch<T, MassTag>> featureMatchList, double uniformDensity, bool useDriftTime) where T : Feature, new()
        {
            double logLikelihood = 0.0;
            if (useDriftTime)
            {
                logLikelihood = CalculateNNULogLikelihood(featureMatchList, uniformDensity);
            }
            else
            {
                List<Matrix> dataList = new List<Matrix>();
                List<double> priorList = new List<double>();
                foreach (FeatureMatch<T, MassTag> match in featureMatchList)
                {
                    dataList.Add(match.DifferenceVector);
                    priorList.Add(match.TargetFeature.PriorProbability);
                }
                logLikelihood = ExpectationMaximization.NormalNormalUniformLogLikelihood(dataList, priorList, m_meanVectorT, m_covarianceMatrixT, m_meanVectorF, m_covarianceMatrixF, uniformDensity, m_mixtureProportion);
            }
            return logLikelihood;
        }
        /// <summary>
        /// Update the mixture proportion for the normal-normal-uniform mixture model.
        /// </summary>
        /// <typeparam name="T">The type of the observed feature.  Derived from Feature.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to evaluate at.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <param name="alphaList">List to be filled with individual mixture values.</param>
        /// <returns>The updated mixture proportion.</returns>
        private double UpdateNNUMixtureParameter<T> (List<FeatureMatch<T, MassTag>> featureMatchList, double uniformDensity, ref List<double> alphaList) where T: Feature, new()
        {
            double nextMixtureParameter = 0.0;
            alphaList.Clear();

            foreach (FeatureMatch<T, MassTag> match in featureMatchList)
            {
                double alpha = Math.Exp(CalculateNNULogLikelihood(match, uniformDensity));
                alphaList.Add(alpha);
                nextMixtureParameter += alpha;
            }

            return (nextMixtureParameter / alphaList.Count);
        }
        /// <summary>
        /// Overload function to calculate STAC score for MassTag target data.
        /// </summary>
        /// <typeparam name="T">The type of the observed feature.  Derived from Feature.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to train data on.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <param name="useDriftTime">Whether to use drift times in the calculations.</param>
        /// <param name="usePrior">Whether to use prior probabilities in the calculation of the STAC score.</param>
        /// <returns>The STAC score corresponding to featureMatch.</returns>
        private double ComputeSTAC<T>(FeatureMatch<T, MassTag> featureMatch, double uniformDensity, bool useDriftTime, bool usePrior) where T : Feature, new()
        {
            if (usePrior)
            {
                return Math.Exp(CalculateNNULogLikelihood(featureMatch, uniformDensity));
            }
            else
            {
                return Math.Exp(CalculateNULogLikelihood(featureMatch, uniformDensity));
            }
        }
        /// <summary>
        /// Function to calculate STAC score.
        /// </summary>
        /// <typeparam name="T">Observed feature to be matched.  Derived from Feature.  Usually UMC or UMCCluster.</typeparam>
        /// <typeparam name="U">Feature to be matched to.  Derived from Feature.  Usually AMTTag.</typeparam>
        /// <param name="featureMatchList">List of FeatureMatches to train data on.</param>
        /// <param name="uniformDensity">Density of uniform distribution.</param>
        /// <param name="useDriftTime">Whether to use drift times in the calculations.</param>
        /// <param name="usePrior">Whether to use prior probabilities in the calculation of the STAC score.</param>
        /// <returns>The STAC score corresponding to featureMatch.</returns>
        private double ComputeSTAC<T, U> (FeatureMatch<T,U> featureMatch, double uniformDensity, bool useDriftTime, bool usePrior) where T: Feature, new() where U: Feature, new()
        {
            return Math.Exp(CalculateNULogLikelihood(featureMatch,uniformDensity));
        }
        #endregion
    }
}
