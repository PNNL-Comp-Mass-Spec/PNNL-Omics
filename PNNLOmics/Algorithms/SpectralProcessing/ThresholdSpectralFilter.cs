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
        /// <param name="spectrum"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public MSSpectra Threshold(MSSpectra spectrum, double threshold)
        {
            MSSpectra filteredSpectrum = new MSSpectra();
            var filteredPeaks = (from peak in spectrum.Peaks
                                 where peak.Y > threshold
                                 orderby peak.X ascending
                                 select peak).ToList();

            foreach (XYData peak in filteredPeaks)
            {
                XYData data = new XYData(peak.X, peak.Y);
                filteredSpectrum.Peaks.Add(data);
            }

            return filteredSpectrum;
        }

        #endregion
    }
}
