using System.Collections.Generic;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Extensions
{
    public static class PeptideExtensions
    {
        public static Dictionary<int, List<Peptide>> CreateScanMaps(this IEnumerable<Peptide> peptides)
        {
            var peptideMap = new Dictionary<int, List<Peptide>>();
            foreach (var p in peptides)
            {
                if (!peptideMap.ContainsKey(p.Scan))
                {
                    peptideMap.Add(p.Scan, new List<Peptide>());
                }
                peptideMap[p.Scan].Add(p);
            }

            return peptideMap;
        }

        public static UMCLight GetParentUmc(this Peptide peptide)
        {
            if (peptide == null) return null;

            if (peptide.Spectrum == null) return null;

            return peptide.Spectrum.GetParentUmc();
        }
    }
}