using System;
using System.Collections.Generic;
using PNNLOmics.Algorithms.Alignment.SpectralMatches;
using PNNLOmics.Algorithms.Alignment.SpectralMatching;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.Alignment
{
	[Obsolete("Code moved to MultiAlignWinOmics: MultiAlignCore.Algorithms.Alignment")]
    public interface ISpectralAnchorPointMatchValidator
    {
        void ValidateMatches(IEnumerable<SpectralAnchorPointMatch> matches,
                             IEnumerable<Peptide> peptidesA,
                             IEnumerable<Peptide> peptidesB,
                             SpectralOptions options);

    }
}
