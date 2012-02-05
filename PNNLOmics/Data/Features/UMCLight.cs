using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// Representation of a UMC with only basic information
	/// </summary>
    public class UMCLight : FeatureLight, 
                            IFeatureCluster<MSFeatureLight>, 
                            IChildFeature<UMCClusterLight>
	{
		/// <summary>
		/// Default group ID.
		/// </summary>
		private const int DEFAULT_GROUP_ID = -1;
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		public UMCLight()
		{
			Clear();			
		}
		/// <summary>
		/// Gets or sets the UMC Cluster this feature is part of.
		/// </summary>
		public UMCClusterLight UMCCluster	{ get; set; }
        /// <summary>
        /// Gets or sets the list of MS features for the given UMC.
        /// </summary>
        public List<MSFeatureLight> MSFeatures { get; set; }
        /// <summary>
        /// Gets or sets the first scan number the feature was seen in.
        /// </summary>
        public int ScanStart
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the last scan number the feature was seen in.
        /// </summary>
        public int ScanEnd
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the scan for the feature.
        /// </summary>
        public int Scan
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the sum of abundances from all MS features
        /// </summary>
        public long AbundanceSum
        {
            get;
            set;
        }

		#region Overriden Base Methods
		/// <summary>
		/// Returns a basic string representation of the cluster.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "UMCLight Group ID " + GroupID.ToString() + " " + base.ToString();
		}
		/// <summary>
		/// Compares two objects' values to each other.
		/// </summary>
		/// <param name="obj">Object to compare to.</param>
		/// <returns>True if similar, False if not.</returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			UMCLight umc = obj as UMCLight;
			if (umc == null)
				return false;

			if (ID != umc.ID)
				return false;

			bool isBaseEqual = base.Equals(umc);
			if (!isBaseEqual)
				return false;
			
			return true;
		}
		/// <summary>
		/// Computes a hash code for the cluster.
		/// </summary>
		/// <returns>Hashcode as an integer.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		/// <summary>
		/// Clears the UMC and sets it to its default state.
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			GroupID		= DEFAULT_GROUP_ID;
			UMCCluster	= null;
            Scan        = -1;
            ScanEnd     = Scan;
            ScanStart   = Scan;

            if (MSFeatures == null)
                MSFeatures = new List<MSFeatureLight>();
            MSFeatures.Clear();
		}
		#endregion
        /// <summary>
        /// Calculates the centroid and other statistics about the cluster.
        /// </summary>
        /// <param name="centroid"></param>
        public void CalculateStatistics(ClusterCentroidRepresentation centroid)
        {
            if (MSFeatures == null)
                throw new NullReferenceException("The UMC list was not set to an object reference.");

            if (MSFeatures.Count < 1)
                throw new Exception("No data to compute statistics over.");

            // Lists for holding onto masses etc.
            List<double> net        = new List<double>();
            List<double> mass       = new List<double>();
            List<double> driftTime  = new List<double>();

            // Histogram of representative charge states
            Dictionary<int, int> chargeStates = new Dictionary<int, int>();

            double  sumNet          = 0;
            double  sumMass         = 0;
            double  sumDrifttime    = 0;
            long    sumAbundance    = 0;
            int     minScan         = int.MaxValue;
            int     maxScan         = int.MinValue;
            long    maxAbundance    = long.MinValue;
            double representativeMZ = 0;
            foreach (MSFeatureLight feature in MSFeatures)
            {
                if (feature == null)
                    throw new NullReferenceException("A MS feature was null when trying to calculate cluster statistics.");

                if (feature.Abundance > maxAbundance)
                {
                    maxAbundance     = feature.Abundance;
                    Scan             = feature.Scan;
                    ChargeState      = feature.ChargeState;
                    representativeMZ = feature.Mz;
                }

                net.Add(feature.RetentionTime);
                mass.Add(feature.MassMonoisotopic);
                driftTime.Add(feature.DriftTime);

                sumAbundance    += feature.Abundance;
                sumNet          += feature.RetentionTime;
                sumMass         += feature.MassMonoisotopic;
                sumDrifttime    += feature.DriftTime;
                minScan          = Math.Min(feature.Scan, minScan);
                maxScan          = Math.Max(feature.Scan, maxScan);
            }            
            Abundance       = maxAbundance;
            AbundanceSum    = sumAbundance;
            ScanEnd         = maxScan;
            ScanStart       = minScan;
            int numUMCs     = MSFeatures.Count;
            int median      = 0;

            // Calculate the centroid of the cluster.
            switch (centroid)
            {
                case ClusterCentroidRepresentation.Mean:
                    this.MassMonoisotopic   = (sumMass / numUMCs);
                    this.RetentionTime      = (sumNet / numUMCs);
                    this.DriftTime          = Convert.ToSingle(sumDrifttime / numUMCs);
                    break;
                case ClusterCentroidRepresentation.Median:
                    net.Sort();
                    mass.Sort();
                    driftTime.Sort();

                    // If the median index is odd.  Then take the average.
                    if ((numUMCs % 2) == 0)
                    {
                        median                  = Convert.ToInt32(numUMCs / 2);
                        this.RetentionTime      = (net[median] + net[median - 1]) / 2;
                        this.DriftTime          = Convert.ToSingle((driftTime[median] + driftTime[median - 1]) / 2);
                    }
                    else
                    {
                        median                  = Convert.ToInt32((numUMCs) / 2);
                        this.RetentionTime      = net[median];
                        this.DriftTime          = Convert.ToSingle(driftTime[median]);
                    }                                        
                    break;
                    
            }
            if ((numUMCs % 2) == 1)
            {
                MassMonoisotopic = mass[numUMCs / 2];
            }
            else
            {
                MassMonoisotopic = .5 * (mass[numUMCs / 2 - 1] + mass[numUMCs / 2]);
            }

            List<double> distances  = new List<double>();
            double distanceSum      = 0;
            foreach (MSFeatureLight umc in MSFeatures)
            {
                double netValue   = NET - umc.NET;
                double massValue  = MassMonoisotopic - umc.MassMonoisotopic;
                double driftValue = DriftTime - umc.DriftTime;
                double distance   = Math.Sqrt((netValue * netValue) + (massValue * massValue) + (driftValue * driftValue));
                distances.Add(distance);
                distanceSum += distance;
            }

            if (centroid == ClusterCentroidRepresentation.Mean)
            {
                Score = Convert.ToSingle(distanceSum / MSFeatures.Count);
            }
            else
            {
                int mid = distances.Count / 2;

                distances.Sort();
                Score = Convert.ToSingle(distances[mid]);
            }                        
        }


        #region IFeatureCluster<MSFeatureLight> Members

        public void AddChildFeature(MSFeatureLight feature)
        {
            
            MSFeatures.Add(feature);
        }

        public List<MSFeatureLight> Features
        {
            get { return MSFeatures; }
        }

        #endregion

        #region IChildFeature<UMCClusterLight> Members

        public void SetParentFeature(UMCClusterLight parentFeature)
        {
            UMCCluster = parentFeature;
        }

        public UMCClusterLight ParentFeature
        {
            get { return UMCCluster; }
        }

        #endregion
    }
}
