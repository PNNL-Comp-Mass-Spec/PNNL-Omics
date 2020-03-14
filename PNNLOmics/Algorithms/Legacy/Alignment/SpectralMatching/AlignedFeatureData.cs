using System;
using System.Collections.Generic;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment.SpectralMatching
{
    /// <summary>
    /// Holds data for features that were aligned.
    /// </summary>
	[Obsolete("Code moved to MultiAlignWinOmics: MultiAlignCore.Algorithms.Alignment.SpectralMatching")]
    public class AlignedFeatureData
    {
        /// <summary>
        /// Gets or sets the baseline features
        /// </summary>
        public Tuple<List<UMCLight>, List<MSFeatureLight>> Baseline { get; set; }
        /// <summary>
        /// Gets or sets the alignee features
        /// </summary>
        public Tuple<List<UMCLight>, List<MSFeatureLight>> Alignee { get; set; }
    }
}
