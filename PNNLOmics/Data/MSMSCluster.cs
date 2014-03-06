using System;
using System.Collections.Generic;

using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Cluster of MS/MS spectra using matches through features.
    /// </summary>
    public class MSMSCluster : BaseData
    {
        public MSMSCluster()
        {
            Clear();
        }
        /// <summary>
        /// Gets or sets the list of available features.
        /// </summary>
        public List<MSFeatureLight> Features
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the list of spectra associated with the 
        /// </summary>
        public double MeanScore
        {
            get;
            set;
        }

        /// <summary>
        /// Resets the cluster.
        /// </summary>
        public override void Clear()
        {
            Features  = new List<MSFeatureLight>();
            MeanScore = double.NaN;
            ID = -1;
        }
    }
}
