﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data;

namespace PNNLOmics.Utilities
{
    static public class MathUtilities
    {
		private const string SCIENTIFIC_NOTATION_CLEANUP_REGEX = "0+E";
		private static Regex m_scientificNotationTrim = new Regex(SCIENTIFIC_NOTATION_CLEANUP_REGEX, RegexOptions.Compiled);

        #region Statistical distributions
        /// <summary>
        /// Finds the density of the n-variate normal distribution with mean meanVector and covariance structure covarianceMatrix 
        /// at the value xVector.
        /// </summary>
        /// <param name="xVector">Value at which the density is to be evaluated.  [n x 1]</param>
        /// <param name="meanVector">Mean vector for the density.  [n x 1]</param>
        /// <param name="covarianceMatrix">Symmetric covariance matrix.  [n x n]</param>
        /// <returns>Double</returns>
        static public double MultivariateNormalDensity(Matrix xVector, Matrix meanVector, Matrix covarianceMatrix)
        {
			var covarianceMatrixDeterminant = covarianceMatrix.Determinant();

			if (covarianceMatrixDeterminant != 0)
			{
				var numberOfRows = covarianceMatrix.RowCount;
				var xMinusMean = xVector - meanVector;
				var xMinusMeanPrime = xMinusMean.Clone();
				xMinusMeanPrime.Transpose();
				var covarianceInverseMatrix = covarianceMatrix.Inverse();
				var exponent = xMinusMeanPrime * covarianceInverseMatrix * xMinusMean;
				var denominator = Math.Sqrt(Math.Pow((2 * Math.PI), numberOfRows) * Math.Abs(covarianceMatrixDeterminant));
				return Math.Exp(-0.5 * exponent[0, 0]) / denominator;
			}
            return 0.0;
        }
        #endregion

        #region Histogram functions
        /// <summary>
        /// Finds the number of points in each bin of binWidth corresponding to a histogram of the values list.
        /// </summary>
        /// <param name="values">List of values to compute histogram counts for.</param>
        /// <param name="binWidth">Width of bins to use in computing histogram values.</param>
        /// <returns>List of XYData with X being midpoints of histogram bins and Y being count in the bin.</returns>
        static public List<XYData> GetHistogramValues(List<double> values, double binWidth)
        {
            if (binWidth > 0 && values.Count > 0)
            {
                values.Sort();
                var nValues = values.Count;
                var minValue = values[0];
                var maxValue = values[nValues];

                var range = maxValue - minValue;
                var bins = (uint)Math.Ceiling(range / binWidth);
                var binRange = binWidth * bins;

                var histogramValues = new List<XYData>(0);

                var currentBin = new XYData(0, 0);
                currentBin.X = Convert.ToSingle(Math.Round(minValue - (binRange - range) / 2, 2));
                for (var i = 0; i < nValues; i++)
                {
                    if (values[i] <= (currentBin.X + binWidth))
                    {
                        currentBin.Y++;
                    }
                    else
                    {
                        currentBin.X += Convert.ToSingle(0.5 * binWidth);
                        histogramValues.Add(currentBin);
                        currentBin = new XYData(currentBin.X + Convert.ToSingle(0.5F * binWidth), 0);
                    }
                }
                histogramValues.Add(currentBin);

                return histogramValues;
            }
            throw new InvalidOperationException("Invalid parameters passed to function GetHistogramValues.");
        }

        /// <summary>
        /// Finds the relative frequency of points in each bin of binWidth corresponding to a histogram of the values list.
        /// </summary>
        /// <param name="values">List of values to compute histogram counts for.</param>
        /// <param name="binWidth">Width of bins to use in computing histogram values.</param>
        /// <returns>List of XYData with X being midpoints of histogram bins and Y being relative frequency in the bin.</returns>
        static public List<XYData> GetRelativeFrequencyHistogramValues(List<double> values, double binWidth)
        {
            var histogramValues = GetHistogramValues(values, binWidth);
            var nValues = values.Count;
            for (var i = 0; i < histogramValues.Count; i++)
            {
                histogramValues[i].Y /= nValues;
            }
            return histogramValues;
        }


        #endregion


        public static void TwoDem(List<double> x, List<double> y, out double p, out double u, out double muX,
                           out double muY, out double stdX, out double stdY)
        {
            const int numIterations = 40;
            var numPoints = x.Count;
            var pVals = new double[2, numPoints];

            double minX = x[0], maxX = x[0];
            double minY = y[0], maxY = y[0];

            for (var pointNumber = 0; pointNumber < numPoints; pointNumber++)
            {
                if (x[pointNumber] < minX)
                {
                    minX = x[pointNumber];
                }
                if (x[pointNumber] > maxX)
                {
                    maxX = x[pointNumber];
                }
                if (y[pointNumber] < minY)
                {
                    minY = y[pointNumber];
                }
                if (y[pointNumber] > maxY)
                {
                    maxY = y[pointNumber];
                }
            }

            u = 1.0 / ((maxX - minX) * (maxY - minY));
            p = 0.5;

            CalcMeanAndStd(x, out muX, out stdX);
            stdX = stdX / 3.0;
            CalcMeanAndStd(y, out muY, out stdY);
            stdY = stdY / 3.0;

            for (var iterNum = 0; iterNum < numIterations; iterNum++)
            {
                // Calculate current probability assignments
                for (var pointNum = 0; pointNum < numPoints; pointNum++)
                {
                    var xDiff = (x[pointNum] - muX) / stdX;
                    var yDiff = (y[pointNum] - muY) / stdY;
                    pVals[0, pointNum] = p * Math.Exp(-0.5 * (xDiff * xDiff + yDiff * yDiff)) / (2 * Math.PI * stdX * stdY);
                    pVals[1, pointNum] = (1 - p) * u;
                    var sum = pVals[0, pointNum] + pVals[1, pointNum];
                    pVals[0, pointNum] = pVals[0, pointNum] / sum;
                    pVals[1, pointNum] = pVals[1, pointNum] / sum;
                }

                // Calculates new estimates from maximization step
                double pNumerator = 0;
                double muXNumerator = 0;
                double muYNumerator = 0;
                double sigmaXNumerator = 0;
                double sigmaYNumerator = 0;

                double pDenominator = 0;
                double denominator = 0;

                for (var pointNum = 0; pointNum < numPoints; pointNum++)
                {
                    pNumerator = pNumerator + pVals[0, pointNum];
                    pDenominator = pDenominator + (pVals[0, pointNum] + pVals[1, pointNum]);

                    var xDiff = (x[pointNum] - muX);
                    muXNumerator = muXNumerator + pVals[0, pointNum] * x[pointNum];
                    sigmaXNumerator = sigmaXNumerator + pVals[0, pointNum] * xDiff * xDiff;

                    var yDiff = (y[pointNum] - muY);
                    muYNumerator = muYNumerator + pVals[0, pointNum] * y[pointNum];
                    sigmaYNumerator = sigmaYNumerator + pVals[0, pointNum] * yDiff * yDiff;

                    denominator = denominator + pVals[0, pointNum];
                }

                muX = muXNumerator / denominator;
                muY = muYNumerator / denominator;
                stdX = Math.Sqrt(sigmaXNumerator / denominator);
                stdY = Math.Sqrt(sigmaYNumerator / denominator);
                p = pNumerator / pDenominator;
            }
        }

        public static void CalcMeanAndStd(List<double> values, out double mean, out double stdev)
        {
            var numPoints = values.Count;
            double sumSquare = 0;
            double sum = 0;
            for (var pointNum = 0; pointNum < numPoints; pointNum++)
            {
                var val = values[pointNum];
                sum = sum + val;
                sumSquare = sumSquare + (val * val);
            }
            mean = sum / numPoints;
            stdev = Math.Sqrt((numPoints * sumSquare - sum * sum)) / (Math.Sqrt(numPoints) * Math.Sqrt(numPoints - 1));
        }

        #region Helper functions
        /// <summary>
        /// Convert the difference between two masses to a difference in parts per million (PPM).
        /// </summary>
        /// <param name="mass1">First mass.  (Aligned mass of observedFeature)</param>
        /// <param name="mass2">Second mass.  (Aligned mass of targetFeature)</param>
        /// <returns></returns>
        static public double MassDifferenceInPPM(double mass1, double mass2)
        {
            return ((mass1 - mass2) / mass2 * 1000000);
        }
        #endregion
    }
}