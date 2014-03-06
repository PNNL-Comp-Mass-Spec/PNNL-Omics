using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmicsIO.IO
{
    public class MsgfTabReader : BaseTextFileReader<Peptide>, ISequenceFileReader
    {

        protected override Dictionary<string, int> CreateColumnMapping(System.IO.TextReader textReader)
        {
            Dictionary<String, int> columnMap = new Dictionary<String, int>(StringComparer.CurrentCultureIgnoreCase);            
            String[] columnTitles = textReader.ReadLine().Split('\t', '\n');
            int numOfColumns = columnTitles.Length;

            for (int i = 0; i < numOfColumns; i++)
            {
                string column = columnTitles[i].ToLower().Trim();
                switch (column)
                {
                    case "scannum":
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
                    case "msgfscore":
                        columnMap.Add("Peptide.Score", i);
                        break;
                    case "evalue":
                        columnMap.Add("peptide.evalue", i);
                        break;
                    case "precursor":
                        columnMap.Add("Peptide.PrecursorMz", i);
                        break;
                    case "denovoscore":
                        columnMap.Add("Peptide.QualityScore", i);
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
            List<Peptide> peptides = new List<Peptide>();
            string line = "";
            while ((line = textReader.ReadLine()) != null)
            {
                string[] columns = line.Split('\t', '\n');

                Peptide peptide = new Peptide();
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
                    Protein protein = new Protein();
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
                if (columnMapping.ContainsKey("Peptide.QualityScore"))
                {
                    peptide.QualityScore = Convert.ToDouble(columns[columnMapping["Peptide.QualityScore"]]);
                }
                peptides.Add(peptide);
            }
            return peptides;
        }
    }
}
