using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmicsIO.IO
{
    public class MsgfReader : BaseTextFileReader<Peptide>, ISequenceFileReader
    {

        protected override Dictionary<string, int> CreateColumnMapping(System.IO.TextReader textReader)
        {
            Dictionary<String, int> columnMap = new Dictionary<String, int>(StringComparer.CurrentCultureIgnoreCase);

            // TODO: Different types of delimiters?
            String[] columnTitles = textReader.ReadLine().Split('\t', '\n');
            int numOfColumns = columnTitles.Length;

            for (int i = 0; i < numOfColumns; i++)
            {
                string column = columnTitles[i].ToLower().Trim();
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
                    case "specprob":
                        columnMap.Add("Peptide.Score", i);
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

                peptides.Add(peptide);
            }
            return peptides;
        }
    }
}
