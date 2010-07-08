using System;
using System.Collections.Generic;
using System.Text;
using PNNLOmics.Algorithms.ConformationDetection.Data;

namespace PNNLOmics.Algorithms.ConformationDetection.Util
{
	public static class MathUtil
	{
		public static double Max(List<double> doubleList)
		{
			int index;

			return Max(doubleList, out index);
		}

		public static double Max(List<double> doubleList, out int listIndex)
		{
			listIndex = -1;
			if (doubleList.Count == 0)
			{
				return double.NaN;
			}

			double max = doubleList[0];
			listIndex = 0;
			for (int i = 0; i < doubleList.Count; i++)
			{
				if (doubleList[i] > max)
				{
					max = doubleList[i];
					listIndex = i;
				}
			}

			return max;
		}

		public static double Median(List<double> X)
		{
			throw new NotImplementedException();
		}

		public static double Min(List<double> doubleList)
		{
			int index;

			return Min(doubleList, out index);
		}

		public static double Min(List<double> doubleList, out int listIndex)
		{
			listIndex = -1;
			if (doubleList.Count == 0)
			{
				return double.NaN;
			}

			double min = doubleList[0];
			listIndex = 0;
			for (int i = 0; i < doubleList.Count; i++)
			{
				if (doubleList[i] < min)
				{
					min = doubleList[i];
					listIndex = i;
				}
			}

			return min;
		}

		public static double Mean(List<double> doubleList)
		{
			if (doubleList.Count == 0)
			{
				return double.NaN;
			}

			double mean = 0.0;

			for (int i = 0; i < doubleList.Count; i++)
			{
				mean += doubleList[i];
			}

			mean = mean / doubleList.Count;

			return mean;
		}

		public static double Var(List<double> doubleList)
		{
			if (doubleList.Count == 0)
			{
				return double.NaN;
			}
			if (doubleList.Count == 1)
			{
				return 0.0;
			}

			double mean = Mean(doubleList);
			double var = 0.0;

			for (int i = 0; i < doubleList.Count; i++)
			{
				var += (doubleList[i] - mean) * (doubleList[i] - mean);
			}

			var = var / (doubleList.Count - 1);

			return var;
		}

		public static double Std(List<double> doubleList)
		{
			if (doubleList.Count == 0)
			{
				return double.NaN;
			}
			if (doubleList.Count == 1)
			{
				return 0.0;
			}

			return Math.Sqrt(Var(doubleList));
		}

		public static double Quantile(List<double> doubleList, double p)
		{
			if (doubleList.Count == 0)
			{
				return double.NaN;
			}

			double quantilePosition = p * doubleList.Count + 0.5;
			int quantileIndex = (int)quantilePosition;

			if (quantileIndex < 1)
			{
				return doubleList[0];
			}
			if (quantileIndex > doubleList.Count)
			{
				return doubleList[doubleList.Count - 1];
			}

			if (quantileIndex - quantilePosition == 0)
			{
				return doubleList[quantileIndex - 1];
			}
			else
			{
				return doubleList[quantileIndex - 1] + (quantilePosition - quantileIndex) * (doubleList[quantileIndex] - doubleList[quantileIndex - 1]);
			}
		}

		// Compute Pearson's correlation coefficient between obeserved data and trained data
		public static double PearsonCorr(List<double> xValueList, List<double> yValueList)
		{
			if (xValueList.Count < 2 || xValueList.Count != yValueList.Count)
			{
				return double.NaN;
			}

			double corr = 0.0;
			double xMean = Mean(xValueList);
			double yMean = Mean(yValueList);
			double xStd = Std(xValueList);
			double yStd = Std(yValueList);

			if (xStd == 0 || yStd == 0)
			{
				return double.NaN;
			}

			for (int i = 0; i < xValueList.Count; i++)
			{
				corr += (xValueList[i] - xMean) * (yValueList[i] - yMean);
			}

			return corr / ((xValueList.Count - 1) * xStd * yStd);
		}

		// Compute Pearson's correlation coefficient between obeserved data and trained data
		public static double MeanSquareError(List<double> xValueList, List<double> yValueList)
		{
			if (xValueList.Count != yValueList.Count)
			{
				return double.NaN;
			}

			double mse = 0.0;

			for (int i = 0; i < xValueList.Count; i++)
			{
				mse += Math.Pow((xValueList[i] - yValueList[i]), 2) / yValueList[i];
			}

			return mse;// / xValueList.Count;
		}

		// Compute Spearman's rank correlation coefficient between obeserved data and trained data
		public static double SpearmanCorr(List<double> xValueList, List<double> yValueList)
		{
			if ((xValueList.Count < 2) || (yValueList.Count != xValueList.Count))
			{
				return double.NaN;
			}

			double sumDiffSquare = 0.0;

			List<double> xRankList = AverageRank(xValueList);
			List<double> yRankList = AverageRank(yValueList);

			for (int i = 0; i < xValueList.Count; i++)
			{
				sumDiffSquare += Math.Pow(xRankList[i] - yRankList[i], 2);
			}

			return 1 - 6 * sumDiffSquare / (xValueList.Count * (Math.Pow(xValueList.Count, 2) - 1));
		}

		// Compute average rank
		public static List<double> AverageRank(List<double> xValueList)
		{
			List<double> averageRank = new List<double>();
			List<Pair<double, int>> xValueIndexPairList = new List<Pair<double, int>>();

			// Try to keep track the original index of sorted values
			for (int i = 0; i < xValueList.Count; i++)
			{
				xValueIndexPairList.Add(new Pair<double, int>(xValueList[i], i));
			}
			xValueIndexPairList.Sort(Pair<double, int>.PairFirstComparison);

			for (int i = 0; i < xValueList.Count; i++)
			{
				averageRank.Add(-1.0);
			}

			// Need to find duplicate values 
			int duplicatePositionStart = 0;
			int duplicatePositionEnd = 0;
			double tmpAverageRank = 0.0;

			for (int i = 1; i < xValueIndexPairList.Count; i++)
			{
				if (xValueIndexPairList[i].First == xValueIndexPairList[i - 1].First)
				{
					duplicatePositionEnd++;
				}
				else
				{
					// All duplicates are found, compute value and update (duplicatePositionStart,duplicatePositionEnd)
					tmpAverageRank = (duplicatePositionStart + duplicatePositionEnd) / 2.0 + 1;

					for (int j = duplicatePositionStart; j <= duplicatePositionEnd; j++)
					{
						averageRank[xValueIndexPairList[j].Second] = tmpAverageRank;
					}

					duplicatePositionStart = i;
					duplicatePositionEnd = i;
				}
			}
			// Update the last subgroup of values close to the end
			tmpAverageRank = (duplicatePositionStart + duplicatePositionEnd) / 2;

			for (int j = duplicatePositionStart; j <= duplicatePositionEnd; j++)
			{
				averageRank[xValueIndexPairList[j].Second] = tmpAverageRank + 1;
			}

			return averageRank;
		}

		public static List<double> BoxSmooth(List<double> xValueList, List<double> yValueList)
		{
			if (xValueList.Count != yValueList.Count)
			{
				return null;
			}

			List<double> returnList = new List<double>();
			returnList.Add(yValueList[0]);

			int arrayLength = xValueList.Count;
			for (int i = 1; i < arrayLength - 1; i++)
			{
				double sum = yValueList[i - 1] + yValueList[i] + yValueList[i + 1];
				returnList.Add(sum);
			}
			returnList.Add(yValueList[arrayLength - 1]);
			return returnList;
		}
	}
}
