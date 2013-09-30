using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;
using System.IO;

namespace PNNLOmicsIO.IO
{
    /// <summary>
    /// Writes Mascot Generic File formatted MS/MS spectra.
    /// </summary>
    public class MgfFileReader: IMsMsSpectraReader
    {
        /// <summary>
        /// Creates a MGF file based on the spectra provided.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msmsFeatures"></param>
        public List<MSSpectra> Read(string path)
        {
            string[] lines  = File.ReadAllLines(path);
            int mode        = 0;

            List<MSSpectra> spectra   = new List<MSSpectra>();
            MSSpectra currentSpectrum = null;
            string [] delimeter       = new string[] {" "};

            for(int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].ToUpper();
                if (line.Contains("BEGIN IONS"))
                {
                    mode    = 0;
                }
                else if (line.Contains("CHARGE="))
                {
                    mode            = 1;
                    i               = i + 1;
                    currentSpectrum = new MSSpectra();
                }
                else if (line.Contains("END IONS"))
                {
                    mode            = 0;
                    if (currentSpectrum != null)
                    {
                        spectra.Add(currentSpectrum);
                    }
                }


                if (mode == 1)
                {
                    string[] data = line.Split(delimeter,
                                                StringSplitOptions.RemoveEmptyEntries);

                    if (data.Length < 2)
                        continue;

                    try
                    {
                        double x = Convert.ToDouble(data[0]);
                        double y = Convert.ToDouble(data[1]);
                        XYData datum = new XYData(x, y);
                        currentSpectrum.Peaks.Add(datum);
                    }
                    catch
                    {
                    }
                }                
            }

            return spectra;
        }
    }
}
