using System.Collections.Generic;
using System.IO;
using PNNLOmics.Data;
using System;

namespace PNNLOmics.IO.FileReaders
{
    public class ScansFileReader: BaseTextFileReader<ScanSummary>
    {
        #region Column constants
        private const string SCAN_NUMBER        = "scan";
        private const string SCAN_TIME          = "scanTime";
        private const string TYPE               = "abundance";
        private const string BPI                = "fit";
        private const string BPI_MZ             = "bpimz";
        private const string TIC                = "tic";
        private const string NUM_PEAKS          = "numberOfPeaks";
        private const string NUM_DEISOTOPED     = "numberOfDeisotoped";
        #endregion
        
        /// <summary>
        /// Maps the .scans file header to a dictionary for column mapping.  
        /// </summary>
        /// <param name="textReader"></param>
        /// <returns></returns>
		protected override Dictionary<string, int> CreateColumnMapping(TextReader textReader)
		{
			Dictionary<string, int> columnMap   = new Dictionary<string, int>();
			string[] columnTitles               = textReader.ReadLine().Split('\t', '\n');
			int numOfColumns                    = columnTitles.Length;                       
			for (int i = 0; i < numOfColumns; i++)
			{
                string title    = columnTitles[i].Trim();
                title           = title.ToLower();
				switch (columnTitles[i].Trim())
				{                      				
                    case "scan_num":
                        columnMap.Add(SCAN_NUMBER, i);
                        break;
                    case "scan_time":                        
						columnMap.Add(SCAN_TIME, i);
                        break;
                    case "type":                 
						columnMap.Add(TYPE, i);
                        break;
                    case "bpi":	                 
						columnMap.Add(BPI, i);
                        break;
                    case "bpi_mz":                 
						columnMap.Add(BPI_MZ, i);
                        break;
                    case "tic":	                 
						columnMap.Add(TIC, i);
                        break;
                    case "num_peaks":                 
						columnMap.Add(NUM_PEAKS, i);
                        break;
                    case "num_deisotoped":	                 
						columnMap.Add(NUM_DEISOTOPED, i);
                        break;                    			
					default:						
						break;
				}
			}
			return columnMap;
		}

        protected override IEnumerable<ScanSummary> SaveFileToEnumerable(System.IO.TextReader textReader, Dictionary<string, int> columnMapping)
        {
            List<ScanSummary> scans = new List<ScanSummary>();
            int     currentId       = 0;
            string  line            = "";


            while ((line = textReader.ReadLine()) != null)
            {
                string[] columns    = line.Split(new string[] { Delimeter }, 0, StringSplitOptions.RemoveEmptyEntries);
                ScanSummary scan = new ScanSummary();

                if (columnMapping.ContainsKey(SCAN_NUMBER))     scan.Scan               = int.Parse(columns[columnMapping[SCAN_NUMBER]]);
                if (columnMapping.ContainsKey(SCAN_TIME))       scan.Time               = double.Parse(columns[columnMapping[SCAN_TIME]]);
                if (columnMapping.ContainsKey(TYPE))            scan.MSLevel            = int.Parse(columns[columnMapping[TYPE]]);
                if (columnMapping.ContainsKey(BPI))             scan.Bpi                = long.Parse(columns[columnMapping[BPI]]);
                if (columnMapping.ContainsKey(BPI_MZ))          scan.BpiMz              = int.Parse(columns[columnMapping[BPI_MZ]]);
                if (columnMapping.ContainsKey(TIC))             scan.Tic                = long.Parse(columns[columnMapping[TIC]]);
                if (columnMapping.ContainsKey(NUM_PEAKS))       scan.NumberOfPeaks      = int.Parse(columns[columnMapping[NUM_PEAKS]]);
                if (columnMapping.ContainsKey(NUM_DEISOTOPED))  scan.NumberOfDeisotoped = int.Parse(columns[columnMapping[NUM_DEISOTOPED]]);
                           
                scans.Add(scan);
                currentId++;
            }
            return scans;
        }
    }
}
