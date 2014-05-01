using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// Representation of a UMC with only basic information
	/// </summary>
    public class UMCLight : FeatureLight,
                            IFeatureCluster<MSFeatureLight>,        // This allows for ms features
                            IFeatureCluster<UMCLight>,              // This allows for labeled devlepment
                            IChildFeature<UMCClusterLight>
	{
		/// <summary>
		/// Default group ID.
		/// </summary>
		private const int DEFAULT_GROUP_ID = -1;
        private List<UMCLight> m_umcList;
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		public UMCLight()
		{
			Clear();			
		}

        public UMCLight(UMCLight feature)
        {
            HPositive                          = new Dictionary<int, double>();
            HNegative                          = new Dictionary<int, double>();
            Abundance                          = feature.Abundance;
            AbundanceSum                       = feature.AbundanceSum;
            AmbiguityScore                     = feature.AmbiguityScore;
            AverageDeconFitScore               = feature.AverageDeconFitScore;
            AverageInterferenceScore           = feature.AverageInterferenceScore;           
            ChargeState                        = feature.ChargeState;
            ClusterID                          = feature.ClusterID;
            ConformationFitScore               = feature.ConformationFitScore;
            ConformationID                     = feature.ConformationID;
            DriftTime                          = feature.DriftTime;
            GroupID                            = feature.GroupID; 
            ID                                 = feature.ID;
            MassMonoisotopic                   = feature.MassMonoisotopic;
            MassMonoisotopicAligned            = feature.MassMonoisotopicAligned;
            Mz                                 = feature.Mz;
            Net                                = feature.Net;
            NetAligned                         = feature.NetAligned;
            RetentionTime                      = feature.RetentionTime;
            SaturatedMemberCount               = feature.SaturatedMemberCount ;
            Scan                               = feature.Scan;
            ScanAligned                        = feature.ScanAligned;
            ScanEnd                            = feature.ScanEnd;
            ScanStart                          = feature.ScanStart;
            Score                              = feature.Score;
            SpectralCount                      = feature.SpectralCount;
            Tightness                          = feature.Tightness;
            MsMsCount                          = feature.MsMsCount;

            /// Charge state and Isotopic Chromatograms
            ChargeStateChromatograms           = new Dictionary<int, Chromatogram>();    
            IsotopeChromatograms               = new Dictionary<int, List<Chromatogram>>();
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
        public int ScanAligned
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
        public int SpectralCount
        {
            get;
            set;
        }
        public double Mz
        {
            get;
            set;
        }
        public double Tightness
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the trailing width of the XIC profile based on charge state.
        /// </summary>
        public Dictionary<int, double> HPositive
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the value of the leading width of the XIC profile based on charge state.
        /// </summary>
        public Dictionary<int, double> HNegative
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the chromatograms based on charge state.
        /// </summary>
        public Dictionary<int, Chromatogram> ChargeStateChromatograms
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the chromatograms for each isotope for a given charge state.
        /// </summary>
        public Dictionary<int, List<Chromatogram>> IsotopeChromatograms
        {
            get;
            set;
        }
        public double MeanIsotopicRsquared
        {
            get;
            set;
        }
        public double MeanChargeStateRsquared
        {
            get;
            set;
        }
        

        #region IMS Data Members
        public double AverageInterferenceScore
        {
            get;
            set;
        }
        public double ConformationFitScore
        {
            get;
            set;
        }
        public double AverageDeconFitScore
        {
            get;
            set;
        }
        public int SaturatedMemberCount
        {
            get;
            set;
        }
        public int ClusterID
        {
            get;
            set;
        }
        public int ConformationID
        {
            get;
            set;
        }
        #endregion


        #region Overriden Base Methods
        /// <summary>
		/// Returns a basic string representation of the cluster.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "UMCLight Group ID " + GroupID + " " + base.ToString();
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

			var umc = obj as UMCLight;
			if (umc == null)
				return false;

			if (ID != umc.ID)
				return false;

			var isBaseEqual = base.Equals(umc);
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

            ChargeStateChromatograms = new Dictionary<int, Chromatogram>();
            IsotopeChromatograms     = new Dictionary<int, List<Chromatogram>>();

            HPositive   = new Dictionary<int, double>();
            HNegative   = new Dictionary<int, double>();
			GroupID		= DEFAULT_GROUP_ID;
			UMCCluster	= null;
            Scan        = -1;
            ScanEnd     = Scan;
            ScanStart   = Scan;
            Tightness   = double.NaN;
            MsMsCount   = 0;

            if (MSFeatures == null)
                MSFeatures = new List<MSFeatureLight>();
            MSFeatures.Clear();

            if (m_umcList == null)
                m_umcList = new List<UMCLight>();
            m_umcList.Clear();
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
                throw new Exception("No data in feature to compute statistics over.");

            // Lists for holding onto masses etc.
            var net        = new List<double>();
            var mass       = new List<double>();
            var driftTime  = new List<double>();

            // Histogram of representative charge states
            var chargeStates = new Dictionary<int, int>();

            double  sumNet          = 0;
            double  sumMass         = 0;
            double  sumDrifttime    = 0;
            long    sumAbundance    = 0;
            var     minScan         = int.MaxValue;
            var     maxScan         = int.MinValue;
            var    maxAbundance    = long.MinValue;
            double representativeMZ = 0;
            foreach (var feature in MSFeatures)
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
                sumMass         += feature.MassMonoisotopicAligned;
                sumDrifttime    += feature.DriftTime;
                minScan          = Math.Min(feature.Scan, minScan);
                maxScan          = Math.Max(feature.Scan, maxScan);
            }            
            Abundance       = maxAbundance;
            AbundanceSum    = sumAbundance;
            ScanEnd         = maxScan;
            ScanStart       = minScan;
            var numUMCs     = MSFeatures.Count;
            var median      = 0;

            // Calculate the centroid of the cluster.
            switch (centroid)
            {
                case ClusterCentroidRepresentation.Mean:
                    MassMonoisotopic   = (sumMass / numUMCs);
                    RetentionTime      = (sumNet / numUMCs);
                    DriftTime          = Convert.ToSingle(sumDrifttime / numUMCs);
                    break;
                case ClusterCentroidRepresentation.Median:
                    net.Sort();
                    mass.Sort();
                    driftTime.Sort();

                    // If the median index is odd.  Then take the average.
                    if ((numUMCs % 2) == 0)
                    {
                        median                  = Convert.ToInt32(numUMCs / 2);
                        RetentionTime      = (net[median] + net[median - 1]) / 2;
                        DriftTime          = Convert.ToSingle((driftTime[median] + driftTime[median - 1]) / 2);
                    }
                    else
                    {
                        median                  = Convert.ToInt32((numUMCs) / 2);
                        RetentionTime      = net[median];
                        DriftTime          = Convert.ToSingle(driftTime[median]);
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

            var distances  = new List<double>();
            double distanceSum      = 0;
            foreach (var umc in MSFeatures)
            {
                var netValue   = Net - umc.Net;
                var massValue  = MassMonoisotopic - umc.MassMonoisotopicAligned;
                var driftValue = DriftTime - umc.DriftTime;
                var distance   = Math.Sqrt((netValue * netValue) + (massValue * massValue) + (driftValue * driftValue));
                distances.Add(distance);
                distanceSum += distance;
            }

            if (centroid == ClusterCentroidRepresentation.Mean)
            {
                Score = Convert.ToSingle(distanceSum / MSFeatures.Count);
                Tightness = Score;
            }
            else
            {
                var mid = distances.Count / 2;

                distances.Sort();
                Score       = Convert.ToSingle(distances[mid]);
                Tightness   = Score;
            }
            Mz = representativeMZ;
        }

        #region IFeatureCluster<MSFeatureLight> Members

        public void AddChildFeature(MSFeatureLight feature)
        {            
            feature.SetParentFeature(this);
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

        public void AddChildFeature(UMCLight feature)
        {
            m_umcList.Add(feature);
            feature.MSFeatures.ForEach(AddChildFeature);
        }

        List<UMCLight> IFeatureCluster<UMCLight>.Features
        {
            get { return m_umcList; }
        }

    }
}
