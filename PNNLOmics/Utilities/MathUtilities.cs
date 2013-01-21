using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using MathNet.Numerics;
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
			double covarianceMatrixDeterminant = covarianceMatrix.Determinant();

			if (covarianceMatrixDeterminant != 0)
			{
				int numberOfRows = covarianceMatrix.RowCount;
				Matrix xMinusMean = xVector - meanVector;
				Matrix xMinusMeanPrime = xMinusMean.Clone();
				xMinusMeanPrime.Transpose();
				Matrix covarianceInverseMatrix = covarianceMatrix.Inverse();
				Matrix exponent = xMinusMeanPrime * covarianceInverseMatrix * xMinusMean;
				double denominator = Math.Sqrt(Math.Pow((2 * Math.PI), numberOfRows) * Math.Abs(covarianceMatrixDeterminant));
				return Math.Exp(-0.5 * exponent[0, 0]) / denominator;
			}
			else
			{
				return 0.0;
			}
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

                XYData currentBin = new XYData(0, 0);
                currentBin.X = Convert.ToSingle(Math.Round(minValue - (binRange - range) / 2, 2));
                for (int i = 0; i < nValues; i++)
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

		/// <summary>
		/// Convert value to a string with 5 digits of precision
		/// </summary>
		/// <param name="value">Number to convert to text</param>
		/// <returns>Number as text; numbers larger than 1000000 or smaller than 0.000001 will be in scientific notation</returns>
		public static string ValueToString(double value)
		{
			return ValueToString(value, 5, 1000000);
		}

		/// <summary>
		/// Convert value to a string with the specified digits of precision
		/// </summary>
		/// <param name="value">Number to convert to text</param>
		/// <param name="digitsOfPrecision">Total digits of precision (before and after the decimal point)</param>
		/// <returns>Number as text; numbers larger than 1000000 or smaller than 0.000001 will be in scientific notation</returns>
		public static string ValueToString(double value, int digitsOfPrecision)
		{
			return ValueToString(value, digitsOfPrecision, 1000000);
		}

		/// <summary>
		/// Convert value to a string with the specified digits of precision and customized scientific notation threshold
		/// </summary>
		/// <param name="value">Number to convert to text</param>
		/// <param name="digitsOfPrecision">Total digits of precision (before and after the decimal point)</param>
		/// <param name="scientificNotationThreshold">Values larger than this threshold (positive or negative) will be converted to scientific notation</param>
		/// <returns>Number as text</returns>
		public static string ValueToString(double value, int digitsOfPrecision, double scientificNotationThreshold)
		{
			string strValue;
			string strMantissa;

			if (digitsOfPrecision < 1)
				digitsOfPrecision = 1;

			scientificNotationThreshold = Math.Abs(scientificNotationThreshold);
			if (scientificNotationThreshold < 10)
				scientificNotationThreshold = 10;

			try
			{
				strMantissa = "0." + new string('0', Math.Max(digitsOfPrecision - 1, 1)) + "E+00";

				if (value == 0)
				{
					strValue = "0";
				}
				else if (Math.Abs(value) <= 1 / scientificNotationThreshold ||
					     Math.Abs(value) >= scientificNotationThreshold)
				{
					// Use scientific notation
					strValue = value.ToString(strMantissa);
				}
				else if (Math.Abs(value) < 1)
				{
					int intDigitsAfterDecimal = (int)Math.Floor(-Math.Log10(Math.Abs(value))) + digitsOfPrecision;
					string strFormatString = "0." + new string('0', intDigitsAfterDecimal);

					strValue = value.ToString(strFormatString);
					if (double.Parse(strValue) == 0)
					{
						// Value was converted to 0; use scientific notation
						strValue = value.ToString(strMantissa);
					}
					else
					{
						strValue = strValue.TrimEnd('0').TrimEnd('.');
					}
				}
				else
				{
					int intDigitsAfterDecimal = digitsOfPrecision - (int)Math.Ceiling(Math.Log10(Math.Abs(value)));

					if (intDigitsAfterDecimal > 0)
					{
						strValue = value.ToString("0." + new string('0', intDigitsAfterDecimal));
						strValue = strValue.TrimEnd('0').TrimEnd('.');
					}
					else
					{
						strValue = value.ToString("0");
					}
				}

				if (digitsOfPrecision > 1)
				{
					// Look for numbers with scientific notation
					// If there is a series of zeroes before the E then remove them
					// For example, change 1.5000E-43 to 1.5E-43
					if (m_scientificNotationTrim.IsMatch(strValue))
					{
						strValue = m_scientificNotationTrim.Replace(strValue, "E");

						// The number may now look like 1.E+43
						// If it does, then re-insert a zero after the decimal point
						strValue = strValue.Replace(".E", ".0E");
					}
				}

				return strValue;

			}
			catch (System.Exception ex)
			{
				Console.WriteLine("Error in ValueToString: " + ex.Message);
				return value.ToString();
			}

		}

        #endregion
    }
}