
namespace PNNLOmics.Algorithms.PeakDetection
{
    /// <summary>
    /// paramaters used for calculating a threshold
    /// </summary>
 
    public class PeakThresholderParameters
    {
        /// <summary>
        /// Gets or sets a bool asking if we have applied a thresholding algorithm to the data
        /// </summary>
        public bool isDataThresholded { get; set; }

        /// <summary>
        /// Gets or sets the ratio of highest piont at the peak apex to the lowet peak valley.
        /// </summary>
        public float SignalToShoulderCuttoff { get; set; }

        /// <summary>
        /// Gets or sets a method for how a threshold line is chosen.
        /// </summary>
        public string ThresholdMethod { get; set; }

        /// <summary>
        /// Gets and sets the noise quality of the data.  Has the noise been removed yet.  Orbitrap data has the noise removed.
        /// </summary>
        public InstrumentDataNoiseType DataNoiseType { get; set; }

        /// <summary>
        /// default constructor that sets default threshold parameters
        /// </summary>
        public PeakThresholderParameters()
        {
            Clear();
        }

        /// <summary>
        /// initializes the default values 
        /// </summary>
        public void Clear()
        {
            this.isDataThresholded = false;
            this.SignalToShoulderCuttoff = 3;
            this.ThresholdMethod = "AveragePlusSigma";
            this.DataNoiseType = InstrumentDataNoiseType.Standard;
        }
    }
}
