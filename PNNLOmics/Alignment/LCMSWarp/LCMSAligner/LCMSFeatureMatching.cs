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
        /// <summary>
        /// Method to align two UMCLight Enumerables, the first parameter
        /// used as the baseline to align the second parameter
        /// </summary>
        /// <param name="baseline"></param>
        /// <param name="features"></param>
        /// <returns></returns>
        public LcmsAlignmentData Align(IEnumerable<UMCLight> baseline, IEnumerable<UMCLight> features)
        {
            var options = new LcmsAlignmentOptions();

            var aligner = new LcmsWarpFeatureAligner();

            return aligner.AlignFeatures(baseline as List<UMCLight>, features as List<UMCLight>, options);
        }

        /// <summary>
        /// Method to align a UMCLight Enumerable to a MassTagLight enumberable.
        /// The MassTagLight Enumberable is used as the baseline to align the
        /// UMCLight enumberable.
        /// </summary>
        /// <param name="baseline"></param>
        /// <param name="features"></param>
        /// <returns></returns>
        public LcmsAlignmentData Align(IEnumerable<MassTagLight> baseline, IEnumerable<UMCLight> features)
        {
            var options = new LcmsAlignmentOptions();

            var aligner = new LcmsWarpFeatureAligner();

            return aligner.AlignFeatures(baseline as List<MassTagLight>, features as List<UMCLight>, options, true);
        }
    }
}
