using System;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.Alignment.SpectralMatching
{
    /// <summary>
    /// Defines an anchor point
    /// </summary>
	[Obsolete("Code moved to MultiAlignWinOmics: MultiAlignCore.Algorithms.Alignment.SpectralMatching")]
    public class SpectralAnchorPoint
    {
        public Peptide   Peptide    { get; set; }
        public MSSpectra Spectrum   { get; set; }
        public double    Net        { get; set; }
        public double    NetAligned { get; set; }
        public double    Mass       { get; set; }
        public double    Mz         { get; set; }
        public double    MzAligned  { get; set; }
        public int       Scan       { get; set; }

        public bool IsTrue { get; set; }
    }
}
