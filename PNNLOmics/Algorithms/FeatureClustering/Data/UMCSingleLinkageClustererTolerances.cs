using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.FeatureClustering.Data
{
    /// <summary>
    /// Tolerances for the single linkage clustering algorithm.
    /// </summary>
    public class UMCSingleLinkageClustererTolerances
    {
        /// <summary>
        /// Default drift time value in ms.
        /// </summary>
        public const double CONST_DEFAULT_DRIFT_TIME = 10.0;
        /// <summary>
        /// Default normalized elution time (NET) value as % of total experiment.
        /// </summary>
        public const double CONST_DEFAULT_NET        = .02;
        /// <summary>
        /// Default mass value in parts per million (ppm).
        /// </summary>
        public const double CONST_DEFAULT_MASS       = 15.0;

        #region Properties 
        /// <summary>
        /// Gets or sets the drift time tolerance.
        /// </summary>
        public double DriftTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the mass tolerance.
        /// </summary>
        public double Mass
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the normalized elution time (NET) tolerance.
        /// </summary>
        public double NET
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Resets the tolerances to their default values.
        /// </summary>
        public void Clear()
        {
            DriftTime   = CONST_DEFAULT_DRIFT_TIME;
            Mass        = CONST_DEFAULT_MASS;
            NET         = CONST_DEFAULT_NET;
        }
    }
}
