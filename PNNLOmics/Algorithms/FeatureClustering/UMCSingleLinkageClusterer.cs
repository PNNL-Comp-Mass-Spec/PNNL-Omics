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
using PNNLOmics.Algorithms.FeatureClustering.Data;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// Clusters UMC's (LC-MS Features, LC-IMS-MS Features) into UMC Clusters.
    /// </summary>
    public class UMCSingleLinkageClusterer: IClusterer<UMC, UMCCluster>
    {
        #region Members
        /// <summary>
        /// Clustering parameters 
        /// </summary>
        private UMCSingleLinkageClustererParameters m_parameters;
        #endregion

        /// <summary>
        /// Default Constructor.  This sets the parameters and tolerances to their default values.
        /// </summary>
        public UMCSingleLinkageClusterer()
        {
            m_parameters = new UMCSingleLinkageClustererParameters();
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

        #region Private Methods
        /// <summary>
        /// Checks the two clusters to see if they have UMC's with the same group ID's.
        /// </summary>
        /// <param name="clusterX">Cluster x.</param>
        /// <param name="clusterY">Cluster y.</param>
        /// <returns>True if the clusters overlap based on group ID or false if they do not.</returns>
        private bool DoClustersHaveGroupOverlap(UMCCluster clusterX, UMCCluster clusterY)
        {
            bool overlap = false;

            foreach (UMC umcX in clusterX.UMCList)
            {
                foreach(UMC umcY in clusterY.UMCList)
                {
                    if (umcX.GroupID == umcY.GroupID)
                        return true;
                }
            }

            return overlap;
        }
        /// <summary>
        /// Performs single linkage clustering over the data and returns a list of UMC clusters.
        /// </summary>
        /// <param name="data">Data to cluster on.</param>
        /// <param name="distances">pairwise distance between UMC's.</param>
        /// <returns>List of UMC clusters.</returns>
        private List<UMCCluster> LinkUMCs(List<PairwiseUMCDistance> distances)
        {
            /// 
            /// Create a new list of clusters 
            /// 
            List<UMCCluster> clusters = new List<UMCCluster>();

            /// 
            /// Sort the distances 
            /// 
            distances.Sort(new PairwiseUMCDistance());

            /// 
            /// Then do the linking 
            /// 
            foreach (PairwiseUMCDistance distance in distances)
            {
                UMC featureX = distance.FeatureX;
                UMC featureY = distance.FeatureY;

                UMCCluster clusterX = featureX.UmcCluster;
                UMCCluster clusterY = featureY.UmcCluster;

                /// 
                /// Determine if they are already clustered into the same cluster
                /// 
                if (clusterX == clusterY && clusterX != null)
                    continue;

                /// 
                /// Now we make sure we don't merge two clusters that have the same group ID.
                /// 
                bool hasOverlap = DoClustersHaveGroupOverlap(clusterX, clusterY);

                /// 
                /// If there is no overlap of group ID's then we can go ahead and merge the clusters.
                /// 
                if (hasOverlap == false)
                {
                    UMCCluster cluster = new UMCCluster();

                    /// 
                    /// Remove the references for all the clusters in the group 
                    /// and point them to the new cluster.
                    /// 
                    foreach (UMC umcX in clusterX.UMCList)
                    {
                        umcX.UmcCluster = cluster;
                        cluster.UMCList.Add(umcX);
                    }
                    foreach (UMC umcY in clusterY.UMCList)
                    {
                        umcY.UmcCluster = cluster;
                        cluster.UMCList.Add(umcY);
                    }
                    /// 
                    /// Remove the old cluster
                    /// 
                    clusters.Remove(clusterX);
                    clusters.Remove(clusterY);

                    /// 
                    /// Then add it back into the main list of clusters
                    /// 
                    clusters.Add(cluster);
                }
            }

            return clusters;
        }
        /// <summary>
        /// Calculates pairwise distances between features in the list of potential features to cluster.        
        /// </summary>
        /// <param name="start">Start UMC index.</param>
        /// <param name="stop">Stop UMC index.</param>
        /// <param name="data">List of data to compute distances over.</param>
        /// <returns>List of UMC distances to consider during clustering.</returns>
        private List<PairwiseUMCDistance> CalculatePairWiseDistances(int start, int stop, List<UMC> data)
        {
            double massTolerance    = m_parameters.Tolerances.Mass;
            double netTolerance     = m_parameters.Tolerances.NET;            
            double driftTolerance   = m_parameters.Tolerances.DriftTime;

            List<PairwiseUMCDistance> distances = new List<PairwiseUMCDistance>();
            for(int i = start; i < stop - 1; i++) // (int i = start; i < stop - 1; i++)
            {
                UMC featureX = data[i];
                for(int j = i + 1; j < stop - 1; j++)
                {
                    /// 
                    /// Don't calculate distance to self.
                    /// 
                    if (i == j)
                        continue;

                    UMC featureY = data[j];

                    /// 
                    /// Don't calculate distance to self....?
                    /// 
                    if (featureX.ID == featureY.ID)
                        continue;

                    /// 
                    /// Don't calculate distance to other features within same group
                    /// 
                    if (featureY.GroupID == featureX.GroupID)
                        continue;

                    /// 
                    /// Calculate the distances here (using a cube).  We dont care if we are going to re-compute
                    /// these again later, because here we want to fall within the cube, the distance function used
                    /// later is more related to determining a scalar value instead.
                    /// 
                    double massDiff  = Math.Abs(Feature.ComputeMassPPMDifference( featureX.MassMonoisotopicAligned, 
                                                                        featureY.MassMonoisotopicAligned));
                    double netDiff   = Math.Abs(featureX.NETAligned - featureY.NETAligned);
                    double driftDiff = Math.Abs(featureX.DriftTime  - featureY.DriftTime);

                    /// 
                    /// Make sure we fall within the distance range before computing...
                    /// 
                    if (massDiff <= massTolerance && netDiff <= netTolerance && driftDiff <= driftTolerance)
                    {

                        /// 
                        /// If IMS or equivalent only cluster similar charge states
                        /// 
                        if (m_parameters.OnlyClusterSameChargeStates == true)
                        {
                            /// 
                            /// Make sure it's the same charge state
                            /// 
                            if (featureX.ChargeState == featureY.ChargeState)
                            {
                                /// 
                                /// Calculate the pairwise distance
                                /// 
                                PairwiseUMCDistance pairwiseDistance = new PairwiseUMCDistance();
                                pairwiseDistance.FeatureX = featureX;
                                pairwiseDistance.FeatureY = featureY;                                
                                pairwiseDistance.Distance = m_parameters.DistanceFunction(featureX, featureY);
                            }
                        }
                        else
                        {
                            /// 
                            /// Calculate the pairwise distance
                            /// 
                            PairwiseUMCDistance pairwiseDistance = new PairwiseUMCDistance();
                            pairwiseDistance.FeatureX = featureX;
                            pairwiseDistance.FeatureY = featureY;
                            pairwiseDistance.Distance = m_parameters.DistanceFunction(featureX, featureY);
                        }
                    }
                }
            }
            return distances;
        }
        #endregion

        #region IClusterer<UMC,UMCCluster> Members
        /// <summary>
        /// Clusters UMC's into UMC Clusters.
        /// </summary>
        /// <param name="data">List of data to cluster.</param>
        /// <returns>List of clustered data.</returns>
        public void Cluster(List<UMC> data, List<UMCCluster> clusters)
        {
            /**********************************************************************************************
             * 
             * This clustering algorithm first sorts the list of input UMC's by mass.  It then
             * iterates through this list partitioning the data into blocks of UMC's based on a 
             * mass tolerance.  When it finds gaps larger or equal to the mass (ppm) tolerance
             * specified by the user, it will process the data before the gap (a block) until the 
             * current index of the features in question.  
             * 
             **********************************************************************************************/
            
            /// 
            /// The first thing we do is to sort the features based on mass since we know that has the least
            /// variability in the data across runs.
            /// 
            data.Sort(new Comparison<UMC>(UMC.MassAlignedComparison));

            /// 
            /// Now partition the data based on mass ranges and the parameter values.
            /// 
            double massTolerance  = m_parameters.Tolerances.Mass;
            /// 
            /// This is the index of first feature of a given mass partition.
            /// 
            int startFeatureIndex = 0;

            for (int i = 0; i < data.Count - 1; i++)
            {
                /// 
                /// Here we compute the ppm mass difference between consecutive features (based on mass).
                /// This will determine if we cluster a block of data or not.
                ///                 
                UMC featureX    = data[i];
                UMC featureY    = data[i + 1];
                double ppm      = Feature.ComputeMassPPMDifference( featureX.MassMonoisotopicAligned, 
                                                                    featureY.MassMonoisotopicAligned);

                /// 
                /// If the difference is greater than the tolerance then we cluster 
                /// the lower masses.
                /// 
                if (ppm > massTolerance)
                {
                    /// 
                    /// If this is true then we only have one feature to cluster
                    /// 
                    if (startFeatureIndex == i)
                    {
                        UMCCluster cluster  = new UMCCluster();
                        featureX.UmcCluster = cluster; 
                        cluster.UMCList.Add(featureX);
                        clusters.Add(cluster);
                    }
                    else
                    {
                        /// 
                        /// Otherwise we have more than one feature to cluster to consider.
                        /// 
                        List<PairwiseUMCDistance> distances = CalculatePairWiseDistances(startFeatureIndex, i, data);
                        List<UMCCluster>  blockClusters     = LinkUMCs(distances);
                        clusters.AddRange(blockClusters);
                    }
                    startFeatureIndex = i + 1;
                }
            }                    
        }
        #endregion

        /// <summary>
        /// Holds the distance between two features and indices.
        /// </summary>
        internal class PairwiseUMCDistance: IComparer<PairwiseUMCDistance>
        {
            #region Properties
            /// <summary>
            /// Gets or sets the x feature.
            /// </summary>
            public UMC FeatureX { get; set; }
            /// <summary>
            /// Gets or sets the y feature.
            /// </summary>
            public UMC FeatureY { get; set; }
            /// <summary>
            /// Gets or sets the distance between the two features.
            /// </summary>
            public double Distance { get; set; }
            #endregion

            #region IComparer<PairwiseUMCDistance> Members
            /// <summary>
            /// Compares the distance between x and y.
            /// </summary>
            /// <param name="x">Feature x.</param>
            /// <param name="y">Feature y.</param>
            /// <returns>Returns an integer value determining if x is greater than, less than, or equal to y.</returns>
            public int Compare(PairwiseUMCDistance x, PairwiseUMCDistance y)
            {
                return x.Distance.CompareTo(y.Distance);
            }
            #endregion
        }
    }
}
