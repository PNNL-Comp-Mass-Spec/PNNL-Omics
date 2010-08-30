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
            GroupID         = cluster.GroupID;
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
            Corrected       = umc.Corrected;
            DriftTime       = umc.DriftTime;
            ElutionTime     = umc.ElutionTime;
            GroupID         = umc.GroupID;
            ID              = umc.ID;
            MassMonoisotopic        = umc.MassMonoisotopic;
            MassMonoisotopicAligned = umc.MassMonoisotopicAligned;
            MZ              = umc.MZ;
            MZCorrected     = umc.MZCorrected;
            NETAligned      = umc.NET; 
            ScanLC          = umc.ScanLC;
            ScanLCAligned   = umc.ScanLCAligned; 
            Suspicious      = umc.Suspicious;

            
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
        public void Clear()
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
