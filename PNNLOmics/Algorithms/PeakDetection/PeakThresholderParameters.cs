
namespace PNNLOmics.Algorithms.PeakDetection
{
    /// <summary>
    /// Parameters used for calculating a threshold
    /// </summary>

    public class PeakThresholderParameters
    {
        /// <summary>
        /// Gets or sets a bool asking if we have applied a thresholding algorithm to the data
        /// </summary>
        public bool isDataThresholded { get; set; }

        /// <summary>
        /// Gets or sets the ratio of highest point at the peak apex to the lowest peak valley.
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
            isDataThresholded = false;
            SignalToShoulderCuttoff = 3;
            ThresholdMethod = "AveragePlusSigma";
            DataNoiseType = InstrumentDataNoiseType.Standard;
        }
    }
}
