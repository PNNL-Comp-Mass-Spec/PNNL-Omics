using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    public class PrecursorInfo
    { 
        /// <summary>
        /// MS=0 or MSMS=1
        /// </summary>
        public int MSLevel { get; set; }

        /// <summary>
        /// Charge of the precursor Ion
        /// </summary>
        public int PrecursorCharge { get; set; }

        /// <summary>
        /// PrecursorScanNumber
        /// </summary>
        public int PrecursorScan { get; set; }

        /// <summary>
        /// Precursor m/z
        /// </summary>
        public double PrecursorMZ { get; set; }

        /// <summary>
        /// Precursor intensity
        /// </summary>
        public float PrecursorIntensity { get; set; }

        public PrecursorInfo()
        {
            PrecursorMZ = 0;
            PrecursorIntensity = 0;
            MSLevel = -1;
            PrecursorCharge = -1;
            PrecursorScan = -1;
        }
    }
}
