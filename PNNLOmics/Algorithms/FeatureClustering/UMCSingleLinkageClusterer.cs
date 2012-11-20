/*////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * 
 * Name:    UMC Single Linkage Clusterer 
 * File:    UMCSingleLinkageClustering.cs
 * Author:  Brian LaMarche 
 * Purpose: Perform clustering of UMC features across datasets into UMC Clusters.
 * Date:    5-19-2010
 * Revisions:
 *          5-19-2010 - BLL - Created clustering class and algorithm.
 ////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

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
    public class UMCSingleLinkageClusterer<T, U> : LinkageClustererBase<T, U>
        where T : FeatureLight, IChildFeature<U>,   new()
        where U : FeatureLight, IFeatureCluster<T>, new()
	{
		#region Members
		/// <summary>
		/// Compares the masses of two light features.
		/// </summary>
		private Comparison<T> m_massComparer;
		#endregion

		/// <summary>
        /// Default Constructor.  This sets the parameters and tolerances to their default values.
        /// </summary>
        public UMCSingleLinkageClusterer()
        {
            Parameters		= new FeatureClusterParameters<T>();
			m_massComparer	= new Comparison<T>(FeatureLight.MassComparison);
        }

        #region Clustering Methods        
        /// <summary>
        /// Performs single linkage clustering over the data and returns a list of UMC clusters.
        /// </summary>
        /// <param name="data">Data to cluster on.</param>
        /// <param name="distances">pairwise distance between UMC's.</param>
        /// <returns>List of UMC clusters.</returns>        
		private List<U> LinkUMCs(List<PairwiseDistance<T>> distances, Dictionary<int, U> clusters)
        {
            // We assume that the features have already been put into singleton
            // clusters or have a cluster already associated with them.  Otherwise
            // nothing will cluster.

            // Sort links based on distance            
			var newDistances = from element in distances
							   orderby element.Distance
							   select element;
            
            // Then do the linking           
			
            //foreach (PairwiseDistance<UMC> distance in distances)
			foreach (PairwiseDistance<T> distance in newDistances)
            {
                T featureX = distance.FeatureX;
                T featureY = distance.FeatureY;

                U clusterX = featureX.ParentFeature as U;
                U clusterY = featureY.ParentFeature as U;                
                 
                // Determine if they are already clustered into the same cluster                                 
                if (clusterX == clusterY && clusterX != null)
                {
                    continue;
                }
                                                                       
                // Remove the references for all the clusters in the group 
                // and merge them into the other cluster.                    
                foreach (T umcX in clusterX.Features)
                {
                    umcX.SetParentFeature(clusterY);
                    clusterY.AddChildFeature(umcX);
                }

				// Remove the old cluster so we don't process it again.
				clusters.Remove(clusterX.ID);					                
            }

            //return clusters;
			U [] array			= new U[clusters.Values.Count];
			clusters.Values.CopyTo(array, 0);
			List<U> newClusters = new List<U>();
			newClusters.AddRange(array);

			return newClusters;
        }
        #endregion

		
        #region IClusterer<UMC, T> Methods
        /// <summary>
        /// Clusters UMC's into UMC Clusters.
        /// </summary>
        /// <param name="data">List of data to cluster.</param>
        /// <returns>List of clustered data.</returns>
		public override List<U> Cluster(List<T> data, List<U> clusters)
        {
            //  This clustering algorithm first sorts the list of input UMC's by mass.  It then
            //  iterates through this list partitioning the data into blocks of UMC's based on a 
            //  mass tolerance.  When it finds gaps larger or equal to the mass (ppm) tolerance
            //  specified by the user, it will process the data before the gap (a block) until the 
            //  current index of the features in question.  

            // Make sure we have data to cluster first.
            if (data == null)
            {
                throw new NullReferenceException("The input feature data list was null.  Cannot process this data.");
            }

            // Make sure there is no null UMC data in the input list.
            int nullIndex = data.FindIndex(delegate(T x) { return x == null; });            
            if (nullIndex > 0)
            {
                throw new NullReferenceException("The feature at index " + nullIndex.ToString() + " was null.  Cannot process this data.");
            }
			

			// The first thing we do is to sort the features based on mass since we know that has the least
            // variability in the data across runs.
            data.Sort(m_massComparer);
			
            // Now partition the data based on mass ranges and the parameter values.
            double massTolerance  = Parameters.Tolerances.Mass;

            // This is the index of first feature of a given mass partition.
            int startUMCIndex = 0;
            int totalFeatures = data.Count;
            for (int i = 0; i < totalFeatures - 1; i++)
            {
                // Here we compute the ppm mass difference between consecutive features (based on mass).
                // This will determine if we cluster a block of data or not.                
                T umcX              = data[i];
				T umcY              = data[i + 1];
                double massDiff     = Math.Abs(Feature.ComputeMassPPMDifference(umcX.MassMonoisotopicAligned, umcY.MassMonoisotopicAligned));

                // If the difference is greater than the tolerance then we cluster 
                //  - we dont check the sign of the ppm because the data should be sorted based on mass.
                if (massDiff > massTolerance)
                {                    
                    // If start UMC Index is equal to one, then that means the feature at startUMCIndex
                    // could not find any other features near it within the mass tolerance specified.
                    if (startUMCIndex == i)
                    {
                        U cluster  = new U();
                        cluster.AmbiguityScore = m_maxDistance;
                        umcX.SetParentFeature(cluster); 
                        cluster.AddChildFeature(umcX);
                        clusters.Add(cluster);
                    }
                    else
                    {
                        // Otherwise we have more than one feature to cluster to consider.												
						List<PairwiseDistance<T>> distances     = CalculatePairWiseDistances(startUMCIndex, i, data);
						Dictionary<int, U> localClusters        = CreateSingletonClusters(data, startUMCIndex, i);                        
                        List<U>  blockClusters                  = LinkUMCs(distances, localClusters);

                        CalculateAmbiguityScore(blockClusters);
                        clusters.AddRange(blockClusters);
                    }
                    startUMCIndex = i + 1;
                }
            }

            // Make sure that we cluster what is left over.
            if (startUMCIndex < totalFeatures)
            {
				List<PairwiseDistance<T>> distances = CalculatePairWiseDistances(startUMCIndex, totalFeatures - 1, data);                
				Dictionary<int, U> localClusters    = CreateSingletonClusters(data, startUMCIndex, totalFeatures - 1);                                                
                List<U> blockClusters               = LinkUMCs(distances, localClusters);
                        CalculateAmbiguityScore(blockClusters);
                clusters.AddRange(blockClusters);
            }

			foreach (U cluster in clusters)
            {
                cluster.CalculateStatistics(Parameters.CentroidRepresentation);
            }
            return clusters;
        }
        #endregion   
        
        public override string ToString()
        {
            return "single linkage clustering";
        }
    }
}
