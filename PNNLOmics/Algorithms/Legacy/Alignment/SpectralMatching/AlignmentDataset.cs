using System;
using System.IO;

namespace PNNLOmics.Algorithms.Alignment.SpectralMatching
{

    /// <summary>
    /// Encapsulates dataset information 
    /// </summary>
    [Obsolete("Code moved to MultiAlignWinOmics: MultiAlignCore.Algorithms.Alignment.SpectralMatching")]
    public class AlignmentDataset
    {
        public AlignmentDataset()
        {
        }

        public AlignmentDataset(string basePath, string name)
        {
            RawFile     = Path.Combine(basePath, name + ".raw");
            PeptideFile = Path.Combine(basePath, name + "_msgfplus_fht.txt");
            FeatureFile = Path.Combine(basePath, name + "_isos.csv");
        }

        public AlignmentDataset(string raw, string feature, string peptide)
        {
            RawFile     = raw;
            PeptideFile = peptide;
            FeatureFile = feature;
        }

        public AlignmentDataset(string basePath, string raw, string feature, string peptide)
        {
            RawFile     = Path.Combine(basePath, raw);
            PeptideFile = Path.Combine(basePath, peptide);
            FeatureFile = Path.Combine(basePath, feature);
        }

        public string RawFile { get; set; }
        public string FeatureFile { get; set; }
        public string PeptideFile { get; set; }
    }
}
