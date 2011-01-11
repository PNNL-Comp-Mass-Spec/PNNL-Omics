
namespace PNNLOmics.Algorithms.PeakDetection
{

    //TODO: scott - add xml comments
    /// <summary>
    /// 
    /// </summary>
    public class KronewitterPeakDetectorParameters
    {

        //TODO: scott - add xml comments
        /// <summary>
        /// 
        /// </summary>
        public PeakCentroiderParameters CentroidParameters { get; set; }
        //TODO: scott - add xml comments
        public PeakThresholdParameters ThresholdParameters { get; set; }
        //TODO: scott - add xml comments -- this does not belong in here.
        public int ScanNumber { get; set; }//so we can attach a scan number to each peak   


        //TODO: scott - add xml comments
        public KronewitterPeakDetectorParameters()
        {
            this.CentroidParameters = new PeakCentroiderParameters();
            this.ThresholdParameters = new PeakThresholdParameters();
            this.ScanNumber = 0;
        }
    }


    //TODO: scott - move this into a separate file.
    /// <summary>
    /// Sets the options for finding the FWHM of the data.  Unassigned is the default
    /// Interpolated occures when there is enough data to interpolate betwen 2 detected data points.  this works well for resolved peaks
    /// Linear Extrapolation.  When there are not enough data points N=2, project a linear line to get the FWHM
    /// Quadratic Extrapolation.  When ther are more than 2 points on a side we can fit a curve.  The curve is fit to the full peak, not just the side to prevent convex curves
    /// </summary>
    public enum FWHMPointFindingOptions
    {
        //TODO: scott - add xml comments
        Unassigned,
        //TODO: scott - add xml comments
        Interpolated,
        //TODO: scott - add xml comments
        LinearExtrapolation,
        //TODO: scott - add xml comments
        QuadraticExtrapolation
    }
}
