using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;
using System.IO;

namespace PNNLOmicsIO.IO
{
    public class MgfFileWriter: IMsMsSpectraWriter
    {
        /// <summary>
        /// Creates a MGF file based on the spectra provided.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msmsFeatures"></param>
        public void Write(string path, IEnumerable<MSSpectra> msmsFeatures)
        {

            using (TextWriter writer = File.CreateText(path))
            {
                foreach (MSSpectra feature in msmsFeatures)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("BEGIN IONS");
                    sb.Append(Environment.NewLine);
                    sb.Append(string.Format("TITLE={0}.1.dta", feature.Scan));
                    sb.Append(Environment.NewLine);

                    sb.Append(string.Format("PEPMASS={0}", feature.PrecursorMZ));
                    sb.Append(Environment.NewLine);

                    sb.Append(string.Format("CHARGE={0}+", feature.PrecursorChargeState));
                    sb.Append(Environment.NewLine);

                    foreach (XYData peak in feature.Peaks)
                    {
                        sb.Append(Math.Round(peak.X, 5));
                        sb.Append(" ");
                        sb.Append(peak.Y);
                        sb.Append(Environment.NewLine);
                    }
                    sb.Append("END IONS");
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    writer.WriteLine(sb.ToString());
                }
            }            
        }
    }
}
