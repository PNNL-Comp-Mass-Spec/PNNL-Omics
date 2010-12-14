using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Data
{
    //TODO: Change name of processed peak to something more specific?
    public class ProcessedPeak:Peak
    {
        //TODO set up new peak object so we can return a list
        //TODO break up regions into functions
        
        /// <summary>
        /// the Scan Number this peak was found in.
        /// </summary>
        public int ScanNumber {get;set;}

        /// <summary>
        /// the lower of the two local minima (lowest between the minima lower in mass and the minima higher in mass)
        /// </summary>
        public double LocalLowestMinimaHeight { get; set; }

        /// <summary>
        /// the closes minima on the lower mass side of the peak has this index.
        /// </summary>
        public int MinimaOfLowerMassIndex { get; set; }

        /// <summary>
        /// the closes minima on the higher mass side of the peak has this index.
        /// </summary>
        public int MinimaOfHigherMassIndex { get; set; }

        /// <summary>
        /// Maxintensity/noise threshold (Xsigma above the average noise)
        /// </summary>
        public double SignalToNoiseGlobal { get; set; }

        /// <summary>
        /// Maxintensity/local lowest minima
        /// </summary>
        public double SignalToNoiseLocalMinima { get; set; }

        /// <summary>
        /// Maxintensity/average of local minima
        /// </summary>
        public double SignalToBackground { get; set; }

        public override void Clear()
        {
            this.ScanNumber = 0;
            this.LocalLowestMinimaHeight = 0;
            this.MinimaOfHigherMassIndex = 0;
            this.MinimaOfLowerMassIndex = 0;
        }
    }
}
