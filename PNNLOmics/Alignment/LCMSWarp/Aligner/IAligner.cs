using PNNLOmics.Data.Features;
using System.Collections.Generic;
using PNNLOmics.Data.MassTags;

namespace Aligner
{
    public interface IAligner
    {
        AlignmentData Align(IEnumerable<UMCLight> baseline, IEnumerable<UMCLight> features);

        AlignmentData Align(IEnumerable<MassTagLight> baseline, IEnumerable<UMCLight> features);
    }
}
