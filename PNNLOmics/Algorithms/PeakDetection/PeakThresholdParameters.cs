using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.PeakDetection
{
    public class PeakThresholdParameters
    {
        /// <summary>
        /// Have we applied a thresholding algorithm to the data
        /// </summary>
        public bool isDataThresholded { get; set; }
        
        /// <summary>
        /// Ratio of highest piont at the peak apex to the lowet peak valley.
        /// </summary>
        public float SignalToShoulderCuttoff { get; set; }
        
        /// <summary>
        /// How a threshold line is chosen.
        /// </summary>
        public string ThresholdMethod { get; set; }
        
        /// <summary>
        /// Scan number the data came from.
        /// </summary>
        public int ScanNumber { get; set; }
        
        /// <summary>
        /// Has the noise been removed yet.  Orbitrap data has the noise removed.
        /// </summary>
        public InstrumentDataNoiseType DataNoiseType { get; set; }

        public PeakThresholdParameters()
        {
            this.isDataThresholded = false;
            this.SignalToShoulderCuttoff = 3;
            this.ThresholdMethod = "AveragePlusSigma";
            this.ScanNumber = 0;
            this.DataNoiseType = InstrumentDataNoiseType.Standard;
        }
    }
}
