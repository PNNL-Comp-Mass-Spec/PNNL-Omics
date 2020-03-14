using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;
using PNNLOmics.Annotations;
using PNNLOmics.Data;

namespace PNNLOmics.Utilities
{
    public static class MathUtilities
    {
        #region Statistical distributions
        /// <summary>
        /// Finds the density of the n-variate normal distribution with mean meanVector and covariance structure covarianceMatrix
        /// at the value xVector.
        /// </summary>
        /// <param name="xVector">Value at which the density is to be evaluated.  [n x 1]</param>
        /// <param name="meanVector">Mean vector for the density.  [n x 1]</param>
        /// <param name="covarianceMatrix">Symmetric covariance matrix.  [n x n]</param>
        /// <returns>Double</returns>
        static public double MultivariateNormalDensity(DenseMatrix xVector, DenseMatrix meanVector, DenseMatrix covarianceMatrix)
        {
            var covarianceMatrixDeterminant = covarianceMatrix.Determinant();

            if (!(Math.Abs(covarianceMatrixDeterminant) > double.Epsilon)) return 0.0;
            var numberOfRows = covarianceMatrix.RowCount;
            var xMinusMean = xVector - meanVector;
            var xMinusMeanPrime = xMinusMean.Transpose();
            var covarianceInverseMatrix = covarianceMatrix.Inverse();

            var exponent = xMinusMeanPrime * covarianceInverseMatrix * xMinusMean;
            var denominator = Math.Sqrt(Math.Pow((2 * Math.PI), numberOfRows) * Math.Abs(covarianceMatrixDeterminant));
            return Math.Exp(-0.5 * exponent[0, 0]) / denominator;

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
            if (!(binWidth > 0) || values.Count <= 0)
                throw new InvalidOperationException("Invalid parameters passed to function GetHistogramValues.");

            values.Sort();
            var nValues = values.Count;
            var minValue = values[0];
            var maxValue = values[nValues];

            var range = maxValue - minValue;
            var bins = (uint)Math.Ceiling(range / binWidth);
            var binRange = binWidth * bins;

            var histogramValues = new List<XYData>(0);

            var currentBin = new XYData(0, 0) {X = Convert.ToSingle(Math.Round(minValue - (binRange - range)/2, 2))};
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

        /// <summary>
        /// Finds the relative frequency of points in each bin of binWidth corresponding to a histogram of the values list.
        /// </summary>
        /// <param name="values">List of values to compute histogram counts for.</param>
        /// <param name="binWidth">Width of bins to use in computing histogram values.</param>
        /// <returns>List of XYData with X being midpoints of histogram bins and Y being relative frequency in the bin.</returns>
        [UsedImplicitly]
        static public List<XYData> GetRelativeFrequencyHistogramValues(List<double> values, double binWidth)
        {
            var histogramValues = GetHistogramValues(values, binWidth);
            var nValues = values.Count;
            foreach (var t in histogramValues)
            {
                t.Y /= nValues;
            }
            return histogramValues;
        }
        #endregion

        /// <summary>
        /// Two dimensional expectation maximization
        /// </summary>
        /// <param name="x">Difference between alignee feature and baseline feature, X value (e.g. mass)</param>
        /// <param name="y">Difference between alignee feature and baseline feature, Y value (e.g. NET)</param>
        /// <param name="p">Probability of belonging to the normal distribution (output)</param>
        /// <param name="u">Probability density of false hits (output)</param>
        /// <param name="muX">Mean of X values (output)</param>
        /// <param name="muY">Mean of Y values (output)</param>
        /// <param name="stdX">Standard deviation of X values (output)</param>
        /// <param name="stdY">Standard deviation of Y values (output)</param>
        /// <remarks>Used by LCMSWarp for computation of likelihood</remarks>
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
                // (expectation step)
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

                // Calculate new estimates from maximization step
                // (maximization step)
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

        [UsedImplicitly]
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
        static public double MassDifferenceInPpm(double mass1, double mass2)
        {
            return ((mass1 - mass2) / mass2 * 1000000);
        }

        /// <summary>
        /// Convert value to a string with 5 digits of precision
        /// </summary>
        /// <param name="value">Number to convert to text</param>
        /// <returns>Number as text; numbers larger than 1000000 or smaller than 0.000001 will be in scientific notation</returns>
        public static string ValueToString(double value)
        {
            return StringUtilities.ValueToString(value, 5, 1000000);
        }

        /// <summary>
        /// Convert value to a string with the specified digits of precision
        /// </summary>
        /// <param name="value">Number to convert to text</param>
        /// <param name="digitsOfPrecision">Total digits of precision (before and after the decimal point)</param>
        /// <returns>Number as text; numbers larger than 1000000 or smaller than 0.000001 will be in scientific notation</returns>
        [Obsolete("Use StringUtilities.ValueToString")]
        public static string ValueToString(double value, int digitsOfPrecision)
        {
            return StringUtilities.ValueToString(value, (byte)digitsOfPrecision, 1000000);
        }

        /// <summary>
        /// Convert value to a string with the specified digits of precision and customized scientific notation threshold
        /// </summary>
        /// <param name="value">Number to convert to text</param>
        /// <param name="digitsOfPrecision">Total digits of precision (before and after the decimal point)</param>
        /// <param name="scientificNotationThreshold">Values larger than this threshold (positive or negative) will be converted to scientific notation</param>
        /// <returns>Number as text</returns>
        /// [Obsolete("Use StringUtilities.ValueToString")]
        public static string ValueToString(double value, int digitsOfPrecision, double scientificNotationThreshold)
        {
            return StringUtilities.ValueToString(value, (byte)digitsOfPrecision, scientificNotationThreshold);
        }

        #endregion

    }
}