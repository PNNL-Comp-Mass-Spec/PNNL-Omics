﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PNNLOmics.Data;

namespace PNNLOmicsIO.IO
{
    public class DtaFileWriter : IMsMsSpectraWriter
    {
        /// <summary>
        /// Creates a DTA file based on the spectra provided.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msmsFeatures"></param>
        public void Write(string path, IEnumerable<MSSpectra> msmsFeatures)
        {
            string baseName = Path.GetFileNameWithoutExtension(path);
            using (TextWriter writer = File.CreateText(path))
            {
                Write(writer, baseName, msmsFeatures);
            }
        }

        /// <summary>
        /// Creates the DTA file based on the spectra provided.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msmsFeatures"></param>
        public void Append(string path, IEnumerable<MSSpectra> msmsFeatures)
        {
            string baseName = Path.GetFileNameWithoutExtension(path);
            using (TextWriter writer = File.AppendText(path))
            {
                Write(writer, baseName, msmsFeatures);
            }
        }

        private void Write(TextWriter writer, string baseName, IEnumerable<MSSpectra> msmsFeatures)
        {
            foreach (MSSpectra feature in msmsFeatures)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("=================================== \"{0}.{2}.{1}.{2}.dta\" ==================================",
                                            baseName,
                                            feature.Scan,
                                            feature.PrecursorChargeState);
                sb.Append(Environment.NewLine);
                sb.AppendFormat("{0} {1} scan={2} cs={1}", feature.PrecursorMZ, feature.PrecursorChargeState, feature.Scan);
                sb.Append(Environment.NewLine);
                foreach (XYData peak in feature.Peaks)
                {
                    sb.Append(Math.Round(peak.X, 5));
                    sb.Append(" ");
                    sb.Append(peak.Y);
                    sb.Append(Environment.NewLine);
                }
                sb.Append(Environment.NewLine);
                writer.WriteLine(sb.ToString());
                sb.Append(Environment.NewLine);
            }
        }
    }
}