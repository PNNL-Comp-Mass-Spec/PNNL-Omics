using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace PNNLOmics.Algorithms.FeatureFinding.Control
{
	public class IniReader
	{
		private String m_path;

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(String section, String key, String def, StringBuilder retVal, int size, String filePath);

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
					settings.LCDaCorrectionMax = 0;
				}
				else
				{
					settings.LCDaCorrectionMax = readValue;
				}
			}

			value = IniReadValue("UMCCreationOptions", "IMSMaxDaCorrection");
			if (!value.Equals(String.Empty))
			{
				int readValue = int.Parse(value);

				if (readValue < 0)
				{
					settings.IMSDaCorrectionMax = 0;
				}
				else
				{
					settings.IMSDaCorrectionMax = readValue;
				}
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

		private String IniReadValue(String Section, String Key)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			int i = GetPrivateProfileString(Section, Key, "", stringBuilder, 255, this.m_path);
			return stringBuilder.ToString();
		}
	}
}
