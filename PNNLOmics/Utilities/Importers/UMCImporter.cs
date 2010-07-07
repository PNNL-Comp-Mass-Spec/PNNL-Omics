using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;
using System.IO;

namespace PNNLOmics.Utilities.Importers
{
    public class UMCImporter : ImporterBase<List<UMC>>
    {

        #region Constructors

        public UMCImporter(string filename, char delimiter)
        {
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
                if (sr.Peek() == -1)
                {
                    sr.Close();
                    throw new InvalidDataException("There is no data in the UMC data object");

                }
                string headerLine = sr.ReadLine();
                List<string> headers = ProcessLine(headerLine);
                m_columnHeaders = ProcessLine(headerLine);
                
                bool areHeadersValid = validateHeaders();
                
                if (!areHeadersValid)
                {
                    throw new InvalidDataException("There is a problem with the column headers in the UMC data");
                }

                string line;
                int counter = 1;
                while (sr.Peek() > -1)
                {
                    line = sr.ReadLine();
                    List<string> processedData = ProcessLine(line);
                    if (processedData.Count != headers.Count)    // new line is in the wrong format... could be blank
                    {
                        throw new InvalidDataException("Data in UMC row #" + counter.ToString() + "is invalid - \nThe number of columns does not match that of the header line");
                    }

                    UMC umc = convertTextToUMCData(processedData);
                    umcList.Add(umc);
                    counter++;

                }
                sr.Close();
            }

            return umcList;

     
        }

        private UMC convertTextToUMCData(List<string> processedData)
        {
            UMC umc = new UMC();

            umc.ID = ParseIntField(LookupData(processedData, "UMCIndex"));
            umc.ScanLCStart = ParseIntField(LookupData(processedData, "ScanStart"));
            umc.ScanLCEnd = ParseIntField(LookupData(processedData, "ScanEnd"));
            umc.ScanLC = ParseIntField(LookupData(processedData, "ScanClassRep"));
            umc.NET = ParseDoubleField(LookupData(processedData, "NETClassRep"));
            umc.MassMonoisotopic = ParseDoubleField(LookupData(processedData, "UMCMonoMW"));
            
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

        private bool validateHeaders()
        {
            if (m_columnHeaders.Count < 10) return false;
            if (m_columnHeaders[0] != "UMCIndex") return false;
            if (m_columnHeaders[1] != "ScanStart") return false;
            return true;
        }
        #endregion

        #region Private Methods
        #endregion




   
    }
}
