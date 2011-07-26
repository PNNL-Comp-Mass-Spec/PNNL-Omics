using System;
using System.Collections.Generic;
using System.Text;
using PNNLOmics.Data.Features;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// Performs the base linkage types.
    /// </summary>
    /// <typeparam name="T">Features to cluster.</typeparam>
    /// <typeparam name="T">Clusters produced.</typeparam>
	public abstract class LinkageClustererBase<T, U> : IClusterer<T, U>
        where T : FeatureLight, IChildFeature<U>, new()    
        where U : FeatureLight, IFeatureCluster<T>, new()
	{
        public LinkageClustererBase()
        {
            SeedClusterID = 0;
        }

        public LinkageClustererBase(int id)
        {
            SeedClusterID = id;
        }
        /// <summary>
        /// Gets or sets the initial cluster Id to use when assingning ID's to a cluster.
        /// </summary>
        public int SeedClusterID
        {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the parameters used 
		/// </summary>
		public FeatureClusterParameters<T> Parameters { get; set; }

		/// <summary>
		/// Clusters the UMC data and returns a list of valid UMC Clusters.
		/// </summary>
		/// <param name="data">Data to cluster.</param>
		/// <returns>List of UMC clusters.</returns>
		public List<U> Cluster(List<T> data)
		{
			return this.Cluster(data, new List<U>());
		}

		public abstract List<U> Cluster(List<T> data, List<U> clusters);

		/// <summary>
		/// Creates a list of singleton clusters from the UMC data between start and stop.
		/// </summary>
		/// <param name="data">Data to create singleton's from.</param>
		/// <param name="start">Start UMC index.</param>
		/// <param name="stop">Stop UMC Index.</param>
		/// <returns>List of singleton clusters.</returns>
		//private List<T> CreateSingletonClusters(List<UMC> data, int start, int stop)
		protected Dictionary<int, U> CreateSingletonClusters(List<T> data, int start, int stop)
		{			
			Dictionary<int, U> clusters = new Dictionary<int, U>();
			int tempID                  = SeedClusterID;

			for (int i = start; i <= stop; i++)
			{				
                T umc        = data[i];

				// Add the feature to the parent so the cluster will point to the child.
                U parentFeature         = new U();
                int id                  = tempID++;
				parentFeature.ID        = id;

                parentFeature.AddChildFeature(umc);
                umc.SetParentFeature(parentFeature);		

				// Add to output list...
				clusters.Add(id, parentFeature);
			}
            foreach (U cluster in clusters.Values)
            {
                cluster.CalculateStatistics(Parameters.CentroidRepresentation);
            }

            SeedClusterID = tempID;

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
		protected virtual List<PairwiseDistance<T>> CalculatePairWiseDistances(int start, int stop, List<T> data)
		{
			double massTolerance                = Parameters.Tolerances.Mass;
			double netTolerance                 = Parameters.Tolerances.RetentionTime;
			double driftTolerance               = Parameters.Tolerances.DriftTime;
			bool onlyClusterSameChargeStates    = Parameters.OnlyClusterSameChargeStates;

			List<PairwiseDistance<T>> distances = new List<PairwiseDistance<T>>();
			for (int i = start; i < stop; i++)
			{
				T featureX          = data[i];
				double driftTimeX   = featureX.DriftTime;
				double netAlignedX  = featureX.RetentionTime;
				double massAlignedX = featureX.MassMonoisotopic;
				int chargeStateX    = featureX.ChargeState;			

				for (int j = i + 1; j <= stop; j++)
				{
					// Don't calculate distance to self.                    
					T featureY      = data[j];					

					// Calculate the distances here (using a cube).  We dont care if we are going to re-compute
					// these again later, because here we want to fall within the cube, the distance function used
					// later is more related to determining a scalar value instead.
                    bool withinRange = Parameters.RangeFunction(featureX, featureY);

					// Make sure we fall within the distance range before computing...
					if (withinRange)
					{
						// If IMS or equivalent only cluster similar charge states                        
						if (onlyClusterSameChargeStates)
						{
							// Make sure it's the same charge state
							if (chargeStateX == featureY.ChargeState)
							{
								// Calculate the pairwise distance
								PairwiseDistance<T> pairwiseDistance    = new PairwiseDistance<T>();
								pairwiseDistance.FeatureX               = featureX;
								pairwiseDistance.FeatureY               = featureY;
								pairwiseDistance.Distance               = Parameters.DistanceFunction(featureX, featureY);
								distances.Add(pairwiseDistance);
							}
						}
						else
						{
							// Calculate the pairwise distance
							PairwiseDistance<T> pairwiseDistance    = new PairwiseDistance<T>();
							pairwiseDistance.FeatureX               = featureX;
							pairwiseDistance.FeatureY               = featureY;
							pairwiseDistance.Distance               = Parameters.DistanceFunction(featureX, featureY);
							distances.Add(pairwiseDistance);
						}
					}
				}
			}
			return distances;
		}
	}
}
