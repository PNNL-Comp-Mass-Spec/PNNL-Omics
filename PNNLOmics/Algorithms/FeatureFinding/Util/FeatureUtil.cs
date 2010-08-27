using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.SQLite;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureFinding.Data;
using PNNLOmics.Algorithms.FeatureFinding.Control;
using PNNLOmics.Generic;

namespace PNNLOmics.Algorithms.FeatureFinding.Util
{
    // TODO: Kevin XML comment here
	public static class FeatureUtil
	{
        // TODO: Kevin Refactor namespace and class names to be Utility instead of Util
        // TODO: Kevin Outputers should possibly go to an exporter class?
		/// <summary>
		/// Writes a list of UMCs to a tab-delimited file for use in VIPER or MultiAlign.
		/// </summary>
		/// <typeparam name="T">UMC</typeparam>
		/// <param name="featureList">List of UMCs</param>
		/// <param name="settings">Settings object</param>
		public static void WriteUMCListToFile<T>(List<T> umcList, Settings settings) where T : UMC
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

			foreach (T umc in umcList)
			{
				featureWriter.WriteLine(umc.ID + "\t" + umc.MassMonoisotopicMedian.ToString("0.00000") + "\t" + umc.MassMonoisotopicMedian.ToString("0.00000") + "\t"
							+ umc.MassMonoisotopicMinimum + "\t" + umc.MassMonoisotopicMaximum + "\t" + ScanLCMapHolder.ScanLCMap[umc.ScanLCStart] + "\t" + ScanLCMapHolder.ScanLCMap[umc.ScanLCEnd] + "\t"
							+ ScanLCMapHolder.ScanLCMap[umc.ScanLCOfMaxAbundance] + "\t" + umc.MSFeatureList.Count + "\t" + umc.AbundanceMaximum + "\t" + umc.AbundanceSum + "\t"
							+ umc.MZ + "\t" + umc.ChargeState + "\t" + umc.ChargeMaximum + "\t" + umc.DriftTime + "\t" + umc.ConformationFitScore);

				Dictionary<int, int> cmcMap = new Dictionary<int, int>();

				foreach (MSFeature msFeature in umc.MSFeatureList)
				{
					mapWriter.WriteLine(umc.ID + "\t" + msFeature.IndexInFile + "\t" + msFeature.ID);

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
					cmcWriter.WriteLine(umc.ID + "\t" + key + "\t" + cmcMap[key]);
				}
			}

			featureWriter.Close();
			mapWriter.Close();
			cmcWriter.Close();
		}

		/// <summary>
		/// Outputs a list of UMCs to a SQLite database.
		/// INCOMPLETE METHOD. DOES NOT WORK YET.
		/// </summary>
		/// <typeparam name="T">UMC</typeparam>
		/// <param name="featureList">List of UMCs</param>
		/// <param name="settings">Settings object</param>
		public static void WriteUMCListToSQLite<T>(List<T> umcList, Settings settings) where T : UMC
		{
			// TODO: Finish this method

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

		/// <summary>
		/// Outputs a list of UMCs to a tab-delimited in such a way that the abundance profile of each UMC is made clear.
		/// </summary>
		/// <typeparam name="T">UMC</typeparam>
		/// <param name="featureList">List of UMCs</param>
		/// <param name="settings">Settings object</param>
		public static void CreateAbundanceProfile<T>(List<T> umcList, Settings settings) where T : UMC
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			String outputDirectory = "";

			if (!settings.OutputDirectory.Equals(String.Empty))
			{
				outputDirectory = settings.OutputDirectory + "\\";
			}

			TextWriter profileWriter = new StreamWriter(outputDirectory + baseFileName + "_AbundanceProfiles.txt");

			profileWriter.WriteLine("Feature_index\tMass\tCharge\tScan\tAbundance");

			foreach (T umc in umcList)
			{
				List<MSFeature> msFeatureList = umc.MSFeatureList;
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

					profileWriter.WriteLine(umc.ID + "\t" + umc.MassMonoisotopicMedian + "\t" + currentCharge + "\t" + currentScanLC + "\t" + totalbundance);
				}
			}
		}

		/// <summary>
		/// Corrects the monoisotopic mass of a UMC (based on Da correction algorithm results) by assuming the most abundant
		/// MS Feature of the UMC contains the correct mass. All other masses will be corrected in increments of 1 Da in respect 
		/// to the mass of the most abundant MS Feature.
		/// </summary>
		/// <typeparam name="T">UMC</typeparam>
		/// <param name="feature">The UMC to be corrected</param>
		public static void CorrectMassMostAbundant<T>(T umc) where T : UMC
		{
			if (umc.HasDaltonError)
			{
				double massReference = umc.MassOfMaxAbundance;

				foreach (MSFeature msFeature in umc.MSFeatureList)
				{
					int differenceInt = (int)Math.Round(massReference - msFeature.MassMonoisotopic);

					if (differenceInt != 0)
					{
						msFeature.MassMonoisotopic += differenceInt;
						//msFeature.Mz = (msFeature.MassMonoisotopic / msFeature.Charge) + 1.00727849;
                        // TODO: Kevin 1.00727849 should be a constant
                        // TODO: Kevin Remove MZCorrected completely?
						msFeature.MZCorrected = (msFeature.MassMonoisotopic / msFeature.ChargeState) + 1.00727849;
						msFeature.IsDaltonCorrected = true;
                        msFeature.MassOffset += differenceInt;
					}
				}
			}
		}

		/// <summary>
		/// Writes a list of MS Features that are contained in a List of LC-MS Features to a tab-delimited file.
		/// </summary>
		/// <param name="featureList">LC-MS Feature list</param>
		/// <param name="settings">Settings object</param>
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
												msFeature.IntensityOriginal + "\t" + msFeature.Fit + "\t" + msFeature.IsSuspicious);
				}
			}

			msFeatureWriter.Close();
		}

		/// <summary>
		/// Writes a list of MS Features that are contained in a List of IMS-MS Features to a tab-delimited file.
		/// </summary>
		/// <param name="featureList">IMS-MS Feature list</param>
		/// <param name="settings">Settings object</param>
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
												msFeature.IntensityOriginal + "\t" + msFeature.Fit + "\t" + msFeature.IsSuspicious + "\t" + msFeature.IsDaltonCorrected + "\t" + msFeature.MassOffset);
				}
			}

			imsmsFeatureWriter.Close();
			msFeatureWriter.Close();
		}

		/// <summary>
		/// Writes a list of MS Features that are contained in a List of LC-IMS-MS Features to a tab-delimited file.
		/// </summary>
		/// <param name="featureList">LC-IMS-MS Feature list</param>
		/// <param name="settings">Settings object</param>
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
													msFeature.IntensityOriginal + "\t" + msFeature.Fit + "\t" + msFeature.IsSuspicious + "\t" + msFeature.IsDaltonCorrected + "\t" + msFeature.MassOffset);
					}
				}
			}

			imsmsFeatureWriter.Close();
			msFeatureWriter.Close();
		}

		/// <summary>
		/// Outputs the mass-abundance relationship of each MS Feature to a tab-delimited file.
		/// </summary>
		/// <param name="featureList">List of IMS-MS Features</param>
		/// <param name="settings">Settings object</param>
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

		/// <summary>
		/// Writes a List of MS Features that are contained in a List of UMCs to a tab-delimited file.
		/// </summary>
		/// <typeparam name="T">UMC</typeparam>
		/// <param name="featureList">List of UMCs</param>
		/// <param name="settings">Settings object</param>
		public static void WriteMSFeaturesToFile<T>(List<T> umcList, Settings settings) where T : UMC
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];
			String outputDirectory = "";

			if (!settings.OutputDirectory.Equals(String.Empty))
			{
				outputDirectory = settings.OutputDirectory + "\\";
			}

			TextWriter msFeatureWriter = new StreamWriter(outputDirectory + baseFileName + "_MSFeatures.txt");

			msFeatureWriter.WriteLine("Feature_Index\tMS_Feature_Index\tFiltered_MS_Feature_Index\tLC_Scan\tMono_mass\tCharge\tAbundance\tOriginal_Intensity\tFit\tFlag");

			foreach (T umc in umcList)
			{
				foreach (MSFeature msFeature in umc.MSFeatureList)
				{
					msFeatureWriter.WriteLine(umc.ID + "\t" + msFeature.IndexInFile + "\t" + msFeature.ID + "\t" + ScanLCMapHolder.ScanLCMap[msFeature.ScanLC] + "\t" +
												msFeature.MassMonoisotopic + "\t" + msFeature.ChargeState + "\t" + msFeature.Abundance + "\t" +
												msFeature.IntensityOriginal + "\t" + msFeature.Fit + "\t" + msFeature.IsSuspicious);
				}
			}

			msFeatureWriter.Close();
		}

		/// <summary>
		/// Filters a List of UMCs based on Maximum Abundance
		/// </summary>
		/// <typeparam name="T">UMC</typeparam>
		/// <param name="featureList">List of UMCs to filter</param>
		/// <param name="abundanceMax">The abundance value used for filtering</param>
		/// <returns>A filtered List of UMCs</returns>
		public static List<T> FilterFeatureListByAbundanceMaximum<T>(List<T> umcList, uint abundanceMax) where T : UMC
		{
			List<T> filteredFeatureList = new List<T>();

			foreach (T umc in umcList)
			{
				if (umc.AbundanceMaximum >= abundanceMax)
				{
					filteredFeatureList.Add(umc);
				}
			}

			return filteredFeatureList;
		}

		/// <summary>
		/// Does a Binary Search on a List based on a given object and inserts the object into the appropriate location.
		/// This allows for the List to stay sorted as you insert objects.
		/// </summary>
		/// <typeparam name="T">UMC</typeparam>
		/// <param name="umcList">UMC List</param>
		/// <param name="umc">UMC object to be inserted</param>
		/// <param name="comparer">AnyonymousComparer to be used for the binary search</param>
		public static void SearchAndInsert<T>(List<T> umcList, T umc, AnonymousComparer<T> comparer) where T : UMC
		{
			int index = umcList.BinarySearch(umc, comparer);

			if (index < 0)
			{
				umcList.Insert(~index, umc);
			}
			else
			{
				umcList.Insert(index, umc);
			}
		}
	}
}
