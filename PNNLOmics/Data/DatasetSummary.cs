using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Encapsulates the data about a dataset.
    /// </summary>
    public class DatasetSummary
    {

        /// <summary>
        /// Information about a given raw dataset.
        /// </summary>
        public DatasetSummary()
        {
        }

        /// <summary>
        /// Gets or sets the summary data associated with the meta-data.
        /// </summary>
        public Dictionary<int, ScanSummary> ScanMetaData
        {
            get;
            set;
        }
    }
}
