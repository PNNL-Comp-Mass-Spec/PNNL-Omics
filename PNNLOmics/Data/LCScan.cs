using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Encapsulates Scan information of the LC dimension
    /// </summary>
    public class LCScan: Scan
    {
        /// <summary>
        /// List of all IMS Scans available in the dataset for this scan.
        /// </summary>
        private IList<IMSScan> m_imsScanList;
        /// <summary>
        /// Normalized Elution Time
        /// </summary>
        private float m_NET;

        /// <summary>
        /// Gets or sets the normalized elution time (NET) of the LC Scan referenced to another LC Scan from another dataset.
        /// </summary>
        public float NET
        {
            get { return m_NET; }
            set { m_NET = value; }
        }

        /// <summary>
        /// Gets or sets the list of IMS Scans if data contains IMS data.
        /// </summary>
        public IList<IMSScan> ImsScanList
        {
            get { return m_imsScanList; }
            set { m_imsScanList = value; }
        }
    }
}
