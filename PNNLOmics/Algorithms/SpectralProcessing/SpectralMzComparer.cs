using System;
using System.Collections.Generic;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.SpectralComparisons
{
    /// <summary>
    /// Compares two spectra based on their 
    /// </summary>
    public class SpectralMzComparer: ISpectralComparer
    {
        /// <summary>
        /// Gets or sets the mass tolerance in m/z
        /// </summary>
        public double MassTolerance
        {
            get;
            set;
        }

        #region ISpectralComparer Members
        /// <summary>
        /// Compares two spectra finding similar peaks in two masses.
        /// </summary>
        /// <param name="spectraX"></param>
        /// <param name="spectraY"></param>
        /// <returns></returns>
        public double CompareSpectra(MSSpectra xSpectrum, MSSpectra ySpectrum)
        {
            List<XYData> x = xSpectrum.Peaks;
            List<XYData> y = ySpectrum.Peaks;
            
            int i       = 0;
            int j       = 0;
            double eps  = .0000001;
            int Nx      = x.Count;
            int Ny      = y.Count;

            Dictionary<int, int> distanceMap = new Dictionary<int,int>();

            while(i < Nx)
            {
                double massX    = x[i].X;
                double massMax  = 0;
                double massMin  = 0;

                if (i + 1 >= Nx)
                {
                    massMax = MassTolerance;
                }
                else
                {
                    massMax = x[i + 1].X - massX - eps;
                }

                if (i == 0)
                {
                    massMin = MassTolerance;
                }
                else
                {
                    massMin = massX - x[i - 1].X - eps;
                }

                double mzTol     = Math.Min(Math.Min(MassTolerance, massMax), massMin);
                double bestDelta = mzTol; 
                int bestMatch    = -1;

                while (j < Ny)
                {
                    double massY        = y[j].X;
                    double deltaMass    = massX - massY;
                    double absDelta     = Math.Abs(deltaMass);

                    if (absDelta >= bestDelta && deltaMass < 0)
                    {
                        break;
                    }
                    else
                    {
                        bestMatch = j;
                    }
                    j = j + 1;
                }   

                if (bestMatch >= 0)
                {
                    distanceMap.Add(i, j);                       
                }
                i = i + 1;
            }

            // Score
            int nx      = x.Count;
            int ny      = y.Count;
            int matches = distanceMap.Keys.Count;

            return Convert.ToDouble(matches) / Convert.ToDouble(Math.Max(nx, ny));            
        }
        #endregion
    }
}
