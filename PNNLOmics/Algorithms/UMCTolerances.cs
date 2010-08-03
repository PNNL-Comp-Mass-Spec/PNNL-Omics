/*////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * 
 * Name:    UMC Tolerances
 * File:    UMCTolerances.cs
 * Author:  Brian LaMarche 
 * Purpose: Tolerances for UMC data.
 * Date:    5-19-2010
 * Revisions:
 *          05-19-2010 - BLL - Created clustering class and algorithm.
 *          08-02-2010 - BLL - Moved the tolerances out of the feature clustering namespace and into 
 *                             a more generic namespace since they are not specific to the UMC clustering
 *                             algorithm.
 ////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
using System;

namespace PNNLOmics.Algorithms
{
    /// <summary>
    /// Tolerances for the single linkage clustering algorithm.
    /// </summary>
    public class UMCTolerances
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
