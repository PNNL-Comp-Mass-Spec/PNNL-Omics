using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.Alignment
{
	public static class LinearEquationCalculator
	{
		public static LinearEquation CalculateLinearEquation(List<XYData> xyDataList)
		{
			LinearEquation linearEquation = new LinearEquation();

			double sumX = 0;
			double sumY = 0;
			double sumXTimesY = 0;
			double sumXSquared = 0;
			double sumYSquared = 0;
			double numPoints = xyDataList.Count;

			foreach (XYData xyData in xyDataList)
			{
				double xValue = xyData.X;
				double yValue = xyData.Y;

				sumX += xValue;
				sumY += yValue;
				sumXTimesY += (xValue * yValue);
				sumXSquared += (xValue * xValue);
				sumYSquared += (yValue * yValue);
			}

			double intercept = ((sumY * sumXSquared) - (sumX * sumXTimesY)) / ((numPoints * sumXSquared) - (sumX * sumX));
			double slope = ((numPoints * sumXTimesY) - (sumX * sumY)) / ((numPoints * sumXSquared) - (sumX * sumX));

			linearEquation.Intercept = intercept;
			linearEquation.Slope = slope;

			return linearEquation;
		}

		public static double CalculateNewValue(double xValue, LinearEquation linearEquation)
		{
			return (linearEquation.Slope * xValue) + linearEquation.Intercept;
		}
	}
}
