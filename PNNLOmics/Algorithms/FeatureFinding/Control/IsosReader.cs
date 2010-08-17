using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureFinding.Data;

namespace PNNLOmics.Algorithms.FeatureFinding.Control
{
	/// <summary>
	/// Reads a text-based delimited Isos file and creates a List of MS Features based on the file contents.
	/// </summary>
	public class IsosReader
	{
		#region AutoProperties
		private StreamReader IsosFileReader { get; set; }
		private TextWriter IsosFileWriter { get; set; }
		private Logger Logger { get; set; }
		public Dictionary<String, int> ColumnMap { get; set; }
		public List<MSFeature> MSFeatureList { get; set; }
		public int NumOfUnfilteredMSFeatures { get; set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor for passing in a String containing the location of the ISOS csv file
		/// </summary>
		/// <param name="settings">Reference to the Settings object</param>
		/// <param name="logger">Logger object</param>
		public IsosReader(ref Settings settings, Logger logger)
		{
			this.Logger = logger;
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];

			this.IsosFileReader = new StreamReader(settings.InputDirectory + settings.InputFileName);
			this.IsosFileWriter = new StreamWriter(settings.OutputDirectory + baseFileName + "_Filtered_isos.csv");
			this.ColumnMap = CreateColumnMapping();
			this.MSFeatureList = SaveDataToMSFeatureList(ref settings);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Fills in the Column Map with the appropriate values.
		/// The Map will have a Column Property (e.g. MSFeature.Frame) mapped to a Column Number.
		/// </summary>
		/// <returns>The column map as a Dictionary object</returns>
		private Dictionary<String, int> CreateColumnMapping()
		{
			Dictionary<String, int> columnMap = new Dictionary<String, int>();

			String firstLine = IsosFileReader.ReadLine();

			if (firstLine == null)
			{
				return null;
			}

			String[] columnTitles = firstLine.Split('\t', ',', '\n');
			IsosFileWriter.WriteLine(firstLine);

			for (int i = 0; i < columnTitles.Length; i++)
			{
				switch (columnTitles[i].Trim().ToLower())
				{
					case "frame_num":
						columnMap.Add("MSFeature.Frame", i);
						break;
					case "scan_num":
						columnMap.Add("MSFeature.Frame", i);
						break;
					case "lc_scan_num":
						columnMap.Add("MSFeature.Frame", i);
						break;
					case "ims_scan_num":
						columnMap.Add("MSFeature.ScanIMS", i);
						break;
					case "charge":
						columnMap.Add("MSFeature.Charge", i);
						break;
					case "abundance":
						columnMap.Add("MSFeature.Abundance", i);
						break;
					case "mz":
						columnMap.Add("MSFeature.Mz", i);
						break;
					case "fit":
						columnMap.Add("MSFeature.Fit", i);
						break;
					case "average_mw":
						columnMap.Add("MSFeature.MassAverage", i);
						break;
					case "monoisotopic_mw":
						columnMap.Add("MSFeature.MassMonoisotopic", i);
						break;
					case "mostabundant_mw":
						columnMap.Add("MSFeature.MassMostAbundant", i);
						break;
					case "fwhm":
						columnMap.Add("MSFeature.Fwhm", i);
						break;
					case "signal_noise":
						columnMap.Add("MSFeature.SignalNoise", i);
						break;
					case "mono_abundance":
						columnMap.Add("MSFeature.AbundanceMono", i);
						break;
					case "mono_plus2_abundance":
						columnMap.Add("MSFeature.AbundancePlus2", i);
						break;
					case "orig_intensity":
						columnMap.Add("MSFeature.IntensityOriginal", i);
						break;
					case "tia_orig_intensity":
						columnMap.Add("MSFeature.IntensityOriginalTIA", i);
						break;
					case "drift_time":
						columnMap.Add("MSFeature.DriftTimeIMS", i);
						break;
					case "cumulative_drift_time":
						columnMap.Add("MSFeature.DriftTimeCumulative", i);
						break;
					case "flag":
						columnMap.Add("MSFeature.ErrorFlag", i);
						break;
					default:
						//Title not found.
						break;
				}
			}

			if (columnMap.Count == 0)
			{
				//TODO: Create default mapping?
				Logger.Log("Isos file does not contain column headers. Cannot continue.");
				throw new ApplicationException("Isos file does not contain column headers. Cannot continue.");
			}

			return columnMap;
		}

		/// <summary>
		/// Saves the data from a ISOS csv file to an List of MSFeature Objects.
		/// </summary>
		/// <param name="settings">Reference to the Settings object</param>
		/// <returns>List of MS Features</returns>
		private List<MSFeature> SaveDataToMSFeatureList(ref Settings settings)
		{
			List<MSFeature> msFeatureList = new List<MSFeature>();
			String line;
			MSFeature msFeature;
			NumOfUnfilteredMSFeatures = 0;
			int msFeatureIndex = 0;
			int currentFrame = 0;

			// Read the rest of the Stream, 1 line at a time, and save the appropriate data into new Objects
			for (int i = 0; (line = IsosFileReader.ReadLine()) != null; i++)
			{
				try
				{
					String[] columns = line.Split(',', '\t', '\n');

					msFeature = new MSFeature();
					msFeature.IndexInFile = i;

					if (ColumnMap.ContainsKey("MSFeature.Frame"))
					{
						int frame = Int32.Parse(columns[ColumnMap["MSFeature.Frame"]]);

						if (i == 0)
						{
							currentFrame = frame;
							ScanLCMapHolder.ScanLCMap.Add(ScanLCMapHolder.ScanLCIndex, frame);
						}
						if (frame != currentFrame)
						{
							currentFrame = frame;
							ScanLCMapHolder.ScanLCIndex++;
							ScanLCMapHolder.ScanLCMap.Add(ScanLCMapHolder.ScanLCIndex, frame);
						}

						msFeature.ScanLC = ScanLCMapHolder.ScanLCIndex;
					}

					if (ColumnMap.ContainsKey("MSFeature.ScanIMS")) msFeature.ScanIMS = Int32.Parse(columns[ColumnMap["MSFeature.ScanIMS"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.Charge")) msFeature.ChargeState = Int16.Parse(columns[ColumnMap["MSFeature.Charge"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.Abundance")) msFeature.Abundance = Int32.Parse(columns[ColumnMap["MSFeature.Abundance"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.Mz")) msFeature.MZ = Double.Parse(columns[ColumnMap["MSFeature.Mz"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.Fit")) msFeature.Fit = float.Parse(columns[ColumnMap["MSFeature.Fit"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.MassMonoisotopic")) msFeature.MassMonoisotopic = Double.Parse(columns[ColumnMap["MSFeature.MassMonoisotopic"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.MassMostAbundant")) msFeature.MassMostAbundant = Double.Parse(columns[ColumnMap["MSFeature.MassMostAbundant"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.Fwhm")) msFeature.Fwhm = float.Parse(columns[ColumnMap["MSFeature.Fwhm"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.SignalNoise")) msFeature.SignalToNoiseRatio = float.Parse(columns[ColumnMap["MSFeature.SignalNoise"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.AbundanceMono")) msFeature.AbundanceMono = Int32.Parse(columns[ColumnMap["MSFeature.AbundanceMono"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.AbundancePlus2")) msFeature.AbundancePlus2 = Int32.Parse(columns[ColumnMap["MSFeature.AbundancePlus2"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.IntensityOriginal")) msFeature.IntensityOriginal = float.Parse(columns[ColumnMap["MSFeature.IntensityOriginal"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.IntensityOriginalTIA")) msFeature.IntensityOriginalTIA = float.Parse(columns[ColumnMap["MSFeature.IntensityOriginalTIA"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.DriftTimeIMS")) msFeature.DriftTime = float.Parse(columns[ColumnMap["MSFeature.DriftTimeIMS"]], System.Globalization.NumberStyles.Any);
					if (ColumnMap.ContainsKey("MSFeature.ErrorFlag")) msFeature.IsSuspicious = columns[ColumnMap["MSFeature.ErrorFlag"]].Equals("") ? false : Int32.Parse(columns[ColumnMap["MSFeature.ErrorFlag"]], System.Globalization.NumberStyles.Any) == 1;

					if (PassesFilters(msFeature, ref settings))
					{
						msFeature.ID = msFeatureIndex;
						msFeatureList.Add(msFeature);
						IsosFileWriter.WriteLine(line);
						msFeatureIndex++;
					}

					NumOfUnfilteredMSFeatures++;
				}
				catch (Exception e)
				{
					Logger.Log("Error while reading line in isos file. Skipping Line #" + (i + 2));
					Console.WriteLine(e.StackTrace);
				}
			}

			IsosFileReader.Close();
			IsosFileWriter.Close();

			return msFeatureList;
		}

		/// <summary>
		/// Determines if an MS Feature passes the filters defined by the Settings object
		/// </summary>
		/// <param name="msFeature">MS Feature to be tested</param>
		/// <param name="settings">Reference to the Settings object</param>
		/// <returns>true if the MS Feature passes, false otherwise</returns>
		private bool PassesFilters(MSFeature msFeature, ref Settings settings)
		{
			if (ColumnMap.ContainsKey("MSFeature.Frame"))
			{
				if (msFeature.ScanLC < settings.ScanLCMin || msFeature.ScanLC > settings.ScanLCMax) return false;
			}

			if (ColumnMap.ContainsKey("MSFeature.ScanIMS"))
			{
				if (msFeature.ScanIMS < settings.ScanIMSMin || msFeature.ScanIMS > settings.ScanIMSMax) return false;
			}

			if (ColumnMap.ContainsKey("MSFeature.Fit"))
			{
				if (msFeature.Fit > settings.FitMax) return false;
			}

			if (ColumnMap.ContainsKey("MSFeature.Abundance"))
			{
				if (msFeature.Abundance < settings.IntensityMin) return false;
			}

			if (ColumnMap.ContainsKey("MSFeature.MassMonoisotopic"))
			{
				if (msFeature.MassMonoisotopic < settings.MassMonoisotopicStart || msFeature.MassMonoisotopic > settings.MassMonoisotopicEnd) return false;
			}

			return true;
		}
		#endregion
	}
}
