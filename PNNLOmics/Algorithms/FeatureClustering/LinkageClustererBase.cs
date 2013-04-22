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
	public abstract class LinkageClustererBase<T, U> : IProgressNotifer, IClusterer<T, U>
        where T : FeatureLight, IChildFeature<U>, new()    
        where U : FeatureLight, IFeatureCluster<T>, new()
	{

        /// <summary>
        /// Compares the masses of two light features.
        /// </summary>
        protected Comparison<T> m_massComparer;

        /// <summary>
        /// Maximum distance when calculating ambiguity
        /// </summary>
        protected double m_maxDistance;

        public LinkageClustererBase()
        {
            SeedClusterID  = 0;
            m_maxDistance  = 10000;
            m_massComparer = new Comparison<T>(FeatureLight.MassAlignedComparison);


            ClusterReprocessor = new MedianSplitReprocessor<T, U>();
        }

        public LinkageClustererBase(int id)
        {
            SeedClusterID = id;
            m_maxDistance = 10000;
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
        /// Gets or sets the cluster reprocessor.
        /// </summary>
        public IClusterReprocessor<T, U> ClusterReprocessor
        {
            get;
            set;
        }

		/// <summary>
		/// Clusters the UMC data and returns a list of valid UMC Clusters.
		/// </summary>
		/// <param name="data">Data to cluster.</param>
		/// <returns>List of UMC clusters.</returns>
		public List<U> Cluster(List<T> data)
		{
			return this.Cluster(data, new List<U>());
		}


        private void Reprocess(List<U> clusters)
        {
            if (ClusterReprocessor != null)
            {
                ClusterReprocessor.ProcessClusters(clusters);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="distances"></param>
        /// <param name="clusters"></param>
        /// <returns></returns>
        public abstract List<U> LinkFeatures(List<PairwiseDistance<T>> distances, Dictionary<int, U> clusters);

        /// <summary>
        /// Clusters a set of data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="clusters"></param>
        /// <returns></returns>
		public virtual List<U> Cluster(List<T> data, List<U> clusters)
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
				throw new NullReferenceException("The input feature data list was null.  Cannot process this data.");
			}

			// Make sure there is no null UMC data in the input list.
			int nullIndex = data.FindIndex(delegate(T x) { return x == null; });
			if (nullIndex > 0)
			{
				throw new NullReferenceException("The feature at index " + nullIndex.ToString() + " was null.  Cannot process this data.");
			}

            OnNotify("Sorting cluster mass list");

			// The first thing we do is to sort the features based on mass since we know that has the least variability in the data across runs.
			data.Sort(m_massComparer);

			// Now partition the data based on mass ranges and the parameter values.
			double massTolerance = Parameters.Tolerances.Mass;

			// This is the index of first feature of a given mass partition.
			int startUMCIndex = 0;
			int totalFeatures = data.Count;


            OnNotify("Detecting mass partitions");
            int tenPercent = Convert.ToInt32(totalFeatures * .1);
            int counter    = 0;
            int percent    = 0;

			for (int i = 0; i < totalFeatures - 1; i++)
			{
                if (counter > tenPercent)
                {
                    counter = 0;
                    percent += 10;
                    OnNotify(string.Format("Clustering Completed...{0}%", percent));
                }
                counter++;

				// Here we compute the ppm mass difference between consecutive features (based on mass).
				// This will determine if we cluster a block of data or not.                
				T   umcX = data[i];
				T   umcY = data[i + 1];
                double ppm = Math.Abs(Feature.ComputeMassPPMDifference(umcX.MassMonoisotopicAligned, umcY.MassMonoisotopicAligned));

				// If the difference is greater than the tolerance then we cluster 
				//  - we dont check the sign of the ppm because the data should be sorted based on mass.
				if (ppm > massTolerance)
				{
					// If start UMC Index is equal to one, then that means the feature at startUMCIndex
					// could not find any other features near it within the mass tolerance specified.
					if (startUMCIndex == i)
                    {
                        U cluster               = new U();
                        cluster.AmbiguityScore  = m_maxDistance;
                        umcX.SetParentFeature(cluster);
                        cluster.AddChildFeature(umcX);
                        clusters.Add(cluster);
					}
					else
					{
						// Otherwise we have more than one feature to to consider.												
						List<PairwiseDistance<T>> distances     = CalculatePairWiseDistances(startUMCIndex, i, data);
						Dictionary<int, U> localClusters        = CreateSingletonClusters(data, startUMCIndex, i);
						List<U> blockClusters                   = LinkFeatures(distances, localClusters);
                        CalculateAmbiguityScore(blockClusters);
						clusters.AddRange(blockClusters);
					}

					startUMCIndex = i + 1;
				}
			}

			// Make sure that we cluster what is left over.
			if (startUMCIndex < totalFeatures)
			{

                OnNotify(string.Format("Clustering last partition...{0}%", percent));
				List<PairwiseDistance<T>> distances = CalculatePairWiseDistances(startUMCIndex, totalFeatures - 1, data);
				Dictionary<int, U> localClusters    = CreateSingletonClusters(data, startUMCIndex, totalFeatures - 1);
				List<U> blockClusters               = LinkFeatures(distances, localClusters);
                CalculateAmbiguityScore(blockClusters);
				clusters.AddRange(blockClusters);
			}


            OnNotify("Generating cluster statistics");
			foreach (U cluster in clusters)
			{
				cluster.CalculateStatistics(Parameters.CentroidRepresentation);
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
        /// Calculates the minimum distance between two clusters by pairwise feature comparisons.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected virtual double CalculateMinimumFeatureDistance(U x, U y)
        {
            double distance = m_maxDistance;

            foreach (T featureX in x.Features)
            {                
                foreach(T featureY in y.Features)
                {
                    double tempDistance = Parameters.DistanceFunction(featureX, featureY);
                    distance = Math.Min(tempDistance, distance);
                }                
            }

            return distance;
        }
        /// <summary>
        /// Calculates the ambiguity score
        /// </summary>
        /// <param name="clusters"></param>
        protected virtual void CalculateAmbiguityScore(List<U> clusters)
        {
            for (int i = 0; i < clusters.Count; i++)
            {
                double minDistance  = m_maxDistance;
                U clusterI          = clusters[i];

                for (int j = 0; j < clusters.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    U clusterJ = clusters[j];

                    double distance = CalculateMinimumFeatureDistance(clusterJ, clusterI);                    
                    minDistance = Math.Min(minDistance, distance);                    
                }
                clusterI.AmbiguityScore = minDistance;
            }
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
				double massAlignedX = featureX.MassMonoisotopicAligned;
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

        #region IProgressNotifer Members

        public event EventHandler<ProgressNotifierArgs> Progress;

        protected void OnNotify(string message)
        {
            if (Progress != null)
            {
                Progress(this, new ProgressNotifierArgs(message));
            }
        }

        #endregion


        /// <summary>
        /// Determines if the algorithm should allow for clusters to be formed if they are not within tolerance.
        /// </summary>
        public bool ShouldTestClustersWithinTolerance
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if two clusters are within mass, NET, and drift time tolerances
        /// </summary>
        /// <param name="clusterX">One of the two clusters to test</param>
        /// <param name="clusterY">One of the two clusters to test</param>
        /// <returns>True if clusters are within tolerance, false otherwise</returns>
        protected virtual bool AreClustersWithinTolerance(U clusterX, U clusterY)
        {
            if (!ShouldTestClustersWithinTolerance)
                return true;

            // Grab the tolerances
            double massTolerance  = Parameters.Tolerances.Mass;
            double netTolerance   = Parameters.Tolerances.RetentionTime;
            double driftTolerance = Parameters.Tolerances.DriftTime;

            // Calculate differences
            double massDiff  = Math.Abs(Feature.ComputeMassPPMDifference(clusterX.MassMonoisotopicAligned, clusterY.MassMonoisotopicAligned));
            double netDiff   = Math.Abs(clusterX.RetentionTime - clusterY.RetentionTime);
            double driftDiff = Math.Abs(clusterX.DriftTime   - clusterY.DriftTime);

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
        protected virtual bool AreClustersWithinTolerance(T clusterX, T clusterY)
        {
            if (!ShouldTestClustersWithinTolerance)
                return true;

            // Grab the tolerances
            double massTolerance  = Parameters.Tolerances.Mass;
            double netTolerance   = Parameters.Tolerances.RetentionTime;
            double driftTolerance = Parameters.Tolerances.DriftTime;

            // Calculate differences
            double massDiff = Math.Abs(Feature.ComputeMassPPMDifference(clusterX.MassMonoisotopicAligned, clusterY.MassMonoisotopicAligned));
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
