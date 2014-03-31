using Processor;
using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;
using System.Collections.Generic;

namespace Aligner
{
    public class FeatureMatching : IAligner
    {
        public AlignmentData Align(IEnumerable<UMCLight> baseline, IEnumerable<UMCLight> features)
        {
            AlignmentOptions options = new AlignmentOptions();

            LcmsWarpFeatureAligner aligner = new LcmsWarpFeatureAligner();

            return aligner.AlignFeatures(baseline as List<UMCLight>, features as List<UMCLight>, options);
        }

        public AlignmentData Align(IEnumerable<MassTagLight> baseline, IEnumerable<UMCLight> features)
        {
            AlignmentOptions options = new AlignmentOptions();

            LcmsWarpFeatureAligner aligner = new LcmsWarpFeatureAligner();

            return aligner.AlignFeatures(baseline as List<MassTagLight>, features as List<UMCLight>, options, true);
        }
    }
}
