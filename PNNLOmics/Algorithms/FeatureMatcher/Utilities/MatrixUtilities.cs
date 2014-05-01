using System;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data.Features;
using PNNLOmics.Utilities;

namespace PNNLOmics.Algorithms.FeatureMatcher.Utilities
{
    static public class MatrixUtilities
    {
        #region Remove rows/columns
        /// <summary>
        /// Removes the row and column in which a 0 occurs along the diagonal of a square matrix.
        /// </summary>
        /// <param name="matrix">A square matrix, possibly with a 0 on the diagonal.</param>
        /// <returns>A square matrix with no 0's on the diagonal.</returns>
        static public Matrix ReduceMatrix(Matrix matrix)
        {
            var rows = matrix.RowCount;

            if (rows != matrix.ColumnCount)
            {
                throw new InvalidOperationException("Matrix is not square in function ReduceMatrix.");
            }

            if (rows == matrix.Rank())
            {
                return matrix;
            }
            var reducedDimension = matrix.Rank();
            var reducedMatrix = matrix.Clone();
                
            for (var rIndex = 0; rIndex < rows; rIndex++)
            {
                if (matrix[rIndex, rIndex] == 0)
                {
                    reducedMatrix = ReduceMatrix(reducedMatrix, rIndex);
                }
            }

            return reducedMatrix;
        }
        /// <summary>
        /// Remove the given row and column from a matrix.
        /// </summary>
        /// <param name="matrix">The matrix from with the row and column are to be removed.</param>
        /// <param name="rowColumnIndex">The index of the row and column which are to be removed.</param>
        /// <returns>A copy of matrix without the row and column given by rowColumnIndex.</returns>
        static public Matrix ReduceMatrix(Matrix matrix, int rowColumnIndex)
        {
            var rows = matrix.RowCount;

            if (rowColumnIndex >= rows)
            {
                throw new InvalidOperationException("Given rowColumnIndex is out of range of matrix in function ReduceMatrix.");
            }
            if (rows != matrix.ColumnCount)
            {
                throw new InvalidOperationException("Matrix is not square in function ReduceMatrix.");
            }
            var reducedMatrix = new Matrix(rows-1, rows-1, 0.0);
            var rowIndex = 0;

            for (var rIndex = 0; rIndex < rows; rIndex++)
            {
                if (rIndex != rowColumnIndex)
                {
                    var colIndex = 0;
                    for (var cIndex = 0; cIndex < rows; cIndex++)
                    {
                        if (cIndex != rowColumnIndex)
                        {
                            reducedMatrix[rowIndex, colIndex] = matrix[rIndex, cIndex];
                            colIndex++;
                        }
                    }
                    rowIndex++;
                }
            }

            return reducedMatrix;
        }
        /// <summary>
        /// Remove a single row from an [n x 1] matix.
        /// </summary>
        /// <param name="matrix">The original matrix.</param>
        /// <param name="rowIndex">The index of the row to be removed.</param>
        /// <returns>The matrix without the given row.</returns>
        static public Matrix RemoveRow(Matrix matrix, int rowIndex)
        {
            var rows = matrix.RowCount;

            if (rowIndex >= rows)
            {
                throw new InvalidOperationException("Given rowIndex is out of range of matrix in function RemoveRow.");
            }

            if (matrix.ColumnCount > 1)
            {
                throw new InvalidOperationException("Given matrix in function RemoveRow must have no more than 1 column.");
            }

            var reducedMatrix= new Matrix(rows-1,1,0.0);
            var rowCount = 0;

            for (var rIndex = 0; rIndex < rows; rIndex++)
            {
                if (rIndex != rowIndex)
                {
                    reducedMatrix[rowCount, 0] = matrix[rIndex, 0];
                    rowCount++;
                }
            }

            return reducedMatrix;
        }
        #endregion

        #region Difference vectors

        /// <summary>
        /// Find the differences between any two features.
        /// </summary>
        /// <typeparam name="T">Feature or derived class.</typeparam>
        /// <typeparam name="U">Feature or derived class.</typeparam>
        /// <param name="feature1">Observed feature to be compared to other feature.</param>
        /// <param name="feature2">Feature (MassTag) to be compared to.</param>
        /// <param name="driftTime">true/false:  Whether or not to include the drift time difference.</param>
        /// <returns>An [n x 1] Matrix containing the differences between the two features.</returns>
        public static Matrix Differences<T, U>(T feature1, U feature2, bool driftTime)
            where T : FeatureLight
            where U : FeatureLight
        {
            var dimension = 2;
            if (driftTime)
                dimension++;
            var differences = new Matrix(dimension, 1, 0.0);

			if (feature1.MassMonoisotopicAligned != double.NaN && feature1.MassMonoisotopicAligned > 0.0)
			{
				differences[0, 0] = MathUtilities.MassDifferenceInPPM(feature1.MassMonoisotopicAligned, feature2.MassMonoisotopic);
			}
			else
			{
				differences[0, 0] = MathUtilities.MassDifferenceInPPM(feature1.MassMonoisotopic, feature2.MassMonoisotopic);
			}

			if (feature1.NetAligned != double.NaN && feature1.NetAligned > 0.0)
			{
				differences[1, 0] = feature1.NetAligned - feature2.Net;
			}
			else
			{
				differences[1, 0] = feature1.Net - feature2.Net;
			}

			if (driftTime)
			{
				double feature1DriftTime = 0;
				double feature2DriftTime = 0;

				if (feature1.DriftTimeAligned != double.NaN && feature1.DriftTimeAligned > 0.0)
				{
					feature1DriftTime = feature1.DriftTimeAligned;
				}
				else
				{
					feature1DriftTime = feature1.DriftTime;
				}
				if (feature2.DriftTimeAligned != double.NaN && feature2.DriftTimeAligned > 0.0)
				{
					feature2DriftTime = feature2.DriftTimeAligned;
				}
				else
				{
					feature2DriftTime = feature2.DriftTime;
				}

				differences[2, 0] = feature1DriftTime - feature2DriftTime;
			}

            return differences;
        }
        
        
        #endregion
    }
}
