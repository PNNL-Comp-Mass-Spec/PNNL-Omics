using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.PeakDetection
{
    public class PeakDetectorParameters
    {
        public PeakCentroidParameters CentroidParameters { get; set; }
        public PeakThresholdParameters ThresholdParameters { get; set; }
        public int ScanNumber { get; set; }//so we can attach a scan number to each peak   

        public PeakDetectorParameters()
        {
            this.CentroidParameters = new PeakCentroidParameters();
            this.ThresholdParameters = new PeakThresholdParameters();
            this.ScanNumber = 0;
        }
    }

    /// <summary>
    /// Sets the options for finding the FWHM of the data.  Unassigned is the default
    /// Interpolated occures when there is enough data to interpolate betwen 2 detected data points.  this works well for resolved peaks
    /// Linear Extrapolation.  When there are not enough data points N=2, project a linear line to get the FWHM
    /// Quadratic Extrapolation.  When ther are more than 2 points on a side we can fit a curve.  The curve is fit to the full peak, not just the side to prevent convex curves
    /// </summary>
    public enum FWHMPointFindingOptions
    {
        Unassigned,
        Interpolated,
        LinearExtrapolation,
        QuadraticExtrapolation
    }
}
