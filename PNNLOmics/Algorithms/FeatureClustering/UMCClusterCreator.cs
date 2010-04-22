using System;
using System.Collections.Generic;

using System.Linq;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// Clusters UMC's (LC-MS Features, LC-IMS-MS Features) into UMC Clusters.
    /// </summary>
    public class UMCClusterCreator
    {
        public UMCClusterCreator()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="features"></param>
        public IList<UMCCluster> ClusterFeatures(IList<UMC> features)
        {
            SortedList<UMC, UMC> x;            
            return null;
        }
    }

    public class FeatureMonoisotopicMassComparer : IComparer<Feature>
    {
        #region IComparer<Feature> Members

        public int Compare(Feature x, Feature y)
        {
           // return x.MassMonoisotopic
            return 0;
        }

        #endregion
    }
}
