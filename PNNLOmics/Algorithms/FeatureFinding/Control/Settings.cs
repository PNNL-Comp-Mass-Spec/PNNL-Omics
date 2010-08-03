using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.FeatureFinding.Control
{
	/// <summary>
	/// Class to hold the user-defined Settings for the Feature Finding algorithm
	/// </summary>
	public class Settings
	{
		public string InputDirectory { get; set; }
		public string InputFileName { get; set; }
		public string OutputDirectory { get; set; }

		public short FeatureLengthMin { get; set; }
		public short MinimumDifferenceInMedianPpmMassToSplit { get; set; }
		public short LCGapSizeMax { get; set; }
		public short IMSGapSizeMax { get; set; }

		public int LCDaltonCorrectionMax { get; set; }
		public int IMSDaltonCorrectionMax { get; set; }
		public int ScanIMSMin { get; set; }
		public int ScanIMSMax { get; set; }
		public int ScanLCMin { get; set; }
		public int ScanLCMax { get; set; }
		public int PeakWidthMinimum { get; set; }

		public float FitMax { get; set; }
		public float IntensityMin { get; set; }
		public float MassMonoisotopicConstraint { get; set; }
		public float UMCFitScoreMinimum { get; set; }

		public double MassMonoisotopicStart { get; set; }
		public double MassMonoisotopicEnd { get; set; }
        public double SmoothingStDev { get; set; }

		public bool MassMonoisotopicConstraintIsPPM { get; set; }
		public bool UseGenericNET { get; set; }
		public bool UseCharge { get; set; }
		public bool Split { get; set; }
		public bool UseConformationDetection { get; set; }
		public bool UseConformationIndex { get; set; }
		public bool ReportFittedTime { get; set; }
		public bool IgnoreIMSDriftTime { get; set; }

		/// <summary>
		/// This constructor will automatically select default values for every setting.
		/// </summary>
		public Settings()
		{
			// Default Settings
			this.InputDirectory = "";
			this.InputFileName = "";
			this.OutputDirectory = "";
			this.FitMax = 0.15f;
			this.IntensityMin = 2500;
			this.ScanIMSMin = 0;
			this.ScanIMSMax = int.MaxValue;
			this.ScanLCMin = 0;
			this.ScanLCMax = int.MaxValue;
			this.MassMonoisotopicStart = 0;
			this.MassMonoisotopicEnd = 15000;
			this.MassMonoisotopicConstraint = 20f;
			this.MassMonoisotopicConstraintIsPPM = true;
			this.FeatureLengthMin = 3;
			this.UseGenericNET = true;
			this.UseCharge = false;
			this.LCGapSizeMax = 5;
			this.IMSGapSizeMax = 5;
			this.MinimumDifferenceInMedianPpmMassToSplit = 4;
			this.Split = true;
			this.LCDaltonCorrectionMax = 3;
			this.SmoothingStDev = 0.35;
			this.UMCFitScoreMinimum = 0f;
			this.PeakWidthMinimum = 3;
			this.UseConformationDetection = true;
			this.UseConformationIndex = false;
			this.ReportFittedTime = false;
			this.IgnoreIMSDriftTime = false;
		}

		/// <summary>
		/// Prints out an example Settings file to the console.
		/// </summary>
		public static void PrintExampleSettings()
		{
			Console.WriteLine("");
			Console.WriteLine("[Files]");
			Console.WriteLine("InputFileName=InputFile_isos.csv");
			Console.WriteLine("OutputDirectory=C:\\");
			Console.WriteLine("[DataFilters]");
			Console.WriteLine("MaxIsotopicFit=0.15");
			Console.WriteLine("MinimumIntensity=0");
			Console.WriteLine("IMSMinScan=0");
			Console.WriteLine("IMSMaxScan=0");
			Console.WriteLine("LCMinScan=0");
			Console.WriteLine("LCMaxScan=0");
			Console.WriteLine("MonoMassStart=0");
			Console.WriteLine("MonoMassEnd=15000");
			Console.WriteLine("[UMCCreationOptions]");
			Console.WriteLine("IgnoreIMSDriftTime=False");
			Console.WriteLine("MonoMassConstraint=12");
			Console.WriteLine("MonoMassConstraintIsPPM=True");
			Console.WriteLine("UsegenericNET=True");
			Console.WriteLine("UseCharge=True");
			Console.WriteLine("MinFeatureLengthPoints=3");
			Console.WriteLine("LCGapMaxSize=4");
			Console.WriteLine("IMSGapMaxSize=4");
			Console.WriteLine("LCMaxDaCorrection=0");
			Console.WriteLine("IMSMaxDaCorrection=0");
			Console.WriteLine("UMCFitScoreMinimum=0.9");
			Console.WriteLine("[UMCSplittingOptions]");
			Console.WriteLine("Split=True");
			Console.WriteLine("MinimumDifferenceInMedianPpmMassToSplit=4");
			Console.WriteLine("[DriftProfileOptions]");
			Console.WriteLine("UseConformationDetection=True");
			Console.WriteLine("UseConformationIndex=False");
			Console.WriteLine("ReportFittedTime=False");
			Console.WriteLine("PeakWidthMinimum=3");
			Console.WriteLine("SmoothingStDev=0.35");
		}
	}
}
