using System;
using System.Collections.Generic;
using System.Text;

namespace PNNLOmics.Data.Features
{
    /// <summary>
    /// Class that contains information and references to a cluster of UMC's.
    /// </summary>
    public class UMCCluster: Feature
    {
        /// <summary>
        /// List of UMC's that define the cluster.
        /// </summary>
        private List<UMC> m_umcList;

        /// <summary>
        /// Default constructor for a cluster.
        /// </summary>
        public UMCCluster()
        {
            m_umcList   = new List<UMC>();
            ID          = -1;
        }
        /// <summary>
        /// Copy constructor. 
        /// </summary>
        /// <param name="cluster"></param>
        public UMCCluster(UMCCluster cluster)
        {            
            List<UMC> umcs  = new List<UMC>();
            umcs.AddRange(cluster.UMCList);
            m_umcList       = umcs;
            
            Abundance       = cluster.Abundance;
            ChargeState     = cluster.ChargeState;
            IsDaltonCorrected       = cluster.IsDaltonCorrected;
            DriftTime       = cluster.DriftTime;
            ElutionTime     = cluster.ElutionTime;            
            ID              = cluster.ID;
            MassMonoisotopic        = cluster.MassMonoisotopic;
            MassMonoisotopicAligned = cluster.MassMonoisotopicAligned;
            MZ              = cluster.MZ;
            MZCorrected     = cluster.MZCorrected;
            NETAligned      = cluster.NET; 
            ScanLC          = cluster.ScanLC;
            ScanLCAligned   = cluster.ScanLCAligned; 
            IsSuspicious      = cluster.IsSuspicious;            
        }
        
        /// <summary>
        /// Creates a UMC Cluster from the umc, while also connecting them together.
        /// </summary>
        /// <param name="cluster"></param>
        public UMCCluster(UMC umc)
        {            
            List<UMC> umcs  = new List<UMC>();
            umcs.Add(umc);

            m_umcList       = umcs;
            umc.UmcCluster  = this; 
            
            Abundance       = umc.Abundance;
            ChargeState     = umc.ChargeState;
            IsDaltonCorrected = umc.IsDaltonCorrected;
            DriftTime       = umc.DriftTime;
            ElutionTime     = umc.ElutionTime;            
            ID              = umc.ID;
            MassMonoisotopic        = umc.MassMonoisotopic;
            MassMonoisotopicAligned = umc.MassMonoisotopicAligned;
            MZ              = umc.MZ;
            MZCorrected     = umc.MZCorrected;
            NETAligned      = umc.NET; 
            ScanLC          = umc.ScanLC;
            ScanLCAligned   = umc.ScanLCAligned; 
            IsSuspicious      = umc.IsSuspicious;            
        }

        #region Properties
        /// <summary>
        /// Gets or sets the list of UMC's that comprise this cluster.
        /// </summary>
        public List<UMC> UMCList
        {
            get { return m_umcList; }
            set { m_umcList = value; }
        }
        #endregion

        #region BaseData<UMCCluster> Members
        /// <summary>
        /// Resets the object to it's default values.
        /// </summary>
        public override void Clear()
        {
            base.Clear();

            /// 
            /// Clears the list of UMC's, or if null recreates the list.
            /// 
            if (m_umcList == null)
                m_umcList = new List<UMC>();
            else
                m_umcList.Clear();
        }
        #endregion

        /// <summary>
        /// Calculates the centroid and other statistics about the cluster.
        /// </summary>
        /// <param name="centroid"></param>
        public void CalculateStatistics(ClusterCentroidRepresentation centroid)
        {
            if (UMCList == null)
                throw new NullReferenceException("The UMC list was not set to an object reference.");

            if (UMCList.Count < 1)
                throw new Exception("No data to in cluster to compute statistics over.");

            // Lists for holding onto masses etc.
            List<double> net        = new List<double>();
            List<double> mass       = new List<double>();
            List<double> driftTime  = new List<double>();

            // Histogram of representative charge states
            Dictionary<int, int> chargeStates = new Dictionary<int, int>(); 

            double sumNet       = 0;
            double sumMass      = 0;
            double sumDrifttime = 0;

            foreach (UMC umc in UMCList)
            {

                if (umc == null)
                    throw new NullReferenceException("A UMC was null when trying to calculate cluster statistics.");

                net.Add(umc.NETAligned);
                mass.Add(umc.MassMonoisotopicAligned);
                driftTime.Add(umc.DriftTime);

                sumNet          += umc.NETAligned;
                sumMass         += umc.MassMonoisotopicAligned;
                sumDrifttime    += umc.DriftTime;

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
            
            int numUMCs = UMCList.Count;
            int median = 0;

            // Calculate the centroid of the cluster.
            switch (centroid)
            {
                case ClusterCentroidRepresentation.Mean:
                    this.MassMonoisotopic   = (sumMass / numUMCs);
                    this.NET                = (sumNet / numUMCs);
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
                        this.NET                = (net[median] + net[median - 1]) / 2;
                        this.DriftTime          = Convert.ToSingle((driftTime[median] + driftTime[median - 1]) / 2);
                    }
                    else
                    {
                        median                  = Convert.ToInt32((numUMCs) / 2);
                        this.MassMonoisotopic   = mass[median];
                        this.NET                = net[median];
                        this.DriftTime          = Convert.ToSingle(driftTime[median]);
                    }                    
                    break;
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

            UMCCluster cluster = obj as UMCCluster;
            if (cluster == null)
                return false;

			bool isBaseEqual = base.Equals(cluster);
            if (!isBaseEqual)
                return false;

            if (UMCList == null && cluster.UMCList != null)
                return  false;
            if (UMCList != null && cluster.UMCList == null)
                return false;
            
            if (UMCList.Count != cluster.UMCList.Count)
                return false;
            
            foreach(UMC umc in UMCList)
            {
                int index = cluster.UMCList.FindIndex(delegate(UMC x) { return x.Equals(umc); });
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
        #endregion
    }
}
