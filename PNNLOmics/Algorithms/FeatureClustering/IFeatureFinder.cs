using System.Collections.Generic;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// Creates UMC's based on MS Features.
    /// </summary>
	[System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Clustering")]
    public interface IFeatureFinder: IProgressNotifer
    {
        /// <summary>
        /// Finds features from the file of MS Features.
        /// </summary>
        List<UMCLight> FindFeatures(List<MSFeatureLight> features, 
                                    LcmsFeatureFindingOptions   options,                                     
                                    ISpectraProvider provider);
    }
}
