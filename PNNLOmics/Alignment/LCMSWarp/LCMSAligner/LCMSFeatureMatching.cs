using System.Collections.Generic;
using PNNLOmics.Alignment.LCMSWarp.LCMSProcessor;
using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;

namespace PNNLOmics.Alignment.LCMSWarp.LCMSAligner
{
    /// <summary>
    /// Object which performs feature alignment through LCMSWarp
    /// </summary>
    public class FeatureMatching : IAligner
    {
        public LcmsAlignmentData Align(IEnumerable<UMCLight> baseline, IEnumerable<UMCLight> features)
        {
            LcmsAlignmentOptions options = new LcmsAlignmentOptions();

            LcmsWarpFeatureAligner aligner = new LcmsWarpFeatureAligner();

            return aligner.AlignFeatures(baseline as List<UMCLight>, features as List<UMCLight>, options);
        }

        public LcmsAlignmentData Align(IEnumerable<MassTagLight> baseline, IEnumerable<UMCLight> features)
        {
            LcmsAlignmentOptions options = new LcmsAlignmentOptions();

            LcmsWarpFeatureAligner aligner = new LcmsWarpFeatureAligner();

            return aligner.AlignFeatures(baseline as List<MassTagLight>, features as List<UMCLight>, options, true);
        }
    }
}
