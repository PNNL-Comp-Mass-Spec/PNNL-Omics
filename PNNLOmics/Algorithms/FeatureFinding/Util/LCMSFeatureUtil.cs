using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using PNNLOmics.Algorithms.FeatureFinding.Control;
using PNNLOmics.Data.Features;
using PNNLOmics.Generic;
using PNNLOmics.Algorithms.FeatureFinding.Data;

namespace PNNLOmics.Algorithms.FeatureFinding.Util
{
	/// <summary>
	/// Utility class for LC-MS Features.
	/// </summary>
	public class LCMSFeatureUtil
	{
		private Logger m_logger;
		private int m_numOfMSFeaturesMappedToLCMSFeatures;
		private bool m_useCharge;
		private Settings m_settings;

		/// <summary>
		/// Constructor that takes a Settings object and a Logger object.
		/// </summary>
		/// <param name="settings">Settings object</param>
		/// <param name="logger">Logger object</param>
		public LCMSFeatureUtil(Settings settings, Logger logger)
		{
			m_settings = settings;
			m_useCharge = m_settings.UseCharge;
			m_logger = logger;
		}

		/// <summary>
		/// The total number of MS Features that are mapped to an LC-MS Feature
		/// </summary>
		public int NumOfMSFeaturesMappedToLCMSFeatures
		{
			get { return m_numOfMSFeaturesMappedToLCMSFeatures; }
		}

		/*
		public List<LCMSFeature> ProcessMSFeaturesOld(List<MSFeature> msFeatureList)
		{
			List<LCMSFeature> lcmsFeatureList = new List<LCMSFeature>();
			LCMSFeature currentLCMSFeature = null;
			m_numOfMSFeatures = msFeatureList.Count;

			// Sorts by mass descending
			msFeatureList.Sort();

			// Stop processing when there is only 1 MSFeature left in the list
			while(msFeatureList.Count > 1)
			{
				MSFeature currentMSFeature = msFeatureList[msFeatureList.Count - 1];

				// If the MSFeature does not belong to an LCMSFeature, then create an LCMSFeature, link the MSFeature to the LCMSFeature, and add the LCMSFeature to the list of LCMSFeatures
				if (currentMSFeature.LCMSFeature == null)
				{
					currentLCMSFeature = new LCMSFeature(0);

					AddMSFeatureToLCMSFeature(currentMSFeature, currentLCMSFeature);
					lcmsFeatureList.Add(currentLCMSFeature);
				}
				// If the MSFeature does belong to an LCMSFeature, use it
				else
				{
					currentLCMSFeature = currentMSFeature.LCMSFeature;
				}

				// Build the LCMSFeature by using the MSFeatures that are earlier on in the msFeatureList than the Current MSFeature
				CreateLCMSFeature(currentMSFeature, currentLCMSFeature, ref msFeatureList, ref lcmsFeatureList);

				// Remove the current MSFeature (last in the list), we don't need it anymore
				msFeatureList.RemoveAt(msFeatureList.Count - 1);
			}

			return lcmsFeatureList;
		}
		*/

		/// <summary>
		/// Clusters MS Features in the LC dimension to create LC-MS Features.
		/// </summary>
		/// <param name="msFeatureList">List of MS Features to be clustered</param>
		/// <returns>List of LC-MS Features</returns>
		public List<LCMSFeature> ProcessMSFeatures(List<MSFeature> msFeatureList)
		{
			MSFeature currentMSFeature = null;
			List<LCMSFeature> openLCMSFeatureList = new List<LCMSFeature>();
			List<LCMSFeature> completeLCMSFeatureList = new List<LCMSFeature>();
			int currentScanLC = 0;
			double massToleranceBase = m_settings.MassMonoisotopicConstraint;
			AnonymousComparer<LCMSFeature> comparer = new AnonymousComparer<LCMSFeature>(new Comparison<LCMSFeature>(UMC.MassMaxComparison));
			int lcDaCorrectionMax = m_settings.LCDaltonCorrectionMax;

			// Sort MS Features by LC Scan, then Mass
			msFeatureList.Sort(new Comparison<MSFeature>(Feature.ScanLCAndMassComparison));

			// Iterate over all MS Features
			int i = 0;
			while (i < msFeatureList.Count)
			{
				// Grab the current MSFeature and keep track of its LC Scan
				currentMSFeature = msFeatureList[i];
				currentScanLC = currentMSFeature.ScanLC;

				// Keeps track of how many MS Features we process for the current LC Scan
				int msFeatureCount = 0;

				for (int j = i; j < msFeatureList.Count; j++)
				{
					// If this is not the first iteration, get the current MS Feature
					if (j != i)
					{
						currentMSFeature = msFeatureList[j];
					}

					// Keep going as long as we are still in the same LC Scan
					if (currentMSFeature.ScanLC == currentScanLC)
					{
						// Calculate the High and Low Mass Tolerance for the Current MS Feature
						double massTolerance = massToleranceBase * currentMSFeature.MassMonoisotopic / 1000000;
						double massToleranceHigh = currentMSFeature.MassMonoisotopic + massTolerance;

						// Keeps track of if the Current MS Feature finds an LC-MS Feature to live in
						Boolean found = false;

						// Iterate over all Open LC-MS Features
						foreach (LCMSFeature lcmsFeature in openLCMSFeatureList)
						{
							// Stop if we find an LC-MS feature with too high of a Mass (greater than the 1 Da Error tolerance)
							if (lcmsFeature.MassMonoisotopicMaximum >= massToleranceHigh + lcDaCorrectionMax)
							{
								break;
							}

							// If the user has set to not use Charge or the charge states are equal
							else if (!m_settings.UseCharge || lcmsFeature.ChargeState == currentMSFeature.ChargeState)
							{
								double massDifference = Math.Abs(currentMSFeature.MassMonoisotopic - lcmsFeature.MassOfMaxAbundance);

								// If the IMS-MS Feature is within the tolerance
								//if (massDifference < massTolerance)
								// If the LC-MS Feature is within the tolerance or within the 1 Da tolerance
								if (TryInsertMSFeature(lcmsFeature, currentMSFeature))
								{
									// Add the MS Feature to the LC-MS Feature
									lcmsFeature.AddMSFeature(currentMSFeature);

									// Set that we found an LC-MS Feature and break from the loop
									found = true;
									break;
								}
							}
						}

						// If we did not find an LC-MS Feature, Create a new LC-MS Feature
						if (!found)
						{
							LCMSFeature newLCMSFeature = new LCMSFeature(lcDaCorrectionMax);
							newLCMSFeature.AddMSFeature(currentMSFeature);

							UniqueMass uniqueMass = newLCMSFeature.MassList[(int)lcDaCorrectionMax];
							uniqueMass.Mass = currentMSFeature.MassMonoisotopic;
							uniqueMass.MSFeatureList.Add(currentMSFeature);

							newLCMSFeature.ChargeState = currentMSFeature.ChargeState;

							FeatureUtil.SearchAndInsert(openLCMSFeatureList, newLCMSFeature, comparer);
							completeLCMSFeatureList.Add(newLCMSFeature);
						}
					}

					// When we get to the next LC Scan, stop
					else
					{
						break;
					}

					// Increment the counter after everything so that the counter does not increment when the LC Scan changes.
					// When the LC Scan changes, we still need to process the Current MSFeature
					msFeatureCount++;
				}

				// Allow for LC Gap
				List<LCMSFeature> lcmsFeaturesToLeaveOpen = new List<LCMSFeature>();
				foreach (LCMSFeature lcmsFeature in openLCMSFeatureList)
				{
					if ((int)currentScanLC - (int)lcmsFeature.ScanLCEnd - 1 <= m_settings.LCGapSizeMax)
					{
						FeatureUtil.SearchAndInsert(lcmsFeaturesToLeaveOpen, lcmsFeature, comparer);
					}
				}
				openLCMSFeatureList = lcmsFeaturesToLeaveOpen;

				// Make it so that "i" will point to the first MSFeature that is on a new LC Scan
				i += msFeatureCount;
			}

			return completeLCMSFeatureList;
		}

		/// <summary>
		/// Attempts to insert an MS Feature into an LC-MS Feature based on specific criteria.
		/// </summary>
		/// <param name="lcmsFeature">LC-MS Feature to be inserted into</param>
		/// <param name="msFeature">MS Feature to be inserted</param>
		/// <returns>True is inserted, false otherwise</returns>
		private bool TryInsertMSFeature(LCMSFeature lcmsFeature, MSFeature msFeature)
		{
			List<UniqueMass> massList = lcmsFeature.MassList;
			int middleOfMassList = (int)Math.Floor(massList.Count / 2.0);

			// Find out which bin the MSFeature would possibly belong to
			double difference = msFeature.MassMonoisotopic - massList[middleOfMassList].Mass;
			int differenceInt = (int)Math.Round(difference);

			if (differenceInt < -middleOfMassList || differenceInt > middleOfMassList)
			{
				return false;
			}

			// Grab the necessary bin
			UniqueMass uniqueMass = massList[middleOfMassList + differenceInt];

			// If this bin is closed, exit
			if (uniqueMass == null)
			{
				return false;
			}

			// Calculate the PPM tolerance
			double massTolerance = m_settings.MassMonoisotopicConstraint * msFeature.MassMonoisotopic / 1000000;

			// If this is the first Mass going into this bin
			if (uniqueMass.MSFeatureList.Count == 0 && (lcmsFeature.Suspicious || msFeature.Suspicious))
			{
				// Adjust the mass so that it is within 1 Da of the middle bin for this LC-MS Feature and calculate the Difference
				double massAdjusted = msFeature.MassMonoisotopic - differenceInt;
				double massDifference = Math.Abs(massAdjusted - massList[middleOfMassList].Mass);

				// If within the tolerance
				if (massDifference < massTolerance)
				{
					// Add the MSFeature to the bin
					uniqueMass.Mass = msFeature.MassMonoisotopic;
					uniqueMass.MSFeatureList.Add(msFeature);

					// Close the appropriate bins
					if (differenceInt < 0)
					{
						for (int i = 0; differenceInt + massList.Count + i < massList.Count; i++)
						{
							massList[differenceInt + massList.Count + i] = null;
						}
					}
					else if (differenceInt > 0)
					{
						for (int i = 0; differenceInt - 1 - i >= 0; i++)
						{
							massList[differenceInt - 1 - i] = null;
						}
					}

					lcmsFeature.DaError = true;

					return true;
				}

				// If not within the tolerance, exit
				else
				{
					return false;
				}
			}

			// There is already a Mass in this bin
			else
			{
				double massDifference = Math.Abs(msFeature.MassMonoisotopic - uniqueMass.Mass);

				// If within the tolerance
				if (massDifference < massTolerance)
				{
					// Add the MSFeature to the bin
					uniqueMass.Mass = msFeature.MassMonoisotopic;
					uniqueMass.MSFeatureList.Add(msFeature);
					return true;
				}

				// If not within the tolerance, exit
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Refines a List of LC-MS Feature based on the number of MS Features
		/// </summary>
		/// <param name="lcmsFeatureList">List of LC-MS Features to be refined</param>
		/// <returns>Refined List of LC-MS Features</returns>
		public List<LCMSFeature> RefineLCMSFeaturesByFeatureLength(List<LCMSFeature> lcmsFeatureList)
		{
			List<LCMSFeature> lcmsFeaturesToKeep = new List<LCMSFeature>();

			foreach (LCMSFeature lcmsFeature in lcmsFeatureList)
			{
				if (lcmsFeature.MSFeatureList.Count < m_settings.FeatureLengthMin)
				{
					foreach (MSFeature msFeature in lcmsFeature.MSFeatureList)
					{
						msFeature.UMC = null;
					}
				}
				else
				{
					lcmsFeaturesToKeep.Add(lcmsFeature);
				}
			}

			return lcmsFeaturesToKeep;
		}

		/// <summary>
		/// Calculates statistics of a List of LC-MS Features
		/// </summary>
		/// <param name="lcmsFeatureList">List of LC-MS Features</param>
		/// <returns>List of LC-MS Features that have statistics calculated</returns>
		public List<LCMSFeature> CalculateLCMSFeatureStatistics(List<LCMSFeature> lcmsFeatureList)
		{
			int lcmsFeatureIndex = 0;
			m_numOfMSFeaturesMappedToLCMSFeatures = 0;

			foreach (LCMSFeature lcmsFeature in lcmsFeatureList)
			{
				int msFeatureIndex = 0;
				int msFeatureCount = lcmsFeature.MSFeatureList.Count;
				m_numOfMSFeaturesMappedToLCMSFeatures += msFeatureCount;
				double massMonoisotopicSum = 0;
				List<double> massMonoisotopicList = new List<double>();

				lcmsFeature.Clear();
				lcmsFeature.ID = lcmsFeatureIndex;

				foreach (MSFeature msFeature in lcmsFeature.MSFeatureList)
				{
					if (msFeature.ScanLC < lcmsFeature.ScanLCStart)
					{
						lcmsFeature.ScanLCStart = msFeature.ScanLC;
					}
					if (msFeature.ScanLC > lcmsFeature.ScanLCEnd)
					{
						lcmsFeature.ScanLCEnd = msFeature.ScanLC;
					}
					if (msFeature.Abundance > lcmsFeature.AbundanceMaximum)
					{
						lcmsFeature.AbundanceMaximum = msFeature.Abundance;
						lcmsFeature.ChargeState = msFeature.ChargeState;
						lcmsFeature.MZ = msFeature.MZ;
						lcmsFeature.ScanLCOfMaxAbundance = msFeature.ScanLC;
					}
					if (msFeature.MassMonoisotopic < lcmsFeature.MassMonoisotopicMinimum)
					{
						lcmsFeature.MassMonoisotopicMinimum = msFeature.MassMonoisotopic;
					}
					if (msFeature.MassMonoisotopic > lcmsFeature.MassMonoisotopicMaximum)
					{
						lcmsFeature.MassMonoisotopicMaximum = msFeature.MassMonoisotopic;
					}

					lcmsFeature.AbundanceSum += msFeature.Abundance;
					massMonoisotopicSum += msFeature.MassMonoisotopic;
					massMonoisotopicList.Add(msFeature.MassMonoisotopic);

					msFeatureIndex++;
				}

				lcmsFeature.MassMonoisotopicAverage = massMonoisotopicSum / msFeatureCount;

				massMonoisotopicList.Sort();
				if (msFeatureCount % 2 == 1)
				{
					lcmsFeature.MassMonoisotopicMedian = massMonoisotopicList[msFeatureCount / 2];
				}
				else
				{
					lcmsFeature.MassMonoisotopicMedian = 0.5 * (massMonoisotopicList[msFeatureCount / 2 - 1] + massMonoisotopicList[msFeatureCount / 2]);
				}

				lcmsFeatureIndex++;
			}

			return lcmsFeatureList;
		}

		/// <summary>
		/// Splits a List of LC-MS Features based on the LC-dimension gap size
		/// </summary>
		/// <param name="lcmsFeatureList">List of LC-MS Features to be split</param>
		/// <returns>New List of LC-MS Features</returns>
		public List<LCMSFeature> SplitLCMSFeaturesByGapSize(List<LCMSFeature> lcmsFeatureList)
		{
			List<LCMSFeature> newLCMSFeatureList = new List<LCMSFeature>();

			for (int i = 0; i < lcmsFeatureList.Count; i++)
			{
				LCMSFeature lcmsFeature = lcmsFeatureList[i];

				// Grab the MSFeatures associated with this LCMSFeature and sort them by Frame (LC Scan)
				List<MSFeature> msFeatureList = lcmsFeature.MSFeatureList;
				msFeatureList.Sort(new Comparison<MSFeature>(Feature.ScanLCComparison));

				List<MSFeatureGroup> msFeatureGroupList = new List<MSFeatureGroup>();

				// Group the MSFeatures together based on identical Frame (LC Scan) and add the MSFeatureGroup to the msFeatureGroupList
				int j = 0;
				while (j < msFeatureList.Count)
				{
					MSFeature currentMSFeature = msFeatureList[j];

					List<MSFeature> msFeatureListForFrame = new List<MSFeature>();
					msFeatureListForFrame.Add(currentMSFeature);

					int currentFrame = currentMSFeature.ScanLC;

					/// Iterate through the rest of the MSFeatures
					/// If the frame of the MSFeature is the same as the frame of the current frame, add it to the list.
					/// If the frame is not the same, stop iterating
					for (int k = j + 1; k < msFeatureList.Count; k++)
					{
						MSFeature nextMSFeature = msFeatureList[k];
						if (nextMSFeature.ScanLC == currentFrame)
						{
							msFeatureListForFrame.Add(nextMSFeature);
						}
						else
						{
							break;
						}
					}

					msFeatureGroupList.Add(new MSFeatureGroup(msFeatureListForFrame));

					// Update the iteration counter to skip through all of the MSFeatures that were added to the MSFeatureGroup
					j += msFeatureListForFrame.Count;
				}

				// Iterate through the MSFeatureGroups and split if necessary
				for (j = 0; j < msFeatureGroupList.Count - 1; j++)
				{
					MSFeatureGroup currentMSFeatureGroup = msFeatureGroupList[j];
					MSFeatureGroup nextMSFeatureGroup = msFeatureGroupList[j + 1];

					// If the gap size is too large
					if ((nextMSFeatureGroup.ScanLC - currentMSFeatureGroup.ScanLC - 1) > m_settings.LCGapSizeMax)
					{
						// Check if the LCMSFeature should be split
						if (DecideToSplit(lcmsFeature, nextMSFeatureGroup.ScanLC))
						{
							LCMSFeature newLCMSFeature = SplitLCMSFeature(ref lcmsFeature, nextMSFeatureGroup.ScanLC);
							newLCMSFeatureList.Add(newLCMSFeature);
						}
					}
				}

				// Build up a new list of LCMSFeatures
				newLCMSFeatureList.Add(lcmsFeature);
			}

			return newLCMSFeatureList;
		}

		/*
		private void CreateLCMSFeature(MSFeature currentMSFeature, LCMSFeature currentLCMSFeature, ref List<MSFeature> msFeatureList, ref List<LCMSFeature> lcmsFeatureList)
		{
			int msFeatureIndex = msFeatureList.Count - 2;
			MSFeature matchedMSFeature = null;
			double massTolerance = m_settings.MassMonoisotopicConstraint;

			// Convert from ppm to Da tolerance if necessary
			if (m_settings.MassMonoisotopicConstraintIsPPM)
			{
				massTolerance *= currentMSFeature.MassMonoisotopic / 1000000;
			}

			massTolerance += currentMSFeature.MassMonoisotopic;

			while (msFeatureIndex >= 0)
			{
				matchedMSFeature = msFeatureList[msFeatureIndex];

				// If we get past the Mass Tolerance for this LCMSFeature, then exit.
				//TODO: I feel like this should be >, but the old code has it as >=
				if (matchedMSFeature.MassMonoisotopic >= massTolerance)
				{
					return;
				}

				// Only move on if the Charge States are equal. If they are not equal, we can ignore the MSFeature.
				if (!m_useCharge || matchedMSFeature.Charge == currentMSFeature.Charge)
				{
					LCMSFeature matchedLCMSFeature = matchedMSFeature.LCMSFeature;

					// If the Matched MSFeature does not belong to an LCMSFeature, then add it to the LCMSFeature.
					if (matchedLCMSFeature == null)
					{
						// IF within max distance
						//TODO: I think this should be <=, but the old code has it as <
						if (CalculateDistance(currentMSFeature, matchedMSFeature) < m_settings.DistanceMax)
						{
							AddMSFeatureToLCMSFeature(matchedMSFeature, currentLCMSFeature);
						}
					}
					// If the MSFeatures belong to different LCMSFeatures, then merge the LCMSFeatures.
					else if (!matchedLCMSFeature.Equals(currentLCMSFeature))
					{
						// IF within max distance
						//TODO: I think this should be <=, but the old code has it as <
						if (CalculateDistance(currentMSFeature, matchedMSFeature) < m_settings.DistanceMax)
						{
							MergeLCMSFeatures(currentLCMSFeature, matchedLCMSFeature, ref lcmsFeatureList);
						}
					}
					// If the MSFeatures already belong to the same LCMSFeature, do nothing.
				}

				msFeatureIndex--;
			}
		}
		*/

		/*
		private double CalculateDistance(MSFeature msFeature1, MSFeature msFeature2)
		{
			if (m_settings.MassMonoisotopicConstraintIsPPM)
			{
				if (Math.Abs((msFeature1.MassMonoisotopic - msFeature2.MassMonoisotopic) * m_settings.MassMonoisotopicWeight / msFeature1.MassMonoisotopic * 1000000) > m_settings.MassMonoisotopicConstraint)
				{
					return Double.MaxValue;
				}
			}
			else
			{
				if (Math.Abs(msFeature1.MassMonoisotopic - msFeature2.MassMonoisotopic) > m_settings.MassMonoisotopicConstraint)
				{
					return Double.MaxValue;
				}
			}

			if (m_settings.MassAverageConstraintIsPPM)
			{
				if (Math.Abs((msFeature1.MassAverage - msFeature2.MassAverage) * m_settings.MassAverageWeight / msFeature1.MassAverage * 1000000) > m_settings.MassAverageConstraint)
				{
					return Double.MaxValue;
				}
			}
			else
			{
				if (Math.Abs(msFeature1.MassAverage - msFeature2.MassAverage) > m_settings.MassAverageConstraint)
				{
					return Double.MaxValue;
				}
			}

			float squareRootDistance = 0;
			squareRootDistance += (float)Math.Pow((msFeature1.MassMonoisotopic - msFeature2.MassMonoisotopic), 2) * (float)Math.Pow(m_settings.MassMonoisotopicWeight, 2);
			squareRootDistance += (float)Math.Pow((msFeature1.MassAverage - msFeature2.MassAverage), 2) * (float)Math.Pow(m_settings.MassAverageWeight, 2);
			squareRootDistance += (float)Math.Pow((Math.Log10(msFeature1.Abundance) - Math.Log10(msFeature2.Abundance)), 2) * (float)Math.Pow(m_settings.LogAbundanceWeight, 2);
			if (m_settings.UseGenericNET)
			{
				double netDistance = (double)((int)msFeature1.ScanLCAdjusted - (int)msFeature2.ScanLCAdjusted) / (double)(m_settings.FrameMax - m_settings.FrameMin);
				squareRootDistance += (float)Math.Pow(netDistance, 2) * (float)Math.Pow(m_settings.NetWeight, 2);
			}
			else
			{
				squareRootDistance += (float)Math.Pow(((int)msFeature1.ScanLCAdjusted - (int)msFeature2.ScanLCAdjusted), 2) * (float)Math.Pow(m_settings.ScanWeight, 2);
			}
			squareRootDistance += (float)Math.Pow((msFeature1.Fit - msFeature2.Fit), 2) * (float)Math.Pow(m_settings.FitWeight, 2);
			squareRootDistance += (float)Math.Pow((msFeature1.DriftTimeIMS - msFeature2.DriftTimeIMS), 2) * (float)Math.Pow(m_settings.DriftTimeIMSWeight, 2);
			return (float)Math.Sqrt(squareRootDistance);
		}
		  
		private void MergeLCMSFeatures(LCMSFeature msFeatureToKeep, LCMSFeature lcmsFeatureToMerge, ref List<LCMSFeature> lcmsFeatureList)
		{
			// Loop through each MSFeature in the LCMSFeature and add each of those MSFeatures to the Current LCMSFeature.
			foreach (MSFeature msFeature in lcmsFeatureToMerge.MSFeatureList)
			{
				AddMSFeatureToLCMSFeature(msFeature, msFeatureToKeep);
			}

			// Remove the merged LCMSFeature from the list of LCMSFeaturess
			lcmsFeatureList.Remove(lcmsFeatureToMerge);
		} 
		*/

		/// <summary>
		/// Adds an MS Feature to an LC-MS Feature
		/// </summary>
		/// <param name="msFeature">MS Feature to be added</param>
		/// <param name="lcmsFeature">LC-MS Feature to be added into</param>
		private void AddMSFeatureToLCMSFeature(MSFeature msFeature, LCMSFeature lcmsFeature)
		{
			msFeature.UMC = lcmsFeature;
			lcmsFeature.AddMSFeature(msFeature);
		}

		/// <summary>
		/// Decides whether to split an LC-MS Feature or not.
		/// </summary>
		/// <param name="lcmsFeature">LC-MS Feature</param>
		/// <param name="scanLC">The LC Scan to be used as the splitting point</param>
		/// <returns>True if it should be split, false otherwise</returns>
		private bool DecideToSplit(LCMSFeature lcmsFeature, int scanLC)
		{
			List<double> massMonoisotopicLeftList = new List<double>();
			List<double> massMonoisotopicRightList = new List<double>();

			double massMonoisotopicLeftMedian = 0;
			double massMonoisotopicRightMedian = 0;

			// Group the MSFeatures of each side of the LCMSFeature into 2 groups, 1 group for each side of the possible split
			foreach (MSFeature msFeature in lcmsFeature.MSFeatureList)
			{
				if (msFeature.ScanLC < scanLC)
				{
					massMonoisotopicLeftList.Add(msFeature.MassMonoisotopic);
				}
				else
				{
					massMonoisotopicRightList.Add(msFeature.MassMonoisotopic);
				}
			}

			// If either side of the LCMSFeature will be too short, do not split
			if (massMonoisotopicLeftList.Count < m_settings.FeatureLengthMin || massMonoisotopicRightList.Count < m_settings.FeatureLengthMin)
			{
				return false;
			}

			massMonoisotopicLeftList.Sort();
			massMonoisotopicRightList.Sort();

			// Calculate Median Mass of each side of the possible split
			if (massMonoisotopicLeftList.Count % 2 == 1)
			{
				massMonoisotopicLeftMedian = massMonoisotopicLeftList[massMonoisotopicLeftList.Count / 2];
			}
			else
			{
				massMonoisotopicLeftMedian = 0.5 * (massMonoisotopicLeftList[massMonoisotopicLeftList.Count / 2 - 1] + massMonoisotopicLeftList[massMonoisotopicLeftList.Count / 2]);
			}

			if (massMonoisotopicRightList.Count % 2 == 1)
			{
				massMonoisotopicRightMedian = massMonoisotopicRightList[massMonoisotopicRightList.Count / 2];
			}
			else
			{
				massMonoisotopicRightMedian = 0.5 * (massMonoisotopicRightList[massMonoisotopicRightList.Count / 2 - 1] + massMonoisotopicRightList[massMonoisotopicRightList.Count / 2]);
			}

			// Calculate the mass tolerance that the Median Mass of the Right side of the split must be within
			double massTolerance = m_settings.MinimumDifferenceInMedianPpmMassToSplit;
			massTolerance *= massMonoisotopicLeftMedian / 1000000;
			double massToleranceHigh = massMonoisotopicLeftMedian + massTolerance;
			double massToleranceLow = massMonoisotopicLeftMedian - massTolerance;

			if (massMonoisotopicRightMedian < massToleranceLow || massMonoisotopicRightMedian > massToleranceHigh)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Does the actual splitting of the LC-MS Feature
		/// </summary>
		/// <param name="lcmsFeature">LC-MS Feature to be split</param>
		/// <param name="firstScanLCToKeep">The LC Scan used as the splitting point</param>
		/// <returns>The newly created LC-MS Feature (the part that was split off)</returns>
		private LCMSFeature SplitLCMSFeature(ref LCMSFeature lcmsFeature, int firstScanLCToKeep)
		{
			LCMSFeature newLCMSFeature = new LCMSFeature(0);
			List<MSFeature> msFeaturesToKeep = new List<MSFeature>();
			List<MSFeature> msFeaturesToSplit = new List<MSFeature>();

			// Keep track of 2 groups of MSFeatures: MSFeatures to split and MSFeatures to not split
			foreach (MSFeature msFeature in lcmsFeature.MSFeatureList)
			{
				// If the MSFeature should be split, associate the newly created LCMSFeature with the MSFeature
				if (msFeature.ScanLC < firstScanLCToKeep)
				{
					msFeaturesToSplit.Add(msFeature);
					msFeature.UMC = newLCMSFeature;
				}
				else
				{
					msFeaturesToKeep.Add(msFeature);
				}
			}

			// Associate the 2 lists of MSFeatures with the appropriate LCMSFeature
			lcmsFeature.MSFeatureList = msFeaturesToKeep;
			newLCMSFeature.MSFeatureList = msFeaturesToSplit;

			return newLCMSFeature;
		}
	}
}