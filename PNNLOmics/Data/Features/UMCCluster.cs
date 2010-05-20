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
        private IList<UMC> m_umcList;

        /// <summary>
        /// Default constructor for a cluster.
        /// </summary>
        public UMCCluster()
        {
            m_umcList = new List<UMC>();
        }

        #region Properties
        /// <summary>
        /// Gets or sets the list of UMC's that comprise this cluster.
        /// </summary>
        public IList<UMC> UMCList
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
    }
}
