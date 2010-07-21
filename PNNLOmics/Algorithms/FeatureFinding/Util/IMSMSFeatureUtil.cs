using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
//using System.Threading.Tasks;
//using System.Collections.Concurrent;
using System.Linq;
using UIMFLibrary;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureFinding.Control;
using PNNLOmics.Generic;
using PNNLOmics.Algorithms.ConformationDetection.Util;

namespace PNNLOmics.Algorithms.FeatureFinding.Util
{
	/// <summary>
	/// Utility class for Features that are involved with Ion Mobility Spectrometry, including IMS-MS and LC-IMS-MS Features.
	/// </summary>
	public class IMSMSFeatureUtil
	{
		private Logger m_logger;
		private Settings m_settings;

		/// <summary>
		/// Contructor that takes a Settings object and a Logger object
		/// </summary>
		/// <param name="settings">Settings object</param>
		/// <param name="logger">Logger object</param>
		public IMSMSFeatureUtil(Settings settings, Logger logger)
		{
			m_logger = logger;
			m_settings = settings;
		}

		/// <summary>
		/// Clusters a List of MS Features together across the IMS dimension to create a List of IMS-MS Features.
		/// </summary>
		/// <param name="msFeatureList">List of MS Features to be clustered</param>
		/// <returns>List of IMS-MS Features</returns>
		public List<IMSMSFeature> ProcessMSFeatures(List<MSFeature> msFeatureList)
		{
			/*
			 * These lists will be used to hold different sets of IMS-MS Features based on the current state of the Feature:
			 * 
			 *		openIMSMSFeatureList		- List of current IMS-MS Features that are still open
			 *										* An open IMS-MS Feature is a Feature that exists in the current LC Scan that MS Features
			 *										* can still be added to.
			 *		newIMSMSFeatureList			- List of new IMS-MS Features that are not yet included in the openIMSIMSFeatureList
			 *										* The List of new IMS-MS Features will be dumped into the openIMSMSFeatureList after processing
			 *										* for an entire IMS Scan is finished.
			 *		imsmsFeatureListForLCScan	- List of all IMS-MS Features that exist in the current LC Scan
			 *										* The IMS-MS Features in imsmsFeatureListForLCScan are dumped into completeIMSMSFeatureList 
			 *										* after processing for an entire LC Scan is finished.
			 *		completeIMSMSFeatureList	- List of all IMS-MS Features that have been created
			 *		
			 */
			List<IMSMSFeature> openIMSMSFeatureList = new List<IMSMSFeature>();
			List<IMSMSFeature> newIMSMSFeatureList = new List<IMSMSFeature>();
			List<IMSMSFeature> imsmsFeatureListForLCScan = new List<IMSMSFeature>();
			List<IMSMSFeature> completeIMSMSFeatureList = new List<IMSMSFeature>();

			MSFeature currentMSFeature = null;
            int currentScanLC = 0;
			int currentScanIMS = 0;
			int imsDaltonCorrectionMax = m_settings.IMSDaltonCorrectionMax;
			double massToleranceBase = m_settings.MassMonoisotopicConstraint;

			/*
			 * Steps for clustering MS Features into IMS-MS Features:
			 * 
			 *	- Grab the first MS Feature and store both the LC Scan and IMS Scan numbers of the MS Feature (currentScanIMS and currentScanLC)
			 *	- Iterate over all MS Features
			 *		- For all MS Features (including the first MS Feature)
			 *			- If this MS Feature has the same IMS Scan number as the stored IMS Scan number (currentScanIMS)
			 *				- Search the Open IMS-MS Features for a valid match (based on mass and charge)
			 *				- If 1 or more valid matches are found
			 *					- Add the MS Feature to the "closest" (based on mass) IMS-MS Feature
			 *				- If no valid matches are found
			 *					- Create a new IMS-MS Feature
			 *					- Add the new IMS-MS Feature to the New IMS-MS Features List and the IMS-MS Feature List For LC Scan (imsmsFeatureListForLCScan)
			 *			- If this MS Feature has a different IMS Scan number than the stored IMS Scan number (currentScanIMS)
			 *				- If this MS Feature has the same LC Scan number
			 *					- Refine the Open IMS-MS Feature List to only contain IMS-MS Features that will not have too big of an IMS gap 
			 *					  now that we have reached the next IMS Scan number
			 *					- Update currentScanIMS to reflect the new IMS Scan Number 
			 *				- If this MS Feature has a different LC Scan number than the stored LC Scan number (currentScanLC)
			 *					- Clear out the Open IMS-MS Feature List
			 *					- Run Dalton correction on all of the IMS-MS Features in imsmsFeatureListForLCScan
			 *					- Add these Dalton-corrected IMS-MS Feature to the Complete List of IMS-MS Features
			 *					- Clear out the imsmsFeatureListForLCScan
			 *					- Update the currentScanIMS to reflect the new IMS Scan Number
			 *					- Update the currentScanLC to reflect the new LC Scan Number
			 *					- Start iterating over the MS Features agin but re-process the current MS Feature
			 *						- It is important to remember that the current MS Feature was not placed into an IMS-MS feature because we
			 *						  we reached a new LC Scan
			 */

			// Sort MS Features by LC Scan, then IMS Scan, then Mass
			msFeatureList.Sort(new Comparison<MSFeature>(Feature.ScanLCAndDriftTimeAndMassComparison));

			// Iterate over all MS Features
			int i = 0;
			while (i < msFeatureList.Count)
			{
				// Grab the current MSFeature and keep track of its LC Scan and IMS Scan
				currentMSFeature = msFeatureList[i];
				currentScanLC = currentMSFeature.ScanLC;
				currentScanIMS = currentMSFeature.ScanIMS;

				// Keeps track of how many MS Features we process for the current IMS Scan
				int msFeatureCount = 0;

                // Iterate over all MS Features until we reach a new IMS Scan
				for (int j = i; j < msFeatureList.Count; j++)
				{
					// Get the current MS Feature
					currentMSFeature = msFeatureList[j];

					// Keep going as long as we are still in the same IMS Scan
					if (currentMSFeature.ScanIMS == currentScanIMS)
					{
						// Calculate the High and Low Mass Tolerance for the Current MS Feature
						double massTolerance = massToleranceBase * currentMSFeature.MassMonoisotopic / 1000000;
						double massToleranceHigh = currentMSFeature.MassMonoisotopic + massTolerance;

						var query = from IMSMSFeature imsmsFeature in openIMSMSFeatureList
									where imsmsFeature.ChargeState == currentMSFeature.ChargeState
											&& imsmsFeature.MassOfScanIMSMax >= currentMSFeature.MassMonoisotopic - massTolerance
											&& imsmsFeature.MassOfScanIMSMax <= currentMSFeature.MassMonoisotopic + massTolerance
											&& !imsmsFeature.ScanIMSList.Contains(currentMSFeature.ScanIMS)
									orderby Math.Abs(imsmsFeature.MassOfScanIMSMax - currentMSFeature.MassMonoisotopic)
									select imsmsFeature;

						if (query.Count() > 0)
						{
							// Use the "closest" IMS-MS Feature that was returned by the query
							IMSMSFeature imsmsFeature = query.First();
							imsmsFeature.AddMSFeature(currentMSFeature);
						}
						else
						{
							// If we did not find an IMS-MS Feature, Create a new IMS-MS Feature
							IMSMSFeature newIMSMSFeature = new IMSMSFeature();
							newIMSMSFeature.AddMSFeature(currentMSFeature);
							newIMSMSFeature.ScanLC = currentMSFeature.ScanLC;
							newIMSMSFeature.ChargeState = currentMSFeature.ChargeState;

							newIMSMSFeatureList.Add(newIMSMSFeature);
							imsmsFeatureListForLCScan.Add(newIMSMSFeature);
						}
					}

					// When we get to the next IMS Scan, stop
					else
					{
						break;
					}

					// Increment the counter after everything so that the counter does not increment when the IMS Scan changes.
					// When the IMS Scan changes, we still need to process the Current MSFeature
					msFeatureCount++;
				}

                // TODO: Kevin - STOPPED CODE REVIEW HERE

				// If we get to a new LC Scan, then Close all IMS-MS Features
				if (currentMSFeature.ScanLC != currentScanLC)
				{
					openIMSMSFeatureList.Clear();
					newIMSMSFeatureList.Clear();

					// Dalton Correction
					if (m_settings.IMSDaltonCorrectionMax > 0)
					{
						imsmsFeatureListForLCScan = AppendIMSMSFeatures(imsmsFeatureListForLCScan);
						imsmsFeatureListForLCScan = FillGaps(imsmsFeatureListForLCScan);
					}

					// Add this LC Scan's IMS-MS Feature List to the Complete IMS-MS feature List and then clear it 
					completeIMSMSFeatureList.AddRange(imsmsFeatureListForLCScan);
					imsmsFeatureListForLCScan.Clear();
				}

				// If we stay in the same LC Scan
				else
				{
					// Allow for IMS Gap
					var query = from IMSMSFeature imsmsFeature in openIMSMSFeatureList
								where (int)currentScanIMS - (int)imsmsFeature.ScanIMSEnd - 1 <= m_settings.IMSGapSizeMax
								select imsmsFeature;

					openIMSMSFeatureList = query.ToList<IMSMSFeature>();
					openIMSMSFeatureList.AddRange(newIMSMSFeatureList);
					newIMSMSFeatureList.Clear();
				}

				// Make it so that "i" will point to the first MSFeature that is on a new IMS Scan
				i += msFeatureCount;
			}

			// Since the LC Scan technically will not change when we finish, we need to process the Gap Filler for the last LC Scan
			if (m_settings.IMSDaltonCorrectionMax > 0)
			{
				imsmsFeatureListForLCScan = AppendIMSMSFeatures(imsmsFeatureListForLCScan);
				imsmsFeatureListForLCScan = FillGaps(imsmsFeatureListForLCScan);
			}

			completeIMSMSFeatureList.AddRange(imsmsFeatureListForLCScan);
			imsmsFeatureListForLCScan.Clear();

			return completeIMSMSFeatureList;
		}

		/*
		public List<IMSMSFeature> GetIMSMSFeaturesProfileStatistics(List<IMSMSFeature> imsmsFeatureList)
		{
			IMSMSFeature currentIMSMSFeature;	

			for (int i = 0; i < imsmsFeatureList.Count; i++)
			{
				// Get the mono statistics of current IMS-MSFeature by weighted average of all ms features.
				currentIMSMSFeature = imsmsFeatureList[i];
				currentIMSMSFeature.Charge = currentIMSMSFeature.MSFeatureList[0].Charge;

				double mzAvgCurrentIMSMSFeature = 0.0;
				double fitAvgCurrentIMSMSFeature = 0.0;
				double fwhmAvgCurrentIMSMSFeature = 0.0;
				double intensityAvgCurrentIMSMSFeature = 0.0;
				double abundanceAvgCurrentIMSMSFeature = 0.0;
				double sumIntensityCurrentIMSMSFeature = 0.0;
				double intensityCurrentMSFeature = 0.0;
				List<double> monoMZTable = new List<double>();
				bool inMonoMZHashtable;

				for (int j = 0; j < currentIMSMSFeature.MSFeatureList.Count; j++)
				{
					// Check if current mz in monoMZHashtable
					inMonoMZHashtable = false;
					for (int k = 0; k < monoMZTable.Count; k++)
					{
						if (Math.Abs(monoMZTable[k] - currentIMSMSFeature.MSFeatureList[j].Mz) < 0.5 / currentIMSMSFeature.Charge)
						{
							inMonoMZHashtable = true;
							break;
						}
					}

					if (!inMonoMZHashtable)
					{
						monoMZTable.Add(currentIMSMSFeature.MSFeatureList[j].Mz);
					}

					// Compute the average intensity of feature
					intensityAvgCurrentIMSMSFeature += currentIMSMSFeature.MSFeatureList[j].IntensityOriginal;
					abundanceAvgCurrentIMSMSFeature += currentIMSMSFeature.MSFeatureList[j].Abundance;
				}
				
				// Depends how to define the mz of IMSMS feature
				// To avoid 1 Da problem
				if (monoMZTable.Count > 1)
				{
					monoMZTable.Sort();
				}
				currentIMSMSFeature.MZAvgAllMSFeatures = monoMZTable[0];
				currentIMSMSFeature.Fwhm = currentIMSMSFeature.MSFeatureList[0].Fwhm;
				currentIMSMSFeature.FitScoreDecon = currentIMSMSFeature.MSFeatureList[0].Fit;
				currentIMSMSFeature.IntensityAvgAllMSFeatures = intensityAvgCurrentIMSMSFeature / currentIMSMSFeature.MSFeatureList.Count;
				currentIMSMSFeature.AbundanceAvgAllMSFeatures = abundanceAvgCurrentIMSMSFeature / currentIMSMSFeature.MSFeatureList.Count;

				imsmsFeatureList[i].MZAvgAllMSFeatures = currentIMSMSFeature.MZAvgAllMSFeatures;
				imsmsFeatureList[i].IntensityAvgAllMSFeatures = currentIMSMSFeature.IntensityAvgAllMSFeatures;
				imsmsFeatureList[i].AbundanceAvgAllMSFeatures = currentIMSMSFeature.AbundanceAvgAllMSFeatures;
				imsmsFeatureList[i].Charge = currentIMSMSFeature.Charge;
				imsmsFeatureList[i].Fwhm = currentIMSMSFeature.Fwhm;

			}

			if (false)
			{
				for (int i = 0; i < imsmsFeatureList.Count; i++)
				{
					// Get the mono statistics of current IMS-MSFeature by weighted average of all ms features.
					currentIMSMSFeature = imsmsFeatureList[i];
					double mzAvgCurrentIMSMSFeature = 0.0;
					double fwhmAvgCurrentIMSMSFeature = 0.0;
					for (int j = 0; j < currentIMSMSFeature.MSFeatureList.Count; j++)
					{
						mzAvgCurrentIMSMSFeature += currentIMSMSFeature.MSFeatureList[j].Mz;
						fwhmAvgCurrentIMSMSFeature += currentIMSMSFeature.MSFeatureList[j].Fwhm;
					}

					currentIMSMSFeature.MZAvgAllMSFeatures = mzAvgCurrentIMSMSFeature / currentIMSMSFeature.MSFeatureList.Count;
					currentIMSMSFeature.Fwhm = fwhmAvgCurrentIMSMSFeature / currentIMSMSFeature.MSFeatureList.Count;
					currentIMSMSFeature.Charge = currentIMSMSFeature.MSFeatureList[0].Charge;

				}
			}
			if (false)
			{
				for (int i = 0; i < imsmsFeatureList.Count; i++)
				{
					// Get the mono statistics of current IMS-MSFeature by weighted average of all ms features.
					currentIMSMSFeature = imsmsFeatureList[i];
					double mzAvgCurrentIMSMSFeature = 0.0;
					double fwhmAvgCurrentIMSMSFeature = 0.0;
					double sumIntensityCurrentIMSMSFeature = 0.0;
					double intensityCurrentMSFeature = 0.0;
					for (int j = 0; j < currentIMSMSFeature.MSFeatureList.Count; j++)
					{
						intensityCurrentMSFeature = currentIMSMSFeature.MSFeatureList[j].IntensityOriginal;
						mzAvgCurrentIMSMSFeature += currentIMSMSFeature.MSFeatureList[j].Mz * intensityCurrentMSFeature;
						fwhmAvgCurrentIMSMSFeature += currentIMSMSFeature.MSFeatureList[j].Fwhm * intensityCurrentMSFeature;
						sumIntensityCurrentIMSMSFeature += intensityCurrentMSFeature;
					}

					currentIMSMSFeature.MZAvgAllMSFeatures = mzAvgCurrentIMSMSFeature / sumIntensityCurrentIMSMSFeature;
					currentIMSMSFeature.Fwhm = fwhmAvgCurrentIMSMSFeature / sumIntensityCurrentIMSMSFeature;
					currentIMSMSFeature.Charge = currentIMSMSFeature.MSFeatureList[0].Charge;

				}
			}
			return imsmsFeatureList;
		}
		*/

		/// <summary>
		/// Clusters a List of IMS-MS Features together across the LC dimension to create LC-IMS-MS Features.
		/// </summary>
		/// <param name="imsmsFeatureList">List of IMS-MS Features to be clustered</param>
		/// <returns>A List of LC-IMS-MS Features</returns>
		public List<LCIMSMSFeature> ProcessIMSMSFeatures(List<IMSMSFeature> imsmsFeatureList)
		{
			IMSMSFeature currentIMSMSFeature = new IMSMSFeature();

			List<LCIMSMSFeature> openLCIMSMSFeatureList = new List<LCIMSMSFeature>();
			List<LCIMSMSFeature> newLCIMSMSFeatureList = new List<LCIMSMSFeature>();
			List<LCIMSMSFeature> completeLCIMSMSFeatureList = new List<LCIMSMSFeature>();

			AnonymousComparer<LCIMSMSFeature> comparer = new AnonymousComparer<LCIMSMSFeature>(new Comparison<LCIMSMSFeature>(UMC.MassofMaxAbundanceComparison));

			int currentScanLC = 0;
			double massToleranceBase = m_settings.MassMonoisotopicConstraint;

			imsmsFeatureList.Sort(new Comparison<IMSMSFeature>(UMC.ScanLCAndMassOfMaxAbundanceComparison));

			int i = 0;
			while (i < imsmsFeatureList.Count)
			{
				// Grab the current IMS-MS Feature and keep track of its LC Scan
				currentIMSMSFeature = imsmsFeatureList[i];
				currentScanLC = currentIMSMSFeature.ScanLC;

				// Keeps track of how many IMS-MS Features we process for the current LC Scan
				int imsmsFeatureCount = 0;

				for (int j = i; j < imsmsFeatureList.Count; j++)
				{
					// If this is not the first iteration, get the current IMS-MS Feature
					if (j != i)
					{
						currentIMSMSFeature = imsmsFeatureList[j];
					}

					// Keep going as long as we are still in the same LC Scan
					if (currentIMSMSFeature.ScanLC == currentScanLC)
					{
						// Calculate the High and Low Mass Tolerance for the Current IMS-MS Feature
						double massTolerance = massToleranceBase * currentIMSMSFeature.MassOfMaxAbundance / 1000000;
						double massToleranceLow = currentIMSMSFeature.MassOfMaxAbundance - massTolerance;
						double massToleranceHigh = currentIMSMSFeature.MassOfMaxAbundance + massTolerance;

						// Keeps track of if the Current IMS-MS Feature finds an LC-IMS-MS Feature to live in
						Boolean found = false;

						// Iterate over all Open LC-IMS-MS Features
						foreach (LCIMSMSFeature lcimsmsFeature in openLCIMSMSFeatureList)
						{
							// Stop if we find an LC-IMS-MS feature with too high of a Median Mass
							if (lcimsmsFeature.MassOfMaxAbundance > massToleranceHigh)
							{
								break;
							}

							// Only consider this LC-IMS-MS Feature if it has not already been combined with an IMS-MS Feature from this LC Scan
							else if (lcimsmsFeature.ScanLCEnd != currentScanLC)
							{
								// If the user has set to not use Charge or the charge states are equal
								if (!m_settings.UseCharge || lcimsmsFeature.ChargeState == currentIMSMSFeature.ChargeState)
								{
									// If the LC-IMS-MS Feature is within the tolerance
									if (lcimsmsFeature.MassOfMaxAbundance > massToleranceLow)
									{
										if (!m_settings.UseConformationIndex || lcimsmsFeature.ConformationIndex == currentIMSMSFeature.ConformationIndex)
										{
											// Drift Time of most abundant MSFeature Should be close
											if (Math.Abs(lcimsmsFeature.DriftTimeOfScanLCMax - currentIMSMSFeature.DriftTime) < (0.1 * (double)m_settings.IMSGapSizeMax))
											{
												// Add the IMS-MS Feature to the LC-IMS-MS Feature and Recalculate the LC-IMS-MS Feature 
												lcimsmsFeature.AddIMSMSFeature(currentIMSMSFeature);

												// Set that we found an LC-IMS-MS Feature and break out of the loop
												found = true;
												break;
											}
										}
									}
								}
							}
						}

						// If we did not find an LC-IMS-MS Feature, Create a new LC-IMS-MS Feature (Make sure to Recalculate)
						if (!found)
						{
							LCIMSMSFeature newLCIMSMSFeature = new LCIMSMSFeature();
							newLCIMSMSFeature.AddIMSMSFeature(currentIMSMSFeature);
							newLCIMSMSFeature.ChargeState = currentIMSMSFeature.ChargeState;
							newLCIMSMSFeature.ConformationIndex = currentIMSMSFeature.ConformationIndex;

							newLCIMSMSFeatureList.Add(newLCIMSMSFeature);
							completeLCIMSMSFeatureList.Add(newLCIMSMSFeature);
						}
					}

					// When we get to the next LC Scan, stop
					else
					{
						break;
					}

					// Increment the counter after everything so that the counter does not increment when the LC Scan changes.
					// When the LC Scan changes, we still need to process the Current IMS-MSFeature
					imsmsFeatureCount++;
				}

				// Allow for LC Gap
				List<LCIMSMSFeature> lcimsmsFeaturesToLeaveOpen = new List<LCIMSMSFeature>();
				foreach (LCIMSMSFeature lcimsmsFeature in openLCIMSMSFeatureList)
				{
					if ((int)currentScanLC - (int)lcimsmsFeature.ScanLCEnd - 1 <= m_settings.LCGapSizeMax)
					{
						FeatureUtil.SearchAndInsert(lcimsmsFeaturesToLeaveOpen, lcimsmsFeature, comparer);
					}
				}
				openLCIMSMSFeatureList = lcimsmsFeaturesToLeaveOpen;

				// Open all New LC-IMS-MS Features
				foreach (LCIMSMSFeature lcimsmsFeature in newLCIMSMSFeatureList)
				{
					FeatureUtil.SearchAndInsert(openLCIMSMSFeatureList, lcimsmsFeature, comparer);
				}
				newLCIMSMSFeatureList.Clear();

				// Make it so that "i" will point to the first IMS-MS Feature that is on a new LC Scan
				i += imsmsFeatureCount;
			}

			// This is where we fill in LC gaps
			if (m_settings.LCDaltonCorrectionMax > 0)
			{
				completeLCIMSMSFeatureList = AppendLCIMSMSFeatures(completeLCIMSMSFeatureList);
				completeLCIMSMSFeatureList = FillGaps(completeLCIMSMSFeatureList);
			}

			return completeLCIMSMSFeatureList;
		}

		/// <summary>
		/// Part of the Dalton Correction algrotihm. This method fills in IMS-MS Feature that have "gaps" in the IMS dimension with
		/// IMS-MS Features that fit the gap.
		/// </summary>
		/// <param name="imsmsFeatureList">List of IMS-MS Features</param>
		/// <returns>New List of IMS-MS Features</returns>
		private List<IMSMSFeature> FillGaps(List<IMSMSFeature> imsmsFeatureList)
		{
			List<IMSMSFeature> imsmsfeaturesToRemove = new List<IMSMSFeature>();

			var gapQuery = from IMSMSFeature imsmsFeature in imsmsFeatureList
						   where imsmsFeature.GapList.Count > 0
						   select imsmsFeature;

			for (int i = 0; i < gapQuery.Count(); i++)
			{
				IMSMSFeature gappedIMSMSFeature = gapQuery.ElementAt(i);

				// If the current IMS-MS Feature has not been involved in a gap fill
				if (!gappedIMSMSFeature.Remove)
				{
					IMSMSFeature featureToRemove = null;

					var query = from IMSMSFeature imsmsFeatureToFillGap in imsmsFeatureList
								where imsmsFeatureToFillGap.ScanLC == gappedIMSMSFeature.ScanLC
										&& imsmsFeatureToFillGap.ChargeState == gappedIMSMSFeature.ChargeState
										&& ((imsmsFeatureToFillGap.ScanIMSStart > gappedIMSMSFeature.ScanIMSStart && imsmsFeatureToFillGap.ScanIMSStart < gappedIMSMSFeature.ScanIMSEnd)
											|| (imsmsFeatureToFillGap.ScanIMSEnd < gappedIMSMSFeature.ScanIMSEnd && imsmsFeatureToFillGap.ScanIMSEnd > gappedIMSMSFeature.ScanIMSStart))
								orderby Math.Abs(imsmsFeatureToFillGap.MassMonoisotopicMedian - gappedIMSMSFeature.MassMonoisotopicMedian)
								select imsmsFeatureToFillGap;

					foreach (IMSMSFeature imsmsFeatureToFillGap in query)
					{
						bool canFillGap = false;

						// First check if the IMS-MS Feature can fill in at least 1 gap
						foreach (int gap in gappedIMSMSFeature.GapList)
						{
							if (imsmsFeatureToFillGap.ScanIMSList.Contains(gap))
							{
								double massOfScanIMSBeforeGap = -100;
								double massOfFiller = -50;

								// Find the Mass of the MS Feature in the Current IMS-MS Feature that is in the first IMS Scan before the beginning of the Gap
								gappedIMSMSFeature.MSFeatureList.Sort(MSFeature.ScanIMSComparison);
								foreach (MSFeature msFeature in gappedIMSMSFeature.MSFeatureList)
								{
									if (msFeature.ScanIMS < gap)
									{
										massOfScanIMSBeforeGap = msFeature.MassMonoisotopic;
									}
									else
									{
										break;
									}
								}

								// Find the Mass of the MS Feature that is first MS Feature that will be used to fill the Gap
								foreach (MSFeature msFeature in imsmsFeatureToFillGap.MSFeatureList)
								{
									if (msFeature.ScanIMS == gap)
									{
										massOfFiller = msFeature.MassMonoisotopic;
										break;
									}
								}

								// Check if the Mass for the Gap Filler is within the Mass Tolerance of the Gap
								double massToleranceOfScanIMSBeforeGap = m_settings.MassMonoisotopicConstraint * massOfScanIMSBeforeGap / 1000000 * 1.25;
								double massDifferenceForGap = Math.Abs(massOfScanIMSBeforeGap - massOfFiller);

								bool broke = false;

								// Check the mass difference for each Da until we reach past the Max Da Correction
								for (int j = 1; j <= m_settings.IMSDaltonCorrectionMax; j++)
								{
									// Mass Difference must be within j Da +- X ppm
									if (massDifferenceForGap < (j + massToleranceOfScanIMSBeforeGap) && massDifferenceForGap > (j - massToleranceOfScanIMSBeforeGap))
									{
										canFillGap = true;
										broke = true;
										break;
									}
								}

								if (broke) break;
							}
						}

						if (canFillGap)
						{
							// Next make sure that the IMS-MS Features do not contain the same IMS Scan
							foreach (int scanIMS in gappedIMSMSFeature.ScanIMSList)
							{
								if (imsmsFeatureToFillGap.ScanIMSList.Contains(scanIMS))
								{
									canFillGap = false;
									break;
								}
							}

							// Only fill the Gap if all tests pass
							if (canFillGap)
							{
								gappedIMSMSFeature.DaError = true;
								gappedIMSMSFeature.AddMSFeatureList(imsmsFeatureToFillGap.MSFeatureList);

								FeatureUtil.CorrectMassMostAbundant(gappedIMSMSFeature);
								gappedIMSMSFeature.Recalculate();

								featureToRemove = imsmsFeatureToFillGap;
								imsmsFeatureToFillGap.Remove = true;

								// After filling the gap, if the current IMS-MS Feature contains more gaps, process it again
								if (gappedIMSMSFeature.GapList.Count != 0)
								{
									i--;
								}

								// Exit the gap filling process
								break;
							}
						}
					}

					if (featureToRemove != null)
					{
						imsmsFeatureList.Remove(featureToRemove);
					}
				}
			}

			return imsmsFeatureList;
		}

		/// <summary>
		/// Part of the Dalton Correction algrotihm. This method fills in LC-IMS-MS Feature that have "gaps" in the LC dimension with
		/// LC-IMS-MS Features that fit the gap.
		/// </summary>
		/// <param name="lcimsmsFeatureList">List of LC-IMS-MS Features</param>
		/// <returns>New List of LC-IMS-MS Features</returns>
		private List<LCIMSMSFeature> FillGaps(List<LCIMSMSFeature> lcimsmsFeatureList)
		{
			List<LCIMSMSFeature> gappedLCIMSMSFeatureList = new List<LCIMSMSFeature>();
			List<LCIMSMSFeature> lcimsmsfeaturesToRemove = new List<LCIMSMSFeature>();

			AnonymousComparer<IMSMSFeature> scanLCComparer = new AnonymousComparer<IMSMSFeature>(new Comparison<IMSMSFeature>(Feature.ScanLCComparison));
			AnonymousComparer<LCIMSMSFeature> scanLCStartComparer = new AnonymousComparer<LCIMSMSFeature>(new Comparison<LCIMSMSFeature>(UMC.ScanLCStartComparison));

			lcimsmsFeatureList.Sort(new Comparison<LCIMSMSFeature>(UMC.ScanLCStartComparison));

			foreach (LCIMSMSFeature lcimsmsFeature in lcimsmsFeatureList)
			{
				lcimsmsFeature.CalculateGapList();

				if (lcimsmsFeature.GapLCList.Count > 0)
				{
					gappedLCIMSMSFeatureList.Add(lcimsmsFeature);
				}
			}

			for (int i = 0; i < gappedLCIMSMSFeatureList.Count; i++)
			{
				LCIMSMSFeature gappedLCIMSMSFeature = gappedLCIMSMSFeatureList[i];

				// If the current LC-IMS-MS Feature has not been involved in a gap fill
				if (!gappedLCIMSMSFeature.Remove)
				{
					int indexOfFeatureToRemove = -1;

					LCIMSMSFeature dummyLCIMSMSFeature = new LCIMSMSFeature();

					dummyLCIMSMSFeature.ScanLCStart = gappedLCIMSMSFeature.ScanLCStart;
					int startIndex = lcimsmsFeatureList.BinarySearch(dummyLCIMSMSFeature, scanLCStartComparer);
					if (startIndex < 0) startIndex = ~startIndex;

					dummyLCIMSMSFeature.ScanLCStart = gappedLCIMSMSFeature.ScanLCEnd + 1;
					int endIndex = lcimsmsFeatureList.BinarySearch(dummyLCIMSMSFeature, scanLCStartComparer);
					if (endIndex < 0) endIndex = ~endIndex;

					for (int index = startIndex; index < endIndex; index++)
					{
						LCIMSMSFeature lcimsmsFeatureToFillGap = lcimsmsFeatureList[index];

						if (!lcimsmsFeatureToFillGap.Remove)
						{

							if (!m_settings.UseCharge || gappedLCIMSMSFeature.ChargeState == lcimsmsFeatureToFillGap.ChargeState)
							{
								if (!m_settings.UseConformationIndex || lcimsmsFeatureToFillGap.ConformationIndex == gappedLCIMSMSFeature.ConformationIndex)
								{
									// Drift Times should line up
									if (Math.Abs(lcimsmsFeatureToFillGap.DriftTimeOfScanLCMax - gappedLCIMSMSFeature.DriftTime) < (0.1 * (double)m_settings.IMSGapSizeMax))
									{
										bool canFillGap = false;

										// First check if the LC-IMS-MS Feature can fill in at least 1 gap
										foreach (int gap in gappedLCIMSMSFeature.GapLCList)
										{
											if (lcimsmsFeatureToFillGap.ScanLCList.Contains(gap))
											{
												IMSMSFeature dummyIMSMSFeature = new IMSMSFeature();
												dummyIMSMSFeature.ScanLC = gap;

												List<IMSMSFeature> imsmsFeatureListForScanLCBeforeGap = gappedLCIMSMSFeature.IMSMSFeatureList;
												List<IMSMSFeature> imsmsFeatureListForFiller = lcimsmsFeatureToFillGap.IMSMSFeatureList;

												int indexForScanLCBeforeGap = imsmsFeatureListForScanLCBeforeGap.BinarySearch(dummyIMSMSFeature, scanLCComparer);
												int indexForFiller = imsmsFeatureListForFiller.BinarySearch(dummyIMSMSFeature, scanLCComparer);

												double massOfScanLCBeforeGap = imsmsFeatureListForScanLCBeforeGap[~indexForScanLCBeforeGap - 1].MassOfMaxAbundance;
												double massOfFiller = imsmsFeatureListForFiller[indexForFiller].MassOfMaxAbundance;

												// Check if the Mass for the Gap Filler is within the Mass Tolerance of the Gap
												double massToleranceOfScanLCBeforeGap = m_settings.MassMonoisotopicConstraint * massOfScanLCBeforeGap / 1000000 * 1.25;
												double massDifferenceForGap = Math.Abs(massOfScanLCBeforeGap - massOfFiller);

												bool broke = false;

												// Check the mass difference for each Da until we reach past the Max Da Correction
												for (int j = 1; j <= m_settings.LCDaltonCorrectionMax; j++)
												{
													// Mass Difference must be within j Da +- X ppm
													if (massDifferenceForGap < (j + massToleranceOfScanLCBeforeGap) && massDifferenceForGap > (j - massToleranceOfScanLCBeforeGap))
													{
														canFillGap = true;
														broke = true;
														break;
													}
												}

												if (broke) break;
											}
										}

										if (canFillGap)
										{
											// Next make sure that the LC-IMS-MS Features do not contain the same LC Scan
											foreach (int scanLC in gappedLCIMSMSFeature.ScanLCList)
											{
												if (lcimsmsFeatureToFillGap.ScanLCList.Contains(scanLC))
												{
													canFillGap = false;
													break;
												}
											}

											// Only fill the Gap if all tests pass
											if (canFillGap)
											{
												gappedLCIMSMSFeature.DaError = true;
												gappedLCIMSMSFeature.AddIMSMSFeatureList(lcimsmsFeatureToFillGap.IMSMSFeatureList);

												FeatureUtil.CorrectMassMostAbundant(gappedLCIMSMSFeature);
												foreach (IMSMSFeature imsmsFeature in gappedLCIMSMSFeature.IMSMSFeatureList)
												{
													imsmsFeature.Recalculate();
												}
												gappedLCIMSMSFeature.Recalculate();
												gappedLCIMSMSFeature.CalculateGapList();

												indexOfFeatureToRemove = index;
												lcimsmsFeatureToFillGap.Remove = true;

												// After filling the gap, if the current LC-IMS-MS Feature contains more gaps, process it again
												if (gappedLCIMSMSFeature.GapLCList.Count != 0)
												{
													i--;
												}

												// Exit the gap filling process
												break;
											}
										}
									}
								}
							}
						}
					}
				}
			}

			var query = from LCIMSMSFeature lcimsmsFeature in lcimsmsFeatureList
						where !lcimsmsFeature.Remove
						select lcimsmsFeature;

			return query.ToList();
		}

		/// <summary>
		/// Part of the Dalton Correction algrotihm. This method appends IMS-MS Features with other IMS-MS Features that meet specific criteria.
		/// </summary>
		/// <param name="imsmsFeatureList">List of IMS-MS Features</param>
		/// <returns>New List of IMS-MS Features</returns>
		private List<IMSMSFeature> AppendIMSMSFeatures(List<IMSMSFeature> imsmsFeatureList)
		{
			// Sort all MS Feature Lists by IMS Scan
			foreach (IMSMSFeature imsmsFeature in imsmsFeatureList)
			{
				imsmsFeature.MSFeatureList.Sort(MSFeature.ScanIMSComparison);
			}

			for (int i = 0; i < imsmsFeatureList.Count; i++)
			{
				IMSMSFeature currentIMSMSFeature = imsmsFeatureList[i];

				// If the current IMS-MS Feature has not been involved in an Append
				if (!currentIMSMSFeature.Remove)
				{
					double massTolerance = m_settings.MassMonoisotopicConstraint * currentIMSMSFeature.MassOfMaxAbundance / 1000000 * 1.25;

					for (int j = 1; j <= m_settings.IMSDaltonCorrectionMax; j++)
					{
						var appendQuery = from IMSMSFeature imsmsFeatureToAppend in imsmsFeatureList
										  where imsmsFeatureToAppend.ScanLC == currentIMSMSFeature.ScanLC
												  && imsmsFeatureToAppend.ChargeState == currentIMSMSFeature.ChargeState
												  && (Math.Abs(imsmsFeatureToAppend.MassOfMaxAbundance - currentIMSMSFeature.MassOfMaxAbundance) < (j + massTolerance) && Math.Abs(imsmsFeatureToAppend.MassOfMaxAbundance - currentIMSMSFeature.MassOfMaxAbundance) > (j - massTolerance))
												  && ((imsmsFeatureToAppend.ScanIMSEnd < currentIMSMSFeature.ScanIMSStart && (int)currentIMSMSFeature.ScanIMSStart - (int)imsmsFeatureToAppend.ScanIMSEnd - 1 <= m_settings.IMSGapSizeMax)
														  || (imsmsFeatureToAppend.ScanIMSStart > currentIMSMSFeature.ScanIMSEnd && (int)imsmsFeatureToAppend.ScanIMSStart - (int)currentIMSMSFeature.ScanIMSEnd - 1 <= m_settings.IMSGapSizeMax))
												  && !imsmsFeatureToAppend.Remove
										  orderby Math.Abs(imsmsFeatureToAppend.MassOfMaxAbundance - currentIMSMSFeature.MassOfMaxAbundance)
										  select imsmsFeatureToAppend;

						if (appendQuery.Count() > 0)
						{
							IMSMSFeature imsmsFeatureToAppend = appendQuery.First();
							imsmsFeatureToAppend.Remove = true;

							currentIMSMSFeature.DaError = true;
							currentIMSMSFeature.AddMSFeatureList(imsmsFeatureToAppend.MSFeatureList);

							FeatureUtil.CorrectMassMostAbundant(currentIMSMSFeature);
							currentIMSMSFeature.Recalculate();

							i--;
							break;
						}
					}
				}
			}

			var query = from IMSMSFeature imsmsFeature in imsmsFeatureList
						where !imsmsFeature.Remove
						select imsmsFeature;

			return query.ToList();
		}

		/// <summary>
		/// Part of the Dalton Correction algrotihm. This method appends LC-IMS-MS Features with other LC-IMS-MS Features that meet specific criteria.
		/// </summary>
		/// <param name="lcimsmsFeatureList">List of LC-IMS-MS Features</param>
		/// <returns>New List of LC-IMS-MS Features</returns>
		private List<LCIMSMSFeature> AppendLCIMSMSFeatures(List<LCIMSMSFeature> lcimsmsFeatureList)
		{
			AnonymousComparer<IMSMSFeature> scanLCComparer = new AnonymousComparer<IMSMSFeature>(new Comparison<IMSMSFeature>(Feature.ScanLCComparison));
			AnonymousComparer<LCIMSMSFeature> scanLCStartComparer = new AnonymousComparer<LCIMSMSFeature>(new Comparison<LCIMSMSFeature>(UMC.ScanLCStartComparison));

			List<LCIMSMSFeature> lcimsmsFeatureListCopy = new List<LCIMSMSFeature>();
			lcimsmsFeatureListCopy.AddRange(lcimsmsFeatureList);

			for (int i = 0; i < lcimsmsFeatureList.Count; i++)
			{
				LCIMSMSFeature currentLCIMSMSFeature = lcimsmsFeatureList[i];

				if (!currentLCIMSMSFeature.Remove)
				{
					double massTolerance = m_settings.MassMonoisotopicConstraint * currentLCIMSMSFeature.MassOfMaxAbundance / 1000000 * 1.25;

					for (int j = 1; j <= m_settings.LCDaltonCorrectionMax; j++)
					{
						var appendQuery = from LCIMSMSFeature lcimsmsFeatureToAppend in lcimsmsFeatureListCopy
										  where lcimsmsFeatureToAppend.ChargeState == currentLCIMSMSFeature.ChargeState
												  && (Math.Abs(lcimsmsFeatureToAppend.MassOfMaxAbundance - currentLCIMSMSFeature.MassOfMaxAbundance) < (j + massTolerance) && Math.Abs(lcimsmsFeatureToAppend.MassOfMaxAbundance - currentLCIMSMSFeature.MassOfMaxAbundance) > (j - massTolerance))
												  && Math.Abs(lcimsmsFeatureToAppend.DriftTime - currentLCIMSMSFeature.DriftTimeOfScanLCMax) < (0.1 * (double)m_settings.IMSGapSizeMax)
												  && ((lcimsmsFeatureToAppend.ScanLCEnd < currentLCIMSMSFeature.ScanLCStart && (int)currentLCIMSMSFeature.ScanLCStart - (int)lcimsmsFeatureToAppend.ScanLCEnd - 1 <= m_settings.LCGapSizeMax)
														  || (lcimsmsFeatureToAppend.ScanLCStart > currentLCIMSMSFeature.ScanLCEnd && (int)lcimsmsFeatureToAppend.ScanLCStart - (int)currentLCIMSMSFeature.ScanLCEnd - 1 <= m_settings.LCGapSizeMax))
												  && !lcimsmsFeatureToAppend.Remove
										  orderby Math.Abs(lcimsmsFeatureToAppend.MassOfMaxAbundance - currentLCIMSMSFeature.MassOfMaxAbundance)
										  select lcimsmsFeatureToAppend;

						if (appendQuery.Count() > 0)
						{
							LCIMSMSFeature lcimsmsFeatureToAppend = appendQuery.First();
							lcimsmsFeatureToAppend.Remove = true;

							currentLCIMSMSFeature.DaError = true;
							currentLCIMSMSFeature.AddIMSMSFeatureList(lcimsmsFeatureToAppend.IMSMSFeatureList);

							FeatureUtil.CorrectMassMostAbundant(currentLCIMSMSFeature);
							currentLCIMSMSFeature.Recalculate();

							i--;
							break;
						}
					}
				}
			}

			var query = from LCIMSMSFeature lcimsmsFeature in lcimsmsFeatureList
						where !lcimsmsFeature.Remove
						select lcimsmsFeature;

			return query.ToList();
		}

		/// <summary>
		/// Calculates statistics of the given IMS-MS Features.
		/// Currently, this only includes attaching an ID to the Feature.
		/// </summary>
		/// <param name="imsmsFeatureList">List of IMS-MS Features</param>
		/// <returns>New List of IMS-MS Features that have statistics calculated</returns>
		public List<IMSMSFeature> CalculateIMSMSFeatureStatistics(List<IMSMSFeature> imsmsFeatureList)
		{
			imsmsFeatureList.Sort(new Comparison<IMSMSFeature>(Feature.ScanLCAndMassComparison));

			int imsmsFeatureIndex = 0;

			foreach (IMSMSFeature imsmsFeature in imsmsFeatureList)
			{
				imsmsFeature.ID = imsmsFeatureIndex;
				imsmsFeatureIndex++;
			}

			return imsmsFeatureList;
		}

		/// <summary>
		/// Calculates statistics of the given LC-IMS-MS Features.
		/// </summary>
		/// <param name="lcimsmsFeatureList">List of LC-IMS-MS Features</param>
		/// <returns>New List of LC-IMS-MS Features that have statistics calculated</returns>
		public List<LCIMSMSFeature> CalculateLCIMSMSFeatureStatistics(List<LCIMSMSFeature> lcimsmsFeatureList)
		{
			lcimsmsFeatureList.Sort(new Comparison<LCIMSMSFeature>(UMC.ScanLCStartAndMassComparison));

			List<double> conformationFitScoreList = new List<double>();
			int lcimsmsFeatureIndex = 0;

			foreach (LCIMSMSFeature lcimsmsFeature in lcimsmsFeatureList)
			{
				lcimsmsFeature.ID = lcimsmsFeatureIndex;
				lcimsmsFeatureIndex++;

				List<double> massMonoisotopicList = new List<double>();
				int abundanceSum = 0;
				int msFeatureCount = 0;

				foreach (MSFeature msFeature in lcimsmsFeature.MSFeatureList)
				{
					massMonoisotopicList.Add(msFeature.MassMonoisotopic);
					abundanceSum += msFeature.Abundance;
					msFeatureCount++;
				}

				massMonoisotopicList.Sort();

				if (massMonoisotopicList.Count % 2 == 1)
				{
					lcimsmsFeature.MassMonoisotopicMedian = massMonoisotopicList[massMonoisotopicList.Count / 2];
				}
				else
				{
					lcimsmsFeature.MassMonoisotopicMedian = 0.5 * (massMonoisotopicList[massMonoisotopicList.Count / 2 - 1] + massMonoisotopicList[massMonoisotopicList.Count / 2]);
				}

				lcimsmsFeature.MassMonoisotopicMinimum = massMonoisotopicList[0];
				lcimsmsFeature.MassMonoisotopicMaximum = massMonoisotopicList[massMonoisotopicList.Count - 1];

				foreach (IMSMSFeature imsmsFeature in lcimsmsFeature.IMSMSFeatureList)
				{
					conformationFitScoreList.Add(imsmsFeature.ConformationFitScore);
				}

				if (conformationFitScoreList.Count % 2 == 1)
				{
					lcimsmsFeature.ConformationFitScore = conformationFitScoreList[conformationFitScoreList.Count / 2];
				}
				else
				{
					lcimsmsFeature.ConformationFitScore = 0.5 * (conformationFitScoreList[conformationFitScoreList.Count / 2 - 1] + conformationFitScoreList[conformationFitScoreList.Count / 2]);
				}
			}

			return lcimsmsFeatureList;
		}

		/// <summary>
		/// Performs conformation detection on the given List of IMS-MS Features
		/// </summary>
		/// <param name="imsmsFeatureList">List of IMS-MS Features</param>
		/// <param name="uimfReader">DataReader object representing the UIMF file to be used as the raw data</param>
		/// <returns>New List of IMS-MS Features that underwent conformation detection</returns>
		public List<IMSMSFeature> PerformConformationDetection(List<IMSMSFeature> imsmsFeatureList, DataReader uimfReader)
		{
			List<IMSMSFeature> newIMSMSFeatureList = new List<IMSMSFeature>();

			ConformerUtil conformerUtil = new ConformerUtil(m_settings, uimfReader);

			foreach (IMSMSFeature imsmsFeature in imsmsFeatureList)
			{
				List<double> driftTimeFitScoreList;
				List<float> driftTimeList = conformerUtil.ComputeDriftTime(imsmsFeature, 3, out driftTimeFitScoreList);

				if (driftTimeList.Count == 0)
				{
					newIMSMSFeatureList.Add(imsmsFeature);
				}
				else
				{
					// First update the original IMS-MS feature with the first Drift Time
					imsmsFeature.DriftTime = driftTimeList[0];
					imsmsFeature.ConformationIndex = 0;
					imsmsFeature.ConformationFitScore = driftTimeFitScoreList[0];
					newIMSMSFeatureList.Add(imsmsFeature);

					// Then create new IMS-MS Features for the rest of the Drift Times
					for (int i = 1; i < driftTimeList.Count; i++)
					{
						IMSMSFeature newIMSMSFeature = new IMSMSFeature(imsmsFeature);
						newIMSMSFeature.DriftTime = driftTimeList[i];
						newIMSMSFeature.ConformationIndex = i;
						newIMSMSFeature.ConformationFitScore = driftTimeFitScoreList[i];
						newIMSMSFeatureList.Add(newIMSMSFeature);
					}
				}
			}

			return newIMSMSFeatureList;
		}

		/// <summary>
		/// Refines a List of IMS-MS Features based on the feature length (# of MS Features)
		/// </summary>
		/// <param name="imsmsFeatureList">List of IMS-MS Features</param>
		/// <returns>Refined List of IMS-MS Features</returns>
		public List<IMSMSFeature> RefineIMSMSFeaturesByFeatureLength(List<IMSMSFeature> imsmsFeatureList)
		{
			List<IMSMSFeature> imsmsFeaturesToKeep = new List<IMSMSFeature>();

			foreach (IMSMSFeature imsmsFeature in imsmsFeatureList)
			{
				if (imsmsFeature.MSFeatureList.Count >= m_settings.FeatureLengthMin)
				{
					imsmsFeaturesToKeep.Add(imsmsFeature);
				}
			}

			return imsmsFeaturesToKeep;
		}

		/// <summary>
		/// Refines a List of LC-IMS-MS Features based on the feature length (# of MS Features)
		/// </summary>
		/// <param name="lcimsmsFeatureList">List of LC-IMS-MS Features</param>
		/// <returns>Refined List of LC-IMS-MS Features</returns>
		public List<LCIMSMSFeature> RefineLCIMSMSFeaturesByFeatureLength(List<LCIMSMSFeature> lcimsmsFeatureList)
		{
			List<LCIMSMSFeature> lcimsmsFeaturesToKeep = new List<LCIMSMSFeature>();

			foreach (LCIMSMSFeature lcimsmsFeature in lcimsmsFeatureList)
			{
				if (lcimsmsFeature.MSFeatureList.Count >= m_settings.FeatureLengthMin)
				{
					lcimsmsFeaturesToKeep.Add(lcimsmsFeature);
				}
			}

			return lcimsmsFeaturesToKeep;
		}
	}
}
