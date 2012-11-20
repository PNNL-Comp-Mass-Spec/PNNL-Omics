using System.Collections.Generic;
using PNNLOmics.Data;
using System;

namespace PNNLOmics.Data
{    
    /// <summary>
    /// Interface for objects that have access to raw data.
    /// </summary>
    public interface ISpectraProvider: IDisposable
    {
        /// <summary>
        /// Retrieves the scan from the underlying stream.
        /// </summary>
        /// <param name="scan"></param>
        /// <returns></returns>
        List<XYData> GetRawSpectra(int scan, int group);
        /// <summary>
        /// Retrieves the scan from the underlying stream of MSn type n = scanLevel
        /// </summary>
        /// <param name="scan"></param>
        /// <param name="group"></param>
        /// <param name="scanLevel">MS Level of the scan</param>
        /// <returns></returns>
        List<XYData> GetRawSpectra(int scan, int group, int scanLevel);
        /// <summary>
        /// Retrieves a list of MS/MS spectra from the given group.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        List<MSSpectra> GetMSMSSpectra(int group);
        /// <summary>
        /// Get a list of MS/MS spectra, but exclude if it exists in the dictionary of provided scans.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="excludeMap"></param>
        /// <returns></returns>
        List<MSSpectra> GetMSMSSpectra(int group, Dictionary<int, int> excludeMap);
        /// <summary>
        /// Adds a file ID to the path for multi-file support.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="groupID"></param>
        void AddDataFile(string path, int groupID);
    }
}