using System.Collections.Generic;
using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;

namespace PNNLOmics.Alignment.LCMSWarp.LCMSAligner
{
    /// <summary>
    /// Interface for objects which will use LcmsWarp to align two or more datasets
    /// </summary>
    public interface IAligner
    {
        /// <summary>
        /// Alignment method using two sets of UMCLight objects
        /// </summary>
        /// <param name="baseline"></param>
        /// <param name="features"></param>
        /// <returns></returns>
        LcmsAlignmentData Align(IEnumerable<UMCLight> baseline, IEnumerable<UMCLight> features);

        /// <summary>
        /// Alignment method using a set of MassTagLights as the baseline to align the set of UMCLights
        /// </summary>
        /// <param name="baseline"></param>
        /// <param name="features"></param>
        /// <returns></returns>
        LcmsAlignmentData Align(IEnumerable<MassTagLight> baseline, IEnumerable<UMCLight> features);
    }
}
