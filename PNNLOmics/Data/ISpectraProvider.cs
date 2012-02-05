using System.Collections.Generic;
using PNNLOmics.Data;

namespace PNNLOmics.Data
{    
    /// <summary>
    /// Interface for objects that have access to raw data.
    /// </summary>
    public interface ISpectraProvider
    {
        /// <summary>
        /// Retrieves the scan from the underlying stream.
        /// </summary>
        /// <param name="scan"></param>
        /// <returns></returns>
        List<XYData> GetRawSpectra(int scan, int group);
    }
}
