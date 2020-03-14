using System;

namespace PNNLOmics.Algorithms.Alignment.LcmsWarp
{
    /// <summary>
    /// Enumeration for possible regression types for LCMS
    /// </summary>
    [Obsolete("Code moved to MultiAlignWinOmics: MultiAlignCore.Algorithms.Alignment.LcmsWarp")]
	public enum LcmsWarpRegressionType
    {
        /// <summary>
        /// Performs piecewise linear regression for all of the sections
        /// </summary>
        Central = 0,
        /// <summary>
        /// Performs an LSQ Regression for all the sections
        /// </summary>
        LeastSquares,
        /// <summary>
        /// Performs a combination of both regression types (Central and LSQ)
        /// </summary>
        Hybrid
    };
}