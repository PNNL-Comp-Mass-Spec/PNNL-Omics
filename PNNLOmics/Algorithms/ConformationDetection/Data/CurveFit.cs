using System;
using System.Collections.Generic;
using System.Text;
using Mapack;

namespace PNNLOmics.Algorithms.ConformationDetection.Data
{
	public static class CurveFit
	{
		public static double[] PolynomialFit(List<double> xValueList, List<double> yValueList, int nOrder)
		{
			try
			{
				if (xValueList.Count != yValueList.Count || xValueList.Count == 0)
				{
					return null;
				}

				if (xValueList.Count <= nOrder)
				{
					// Not enough data pairs for fitting.
					return null;
				}

				// Make sure that X is in ascending order.
				int numPoints = xValueList.Count;
				double[] polyCoefficients = new double[nOrder + 1];

				double[] weight = new double[numPoints];
				for (int i = 0; i < numPoints; i++)
				{
					weight[i] = 1.0;
				}

				// Weighted powers of X 
				Matrix weightedXValues = new Matrix(nOrder + 1, numPoints);
				for (int i = 0; i < numPoints; i++)
				{
					weightedXValues[0, i] = weight[i];
					for (int j = 1; j < (nOrder + 1); j++)
					{
						weightedXValues[j, i] = weightedXValues[j - 1, i] * xValueList[i];
					}
				}

				// Weighted Y
				Matrix weightedYValues = new Matrix(numPoints, 1);
				for (int i = 0; i < numPoints; i++)
				{
					weightedYValues[i, 0] = weight[i] * yValueList[i];
				}

				Matrix transposedWeightedXValues = weightedXValues.Transpose();
				Matrix outerProductWeightedXValues = weightedXValues * transposedWeightedXValues;
				Matrix invertOuterProductWeightedXValues = outerProductWeightedXValues.Inverse;
				if (invertOuterProductWeightedXValues == null)
				{
					return null;
				}

				Matrix B = invertOuterProductWeightedXValues * weightedXValues * weightedYValues;

				// First row fitted y; second row fitted residue. 
				Matrix fitTable = new Matrix(2, numPoints);

				// calculate the least squares y values 
				double yValuesFit = 0;
				double xValuesPow = 0;
				double meanSquaredError = 0.0;

				for (int i = 0; i < numPoints; i++)
				{
					polyCoefficients[0] = B[0, 0];
					yValuesFit = B[0, 0];
					xValuesPow = xValueList[i];
					for (int j = 1; j <= nOrder; j++)
					{
						polyCoefficients[j] = B[j, 0];
						yValuesFit += B[j, 0] * xValuesPow;
						xValuesPow = xValuesPow * xValueList[i];
					}
					fitTable[0, i] = yValuesFit;
					fitTable[1, i] = yValueList[i] - yValuesFit;
					meanSquaredError += yValueList[i] - yValuesFit;
				}

				return polyCoefficients;

			}
			catch (Exception e)
			{
				return null;
			}
		}

		public static double[] GaussianFit(List<double> xValueList, List<double> yValueList, double threshold)
		{
			if (xValueList.Count != yValueList.Count)
			{
				return null;
			}

			// This is for an unimplemented feature. For now, it is set to 0 so that it will not effect anything.
			double yValueMax = 0;

			List<double> xValueFilteredList = new List<double>();
			List<double> yValueFilteredList = new List<double>();

			for (int i = 0; i < xValueList.Count; i++)
			{
				if (yValueList[i] > yValueMax * threshold)
				{
					xValueFilteredList.Add(xValueList[i]);
					yValueFilteredList.Add(Math.Log(yValueList[i]));
				}
			}

			// The order for Guassian shape is 2.
			double[] polyCoefficients = PolynomialFit(xValueFilteredList, yValueFilteredList, 2);

			// Function y = amount * exp( -(x-mu)^2 / (2*sigma^2) ).
			if (polyCoefficients != null)
			{
				double sigma = Math.Sqrt(-1 / (2 * polyCoefficients[2]));
				double mu = -polyCoefficients[1] / (2 * polyCoefficients[2]);
				double amount = Math.Exp(polyCoefficients[0] + mu * mu / (2 * sigma * sigma));

				double[] gaussianParameters = new double[3] { mu, sigma, amount };
				return gaussianParameters;
			}
			else
			{
				return null;
			}
		}

		public static List<double> MovingAverage(List<double> xValueList, int period)
		{
			int numPoints = xValueList.Count;

			if (period > numPoints)
			{
				return xValueList;
			}

			List<double> newList = new List<double>();

			// End effect of moving average: int/int
			int numEndPoints = period / 2;
			for (int i = 0; i < numEndPoints; i++)
			{
				newList.Add(xValueList[i]);
			}

			for (int i = 0; i < numPoints - period + 1; i++)
			{
				double sum = 0;
				int counter = 0;
				for (int j = 0; j < period; j++)
				{
					sum += xValueList[i + j];
					counter++;
				}
				newList.Add(sum / counter);
			}

			// End effect of moving average
			for (int i = numPoints - numEndPoints; i < numPoints; i++)
			{
				newList.Add(xValueList[i]);
			}

			return newList;
		}

		public static List<double> KDESmooth(List<double> xValueList, List<double> yValueList, double bandwidth)
		{
			int numPoints = xValueList.Count;
			int numBins = numPoints;

			List<double> newYList = new List<double>();

			foreach (double point in xValueList)
			{
				double sumWInv = 0;
				double sumXoW = 0;
				double sumX2oW = 0;
				double sumYoW = 0;
				double sumXYoW = 0;
				for (int j = 0; j < numPoints; j++)
				{
					double x = xValueList[j];
					double y = yValueList[j];
					double standardized = Math.Abs(x - point) / bandwidth;
					double w = 0;
					if (standardized < 6)
					{
						w = (2 * Math.Sqrt(2 * Math.PI) * Math.Exp(-2 * standardized * standardized));
						sumWInv += 1 / w;
					}
					sumXoW += x * w;
					sumX2oW += x * x * w;
					sumYoW += y * w;
					sumXYoW += x * y * w;
				}

				double intercept = 1 / (sumWInv * sumX2oW - sumXoW * sumXoW) * (sumX2oW * sumYoW - sumXoW * sumXYoW);
				double slope = 1 / (sumWInv * sumX2oW - sumXoW * sumXoW) * (sumWInv * sumXYoW - sumXoW * sumYoW);
				newYList.Add(intercept + slope * point);
			}

			return newYList;
		}
	}
}
