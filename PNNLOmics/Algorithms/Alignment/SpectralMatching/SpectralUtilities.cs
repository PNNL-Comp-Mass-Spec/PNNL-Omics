﻿using System;
using System.Collections.Generic;
using PNNLOmics.Algorithms.SpectralProcessing;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.Alignment.SpectralMatches
{
    public static class SpectralUtilities
    {
         static SpectralUtilities()
        {
            ShouldLogScale = false;
        }

        public static bool ShouldLogScale { get; set; }

        public static MSSpectra GetSpectrum(ISpectraProvider reader, int scan, int group, double mzTolerance = .5)
        {
            var summary = new ScanSummary();
            var spectrum   = reader.GetSpectrum(scan, group, 2, out summary, true);

            if (ShouldLogScale)
            {
                foreach (var peak in spectrum.Peaks)
                {
                    peak.Y = Math.Log(peak.Y, 2);
                }
            }            
            return spectrum;
        }

        public static MSSpectra GetSpectra(double mzTolerance,
                                           double percent,
                                           ISpectraFilter filter,
                                           ISpectraProvider readerY,                                           
                                           int scany,
                                           int numberRequiredPeaks)
        {
            var spectrum = GetSpectrum(readerY,
                                              scany,
                                              0,
                                              mzTolerance);

            if (spectrum.Peaks.Count < numberRequiredPeaks)
                return null;

            spectrum.Peaks = filter.Threshold(spectrum.Peaks, percent);
            spectrum.Peaks = XYData.Bin(spectrum.Peaks,
                                                0,
                                                2000,
                                                mzTolerance);
            return spectrum;
        }
    }
}
