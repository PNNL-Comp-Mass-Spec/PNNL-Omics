using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.SpectralProcessing
{
    /// <summary>
    /// Filters spectra based on raw thresholds
    /// </summary>
    public class ThresholdSpectralFilter : ISpectraFilter
    {
        #region ISpectraFilter Members
        /// <summary>
        /// Filters a spectra based on a raw intensity.
        /// </summary>
        public List<XYData> Threshold(List<XYData> peaks, double threshold)
        {
            
            var filteredPeaks = (from peak in peaks
                                 where peak.Y > threshold
                                 orderby peak.X ascending
                                 select peak).ToList();
            return filteredPeaks;
        }

        #endregion
    }
}
