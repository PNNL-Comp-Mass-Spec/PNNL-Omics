using PNNLOmics.Algorithms.Alignment.SpectralMatches;
using PNNLOmics.Algorithms.Alignment.SpectralMatching;
using PNNLOmics.Data;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Alignment
{
    public interface ISpectralAnchorPointMatchValidator
    {
        void ValidateMatches(IEnumerable<SpectralAnchorPointMatch> matches,
                             IEnumerable<Peptide> peptidesA,
                             IEnumerable<Peptide> peptidesB,
                             SpectralOptions options);

    }
}
