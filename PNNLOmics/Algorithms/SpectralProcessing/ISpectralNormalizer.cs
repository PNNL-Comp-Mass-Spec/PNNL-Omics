using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.SpectralProcessing
{
    /// <summary>
    /// Converts a spectra
    /// </summary>
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
