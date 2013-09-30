using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.SpectralProcessing
{
    /// <summary>
    /// Converts a spectrum into a binary one
    /// </summary>
    public class BinarySpectraNormalizer : ISpectralNormalizer
    {
        #region ISpectralNormalizer Members
        /// <summary>
        /// Converts a spectra into a binary one.
        /// </summary>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        public MSSpectra Normalize(MSSpectra spectrum)
        {
            MSSpectra filteredSpectrum = new MSSpectra();

            foreach (XYData peak in spectrum.Peaks)
            {
                XYData data = new XYData(peak.X, 1);
                filteredSpectrum.Peaks.Add(data);
            }

            return filteredSpectrum;
        }
        #endregion
    }
}
