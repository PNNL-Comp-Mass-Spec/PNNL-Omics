using System;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// Interface describing how to cluster 
    /// </summary>
    /// <typeparam name="T">Type to cluster data objects of.</typeparam>
    /// <typeparam name="U">Type of cluster output objects.</typeparam>
    public interface IClusterer<T, U>
    {
        /// <summary>
        /// Clusters the data objects provided in the list.
        /// </summary>
        /// <param name="data">Data to cluster.</param>
        /// <returns>List of cluster objects defined over input data.</returns>
        void Cluster(List<T> data, List<U> clusters);
    }
}
