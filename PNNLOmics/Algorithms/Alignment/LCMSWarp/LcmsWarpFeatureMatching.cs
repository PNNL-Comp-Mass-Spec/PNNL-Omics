using System.Collections.Generic;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;

namespace PNNLOmics.Algorithms.Alignment.LCMSWarp
{
    /// <summary>
    /// Object which performs feature alignment through LCMSWarp
    /// </summary>
    public class FeatureMatching : 
        IFeatureAligner<IEnumerable<UMCLight>, IEnumerable<UMCLight>, LcmsWarpAlignmentData>,
        IFeatureAligner<IEnumerable<MassTagLight>, IEnumerable<UMCLight>, LcmsWarpAlignmentData>      
    {

        public FeatureMatching()
            : this(new LcmsWarpAlignmentOptions())
        {
        }

        public FeatureMatching(LcmsWarpAlignmentOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// Gets or sets LCMSWarp options
        /// </summary>
        public LcmsWarpAlignmentOptions  Options { get; set; }
        /// <summary>
        /// Gets or sets the baseline spectra provider
        /// </summary>
        public ISpectraProvider BaselineSpectraProvider { get; set; }
        /// <summary>
        /// Gets or sets the alignee spectra provider.
        /// </summary>
        public ISpectraProvider AligneeSpectraProvider { get; set; }

        /// <summary>
        /// Method to align two UMCLight Enumerables, the first parameter
        /// used as the baseline to align the second parameter
        /// </summary>
        /// <param name="baseline"></param>
        /// <param name="features"></param>
        /// <returns></returns>
        public LcmsWarpAlignmentData Align(IEnumerable<UMCLight> baseline, IEnumerable<UMCLight> features)
        {            
            var aligner = new LcmsWarpFeatureAligner();
            return aligner.AlignFeatures(baseline as List<UMCLight>, features as List<UMCLight>, Options);
        }

        /// <summary>
        /// Method to align a UMCLight Enumerable to a MassTagLight enumberable.
        /// The MassTagLight Enumberable is used as the baseline to align the
        /// UMCLight enumberable.
        /// </summary>
        /// <param name="baseline"></param>
        /// <param name="features"></param>
        /// <returns></returns>
        public LcmsWarpAlignmentData Align(IEnumerable<MassTagLight> baseline, IEnumerable<UMCLight> features)
        {            
            var aligner = new LcmsWarpFeatureAligner();
            return aligner.AlignFeatures(baseline as List<MassTagLight>, features as List<UMCLight>, Options);
        }

        public event System.EventHandler<ProgressNotifierArgs> Progress;
    }
}
