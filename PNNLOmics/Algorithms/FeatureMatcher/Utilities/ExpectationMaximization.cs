using System.Collections.Generic;
using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Utilities;
using PNNLOmics.Algorithms.FeatureMatcher.Data;

namespace PNNLOmics.Algorithms.FeatureMatcher.Utilities
{
    public static class ExpectationMaximization
    {
        # region Members
        const double EPSILON = 0.0001;
        const int MAX_ITERATIONS = 500;
        #endregion


        # region Update normal functions
        /// <summary>
        /// Update mean vector of normal distribution.  Used for Normal-Uniform mixture.
        /// </summary>
        /// <param name="dataList">List of difference matrices.</param>
        /// <param name="alphaList">List of mixture estimates corresponding to differences.</param>
        /// <returns>Updated Matrix containing mean of normal distribution.</returns>
        public static Matrix UpdateNormalMeanVector(List<Matrix> dataList, List<double> alphaList)
        {
            Matrix numerator = new Matrix(dataList[0].RowCount, 1, 0.0);
            double denominator = 0.0;

            for (int i = 0; i < dataList.Count; i++)
            {
                numerator += alphaList[i] * dataList[i];
                denominator += alphaList[i];
            }

            return ((1 / denominator) * numerator);
        }
        /// <summary>
        /// Update mean vector of normal distribution.  Used for Normal-Normal-Uniform mixture.
        /// </summary>
        /// <param name="dataList">List of difference matrices.</param>
        /// <param name="alphaList">List of mixture estimates corresponding to differences.</param>
        /// <param name="priorList">List of prior probabilities corresponding to differences.</param>
        /// <param name="secondNormal">Whether the data is from the second of the normal distributions, i.e. incorrect in AMT database.</param>
        /// <returns>Updated Matrix containing mean of normal distribution.</returns>
        public static Matrix UpdateNormalMeanVector(List<Matrix> dataList, List<double> alphaList, List<double> priorList, bool secondNormal)
        {
            Matrix numerator = new Matrix(dataList[0].RowCount, 1, 0.0);
            double denominator = 0.0;

            double multiplier = 1.0;
            double adder = 0;

            if (secondNormal)
            {
                multiplier = -1.0;
                adder = 1.0;
            }

            for (int i = 0; i < dataList.Count; i++)
            {
                double weight = alphaList[i] * (adder + multiplier * priorList[i]);
                numerator += weight * dataList[i];
                denominator += weight;
            }

            return ((1 / denominator) * numerator);
        }
        /// <summary>
        /// Update covariance matrix of normal distribution.  Used for Normal-Uniform mixture.
        /// </summary>
        /// <param name="dataList">List of difference matrices.</param>
        /// <param name="meanVector">Matrix containing mean parameters for the normal distribution.</param>
        /// <param name="alphaList">List of mixture estimates corresponding to differences</param>
        /// <param name="independent">Whether the dimensions of the normal distribution should be considered normal.  Returns a diagonal Matrix if true.  Should use false if unknown.</param>
        /// <returns>Updated Matrix containing covariances of normal distribution.</returns>
        public static Matrix UpdateNormalCovarianceMatrix(List<Matrix> dataList, Matrix meanVector, List<double> alphaList, bool independent)
        {
            Matrix numerator = new Matrix(meanVector.RowCount, meanVector.RowCount, 0.0);
            double denominator = 0.0;

            for (int i = 0; i < dataList.Count; i++)
            {
                Matrix dataT = dataList[i].Clone();
                dataT.Transpose();
                numerator += alphaList[i] * dataList[i] * dataT;
                denominator += alphaList[i];
            }

            Matrix covarianceMatrix = (1 / denominator) * numerator;

            if (independent)
            {
                Matrix indCovarianceMatrix = new Matrix(covarianceMatrix.RowCount, covarianceMatrix.ColumnCount, 0.0);
                for (int i = 0; i < covarianceMatrix.ColumnCount; i++)
                {
                    indCovarianceMatrix[i, i] = covarianceMatrix[i, i];
                }
                covarianceMatrix = indCovarianceMatrix.Clone();
            }

            for (int i = 0; i < meanVector.RowCount; i++)
            {
                if (covarianceMatrix[i, i] < 0.001)
                {
                    covarianceMatrix[i, i] = 0.001;
                }
            }

            return (covarianceMatrix);
        }
        /// <summary>
        /// Update covariance matrix of normal distribution.  Used for Normal-Normal-Uniform mixture.
        /// </summary>
        /// <param name="dataList">List of difference matrices.</param>
        /// <param name="meanVector">Matrix containing mean parameters for the normal distribution.</param>
        /// <param name="alphaList">List of mixture estimates corresponding to differences</param>
        /// <param name="priorList">List of prior probabilities corresponding to differences.</param>
        /// <param name="independent">Whether the dimensions of the normal distribution should be considered normal.  Returns a diagonal Matrix if true.  Should use false if unknown.</param>
        /// <param name="secondNormal">Whether the data is from the second of the normal distributions, i.e. incorrect in AMT database.</param>
        /// <returns>Updated Matrix containing covariances of normal distribution.</returns>
        public static Matrix UpdateNormalCovarianceMatrix(List<Matrix> dataList, Matrix meanVector, List<double> alphaList, List<double> priorList, bool independent, bool secondNormal)
        {
            Matrix numerator = new Matrix(meanVector.RowCount, meanVector.RowCount, 0.0);
            double denominator = 0.0;

            double multiplier = 1.0;
            double adder = 0;

            if (secondNormal)
            {
                multiplier = -1.0;
                adder = 1.0;
            }

            for (int i = 0; i < dataList.Count; i++)
            {
                Matrix dataT = dataList[i].Clone();
                dataT.Transpose();
                double weight = alphaList[i] * (adder + multiplier * priorList[i]);
                numerator += weight * dataList[i] * dataT;
                denominator += weight;
            }

            Matrix covarianceMatrix = (1 / denominator) * numerator;

            if (independent)
            {
                Matrix indCovarianceMatrix = new Matrix(covarianceMatrix.RowCount, covarianceMatrix.ColumnCount, 0.0);
                for (int i = 0; i < covarianceMatrix.ColumnCount; i++)
                {
                    indCovarianceMatrix[i, i] = covarianceMatrix[i, i];
                }
                covarianceMatrix = indCovarianceMatrix.Clone();
            }

            for (int i = 0; i < meanVector.RowCount; i++)
            {
                if (covarianceMatrix[i, i] < 0.000001)
                {
                    covarianceMatrix[i, i] = 0.000001;
                }
            }

            return (covarianceMatrix);
        }
        #endregion

        # region Normal-Uniform mixture
        /// <summary>
        /// Performs the EM algorithm on the given data and returns the parameters.
        /// </summary>
        /// <param name="dataList">A List containing matrices of the data.  These matrices must be of the same dimension as the mean vector.</param>
        /// <param name="meanVector">An [n x 1] matrix initially containing rough estimates and returning the mean of the normal distribution.</param>
        /// <param name="covarianceMatrix">An [n x n] positive definite matrix initially containing rough estimates and returning the covariance matrix for the normal distribution.</param>
        /// <param name="uniformTolerances">An [n x 1] matrix containing the half-width of the uniform distribution in each of the n-dimensions.</param>
        /// <param name="mixtureParameter">The proportion of the List thought to be from the normal distribution.  An initial estimate is passed and a refined proportion is returned.</param>
        /// <param name="logLikelihood">A placeholder to which the loglikelihood of the function will be returned.</param>
        /// <param name="independent">true/false:  Whether or not the multivariate normal distribution should be treated as independent, i.e. a diagonal covariance matrix.</param>
        /// <returns>A boolean flag indicating whether the EM algorithm achieved convergence.</returns>
        public static bool NormalUniformMixture(List<Matrix> dataList, ref Matrix meanVector, ref Matrix covarianceMatrix, Matrix uniformTolerances, ref double mixtureParameter, bool independent)
        {
            int iteration = 0;
            bool converges = false;
            // Check that the dimensions of the matrices agree.
            if (!(dataList[0].RowCount == meanVector.RowCount && dataList[0].ColumnCount == meanVector.ColumnCount && covarianceMatrix.RowCount == covarianceMatrix.ColumnCount
                   && covarianceMatrix.Rank() == covarianceMatrix.ColumnCount && covarianceMatrix.RowCount == meanVector.RowCount && uniformTolerances.RowCount == meanVector.RowCount))
            {
                throw new InvalidOperationException("Dimensions of matrices do not agree in ExpectationMaximization.NormalUniformMixture function.");
            }
            // Calculate the uniform density based on the tolerances passed.
            double uniformDensity = 1.0;
            for (int i = 0; i < uniformTolerances.RowCount; i++)
            {
                uniformDensity /= (2 * uniformTolerances[i, 0]);
            }
            // Calculate the starting loglikelihood and initialize a variable for the loglikelihood at the next iteration.
            double logLikelihood = NormalUniformLogLikelihood(dataList, meanVector, covarianceMatrix, uniformDensity, mixtureParameter);
            double nextLogLikelihood = 0.0;
            // Initialize the individual observation mixture estimates to the given mixture parameter and a list of priors to 1.
            List<double> alphaList = new List<double>(dataList.Count);
            List<double> priorList = new List<double>(dataList.Count);
            for (int i = 0; i < dataList.Count; i++)
            {
                alphaList.Add(mixtureParameter);
                priorList.Add(1.0);
            }
            // Step through the EM algorithm up to m_maxIterations time.
            while (iteration <= MAX_ITERATIONS)
            {
                // Update the parameters in the following order: mixture parameters, mean, covariance.
                mixtureParameter = UpdateNormalUniformMixtureParameter(dataList, meanVector, covarianceMatrix, mixtureParameter, uniformDensity, ref alphaList);
                meanVector = UpdateNormalMeanVector(dataList, alphaList, priorList, false);
                covarianceMatrix = UpdateNormalCovarianceMatrix(dataList, meanVector, alphaList, priorList, independent, false);
                // Calculate the loglikelihood based on the new parameters.
                nextLogLikelihood = NormalUniformLogLikelihood(dataList, meanVector, covarianceMatrix, uniformDensity, mixtureParameter);
                // Increment the counter to show that another iteration has been completed.
                iteration++;
                // Set the convergence flag and exit the while loop if the convergence criteria is met.
                if (Math.Abs(nextLogLikelihood - logLikelihood) < EPSILON)
                {
                    converges = true;
                    break;
                }
                // Update the loglikelihood.
                logLikelihood = nextLogLikelihood;
            }
            // Return the convergence flag, which is still false unless changed to true above.
            return (converges);
        }
        /// <summary>
        /// Calculate the loglikelihood for a normal-uniform mixture.
        /// </summary>
        /// <param name="data">Matrix of data.</param>
        /// <param name="meanVector">Matrix containing the current means.</param>
        /// <param name="covarianceMatrix">The current covariance matrix.</param>
        /// <param name="uniformDensity">The density of the uniform distribution.</param>
        /// <param name="mixtureParameter">The current mixture parameter.</param>
        /// <returns>The value of the loglikelihood evaluated at data.</returns>
        static public double NormalUniformLogLikelihood(Matrix data, Matrix meanVector, Matrix covarianceMatrix, double uniformDensity, double mixtureParameter, ref double stacScore)
        {
            double normalDensity = MathUtilities.MultivariateNormalDensity(data, meanVector, covarianceMatrix);
            if (normalDensity > 0)
            {
				double posteriorReal = mixtureParameter * normalDensity;
				double posteriorFalse = (1 - mixtureParameter) * uniformDensity;

				stacScore = (posteriorReal) / (posteriorReal + posteriorFalse);

				return Math.Log(posteriorReal + posteriorFalse);
            }
            return 0.0;
        }
        /// <summary>
        /// Calculate the loglikelihood for a normal-uniform mixture.
        /// </summary>
        /// <param name="data">List of Matrices of data.</param>
        /// <param name="meanVector">Matrix containing the current means.</param>
        /// <param name="covarianceMatrix">The current covariance matrix.</param>
        /// <param name="uniformDensity">The density of the uniform distribution.</param>
        /// <param name="mixtureParameter">The current mixture parameter.</param>
        /// <returns>The value of the loglikelihood evaluated over dataList.</returns>
        static public double NormalUniformLogLikelihood(List<Matrix> dataList, Matrix meanVector, Matrix covarianceMatrix, double uniformDensity, double mixtureParameter)
        {
            double loglikelihood = 0.0;
			double stacScore = 0.0;
            foreach (Matrix data in dataList)
            {
                loglikelihood += NormalUniformLogLikelihood(data, meanVector, covarianceMatrix, uniformDensity, mixtureParameter, ref stacScore);
            }
            return loglikelihood;
        }
        /// <summary>
        /// Update the mixture parameter for the normal-uniform mixture model.
        /// </summary>
        /// <param name="dataList">List of matrices containing the data.</param>
        /// <param name="meanVector">Matrix containing the current means.</param>
        /// <param name="covarianceMatrix">The current covariance matrix.</param>
        /// <param name="mixtureParameter">The current mixture parameter.</param>
        /// <param name="uniformDensity">The density of the uniform distribution.</param>
        /// <param name="alphaList">A List of observation-wise mixture proportion estimates to be updated and returned.</param>
        /// <returns>The updated mixture parameter.</returns>
        static public double UpdateNormalUniformMixtureParameter(List<Matrix> dataList, Matrix meanVector, Matrix covarianceMatrix, double mixtureParameter, double uniformDensity, ref List<double> alphaList)
        {
            double nextMixtureParameter = 0.0;
			double stacScore = 0.0;

            for (int i = 0; i < dataList.Count; i++)
            {
                alphaList[i] = Math.Exp(NormalUniformLogLikelihood(dataList[i], meanVector, covarianceMatrix, uniformDensity, mixtureParameter, ref stacScore));
                nextMixtureParameter += alphaList[i];
            }

            return (nextMixtureParameter / alphaList.Count);
        }
        # endregion

        #region Normal-Normal-Uniform mixture
        /// <summary>
        /// Calculate the loglikelihood for a normal-normal-uniform mixture.
        /// </summary>
        /// <param name="data">Matrix of data.</param>
        /// <param name="prior">The prior probability of being correct, i.e. the probability of being from the normal distribution with parameters meanVectorT and covarianceMatrixT.</param>
        /// <param name="meanVectorT">Matrix containing the current means for the true normal distribution.</param>
        /// <param name="covarianceMatrixT">The current covariance matrix for the true normal distribution.</param>
        /// <param name="meanVectorF">Matrix containing the current means for the false normal distribution.</param>
        /// <param name="covarianceMatrixF">The current covariance matrix for the false normal distribution.</param>
        /// <param name="uniformDensity">The density of the uniform distribution.</param>
        /// <param name="mixtureParameter">The current mixture parameter.</param>
        /// <returns>The value of the loglikelihood evaluated at data.</returns>
        static public double NormalNormalUniformLogLikelihood(Matrix data, double prior, Matrix meanVectorT, Matrix covarianceMatrixT, Matrix meanVectorF, Matrix covarianceMatrixF, double uniformDensity, double mixtureParameter, ref double stacScore)
        {
            double normalDensityT = MathUtilities.MultivariateNormalDensity(data, meanVectorT, covarianceMatrixT);
            double normalDensityF = MathUtilities.MultivariateNormalDensity(data, meanVectorF, covarianceMatrixF);
            if (normalDensityT > 0)
            {
				double posteriorReal = mixtureParameter * prior * normalDensityT;
				double posteriorIReal = mixtureParameter * (1 - prior) * normalDensityF;
				double posteriorFalse = (1 - mixtureParameter) * uniformDensity;

				stacScore = (posteriorReal) / (posteriorReal + posteriorIReal + posteriorFalse);

				return Math.Log(posteriorReal + posteriorIReal + posteriorFalse);
            }

			stacScore = 0.0;
            return 0.0;
        }

		// TODO: XML Comments
		static public double GetAlpha(Matrix data, double prior, Matrix meanVectorT, Matrix covarianceMatrixT, Matrix meanVectorF, Matrix covarianceMatrixF, double uniformDensity, double mixtureParameter)
		{
			double normalDensityT = MathUtilities.MultivariateNormalDensity(data, meanVectorT, covarianceMatrixT);
			double normalDensityF = MathUtilities.MultivariateNormalDensity(data, meanVectorF, covarianceMatrixF);
			if (normalDensityT > 0)
			{
				double posteriorReal = mixtureParameter * prior * normalDensityT;
				double posteriorIReal = mixtureParameter * (1 - prior) * normalDensityF;
				double posteriorFalse = (1 - mixtureParameter) * uniformDensity;

				return (posteriorReal + posteriorIReal) / (posteriorReal + posteriorIReal + posteriorFalse);
			}
			return 0.0;
		}
        /// <summary>
        /// Calculate the loglikelihood for a normal-normal-uniform mixture.
        /// </summary>
        /// <param name="data">List of Matrices of data.</param>
        /// <param name="prior">List of prior probabilities of being correct, i.e. the probability of being from the normal distribution with parameters meanVectorT and covarianceMatrixT.</param>
        /// <param name="meanVectorT">Matrix containing the current means for the true normal distribution.</param>
        /// <param name="covarianceMatrixT">The current covariance matrix for the true normal distribution.</param>
        /// <param name="meanVectorF">Matrix containing the current means for the false normal distribution.</param>
        /// <param name="covarianceMatrixF">The current covariance matrix for the false normal distribution.</param>
        /// <param name="uniformDensity">The density of the uniform distribution.</param>
        /// <param name="mixtureParameter">The current mixture parameter.</param>
        /// <returns>The value of the loglikelihood evaluated over data.</returns>
        static public double NormalNormalUniformLogLikelihood(List<Matrix> dataList, List<double> prior, Matrix meanVectorT, Matrix covarianceMatrixT, Matrix meanVectorF, Matrix covarianceMatrixF, double uniformDensity, double mixtureParameter)
        {
            double logLikelihood = 0.0;
			double stacScore = 0.0;
            for (int i = 0; i < dataList.Count; i++)
            {
				logLikelihood += NormalNormalUniformLogLikelihood(dataList[i], prior[i], meanVectorT, covarianceMatrixT, meanVectorF, covarianceMatrixF, uniformDensity, mixtureParameter, ref stacScore);
            }
            return logLikelihood;
        }
		
		// TODO: XML Comments
		static public List<double> GetAlphaList(List<Matrix> dataList, List<double> prior, Matrix meanVectorT, Matrix covarianceMatrixT, Matrix meanVectorF, Matrix covarianceMatrixF, double uniformDensity, double mixtureParameter)
		{
			List<double> alphaList = new List<double>(dataList.Count);
			for (int i = 0; i < dataList.Count; i++)
			{
				alphaList.Add(GetAlpha(dataList[i], prior[i], meanVectorT, covarianceMatrixT, meanVectorF, covarianceMatrixF, uniformDensity, mixtureParameter));
			}
			return alphaList;
		}
        /// <summary>
        /// Update the mixture parameter for the normal-normal-uniform mixture model.
        /// </summary>
        /// <param name="dataList">List of matrices containing the data.</param>
        /// <param name="prior">The prior probability of being correct, i.e. the probability of being from the normal distribution with parameters meanVectorT and covarianceMatrixT.</param>
        /// <param name="meanVectorT">Matrix containing the current means for the true normal distribution.</param>
        /// <param name="covarianceMatrixT">The current covariance matrix for the true normal distribution.</param>
        /// <param name="meanVectorF">Matrix containing the current means for the false normal distribution.</param>
        /// <param name="covarianceMatrixF">The current covariance matrix for the false normal distribution.</param>
        /// <param name="mixtureParameter">The current mixture parameter.</param>
        /// <param name="uniformDensity">The density of the uniform distribution.</param>
        /// <param name="alphaList">A List of observation-wise mixture proportion estimates to be updated and returned.</param>
        /// <returns>The updated mixture parameter.</returns>
        static public double UpdateNormalNormalUniformMixtureParameter(List<Matrix> dataList, List<double> priorList, Matrix meanVectorT, Matrix covarianceMatrixT, Matrix meanVectorF, Matrix covarianceMatrixF, double mixtureParameter, double uniformDensity, ref List<double> alphaList)
        {
            double nextMixtureParameter = 0.0;
            double numerator = 0.0;
            for (int i = 0; i < dataList.Count; i++)
            {
                numerator = priorList[i] * MathUtilities.MultivariateNormalDensity(dataList[i], meanVectorT, covarianceMatrixT) + (1 - priorList[i]) * MathUtilities.MultivariateNormalDensity(dataList[i], meanVectorF, covarianceMatrixF);
                alphaList[i] = (numerator * mixtureParameter) / (numerator * mixtureParameter + (1 - mixtureParameter) * uniformDensity);
                nextMixtureParameter += alphaList[i];
            }
            return (nextMixtureParameter / alphaList.Count);
        }
        #endregion
    }
}