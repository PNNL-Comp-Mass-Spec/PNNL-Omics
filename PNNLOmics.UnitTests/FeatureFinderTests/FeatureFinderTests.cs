using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Algorithms.FeatureFinding.Control;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.IO;
using PNNLOmics.Algorithms.FeatureFinding.Data;
using PNNLOmics.Algorithms.FeatureFinding.Util;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.ConformationDetection.Util;

namespace PNNLOmics.UnitTests.FeatureFinderTests
{
	public class FeatureFinderTests
	{
		[Test]
		public void PrintExampleSettings()
		{
			Settings.PrintExampleSettings();
		}

		[Test]
		public void ReadINIFileAndCreateSettings()
		{
			IniReader iniReader = new IniReader("../../lib/TestSettings.ini");
			Settings settings = iniReader.CreateSettings();

			Assert.AreEqual("Test_Small_LC_isos.csv", settings.InputFileName);
			Assert.AreEqual("..\\..\\lib\\", settings.OutputDirectory);
			Assert.AreEqual(0.15f, settings.FitMax);
			Assert.AreEqual(900, settings.IntensityMin);
			Assert.AreEqual(0, settings.ScanIMSMin);
			Assert.AreEqual(int.MaxValue, settings.ScanIMSMax);
			Assert.AreEqual(0, settings.ScanLCMin);
			Assert.AreEqual(int.MaxValue, settings.ScanLCMax);
			Assert.AreEqual(0, settings.MassMonoisotopicStart);
			Assert.AreEqual(15000, settings.MassMonoisotopicEnd);
			Assert.AreEqual(false, settings.IgnoreIMSDriftTime);
			Assert.AreEqual(20, settings.MassMonoisotopicConstraint);
			Assert.AreEqual(true, settings.MassMonoisotopicConstraintIsPPM);
			Assert.AreEqual(true, settings.UseGenericNET);
			Assert.AreEqual(true, settings.UseCharge);
			Assert.AreEqual(3, settings.FeatureLengthMin);
			Assert.AreEqual(4, settings.LCGapSizeMax);
			Assert.AreEqual(4, settings.IMSGapSizeMax);
			Assert.AreEqual(1, settings.LCDaltonCorrectionMax);
			Assert.AreEqual(1, settings.IMSDaltonCorrectionMax);
			Assert.AreEqual(true, settings.Split);
			Assert.AreEqual(4, settings.MinimumDifferenceInMedianPpmMassToSplit);
			Assert.AreEqual(0.9f, settings.UMCFitScoreMinimum);
			Assert.AreEqual(true, settings.UseConformationDetection);
			Assert.AreEqual(false, settings.UseConformationIndex);
			Assert.AreEqual(false, settings.ReportFittedTime);
			Assert.AreEqual(0.20d, settings.SmoothingStDev);
			Assert.AreEqual(3, settings.PeakWidthMinimum);
		}

		[Test]
		public void LoggerTest()
		{
			Settings settings = new Settings();
			settings.InputFileName = "../../lib/Test_Small_LC_isos.csv";
			Logger logger = new Logger(settings);
			Assert.NotNull(logger);

			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			Assert.True(File.Exists(settings.OutputDirectory + baseFileName + "_FeatureFinder_Log.txt"));

			logger.Log("This is a test logging statement.");
			logger.CloseLog();

			Assert.Throws<System.ObjectDisposedException>(delegate
			{
				logger.Log("This should fail");
			});

			// Clean-up created files
			File.Delete(settings.OutputDirectory + baseFileName + "_FeatureFinder_Log.txt");
		}

		[Test]
		public void ReadIsosLC()
		{
			Settings settings = new Settings();
			settings.IntensityMin = 0;
			settings.FitMax = 1;
			settings.InputFileName = "../../lib/Test_Small_LC_isos.csv";
			Logger logger = new Logger(settings);

			IsosReader isosReader = new IsosReader(ref settings, logger);

			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			Assert.True(File.Exists(baseFileName + "_Filtered_isos.csv"));

			Assert.AreEqual(57, isosReader.MSFeatureList.Count);
			Assert.AreEqual(58, isosReader.NumOfUnfilteredMSFeatures);
			Assert.AreEqual(12, isosReader.ColumnMap.Count);
			Assert.AreEqual(4, ScanLCMapHolder.ScanLCMap.Count);

			Assert.AreEqual(1, ScanLCMapHolder.ScanLCMap[1]);
			Assert.AreEqual(22, ScanLCMapHolder.ScanLCMap[2]);
			Assert.AreEqual(27, ScanLCMapHolder.ScanLCMap[3]);
			Assert.AreEqual(28, ScanLCMapHolder.ScanLCMap[4]);

			// Clean-up created files
			logger.CloseLog();
			File.Delete(baseFileName + "_FeatureFinder_Log.txt");
			File.Delete(baseFileName + "_Filtered_isos.csv");
			ScanLCMapHolder.Reset();
		}

		[Test]
		public void ReadIsosIMS()
		{
			Settings settings = new Settings();
			settings.IntensityMin = 0;
			settings.FitMax = 1;
			settings.InputFileName = "../../lib/Test_Small_IMS_isos.csv";
			Logger logger = new Logger(settings);

			IsosReader isosReader = new IsosReader(ref settings, logger);

			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			Assert.True(File.Exists(baseFileName + "_Filtered_isos.csv"));

			Assert.AreEqual(97, isosReader.MSFeatureList.Count);
			Assert.AreEqual(97, isosReader.NumOfUnfilteredMSFeatures);
			Assert.AreEqual(18, isosReader.ColumnMap.Count);
			Assert.AreEqual(4, ScanLCMapHolder.ScanLCMap.Count);

			Assert.AreEqual(1, ScanLCMapHolder.ScanLCMap[1]);
			Assert.AreEqual(2, ScanLCMapHolder.ScanLCMap[2]);
			Assert.AreEqual(3, ScanLCMapHolder.ScanLCMap[3]);
			Assert.AreEqual(4, ScanLCMapHolder.ScanLCMap[4]);

			// Clean-up created files
			logger.CloseLog();
			File.Delete(baseFileName + "_FeatureFinder_Log.txt");
			File.Delete(baseFileName + "_Filtered_isos.csv");
			ScanLCMapHolder.Reset();
		}

		[Test]
		public void CreateLCMSFeatures()
		{
			IniReader iniReader = new IniReader("../../lib/TestSettings.ini");
			Settings settings = iniReader.CreateSettings();
			Logger logger = new Logger(settings);
			IsosReader isosReader = new IsosReader(ref settings, logger);

			LCMSFeatureUtil lcmsFeatureUtil = new LCMSFeatureUtil(settings, logger);
			List<LCMSFeature> lcmsFeatureList = lcmsFeatureUtil.ProcessMSFeatures(isosReader.MSFeatureList);

			Assert.AreEqual(8, lcmsFeatureList.Count);

			// Clean-up created files
			logger.CloseLog();
			String baseFileName = Regex.Split(settings.OutputDirectory + settings.InputFileName, "_isos")[0];
			File.Delete(baseFileName + "_FeatureFinder_Log.txt");
			File.Delete(baseFileName + "_Filtered_isos.csv");
			ScanLCMapHolder.Reset();
		}

		[Test]
		public void CreateIMSMSFeatures()
		{
			IniReader iniReader = new IniReader("../../lib/TestSettings.ini");
			Settings settings = iniReader.CreateSettings();
			settings.InputFileName = "Test_Small_IMS_isos.csv";
			Logger logger = new Logger(settings);
			IsosReader isosReader = new IsosReader(ref settings, logger);

			IMSMSFeatureUtil imsmsFeatureUtil = new IMSMSFeatureUtil(settings, logger);
			List<IMSMSFeature> imsmsFeatureList = imsmsFeatureUtil.CreateIMSMSFeatures(isosReader.MSFeatureList);
			Assert.AreEqual(20, imsmsFeatureList.Count);

			imsmsFeatureList = imsmsFeatureUtil.RefineIMSMSFeaturesByFeatureLength(imsmsFeatureList);
			Assert.AreEqual(9, imsmsFeatureList.Count);

			// Clean-up created files
			logger.CloseLog();
			String baseFileName = Regex.Split(settings.OutputDirectory + settings.InputFileName, "_isos")[0];
			File.Delete(baseFileName + "_FeatureFinder_Log.txt");
			File.Delete(baseFileName + "_Filtered_isos.csv");
			ScanLCMapHolder.Reset();
		}

		[Test]
		public void CreateLCIMSMSFeatures()
		{
			IniReader iniReader = new IniReader("../../lib/TestSettings.ini");
			Settings settings = iniReader.CreateSettings();
			settings.InputFileName = "Test_Small_IMS_isos.csv";
			Logger logger = new Logger(settings);
			IsosReader isosReader = new IsosReader(ref settings, logger);

			IMSMSFeatureUtil imsmsFeatureUtil = new IMSMSFeatureUtil(settings, logger);
			List<IMSMSFeature> imsmsFeatureList = imsmsFeatureUtil.CreateIMSMSFeatures(isosReader.MSFeatureList);
			List<LCIMSMSFeature> lcimsmsFeatureList = imsmsFeatureUtil.CreateLCIMSMSFeatures(imsmsFeatureList);
			Assert.AreEqual(11, lcimsmsFeatureList.Count);

			imsmsFeatureList = imsmsFeatureUtil.RefineIMSMSFeaturesByFeatureLength(imsmsFeatureList);
			lcimsmsFeatureList = imsmsFeatureUtil.CreateLCIMSMSFeatures(imsmsFeatureList);
			Assert.AreEqual(4, lcimsmsFeatureList.Count);

			// Clean-up created files
			logger.CloseLog();
			String baseFileName = Regex.Split(settings.OutputDirectory + settings.InputFileName, "_isos")[0];
			File.Delete(baseFileName + "_FeatureFinder_Log.txt");
			File.Delete(baseFileName + "_Filtered_isos.csv");
			ScanLCMapHolder.Reset();
		}

		[Test]
		public void IsosWriter()
		{
			IniReader iniReader = new IniReader("../../lib/TestSettings.ini");
			Settings settings = iniReader.CreateSettings();
			settings.InputFileName = "Test_Small_IMS_isos.csv";
			Logger logger = new Logger(settings);
			IsosReader isosReader = new IsosReader(ref settings, logger);

			IsosWriter isosWriter = new IsosWriter(settings, isosReader.MSFeatureList, isosReader.ColumnMap);

			// Clean-up created files
			logger.CloseLog();
			String baseFileName = Regex.Split(settings.OutputDirectory + settings.InputFileName, "_isos")[0];
			File.Delete(baseFileName + "_FeatureFinder_Log.txt");
			File.Delete(baseFileName + "_Filtered_isos.csv");
			ScanLCMapHolder.Reset();
		}

		[Test]
		public void WriteLCMSFeaturesToFile()
		{
			IniReader iniReader = new IniReader("../../lib/TestSettings.ini");
			Settings settings = iniReader.CreateSettings();
			Logger logger = new Logger(settings);
			IsosReader isosReader = new IsosReader(ref settings, logger);

			LCMSFeatureUtil lcmsFeatureUtil = new LCMSFeatureUtil(settings, logger);
			List<LCMSFeature> lcmsFeatureList = lcmsFeatureUtil.ProcessMSFeatures(isosReader.MSFeatureList);
			lcmsFeatureList = lcmsFeatureUtil.CalculateLCMSFeatureStatistics(lcmsFeatureList);

			FeatureUtil.WriteUMCListToFile<LCMSFeature>(lcmsFeatureList, settings);

			String baseFileName = Regex.Split(settings.OutputDirectory + settings.InputFileName, "_isos")[0];

			Assert.True(File.Exists(baseFileName + "_LCMSFeatures.txt"));
			Assert.True(File.Exists(baseFileName + "_LCMSFeatureToPeakMap.txt"));
			Assert.True(File.Exists(baseFileName + "_CMC.txt"));

			Assert.AreEqual(9, File.ReadAllLines(baseFileName + "_LCMSFeatures.txt").Length);
			Assert.AreEqual(16, File.ReadAllLines(baseFileName + "_LCMSFeatureToPeakMap.txt").Length);
			Assert.AreEqual(9, File.ReadAllLines(baseFileName + "_CMC.txt").Length);

			// Clean-up created files
			logger.CloseLog();
			File.Delete(baseFileName + "_FeatureFinder_Log.txt");
			File.Delete(baseFileName + "_Filtered_isos.csv");
			File.Delete(baseFileName + "_LCMSFeatures.txt");
			File.Delete(baseFileName + "_LCMSFeatureToPeakMap.txt");
			File.Delete(baseFileName + "_CMC.txt");
			ScanLCMapHolder.Reset();
		}

		[Test]
		public void TestAverageRank()
		{
			List<double> xValueList = new List<double>()
			{
				0.8, 1.2,1.2,2.3,18//0,20,28,27,50,29,7,17,6,12
			};
			List<double> rankListExpect = new List<double>()
			{
				1, 2.5, 2.5,4,5//1,6,8,7,10,9,3,5,2,4
			};

			List<double> rankList = MathUtil.AverageRank(xValueList);

			double tmp = 0.0;
			for (int i = 0; i < rankList.Count; i++)
			{
				tmp += Math.Abs(rankList[i] - rankListExpect[i]);
			}
			Assert.AreEqual(0, tmp);
		}

		[Test]
		public void TestSpearmanCorr()
		{
			List<double> xValueList = new List<double>()
			{
				86,97,99,100,101,103,106,110,112,113
			};

			List<double> yValueList = new List<double>()
			{
				0,20,28,27,50,29,7,17,6,12
			};

			double corr = MathUtil.SpearmanCorr(xValueList, yValueList);
			Assert.AreEqual(corr, 1 - 6.0 * 194.0 / (10 * (100 - 1)));
		}

		[Test]
		public void TestPearsonCorr()
		{
			List<double> xValueList = new List<double>()
			{
				86,97,99,100,101,103,106,110,112,113
			};

			List<double> yValueList = new List<double>()
			{
				0,20,28,27,50,29,7,17,6,12
			};

			double corr = MathUtil.PearsonCorr(xValueList, yValueList);
			Assert.AreEqual(corr, -0.037601473846875955);
		}
	}
}
