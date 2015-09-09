using System;
using PNNLOmics.Algorithms.SpectralProcessing;

namespace PNNLOmics.Algorithms.Alignment.SpectralMatches
{

    /// <summary>
    /// Options for computing spectral comparisons
    /// </summary>
	[Obsolete("Code moved to MultiAlignWinOmics: MultiAlignCore.Algorithms.Alignment.SpectralMatching")]
    public class SpectralOptions
    {
        public double MzBinSize { get; set; }
        public double NetTolerance { get; set; }
        public double MzTolerance { get; set; }
        public double TopIonPercent { get; set; }
        public double Fdr { get; set; }
        public double SimilarityCutoff { get; set; }
        public double IdScore { get; set; }
        public SpectralComparison ComparerType { get; set; }
        /// <summary>
        /// Gets or sets the number of required peaks 
        /// </summary>
        public int RequiredPeakCount { get; set; }
    }
}
