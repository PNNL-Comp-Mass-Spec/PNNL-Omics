using System.Collections.Generic;
using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Utilities;

namespace PNNLOmics.Algorithms.FeatureMatcher.Utilities
{
    static public class ExpectationMaximization
    {
        # region Algorithm variables
        /// <summary>
        /// The maximum number of EM iterations to perform.
        /// </summary>
        static private int m_maxIterations;

        static public int MaxIterations
        {
            get { return m_maxIterations; }
            set { m_maxIterations = value; }
        }
        /// <summary>
        /// The current number of iterations.
        /// </summary>
        static private int m_iteration;

        static public int Iteration
        {
            get { return m_iteration; }
            set { m_iteration = value; }
        }
        /// <summary>
        /// The convergence criteria.  The log likelihood must change by less than this amount to achieve convergence.
        /// </summary>
        static private double m_epsilon;

        static public double Epsilon
        {
            get { return m_epsilon; }
            set { m_epsilon = value; }
        }
        /// <summary>
        /// A variable asserting whether or not the algorithm reached the convergence criteria.
        /// </summary>
        static private bool m_converges;

        static public bool Converges
        {
            get { return m_converges; }
        }
        #endregion

        # region Variable routines
        /// <summary>
        /// Sets epsilon and the number of iterations to default values if not already set.
        /// </summary>
        static private void CheckParameters()
        {
            if (m_epsilon == 0)
            {
                m_epsilon = 0.0001;
            }
            if (m_maxIterations == 0)
            {
                m_maxIterations = 500;
            }
        }
        /// <summary>
        /// Resets the iteration count to 0.
        /// </summary>
        static private void ResetIterations()
        {
            m_iteration = 0;
        }
        # endregion

        # region Update normal parameters
        static private Matrix UpdateNormalMeanVector(List<Matrix> dataList, List<double> alphaList, List<double> priorList, bool secondNormal)
        {
            Matrix numerator = new Matrix(dataList[0].RowCount,1,0.0);
            double denominator = 0.0;

            double multiplier = 1.0;
            double adder = 0;

            if( secondNormal )
            {
                multiplier = -1.0;
                adder = 1.0;
            }

            for (int i = 0; i < dataList.Count; i++)
            {
                numerator += alphaList[i] * (adder + multiplier * priorList[i]) * dataList[i];
                denominator += alphaList[i] * (adder + multiplier * priorList[i]);
            }

            return ((1/denominator)*numerator);
        }

        static private Matrix UpdateNormalCovarianceMatrix(List<Matrix> dataList, Matrix meanVector, List<double> alphaList, List<double> priorList, bool independent, bool secondNormal)
        {
            Matrix numerator = new Matrix(meanVector.RowCount, meanVector.RowCount, 0.0);
            double denominator = 0.0;

            double multiplier = 1.0;
            double adder = 0;

            if( secondNormal )
            {
                multiplier = -1.0;
                adder = 1.0;
            }

            for (int i = 0; i < dataList.Count; i++)
            {
                Matrix dataT = dataList[i].Clone();
                dataT.Transpose();
                numerator += alphaList[i] * (adder + multiplier * priorList[i]) * dataList[i] * dataT;
                denominator += alphaList[i] * (adder + multiplier * priorList[i]);
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
                if(covarianceMatrix[i,i]<0.000001)
                {
                    covarianceMatrix[i, i] = 0.000001;
                }
            }

            return (covarianceMatrix);
        }
        # endregion

        # region Normal Uniform mixture
        /// <summary>
        /// Performs the EM algorithm on the given data and returns the parameters.  A minimum of 100 values must be passed in order for the EM algorithm to perform.
        /// </summary>
        /// <param name="dataList">A List containing matrices of the data.  These matrices must be of the same dimension as the mean vector.</param>
        /// <param name="meanVector">An [n x 1] matrix initially containing rough estimates and returning the mean of the normal distribution.</param>
        /// <param name="covarianceMatrix">An [n x n] positive definite matrix initially containing rough estimates and returning the covariance matrix for the normal distribution.</param>
        /// <param name="uniformTolerances">An [n x 1] matrix containing the half-width of the uniform distribution in each of the n-dimensions.</param>
        /// <param name="mixtureParameter">The proportion of the List thought to be from the normal distribution.  An initial estimate is passed and a refined proportion is returned.</param>
        /// <param name="logLikelihood">A placeholder to which the loglikelihood of the function will be returned.</param>
        /// <param name="independent">true/false:  Whether or not the multivariate normal distribution should be treated as independent, i.e. a diagonal covariance matrix.</param>
        /// <returns>A boolean flag indicating whether the EM algorithm achieved convergence.</returns>
        static public bool NormalUniformMixture(List<Matrix> dataList, ref Matrix meanVector, ref Matrix covarianceMatrix, Matrix uniformTolerances, ref double mixtureParameter, 
                                                    ref double logLikelihood, bool independent)
        {
            // Check that the dimensions of the matrices agree.
            if( !(dataList[0].RowCount==meanVector.RowCount && dataList[0].ColumnCount==meanVector.ColumnCount && covarianceMatrix.RowCount==covarianceMatrix.ColumnCount
                   && covarianceMatrix.Rank()==covarianceMatrix.ColumnCount && covarianceMatrix.RowCount==meanVector.RowCount && uniformTolerances.RowCount==meanVector.RowCount) )
            {
                throw new InvalidOperationException("Dimensions of matrices do not agree in ExpectationMaximization.NormalUniformMixture function.");
            }
            // Check that a minimal sufficient amount of data is available for reasonable estimates (for up to 3 dimensions).
            if (dataList.Count < 100)
            {
                throw new NotSupportedException("Too few data points passed to use ExpectationMaximization.NormalUniformMixture.");
            }
            // Check that the EM parameters are set and reset the iteration count to 0.
            CheckParameters();
            ResetIterations();
            // Calculate the uniform density based on the tolerances passed.
            double uniformDensity = 1.0;
            for (int i = 0; i < uniformTolerances.RowCount; i++)
            {
                uniformDensity /= (2*uniformTolerances[i,0]);
            }
            // Calculate the starting loglikelihood and initialize a variable for the loglikelihood at the next iteration.
            logLikelihood = NormalUniformLogLikelihood(dataList, meanVector, covarianceMatrix, uniformDensity, mixtureParameter);
            double nextLogLikelihood=0.0;
            // Initialize the individual observation mixture estimates to the given mixture parameter and a list of priors to 1.
            List<double> alphaList = new List<double>(dataList.Count);
            List<double> priorList = new List<double>(dataList.Count);
            for (int i = 0; i < dataList.Count; i++)
            {
                alphaList.Add(mixtureParameter);
                priorList.Add(1.0);
            }
            // Step through the EM algorithm up to m_maxIterations time.
            while (m_iteration <= m_maxIterations)
            {
                // Update the parameters in the following order: mixture parameters, mean, covariance.
                mixtureParameter = UpdateNormalUniformMixtureParameter(dataList, meanVector, covarianceMatrix, mixtureParameter, uniformDensity, ref alphaList);
                meanVector = UpdateNormalMeanVector(dataList, alphaList, priorList, false);
                covarianceMatrix = UpdateNormalCovarianceMatrix(dataList, meanVector, alphaList, priorList, independent, false);
                // Calculate the loglikelihood based on the new parameters.
                nextLogLikelihood = NormalUniformLogLikelihood(dataList, meanVector, covarianceMatrix, uniformDensity, mixtureParameter);
                // Increment the counter to show that another iteration has been completed.
                m_iteration++;
                // Set the convergence flag and exit the while loop if the convergence criteria is met.
                if (Math.Abs(nextLogLikelihood - logLikelihood) < m_epsilon)
                {
                    m_converges = true;
                    break;
                }
                // Update the loglikelihood.
                logLikelihood = nextLogLikelihood;
            }
            // Return the convergence flag, which is still false unless changed to true above.
            return (m_converges);
        }
        /// <summary>
        /// Calculate the loglikelihood for a normal uniform mixture.
        /// </summary>
        /// <param name="dataList">List containing matrices of the data.</param>
        /// <param name="meanVector">Matrix containing the current means.</param>
        /// <param name="covarianceMatrix">The current covariance matrix.</param>
        /// <param name="uniformDensity">The density of the uniform distribution.</param>
        /// <param name="mixtureParameter">The current mixture parameter.</param>
        /// <returns></returns>
        static private double NormalUniformLogLikelihood(List<Matrix> dataList, Matrix meanVector, Matrix covarianceMatrix, double uniformDensity, double mixtureParameter)
        {
            double logLikelihood = 0;
            double numerator = 0;
            for (int i = 0; i < dataList.Count; i++)
            {
                double normalDensity = MathUtilities.MultivariateNormalDensity(dataList[i], meanVector, covarianceMatrix);
                if (normalDensity > 0)
                {
                    numerator = mixtureParameter * normalDensity;
                    logLikelihood += Math.Log(numerator) - Math.Log(numerator + (1 - mixtureParameter) * uniformDensity);
                }
            }
            return (logLikelihood);
        }
        /// <summary>
        /// Update the mixture parameter for the normal-uniform mixture model.
        /// </summary>
        /// <param name="dataList">List of matrices containing the data.</param>
        /// <param name="meanVector">Matrix containing the current means.</param>
        /// <param name="covarianceMatrix">The current covariance matrix.</param>
        /// <param name="mixtureParameter">The current mixture parameter.</param>
        /// <param name="uniformDensity">The density of the uniform distribution.</param>
        /// <param name="alphaList">A List of observation-wise mixture proportions estimates is passed initially and an updated list is returned.</param>
        /// <returns></returns>
        static private double UpdateNormalUniformMixtureParameter(List<Matrix> dataList, Matrix meanVector, Matrix covarianceMatrix, double mixtureParameter, double uniformDensity, ref List<double> alphaList)
        {
            double numerator = 0.0;
            double nextMixtureParameter = 0.0;

            for (int i = 0; i < dataList.Count; i++)
            {
                numerator = mixtureParameter * MathUtilities.MultivariateNormalDensity(dataList[i], meanVector, covarianceMatrix);
                alphaList[i] = numerator / (numerator + (1 - mixtureParameter) * uniformDensity);
                nextMixtureParameter += alphaList[i];
            }

            return (nextMixtureParameter/alphaList.Count);
        }
        # endregion

        #region Normal Normal Uniform mixture -- incomplete!!
        static public int NormalNormalUniformMixture(List<double> featureList)
        {
            CheckParameters();
            return (-99);
        }

        static private double NormalNormalUniformLogLikelihood(List<Matrix> dataList, Matrix meanVector, Matrix covarianceMatrix, double uniformDensity, double mixtureParameter)
        {
            double logLikelihood = 0;
            double numerator = 0;
            for (int i = 0; i < dataList.Count; i++)
            {
                double normalDensity = MathUtilities.MultivariateNormalDensity(dataList[i], meanVector, covarianceMatrix);
                if (normalDensity > 0)
                {
                    numerator = mixtureParameter * normalDensity;
                    logLikelihood += Math.Log(numerator) - Math.Log(numerator + (1 - mixtureParameter) * uniformDensity);
                }
            }
            return (logLikelihood);
        }
        #endregion
    }
}