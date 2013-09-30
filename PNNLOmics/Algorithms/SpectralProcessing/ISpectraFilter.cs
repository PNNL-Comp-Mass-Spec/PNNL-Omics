using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.SpectralProcessing
{
    /// <summary>
    /// Defines how a spectra should be filtered.
    /// </summary>
    public interface ISpectraFilter
    {
        /// <summary>
        /// Filters a spectrum based on a threshold - could be a raw intensity, or a percentage of the highest peaks
        /// </summary>
        /// <param name="spectrum"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        MSSpectra Threshold(MSSpectra spectrum,  double threshold);
    }

    

   
}
