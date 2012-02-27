using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    public class PrecursorInfo
    { 
        /// <summary>
        /// Peak coresponding to the Precursor selected for fragmentation
        /// </summary>
        public PNNLOmics.Data.XYData PrecursorXYData { get; set; }

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

        public PrecursorInfo()
        {
            PrecursorXYData = new PNNLOmics.Data.XYData(-1, -1);
            MSLevel = -1;
            PrecursorCharge = -1;
            PrecursorScan = -1;
        }
    }
}
