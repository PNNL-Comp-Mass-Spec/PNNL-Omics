using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    public interface IClusterReprocessor<T, U> 
        where T : FeatureLight, IChildFeature<U>, new()
        where U : FeatureLight, IFeatureCluster<T>, new()
    {

        /// <summary>
        /// Reprocesses clusters and returns a list of new clusters.
        /// </summary>
        /// <param name="clusters"></param>
        /// <returns></returns>
        List<U> ProcessClusters(List<U> clusters);
    }
}
