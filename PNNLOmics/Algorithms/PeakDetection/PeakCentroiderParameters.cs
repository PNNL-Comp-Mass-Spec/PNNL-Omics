
namespace PNNLOmics.Algorithms.PeakDetection
{
    /// <summary>
    /// Parameters used for calculating the centroid mass and apex intensity of a peak
    /// </summary>
    public class PeakCentroiderParameters
    {
        /// <summary>
        /// Gets or sets a bool that asks if the data already been centroided before, such as a stick plot.
        /// </summary>
        public bool IsXYDataCentroided { get; set; }

        /// <summary>
        /// Gets or sets how many points at the top of the peak should we fit to.  3 is the most robust.  others will fail if there are shoulders etc.
        /// </summary>
        public int NumberOfPoints { get; set; } //number of points to fit a parabola at the top to 3,5,7 centered around the max poin

        /// <summary>
        /// Gets or sets a value to be used if the local minimum goes to 0 on both sides of the peak.  If this happens, return this value so signal/noise = signal
        /// </summary>
        public double DefaultShoulderNoiseValue { get; set; }

        /// <summary>
        /// Gets or sets a peak top fit type.  Parabolic or Lorentzian.
        /// </summary>
        public PeakFitType FWHMPeakFitType { get; set; }
        
        /// <summary>
        /// Gets or sets a magic number FWHM when none is present.  Since the data can be loaded as sticks-to-zero data we need to add a width so the deisotoping algorithms will work
        /// </summary>
        public double DefaultFWHMForCentroidedData { get; set; }

        
        /// <summary>
        /// default constructor that loads default values
        /// </summary>
        public PeakCentroiderParameters()
        {
            Clear();
        }

        /// <summary>
        /// default constructor that loads default values
        /// </summary>
        public PeakCentroiderParameters(PeakFitType fitType)
        {
            Clear();
            FWHMPeakFitType = fitType;
        }

        /// <summary>
        /// initializes the parameters to their defaults.
        /// </summary>
        public void Clear()
        {
            IsXYDataCentroided = false;
            NumberOfPoints = 3;//how many point to fit the parabola to
            DefaultShoulderNoiseValue = 1;//if the local minimum goes to 0 on both sides of the peak, return this value so signal/noise = signal
            FWHMPeakFitType = PeakFitType.Lorentzian;//take log first
            DefaultFWHMForCentroidedData = 0.6;
        }
    }

   
}
