﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PNNLOmics.Data.Features
{
    /// <summary>
    /// Class that contains information and references to a cluster of UMC's.
    /// </summary>
    public class UMCCluster: Feature
    {
        private System.Collections.Generic.IList<UMC> m_umcList;

        /// <summary>
        /// Gets or sets the list of UMC's that comprise this cluster.
        /// </summary>
        private IList<UMC> UMCList
        {
            get { return m_umcList; }
            set { m_umcList = value; }
        }

        #region BaseData<UMCCluster> Members
        public void Clear()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
