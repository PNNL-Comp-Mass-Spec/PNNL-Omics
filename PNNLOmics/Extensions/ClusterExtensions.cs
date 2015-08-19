using System.Collections.Generic;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Extensions
{
    public static class ClusterExtensions
{
        /// <summary>
        /// Creates a charge map for a given ms feature list.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, int> CreateClusterSizeHistogram(this IEnumerable<UMCClusterLight> clusters)
        {
            var map = new Dictionary<int, int>();
            foreach (var cluster in clusters)
            {
                if (!map.ContainsKey(cluster.MemberCount))
                {
                    map.Add(cluster.MemberCount, 0);
                }
                map[cluster.MemberCount]++;
            }

            return map;
        }

        /// <summary>
        /// Creates a charge map for a given ms feature list.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, int> CreateClusterDatasetMemeberSizeHistogram(this IEnumerable<UMCClusterLight> clusters)
        {
            var map = new Dictionary<int, int>();
            foreach (var cluster in clusters)
            {
                if (!map.ContainsKey(cluster.DatasetMemberCount))
                {
                    map.Add(cluster.DatasetMemberCount, 0);
                }
                map[cluster.DatasetMemberCount]++;
            }

            return map;
        }


        public static Dictionary<int, int> BuildChargeStateHistogram(this IEnumerable<UMCClusterLight> clusters)
        {
            var chargeHistogram = new Dictionary<int, int>();
            for (var i = 1; i < 10; i++)
            {
                chargeHistogram.Add(i, 0);
            }
            foreach (var cluster in clusters)
            {
                foreach (var feature in cluster.Features)
                {
                    var chargeMap = feature.CreateChargeMap();
                    foreach (var chargeDouble in chargeMap.Keys)
                    {
                        if (!chargeHistogram.ContainsKey(chargeDouble))
                            chargeHistogram.Add(chargeDouble, 0);
                        chargeHistogram[chargeDouble] = chargeHistogram[chargeDouble] + 1;
                    }
                }
            }
            return chargeHistogram;
        }

        public static Dictionary<int, int> BuildChargeStateHistogram(this UMCClusterLight cluster)
        {
            var chargeHistogram = new Dictionary<int, int>();
            for (var i = 1; i < 10; i++)
            {
                chargeHistogram.Add(i, 0);
            }
            foreach (var feature in cluster.Features)
            {
                var chargeMap = feature.CreateChargeMap();
                foreach (var charge in chargeMap.Keys)
                {
                    if (!chargeHistogram.ContainsKey(charge))
                        chargeHistogram.Add(charge, 0);
                    chargeHistogram[charge] = chargeHistogram[charge] + 1;
                }
            }
            return chargeHistogram;
        }
    }
}