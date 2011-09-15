using System;
using System.Collections.Generic;
using System.IO;
using PNNLOmics.Data;

namespace PNNLOmics.IO.FileReaders
{
	public class MassTagFileReader : BaseTextFileReader<MassTag>
	{
		protected override Dictionary<string, int> CreateColumnMapping(TextReader textReader)
		{
            Dictionary<String, int> columnMap = new Dictionary<String, int>(StringComparer.CurrentCultureIgnoreCase);

			// TODO: Different types of delimiters?
			String[] columnTitles = textReader.ReadLine().Split('\t', '\n');
			int numOfColumns = columnTitles.Length;

			for (int i = 0; i < numOfColumns; i++)
			{
				// TODO: Make everything lower case and call toLower()
				switch (columnTitles[i].Trim())
				{
					case "Mass_Tag_ID":
						columnMap.Add("MassTag.ID", i);
						break;
					case "Mass_Tag_Index":
						columnMap.Add("MassTag.Index", i);
						break;
					case "Conformer_ID":
						columnMap.Add("MassTag.ConformationID", i);
						break;
					case "Monoisotopic_Mass":
						columnMap.Add("MassTag.MassMonoisotopic", i);
						break;
					case "Avg_GANET":
						columnMap.Add("MassTag.NET", i);
						break;
					case "NET_Value_to_Use":
						columnMap.Add("MassTag.NET", i);
						break;
					case "Net_Value_to_Use":
						columnMap.Add("MassTag.NET", i);
						break;
					case "High_Peptide_Prophet_Probability":
						columnMap.Add("MassTag.PriorProbability", i);
						break;
					case "Cnt_GANET":
						columnMap.Add("MassTag.ObservationCount", i);
						break;
					case "NET_Obs_Count":
						columnMap.Add("MassTag.ObservationCount", i);
						break;
					case "PNET":
						columnMap.Add("MassTag.NETPredicted", i);
						break;
					case "Avg_Drift_Time":
						columnMap.Add("MassTag.DriftTime", i);
						break;
					case "Drift_Time_Avg":
						columnMap.Add("MassTag.DriftTime", i);
						break;
					case "High_Discriminant_Score":
						columnMap.Add("MassTag.DiscriminantMax", i);
						break;
					case "StD_GANET":
						columnMap.Add("MassTag.NETStandardDeviation", i);
						break;
					case "Conformer_Charge":
						columnMap.Add("MassTag.ChargeState", i);
						break;
					case "Conformer_Obs_Count":
						columnMap.Add("MassTag.ConformationObservationCount", i);
						break;
					default:
						//Title not found.
						break;
				}
			}

			return columnMap;
		}
		
		// TODO: Update name to ParseFile
		protected override IEnumerable<MassTag> SaveFileToEnumerable(TextReader textReader, Dictionary<string, int> columnMapping)
		{
			List<MassTag> massTagList = new List<MassTag>();
			string line;
			MassTag massTag;
			int currentId = -99;
			int idIndex = 0;

			// Read the rest of the Stream, 1 line at a time, and save the appropriate data into new Objects
			while ((line = textReader.ReadLine()) != null)
			{
				string[] columns = line.Split('\t', '\n');

				if (columnMapping.ContainsKey("MassTag.ID"))
				{
					currentId = Int32.Parse(columns[columnMapping["MassTag.ID"]]);
				}
				else
				{
					currentId = idIndex;
					idIndex++;
				}
				
				massTag = new MassTag();
				massTag.ID = currentId;

				if (columnMapping.ContainsKey("MassTag.Index"))
				{
					massTag.Index = Int32.Parse(columns[columnMapping["MassTag.Index"]]);
				}
				else
				{
					massTag.Index = currentId;
				}

				if (columnMapping.ContainsKey("MassTag.MassMonoisotopic")) massTag.MassMonoisotopic = double.Parse(columns[columnMapping["MassTag.MassMonoisotopic"]]);
				if (columnMapping.ContainsKey("MassTag.NET")) massTag.NET = double.Parse(columns[columnMapping["MassTag.NET"]]);
				if (columnMapping.ContainsKey("MassTag.PriorProbability")) massTag.PriorProbability = double.Parse(columns[columnMapping["MassTag.PriorProbability"]]);
				if (columnMapping.ContainsKey("MassTag.ObservationCount")) massTag.ObservationCount = ushort.Parse(columns[columnMapping["MassTag.ObservationCount"]]);
				if (columnMapping.ContainsKey("MassTag.NETPredicted")) massTag.NETPredicted = double.Parse(columns[columnMapping["MassTag.NETPredicted"]]);
				if (columnMapping.ContainsKey("MassTag.DiscriminantMax")) massTag.DiscriminantMax = double.Parse(columns[columnMapping["MassTag.DiscriminantMax"]]);
				if (columnMapping.ContainsKey("MassTag.NETStandardDeviation")) massTag.NETStandardDeviation = double.Parse(columns[columnMapping["MassTag.NETStandardDeviation"]]);
				if (columnMapping.ContainsKey("MassTag.DriftTime"))
				{
					if (!columns[columnMapping["MassTag.DriftTime"]].Equals("NULL"))
					{
						massTag.DriftTime = float.Parse(columns[columnMapping["MassTag.DriftTime"]]);
						if (columnMapping.ContainsKey("MassTag.ConformationID")) massTag.ConformationID = int.Parse(columns[columnMapping["MassTag.ConformationID"]]);
						if (columnMapping.ContainsKey("MassTag.ChargeState")) massTag.ChargeState = int.Parse(columns[columnMapping["MassTag.ChargeState"]]);
						if (columnMapping.ContainsKey("MassTag.ConformationObservationCount")) massTag.ConformationObservationCount = ushort.Parse(columns[columnMapping["MassTag.ConformationObservationCount"]]);
					}
					else
					{
						massTag.DriftTime = -99;
					}
				}

				massTagList.Add(massTag);
			}

			return massTagList;
		}
	}
}
