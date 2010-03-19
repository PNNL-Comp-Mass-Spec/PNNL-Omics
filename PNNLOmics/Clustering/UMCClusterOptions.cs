using System;

using PNNLOmics.Data;

namespace PNNLOmics.Clustering
{
    /// <summary>
    /// Class that defines UMC Clustering Options
    /// </summary>
    public class UMCClusterOptions: IBaseData
    {
        #region Constants
        /// <summary>
        /// Mass Tolerance.
        /// </summary>
        private const double CONST_DEFAULT_MASS_TOLERANCE = 20.0;
        /// <summary>
        /// NET tolerance.
        /// </summary>
        private const double CONST_DEFAULT_NET_TOLERANCE = .1;
        /// <summary>
        /// Drift time tolerance.
        /// </summary>
        private const double CONST_DEFAULT_DRIFT_TIME_TOLERANCE = 3.0;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public UMCClusterOptions()
        {
            Clear();
        }

        #region Methods
        /// <summary>
        /// Resets the options to their default values.
        /// </summary>
        public void Clear()
        {
            MassTolerance       = CONST_DEFAULT_MASS_TOLERANCE;
            NETTolerance        = CONST_DEFAULT_NET_TOLERANCE;
            DriftTimeTolerance  = CONST_DEFAULT_DRIFT_TIME_TOLERANCE;
            CentroidType        = UMCClusterCentroidType.Median;
            IntensityType       = UMCClusterIntensityType.Max;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the mass tolerance value.
        /// </summary>
        public double MassTolerance { get; set; }
        /// <summary>
        /// Gets or sets the NET tolerance value.
        /// </summary>
        public double NETTolerance { get; set; }
        /// <summary>
        /// Gets or sets the drift time tolerance.
        /// </summary>
        public double DriftTimeTolerance { get; set; }
        /// <summary>
        /// Gets or sets the type of centroid to use when clustering.
        /// </summary>
        public UMCClusterCentroidType  CentroidType { get; set; }
        /// <summary>
        /// Gets or sets the intensity type to use when clustering.
        /// </summary>
        public UMCClusterIntensityType IntensityType { get; set; }
        #endregion
    }
}
