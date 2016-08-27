using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.SpectralProcessing
{
    /// <summary>
    /// Converts a spectra
    /// </summary>
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.SpectralProcessing")]
    public interface ISpectralNormalizer
    {
        /// <summary>
        /// Normalizes a spectrum
        /// </summary>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        MSSpectra Normalize(MSSpectra spectrum);        
    }

   

}
