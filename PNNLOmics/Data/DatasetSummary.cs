﻿using System.Collections.Generic;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Encapsulates the data about a dataset.
    /// </summary>
    public class DatasetSummary
    {
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
