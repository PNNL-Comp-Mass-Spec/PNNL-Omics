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

using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// Clusters UMC's (LC-MS Features, LC-IMS-MS Features) into UMC Clusters.
    /// </summary>
    public class UMCSingleLinkageClusterer: IClusterer<UMC, UMCCluster>
    {
        #region Members
        #endregion

        /// <summary>
        /// Default Constructor.  This sets the parameters and tolerances to their default values.
        /// </summary>
        public UMCSingleLinkageClusterer()
        {
            Parameters = new UMCSingleLinkageClustererParameters();            
        }

        #region Properties
        /// <summary>
        /// Gets or sets the parameters used 
        /// </summary>
        public UMCSingleLinkageClustererParameters Parameters
        {
            get;
            set;
        }
        #endregion

        #region Clustering Methods
        /// <summary>
        /// Checks the two clusters to see if they have UMC's with the same group ID's.
        /// </summary>
        /// <param name="clusterX">Cluster x.</param>
        /// <param name="clusterY">Cluster y.</param>
        /// <returns>True if the clusters overlap based on group ID or false if they do not.</returns>
        private bool DoClustersHaveGroupOverlap(UMCCluster clusterX, UMCCluster clusterY)
        {            
            foreach (UMC umcX in clusterX.UMCList)
            {
                foreach(UMC umcY in clusterY.UMCList)
                {
                    if (umcX.GroupID == umcY.GroupID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Performs single linkage clustering over the data and returns a list of UMC clusters.
        /// </summary>
        /// <param name="data">Data to cluster on.</param>
        /// <param name="distances">pairwise distance between UMC's.</param>
        /// <returns>List of UMC clusters.</returns>
        private List<UMCCluster> LinkUMCs(List<PairwiseUMCDistance> distances, List<UMCCluster> clusters)
        {
            // We assume that the features have already been put into singleton
            // clusters or have a cluster already associated with them.  Otherwise
            // nothing will cluster.

            // Sort links based on distance
            distances.Sort(new PairwiseUMCDistance());
            
            // Then do the linking             
            foreach (PairwiseUMCDistance distance in distances)
            {
                UMC featureX = distance.FeatureX;
                UMC featureY = distance.FeatureY;

                UMCCluster clusterX = featureX.UmcCluster;
                UMCCluster clusterY = featureY.UmcCluster;                
                 
                // Determine if they are already clustered into the same cluster                                 
                if (clusterX == clusterY && clusterX != null)
                {
                    continue;
                }

                // Now we make sure we don't merge two clusters that have
                // the same group ID.
                bool hasOverlap = DoClustersHaveGroupOverlap(clusterX, 
                                                             clusterY);

                // If there is no overlap of group ID's then we can go ahead 
                // and merge the clusters into one of the clusters.                
                if (!hasOverlap)
                {                                        
                    // Remove the references for all the clusters in the group 
                    // and merge them into the other cluster.                    
                    foreach (UMC umcX in clusterX.UMCList)
                    {
                        umcX.UmcCluster = clusterY;
                        clusterY.UMCList.Add(umcX);
                    }                    
                    
                    // Remove the old cluster so we don't process it again.
                    clusters.Remove(clusterX);                    
                }
            }

            return clusters;
        }
        /// <summary>
        /// Creates a list of singleton clusters from the UMC data between start and stop.
        /// </summary>
        /// <param name="data">Data to create singleton's from.</param>
        /// <param name="start">Start UMC index.</param>
        /// <param name="stop">Stop UMC Index.</param>
        /// <returns>List of singleton clusters.</returns>
        private List<UMCCluster> CreateSingletonClusters(List<UMC> data, int start, int stop)
        {
            List<UMCCluster> clusters = new List<UMCCluster>();
            for (int i = start; i < stop; i++)
            {
                UMC umc = data[i];
                
                // Double-reference between cluster and UMC so both can point to each other.
                umc.UmcCluster = new UMCCluster();
                umc.UmcCluster.UMCList.Add(umc);

                // Add to output list...
                clusters.Add(umc.UmcCluster);
            }
            return clusters;
        }
        /// <summary>
        /// Calculates pairwise distances between features in the list of 
        /// potential features to cluster.        
        /// </summary>
        /// <param name="start">Start UMC index.</param>
        /// <param name="stop">Stop UMC index.</param>
        /// <param name="data">List of data to compute distances over.</param>
        /// <returns>List of UMC distances to consider during clustering.</returns>
        private List<PairwiseUMCDistance> CalculatePairWiseDistances(int start,         
                                                                     int stop, 
                                                                List<UMC> data)
        {
            double massTolerance    = Parameters.Tolerances.Mass;
            double netTolerance     = Parameters.Tolerances.NET;            
            double driftTolerance   = Parameters.Tolerances.DriftTime;

            List<PairwiseUMCDistance> distances = new List<PairwiseUMCDistance>();
            for(int i = start; i < stop - 1; i++)
            {
                UMC featureX = data[i];
                for(int j = i + 1; j < stop; j++)
                {
                    // Don't calculate distance to self.                    
                    UMC featureY = data[j];                    

                    // Don't calculate distance to other features within same group
                    if (featureY.GroupID == featureX.GroupID)
                    {
                        continue;
                    }

                    // Calculate the distances here (using a cube).  We dont care if we are going to re-compute
                    // these again later, because here we want to fall within the cube, the distance function used
                    // later is more related to determining a scalar value instead.
                    double massDiff  = Math.Abs(Feature.ComputeMassPPMDifference( featureX.MassMonoisotopicAligned, 
                                                                        featureY.MassMonoisotopicAligned));
                    double netDiff   = Math.Abs(featureX.NETAligned - featureY.NETAligned);
                    double driftDiff = Math.Abs(featureX.DriftTime  - featureY.DriftTime);

                    // Make sure we fall within the distance range before computing...
                    if (massDiff <= massTolerance && netDiff <= netTolerance && driftDiff <= driftTolerance)
                    {                                         
                        // If IMS or equivalent only cluster similar charge states                        
                        if (Parameters.OnlyClusterSameChargeStates)
                        {
                            // Make sure it's the same charge state
                            if (featureX.ChargeState == featureY.ChargeState)
                            {
                                // Calculate the pairwise distance
                                PairwiseUMCDistance pairwiseDistance = new PairwiseUMCDistance();
                                pairwiseDistance.FeatureX = featureX;
                                pairwiseDistance.FeatureY = featureY;                                
                                pairwiseDistance.Distance = Parameters.DistanceFunction(featureX, featureY);
                                distances.Add(pairwiseDistance);
                            }
                        }
                        else
                        {
                            // Calculate the pairwise distance
                            PairwiseUMCDistance pairwiseDistance = new PairwiseUMCDistance();
                            pairwiseDistance.FeatureX = featureX;
                            pairwiseDistance.FeatureY = featureY;
                            pairwiseDistance.Distance = Parameters.DistanceFunction(featureX, featureY);
                            distances.Add(pairwiseDistance);
                        }
                    }
                }
            }
            return distances;
        }
        #endregion

        #region IClusterer<UMC,UMCCluster> Methods
        /// <summary>
        /// Clusters UMC's into UMC Clusters.
        /// </summary>
        /// <param name="data">List of data to cluster.</param>
        /// <returns>List of clustered data.</returns>
        public List<UMCCluster> Cluster(List<UMC> data, List<UMCCluster> clusters)
        {
            //  This clustering algorithm first sorts the list of input UMC's by mass.  It then
            //  iterates through this list partitioning the data into blocks of UMC's based on a 
            //  mass tolerance.  When it finds gaps larger or equal to the mass (ppm) tolerance
            //  specified by the user, it will process the data before the gap (a block) until the 
            //  current index of the features in question.  

            // Make sure we have data to cluster first.
            if (data == null)
            {
                throw new NullReferenceException("The input UMC data list was null.  Cannot process this data.");
            }
            
            // Make sure there is no null UMC data in the input list.
            int nullIndex = data.FindIndex(delegate(UMC x) { return x == null; });            
            if (nullIndex > 0)
            {
                throw new NullReferenceException("The UMC at index " + nullIndex.ToString() + " was null.  Cannot process this data.");
            }
                         
            // The first thing we do is to sort the features based on mass since we know that has the least
            // variability in the data across runs.
            data.Sort(new Comparison<UMC>(UMC.MassAlignedComparison));

            // Now partition the data based on mass ranges and the parameter values.
            double massTolerance  = Parameters.Tolerances.Mass;

            // This is the index of first feature of a given mass partition.
            int startUMCIndex = 0;
            int totalFeatures = data.Count;
            for (int i = 0; i < totalFeatures - 1; i++)
            {
                // Here we compute the ppm mass difference between consecutive features (based on mass).
                // This will determine if we cluster a block of data or not.                
                UMC umcX    = data[i];
                UMC umcY    = data[i + 1];
                double ppm  = Feature.ComputeMassPPMDifference( umcX.MassMonoisotopicAligned, 
                                                                umcY.MassMonoisotopicAligned);

                // If the difference is greater than the tolerance then we cluster 
                //  - we dont check the sign of the ppm because the data should be sorted based on mass.
                if (ppm > massTolerance)
                {                    
                    // If start UMC Index is equal to one, then that means the feature at startUMCIndex
                    // could not find any other features near it within the mass tolerance specified.
                    if (startUMCIndex == i)
                    {
                        UMCCluster cluster  = new UMCCluster();
                        umcX.UmcCluster = cluster; 
                        cluster.UMCList.Add(umcX);
                        clusters.Add(cluster);
                    }
                    else
                    {
                        // Otherwise we have more than one feature to cluster to consider.

                        List<PairwiseUMCDistance> distances = CalculatePairWiseDistances(startUMCIndex, i, data);
                        List<UMCCluster> localClusters      = CreateSingletonClusters(data, startUMCIndex, i);                        
                        List<UMCCluster>  blockClusters     = LinkUMCs(distances, clusters);
                        clusters.AddRange(blockClusters);
                    }
                    startUMCIndex = i + 1;
                }
            }

            // Make sure that we cluster what is left over.
            if (startUMCIndex < totalFeatures)
            {
                List<PairwiseUMCDistance> distances = CalculatePairWiseDistances(startUMCIndex, totalFeatures, data);
                List<UMCCluster> localClusters = CreateSingletonClusters(data, startUMCIndex, totalFeatures);
                List<UMCCluster> blockClusters = LinkUMCs(distances, localClusters);
                clusters.AddRange(blockClusters);
            }

            return clusters;
        }
        /// <summary>
        /// Clusters the UMC data and returns a list of valid UMC Clusters.
        /// </summary>
        /// <param name="data">Data to cluster.</param>
        /// <returns>List of UMC clusters.</returns>
        public List<UMCCluster> Cluster(List<UMC> data)
        {            
            return this.Cluster(data, new List<UMCCluster>());
        }
        #endregion
    }
}
