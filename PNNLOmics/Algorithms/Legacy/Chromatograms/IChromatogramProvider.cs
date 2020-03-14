using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.Chromatograms
{
    /// <summary>
    /// Provides chromatograms to client components to avoid coupling data structure backend or caching scheme.
    /// </summary>
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Chromatograms")]
    public interface IChromatogramProvider
    {
        /// <summary>
        /// Gets the chromatogram at the m/z specified across the scans provided.
        /// </summary>
        /// <param name="mz">mass to charge ratio interested in</param>
        /// <param name="tolerance">Tolerance to look at</param>
        /// <param name="scanStart">Starting Scan</param>
        /// <param name="scanEnd">Ending Scan</param>
        /// <returns>Chromatogram found at the location given</returns>
        Chromatogram GetChromatogram(double mz, double tolerance, int scanStart, int scanEnd);
    }
}
