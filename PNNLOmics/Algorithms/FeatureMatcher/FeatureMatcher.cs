using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Populate the FDR table with default cutoffs.
        /// </summary>
		private List<STACFDR> SetSTACCutoffs()
        {
			List<STACFDR> stacFDRList = new List<STACFDR>();

            for (double cutoff = 0.99; cutoff > 0.90; cutoff -= 0.01)
            {
                STACFDR tableLine = new STACFDR(cutoff);
				stacFDRList.Add(tableLine);
            }
            for (double cutoff = 0.90; cutoff >= 0; cutoff -= 0.10)
            {
                STACFDR tableLine = new STACFDR(cutoff);
				stacFDRList.Add(tableLine);
            }

			return stacFDRList;
        }
        /// <summary>
        /// Fills in the values for the STAC FDR table.
        /// </summary>
        public List<STACFDR> PopulateSTACFDRTable(List<FeatureMatch<T,U>> matchList)
        {
            List<STACFDR> stacFDRList = SetSTACCutoffs();
			matchList.Sort(FeatureMatch<T, U>.STACComparisonDescending);

			// Iterate over all defined cutoff ranges
			for (int cutoffIndex = 0; cutoffIndex < stacFDRList.Count; cutoffIndex++)
			{
				double falseDiscoveries = 0.0;
				int conformationMatches = 0;
				int amtMatches = 0;
				double fdr = 0.0;
				List<U> uniqueTargetList = new List<U>();

				// Find all matches for this particular cutoff
				foreach (FeatureMatch<T, U> match in matchList)
				{
					double stac = match.STACScore;
					if (stac >= stacFDRList[cutoffIndex].Cutoff)
					{
						if (!uniqueTargetList.Contains(match.TargetFeature))
						{
							// Find out if this is a new, unique Mass Tag. If not, then it is just a new conformation.
							var searchForMassTagIDQuery = from target in uniqueTargetList
														  where target.ID == match.TargetFeature.ID
														  select target;

							if (searchForMassTagIDQuery.Count() == 0)
							{
								amtMatches++;
							}

							uniqueTargetList.Add(match.TargetFeature);
							falseDiscoveries += 1 - stac;
							conformationMatches++;
						}
					}
					else
					{
						// Since we have sorted by descending STAC Score, if we get outside the cutoff range, we can stop
						break;
					}
				}

				// After all matches have been found, report the FDR
				if (conformationMatches > 0)
				{
					fdr = falseDiscoveries / (double)conformationMatches;
					stacFDRList[cutoffIndex].FillLine(fdr, conformationMatches, amtMatches, falseDiscoveries);
				}
				else
				{
					stacFDRList[cutoffIndex].FillLine(0, 0, 0, 0);
				}
			}

			return stacFDRList;
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
        #endregion

        #region Public functions
        /// <summary>
        /// Function to call to re-calculate algorithm results. [gord - following comment lies!] Called within constructor by default.
        /// </summary>
        public void MatchFeatures()
        {
            m_matchList.Clear();
            m_matchList = FindMatches(m_observedFeatureList, m_targetFeatureList, m_matchParameters.UserTolerances, 0);

            bool lengthCheck = (m_matchList.Count >= MIN_MATCHES_FOR_NORMAL_ASSUMPTION);
            if (m_matchParameters.ShouldCalculateSTAC && lengthCheck)
            {
				STACInformation stacInformation = new STACInformation(m_matchParameters.UseDriftTime);
				Console.WriteLine("Performing STAC");
				stacInformation.PerformSTAC(m_matchList, m_matchParameters.UserTolerances, m_matchParameters.UseDriftTime, m_matchParameters.UsePriors);

				// Add the Refined Tolerances that STAC calculated
				m_refinedTolerancesList.Add(stacInformation.RefinedTolerances);

				STACParameterList.Add(stacInformation);
				Console.WriteLine("Populating FDR table");
                m_stacFDRList = PopulateSTACFDRTable(m_matchList);
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
        }
        #endregion
    }
}
