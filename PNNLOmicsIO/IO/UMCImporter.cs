using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;
using System.IO;


namespace PNNLOmicsIO.Utilities.Importers
{
    [Obsolete]
    public class UMCImporter : ImporterBase<List<UMC>>
    {
        #region Constructors

        public UMCImporter(string filename, char delimiter)
        {
            if (!File.Exists(filename)) throw new System.IO.IOException("UMCImporter failed. File doesn't exist.");


            this.m_delimiter = delimiter;
            this.FileName = filename;
        }


        #endregion

        #region Properties
        public string FileName { get; set; }
        #endregion

        #region Public Methods
        public override List<UMC> Import()
        {
            return getUMCs();
        }
  
        #endregion

        #region Private Methods
        /// <summary>
        /// Main method for retrieving text data from file. 
        /// </summary>
        /// <returns></returns>
        private List<UMC> getUMCs()
        {
            List<UMC> umcList = new List<UMC>();
            StreamReader reader;

            try
            {
                reader = new StreamReader(this.FileName);
            }
            catch (Exception)
            {

                throw new System.IO.IOException("There was a problem reading the UMC data file.");
            }

            using (StreamReader sr = reader)
            {

                //do an initial check to see if there is anything in the file
                if (sr.Peek() == -1)
                {
                    sr.Close();
                    throw new InvalidDataException("There is no data in the UMC data object");

                }

                string headerLine = sr.ReadLine();
                m_columnHeaders = ProcessLine(headerLine);

                bool areHeadersValid = validateHeaders();

                if (!areHeadersValid)
                {
                    throw new InvalidDataException("There is a problem with the column headers in the UMC data");
                }

                string line;
                int lineCounter = 1;   //used for tracking which line is being processed. 

                //read and process each line of the file
                while (sr.Peek() > -1)
                {
                    line = sr.ReadLine();
                    List<string> processedData = ProcessLine(line);

                    //ensure that processed line is the same size as the header line
                    if (processedData.Count != m_columnHeaders.Count)
                    {
                        throw new InvalidDataException("Data in UMC row #" + lineCounter.ToString() + "is invalid - \nThe number of columns does not match that of the header line");
                    }

                    UMC umc = convertTextToUMCData(processedData);
                    umcList.Add(umc);

                    //increase counter that keeps track of what line we are on... for use in error reporting. 
                    lineCounter++;

                }
                sr.Close();
            }

            return umcList;


        }

        /// <summary>
        /// takes a row of processes text and creates a UMC object from it
        /// </summary>
        /// <param name="processedData"></param>
        /// <returns></returns>
        private UMC convertTextToUMCData(List<string> processedData)
        {
            UMC umc = new UMC();

            umc.ID = ParseIntField(LookupData(processedData, "UMCIndex"));
            umc.ScanLCStart = ParseIntField(LookupData(processedData, "ScanStart"));
            umc.ScanLCEnd = ParseIntField(LookupData(processedData, "ScanEnd"));
            umc.ScanLC = ParseIntField(LookupData(processedData, "ScanClassRep"));
            umc.NET = ParseDoubleField(LookupData(processedData, "NETClassRep"));
            umc.MassMonoisotopic = ParseDoubleField(LookupData(processedData, "UMCMonoMW"));

            //TODO: [gord]  Determine if the following two lines are ok to do... or if we want the UMC properties to take care of this. 
            umc.NETAligned = umc.NET;
            umc.MassMonoisotopicAligned = umc.MassMonoisotopic;

            umc.MassMonoisotopicStandardDeviation = ParseDoubleField(LookupData(processedData, "UMCMWStDev"));
            umc.MassMonoisotopicMinimum = ParseDoubleField(LookupData(processedData, "UMCMWMin"));
            umc.MassMonoisotopicMaximum = ParseDoubleField(LookupData(processedData, "UMCMWMax"));
            umc.Abundance = ParseIntField(LookupData(processedData, "UMCAbundance"));
            umc.ChargeState = ParseShortField(LookupData(processedData, "ClassStatsChargeBasis"));
            umc.ChargeMinimum = ParseShortField(LookupData(processedData, "ChargeStateMin"));
            umc.ChargeMaximum = ParseShortField(LookupData(processedData, "ChargeStateMax"));
            umc.MZ = ParseDoubleField(LookupData(processedData, "UMCMZForChargeBasis"));
            umc.FitScoreAverage = ParseDoubleField(LookupData(processedData, "UMCAverageFit"));

            //TODO: figure out what to do with the other columns found in the VIPER output file (which is being imported here)
            //umc. = ParseIntField(LookupData(processedData, "UMCMemberCount"));
            //umc.UMCMemberCountUsedForAbu = ParseIntField(LookupData(processedData, "UMCMemberCountUsedForAbu"));
            //umc.PairIndex = ParseIntField(LookupData(processedData, "PairIndex"));
            //umc.PairMemberType = ParseIntField(LookupData(processedData, "PairMemberType"));
            //umc.ExpressionRatio = ParseDoubleField(LookupData(processedData, "ExpressionRatio"));
            //umc.ExpressionRatioStDev = ParseDoubleField(LookupData(processedData, "ExpressionRatioStDev"));
            //umc.ExpressionRatioChargeStateBasisCount = ParseIntField(LookupData(processedData, "ExpressionRatioChargeStateBasisCount"));
            //umc.ExpressionRatioMemberBasisCount = ParseIntField(LookupData(processedData, "ExpressionRatioMemberBasisCount"));
            //umc.MultiMassTagHitCount = ParseShortField(LookupData(processedData, "MultiMassTagHitCount"));
            //umc.MassTagID = ParseIntField(LookupData(processedData, "MassTagID"));
            //umc.MassTagMonoMW = ParseDoubleField(LookupData(processedData, "MassTagMonoMW"));
            //umc.MassTagNET = ParseDoubleField(LookupData(processedData, "MassTagNET"));
            //umc.MassTagNETStDev = ParseDoubleField(LookupData(processedData, "MassTagNETStDev"));
            //umc.SLiCScore = ParseDoubleField(LookupData(processedData, "SLiC Score"));
            //umc.DelSLiC = ParseDoubleField(LookupData(processedData, "DelSLiC"));
            //umc.MemberCountMatchingMassTag = ParseIntField(LookupData(processedData, "MemberCountMatchingMassTag"));
            //umc.IsInternalStdMatch = ParseBoolField(LookupData(processedData, "IsInternalStdMatch"));
            //umc.PeptideProphetProbability = ParseDoubleField(LookupData(processedData, "PeptideProphetProbability"));
            //umc.Peptide = LookupData(processedData, "Peptide");
            return umc;
        }

        /// <summary>
        /// simple validation of the 
        /// </summary>
        /// <returns></returns>
        private bool validateHeaders()
        {
            if (m_columnHeaders == null) return false;
            if (m_columnHeaders.Count < 10) return false;
            if (m_columnHeaders[0] != "UMCIndex") return false;
            if (m_columnHeaders[1] != "ScanStart") return false;
            return true;
        }

        #endregion

    }
}
