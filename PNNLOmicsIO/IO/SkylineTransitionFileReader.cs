using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;
using System.IO;

namespace PNNLOmicsIO.IO
{
    public class SkylineTransitionFileReader : ISequenceFileReader
    {
        public IEnumerable<Peptide> Read(string path)
        {
            List<Peptide> peptides  = new List<Peptide>();
            List<string> lines      = File.ReadAllLines(path).ToList();

            Dictionary<string, List<List<string>>> precursorMap = new Dictionary<string, List<List<string>>>();

            foreach (string line in lines)
            {
                List<string> lineData = line.Split(',').ToList();
                if (lineData.Count < 6)
                {
                    continue;
                }

                if (!lineData[5].StartsWith("y"))
                {
                    if (!lineData[5].StartsWith("b"))
                    {
                        continue;
                    }
                }

                if (!precursorMap.ContainsKey(lineData[0]))
                {
                    precursorMap.Add(lineData[0], new List<List<string>>());
                }
                precursorMap[lineData[0]].Add(lineData);
            }

            foreach (string key in precursorMap.Keys)
            {
                List<List<string>> data     = precursorMap[key];
                Peptide peptide             = new Peptide();
                peptide                     = new Peptide();
                peptide.Sequence            = data[0][3];                
                MSSpectra spectrum          = new MSSpectra();
                spectrum.PrecursorMZ        = Convert.ToDouble(key);
                
                foreach (List<string> line in data)
                {
                    double fragment = Convert.ToDouble(line[1]);
                    XYData point    = new XYData(fragment, 100);
                    spectrum.Peaks.Add(point);
                }
                spectrum.Peptide = peptide;
                peptide.Spectrum = spectrum;
                peptides.Add(peptide);
            }
            return peptides;
        }
    }
}
