using System.Collections.Generic;
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
            var map = new Dictionary<int, List<MSSpectra>>();
            foreach (var spectrum in spectra)
            {
                var doesExists = map.ContainsKey(spectrum.GroupId);
                if (!doesExists)
                {
                    map.Add(spectrum.GroupId, new List<MSSpectra>());
                }
                map[spectrum.GroupId].Add(spectrum);
            }
            return map;
        }
    }
}
