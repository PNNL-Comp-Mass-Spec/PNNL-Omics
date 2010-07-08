using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureFinding.Control
{
	public class IsosWriter
	{
		private StreamReader m_isosFileReader;
		private TextWriter m_isosFileWriter;
		private Dictionary<String, int> m_columnMap;
		private List<MSFeature> m_msFeatureList;

		public IsosWriter(Settings settings, List<MSFeature> msFeatureList, Dictionary<String, int> columnMap)
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			m_isosFileReader = new StreamReader(settings.OutputDirectory + baseFileName + "_Filtered_isos.csv");
			m_isosFileWriter = new StreamWriter(settings.OutputDirectory + baseFileName + "_Filtered_New_isos.csv");
			m_columnMap = columnMap;
			m_msFeatureList = msFeatureList;

			WriteIsosFile();

			File.Delete(settings.OutputDirectory + baseFileName + "_Filtered_isos.csv");
			File.Move(settings.OutputDirectory + baseFileName + "_Filtered_New_isos.csv", settings.OutputDirectory + baseFileName + "_Filtered_isos.csv");
		}

		private void WriteIsosFile()
		{
			String line = "";
			int offset = 0;

			m_msFeatureList.Sort(new Comparison<MSFeature>(Feature.IDComparison));

			m_isosFileWriter.WriteLine(m_isosFileReader.ReadLine());

			// Read the rest of the Stream, 1 line at a time, and save the write the appropriate data into the new Isos file
			for (int i = 0; (line = m_isosFileReader.ReadLine()) != null && i + offset < m_msFeatureList.Count; i++)
			{
				MSFeature msFeature = m_msFeatureList[i + offset];
				if (msFeature.ID == i)
				{
					String[] columns = line.Split(',', '\t', '\n');

					if (m_columnMap.ContainsKey("MSFeature.Mz")) columns[m_columnMap["MSFeature.Mz"]] = msFeature.MZ.ToString();
					if (m_columnMap.ContainsKey("MSFeature.MassMonoisotopic")) columns[m_columnMap["MSFeature.MassMonoisotopic"]] = msFeature.MassMonoisotopic.ToString();
					if (m_columnMap.ContainsKey("MSFeature.MassMostAbundant")) columns[m_columnMap["MSFeature.MassMostAbundant"]] = msFeature.MassMostAbundant.ToString();

					string newLine = "";

					foreach (String column in columns)
					{
						newLine = newLine + column + ",";
					}

					newLine = newLine.Remove(newLine.Length - 1);

					m_isosFileWriter.WriteLine(newLine);
				}
				else
				{
					m_isosFileWriter.WriteLine(line);
					offset--;
				}
			}

			m_isosFileWriter.Write(m_isosFileReader.ReadToEnd());

			m_isosFileWriter.Close();
			m_isosFileReader.Close();
		}
	}
}
