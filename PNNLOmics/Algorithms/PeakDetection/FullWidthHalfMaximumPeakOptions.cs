using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.PeakDetection
{
    /// <summary>
    /// Sets the options for finding the FWHM of the data.  Unassigned is the default
    /// Interpolated occures when there is enough data to interpolate betwen 2 detected data points.  this works well for resolved peaks
    /// Linear Extrapolation.  When there are not enough data points N=2, project a linear line to get the FWHM
    /// Quadratic Extrapolation.  When ther are more than 2 points on a side we can fit a curve.  The curve is fit to the full peak, not just the side to prevent convex curves
    /// </summary>
    public enum FullWidthHalfMaximumPeakOptions
    {
        /// <summary>
        /// no method is set for finding data points at half maximum.  This is used as a default setting.
        /// </summary>
        Unassigned,

        /// <summary>
        /// there are enough data points that we can interpolate between them
        /// </summary>
        Interpolated,

        /// <summary>
        /// we are missing points around the half maximum but we have 2 higher intensity points we can use for extrapolating
        /// </summary>
        LinearExtrapolation,

        /// <summary>
        /// we are missing points around the half maximum but we have more than 2 higher intensity points we can use for extrapolating
        /// </summary>
        QuadraticExtrapolation
    }
}
