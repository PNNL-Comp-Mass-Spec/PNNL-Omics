using System;
using System.Collections.Generic;

//TODO: Brian add file header...

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
        //TODO: Make this return a list of new clusters, update method comment.
        //TODO: Make two methods, overloaded, make one that doesnt take a list of clusters.
        void Cluster(List<T> data, List<U> clusters);
    }
}
