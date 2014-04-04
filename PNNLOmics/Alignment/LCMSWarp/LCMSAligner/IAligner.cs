using System.Collections.Generic;
using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;

namespace PNNLOmics.Alignment.LCMSWarp.LCMSAligner
{
    public interface IAligner
    {
        LCMSAlignmentData Align(IEnumerable<UMCLight> baseline, IEnumerable<UMCLight> features);

        LCMSAlignmentData Align(IEnumerable<MassTagLight> baseline, IEnumerable<UMCLight> features);
    }
}
