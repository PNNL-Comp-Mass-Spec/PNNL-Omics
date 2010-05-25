using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data;
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
            int rows = matrix.RowCount;

            if (rows != matrix.ColumnCount)
            {
                throw new InvalidOperationException("Matrix is not square in function ReduceMatrix.");
            }

            if (rows == matrix.Rank())
            {
                return matrix;
            }
            else
            {
                int reducedDimension = matrix.Rank();
                Matrix reducedMatrix = matrix.Clone();
                
                for (int rIndex = 0; rIndex < rows; rIndex++)
                {
                    if (matrix[rIndex, rIndex] == 0)
                    {
                        reducedMatrix = ReduceMatrix(reducedMatrix, rIndex);
                    }
                }

                return reducedMatrix;
            }
        }
        /// <summary>
        /// Remove the given row and column from a matrix.
        /// </summary>
        /// <param name="matrix">The matrix from with the row and column are to be removed.</param>
        /// <param name="rowColumnIndex">The index of the row and column which are to be removed.</param>
        /// <returns>A copy of matrix without the row and column given by rowColumnIndex.</returns>
        static public Matrix ReduceMatrix(Matrix matrix, int rowColumnIndex)
        {
            int rows = matrix.RowCount;

            if (rowColumnIndex >= rows)
            {
                throw new InvalidOperationException("Given rowColumnIndex is out of range of matrix in function ReduceMatrix.");
            }
            if (rows != matrix.ColumnCount)
            {
                throw new InvalidOperationException("Matrix is not square in function ReduceMatrix.");
            }
            Matrix reducedMatrix = new Matrix(rows-1, rows-1, 0.0);
            int rowIndex = 0;

            for (int rIndex = 0; rIndex < rows; rIndex++)
            {
                if (rIndex != rowColumnIndex)
                {
                    int colIndex = 0;
                    for (int cIndex = 0; cIndex < rows; cIndex++)
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
            int rows = matrix.RowCount;

            if (rowIndex >= rows)
            {
                throw new InvalidOperationException("Given rowIndex is out of range of matrix in function RemoveRow.");
            }

            if (matrix.ColumnCount > 1)
            {
                throw new InvalidOperationException("Given matrix in function RemoveRow must have no more than 1 column.");
            }

            Matrix reducedMatrix= new Matrix(rows-1,1,0.0);
            int rowCount = 0;

            for (int rIndex = 0; rIndex < rows; rIndex++)
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
        /// <param name="forSTAC">true/false:  Whether or not the intended use is STAC.</param>
        /// <returns>An [n x 1] Matrix containing the differences between the two features.</returns>
        public static Matrix Differences<T,U>(T feature1, U feature2, bool driftTime, bool forSTAC) where T: Feature where U: Feature
        {
            int dimension = 2;
            if (driftTime)
                dimension++;
            Matrix differences = new Matrix(dimension, 1, 0.0);

            if (feature1.MassMonoisotopicAligned != double.NaN)
                differences[0, 0] = MathUtilities.MassDifferenceInPPM(feature1.MassMonoisotopicAligned, feature2.MassMonoisotopic);
            else
                differences[0, 0] = MathUtilities.MassDifferenceInPPM(feature1.MassMonoisotopic, feature2.MassMonoisotopic);

            if (feature1.NETAligned != double.NaN)
                differences[1, 0] = feature1.NETAligned - feature2.NET;
            else
                differences[1, 0] = feature1.NET - feature2.NET;

            if (driftTime)
                differences[2, 0] = feature1.DriftTime - feature2.DriftTime;

            return differences;
        }
        /// <summary>
        /// Find the differences between a feature and a massTag.
        /// </summary>
        /// <typeparam name="T">Feature or derived class.</typeparam>
        /// <param name="feature">Observed feature to be compared to MassTag.</param>
        /// <param name="massTag">MassTag to be compared to.</param>
        /// <param name="driftTime">true/false:  Whether or not to include the drift time difference.</param>
        /// <param name="forSTAC">true/false:  Whether or not to separate predicted drift time.</param>
        /// <returns>An [n x 1] Matrix containing the differences between the two features.</returns>
        public static Matrix Differences<T>(T feature, MassTag massTag, bool driftTime, bool forSTAC) where T : Feature
        {
            int dimension = 2;
            if (driftTime)
                dimension++;
            if (driftTime && forSTAC)
                dimension++;
            Matrix differences = new Matrix(dimension, 1, 0.0);

            if (feature.MassMonoisotopicAligned != double.NaN)
                differences[0, 0] = MathUtilities.MassDifferenceInPPM(feature.MassMonoisotopicAligned, massTag.MassMonoisotopic);
            else
                differences[0, 0] = MathUtilities.MassDifferenceInPPM(feature.MassMonoisotopic, massTag.MassMonoisotopic);

            if (feature.NETAligned != double.NaN)
                differences[1, 0] = feature.NETAligned - massTag.NET;
            else
                differences[1, 0] = feature.NET - massTag.NET;

            if (driftTime)
            {
                if (forSTAC)
                {
                    if (massTag.DriftTime != 0)
                        differences[2, 0] = feature.DriftTime - massTag.DriftTime;
                    else
                        differences[3, 0] = feature.DriftTime - massTag.DriftTimePredicted;
                }
                else
                {
                    if (massTag.DriftTime != 0)
                        differences[2, 0] = feature.DriftTime - massTag.DriftTime;
                    else
                        differences[2, 0] = feature.DriftTime - massTag.DriftTimePredicted;
                }
            }

            return differences;
        }
        /// <summary>
        /// Find the differences between any two features.
        /// </summary>
        /// <typeparam name="T">Feature or derived class.</typeparam>
        /// <typeparam name="U">Feature or derived class.</typeparam>
        /// <param name="feature1">Observed feature to be compared to other feature.</param>
        /// <param name="feature2">Feature (MassTag) to be compared to.</param>
        /// <param name="driftTime">true/false:  Whether or not to include the drift time difference.</param>
        /// <returns>An [n x 1] Matrix containing the differences between the two features.</returns>
        public static Matrix Differences<T, U>(T feature1, U feature2, bool driftTime) where T: Feature where U: Feature
        {
            return Differences<T, U>(feature1, feature2, driftTime, false);
        }
        /// <summary>
        /// Find the differences between any two features.  Automatically detects the presence of drift time in the observed feature.
        /// </summary>
        /// <typeparam name="T">Feature or derived class.</typeparam>
        /// <typeparam name="U">Feature or derived class.</typeparam>
        /// <param name="feature1">Observed feature to be compared to other feature.</param>
        /// <param name="feature2">Feature (MassTag) to be compared to.</param>
        /// <returns>An [n x 1] Matrix containing the differences between the two features.</returns>
        public static Matrix Differences<T, U>(T feature1, U feature2) where T: Feature where U: Feature
        {
            bool useDriftTime = (feature1.DriftTime != 0);
            return Differences<T, U>(feature1, feature2, useDriftTime);
        }
        #endregion
    }
}
