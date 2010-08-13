using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UIMFLibrary;
using PNNLOmics.Algorithms.FeatureFinding.Control;
using PNNLOmics.Algorithms.FeatureFinding.Util;
using PNNLOmics.Data.Features;

namespace LCMSFeatureFinder
{
	class ConsoleApplication
	{
		private const int LC_DATA = 0;
		private const int IMS_DATA = 1;

		static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				PrintUsage();
				return;
			}

			if (args[0].ToUpper().Equals("/X"))
			{
				Settings.PrintExampleSettings();
				return;
			}

			String iniFile = ProcessFileLocation(args[0]);

			IniReader iniReader = new IniReader(iniFile);
			Settings settings = iniReader.CreateSettings();
			Logger logger = new Logger(settings);

			logger.Log("Loading settings from INI file: " + iniFile);
			logger.Log("Data Filters - ");
			logger.Log(" Minimum LC scan = " + settings.ScanLCMin);
			logger.Log(" Maximum LC scan = " + settings.ScanLCMax);
			logger.Log(" Minimum IMS scan = " + settings.ScanIMSMin);
			logger.Log(" Maximum IMS scan = " + settings.ScanIMSMax);
			logger.Log(" Maximum fit = " + settings.FitMax);
			logger.Log(" Minimum intensity = " + settings.IntensityMin);
			logger.Log(" Mono mass start = " + settings.MassMonoisotopicStart);
			logger.Log(" Mono mass end = " + settings.MassMonoisotopicEnd);

			int dataType = PeekAtIsosFile(settings);

			if (dataType < 0)
			{
				logger.Log("Unknown type of Isos file. Exiting.");
				return;
			}
			else
			{
				IsosReader isosReader = new IsosReader(ref settings, logger);
				logger.Log("Total number of MS Features in _isos.csv file = " + isosReader.NumOfUnfilteredMSFeatures);
				logger.Log("Total number of MS Features we'll consider = " + isosReader.MSFeatureList.Count);

				if (dataType == LC_DATA || settings.IgnoreIMSDriftTime)
				{
					logger.Log("Processing LC-MS Data...");
					RunLCMSFeatureFinder(settings, logger, isosReader);
				}
				else if (dataType == IMS_DATA)
				{
					logger.Log("Processing LC-IMS-MS Data...");

					DataReader uimfReader = null;

					if (settings.UseConformationDetection)
					{
						uimfReader = new UIMFLibrary.DataReader();
						if (!uimfReader.OpenUIMF(settings.InputDirectory + settings.InputFileName.Replace("_isos.csv", ".uimf")))
						{
							throw new FileNotFoundException("Could not find file '" + settings.InputDirectory + settings.InputFileName.Replace("_isos.csv", ".uimf") + "'.");
						}
						logger.Log("UIMF file has been opened.");
					}

					RunLCIMSMSFeatureFinder(settings, logger, isosReader, uimfReader);
				}

				logger.Log("Finished!");
				logger.CloseLog();
			}
		}

		/// <summary>
		/// Runs the necessary steps for LC-MS Feature Finding
		/// </summary>
		/// <param name="settings">Settings object</param>
		/// <param name="logger">Logger object</param>
		private static void RunLCMSFeatureFinder(Settings settings, Logger logger, IsosReader isosReader)
		{
			LCMSFeatureUtil lcmsFeatureUtil = new LCMSFeatureUtil(settings, logger);

			logger.Log("Creating UMCs...");
			List<LCMSFeature> lcmsFeatureList = lcmsFeatureUtil.ProcessMSFeatures(isosReader.MSFeatureList);
			logger.Log("Total Number of Unfiltered UMCs = " + lcmsFeatureList.Count);

			logger.Log("Filtering out short UMCs...");
			lcmsFeatureList = lcmsFeatureUtil.RefineLCMSFeaturesByFeatureLength(lcmsFeatureList);
			logger.Log("Total Number of Filtered UMCs = " + lcmsFeatureList.Count);

			if (settings.LCDaltonCorrectionMax > 0)
			{
				List<int> corrections = new List<int>();
				int featuresWithDaErrors = 0;
				int numberOfCorrections = 0;

				foreach (LCMSFeature lcmsFeature in lcmsFeatureList)
				{
					if (lcmsFeature.DaError)
					{
						featuresWithDaErrors++;
						foreach (MSFeature msFeature in lcmsFeature.MSFeatureList)
						{
							if (msFeature.Corrected)
							{
								while (corrections.Count <= Math.Abs(msFeature.MassOffset))
								{
									corrections.Add(0);
								}

								corrections[Math.Abs(msFeature.MassOffset)]++;

								msFeature.Corrected = false;
								numberOfCorrections++;
							}
						}
					}
				}
			}

			if (settings.Split)
			{
				logger.Log("Splitting UMCs by gap size...");
				lcmsFeatureList = lcmsFeatureUtil.SplitLCMSFeaturesByGapSize(lcmsFeatureList);
				logger.Log("Filtering out short UMCs...");
				lcmsFeatureList = lcmsFeatureUtil.RefineLCMSFeaturesByFeatureLength(lcmsFeatureList);
				logger.Log("Total Number of UMCs after splitting = " + lcmsFeatureList.Count);
			}

			logger.Log("Calculating UMC statistics...");
			lcmsFeatureList = lcmsFeatureUtil.CalculateLCMSFeatureStatistics(lcmsFeatureList);

			double numOfMSFeaturesMappedToLCMSFeatures = lcmsFeatureUtil.NumOfMSFeaturesMappedToLCMSFeatures;
			logger.Log("Total Number of peaks that map to UMCs = " + numOfMSFeaturesMappedToLCMSFeatures);
			logger.Log("Percentage of unfiltered peaks that map to UMCs = " + string.Format("{0:00.00%}", numOfMSFeaturesMappedToLCMSFeatures / isosReader.NumOfUnfilteredMSFeatures));
			logger.Log("Percentage of filtered peaks that map to UMCs = " + string.Format("{0:00.00%}", numOfMSFeaturesMappedToLCMSFeatures / isosReader.MSFeatureList.Count));

			logger.Log("Writing output files...");
			FeatureUtil.WriteUMCListToFile(lcmsFeatureList, settings);
			//FeatureUtil.WriteMSFeaturesToFile(lcmsFeatureList, settings);

			//logger.Log("Creating abundance profiles...");
			//FeatureUtil.CreateAbundanceProfile(lcmsFeatureList, settings);

			logger.Log("Creating filtered Isos file...");
			List<MSFeature> msFeatureList = new List<MSFeature>();
			foreach (LCMSFeature lcmsFeature in lcmsFeatureList)
			{
				msFeatureList.AddRange(lcmsFeature.MSFeatureList);
			}

			IsosWriter isosWriter = new IsosWriter(settings, msFeatureList, isosReader.ColumnMap);
		}

		/// <summary>
		/// Runs the necessary steps for LC-IMS-MS Feature Finding
		/// </summary>
		/// <param name="settings">Settings object</param>
		/// <param name="logger">Logger object</param>
		private static void RunLCIMSMSFeatureFinder(Settings settings, Logger logger, IsosReader isosReader, DataReader uimfReader)
		{
			IMSMSFeatureUtil imsmsFeatureUtil = new IMSMSFeatureUtil(settings, logger);

			// A smart method is still needed in this step.
			logger.Log("Creating IMS-MS Features...");
			List<IMSMSFeature> imsmsFeatureList = imsmsFeatureUtil.ProcessMSFeatures(isosReader.MSFeatureList);
			logger.Log("Total Number of Unfiltered IMS-MS Features = " + imsmsFeatureList.Count);

			logger.Log("Filtering out short IMS-MS Features...");
			imsmsFeatureList = imsmsFeatureUtil.RefineIMSMSFeaturesByFeatureLength(imsmsFeatureList);
			logger.Log("Total Number of Filtered IMS-MS Features = " + imsmsFeatureList.Count);

			//imsmsFeatureList = imsmsFeatureUtil.GetIMSMSFeaturesProfileStatistics(imsmsFeatureList);
			//FeatureUtil.CheckMassAbundanceRelationship(imsmsFeatureList, settings);

			if (settings.IMSDaltonCorrectionMax > 0)
			{
				List<int> corrections = new List<int>();
				int featuresWithDaErrors = 0;
				int numberOfCorrections = 0;

				foreach (IMSMSFeature imsmsFeature in imsmsFeatureList)
				{
					if (imsmsFeature.DaError)
					{
						featuresWithDaErrors++;
						foreach (MSFeature msFeature in imsmsFeature.MSFeatureList)
						{
							if (msFeature.Corrected)
							{
								while (corrections.Count <= Math.Abs(msFeature.MassOffset))
								{
									corrections.Add(0);
								}

								corrections[Math.Abs(msFeature.MassOffset)]++;

								msFeature.Corrected = false;
								numberOfCorrections++;
							}
						}
					}
				}

				logger.Log("IMS-MS Features with Errors = " + featuresWithDaErrors);
				logger.Log("MS Features Corrected = " + numberOfCorrections);

				for (int i = 1; i < corrections.Count; i++)
				{
					logger.Log(i + " Da Corrections = " + corrections[i]);
				}
			}

			if (settings.UseConformationDetection)
			{
				logger.Log("Drift Time Conformer Detection...");
				imsmsFeatureList = imsmsFeatureUtil.PerformConformationDetection(imsmsFeatureList, uimfReader);
				logger.Log("New Total Number of Filtered IMS-MS Features = " + imsmsFeatureList.Count);
			}

			logger.Log("Calculating IMS-MS Feature statistics...");
			imsmsFeatureList = imsmsFeatureUtil.CalculateIMSMSFeatureStatistics(imsmsFeatureList);
			//FeatureUtil.WriteMSFeaturesToFile(imsmsFeatureList, settings);

			//imsmsFeatureUtil.WriteIMSMSFeaturesToFile(imsmsFeatureList);

			logger.Log("Creating LC-IMS-MS Features...");
			List<LCIMSMSFeature> lcimsmsFeatureList = imsmsFeatureUtil.ProcessIMSMSFeatures(imsmsFeatureList);
			logger.Log("Total Number of Unfiltered LC-IMS-MS Features = " + lcimsmsFeatureList.Count);

			if (settings.UseConformationDetection)
			{
				logger.Log("Calculating LC-IMS-MS Feature Fit Scores...");
				lcimsmsFeatureList = imsmsFeatureUtil.CalculateFitScore(lcimsmsFeatureList);

				logger.Log("Filtering LC-IMS-MS Features based on Fit Scores...");
				lcimsmsFeatureList = imsmsFeatureUtil.RefineLCIMSMSFeatures(lcimsmsFeatureList);
				logger.Log("Total Number of Filtered LC-IMS-MS Features = " + lcimsmsFeatureList.Count);
			}

			if (settings.LCDaltonCorrectionMax > 0)
			{
				List<int> corrections = new List<int>();
				int featuresWithDaErrors = 0;
				int numberOfCorrections = 0;

				foreach (LCIMSMSFeature lcimsmsFeature in lcimsmsFeatureList)
				{
					if (lcimsmsFeature.DaError)
					{
						featuresWithDaErrors++;
						foreach (MSFeature msFeature in lcimsmsFeature.MSFeatureList)
						{
							if (msFeature.Corrected)
							{
								while (corrections.Count <= Math.Abs(msFeature.MassOffset))
								{
									corrections.Add(0);
								}

								corrections[Math.Abs(msFeature.MassOffset)]++;

								msFeature.Corrected = false;
								numberOfCorrections++;
							}
						}
					}
				}

				logger.Log("LC-IMS-MS Features with Errors = " + featuresWithDaErrors);
				logger.Log("MS Features Corrected = " + numberOfCorrections);

				for (int i = 1; i < corrections.Count; i++)
				{
					logger.Log(i + " Da Corrections = " + corrections[i]);
				}
			}

			logger.Log("Calculating LC-IMS-MS Feature statistics...");
			lcimsmsFeatureList = imsmsFeatureUtil.CalculateLCIMSMSFeatureStatistics(lcimsmsFeatureList);

			logger.Log("Writing output files...");
			FeatureUtil.WriteUMCListToFile(lcimsmsFeatureList, settings);
			//FeatureUtil.WriteMSFeaturesToFile(lcimsmsFeatureList, settings);

			logger.Log("Creating filtered Isos file...");
			List<MSFeature> msFeatureList = new List<MSFeature>();
			foreach (IMSMSFeature imsmsFeature in imsmsFeatureList)
			{
				msFeatureList.AddRange(imsmsFeature.MSFeatureList);
			}

			IsosWriter isosWriter = new IsosWriter(settings, msFeatureList, isosReader.ColumnMap);
		}

		/// <summary>
		/// Checks to see what type of data is being processed
		/// </summary>
		/// <param name="settings">Settings object</param>
		/// <returns>LC_DATA if LC Data is being processed, IMS_DATA if IMS Data is being processed, -1 if error</returns>
		private static int PeekAtIsosFile(Settings settings)
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			StreamReader isosFileReader = new StreamReader(settings.InputDirectory + settings.InputFileName);

			String firstLine = isosFileReader.ReadLine();

			if (firstLine == null)
			{
				return -1;
			}

			String[] columnTitles = firstLine.Split('\t', ',', '\n');
			foreach (String column in columnTitles)
			{
				if (column.Equals("ims_scan_num"))
				{
					isosFileReader.Close();
					return IMS_DATA;
				}
			}

			isosFileReader.Close();
			return LC_DATA;
		}

		/// <summary>
		/// Formats the file location into a usable format
		/// </summary>
		/// <param name="filename">Original file location</param>
		/// <returns>Processed file location</returns>
		private static string ProcessFileLocation(string fileLocation)
		{
			// Replace all slashes to backslashes since we are working with a Windows directory 
            //TODO: System file separator?
			fileLocation = fileLocation.Replace("/", "\\");
                
			// If the string does not contain ":\" or "\\", move on.
			if (!fileLocation.Contains(":\\") && !fileLocation.StartsWith("\\\\"))
			{
				// Append "." to the front of the string if in the form of "\blabla"
				if (fileLocation.StartsWith("\\"))
				{
					return "." + fileLocation;
				}
				// Append ".\" to the front of the string if in the form of "blabla"
				else
				{
					return ".\\" + fileLocation;
				}
			}

			// filename is in the form of "C:\blabla" or "\\blabla"
			return fileLocation;
		}

		/// <summary>
		/// Prints the correct usage of the application
		/// </summary>
		private static void PrintUsage()
		{
			Console.WriteLine("");
			Console.WriteLine("Syntax: LCMSFeatureFinder.exe SettingsFile.ini\n");
			Console.WriteLine("The settings file defines the input file path and the outputdirectory.");
			Console.WriteLine("It also defines a series of settings used to aid the Feature Finder.\n");
			Console.WriteLine("To see an example settings file, use LCMSFeatureFinder.exe /X");
		}
	}
}
