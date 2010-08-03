using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace PNNLOmics.Algorithms.FeatureFinding.Control
{
	/// <summary>
	/// Class designed for reading an INI Settings file for the LCMSFeatureFinder application and storing its contents into a Setting object.
	/// </summary>
	public class IniReader
	{
		private String m_path;

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(String section, String key, String def, StringBuilder retVal, int size, String filePath);

		/// <summary>
		/// Constructor that takes in an INI file path.
		/// </summary>
		/// <param name="path">File path of the INI file to be read</param>
		public IniReader(String path)
		{
			if (File.Exists(path))
			{
				m_path = path;
			}
			else
			{
				throw new FileNotFoundException("Could not find file '" + path + "'.");
			}
		}

		/// <summary>
		/// Creates a Settings object based on the INI file contents.
		/// </summary>
		/// <returns>Settings object</returns>
		public Settings CreateSettings()
		{
			Settings settings = new Settings();
			String value = "";

			/*
			 * Files Settings
			 */
			value = IniReadValue("Files", "InputFileName");
			if (!value.Equals(String.Empty))
			{
				if (!Path.GetDirectoryName(value).Equals(string.Empty))
				{
					settings.InputDirectory = Path.GetDirectoryName(value) + "\\";
				}

				settings.InputFileName = Path.GetFileName(value);
			}

			value = IniReadValue("Files", "OutputDirectory");
			if (!value.Equals(String.Empty))
			{
				settings.OutputDirectory = value + "\\";
			}
			else
			{
				settings.OutputDirectory = settings.InputDirectory;
			}

			/*
			 * DataFilters Settings
			 */
			value = IniReadValue("DataFilters", "MaxIsotopicFit");
			if (!value.Equals(String.Empty))
			{
				settings.FitMax = float.Parse(value);
			}

			value = IniReadValue("DataFilters", "MinimumIntensity");
			if (!value.Equals(String.Empty))
			{
				settings.IntensityMin = float.Parse(value);
			}

			value = IniReadValue("DataFilters", "IMSMinScan");
			if (!value.Equals(String.Empty))
			{
				settings.ScanIMSMin = int.Parse(value);
			}

			value = IniReadValue("DataFilters", "IMSMaxScan");
			if (!value.Equals(String.Empty))
			{
				settings.ScanIMSMax = int.Parse(value);
				if (settings.ScanIMSMax <= 0) settings.ScanIMSMax = int.MaxValue;
			}

			value = IniReadValue("DataFilters", "LCMinScan");
			if (!value.Equals(String.Empty))
			{
				settings.ScanLCMin = int.Parse(value);
			}

			value = IniReadValue("DataFilters", "LCMaxScan");
			if (!value.Equals(String.Empty))
			{
				settings.ScanLCMax = int.Parse(value);
				if (settings.ScanLCMax <= 0) settings.ScanLCMax = int.MaxValue;
			}

			value = IniReadValue("DataFilters", "MonoMassStart");
			if (!value.Equals(String.Empty))
			{
				settings.MassMonoisotopicStart = Double.Parse(value);
			}

			value = IniReadValue("DataFilters", "MonoMassEnd");
			if (!value.Equals(String.Empty))
			{
				settings.MassMonoisotopicEnd = Double.Parse(value);
			}

			/*
			 * UMCCreationOptions Settings
			 */
			value = IniReadValue("UMCCreationOptions", "IgnoreIMSDriftTime");
			if (!value.Equals(String.Empty))
			{
				settings.IgnoreIMSDriftTime = bool.Parse(value);
			}

			value = IniReadValue("UMCCreationOptions", "MonoMassConstraint");
			if (!value.Equals(String.Empty))
			{
				settings.MassMonoisotopicConstraint = float.Parse(value);
			}

			value = IniReadValue("UMCCreationOptions", "MonoMassConstraintIsPPM");
			if (!value.Equals(String.Empty))
			{
				settings.MassMonoisotopicConstraintIsPPM = bool.Parse(value);
			}

			value = IniReadValue("UMCCreationOptions", "UseGenericNET");
			if (!value.Equals(String.Empty))
			{
				settings.UseGenericNET = bool.Parse(value);
			}

			value = IniReadValue("UMCCreationOptions", "UseCharge");
			if (!value.Equals(String.Empty))
			{
				settings.UseCharge = bool.Parse(value);
			}

			value = IniReadValue("UMCCreationOptions", "MinFeatureLengthPoints");
			if (!value.Equals(String.Empty))
			{
				settings.FeatureLengthMin = short.Parse(value);
			}

			value = IniReadValue("UMCCreationOptions", "LCGapMaxSize");
			if (!value.Equals(String.Empty))
			{
				settings.LCGapSizeMax = short.Parse(value);
			}

			value = IniReadValue("UMCCreationOptions", "IMSGapMaxSize");
			if (!value.Equals(String.Empty))
			{
				settings.IMSGapSizeMax = short.Parse(value);
			}

			value = IniReadValue("UMCCreationOptions", "LCMaxDaCorrection");
			if (!value.Equals(String.Empty))
			{
				int readValue = int.Parse(value);

				if (readValue < 0)
				{
					settings.LCDaltonCorrectionMax = 0;
				}
				else
				{
					settings.LCDaltonCorrectionMax = readValue;
				}
			}

			value = IniReadValue("UMCCreationOptions", "IMSMaxDaCorrection");
			if (!value.Equals(String.Empty))
			{
				int readValue = int.Parse(value);

				if (readValue < 0)
				{
					settings.IMSDaltonCorrectionMax = 0;
				}
				else
				{
					settings.IMSDaltonCorrectionMax = readValue;
				}
			}

			value = IniReadValue("UMCCreationOptions", "UMCFitScoreMinimum");
			if (!value.Equals(String.Empty))
			{
				settings.UMCFitScoreMinimum = float.Parse(value);
			}

			/*
			 * UMCSplittingOptions Settings
			 */
			value = IniReadValue("UMCSplittingOptions", "Split");
			if (!value.Equals(String.Empty))
			{
				settings.Split = bool.Parse(value);
			}

			value = IniReadValue("UMCSplittingOptions", "MinimumDifferenceInMedianPpmMassToSplit");
			if (!value.Equals(String.Empty))
			{
				settings.MinimumDifferenceInMedianPpmMassToSplit = short.Parse(value);
			}

			/*
			 * DriftProfile Settings
			 */
			value = IniReadValue("DriftProfileOptions", "UseConformationDetection");
			if (!value.Equals(String.Empty))
			{
				settings.UseConformationDetection = bool.Parse(value);
			}

			value = IniReadValue("DriftProfileOptions", "UseConformationIndex");
			if (!value.Equals(String.Empty))
			{
				settings.UseConformationIndex = bool.Parse(value);
			}

			value = IniReadValue("DriftProfileOptions", "ReportFittedTime");
			if (!value.Equals(String.Empty))
			{
				settings.ReportFittedTime = bool.Parse(value);
			}

			value = IniReadValue("DriftProfileOptions", "SmoothingStDev");
			if (!value.Equals(String.Empty))
			{
				settings.SmoothingStDev = double.Parse(value);
			}

			value = IniReadValue("DriftProfileOptions", "PeakWidthMinimum");
			if (!value.Equals(String.Empty))
			{
				settings.PeakWidthMinimum = int.Parse(value);
			}

			return settings;
		}

		/// <summary>
		/// Reads a value of an INI key
		/// </summary>
		/// <param name="Section">Section that contains the key</param>
		/// <param name="Key">The key of the value to be read</param>
		/// <returns>The value read as a String</returns>
		private String IniReadValue(String Section, String Key)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			int i = GetPrivateProfileString(Section, Key, "", stringBuilder, 255, this.m_path);
			return stringBuilder.ToString();
		}
	}
}
