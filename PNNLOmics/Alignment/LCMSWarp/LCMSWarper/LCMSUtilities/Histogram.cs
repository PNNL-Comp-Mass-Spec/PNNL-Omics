using System;
using System.Collections.Generic;


namespace PNNLOmics.Alignment.LCMSWarp.LCMSWarper.LCMSUtilities
{
    class Histogram
    {
        public static void CreateHistogram(List<double> InputValues, ref List<double> Bins,
                                            ref List<int> frequency, double ValStep)
        {
            Bins.Clear();
            frequency.Clear();
            int num_pts = InputValues.Count;

            // Tried to pass an empty list to the histogram creator
            if (num_pts == 0)
            {
                return;
            }

            double minVal = InputValues[0];
            double maxVal = InputValues[0];

            foreach (double val in InputValues)
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
            if (minVal == maxVal)
            {
                Bins.Add(minVal);
                frequency.Add(1);
                return;
            }

            int num_bins = (int)Math.Floor((maxVal - minVal) / ValStep);

            double bin_val = minVal;
            for (int i = 0; i < num_bins; i++)
            {
                Bins.Add(bin_val);
                frequency.Add(0);
                bin_val += ValStep;
            }

            for (int i = 0; i < num_pts; i++)
            {
                int bin_index = (int)Math.Floor((InputValues[i] - minVal) / ValStep);
                if (bin_index >= num_bins)
                {
                    bin_index = num_bins - 1;
                }
                frequency[bin_index]++;
            }
        }
    }
}
