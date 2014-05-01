using System.Collections.Generic;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureMatcher.MSnLinker
{
    /// <summary>
    /// Interface for linking features together from MSMS to MSn
    /// </summary>
    public interface IMSnLinker
    {

        /// <summary>
        /// Gets or sets the feature tolerances to use.
        /// </summary>
        FeatureTolerances Tolerances
        {
            get;
            set;
        }        
        /// <summary>
        /// Links MS Features to MSMS Spectra.
        /// </summary>
        /// <param name="features"></param>
        /// <param name="fragmentSpectra">Fragmentation spectra to link</param>
        /// <param name="rawSpectraProvider">Provider that provides access to raw scans if more data is required.</param>
        /// <returns>The number of a times a MSn spectra was mapped to a feature using the spectrum ID as a key.</returns>
        Dictionary<int, int> LinkMSFeaturesToMSn(List<MSFeatureLight> features, List<MSSpectra> fragmentSpectra, ISpectraProvider rawSpectraProvider);
        /// <summary>
        /// Links MS Features to MSMS Spectra.
        /// </summary>
        /// <param name="features"></param>
        /// <param name="fragmentSpectra">Fragmentation spectra to link</param>
        /// <returns>The number of a times a MSn spectra was mapped to a feature using the spectrum ID as a key.</returns>
        Dictionary<int, int> LinkMSFeaturesToMSn(List<MSFeatureLight> features, List<MSSpectra> fragmentSpectra);               
    }
}
