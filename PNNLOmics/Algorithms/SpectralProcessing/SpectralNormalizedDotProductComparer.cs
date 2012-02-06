﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.SpectralComparisons
{
    public class SpectralNormalizedDotProductComparer: ISpectralComparer
    {
        /// <summary>
        /// Constructor that keeps the top forty percent of ions in a spectra by default.
        /// </summary>
        public SpectralNormalizedDotProductComparer()
        {
            TopPercent = .4;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="percent">Percentage of most intense ions to keep in the spectra.</param>
        public SpectralNormalizedDotProductComparer(double percent)
        {
            TopPercent = percent;
        }

        /// <summary>
        /// Gets or sets the top spectra values to keep.
        /// </summary>
        public double TopPercent
        {
            get;
            set;
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
            List<XYData> x = xSpectrum.Peaks;
            List<XYData> y = ySpectrum.Peaks;

            double magX = 0;
            double magY = 0;
            int N       = x.Count;

            // Compute magnitudes of x y spectra
            List<double> xIons = new List<double>(N);
            List<double> yIons = new List<double>(N);
            int xTotalNonZero  = 0;
            int yTotalNonZero  = 0;

            for (int i = 0; i < x.Count; i++)
            {
                if (x[i].Y > 0)
                {
                    xTotalNonZero++;
                }
                if (y[i].Y > 0)
                {
                    yTotalNonZero++;
                }

                xIons.Add(x[i].Y);
                yIons.Add(y[i].Y);
            }
            // Find the top ions to keep.
            List<double> xTopIons = new List<double>(N);
            List<double> yTopIons = new List<double>(N);
            xTopIons.AddRange(xIons);
            yTopIons.AddRange(yIons);

            xTopIons.Sort();
            yTopIons.Sort();
            
            int xTop = Math.Max(0, xTopIons.Count - System.Convert.ToInt32(System.Convert.ToDouble(xTotalNonZero) * TopPercent));
            int yTop = Math.Max(0, yTopIons.Count - System.Convert.ToInt32(System.Convert.ToDouble(yTotalNonZero) * TopPercent));

            double xThreshold = xTopIons[xTop];
            double yThreshold = yTopIons[yTop];

            // Then compute the magnitudes of the spectra
            for (int i = 0; i < x.Count; i++)
            {
                double xIon = xIons[i];
                double yIon = yIons[i];

                if (xIon < xThreshold)
                    xIon = 0;

                if (yIon <= yThreshold)
                    yIon = 0;

                magY += yIon * yIon;
                magX += xIon * xIon;
            }
            magY = Math.Sqrt(magY);
            magX = Math.Sqrt(magX);


            // Normalize each component and calculate the dot product.
            double sum = 0;
            for (int i = 0; i < x.Count; i++)
            {
                double xIon = xIons[i];
                double yIon = yIons[i];

                if (xIon < xThreshold)
                    xIon = 0;

                if (yIon <= yThreshold)
                    yIon = 0;

                xIon /= magX;
                yIon /= magY;
                
                sum += (xIon * yIon);
            }
            
            return sum;
        }
        #endregion
    }
}