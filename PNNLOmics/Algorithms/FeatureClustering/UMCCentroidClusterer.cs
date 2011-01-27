/// <author>Kevin Crowell</author>
/// <datecreated>01-07-2011</datecreated>
/// <summary>Perform clustering of UMC features across datasets into UMC Clusters using average linkage.</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.FeatureClustering
{
	public class UMCCentroidClusterer<T> : LinkageClustererBase<T> where T : UMCClusterLight, new()
	{
		/// <summary>
		/// Compares the masses of two light features.
		/// </summary>
		private Comparison<UMCLight> m_massComparer;

		/// <summary>
        /// Default Constructor.  This sets the parameters and tolerances to their default values.
        /// </summary>
        public UMCCentroidClusterer()
        {
            Parameters = new FeatureClusterParameters();
			m_massComparer = new Comparison<UMCLight>(UMCLight.MassComparison);
        }
        private double GetAverageClusterDistance(T clusterI, T clusterJ, DistanceFunction function)
        {
            double sum = 0;
            foreach (UMCLight featureI in clusterI.UMCList)
            {
                foreach (UMCLight featureJ in clusterJ.UMCList)
                {
                    sum = function(featureI, featureJ);
                }
            }
            sum = sum / Convert.ToDouble(clusterI.UMCList.Count * clusterJ.UMCList.Count);

            return sum;
        }
        /// <summary>
        /// Calculates pairwise distances between features in the list of 
        /// potential features to cluster.        
        /// </summary>
        /// <param name="start">Start UMC index.</param>
        /// <param name="stop">Stop UMC index.</param>
        /// <param name="data">List of data to compute distances over.</param>
        /// <returns>List of UMC distances to consider during clustering.</returns>
        protected List<PairwiseDistance<T>> CalculateDistances(Dictionary<int, T> clusters)
        {
            double massTolerance             = Parameters.Tolerances.Mass;
            double netTolerance              = Parameters.Tolerances.RetentionTime;
            double driftTolerance            = Parameters.Tolerances.DriftTime;
            bool onlyClusterSameChargeStates = Parameters.OnlyClusterSameChargeStates;

            List<PairwiseDistance<T>> distances = new List<PairwiseDistance<T>>();
            foreach(T clusterI in clusters.Values) 
            {                
                double driftTimeX   = clusterI.DriftTime;
                double netAlignedX  = clusterI.RetentionTime;
                double massAlignedX = clusterI.MassMonoisotopic;
                int chargeStateX    = clusterI.ChargeState;
                
                foreach(T clusterJ in clusters.Values)
                {
                    // Don't calculate distance to other features within same group
                    if (clusterI == clusterJ)
                    {
                        continue;
                    }

                    // Calculate the distances here (using a cube).  We dont care if we are going to re-compute
                    // these again later, because here we want to fall within the cube, the distance function used
                    // later is more related to determining a scalar value instead.
                    double massDiff = Math.Abs(Feature.ComputeMassPPMDifference(massAlignedX,
                                                                        clusterJ.MassMonoisotopic));
                    double netDiff = Math.Abs(netAlignedX - clusterJ.RetentionTime);
                    double driftDiff = Math.Abs(driftTimeX - clusterJ.DriftTime);

                    // Make sure we fall within the distance range before computing...
                    if (massDiff <= massTolerance && netDiff <= netTolerance && driftDiff <= driftTolerance)
                    {
                        // If IMS or equivalent only cluster similar charge states                        
                        if (onlyClusterSameChargeStates)
                        {
                            // Make sure it's the same charge state
                            if (chargeStateX == clusterJ.ChargeState)
                            {
                                // Calculate the pairwise distance
                                PairwiseDistance<T> pairwiseDistance = new PairwiseDistance<T>();
                                pairwiseDistance.FeatureX = clusterI;
                                pairwiseDistance.FeatureY = clusterJ;
                                pairwiseDistance.Distance = GetAverageClusterDistance(clusterI, clusterJ, Parameters.DistanceFunction);
                                distances.Add(pairwiseDistance);
                            }
                        }
                        else
                        {
                            // Calculate the pairwise distance
                            PairwiseDistance<T> pairwiseDistance = new PairwiseDistance<T>();
                            pairwiseDistance.FeatureX = clusterI;
                            pairwiseDistance.FeatureY = clusterJ;
                            pairwiseDistance.Distance = GetAverageClusterDistance(clusterI, clusterJ, Parameters.DistanceFunction);
                            distances.Add(pairwiseDistance);
                        }
                    }
                }
            }
            return distances;
        }
		/// <summary>
		/// Clusters UMC's into UMC Clusters using Average Linkage Clustering.
		/// </summary>
		/// <param name="data">List of data to cluster.</param>
		/// <returns>List of clustered data.</returns>
		public override List<T> Cluster(List<UMCLight> data, List<T> clusters)
		{
			/*
			 * This clustering algorithm first sorts the list of input UMC's by mass.  It then iterates
			 * through this list partitioning the data into blocks of UMC's based on a mass tolerance.
			 * When it finds gaps larger or equal to the mass (ppm) tolerance specified by the user,
			 * it will process the data before the gap (a block) until the current index of the features in question.
			 */  

			// Make sure we have data to cluster first.
			if (data == null)
			{
				throw new NullReferenceException("The input UMC data list was null.  Cannot process this data.");
			}

			// Make sure there is no null UMC data in the input list.
			int nullIndex = data.FindIndex(delegate(UMCLight x) { return x == null; });
			if (nullIndex > 0)
			{
				throw new NullReferenceException("The UMC at index " + nullIndex.ToString() + " was null.  Cannot process this data.");
			}


			// The first thing we do is to sort the features based on mass since we know that has the least variability in the data across runs.
			data.Sort(m_massComparer);

			// Now partition the data based on mass ranges and the parameter values.
			double massTolerance = Parameters.Tolerances.Mass;

			// This is the index of first feature of a given mass partition.
			int startUMCIndex = 0;
			int totalFeatures = data.Count;
			for (int i = 0; i < totalFeatures - 1; i++)
			{
				// Here we compute the ppm mass difference between consecutive features (based on mass).
				// This will determine if we cluster a block of data or not.                
				UMCLight umcX = data[i];
				UMCLight umcY = data[i + 1];
				double ppm = Math.Abs(Feature.ComputeMassPPMDifference(umcX.MassMonoisotopic, umcY.MassMonoisotopic));

				// If the difference is greater than the tolerance then we cluster 
				//  - we dont check the sign of the ppm because the data should be sorted based on mass.
				if (ppm > massTolerance)
				{
					// If start UMC Index is equal to one, then that means the feature at startUMCIndex
					// could not find any other features near it within the mass tolerance specified.
					if (startUMCIndex == i)
					{
						T cluster = new T();
						umcX.UMCCluster = cluster;
						cluster.UMCList.Add(umcX);
						clusters.Add(cluster);
					}
					else
					{
						// Otherwise we have more than one feature to to consider.												
                        Dictionary<int, T> localClusters    = CreateSingletonClusters(data, startUMCIndex, i);					
						List<T> blockClusters               = Cluster(localClusters);
						clusters.AddRange(blockClusters);
					}
					startUMCIndex = i + 1;
				}
			}

			// Make sure that we cluster what is left over.
			if (startUMCIndex < totalFeatures)
            {
                Dictionary<int, T> localClusters = CreateSingletonClusters(data, startUMCIndex, totalFeatures - 1);
                List<T> blockClusters            = Cluster(localClusters);
                clusters.AddRange(blockClusters);
			}

			foreach (UMCClusterLight cluster in clusters)
			{
				cluster.CalculateStatistics(Parameters.CentroidRepresentation);
			}

			return clusters;
		}

		/// <summary>
		/// Performs average linkage clustering over the data and returns a list of UMC clusters.
		/// </summary>
		/// <param name="clusters">Singleton clusters</param>		
		/// <returns>List of T clusters.</returns>
		private List<T> Cluster(Dictionary<int, T> clusters)
		{
            bool isClustering = true;            
            while (isClustering)
            {
                isClustering = false;

                // Compute pairwise distances between cluster centroids.
                List<PairwiseDistance<T>> distances = CalculateDistances(clusters);
                
                // Find the minimal distance 
                var newDistances = from element in distances
                                   orderby element.Distance
                                   select element;

                // Link, we dont just take the smallest distance because
                // the two clusters may not be in tolerance.
                foreach (PairwiseDistance<T> distance in newDistances)
                {
                    T clusterX = distance.FeatureX;
                    T clusterY = distance.FeatureY;

                    // Determine if they are already clustered into the same cluster                                 
                    if (clusterX == clusterY && clusterX != null)
                    {
                        continue;
                    }

                    bool areClustersWithinDistance = AreClustersWithinTolerance(clusterX, clusterY);

                    // Only cluster if the distance between the clusters is acceptable                
                    if (areClustersWithinDistance)
                    {
                        // Remove the references for all the clusters in the group and merge them into the other cluster.                    
                        foreach (UMCLight umcX in clusterX.UMCList)
                        {
                            umcX.UMCCluster = clusterY;
                            clusterY.UMCList.Add(umcX);
                        }

                        // Remove the old cluster so we don't process it again.
                        clusters.Remove(clusterX.ID);
                        clusterX.CalculateStatistics(Parameters.CentroidRepresentation);
                        isClustering = true;
                        break;
                    }
                }
            }

			T[] array = new T[clusters.Values.Count];
			clusters.Values.CopyTo(array, 0);
			List<T> newClusters = new List<T>();
			newClusters.AddRange(array);

			return newClusters;
		}

		/// <summary>
		/// Determines if two clusters are within mass, NET, and drift time tolerances
		/// </summary>
		/// <param name="clusterX">One of the two clusters to test</param>
		/// <param name="clusterY">One of the two clusters to test</param>
		/// <returns>True if clusters are within tolerance, false otherwise</returns>
		private bool AreClustersWithinTolerance(UMCClusterLight clusterX, UMCClusterLight clusterY)
		{
			// Grab the tolerances
			double massTolerance = Parameters.Tolerances.Mass;
			double netTolerance = Parameters.Tolerances.RetentionTime;
			double driftTolerance = Parameters.Tolerances.DriftTime;

            //// Calculate the statistics of the Clusters
            //clusterX.CalculateStatistics(Parameters.CentroidRepresentation);
            //clusterY.CalculateStatistics(Parameters.CentroidRepresentation);

			// Calculate differences
			double massDiff = Math.Abs(Feature.ComputeMassPPMDifference(clusterX.MassMonoisotopic, clusterY.MassMonoisotopic));
			double netDiff = Math.Abs(clusterX.RetentionTime - clusterY.RetentionTime);
			double driftDiff = Math.Abs(clusterX.DriftTime - clusterY.DriftTime);

			// Return true only if all differences are within tolerance
			if (massDiff <= massTolerance && netDiff <= netTolerance && driftDiff <= driftTolerance)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
