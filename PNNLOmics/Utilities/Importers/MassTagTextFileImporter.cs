using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;
using System.IO;

namespace PNNLOmics.Utilities.Importers
{
    public class MassTagTextFileImporter : ImporterBase<List<MassTag>>
    {
        #region Constructors
        public MassTagTextFileImporter(string filename)
        {
            if (!File.Exists(filename)) throw new System.IO.IOException("MassTagImporter failed. File doesn't exist.");

            this.m_delimiter = '\t';
            this.FileName = filename;


        }

        #endregion

        #region Properties
        public string FileName { get; set; }

        #endregion

        #region Public Methods
        public override List<MassTag> Import()
        {
            return getMassTags();
        }

        private List<MassTag> getMassTags()
        {

            List<MassTag> massTagList = new List<MassTag>();

            using (StreamReader reader = new StreamReader(this.FileName))
            {
                string headerLine = reader.ReadLine();    //first line is the header line.   

                m_columnHeaders = ProcessLine(headerLine);

                int lineCounter = 1;
                while (reader.Peek() != -1)
                {
                    string line = reader.ReadLine();
                    lineCounter++;

                    List<string> processedLineData = ProcessLine(line);

                    MassTag massTag;
                    try
                    {
                        massTag = convertTextToMassTag(processedLineData);
                    }
                    catch (Exception ex)
                    {
                        string msg = "Importer failed. Error reading line: " + lineCounter.ToString() + "\nDetails: " + ex.Message;
                        throw new Exception(msg);
                    }

                    massTagList.Add(massTag);

                }

                return massTagList;


            }
        }

        private MassTag convertTextToMassTag(List<string> processedLineData)
        {
            MassTag mt = new MassTag();
            mt.ID = ParseIntField(LookupData(processedLineData, "Mass_Tag_ID"));
            mt.MassMonoisotopic = ParseDoubleField(LookupData(processedLineData, "Monoisotopic_Mass"));
            mt.MassMonoisotopicAligned = mt.MassMonoisotopic;


            //TODO: determine which NET to import into...  NET or NETAverage or both 
            mt.NET = ParseDoubleField(LookupData(processedLineData, "NET"));
            mt.NETAverage = ParseDoubleField(LookupData(processedLineData, "NET"));
            mt.NETStandardDeviation = ParseDoubleField(LookupData(processedLineData, "StD_GANET"));

            mt.ObservationCount = (ushort)ParseIntField(LookupData(processedLineData, "Peptide_Obs_Count_Passing_Filter"));

            string peptideSequence = LookupData(processedLineData, "Peptide");

            mt.Molecule = createPeptide(peptideSequence);
            return mt;
        }

        private Molecule createPeptide(string peptideSequence)
        {
            Peptide peptide = new Peptide();
            peptide.Sequence = peptideSequence;

            return peptide;

        }


        #endregion

        #region Private Methods
        #endregion


    }
}
