
namespace PNNLOmics.Algorithms.PeakDetection
{
    //TODO: scott - add xml comments. 
    public class PeakThresholdParameters
    {
        //TODO: scott - add xml comments. Gets or sets 
        /// <summary>
        /// Gets or sets Have we applied a thresholding algorithm to the data
        /// </summary>
        public bool isDataThresholded { get; set; }

        //TODO: scott - add xml comments. Gets or sets 
        /// <summary>
        /// Ratio of highest piont at the peak apex to the lowet peak valley.
        /// </summary>
        public float SignalToShoulderCuttoff { get; set; }

        //TODO: scott - add xml comments. Gets or sets 
        /// <summary>
        /// How a threshold line is chosen.
        /// </summary>
        public string ThresholdMethod { get; set; }

        //TODO: scott - add xml comments. Gets or sets 
        /// <summary>
        /// Scan number the data came from.
        /// </summary>
        public int ScanNumber { get; set; }

        //TODO: scott - add xml comments. Gets or sets 
        /// <summary>
        /// Has the noise been removed yet.  Orbitrap data has the noise removed.
        /// </summary>
        public InstrumentDataNoiseType DataNoiseType { get; set; }


        //TODO: scott - add xml comments. 
        /// <summary>
        /// 
        /// </summary>
        public PeakThresholdParameters()
        {
            Clear();
        }

        //TODO: scott - add xml comments. Gets or sets 
        public void Clear()
        {
            this.isDataThresholded = false;
            this.SignalToShoulderCuttoff = 3;
            this.ThresholdMethod = "AveragePlusSigma";
            this.ScanNumber = 0;
            this.DataNoiseType = InstrumentDataNoiseType.Standard;
        }
    }
}
