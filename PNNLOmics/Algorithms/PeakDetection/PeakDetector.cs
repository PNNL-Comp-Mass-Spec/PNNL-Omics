using System.Collections.ObjectModel;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.PeakDetection
{
    /// <summary>
    /// Abstract base class that defines a way to take XY data and return a list of peaks.
    /// </summary>
    public abstract class PeakDetector
    {
        /// <summary>
        /// Processes a list of XYdata and returns a set of peaks.
        /// </summary>
        /// <param name="rawXYData">List of XY data.</param>
        /// <returns>List of peaks.</returns>
        //TODO: change List<Peak> into a PeakCollection
        //TODO: look into the collection object instead of using a list or other explicit collection object.
        public abstract Collection<Peak> DetectPeaks(Collection<XYData> rawXYData);
    }
}
