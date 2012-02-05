using System;
using System.IO;
using System.Collections.Generic;
using PNNLOmics.Data.Features;
using System.Globalization;

namespace PNNLOmicsIO.IO
{
	public class UMCFileReader : BaseTextFileReader<UMC>
	{
		protected override Dictionary<string, int> CreateColumnMapping(TextReader textReader)
		{
			Dictionary<String, int> columnMap = new Dictionary<String, int>(StringComparer.CurrentCultureIgnoreCase);

			String[] columnTitles = textReader.ReadLine().Split('\t', '\n');
			int numOfColumns = columnTitles.Length;

			for (int i = 0; i < numOfColumns; i++)
			{
				switch (columnTitles[i].Trim())
				{
					case "UMCIndex":
						columnMap.Add("Umc.ID", i);
						break;
					case "Feature_index":
						columnMap.Add("Umc.ID", i);
						break;
					case "Feature_Index":
						columnMap.Add("Umc.ID", i);
						break;
					case "ScanStart":
						columnMap.Add("Umc.ScanLCStart", i);
						break;
					case "Scan_Start":
						columnMap.Add("Umc.ScanLCStart", i);
						break;
					case "ScanEnd":
						columnMap.Add("Umc.ScanLCEnd", i);
						break;
					case "Scan_End":
						columnMap.Add("Umc.ScanLCEnd", i);
						break;
					case "ScanClassRep":
						columnMap.Add("Umc.ScanLC", i);
						columnMap.Add("Umc.ScanLCAligned", i);
						break;
					case "Scan":
						columnMap.Add("Umc.ScanLC", i);
						columnMap.Add("Umc.ScanLCAligned", i);
						break;
					case "NETClassRep":
						columnMap.Add("Umc.NET", i);
						columnMap.Add("Umc.NETAligned", i);
						break;
					case "UMCMonoMW":
						columnMap.Add("Umc.MassMonoisotopic", i);
						columnMap.Add("Umc.MassMonoisotopicAligned", i);
						break;
					case "monoisotopic_mass":
						columnMap.Add("Umc.MassMonoisotopic", i);
						columnMap.Add("Umc.MassMonoisotopicAligned", i);
						break;
					case "Monoisotopic_Mass":
						columnMap.Add("Umc.MassMonoisotopic", i);
						columnMap.Add("Umc.MassMonoisotopicAligned", i);
						break;
					case "UMCMWStDev":
						columnMap.Add("Umc.MassMonoisotopicStandardDeviation", i);
						break;
					case "UMCMZForChargeBasis":
						columnMap.Add("Umc.MZ", i);
						break;
					case "Class_Rep_MZ":
						columnMap.Add("Umc.MZ", i);
						break;
					case "UMCAbundance":
						columnMap.Add("Umc.AbundanceSum", i);
						break;
					case "Abundance":
						columnMap.Add("Umc.AbundanceSum", i);
						break;
					case "MaxAbundance":
						columnMap.Add("Umc.AbundanceMaximum", i);
						break;
					case "Max_Abundance":
						columnMap.Add("Umc.AbundanceMaximum", i);
						break;
					case "ClassStatsChargeBasis":
						columnMap.Add("Umc.ChargeState", i);
						break;
					case "Class_Rep_Charge":
						columnMap.Add("Umc.ChargeState", i);
						break;
					case "ChargeStateMax":
						columnMap.Add("Umc.ChargeMaximum", i);
						break;
					case "UMCMemberCount":
						columnMap.Add("Umc.SpectralCount", i);
						break;
					case "Drift_Time":
						columnMap.Add("Umc.DriftTime", i);
						break;
					case "IMS_Drift_Time":
						columnMap.Add("Umc.DriftTime", i);
						break;
					case "Drift_Time_Uncorrected":
						columnMap.Add("Umc.DriftTimeUncorrected", i);
						break;
					case "Avg_Interference_Score":
						columnMap.Add("Umc.AverageInterferenceScore", i);
						break;
					case "Conformation_Fit_Score":
						columnMap.Add("Umc.ConformationFitScore", i);
						break;
					case "Decon2ls_Fit_Score":
						columnMap.Add("Umc.AverageDeconFitScore", i);
						break;
					case "Members_Percentage":
						columnMap.Add("Umc.MembersPercentageScore", i);
						break;
					case "Combined_Score":
						columnMap.Add("Umc.CombinedScore", i);
						break;
					default:
						//Title not found.
						break;
				}
			}

			return columnMap;
		}

		protected override IEnumerable<UMC> SaveFileToEnumerable(TextReader textReader, Dictionary<string, int> columnMapping)
		{
			List<UMC> umcList = new List<UMC>();
			String line;
			UMC umc;
			int previousId = -99;
			int currentId = -99;
			int idIndex = 0;

			// Read the rest of the Stream, 1 line at a time, and save the appropriate data into new Objects
			while ((line = textReader.ReadLine()) != null)
			{
				String[] columns = line.Split(',', '\t', '\n');

				if (columnMapping.ContainsKey("Umc.ID"))
				{
                    currentId = Int32.Parse(columns[columnMapping["Umc.ID"]]);
				}
				else
				{
					currentId = idIndex;
					idIndex++;
				}


				/// If the UMC ID matches the previous UMC ID, then skip the UMC data.
				///		- It is the same UMC, different peptide, and we have already stored this UMC data.
				if (previousId != currentId)
				{
					umc = new UMC();
					umc.ID = currentId;
					if (columnMapping.ContainsKey("Umc.ScanLCStart")) umc.ScanLCStart = int.Parse(columns[columnMapping["Umc.ScanLCStart"]]);
					if (columnMapping.ContainsKey("Umc.ScanLCEnd")) umc.ScanLCEnd = int.Parse(columns[columnMapping["Umc.ScanLCEnd"]]);
					if (columnMapping.ContainsKey("Umc.ScanLC")) umc.ScanLC = int.Parse(columns[columnMapping["Umc.ScanLC"]]);
					if (columnMapping.ContainsKey("Umc.ScanLCAligned")) umc.ScanLCAligned = int.Parse(columns[columnMapping["Umc.ScanLCAligned"]]);
					if (columnMapping.ContainsKey("Umc.NET")) umc.NET = Double.Parse(columns[columnMapping["Umc.NET"]]);
					if (columnMapping.ContainsKey("Umc.NETAligned")) umc.NETAligned = Double.Parse(columns[columnMapping["Umc.NETAligned"]]);
					if (columnMapping.ContainsKey("Umc.MassMonoisotopic")) umc.MassMonoisotopic = Double.Parse(columns[columnMapping["Umc.MassMonoisotopic"]]);
					if (columnMapping.ContainsKey("Umc.MassMonoisotopicAligned")) umc.MassMonoisotopicAligned = Double.Parse(columns[columnMapping["Umc.MassMonoisotopicAligned"]]);
					if (columnMapping.ContainsKey("Umc.MassMonoisotopicStandardDeviation")) umc.MassMonoisotopicStandardDeviation = Double.Parse(columns[columnMapping["Umc.MassMonoisotopicStandardDeviation"]]);
					if (columnMapping.ContainsKey("Umc.MZ")) umc.MZ = Double.Parse(columns[columnMapping["Umc.MZ"]]);
					if (columnMapping.ContainsKey("Umc.DriftTime")) umc.DriftTime = float.Parse(columns[columnMapping["Umc.DriftTime"]]);
					//if (columnMapping.ContainsKey("Umc.AbundanceSum"))
					//{
					//    umc.AbundanceSum = Int64.Parse(columns[columnMapping["Umc.AbundanceSum"]], NumberStyles.AllowDecimalPoint);
					//}
					if (columnMapping.ContainsKey("Umc.AbundanceMaximum"))
					{
						umc.AbundanceMaximum = int.Parse(columns[columnMapping["Umc.AbundanceMaximum"]]);
					}
					if (columnMapping.ContainsKey("Umc.ChargeState"))
					{
						umc.ChargeState = int.Parse(columns[columnMapping["Umc.ChargeState"]]);
						if (!columnMapping.ContainsKey("Umc.ChargeMaximum"))
						{
							umc.ChargeMaximum = umc.ChargeState;
						}
					}
					if (columnMapping.ContainsKey("Umc.ChargeMaximum"))
					{
						umc.ChargeMaximum = (short)Int16.Parse(columns[columnMapping["Umc.ChargeMaximum"]]);
					}
					
					//if (columnMapping.ContainsKey("Umc.DriftTimeUncorrected")) umc.DriftTimeUncorrected = float.Parse(columns[columnMapping["Umc.DriftTimeUncorrected"]]);
					//if (columnMapping.ContainsKey("Umc.AverageInterferenceScore")) umc.AverageInterferenceScore = Double.Parse(columns[columnMapping["Umc.AverageInterferenceScore"]]);
					//if (columnMapping.ContainsKey("Umc.ConformationFitScore")) umc.ConformationFitScore = Double.Parse(columns[columnMapping["Umc.ConformationFitScore"]]);
					//if (columnMapping.ContainsKey("Umc.AverageDeconFitScore")) umc.AverageDeconFitScore = Double.Parse(columns[columnMapping["Umc.AverageDeconFitScore"]]);
					//if (columnMapping.ContainsKey("Umc.MembersPercentageScore")) umc.MembersPercentageScore = Double.Parse(columns[columnMapping["Umc.MembersPercentageScore"]]);
					//if (columnMapping.ContainsKey("Umc.CombinedScore")) umc.CombinedScore = Double.Parse(columns[columnMapping["Umc.CombinedScore"]]);
					umcList.Add(umc);
					previousId = currentId;
				}
			}

			return umcList;
		}
	}
}
