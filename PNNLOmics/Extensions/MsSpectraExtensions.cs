using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Extensions
{
    static class MsSpectraExtensions
    {
        /// <summary>
        /// Creates a dictionary that for a given list of MS/MS spectra, maps based on their datasets (groupid)
        /// </summary>
        /// <param name="spectra"></param>
        /// <returns></returns>
        public static Dictionary<int, List<MSSpectra>> Group(this IEnumerable<MSSpectra> spectra)
        {
            Dictionary<int, List<MSSpectra>> map = new Dictionary<int, List<MSSpectra>>();
            foreach (MSSpectra spectrum in spectra)
            {
                bool doesExists = map.ContainsKey(spectrum.GroupID);
                if (!doesExists)
                {
                    map.Add(spectrum.GroupID, new List<MSSpectra>());
                }
                map[spectrum.GroupID].Add(spectrum);
            }
            return map;
        }
    }
}
