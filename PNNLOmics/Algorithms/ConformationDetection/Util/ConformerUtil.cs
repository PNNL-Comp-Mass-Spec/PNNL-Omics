using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UIMFLibrary;
using PNNLOmics.Algorithms.FeatureFinding.Control;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.ConformationDetection.Data;
using PNNLOmics.Algorithms.FeatureFinding.Data;

namespace PNNLOmics.Algorithms.ConformationDetection.Util
{
	public class ConformerUtil
	{
		#region Members
		private bool m_reportFittedTime;
		private double m_smoothingStDev;
		private int m_peakWidthMinimum;

		private DataReader m_uimfReader;
		private GlobalParameters m_globalParameters;

		private const double FRAME_PRESSURE_STANDARD = 4.0;
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="settings">Setting object containing parameters to use for conformation detection.</param>
		public ConformerUtil(Settings settings, DataReader uimfReader)
		{
			m_reportFittedTime = settings.ReportFittedTime;
			m_smoothingStDev = settings.SmoothingStDev;
			m_peakWidthMinimum = settings.PeakWidthMinimum;
			m_uimfReader = uimfReader;
			m_globalParameters = m_uimfReader.GetGlobalParameters();
		}
		#endregion

		#region Public functions
		/// <summary>
		/// Method to create a list of drift times and fit scores for the given IMS-MS feature.
		/// </summary>
		/// <param name="imsmsFeature">The IMS-MS feature to detect conformations for.</param>
		/// <param name="uimfReader">A DataReader object connecting to the UIMF file containing the raw information for the IMS-MS feature.</param>
		/// <param name="numFramesToSum">The number of IMS frames that should be summed to obtain the intensities.</param>
		/// <returns>A List of Conformation objects that represent conformation detected in the drift profile.</returns>
		public List<Conformation> ComputeDriftTime(IMSMSFeature imsmsFeature, int numFramesToSum)
		{
			// Create local variables.
			List<double> driftTimeList = new List<double>();
			List<double> intensityList = new List<double>();

			List<Conformation> conformationList = new List<Conformation>();

			// Fetch the data.
			GetIMSProfileOfIMSMSFeature(imsmsFeature, numFramesToSum, driftTimeList, intensityList);

			// Filter out infinite drift times.
			int numPoints = intensityList.Count;
			/*for (int i = numPoints - 1; i >= 0; i--)
			{
				if (double.IsInfinity(driftTimeList[i]) || intensityList[i]==0)
				{
					driftTimeList.RemoveAt(i);
					intensityList.RemoveAt(i);
					i--;
				}
			}*/
			numPoints = intensityList.Count;
			if (numPoints < m_peakWidthMinimum)
			{
				Conformation conformation = new Conformation();
				conformation.DriftTime = float.NaN;
				conformation.FitScore = double.NaN;
				conformation.NumOfPoints = numPoints;
				conformationList.Add(conformation);
				return conformationList;
			}

			// Smooth the data.
			List<double> originalIntensityList = intensityList;
			intensityList = CurveFit.KDESmooth(driftTimeList, intensityList, m_smoothingStDev);

			// Write out the smoothed profile.
			//WriteIMSMSFeatureProfile(imsmsFeature, driftTimeList, intensityList, originalIntensityList);

			List<double> intensityLogList = new List<double>();
			for (int i = 0; i < intensityList.Count; i++)
			{
				intensityLogList.Add(Math.Log(intensityList[i] + 1));
			}

			List<double> intensityChangeList = new List<double>();
			for (int i = 0; i < intensityList.Count - 1; i++)
			{
				intensityChangeList.Add(intensityLogList[i] - intensityLogList[i + 1]);
			}
			intensityChangeList.Add(0);
			// Find the peak information.
			int indexOfIntensityMax = 0;
			double tempIntensityMax = MathUtil.Max(intensityList, out indexOfIntensityMax);
			int firstIndexOfIntensityMax = indexOfIntensityMax;
			double intensityMax = tempIntensityMax;

			// While we still have a peak...
			while (tempIntensityMax > intensityMax * 0.05)
			{
				// Find the ends of the profile.
				int leftPosition = indexOfIntensityMax;
				int rightPosition = indexOfIntensityMax;

				//while (leftPosition > 0 && intensityList[leftPosition - 1] <= intensityList[leftPosition] && intensityList[leftPosition - 1] > tempIntensityMax*0.01)
				//{
				//    leftPosition--;
				//}
				//while (rightPosition < numPoints - 1 && intensityList[rightPosition + 1] <= intensityList[rightPosition] && intensityList[rightPosition + 1] > tempIntensityMax * 0.01)
				//{
				//    rightPosition++;
				//}

				//identify the linear part around the peak
				while (leftPosition > 0)
				{
					if (intensityChangeList[leftPosition - 1] - intensityChangeList[leftPosition] > 0)
					{
						break;
					}
					else
					{
						leftPosition--;
					}
				}
				while (rightPosition < numPoints - 1)
				{
					if (intensityChangeList[rightPosition] - intensityChangeList[rightPosition + 1] > 0)
					{
						break;
					}
					else
					{
						rightPosition++;
					}
				}

				// Create variables to store profile info.
				List<double> driftValueList = new List<double>();
				List<double> trainedValueList = new List<double>();
				List<double> observedValueList = new List<double>();

				// Pull out the selected points.
				int nSelectedPoints = rightPosition - leftPosition + 1;
				List<double> selectedDriftTime = driftTimeList.GetRange(leftPosition, nSelectedPoints);
				List<double> selectedIntensity = intensityList.GetRange(leftPosition, nSelectedPoints);

				// Train the Gaussian model.
				double[] curveParameters = CurveFit.GaussianFit(selectedDriftTime, selectedIntensity, 0);
				if (curveParameters == null)
				{
					break;
				}
				if (curveParameters[1] > 0 && curveParameters[2] > 0)
				{
					// Update the intensity values by "removing" the peak.
					for (int i = 0; i < numPoints; i++)
					{
						// Estimate the value of the Gaussian curve.
						double tempTrainedValue = curveParameters[2] * Math.Exp(-Math.Pow(driftTimeList[i] - curveParameters[0], 2) / (2 * Math.Pow(curveParameters[1], 2)));
						double tempObservedValue = intensityList[i];

						// Store the estimated and observed values.
						if ((i >= leftPosition) && (i <= rightPosition))
						{
							driftValueList.Add(driftTimeList[i]);
							trainedValueList.Add(tempTrainedValue);
							observedValueList.Add(tempObservedValue);
						}

						// Subtract the estimated from the observed and make all small values 0.
						intensityList[i] = tempObservedValue - tempTrainedValue;
						if (Math.Floor(intensityList[i]) < tempIntensityMax * 0.01)
						{
							intensityList[i] = 1;
						}
					}
				}
				else
				{
					break;
				}

				// Check that the peak is the appropriate width and of a minimal number of points before adding to list.
				//double fwhm = 2.35482 * curveParameters[1];
				//if (fwhm >= 0.25 && fwhm <= 1.2 && rightPosition - leftPosition + 1 >= m_peakWidthMinimum)
				//{

				Conformation conformation = new Conformation();
				conformation.NumOfPoints = nSelectedPoints;

				// Calculate the MSE (fit score) for the data.
				double fitScore = MathUtil.PearsonCorr(trainedValueList, observedValueList);
				conformation.FitScore = fitScore;

				// Option for reported drift time.
				if (m_reportFittedTime)
				{
					conformation.DriftTime = (float)curveParameters[0];
				}
				else
				{
					conformation.DriftTime = (float)driftTimeList[indexOfIntensityMax];
				}

				conformationList.Add(conformation);
				// Write out the predicted and observed values.
				//WriteIMSMSFeaturePredictedProfile(imsmsFeature, driftValueList, trainedValueList, observedValueList);
			}

			// Find the next peak.
			tempIntensityMax = MathUtil.Max(intensityList, out indexOfIntensityMax);
			for (int i = 0; i < intensityList.Count; i++)
			{
				intensityLogList[i] = Math.Log(intensityList[i]);
			}
			for (int i = 0; i < intensityLogList.Count - 1; i++)
			{
				intensityChangeList[i] = intensityLogList[i] - intensityLogList[i + 1];
			}

			// If no valid peaks are found, output the drift time of the maximum intensity.
			if (conformationList.Count < 1)
			{
				Conformation conformation = new Conformation();
				conformation.DriftTime = (float)driftTimeList[firstIndexOfIntensityMax];
				conformation.FitScore = 0;
				conformationList.Add(conformation);
			}

			return conformationList;
		}
		#endregion

		#region Private functions
		private void GetTOFProfileForMZRange(int frameNum, int startScan, int endScan, List<double> startMZ, List<double> endMZ, int numFramesToSum, List<double> driftTimeList, List<double> intensityList, FrameParameters frameParameters)
		{
			int timeOffset = m_globalParameters.TimeOffset;
			double binWidth = m_globalParameters.BinWidth;
			double calibrationSlope = frameParameters.CalibrationSlope;
			double calibrationIntercept = frameParameters.CalibrationIntercept;
			double averageTOFLength = frameParameters.AverageTOFLength;
			double framePressure = frameParameters.PressureBack;
			driftTimeList.Clear();
			intensityList.Clear();

			double minMZ = startMZ[0];
			double maxMZ = endMZ[endMZ.Count - 1];

			int globalStartBin = (int)((Math.Sqrt(minMZ) / calibrationSlope + calibrationIntercept) * 1000 / binWidth);
			int globalEndBin = (int)Math.Ceiling(((Math.Sqrt(maxMZ) / calibrationSlope + calibrationIntercept) * 1000 / binWidth));

			int[][] allIntensities = m_uimfReader.GetIntensityBlock(frameNum, 0, startScan, endScan, globalStartBin, globalEndBin);

			// Save the m/z vs. intensity data
			for (int i = 0; i <= (endScan - startScan); i++)
			{
				int scanNum = i + startScan;
				double driftTime = -1.0;
				if (framePressure != 0)
				{
					driftTime = (averageTOFLength * (scanNum + 1) / 1e6) * (FRAME_PRESSURE_STANDARD / framePressure);
				}
				else
				{
					driftTime = (averageTOFLength * (scanNum + 1) / 1e6);
				}
				driftTimeList.Add(driftTime);

				double intensitySumMZRange = 0.0;
				for (int j = 0; j < startMZ.Count; j++)
				{
					int adjustedStartBin = (int)((Math.Sqrt(startMZ[j]) / calibrationSlope + calibrationIntercept) * 1000 / binWidth) - globalStartBin;
					int adjustedEndBin = (int)Math.Ceiling(((Math.Sqrt(endMZ[j]) / calibrationSlope + calibrationIntercept) * 1000 / binWidth) - globalStartBin);

					for (int k = adjustedStartBin; k <= adjustedEndBin; k++)
					{
						intensitySumMZRange += allIntensities[i][k];
					}
				}
				intensityList.Add(intensitySumMZRange);
			}
		}
		/// <summary>
		/// Get the chromatogram for a given MSFeature in ISO file.
		/// </summary>
		private void GetMSProfileOfMSFeature(MSFeature msFeature)
		{
			List<double> mzList = new List<double>();
			List<double> intensityList = new List<double>();

			double currentMonoMZ = msFeature.MZ;

			List<double> startMZ = new List<double>();
			List<double> endMZ = new List<double>();

			double charge = System.Convert.ToDouble(msFeature.ChargeState);
			for (int i = 0; i < 3; i++)
			{
				startMZ.Add(currentMonoMZ + i / charge - msFeature.Fwhm);
				endMZ.Add(currentMonoMZ + i / charge + msFeature.Fwhm);
			}

			//uimfReader.GetSegment((int)currentMSFeature.ScanLC, (int)currentMSFeature.ScanIMS, (int)currentMSFeature.ScanIMS, startMZ, endMZ, driftTimeList, intensityList);

			//uimfReader.GetSegment((int)msFeature.ScanLC, (int)msFeature.ScanIMS, (int)msFeature.ScanIMS, startMZ, endMZ, "tmp.txt");
		}
		/// <summary>
		/// Gets the drift profile for the IMS-MS feature.
		/// </summary>
		/// <param name="imsmsFeature">The IMS-MS feature to detect conformations for.</param>
		/// <param name="uimfReader">A DataReader object connecting to the UIMF file containing the raw information for the IMS-MS feature.</param>
		/// <param name="numFramesToSum">The number of IMS frames that should be summed to obtain the intensities.</param>
		/// <param name="driftTimeList">A List to be filled with drift times.</param>
		/// <param name="intensityList">A List to be filled with intensities.</param>
		private void GetIMSProfileOfIMSMSFeature(IMSMSFeature imsmsFeature, int numFramesToSum, List<double> driftTimeList, List<double> intensityList)
		{
			// Set local parameters.
			int currentScanLC = (int)ScanLCMapHolder.ScanLCMap[imsmsFeature.ScanLC];
			MSFeature maxMSFeature = new MSFeature();
			double maxAbundance = imsmsFeature.AbundanceMaximum;
			foreach (MSFeature msFeature in imsmsFeature.MSFeatureList)
			{
				if (msFeature.Abundance == maxAbundance)
				{
					maxMSFeature = msFeature;
				}
			}
			double currentFWHM = maxMSFeature.Fwhm;
			double currentMonoMZ = maxMSFeature.MassMostAbundant / maxMSFeature.ChargeState + 1.00727649;

			FrameParameters frameParameters = m_uimfReader.GetFrameParameters(currentScanLC);
			int maxNumberOfIMSScans = frameParameters.Scans;

			int scanStartProfile = (int)imsmsFeature.ScanIMSStart;
			int scanEndProfile = (int)imsmsFeature.ScanIMSEnd;
			List<double> startMZ = new List<double>();
			List<double> endMZ = new List<double>();

			// Set ranges over which to look for the original data in the UIMF.
			double charge = Convert.ToDouble(imsmsFeature.ChargeState);
			for (int i = 0; i < 3; i++)
			{
				startMZ.Add(currentMonoMZ + (1.003 * i / charge) - (0.5 * currentFWHM));
				endMZ.Add(currentMonoMZ + (1.003 * i / charge) + (0.5 * currentFWHM));
				//startMZ.Add(currentMonoMZ + (1.003 * i / charge) - (currentMonoMZ * 25 / 1e6));
				//endMZ.Add(currentMonoMZ + (1.003 * i / charge) + (currentMonoMZ * 25 / 1e6));
			}

			// Grab the raw data plus a few extra points on each side.
			int numExtraIMSScans = 0;
			int scanIMSStartWideProfile = Math.Max(scanStartProfile - numExtraIMSScans, 0);
			int scanIMSEndWideProfile = Math.Min(scanEndProfile + numExtraIMSScans, maxNumberOfIMSScans);

			GetTOFProfileForMZRange(currentScanLC, scanIMSStartWideProfile, scanIMSEndWideProfile, startMZ, endMZ, numFramesToSum, driftTimeList, intensityList, frameParameters);
		}
		#region Debug file writers
		/// <summary>
		/// Write the given drift profile for the IMS-MSFeature to a text file.
		/// </summary>
		/// <param name="imsmsFeature">The IMS-MS feature the drift profile is associated with.</param>
		/// <param name="driftTimeList">List of drift times.</param>
		/// <param name="intensityList">List of intensities corresponding to the drift time.</param>
		private void WriteIMSMSFeatureProfile(IMSMSFeature imsmsFeature, List<double> driftTimeList, List<double> intensityList, List<double> originalIntensityList)
		{
			TextWriter imsProfileWriter = new StreamWriter("IMSProfile-LC_" + imsmsFeature.ScanLC + "_MonoMass_" + imsmsFeature.MassMonoisotopicMedian + "_CS_" + imsmsFeature.ChargeState + "_Smoothed.txt");
			for (int i = 0; i < driftTimeList.Count; i++)
			{
				imsProfileWriter.Write(driftTimeList[i]);
				imsProfileWriter.Write("," + originalIntensityList[i]);
				imsProfileWriter.Write("," + intensityList[i] + "\n");
			}
			imsProfileWriter.Close();
		}
		/// <summary>
		/// Write the given drift profile for the IMS-MSFeature to a text file.
		/// </summary>
		/// <param name="imsmsFeature">The IMS-MS feature the drift profile is associated with.</param>
		/// <param name="driftTimeList">List of drift times.</param>
		/// <param name="predictedIntensityList">List of predicted intensities corresponding to the drift times.</param>
		/// <param name="observedIntensityList">List of observed intensities corresponding to the drift times.</param>
		private void WriteIMSMSFeaturePredictedProfile(IMSMSFeature imsmsFeature, List<double> driftTimeList, List<double> predictedIntensityList, List<double> observedIntensityList)
		{
			TextWriter imsProfileWriter = new StreamWriter("IMSProfile-LC_" + imsmsFeature.ScanLC + "_MonoMass_" + imsmsFeature.MassMonoisotopicMedian + "_CS_" + imsmsFeature.ChargeState + "_DriftTime_" + imsmsFeature.DriftTime + "_Profile" + ".txt");
			for (int i = 0; i < predictedIntensityList.Count; i++)
			{
				imsProfileWriter.Write(driftTimeList[i]);
				imsProfileWriter.Write("," + predictedIntensityList[i]);
				imsProfileWriter.Write("," + observedIntensityList[i] + "\n");
			}
			imsProfileWriter.WriteLine();
			imsProfileWriter.Close();
		}
		#endregion
		#endregion
	}
}
