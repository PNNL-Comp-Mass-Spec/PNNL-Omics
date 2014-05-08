﻿using System;
using System.Collections.Generic;
using System.Linq;
using PNNLOmics.Data.Features;
using PNNLOmicsViz.Drawing;

namespace MultiAlignCore.Drawing
{
    public static class HistogramFactory
    {
        /// <summary>
        ///     Creates a charge state histogram from the features provided.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="features"></param>
        /// <returns></returns>
        public static PlotBase CreateChargeStateHistogram<T>(IEnumerable<T> features)
            where T : FeatureLight
        {
            // Map charge states.
            var chargeMap = new Dictionary<int, int>();
            foreach (T feature in features)
            {
                int charge = feature.ChargeState;
                if (!chargeMap.ContainsKey(charge))
                {
                    chargeMap.Add(charge, 0);
                }
                chargeMap[charge] = chargeMap[charge] + 1;
            }
            return new ChargeHistogramPlot(chargeMap, "Charge States");
        }

        /// <summary>
        ///     Creates a histogram plot from the data provided.  Assumes the bin is stored in the first column, count stored in
        ///     the second (0,1) respectively.
        /// </summary>
        /// <param name="histogram"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public static PlotBase CreateHistogram(double[,] histogram, string label)
        {
            var plotData = new Dictionary<double, int>();
            for (int i = 0; i < histogram.GetLength(0); i++)
            {
                plotData.Add(histogram[i, 0], Convert.ToInt32(histogram[i, 1]));
            }
            return CreateHistogram(plotData, label);
        }

        /// <summary>
        ///     Creates a histogram plot from the data provided.
        /// </summary>
        /// <param name="histogram"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public static PlotBase CreateHistogram(Dictionary<int, int> histogram, string label)
        {
            Dictionary<double, int> doubleGram = histogram.Keys.ToDictionary<int, double, int>(key => key,
                key => histogram[key]);

            return new HistogramPlot(doubleGram, label);
        }

        /// <summary>
        ///     Creates a histogram plot from the data provided.
        /// </summary>
        /// <param name="histogram"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public static PlotBase CreateHistogram(Dictionary<double, int> histogram, string label)
        {
            return new HistogramPlot(histogram, label);
        }
    }
}