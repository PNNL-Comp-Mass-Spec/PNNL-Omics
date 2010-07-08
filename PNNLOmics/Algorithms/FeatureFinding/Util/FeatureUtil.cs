using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.SQLite;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureFinding.Data;
using PNNLOmics.Algorithms.FeatureFinding.Control;

namespace PNNLOmics.Algorithms.FeatureFinding.Util
{
	public static class FeatureUtil
	{
		public static void WriteLCFeatureBaseListToFile<T>(List<T> featureList, Settings settings) where T : UMC
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			String outputDirectory = "";

			if (!settings.OutputDirectory.Equals(String.Empty))
			{
				outputDirectory = settings.OutputDirectory + "\\";
			}

			TextWriter featureWriter = new StreamWriter(outputDirectory + baseFileName + "_LCMSFeatures.txt");
			TextWriter mapWriter = new StreamWriter(outputDirectory + baseFileName + "_LCMSFeatureToPeakMap.txt");
			TextWriter cmcWriter = new StreamWriter(outputDirectory + baseFileName + "_CMC.txt");

			featureWriter.WriteLine("Feature_Index\tMonoisotopic_Mass\tAverage_Mono_Mass\tUMC_MW_Min\tUMC_MW_Max\tScan_Start\tScan_End\tScan\tUMC_Member_Count\tMax_Abundance\tAbundance\tClass_Rep_MZ\tClass_Rep_Charge\tCharge_Max\tDrift_Time\tConformation_Fit_Score");
			mapWriter.WriteLine("Feature_Index\tPeak_Index\tFiltered_Peak_Index");
			cmcWriter.WriteLine("Feature_Index\tCharge_State\tIntensity");

			foreach (T feature in featureList)
			{
				featureWriter.WriteLine(feature.ID + "\t" + feature.MassMonoisotopicMedian.ToString("0.00000") + "\t" + feature.MassMonoisotopicMedian.ToString("0.00000") + "\t"
							+ feature.MassMonoisotopicMinimum + "\t" + feature.MassMonoisotopicMaximum + "\t" + ScanLCMapHolder.ScanLCMap[feature.ScanLCStart] + "\t" + ScanLCMapHolder.ScanLCMap[feature.ScanLCEnd] + "\t"
							+ ScanLCMapHolder.ScanLCMap[feature.ScanLCOfMaxAbundance] + "\t" + feature.MSFeatureList.Count + "\t" + feature.AbundanceMaximum + "\t" + feature.AbundanceSum + "\t"
							+ feature.MZ + "\t" + feature.ChargeState + "\t" + feature.ChargeMaximum + "\t" + feature.DriftTime + "\t" + feature.ConformationFitScore);

				Dictionary<int, int> cmcMap = new Dictionary<int, int>();

				foreach (MSFeature msFeature in feature.MSFeatureList)
				{
					mapWriter.WriteLine(feature.ID + "\t" + msFeature.IndexInFile + "\t" + msFeature.ID);

					if (cmcMap.ContainsKey(msFeature.ChargeState))
					{
						cmcMap[msFeature.ChargeState] += msFeature.Abundance;
					}
					else
					{
						cmcMap[msFeature.ChargeState] = msFeature.Abundance;
					}
				}

				foreach (byte key in cmcMap.Keys)
				{
					cmcWriter.WriteLine(feature.ID + "\t" + key + "\t" + cmcMap[key]);
				}
			}

			featureWriter.Close();
			mapWriter.Close();
		}

		public static void WriteLCFeatureBaseListToSQLite<T>(List<T> featureList, Settings settings) where T : UMC
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			String outputDirectory = "";

			if (!settings.OutputDirectory.Equals(String.Empty))
			{
				outputDirectory = settings.OutputDirectory + "\\";
			}

			//TextWriter featureWriter = new StreamWriter(outputDirectory + baseFileName + "_LCMSFeatures.db3");

			// TODO: Should write multiple tables to 1 SQLite file

			// If a SQLite file already exists, delete it
			if (File.Exists(outputDirectory + baseFileName + "_LCMSFeatures.db3"))
			{
				File.Delete(outputDirectory + baseFileName + "_LCMSFeatures.db3");
			}

			using (SQLiteConnection conn = new SQLiteConnection())
			{
				using (SQLiteTransaction transaction = conn.BeginTransaction())
				{

				}
			}

			throw new NotImplementedException();
		}

		public static void CreateAbundanceProfile<T>(List<T> featureList, Settings settings) where T : UMC
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			String outputDirectory = "";

			if (!settings.OutputDirectory.Equals(String.Empty))
			{
				outputDirectory = settings.OutputDirectory + "\\";
			}

			TextWriter profileWriter = new StreamWriter(outputDirectory + baseFileName + "_AbundanceProfiles.txt");

			profileWriter.WriteLine("Feature_index\tMass\tCharge\tScan\tAbundance");

			foreach (T feature in featureList)
			{
				List<MSFeature> msFeatureList = feature.MSFeatureList;
				//msFeatureList.Sort(MSFeature.FrameComparison);
				msFeatureList.Sort(new Comparison<MSFeature>(Feature.ScanLCAndChargeStateComparison));

				for (int i = 0; i < msFeatureList.Count; i++)
				{
					MSFeature currentMSFeature = msFeatureList[i];
					int currentScanLC = currentMSFeature.ScanLC;
					int currentCharge = currentMSFeature.ChargeState;
					int totalbundance = 0;

					while (i < msFeatureList.Count)
					{
						currentMSFeature = msFeatureList[i];

						if (currentMSFeature.ScanLC != currentScanLC)
						{
							i--;
							break;
						}

						totalbundance += currentMSFeature.Abundance;
						i++;
					}

					profileWriter.WriteLine(feature.ID + "\t" + feature.MassMonoisotopicMedian + "\t" + currentCharge + "\t" + currentScanLC + "\t" + totalbundance);
				}
			}
		}

		public static void CorrectMassMostAbundant<T>(T feature) where T : UMC
		{
			if (feature.DaError)
			{
				double massReference = feature.MassOfMaxAbundance;

				foreach (MSFeature msFeature in feature.MSFeatureList)
				{
					int differenceInt = (int)Math.Round(massReference - msFeature.MassMonoisotopic);

					if (differenceInt != 0)
					{
						msFeature.MassMonoisotopic += differenceInt;
						//msFeature.Mz = (msFeature.MassMonoisotopic / msFeature.Charge) + 1.00727849;
						msFeature.MZCorrected = (msFeature.MassMonoisotopic / msFeature.ChargeState) + 1.00727849;
						msFeature.Corrected = true;

						if (msFeature.Corrected) msFeature.CorrectedValue += differenceInt;
						else msFeature.CorrectedValue = differenceInt;
					}
				}
			}
		}

		public static void WriteMSFeaturesToFile(List<LCMSFeature> featureList, Settings settings)
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			String outputDirectory = "";

			if (!settings.OutputDirectory.Equals(String.Empty))
			{
				outputDirectory = settings.OutputDirectory + "\\";
			}

			TextWriter msFeatureWriter = new StreamWriter(outputDirectory + baseFileName + "_MSFeatures.txt");

			msFeatureWriter.WriteLine("Feature_Index\tMS_Index\tFiltered_MS_Index\tLC_Scan\tMono_mass\tCharge\tAbundance\tOriginal_Intensity\tFit\tFlag");

			foreach (LCMSFeature feature in featureList)
			{
				foreach (MSFeature msFeature in feature.MSFeatureList)
				{
					msFeatureWriter.WriteLine(feature.ID + "\t" + msFeature.IndexInFile + "\t" + msFeature.ID + "\t" + ScanLCMapHolder.ScanLCMap[msFeature.ScanLC] + "\t" +
												msFeature.MassMonoisotopic + "\t" + msFeature.ChargeState + "\t" + msFeature.Abundance + "\t" +
												msFeature.IntensityOriginal + "\t" + msFeature.Fit + "\t" + msFeature.Suspicious);
				}
			}

			msFeatureWriter.Close();
		}

		public static void WriteMSFeaturesToFile(List<IMSMSFeature> featureList, Settings settings)
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			String outputDirectory = "";

			if (!settings.OutputDirectory.Equals(String.Empty))
			{
				outputDirectory = settings.OutputDirectory + "\\";
			}

			TextWriter imsmsFeatureWriter = new StreamWriter(outputDirectory + baseFileName + "_IMSMSFeatures.txt");
			TextWriter msFeatureWriter = new StreamWriter(outputDirectory + baseFileName + "_MSFeatures.txt");

			imsmsFeatureWriter.WriteLine("IMS-MS_Feature_Index\tLC_Scan\tMember_Count\tMedian_Mono_Mass\tCharge\tDrift_Time\tIMS_Range\tConformation_Fit");
			msFeatureWriter.WriteLine("IMS_Index\tMS_Index\tFiltered_MS_Index\tLC_Scan\tIMS_Scan\tMono_mass\tCharge\tDrift_Time\tAbundance\tOriginal_Intensity\tFit\tFlag\tCorrected\tCorrected_Magnitude");

			foreach (IMSMSFeature imsmsFeature in featureList)
			{
				imsmsFeatureWriter.WriteLine(imsmsFeature.ID + "\t" + ScanLCMapHolder.ScanLCMap[imsmsFeature.ScanLC] + "\t" + imsmsFeature.MSFeatureList.Count + "\t" + imsmsFeature.MassMonoisotopicMedian + "\t" + imsmsFeature.ChargeState + "\t" + imsmsFeature.DriftTime + "\t" + (imsmsFeature.ScanIMSEnd - imsmsFeature.ScanIMSStart + 1) + "\t" + imsmsFeature.ConformationFitScore);

				foreach (MSFeature msFeature in imsmsFeature.MSFeatureList)
				{
					msFeatureWriter.WriteLine(imsmsFeature.ID + "\t" + msFeature.IndexInFile + "\t" + msFeature.ID + "\t" + ScanLCMapHolder.ScanLCMap[msFeature.ScanLC] + "\t" + msFeature.ScanIMS + "\t" +
												msFeature.MassMonoisotopic + "\t" + msFeature.ChargeState + "\t" + msFeature.DriftTime + "\t" + msFeature.Abundance + "\t" +
												msFeature.IntensityOriginal + "\t" + msFeature.Fit + "\t" + msFeature.Suspicious + "\t" + msFeature.Corrected + "\t" + msFeature.CorrectedValue);
				}
			}

			imsmsFeatureWriter.Close();
			msFeatureWriter.Close();
		}

		public static void WriteMSFeaturesToFile(List<LCIMSMSFeature> featureList, Settings settings)
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			String outputDirectory = "";

			if (!settings.OutputDirectory.Equals(String.Empty))
			{
				outputDirectory = settings.OutputDirectory + "\\";
			}

			TextWriter imsmsFeatureWriter = new StreamWriter(outputDirectory + baseFileName + "_IMSMSFeatures.txt");
			TextWriter msFeatureWriter = new StreamWriter(outputDirectory + baseFileName + "_MSFeatures.txt");

			imsmsFeatureWriter.WriteLine("LC-IMS_Index\tIMS-MS_Feature_Index\tLC_Scan\tMember_Count\tMedian_Mono_Mass\tCharge\tDrift_Time\tConformation_Index\tConformation_Fit_Score");
			msFeatureWriter.WriteLine("LC-IMS_Index\tIMS_Index\tMS_Index\tFiltered_MS_Index\tLC_Scan\tIMS_Scan\tMono_mass\tCharge\tDrift_Time\tAbundance\tOriginal_Intensity\tFit\tFlag\tCorrected\tCorrected_Magnitude");

			foreach (LCIMSMSFeature lcimsmsFeature in featureList)
			{
				foreach (IMSMSFeature imsmsFeature in lcimsmsFeature.IMSMSFeatureList)
				{
					imsmsFeatureWriter.WriteLine(lcimsmsFeature.ID + "\t" + imsmsFeature.ID + "\t" + ScanLCMapHolder.ScanLCMap[imsmsFeature.ScanLC] + "\t" + imsmsFeature.MSFeatureList.Count + "\t" + imsmsFeature.MassMonoisotopicMedian + "\t" +
													imsmsFeature.ChargeState + "\t" + imsmsFeature.DriftTime + "\t" + imsmsFeature.ConformationIndex + "\t" + imsmsFeature.ConformationFitScore);

					foreach (MSFeature msFeature in imsmsFeature.MSFeatureList)
					{
						msFeatureWriter.WriteLine(lcimsmsFeature.ID + "\t" + imsmsFeature.ID + "\t" + msFeature.IndexInFile + "\t" + msFeature.ID + "\t" + ScanLCMapHolder.ScanLCMap[msFeature.ScanLC] + "\t" + msFeature.ScanIMS + "\t" +
													msFeature.MassMonoisotopic + "\t" + msFeature.ChargeState + "\t" + msFeature.DriftTime + "\t" + msFeature.Abundance + "\t" +
													msFeature.IntensityOriginal + "\t" + msFeature.Fit + "\t" + msFeature.Suspicious + "\t" + msFeature.Corrected + "\t" + msFeature.CorrectedValue);
					}
				}
			}

			imsmsFeatureWriter.Close();
			msFeatureWriter.Close();
		}

		public static void CheckMassAbundanceRelationship(List<IMSMSFeature> featureList, Settings settings)
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			String outputDirectory = "";

			if (!settings.OutputDirectory.Equals(String.Empty))
			{
				outputDirectory = settings.OutputDirectory + "\\";
			}

			TextWriter imsmsFeatureWriter = new StreamWriter(outputDirectory + baseFileName + "_Abundances1.txt");
			TextWriter massWriter = new StreamWriter(outputDirectory + baseFileName + "_MassDifferences.txt");
			//TextWriter msFeatureWriter = new StreamWriter(outputDirectory + baseFileName + "_Abundances2.txt");

			imsmsFeatureWriter.WriteLine("IMS-MS_Feature_Index\tLC_Scan\tMember_Count\tMedian_Mono_Mass\tMax_Abundance\tCharge\tMass_Range");
			//msFeatureWriter.WriteLine("LC-IMS_Index\tIMS_Index\tMS_Index\tFiltered_MS_Index\tLC_Scan\tIMS_Scan\tMono_mass\tCharge\tDrift_Time\tAbundance\tOriginal_Intensity\tFit\tFlag\tCorrected\tCorrected_Magnitude");

			//if (lcimsmsFeature.ScanLCAdjustedStart >= 100 && lcimsmsFeature.ScanLCAdjustedStart < 600)
			//{
			foreach (IMSMSFeature imsmsFeature in featureList)
			{

				if (imsmsFeature.MSFeatureList.Count > 5)
				{

					imsmsFeature.MSFeatureList.Sort(new Comparison<MSFeature>(Feature.AbundanceComparison));
					double medianMass = imsmsFeature.MassMonoisotopicMedian;

					foreach (MSFeature msFeature in imsmsFeature.MSFeatureList)
					{
						massWriter.Write(Math.Abs(medianMass - msFeature.MassMonoisotopic) + "\t");

						/*
						msFeatureWriter.WriteLine(lcimsmsFeature.Id + "\t" + imsmsFeature.Id + "\t" + msFeature.IndexInFile + "\t" + msFeature.Id + "\t" + ScanLCMapHolder.ScanLCMap[msFeature.ScanLC] + "\t" + msFeature.ScanIMS + "\t" +
													msFeature.MassMonoisotopic + "\t" + msFeature.Charge + "\t" + msFeature.DriftTimeIMS + "\t" + msFeature.Abundance + "\t" +
													msFeature.IntensityOriginal + "\t" + msFeature.Fit + "\t" + msFeature.ErrorFlag + "\t" + msFeature.Corrected + "\t" + msFeature.CorrectedValue);
						 */
					}

					massWriter.Write("\n");

					imsmsFeatureWriter.WriteLine(imsmsFeature.ID + "\t" + ScanLCMapHolder.ScanLCMap[imsmsFeature.ScanLC] + "\t" + imsmsFeature.MSFeatureList.Count + "\t" + imsmsFeature.MassMonoisotopicMedian + "\t" + imsmsFeature.AbundanceMaximum + "\t" + imsmsFeature.ChargeState + "\t" + (imsmsFeature.MassMonoisotopicMaximum - imsmsFeature.MassMonoisotopicMinimum));

				}
			}
			//}

			imsmsFeatureWriter.Close();
			massWriter.Close();
			//msFeatureWriter.Close();
		}

		public static void WriteMSFeaturesToFile<T>(List<T> featureList, Settings settings) where T : UMC
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			String outputDirectory = "";

			if (!settings.OutputDirectory.Equals(String.Empty))
			{
				outputDirectory = settings.OutputDirectory + "\\";
			}

			TextWriter msFeatureWriter = new StreamWriter(outputDirectory + baseFileName + "_MSFeatures.txt");

			msFeatureWriter.WriteLine("Feature_Index\tMS_Feature_Index\tFiltered_MS_Feature_Index\tLC_Scan\tMono_mass\tCharge\tAbundance\tOriginal_Intensity\tFit\tFlag");

			foreach (T feature in featureList)
			{
				foreach (MSFeature msFeature in feature.MSFeatureList)
				{
					msFeatureWriter.WriteLine(feature.ID + "\t" + msFeature.IndexInFile + "\t" + msFeature.ID + "\t" + ScanLCMapHolder.ScanLCMap[msFeature.ScanLC] + "\t" +
												msFeature.MassMonoisotopic + "\t" + msFeature.ChargeState + "\t" + msFeature.Abundance + "\t" +
												msFeature.IntensityOriginal + "\t" + msFeature.Fit + "\t" + msFeature.Suspicious);
				}
			}

			msFeatureWriter.Close();
		}

		public static List<T> FilterFeatureListByAbundanceMaximum<T>(List<T> featureList, uint abundanceMax) where T : UMC
		{
			List<T> filteredFeatureList = new List<T>();

			foreach (T feature in featureList)
			{
				if (feature.AbundanceMaximum >= abundanceMax)
				{
					filteredFeatureList.Add(feature);
				}
			}

			return filteredFeatureList;
		}
	}
}
