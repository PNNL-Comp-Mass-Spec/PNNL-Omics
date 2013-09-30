using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.SpectralComparisons
{
    public class SpectralPeakCountComparer: ISpectralComparer
    {
        /// <summary>
        /// Constructor that keeps the top forty percent of ions in a spectra by default.
        /// </summary>
        public SpectralPeakCountComparer()
        {
        
        }        

        #region ISpectralComparer Members
        /// <summary>
        /// Computes the dot product of two spectra.
        /// </summary>
        /// <param name="spectraX">Spectrum X</param>
        /// <param name="spectraY">Spectrum Y</param>
        /// <returns>Normalized Dot Product</returns>
        public double CompareSpectra(MSSpectra xSpectrum, MSSpectra ySpectrum)
        {
            List<XYData> a = xSpectrum.Peaks;
            List<XYData> b = ySpectrum.Peaks;
                                               
            // Then compute the magnitudes of the spectra
            double sum  = 0;
            int xc = 0;
            int yc = 0;


            for (int i = 0; i < xSpectrum.Peaks.Count; i++)
            {
                double x = a[i].Y;
                double y = b[i].Y;

                if (x > 0)
                {
                    xc++;
                }

                if (y > 0)
                {
                    yc++;
                }

                if (x > 0 && y > 0)
                {
                    sum++;
                }
            }
            return Convert.ToDouble(sum) / Convert.ToDouble(xc + yc);
        }
        #endregion
    }
}
