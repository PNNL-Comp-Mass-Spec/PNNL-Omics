using System;
using System.Collections.Generic;
using PNNLOmics.Data;

namespace PNNLOmicsIO.IO
{
    public class MsgfReader : BaseTextFileReader<Peptide>, ISequenceFileReader
    {

        protected override Dictionary<string, int> CreateColumnMapping(System.IO.TextReader textReader)
        {
            var columnMap = new Dictionary<String, int>(StringComparer.CurrentCultureIgnoreCase);

            // TODO: Different types of delimiters?
            var columnTitles = textReader.ReadLine().Split('\t', '\n');
            var numOfColumns = columnTitles.Length;

            for (var i = 0; i < numOfColumns; i++)
            {
                var column = columnTitles[i].ToLower().Trim();
                switch (column)
                {
                    case "scan":
                        columnMap.Add("Peptide.Scan", i);
                        break;
                    case "charge":
                        columnMap.Add("Peptide.Charge", i);
                        break;
                    case "protein":
                        columnMap.Add("Peptide.Protein", i);
                        break;
                    case "peptide":
                        columnMap.Add("Peptide.Sequence", i);
                        break;
                    case "msgfdb_specprob":                        
                        columnMap.Add("Peptide.ScorePRISM", i);
                        break;
                    case "specprob":
                        columnMap.Add("Peptide.Score", i);
                        break;
                    case "evalue":
                        columnMap.Add("peptide.evalue", i);
                        break;
                    case "precursormz":
                        columnMap.Add("Peptide.PrecursorMz", i);
                        break;
                    default:
                        break;
                }
            }

            return columnMap;

        }
        public IEnumerable<Peptide> Read(string path)
        {
            return base.ReadFile(path);
        }

        protected override IEnumerable<Peptide> SaveFileToEnumerable(System.IO.TextReader textReader, Dictionary<string, int> columnMapping)
        {
            var peptides = new List<Peptide>();
            var line = "";
            while ((line = textReader.ReadLine()) != null)
            {
                var columns = line.Split('\t', '\n');

                var peptide = new Peptide();
                if (columnMapping.ContainsKey("Peptide.Scan"))
                {
                    peptide.Scan = Convert.ToInt32(columns[columnMapping["Peptide.Scan"]]);
                }
                if (columnMapping.ContainsKey("Peptide.Charge"))
                {
                   peptide.ChargeState = Convert.ToInt32(columns[columnMapping["Peptide.Charge"]]);
                }
                if (columnMapping.ContainsKey("Peptide.Protein"))
                {
                    var protein = new Protein();
                    protein.ProteinDescription = columns[columnMapping["Peptide.Protein"]];
                    peptide.ProteinList.Add(protein);
                }
                if (columnMapping.ContainsKey("Peptide.Sequence"))
                {
                    peptide.Sequence = columns[columnMapping["Peptide.Sequence"]];
                }
                if (columnMapping.ContainsKey("Peptide.Score"))
                {
                    peptide.Score = Convert.ToDouble(columns[columnMapping["Peptide.Score"]]);
                }
                if (columnMapping.ContainsKey("Peptide.ScorePRISM"))
                {
                    peptide.Score = Convert.ToDouble(columns[columnMapping["Peptide.ScorePRISM"]]);
                }
                if (columnMapping.ContainsKey("Peptide.evalue"))
                {
                    peptide.Score = Convert.ToDouble(columns[columnMapping["peptide.evalue"]]);
                }
                if (columnMapping.ContainsKey("Peptide.PrecursorMz"))
                {
                    peptide.Mz = Convert.ToDouble(columns[columnMapping["Peptide.PrecursorMz"]]);
                }
                peptides.Add(peptide);
            }
            return peptides;
        }
    }
}
