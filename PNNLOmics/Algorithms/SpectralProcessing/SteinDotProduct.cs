using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Algorithms.SpectralComparisons;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.SpectralProcessing
{
    public class SteinDotProduct: ISpectralComparer
    {
        public double CompareSpectra(Data.MSSpectra spectraX, Data.MSSpectra spectraY)
        {
            List<XYData> a = spectraX.Peaks;
            List<XYData> b = spectraY.Peaks;

            double magX = 0;
            double magY = 0;

            // Then compute the magnitudes of the spectra
            double sum = 0;
            double sumOne = 0;
            double sumTwo = 0;
            for (int i = 0; i < spectraX.Peaks.Count; i++)
            {
                double x = a[i].Y;
                double y = b[i].Y;

                sum += x*y;
                sumOne += (x*x);
                sumTwo += (y*y);
            }

            
            return sum / (sumOne * sumTwo);
        }
    }
}
