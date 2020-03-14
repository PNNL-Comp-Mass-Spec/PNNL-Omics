
namespace PNNLOmics.Algorithms.PeakDetection
{
    /// <summary>
    /// Has the noise been removed from the data prior.  Orbitrap data has the noise removed.
    /// </summary>
    public enum InstrumentDataNoiseType
    {
        /// <summary>
        /// noise has not been removed
        /// </summary>
        Standard,

        /// <summary>
        /// noise has ben thresholded away prior, such as orbitrap
        /// </summary>
        NoiseRemoved
    }
}
