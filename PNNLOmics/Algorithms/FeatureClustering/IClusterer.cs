/*////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * 
 * Name:    IClusterer Interface 
 * File:    IClsuterer.cs
 * Author:  Brian LaMarche 
 * Purpose: Interface for clustering UMC data.
 * Date:    5-19-2010
 * Revisions:
 *          5-19-2010 - BLL - Created interface.
 ////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
using System;
using System.Collections.Generic;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// Interface describing how to cluster 
    /// </summary>
    /// <typeparam name="T">Type to cluster data objects of.</typeparam>
    /// <typeparam name="U">Type of cluster output objects.</typeparam>
    public interface IClusterer<T, U>
    {
        FeatureClusterParameters Parameters { get; set; }
        /// <summary>
        /// Clusters the data objects provided in the list.
        /// </summary>
        /// <param name="data">Data to cluster.</param>                
        List<U> Cluster(List<T> data, List<U> clusters);
        List<U> Cluster(List<T> data);
    }
}
