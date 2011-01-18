
namespace PNNLOmics.Algorithms.PeakDetection
{

    /// <summary>
    /// Parameters for centroiding and thresholding raw data
    /// </summary>
    public class KronewitterPeakDetectorParameters
    {
        /// <summary>
        /// Parameters used for centroiding algorithms
        /// </summary>
        public PeakCentroiderParameters CentroidParameters { get; set; }
        
        /// <summary>
        /// Parameters used for thresholding algorithms
        /// </summary>
        public PeakThresholderParameters ThresholdParameters { get; set; }
        
        /// <summary>
        /// this default constructor contains both centroiding and thresholding parameter objects.
        /// </summary>
        public KronewitterPeakDetectorParameters()
        {
            this.CentroidParameters = new PeakCentroiderParameters();
            this.ThresholdParameters = new PeakThresholderParameters();
        }
    }


   
}
