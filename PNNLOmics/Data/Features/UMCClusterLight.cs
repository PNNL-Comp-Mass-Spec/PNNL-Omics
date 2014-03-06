using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// Basic representation of a group of UMC's observed across datasets.
	/// </summary>
    public class UMCClusterLight :  FeatureLight,
                                    IFeatureCluster<UMCLight>
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public UMCClusterLight()	
		{
			Clear();
		}		
        /// <summary>
        /// Creates a UMC Cluster from the umc, while also connecting them together.
        /// </summary>
        /// <param name="cluster"></param>
        public UMCClusterLight(UMCLight umc)
        {            
            List<UMCLight> umcs  = new List<UMCLight>();            
            
            UMCList				= umcs;
            umc.UMCCluster		= this;             
            Abundance			= umc.Abundance;
            ChargeState			= umc.ChargeState;
            DriftTime			= umc.DriftTime;
            ID					= umc.ID;
            RetentionTime       = umc.NETAligned;
            MassMonoisotopic    = umc.MassMonoisotopicAligned;                        
            MsMsCount           = umc.MsMsCount;
            AddChildFeature(umc);            
        }		
        /// <summary>
        /// Copy constructor. 
        /// </summary>
        /// <param name="cluster"></param>
        public UMCClusterLight(UMCClusterLight cluster)
        {            
            List<UMCLight> umcs     = new List<UMCLight>();
            umcs.AddRange(cluster.UMCList);
            UMCList				    = umcs;            
            Abundance			    = cluster.Abundance;
            ChargeState			    = cluster.ChargeState;
            DriftTime			    = cluster.DriftTime;
            ID					    = cluster.ID;
            MassMonoisotopic        = cluster.MassMonoisotopic;
            MassMonoisotopicAligned = cluster.MassMonoisotopicAligned;
            MsMsCount               = cluster.MsMsCount;
            IdentifiedSpectraCount  = cluster.IdentifiedSpectraCount;
            MeanSpectralSimilarity  = cluster.MeanSpectralSimilarity;
        }
        public double MeanSpectralSimilarity { get; set; }

        public double Tightness
        {
            get;
            set;
        }
        public int MemberCount
        {
            get;
            set;
        }
        public int DatasetMemberCount
        {
            get;
            set;
        }
        public override double MassMonoisotopicAligned
        {
            get
            {
                return MassMonoisotopic;
            }
            set
            {
                MassMonoisotopic = value;
            }
        }


        public double MassStandardDeviation
        {
            get;set;
        }
        public double NetStandardDeviation
        {
            get;
            set;
        }
        

		/// <summary>
		/// Gets or sets the list of UMC's that comprise this cluster.
		/// </summary>
		public List<UMCLight> UMCList { get; set; }

		/// <summary>
		/// Calculates the centroid and other statistics about the cluster.
		/// </summary>
		/// <param name="centroid"></param>
		public void CalculateStatistics(ClusterCentroidRepresentation centroid)
		{
			if (UMCList == null)
				throw new NullReferenceException("The UMC list was not set to an object reference.");

			if (UMCList.Count < 1)
				throw new Exception("No data to compute statistics over.");

			// Lists for holding onto masses etc.
			List<double> net = new List<double>();
			List<double> mass = new List<double>();
			List<double> driftTime = new List<double>();

			// Histogram of representative charge states
			Dictionary<int, int> chargeStates = new Dictionary<int, int>();

			double sumNet = 0;
			double sumMass = 0;
			double sumDrifttime = 0;

            Dictionary<int, int> datasetMembers = new Dictionary<int,int>();
            MemberCount = UMCList.Count;

			foreach (UMCLight umc in UMCList)
			{

				if (umc == null)
					throw new NullReferenceException("A UMC was null when trying to calculate cluster statistics.");

                if (!datasetMembers.ContainsKey(umc.GroupID))
                {
                    datasetMembers.Add(umc.GroupID, 0);
                }
                datasetMembers[umc.GroupID]++;

				net.Add(umc.RetentionTime);
				mass.Add(umc.MassMonoisotopicAligned);
				driftTime.Add(umc.DriftTime);

				sumNet		 += umc.RetentionTime;
				sumMass		 += umc.MassMonoisotopicAligned;
				sumDrifttime += umc.DriftTime;

				// Calculate charge states.
				if (!chargeStates.ContainsKey(umc.ChargeState))
				{
					chargeStates.Add(umc.ChargeState, 1);
				}
				else
				{
					chargeStates[umc.ChargeState]++;
				}
			}
            
            DatasetMemberCount = datasetMembers.Keys.Count;

			int numUMCs = UMCList.Count;
			int median = 0;

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
						this.MassMonoisotopic   = (mass[median] + mass[median - 1]) / 2;
						this.RetentionTime      = (net[median] + net[median - 1]) / 2;
						this.DriftTime          = Convert.ToSingle((driftTime[median] + driftTime[median - 1]) / 2);
					}
					else
					{
						median                  = Convert.ToInt32((numUMCs) / 2);
						this.MassMonoisotopic   = mass[median];
						this.RetentionTime      = net[median];
						this.DriftTime          = Convert.ToSingle(driftTime[median]);
					}
					break;
			}


            List<double> distances = new List<double>();
            double distanceSum     = 0;

            double massDeviationSum = 0;
            double netDeviationSum  = 0;

            foreach (UMCLight umc in UMCList)
            {
                double netValue   = RetentionTime       - umc.RetentionTime;
                double massValue  = MassMonoisotopic    - umc.MassMonoisotopicAligned;
                double driftValue = DriftTime           - Convert.ToSingle(umc.DriftTime);

                massDeviationSum += (massValue*massValue);
                netDeviationSum += (netValue * netValue);

                double distance = Math.Sqrt((netValue * netValue) + (massValue * massValue) + (driftValue * driftValue));               
                distances.Add(distance);
                distanceSum += distance;
            }

            NetStandardDeviation  = Math.Sqrt(netDeviationSum  / Convert.ToDouble(UMCList.Count));
            MassStandardDeviation = Math.Sqrt(massDeviationSum / Convert.ToDouble(UMCList.Count));

            if (centroid == ClusterCentroidRepresentation.Mean)
            {
                Tightness = Convert.ToSingle(distanceSum / UMCList.Count);
            }
            else
            {
                int mid = distances.Count / 2;

                distances.Sort();
                Tightness = Convert.ToSingle(distances[mid]);
            }
			// Calculate representative charge state as the mode.
			int maxCharge = int.MinValue;
			foreach (int charge in chargeStates.Keys)
			{
				if (maxCharge == int.MinValue || chargeStates[charge] > chargeStates[maxCharge])
				{
					maxCharge = charge;
				}
			}
			this.ChargeState = maxCharge;
		}

		#region Overriden Base Methods
		public override string ToString()
		{
			int size = 0;
			if (UMCList != null)
			{
				size = UMCList.Count;
			}
			return "UMC Cluster (size = " + size.ToString() + ") " + base.ToString();
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

			UMCClusterLight cluster = obj as UMCClusterLight;
			if (cluster == null)
				return false;

			bool isBaseEqual = base.Equals(cluster);
			if (!isBaseEqual)
				return false;

			if (UMCList == null && cluster.UMCList != null)
				return false;
			if (UMCList != null && cluster.UMCList == null)
				return false;

			if (UMCList.Count != cluster.UMCList.Count)
				return false;

			foreach (UMCLight umc in UMCList)
			{
				int index = cluster.UMCList.FindIndex(delegate(UMCLight x) { return x.Equals(umc); });
				if (index < 0)
					return false;
			}
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
		/// Resets the object to it's default values.
		/// </summary>
		public override void Clear()
		{
			base.Clear();

			/// 
			/// Clears the list of UMC's, or if null recreates the list.
			/// 
			if (UMCList == null)
				UMCList = new List<UMCLight>();
			else
				UMCList.Clear();

            MsMsCount               = 0;
            IdentifiedSpectraCount  = 0;
		}
		#endregion      
    
        #region IFeatureCluster<UMCLight> Members

        public void AddChildFeature(UMCLight feature)
        {
            UMCList.Add(feature);
        }

        public List<UMCLight> Features
        {
            get { return UMCList; }
        }

        #endregion

    }
}
