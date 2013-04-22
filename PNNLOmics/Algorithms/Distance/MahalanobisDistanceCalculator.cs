using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace PNNLOmics.Algorithms.Distance
{
	/// <summary>
	/// Contains methods used for calculating the Mahalanobis distance between 2 clusters of data.
	/// </summary>
	public class MahalanobisDistanceCalculator
	{
		/// <summary>
		/// Calculates the arithemtic mean of each column in the given DenseMatrix.
		/// The column index will match with the returned array index.
		/// </summary>
		/// <param name="matrix">An n x m matrix of double values.</param>
		/// <returns>An array that contains an arithmetic mean for each column of the given matrix.</returns>
		public static double[] CalculateArithmeticMean(DenseMatrix matrix)
		{
			int numRows = matrix.RowCount;
			int numColumns = matrix.ColumnCount;

			double[] meanArray = new double[numColumns];

			for (int column = 0; column < numColumns; column++)
			{
				List<double> currentData = new List<double>();
				for (int row = 0; row < numRows; row++)
				{
					currentData.Add(matrix[row, column]);
				}
				meanArray[column] = currentData.Average();
			}
			return meanArray;
		}

		/// <summary>
		/// Creates a new DenseMatrix that is centered around the arithmetic mean.
		/// </summary>
		/// <param name="matrix">An n x m matrix that contains double values.</param>
		/// <param name="arithmeticMeans">The arithmetic means that have already been calculated.</param>
		/// <returns>The centered matrix.</returns>
		public static DenseMatrix CenterMatrixOnArithmeticMean(DenseMatrix matrix, double[] arithmeticMeans)
		{
			DenseMatrix centeredMatrix = new DenseMatrix(matrix.RowCount, arithmeticMeans.Length);

			for (int i = 0; i < matrix.RowCount; i++)
			{
				for (int j = 0; j < arithmeticMeans.Length; j++)
				{
					centeredMatrix[i, j] = matrix[i, j] - arithmeticMeans[j];
				}
			}
			return centeredMatrix;
		}

		/// <summary>
		/// Creates a covariance matrix based on the given matrix and arithmetic means.
		/// </summary>
		/// <param name="matrix">An n x m matrix that contains double values.</param>
		/// /// <param name="arithmeticMeans">The arithmetic means that have already been calculated.</param>
		/// <returns>The covariance matrix.</returns>
		public static DenseMatrix CreateCovarianceMatrix(DenseMatrix matrix, double[] arithmeticMeans)
		{
			DenseMatrix centeredMatrix = CenterMatrixOnArithmeticMean(matrix, arithmeticMeans);

			DenseMatrix covarianceMatrix = (DenseMatrix)centeredMatrix.Transpose() * centeredMatrix * (1.0 / matrix.RowCount);

			return covarianceMatrix;

		}

		/// <summary>
		/// Creates a pooled covariance matrix using 2 given matrices.
		/// </summary>
		/// <param name="covarianceMatrixA">The covariance matrix of group A.</param>
		/// <param name="covarianceMatrixB">The covariance matrix of group B.</param>
		/// <returns>The pooled covariance matrix.</returns>
		public static DenseMatrix CreatePooledCovarianceMatrix(DenseMatrix covarianceMatrixA, DenseMatrix covarianceMatrixB, 
																	DenseMatrix matrixA, DenseMatrix matrixB)
		{
			double numRowsA = matrixA.RowCount;
			double numRowsB = matrixB.RowCount;
			double totalNumRows = numRowsA + numRowsB;

			double weightedFactorA = numRowsA / totalNumRows;
			double weightedFactorB = numRowsB / totalNumRows;

			DenseMatrix pooledCovarianceMatrix = new DenseMatrix(covarianceMatrixA.RowCount, covarianceMatrixB.ColumnCount);

			for (int x = 0; x < covarianceMatrixA.RowCount; x++)
			{
				for (int y = 0; y < covarianceMatrixB.ColumnCount; y++)
				{
					pooledCovarianceMatrix[x, y] = (covarianceMatrixA[x, y] * weightedFactorA) + (covarianceMatrixB[x, y] * weightedFactorB);
				}
			}
			return pooledCovarianceMatrix;

		}

		/// <summary>
		/// Creates an n x 1 matrix that is the mean difference
		/// </summary>
		/// <param name="meanA">The arithmetic mean values of Group A.</param>
		/// <param name="meanB">The arithmetic mean values of Group B.</param>
		/// <returns>The mean difference matrix.</returns>
		public static DenseMatrix CreateMeanDifferenceMatrix(double[] meanA, double[] meanB)
		{
			if(meanA.Length != meanB.Length)
			{
				throw new Exception("The 2 arrays passed in to CreateMeanDifferenceMatrix must be the same length.");
			}

			DenseMatrix meanDifferenceMatrix = new DenseMatrix(meanA.Length, 1);

			for (int x = 0; x < meanA.Length; x++)
            {
				meanDifferenceMatrix[x, 0] = meanA[x] - meanB[x];
            }
		    return meanDifferenceMatrix;
		}

		/// <summary>
		/// Calculates the mahalanobis distance between 2 matrices.
		/// </summary>
		/// <param name="matrixA">The matrix representing group A.</param>
		/// <param name="matrixB">The matrix representing group B.</param>
		/// <returns>The mahalnobis distance.</returns>
		public static double CalculateMahalanobisDistance(DenseMatrix matrixA, DenseMatrix matrixB)
		{
			double[] meanA = CalculateArithmeticMean(matrixA);
			double[] meanB = CalculateArithmeticMean(matrixB);

			DenseMatrix covarianceMatrixA = CreateCovarianceMatrix(matrixA, meanA);
			DenseMatrix covarianceMatrixB = CreateCovarianceMatrix(matrixB, meanB);

			DenseMatrix pooledCovarianceMatrix = CreatePooledCovarianceMatrix(covarianceMatrixA, covarianceMatrixB, matrixA, matrixB);
			DenseMatrix inversePooledCovarianceMatrix = (DenseMatrix)pooledCovarianceMatrix.Inverse();

			DenseMatrix meanDifferenceMatrix = CreateMeanDifferenceMatrix(meanA, meanB);
			DenseMatrix transposedMeanDifferenceMatrix = (DenseMatrix)meanDifferenceMatrix.Transpose();

			Matrix<double> finalMatrix = transposedMeanDifferenceMatrix.Multiply(inversePooledCovarianceMatrix).Multiply(meanDifferenceMatrix);

			double mahalanobisDistance = Math.Sqrt(finalMatrix[0, 0]);

			return mahalanobisDistance;
		}
	}
}