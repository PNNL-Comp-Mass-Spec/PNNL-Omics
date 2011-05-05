using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureMatcher.Data;
using PNNLOmics.Utilities;

namespace PNNLOmics.Algorithms.FeatureMatcher
{
    public class FeatureMatcher<T, U>
        where T : Feature, new()
        where U : Feature, new()
    {
        #region Members
        
        private FeatureMatcherParameters m_matchParameters;

        private List<FeatureMatch<T, U>> m_matchList;
        private List<FeatureMatch<T, U>> m_shiftedMatchList;
        private List<T> m_observedFeatureList;
        private List<U> m_targetFeatureList;

        private double m_shiftFDR;
        private double m_shiftConservativeFDR;
        private double m_errorHistogramFDR;

        private List<STACInformation> m_stacParametersList;
        private List<STACFDR> m_stacFDRList;
        private List<FeatureMatcherTolerances> m_refinedTolerancesList;
        private SLiCInformation m_slicParameters;

        const int MIN_MATCHES_FOR_NORMAL_ASSUMPTION = 50;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the initial parameters used for matching.
        /// </summary>
        public FeatureMatcherParameters MatchParameters
        {
            get { return m_matchParameters; }
            set { m_matchParameters = value; }
        }
        /// <summary>
        /// Gets the list of feature matches.
        /// </summary>
        public List<FeatureMatch<T, U>> MatchList
        {
            get { return m_matchList; }
        }
        /// <summary>
        /// Gets the list of features matched with a shift.
        /// </summary>
        public List<FeatureMatch<T, U>> ShiftedMatchList
        {
            get { return m_shiftedMatchList; }
        }

        /// <summary>
        /// Gets the FDR calculated by using a fixed shift.  Calculated as (# shifted matches)/(# shifted matches + # non-shifted matches).
        /// </summary>
        public double ShiftFDR
        {
            get { return m_shiftFDR; }
        }
        /// <summary>
        /// Gets the FDR calculated by using a fixed shift.  Calculated as 2*(# shifted matches)/(# shifted matches + # non-shifted matches).
        /// </summary>
        public double ShiftConservativeFDR
        {
            get { return m_shiftConservativeFDR; }
        }
        /// <summary>
        /// Gets the FDR calculated by using a mass error histogram.
        /// </summary>
        public double ErrorHistogramFDR
        {
            get { return m_errorHistogramFDR; }
        }

        /// <summary>
        /// Gets the list of parameters trained by STAC.  Each entry is a different charge state.
        /// </summary>
        public List<STACInformation> STACParameterList
        {
            get { return m_stacParametersList; }
        }
        /// <summary>
        /// Get the STAC FDR table.
        /// </summary>
        public List<STACFDR> STACFDRTable
        {
            get { return m_stacFDRList; }
        }
        /// <summary>
        /// Gets the list of refined tolerances used for SLiC and shift.  Each entry is a different charge state.
        /// </summary>
        public List<FeatureMatcherTolerances> RefinedToleranceList
        {
            get { return m_refinedTolerancesList; }
        }
        /// <summary>
        /// Gets the parameters used in calculating SLiC.
        /// </summary>
        public SLiCInformation SLiCParameters
        {
            get { return m_slicParameters; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor for use in matching features.  Uses passed parameters to calculate desired results.
        /// </summary>
        /// <param name="observedFeatureList">List of features observed in an analysis.  Generally UMC or UMCCluster.</param>
        /// <param name="targetFeatureList">List of features to be matched to.  Generally AMTTags.</param>
        /// <param name="matchParameters">FeatureMatcherParameters object containing initial parameters and algorithm settings.</param>
        public FeatureMatcher(List<T> observedFeatureList, List<U> targetFeatureList, FeatureMatcherParameters matchParameters)
        {
            Clear();

            m_observedFeatureList = observedFeatureList;
            m_targetFeatureList = targetFeatureList;
            m_matchParameters = matchParameters;            
        }
        #endregion

        #region Private functions
        /// <summary>
        /// Reset all algorithm results to default values.
        /// </summary>
        private void Clear()
        {
            m_stacFDRList = new List<STACFDR>();
            m_matchParameters = new FeatureMatcherParameters();
            m_shiftFDR = 0;
            m_shiftConservativeFDR = 0;
            m_errorHistogramFDR = 0;
            m_stacParametersList = new List<STACInformation>();
            m_refinedTolerancesList = new List<FeatureMatcherTolerances>();
            m_slicParameters = new SLiCInformation();
            m_matchList = new List<FeatureMatch<T, U>>();
            m_shiftedMatchList = new List<FeatureMatch<T, U>>();
        }
        /// <summary>
        /// Set the list of charge states to be used in IMS calculations.
        /// </summary>
        private void SetChargeStateList()
        {
            m_matchParameters.ChargeStateList.Clear();
            foreach (T observedFeature in m_observedFeatureList)
            {
                if (!m_matchParameters.ChargeStateList.Contains(observedFeature.ChargeState))
                {
                    m_matchParameters.ChargeStateList.Add(observedFeature.ChargeState);
                }
            }
        }
        /// <summary>
        /// Find a list of matches between two lists.
        /// </summary>
        /// <param name="shortObservedList">List of observed features.  Possibly a subset of the entire list corresponding to a particular charge state.</param>
        /// <param name="shortTargetList">List of target features.  Possibly a subset of the entire list corresponding to a particular charge state.</param>
        /// <param name="tolerances">Tolerances to be used for matching.</param>
        /// <param name="shiftAmount">A fixed shift amount to use for populating the shifted match list.</param>
        /// <returns>A list of type FeatureMatch containing matches within the defined region.</returns>
        public List<FeatureMatch<T, U>> FindMatches(List<T> shortObservedList, List<U> shortTargetList, FeatureMatcherTolerances tolerances, double shiftAmount)
        {
            // Create a list to hold the matches until they are returned.
            List<FeatureMatch<T, U>> matchList = new List<FeatureMatch<T, U>>();

            // Set indices to use when iterating over the lists.
            int observedIndex = 0;
            int targetIndex = 0;
            int lowerBound = 0;


            // Sort both lists by mass.
			if (shortObservedList[0].MassMonoisotopicAligned != double.NaN && shortObservedList[0].MassMonoisotopicAligned > 0.0)
            {
                shortObservedList.Sort(new Comparison<T>(Feature.MassAlignedComparison));
            }
            else
            {
                shortObservedList.Sort(new Comparison<T>(Feature.MassComparison));
            }
			if (shortTargetList[0].MassMonoisotopicAligned != double.NaN && shortTargetList[0].MassMonoisotopicAligned > 0.0)
			{
				shortTargetList.Sort(new Comparison<U>(Feature.MassAlignedComparison));
			}
			else
			{
				shortTargetList.Sort(new Comparison<U>(Feature.MassComparison));
			}

            // Locally store the tolerances.
            double massTolerancePPM = tolerances.MassTolerancePPM;
            double netTolerance = tolerances.NETTolerance;
            float driftTimeTolerance = tolerances.DriftTimeTolerance;

            // Iterate through the list of observed features.
            while( observedIndex < shortObservedList.Count )
            {
                if (observedIndex % 100 == 0) Console.WriteLine("Working on UMC index = " + observedIndex);
                // Store the current observed feature locally.
                T observedFeature = shortObservedList[observedIndex];
                // Flag variable that gets set to false when the observed mass is greater than the current mass tag by more than the tolerance.
                bool continueLoop = true;
                // Set the target feature iterator to the current lower bound.
                targetIndex = lowerBound;
                // Iterate through the list of target featrues or until the observed feature is too great.
                while( targetIndex < shortTargetList.Count && continueLoop )
                {
                    // Add any shift to the mass tag.
                    U targetFeature = shortTargetList[targetIndex];

                    // Check to see that the features are within the mass tolearance of one another.
					double massDifference = 0;
                    if (WithinMassTolerance(observedFeature, targetFeature, massTolerancePPM, shiftAmount, out massDifference))
                    {
                        bool withinTolerances = WithinNETTolerance(observedFeature, targetFeature, netTolerance);
                        if (m_matchParameters.UseDriftTime)
                        {
                            withinTolerances = withinTolerances & withinDriftTimeTolerance(observedFeature, targetFeature, driftTimeTolerance);
							withinTolerances = withinTolerances & (observedFeature.ChargeState == targetFeature.ChargeState);
                        }
                        // Create a temporary match between the two and check it against all tolerances before adding to the match list.
                        if (withinTolerances)
                        {
                            FeatureMatch<T, U> match = new FeatureMatch<T, U>();
                            match.AddFeatures(observedFeature, targetFeature, m_matchParameters.UseDriftTime, (shiftAmount > 0));
                            matchList.Add(match);
                        }
                    }
                    else
                    {
                        // Increase the lower bound if the the MassTag masses are too low or set the continueLoop flag to false if they are too high.
						if (massDifference < massTolerancePPM)
                        {
                            lowerBound++;
                        }
                        else
                        {
                            continueLoop = false;
                        }
                    }
                    // Increment the target index.
                    targetIndex++;
                }
                // Increment the observed index.
                observedIndex++;
            }
            // Return the list of matches.
            return matchList;
        }
        /// <summary>
        /// Refine tolerances for use in SLiC and shift match methods.
        /// </summary>
        /// <param name="matchList">List of matches used to train the refined tolerances.</param>
        /// <returns>Refined tolerances for use in SLiC and shift match methods.</returns>
        private FeatureMatcherTolerances FindOptimalTolerances(List<FeatureMatch<T, U>> matchList)
        {
            List<Matrix> differenceMatrixList = new List<Matrix>();
			foreach (FeatureMatch<T, U> featureMatch in matchList)
			{
				differenceMatrixList.Add(featureMatch.ReducedDifferenceVector);
			}

            int rows = differenceMatrixList[0].RowCount;
            Matrix meanVector = new Matrix(rows, 1, 0.0);
            Matrix covarianceMatrix = new Matrix(rows, 1.0);
            double mixtureParameter = 0.5;


            Utilities.ExpectationMaximization.NormalUniformMixture(differenceMatrixList, ref meanVector, ref covarianceMatrix, m_matchParameters.UserTolerances.AsVector(m_matchParameters.UseDriftTime), ref mixtureParameter, true);

            m_slicParameters.MassPPMStDev = Math.Sqrt(covarianceMatrix[0, 0]);
            m_slicParameters.NETStDev = Math.Sqrt(covarianceMatrix[1, 1]);
			if (m_matchParameters.UseDriftTime)
			{
				m_slicParameters.DriftTimeStDev = (float)Math.Sqrt(covarianceMatrix[2, 2]);
			}

            FeatureMatcherTolerances refinedTolerances = new FeatureMatcherTolerances((2.5 * m_slicParameters.MassPPMStDev), (2.5 * m_slicParameters.NETStDev),
                                                            (float)(2.5 * m_slicParameters.DriftTimeStDev));
            refinedTolerances.Refined = true;
            return refinedTolerances;
        }
        /// <summary>
        /// Populate the FDR table with default cutoffs.
        /// </summary>
        private void SetSTACCutoffs()
        {
            m_stacFDRList.Clear();
            for (double cutoff = 0.99; cutoff > 0.90; cutoff -= 0.01)
            {
                STACFDR tableLine = new STACFDR(cutoff);
                m_stacFDRList.Add(tableLine);
            }
            for (double cutoff = 0.90; cutoff >= 0; cutoff -= 0.10)
            {
                STACFDR tableLine = new STACFDR(cutoff);
                m_stacFDRList.Add(tableLine);
            }
        }
        /// <summary>
        /// Fills in the values for the STAC FDR table.
        /// </summary>
        private void PopulateSTACFDRTable()
        {
            SetSTACCutoffs();
            m_matchList.Sort(FeatureMatch<T,U>.STACComparisonDescending);

			// Iterate over all defined cutoff ranges
			for (int cutoffIndex = 0; cutoffIndex < m_stacFDRList.Count; cutoffIndex++)
			{
				double falseDiscoveries = 0.0;
				int matches = 0;
				double fdr = 0.0;

				// Find all matches for this particular cutoff
				foreach (FeatureMatch<T, U> match in m_matchList)
				{
					double stac = match.STACScore;
					if (stac >= m_stacFDRList[cutoffIndex].Cutoff)
					{
						falseDiscoveries += 1 - stac;
						matches++;
					}
					else
					{
						// Since we have sorted by descending STAC Score, if we get outside the cutoff range, we can stop
						break;
					}
				}

				// After all matches have been found, report the FDR
				if (matches > 0)
				{
					fdr = falseDiscoveries / (double)matches;
					m_stacFDRList[cutoffIndex].FillLine(fdr, matches, (int)falseDiscoveries);
				}
				else
				{
					m_stacFDRList[cutoffIndex].FillLine(0, 0, 0);
				}
			}
        }
        /// <summary>
        /// Find the subset of a list which has the same charge state.
        /// </summary>
        /// <typeparam name="T">Any class derived from Feature.</typeparam>
        /// <param name="featureList">Original list of features from which to extract the subset.</param>
        /// <param name="startIndex">Index at which to start search.  Set to the value of the last such item.</param>
        /// <param name="chargeState">The charge state to search for a subset of.</param>
        /// <returns>A list of features with the given charge state.</returns>
        private List<S> ExtractChargeStateList<S>(List<S> featureList, ref int startIndex, int chargeState) where S: Feature, new()
        {         
            return featureList.FindAll(delegate(S x) { return x.ChargeState == chargeState; });
        }
        /// <summary>
        /// Sets the false discovery rate by creating a histogram of the mass errors and comparing the proportion above a threshhold to the area below.
        /// </summary>
        private void SetMassErrorHistogramFDR()
        {
            // Populate the mass error list and create the histogram.
            List<XYData> histogramList = new List<XYData>();
            List<double> massErrorList = new List<double>();
            foreach (FeatureMatch<T, U> match in m_matchList)
            {
                massErrorList.Add(match.DifferenceVector[1, 1]);
            }
            histogramList = MathUtilities.GetHistogramValues(massErrorList, m_matchParameters.HistogramBinWidth);

            int peakIndex = 0;
            double peakValue = 0.0;
            double upperBound = 0.0;
            double lowerBound = 0.0;
            double threshold = 0.0;
            double meanValue = 0.0;
            // Find the maximum and average values.
            for( int i=0; i<histogramList.Count; i++ )
            {
                XYData bin = histogramList[i];
                if (bin.Y > peakValue)
                {
                    peakValue = bin.Y;
                    peakIndex = i;
                }
                meanValue += bin.Y;
            }
            meanValue /= histogramList.Count;
            // Set the threshold to a value between the peak and the average value.
            threshold = meanValue + m_matchParameters.HistogramMultiplier * (peakValue - meanValue);
            // Find the upper bound.
            for (int i = peakIndex; i < histogramList.Count; i++)
            {
                XYData bin = histogramList[i];
                XYData lowerBin = histogramList[i - 1];
                if (bin.Y < threshold && lowerBin.Y >= threshold)
                {
                    upperBound = lowerBin.X + (lowerBin.Y - threshold) / (lowerBin.Y - bin.Y) * (bin.X - lowerBin.X);
                }
            }
            // Find the lower bound.
            for (int i = peakIndex; i >= 0; i--)
            {
                XYData bin = histogramList[i];
                XYData upperBin = histogramList[i + 1];
                if (bin.Y < threshold && upperBin.Y >= threshold)
                {
                    lowerBound = bin.X + (threshold-bin.Y) / (upperBin.Y-bin.Y) * (upperBin.X - bin.X);
                }
            }
            // Count the number of matches within the bounds and calculate the FDR.
            int countInBounds = 0;
            foreach (double massDifference in massErrorList)
            {
                if (massDifference >= lowerBound && massDifference <= upperBound)
                {
                    countInBounds++;
                }
            }
            m_errorHistogramFDR = countInBounds/((upperBound-lowerBound)*threshold);
        }
        /// <summary>
        /// TODO:  Fill this in...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="observedFeature"></param>
        /// <param name="targetFeature"></param>
        /// <param name="massTolerancePPM"></param>
        /// <returns></returns>
        private bool WithinMassTolerance(T observedFeature, U targetFeature, double massTolerancePPM, double shiftAmount, out double difference)            
        {
            if (massTolerancePPM > 0)
            {
                double observedMass = 0; 
                double targetMass = 0;
				if (observedFeature.MassMonoisotopicAligned != double.NaN && observedFeature.MassMonoisotopicAligned > 0.0)
                {
					observedMass = observedFeature.MassMonoisotopicAligned;
                }
                else
                {
                    observedMass = observedFeature.MassMonoisotopic;
                }
				if (targetFeature.MassMonoisotopicAligned != double.NaN && targetFeature.MassMonoisotopicAligned > 0.0)
                {
					targetMass = targetFeature.MassMonoisotopicAligned;
                }
                else
                {
                    targetMass = targetFeature.MassMonoisotopic;
                }
				
				difference = (targetMass + shiftAmount - observedMass) / targetMass * 1E6;
                return (Math.Abs(difference)< massTolerancePPM);
            }
            else
            {
				difference = double.MaxValue;
                return false;
            }
        }
        /// <summary>
        /// TODO:  Fill this in...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="observedFeature"></param>
        /// <param name="targetFeature"></param>
        /// <param name="netTolerance"></param>
        /// <returns></returns>
        private bool WithinNETTolerance(T observedFeature, U targetFeature, double netTolerance)            
        {
            if (netTolerance > 0)
            {
                double targetNET = 0;
                double observedNET = 0;
				if (targetFeature.NETAligned != double.NaN && targetFeature.NETAligned > 0.0)
                {
					targetNET = targetFeature.NETAligned;
                }
                else
                {
                    targetNET = targetFeature.NET;
                }
                if (observedFeature.NETAligned != double.NaN && observedFeature.NETAligned > 0.0)
                {
					observedNET = observedFeature.NETAligned;
                }
                else
                {
                    observedNET = observedFeature.NET;
                }
                double difference = Math.Abs(targetNET - observedNET);
                return (difference < netTolerance);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// TODO:  Another summary...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="observedFeature"></param>
        /// <param name="targetFeature"></param>
        /// <param name="driftTimeTolerance"></param>
        /// <returns></returns>
        private bool withinDriftTimeTolerance(T observedFeature, U targetFeature, float driftTimeTolerance)
        {
            if (driftTimeTolerance > 0)
            {
				double targetDriftTime = 0;
				double observedDriftTime = 0;
				if (targetFeature.DriftTimeAligned != double.NaN && targetFeature.DriftTimeAligned > 0.0)
				{
					targetDriftTime = targetFeature.DriftTimeAligned;
				}
				else
				{
					targetDriftTime = targetFeature.DriftTime;
				}
				if (observedFeature.DriftTimeAligned != double.NaN && observedFeature.DriftTimeAligned > 0.0)
				{
					observedDriftTime = observedFeature.DriftTimeAligned;
				}
				else
				{
					observedDriftTime = observedFeature.DriftTime;
				}

                double difference = Math.Abs(targetDriftTime - observedDriftTime);
                return (difference < driftTimeTolerance);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// TODO:  You're gonna hate this...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observedFeature"></param>
        /// <param name="targetFeature"></param>
        /// <param name="driftTimeTolerance"></param>
        /// <returns></returns>
        private bool WithDriftTimeTolerance(T observedFeature, MassTag targetFeature, float driftTimeTolerance) 
        {
            if (driftTimeTolerance > 0)
            {
                float targetDriftTime = 0;
                float observedDriftTime = observedFeature.DriftTime;
                if (targetFeature.DriftTimePredicted > 0)
                {
                    targetDriftTime = (float)targetFeature.DriftTimePredicted;
                }
                else
                {
                    targetDriftTime = targetFeature.DriftTime;
                }
                float difference = Math.Abs(targetDriftTime - observedDriftTime);
                return (difference < driftTimeTolerance);
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Public functions
        /// <summary>
        /// Function to call to re-calculate algorithm results. [gord - following comment lies!] Called within constructor by default.
        /// </summary>
        public void MatchFeatures()
        {
			//if (m_matchParameters.UseDriftTime)
			//{
			//    // If no charge states were specified in the parameters, fill in the charge state list with all charge states contained in the observed feature list.
			//    if (m_matchParameters.ChargeStateList.Count == 0)
			//    {
			//        SetChargeStateList();
			//    }
			//    // Resize the STAC and refined tolerances list so that they are the same length as the charge state list.
			//    int chargeStateCount = m_matchParameters.ChargeStateList.Count;
			//    m_stacParametersList.Capacity = chargeStateCount;
			//    m_refinedTolerancesList.Capacity = chargeStateCount;
			//    // Sort the feature lists and create indices to store where a charge state ends.
			//    m_observedFeatureList.Sort(new Comparison<T>(Feature.ChargeStateComparison));
			//    int chargeStateIndexObserved = 0;
			//    m_targetFeatureList.Sort(new Comparison<U>(Feature.ChargeStateComparison));
			//    int chargeStateIndexTarget = 0;
			//    // Iterate through each charge state performing the desired operations.
			//    for (int i = 0; i < chargeStateCount; i++)
			//    {
			//        // Create sub-lists of the features with the current charge state.
			//        int chargeState = m_matchParameters.ChargeStateList[i];
			//        List<T> chargeObservedList = ExtractChargeStateList<T>(m_observedFeatureList, ref chargeStateIndexObserved, chargeState);
			//        List<U> chargeTargetList   = ExtractChargeStateList<U>(m_targetFeatureList, ref chargeStateIndexTarget, chargeState);

			//        // If either the observed or target feature lists are empty, then move on to the next charge state
			//        if (chargeObservedList.Count == 0 || chargeTargetList.Count == 0)
			//        {
			//            continue;
			//        }

			//        // Find the matches among the two feature lists.
			//        List<FeatureMatch<T, U>> tempMatchList = new List<FeatureMatch<T, U>>();
			//        tempMatchList = FindMatches(chargeObservedList, chargeTargetList, m_matchParameters.UserTolerances, false, 0);
			//        bool lengthCheck = (tempMatchList.Count < MIN_MATCHES_FOR_NORMAL_ASSUMPTION);

			//        // If not enough matches, move on to the next charge state
			//        if (lengthCheck)
			//        {
			//            continue;
			//        }

			//        // If the STAC score is requested, perform it.
			//        if (m_matchParameters.ShouldCalculateSTAC)
			//        {
			//            STACInformation stacInformation = new STACInformation(m_matchParameters.UseDriftTime);
			//            stacInformation.PerformSTAC(tempMatchList, m_matchParameters.UserTolerances, m_matchParameters.UseDriftTime, m_matchParameters.UsePriors);
			//            STACParameterList.Add(stacInformation);
			//        }
			//        // If 11 Dalton shift FDR is requested, calculate whether each of the temporary matches is within the bounds.
			//        if (m_matchParameters.ShouldCalculateShiftFDR)
			//        {
			//            m_refinedTolerancesList.Add(FindOptimalTolerances(tempMatchList));
			//            for (int j = 0; j < tempMatchList.Count; j++)
			//            {
			//                tempMatchList[j].InRegion(m_refinedTolerancesList[i], m_matchParameters.UseEllipsoid);
			//            }
			//            m_shiftedMatchList.AddRange(FindMatches(chargeObservedList, chargeTargetList, m_refinedTolerancesList[i],
			//                                                        m_matchParameters.UseEllipsoid, m_matchParameters.ShiftAmount));
			//        }
			//        // Add the temporary matches to the match list.
			//        m_matchList.AddRange(tempMatchList);
			//    }
			//    PopulateSTACFDRTable(); // Need to calculate STAC FDR in a smarter way than this.
			//    int count = 0;
			//    for (int j = 0; j < m_matchList.Count; j++)
			//    {
			//        if (m_matchList[j].WithinRefinedRegion)
			//        {
			//            count++;
			//        }
			//    }
			//    m_shiftFDR = (1.0 * count) / m_shiftedMatchList.Count;
			//    m_shiftConservativeFDR = (2.0 * count) / m_shiftedMatchList.Count;
			//}
			//else
			//{
                m_matchList.Clear();
                m_matchList = FindMatches(m_observedFeatureList, m_targetFeatureList, m_matchParameters.UserTolerances, 0);

                bool lengthCheck = (m_matchList.Count >= MIN_MATCHES_FOR_NORMAL_ASSUMPTION);
                if (m_matchParameters.ShouldCalculateSTAC && lengthCheck)
                {
					STACInformation stacInformation = new STACInformation(m_matchParameters.UseDriftTime);
					Console.WriteLine("Performing STAC");
					stacInformation.PerformSTAC(m_matchList, m_matchParameters.UserTolerances, m_matchParameters.UseDriftTime, m_matchParameters.UsePriors);
					STACParameterList.Add(stacInformation);
					Console.WriteLine("Populating FDR table");
                    PopulateSTACFDRTable();
                }
                if (m_matchParameters.ShouldCalculateHistogramFDR)
                {
					Console.WriteLine("Setting Mass Error Histogram FDR");
                    SetMassErrorHistogramFDR();
                }
                if (m_matchParameters.ShouldCalculateShiftFDR)
                {
					Console.WriteLine("Calculating Shift FDR");
                    int count = 0;
                    m_refinedTolerancesList.Add(FindOptimalTolerances(m_matchList));
                    for (int j = 0; j < m_matchList.Count; j++)
                    {
                        m_matchList[j].InRegion(m_refinedTolerancesList[0], m_matchParameters.UseEllipsoid);
                    }
                    for (int j = 0; j < m_matchList.Count; j++)
                    {
                        if (m_matchList[j].WithinRefinedRegion)
                            count++;
                    }
                    m_shiftedMatchList.AddRange(FindMatches(m_observedFeatureList, m_targetFeatureList, m_refinedTolerancesList[0], m_matchParameters.ShiftAmount));
                    m_shiftFDR = (1.0 * count) / m_shiftedMatchList.Count;
                    m_shiftConservativeFDR = (2.0 * count) / m_shiftedMatchList.Count;
                }
                if (m_matchParameters.ShouldCalculateSLiC && lengthCheck)
                {
					Console.WriteLine("Finding SLic Scores");
                    if (m_refinedTolerancesList.Count > 0 && !m_refinedTolerancesList[0].Refined)
                    {
                        m_refinedTolerancesList[0] = FindOptimalTolerances(m_matchList);
                    }
                    // Find SLiC scores.
                }
			//}
        }
        #endregion
    }
}
