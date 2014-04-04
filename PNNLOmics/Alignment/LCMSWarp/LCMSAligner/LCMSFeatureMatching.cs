using System.Collections.Generic;
using PNNLOmics.Alignment.LCMSWarp.LCMSProcessor;
using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;

namespace PNNLOmics.Alignment.LCMSWarp.LCMSAligner
{
    public class FeatureMatching : IAligner
    {
        public LCMSAlignmentData Align(IEnumerable<UMCLight> baseline, IEnumerable<UMCLight> features)
        {
            LCMSAlignmentOptions options = new LCMSAlignmentOptions();

            LCMSWarpFeatureAligner aligner = new LCMSWarpFeatureAligner();

            return aligner.AlignFeatures(baseline as List<UMCLight>, features as List<UMCLight>, options);
        }

        public LCMSAlignmentData Align(IEnumerable<MassTagLight> baseline, IEnumerable<UMCLight> features)
        {
            LCMSAlignmentOptions options = new LCMSAlignmentOptions();

            LCMSWarpFeatureAligner aligner = new LCMSWarpFeatureAligner();

            return aligner.AlignFeatures(baseline as List<MassTagLight>, features as List<UMCLight>, options, true);
        }
    }
}
