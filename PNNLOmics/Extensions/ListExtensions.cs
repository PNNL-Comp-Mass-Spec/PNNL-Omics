using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Generic.Solvers.Status;
using PNNLOmics.Data.Features;
using PNNLOmics.Data;

namespace PNNLOmics.Extensions
{
    public static class FeatureListExtensions
    {
        /// <summary>
        /// Determines the magnitude of the scan range beyond what is defined by the MSFeatures min and max scan based on percentage of total range. 
        /// </summary>
        /// <param name="features"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static int ExtendedScanRange(this List<MSFeatureLight> features, double percentage)
        {
            int max = features.Max(feature => feature.Scan);
            int min = features.Min(feature => feature.Scan);

            int delta = Math.Abs(max - min);

            return Convert.ToInt32(Convert.ToDouble(delta) * percentage);
        }

        public static int MaxScan<T>(this List<T> features) where T : MSFeatureLight
        {
            return features.Max(feature => feature.Scan);
        }
        public static int MinScan<T>(this List<T> features) where T : MSFeatureLight
        {
            return features.Min(feature => feature.Scan);
        }
        public static double MaxAbundance<T>(this List<T> features) where T : MSFeatureLight
        {
            return features.Max(feature => feature.Abundance);
        }
        public static double MinAbundance<T>(this List<T> features) where T : MSFeatureLight
        {
            return features.Min(feature => feature.Abundance);
        }
        public static double Median<T>(this List<T> features) where T : MSFeatureLight
        {
            features.Sort(delegate(T x, T y)
            {
                return x.Mz.CompareTo(y.Mz);
            }
            );
            int count = features.Count / 2;
            return features[count].Mz;
        }
    }

    public static class UMCLightExtensions
    {

        /// <summary>
        /// Creates a charge map for a given ms feature list.
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public static Dictionary<int, List<MSFeatureLight>> CreateChargeMap(this UMCLight feature)
        {
            var chargeMap = new Dictionary<int, List<MSFeatureLight>>();
            foreach (MSFeatureLight msFeature in feature.MSFeatures)
            {
                if (!chargeMap.ContainsKey(msFeature.ChargeState))
                {
                    chargeMap.Add(msFeature.ChargeState, new List<MSFeatureLight>());
                }
                chargeMap[msFeature.ChargeState].Add(msFeature);
            }

            var newChargeMap = new Dictionary<int, List<MSFeatureLight>>();            
            foreach (var charge in chargeMap.Keys)
            {
                var ordered       = chargeMap[charge].OrderBy(x => x.Scan);
                newChargeMap.Add(charge, ordered.ToList());                
            }
            return newChargeMap;
        }
    }


    public static class XYDataListExtensions
    {
        /// <summary>
        /// Finds the closest matching XY data value based on mz assumes list is sorted in ascending order.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static XYData FindByMZ(this List<XYData> data, double mz)
        {
            int i = 0;
            for(int j = 0; j < data.Count; j++)
            {
                if (data[j].X > mz)
                {
                    double diffI = Math.Abs(data[i].X - mz);
                    double diffJ = Math.Abs(data[j].X - mz);

                    if (diffI < diffJ)
                        return data[i];
                    return data[j];
                }
                i = j;                
            }
            return data[data.Count - 1];
        }
    }
}
