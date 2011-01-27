using System;
using System.Collections.Generic;
using System.Text;
using PNNLOmics.Data.Features;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.FeatureClustering
{
	public abstract class LinkageClustererBase<T> : IClusterer<UMCLight, T> where T : UMCClusterLight, new()
	{
		/// <summary>
		/// Gets or sets the parameters used 
		/// </summary>
		public FeatureClusterParameters Parameters { get; set; }

		/// <summary>
		/// Clusters the UMC data and returns a list of valid UMC Clusters.
		/// </summary>
		/// <param name="data">Data to cluster.</param>
		/// <returns>List of UMC clusters.</returns>
		public List<T> Cluster(List<UMCLight> data)
		{
			return this.Cluster(data, new List<T>());
		}

		public abstract List<T> Cluster(List<UMCLight> data, List<T> clusters);

		/// <summary>
		/// Creates a list of singleton clusters from the UMC data between start and stop.
		/// </summary>
		/// <param name="data">Data to create singleton's from.</param>
		/// <param name="start">Start UMC index.</param>
		/// <param name="stop">Stop UMC Index.</param>
		/// <returns>List of singleton clusters.</returns>
		//private List<T> CreateSingletonClusters(List<UMC> data, int start, int stop)
		protected Dictionary<int, T> CreateSingletonClusters(List<UMCLight> data, int start, int stop)
		{
			//List<T> clusters = new List<T>();
			Dictionary<int, T> clusters = new Dictionary<int, T>();

			int tempID = -1;
			for (int i = start; i <= stop; i++)
			{
				UMCLight umc = data[i];

				// Double-reference between cluster and UMC so both can point to each other.
				umc.UMCCluster = new T() as UMCClusterLight;
				umc.UMCCluster.UMCList.Add(umc);
				int id = tempID--;
				umc.UMCCluster.ID = id;

				// Add to output list...
				clusters.Add(id, umc.UMCCluster as T);
			}
            foreach (T cluster in clusters.Values)
            {
                cluster.CalculateStatistics(Parameters.CentroidRepresentation);
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
		protected virtual List<PairwiseDistance<UMCLight>> CalculatePairWiseDistances(int start, int stop, List<UMCLight> data)
		{
			double massTolerance = Parameters.Tolerances.Mass;
			double netTolerance = Parameters.Tolerances.RetentionTime;
			double driftTolerance = Parameters.Tolerances.DriftTime;
			bool onlyClusterSameChargeStates = Parameters.OnlyClusterSameChargeStates;

			List<PairwiseDistance<UMCLight>> distances = new List<PairwiseDistance<UMCLight>>();
			for (int i = start; i < stop; i++)
			{

				UMCLight featureX = data[i];
				double driftTimeX = featureX.DriftTime;
				double netAlignedX = featureX.RetentionTime;
				double massAlignedX = featureX.MassMonoisotopic;
				int chargeStateX = featureX.ChargeState;

				int groupIDX = featureX.GroupID;

				for (int j = i + 1; j <= stop; j++)
				{
					// Don't calculate distance to self.                    
					UMCLight featureY = data[j];

					// Don't calculate distance to other features within same group
					//if (featureY.GroupID == groupIDX)
					{
						//continue;
					}

					// Calculate the distances here (using a cube).  We dont care if we are going to re-compute
					// these again later, because here we want to fall within the cube, the distance function used
					// later is more related to determining a scalar value instead.
					double massDiff = Math.Abs(Feature.ComputeMassPPMDifference(massAlignedX,
																		featureY.MassMonoisotopic));
					double netDiff = Math.Abs(netAlignedX - featureY.RetentionTime);
					double driftDiff = Math.Abs(driftTimeX - featureY.DriftTime);

					// Make sure we fall within the distance range before computing...
					if (massDiff <= massTolerance && netDiff <= netTolerance && driftDiff <= driftTolerance)
					{
						// If IMS or equivalent only cluster similar charge states                        
						if (onlyClusterSameChargeStates)
						{
							// Make sure it's the same charge state
							if (chargeStateX == featureY.ChargeState)
							{
								// Calculate the pairwise distance
								PairwiseDistance<UMCLight> pairwiseDistance = new PairwiseDistance<UMCLight>();
								pairwiseDistance.FeatureX = featureX;
								pairwiseDistance.FeatureY = featureY;
								pairwiseDistance.Distance = Parameters.DistanceFunction(featureX, featureY);
								distances.Add(pairwiseDistance);
							}
						}
						else
						{
							// Calculate the pairwise distance
							PairwiseDistance<UMCLight> pairwiseDistance = new PairwiseDistance<UMCLight>();
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
	}
}
