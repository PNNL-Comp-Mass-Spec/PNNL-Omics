using System.Collections.Generic;
using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data;

namespace PNNLOmics.Utilities
{
    static public class MathUtilities
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
        static public double MultivariateNormalDensity(Matrix xVector, Matrix meanVector, Matrix covarianceMatrix)
        {
            int numberOfRows = covarianceMatrix.RowCount;
            Matrix xMinusMean = xVector - meanVector;
            Matrix xMinusMeanPrime = xMinusMean.Clone();
            xMinusMeanPrime.Transpose();
            Matrix exponent = xMinusMeanPrime * covarianceMatrix.Inverse() * xMinusMean;
            double denominator = Math.Sqrt(Math.Pow((2 * Math.PI), numberOfRows) * Math.Abs(covarianceMatrix.Determinant()));
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
            if (binWidth > 0 && values.Count > 0)
            {
                values.Sort();
                int nValues = values.Count;
                double minValue = values[0];
                double maxValue = values[nValues];

                double range = maxValue - minValue;
                uint bins = (uint)Math.Ceiling(range / binWidth);
                double binRange = binWidth * bins;

                List<XYData> histogramValues = new List<XYData>(0);

                XYData currentBin = new XYData();
                currentBin.X = Math.Round(minValue - (binRange - range) / 2, 2);
                for (int i = 0; i < nValues; i++)
                {
                    if (values[i] <= (currentBin.X + binWidth))
                    {
                        currentBin.Y++;
                    }
                    else
                    {
                        currentBin.X += 0.5 * binWidth;
                        histogramValues.Add(currentBin);
                        currentBin = new XYData(currentBin.X + 0.5 * binWidth, 0);
                    }
                }
                histogramValues.Add(currentBin);

                return histogramValues;
            }
            else
            {
                throw new InvalidOperationException("Invalid parameters passed to function GetHistogramValues.");
            }
        }

        /// <summary>
        /// Finds the relative frequency of points in each bin of binWidth corresponding to a histogram of the values list.
        /// </summary>
        /// <param name="values">List of values to compute histogram counts for.</param>
        /// <param name="binWidth">Width of bins to use in computing histogram values.</param>
        /// <returns>List of XYData with X being midpoints of histogram bins and Y being relative frequency in the bin.</returns>
        static public List<XYData> GetRelativeFrequencyHistogramValues(List<double> values, double binWidth)
        {
            List<XYData> histogramValues = GetHistogramValues(values, binWidth);
            int nValues = values.Count;
            for (int i = 0; i < histogramValues.Count; i++)
            {
                histogramValues[i].Y /= nValues;
            }
            return histogramValues;
        }
        #endregion

        #region XYData manipulators
        /// <summary>
        /// Convert XYData to arrays to interact with other functions more easily.
        /// </summary>
        /// <param name="xyList">List of XYData values to be converted.</param>
        /// <param name="xArray">Array to be populated with X values.</param>
        /// <param name="yArray">Array to be populated with Y values.</param>
        static public void XYDataListToArrays(List<XYData> xyList, double[] xArray, double[] yArray)
        {
            if (xArray.Length == xyList.Count || yArray.Length == xyList.Count)
            {
                for (int i = 0; i < xyList.Count; i++)
                {
                    xArray[i] = xyList[i].X;
                    yArray[i] = xyList[i].Y;
                }
            }
            else
            {
                throw new InvalidOperationException("X and Y arrays must be same length as XYData list in function XYDataListToArrays.");
            }
        }
        #endregion

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