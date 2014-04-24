using System;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Statistics
{
    public class Histogram
    {
        public static void CreateHistogram(List<double> inputValues, ref List<double> bins,
                                            ref List<int> frequency, double valStep)
        {
            bins.Clear();
            frequency.Clear();
            int numPts = inputValues.Count;

            // Tried to pass an empty list to the histogram creator
            if (numPts == 0)
            {
                return;
            }

            double minVal = inputValues[0];
            double maxVal = inputValues[0];

            foreach (double val in inputValues)
            {
                if (val < minVal)
                {
                    minVal = val;
                }
                if (val > maxVal)
                {
                    maxVal = val;
                }
            }

            //Only one unique value in the input values
            if (System.Math.Abs(minVal - maxVal) < double.Epsilon)
            {
                bins.Add(minVal);
                frequency.Add(1);
                return;
            }

            var numBins = (int)Math.Floor((maxVal - minVal) / valStep);

            double binVal = minVal;
            for (int i = 0; i < numBins; i++)
            {
                bins.Add(binVal);
                frequency.Add(0);
                binVal += valStep;
            }

            for (int i = 0; i < numPts; i++)
            {
                var binIndex = (int)Math.Floor((inputValues[i] - minVal) / valStep);
                if (binIndex >= numBins)
                {
                    binIndex = numBins - 1;
                }
                frequency[binIndex]++;
            }
        }
    }
}
