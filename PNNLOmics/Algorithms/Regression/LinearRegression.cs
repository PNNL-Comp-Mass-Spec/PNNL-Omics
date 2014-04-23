using System.Collections.Generic;
using System.Linq;
using PNNLOmics.Algorithms.Alignment;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.Regression
{
    //TODO: Move to regression folder - and rename to linear regression

	public static class LinearRegression
	{
		public static LinearEquation CalculateLinearEquation(IEnumerable<XYData> xyDataList)
		{
			LinearEquation linearEquation = new LinearEquation();

			double sumX = 0;
			double sumY = 0;
			double sumXTimesY = 0;
			double sumXSquared = 0;
			double numPoints = xyDataList.Count();

			foreach (XYData xyData in xyDataList)
			{
				double xValue = xyData.X;
				double yValue = xyData.Y;

				sumX += xValue;
				sumY += yValue;
				sumXTimesY += (xValue * yValue);
				sumXSquared += (xValue * xValue);
			}

			double slope = ((numPoints * sumXTimesY) - (sumX * sumY)) / ((numPoints * sumXSquared) - (sumX * sumX));
			double intercept = ((sumY * sumXSquared) - (sumX * sumXTimesY)) / ((numPoints * sumXSquared) - (sumX * sumX));

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
