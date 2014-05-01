using System;
using System.Collections.Generic;
using System.Linq;
using PNNLOmics.Algorithms.FeatureMatcher.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Utilities;

namespace PNNLOmics.Algorithms.FeatureMatcher
{
    public class FeatureMatcher<TObserved, TTarget>
        where TObserved : FeatureLight, new()
        where TTarget   : FeatureLight, new()
    {
        #region Members
        
        private FeatureMatcherParameters m_matchParameters;

        private List<FeatureMatch<TObserved, TTarget>> m_matchList;
        private List<FeatureMatch<TObserved, TTarget>> m_shiftedMatchList;
        private List<TObserved> m_observedFeatureList;
        private List<TTarget> m_targetFeatureList;

        private double m_shiftFDR;
        private double m_shiftConservativeFDR;
        private double m_errorHistogramFDR;

        private List<STACInformation> m_stacParametersList;
        private List<STACFDR> m_stacFDRList;
        private List<FeatureMatcherTolerances> m_refinedTolerancesList;
        private SLiCInformation m_slicParameters;

        const int MIN_MATCHES_FOR_NORMAL_ASSUMPTION = 1;
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
        public List<FeatureMatch<TObserved, TTarget>> MatchList
        {
            get { return m_matchList; }
        }
        /// <summary>
        /// Gets the list of features matched with a shift.
        /// </summary>
        public List<FeatureMatch<TObserved, TTarget>> ShiftedMatchList
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
        public FeatureMatcher(List<TObserved> observedFeatureList, List<TTarget> targetFeatureList, FeatureMatcherParameters matchParameters)
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
            m_matchList = new List<FeatureMatch<TObserved, TTarget>>();
            m_shiftedMatchList = new List<FeatureMatch<TObserved, TTarget>>();
        }
        
        /// <summary>
        /// Set the list of charge states to be used in IMS calculations.
        /// </summary>
        private void SetChargeStateList()
        {
            m_matchParameters.ChargeStateList.Clear();
            foreach (var observedFeature in m_observedFeatureList)
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
        public List<FeatureMatch<TObserved, TTarget>> FindMatches(List<TObserved> shortObservedList, List<TTarget> shortTargetList, FeatureMatcherTolerances tolerances, double shiftAmount)
        {
            // Create a list to hold the matches until they are returned.
            var matchList = new List<FeatureMatch<TObserved, TTarget>>();

            // Set indices to use when iterating over the lists.
            var observedIndex = 0;
            var targetIndex = 0;
            var lowerBound = 0;


            // Sort both lists by mass.
			if (shortObservedList[0].MassMonoisotopicAligned != double.NaN && shortObservedList[0].MassMonoisotopicAligned > 0.0)
            {
                shortObservedList.Sort(FeatureLight.MassAlignedComparison);
            }
            else
            {
                shortObservedList.Sort(FeatureLight.MassComparison);
            }
			if (shortTargetList[0].MassMonoisotopicAligned != double.NaN && shortTargetList[0].MassMonoisotopicAligned > 0.0)
			{
                shortTargetList.Sort(FeatureLight.MassAlignedComparison);
			}
			else
			{
                shortTargetList.Sort(FeatureLight.MassComparison);
			}

            // Locally store the tolerances.
            var massTolerancePPM = tolerances.MassTolerancePPM;
            var netTolerance = tolerances.NETTolerance;
            var driftTimeTolerance = tolerances.DriftTimeTolerance;

            // Iterate through the list of observed features.
            while( observedIndex < shortObservedList.Count )
            {
                // Store the current observed feature locally.
                var observedFeature = shortObservedList[observedIndex];
                // Flag variable that gets set to false when the observed mass is greater than the current mass tag by more than the tolerance.
                var continueLoop = true;
                // Set the target feature iterator to the current lower bound.
                targetIndex = lowerBound;
                // Iterate through the list of target featrues or until the observed feature is too great.
                while( targetIndex < shortTargetList.Count && continueLoop )
                {
                    // Add any shift to the mass tag.
                    var targetFeature = shortTargetList[targetIndex];

                    // Check to see that the features are within the mass tolearance of one another.
					double massDifference = 0;
                    if (WithinMassTolerance(observedFeature, targetFeature, massTolerancePPM, shiftAmount, out massDifference))
                    {
                        var withinTolerances = WithinNETTolerance(observedFeature, targetFeature, netTolerance);
                        if (m_matchParameters.UseDriftTime)
                        {
                            withinTolerances = withinTolerances & withinDriftTimeTolerance(observedFeature, targetFeature, driftTimeTolerance);
							withinTolerances = withinTolerances & (observedFeature.ChargeState == targetFeature.ChargeState);
                        }
                        // Create a temporary match between the two and check it against all tolerances before adding to the match list.
                        if (withinTolerances)
                        {
                            var match = new FeatureMatch<TObserved, TTarget>();
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
			var stacFDRList = new List<STACFDR>();

            for (var cutoff = 0.99; cutoff > 0.90; cutoff -= 0.01)
            {
                var tableLine = new STACFDR(Math.Round(cutoff, 2));
				stacFDRList.Add(tableLine);
            }
            for (var cutoff = 0.90; cutoff >= 0; cutoff -= 0.10)
            {
                var tableLine = new STACFDR(Math.Round(cutoff, 2));
				stacFDRList.Add(tableLine);
            }

			return stacFDRList;
        }
        /// <summary>
        /// Fills in the values for the STAC FDR table.
        /// </summary>
        public List<STACFDR> PopulateStacfdrTable(List<FeatureMatch<TObserved,TTarget>> matchList)
        {
            var stacFDRList = SetSTACCutoffs();
			matchList.Sort(FeatureMatch<TObserved, TTarget>.STACComparisonDescending);

			// Iterate over all defined cutoff ranges
			for (var cutoffIndex = 0; cutoffIndex < stacFDRList.Count; cutoffIndex++)
			{
				var falseDiscoveries = 0.0;
				var conformationMatches = 0;
				var amtMatches = 0;
				double fdr;
				var uniqueIndexList = new HashSet<int>();
				var uniqueIdList = new HashSet<int>();

				// Find all matches for this particular cutoff
				foreach (var match in matchList)
				{
					var stac = match.STACScore;
					if (stac >= stacFDRList[cutoffIndex].Cutoff)
					{
					    if (uniqueIndexList.Contains(match.TargetFeature.Index)) continue;

					    // Find out if this is a new, unique Mass Tag. If not, then it is just a new conformation.
					    if (!uniqueIdList.Contains(match.TargetFeature.ID))
					    {
					        uniqueIdList.Add(match.TargetFeature.ID);
					        amtMatches++;
					    }

                        uniqueIndexList.Add(match.TargetFeature.Index);
					    falseDiscoveries += 1 - stac;
					    conformationMatches++;
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
					fdr = falseDiscoveries / conformationMatches;
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
        /// Sets the false discovery rate by creating a histogram of the mass errors and comparing the proportion above a threshhold to the area below.
        /// </summary>
        private void SetMassErrorHistogramFdr()
        {
            // Populate the mass error list and create the histogram.
            var massErrorList = m_matchList.Select(match => match.DifferenceVector[1, 1]).ToList();
            var histogramList = MathUtilities.GetHistogramValues(massErrorList, m_matchParameters.HistogramBinWidth);

            var peakIndex = 0;
            var peakValue = 0.0;
            var upperBound = 0.0;
            var lowerBound = 0.0;
            var meanValue = 0.0;
            // Find the maximum and average values.
            for( var i=0; i<histogramList.Count; i++ )
            {
                var bin = histogramList[i];
                if (bin.Y > peakValue)
                {
                    peakValue = bin.Y;
                    peakIndex = i;
                }
                meanValue += bin.Y;
            }
            meanValue /= histogramList.Count;
            // Set the threshold to a value between the peak and the average value.
            double threshold = meanValue + m_matchParameters.HistogramMultiplier * (peakValue - meanValue);
            // Find the upper bound.
            for (var i = peakIndex; i < histogramList.Count; i++)
            {
                var bin = histogramList[i];
                var lowerBin = histogramList[i - 1];
                if (bin.Y < threshold && lowerBin.Y >= threshold)
                {
                    upperBound = lowerBin.X + (lowerBin.Y - threshold) / (lowerBin.Y - bin.Y) * (bin.X - lowerBin.X);
                }
            }
            // Find the lower bound.
            for (var i = peakIndex; i >= 0; i--)
            {
                var bin = histogramList[i];
                var upperBin = histogramList[i + 1];
                if (bin.Y < threshold && upperBin.Y >= threshold)
                {
                    lowerBound = bin.X + (threshold-bin.Y) / (upperBin.Y-bin.Y) * (upperBin.X - bin.X);
                }
            }
            // Count the number of matches within the bounds and calculate the FDR.
            var countInBounds = 0;
            foreach (var massDifference in massErrorList)
            {
                if (massDifference >= lowerBound && massDifference <= upperBound)
                {
                    countInBounds++;
                }
            }
            m_errorHistogramFDR = countInBounds/((upperBound-lowerBound)*threshold);
        }

        private bool WithinMassTolerance(TObserved observedFeature, TTarget targetFeature, double massTolerancePpm, double shiftAmount, out double difference)            
        {
            if (massTolerancePpm > 0)
            {
                double observedMass; 
                double targetMass;
				if (!double.IsNaN(observedFeature.MassMonoisotopicAligned) && observedFeature.MassMonoisotopicAligned > 0.0)
                {
					observedMass = observedFeature.MassMonoisotopicAligned;
                }
                else
                {
                    observedMass = observedFeature.MassMonoisotopic;
                }
				if (!double.IsNaN(targetFeature.MassMonoisotopicAligned) && targetFeature.MassMonoisotopicAligned > 0.0)
                {
					targetMass = targetFeature.MassMonoisotopicAligned;
                }
                else
                {
                    targetMass = targetFeature.MassMonoisotopic;
                }
				
				difference = (targetMass + shiftAmount - observedMass) / targetMass * 1E6;
                return (Math.Abs(difference)< massTolerancePpm);
            }
            difference = double.MaxValue;
            return false;
        }
        private bool WithinNETTolerance(TObserved observedFeature, TTarget targetFeature, double netTolerance)
        {
            if (netTolerance > 0)
            {
                double targetNet;
                if (!double.IsNaN(targetFeature.NetAligned) && targetFeature.NetAligned > 0.0)
                {
					targetNet = targetFeature.NetAligned;
                }
                else
                {
                    targetNet = targetFeature.Net;
                }
                double observedNet;
                if (!double.IsNaN(observedFeature.NetAligned) && observedFeature.NetAligned > 0.0)
                {
					observedNet = observedFeature.NetAligned;
                }
                else
                {
                    observedNet = observedFeature.Net;
                }
                var difference = Math.Abs(targetNet - observedNet);
                return (difference < netTolerance);
            }
            return false;
        }
        private bool withinDriftTimeTolerance(TObserved observedFeature, TTarget targetFeature, float driftTimeTolerance)
        {
            if (driftTimeTolerance > 0)
            {
				double targetDriftTime;
				double observedDriftTime;
				if (!double.IsNaN(targetFeature.DriftTimeAligned) && targetFeature.DriftTimeAligned > 0.0)
				{
					targetDriftTime = targetFeature.DriftTimeAligned;
				}
				else
				{
					targetDriftTime = targetFeature.DriftTime;
				}
				if (!double.IsNaN(observedFeature.DriftTimeAligned) && observedFeature.DriftTimeAligned > 0.0)
				{
					observedDriftTime = observedFeature.DriftTimeAligned;
				}
				else
				{
					observedDriftTime = observedFeature.DriftTime;
				}

                var difference = Math.Abs(targetDriftTime - observedDriftTime);
                return (difference < driftTimeTolerance);
            }
            return false;
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

            var lengthCheck = (m_matchList.Count >= MIN_MATCHES_FOR_NORMAL_ASSUMPTION);
            if (m_matchParameters.ShouldCalculateSTAC && lengthCheck)
            {
				var stacInformation = new STACInformation(m_matchParameters.UseDriftTime);
                
                // Attach the event handlers 
                stacInformation.MessageEvent += StacInformationMessageHandler;                
                stacInformation.IterationEvent += StacInformationIterate;
                stacInformation.DebugEvent += StacInformationDebugHandler;

				ReportMessage("Performing STAC");
				stacInformation.PerformStac(m_matchList, m_matchParameters.UserTolerances, m_matchParameters.UseDriftTime, m_matchParameters.UsePriors);

				// Add the Refined Tolerances that STAC calculated
				m_refinedTolerancesList.Add(stacInformation.RefinedTolerances);

				STACParameterList.Add(stacInformation);
                ReportMessage("Populating FDR table");
                m_stacFDRList = PopulateStacfdrTable(m_matchList);
            }
            if (m_matchParameters.ShouldCalculateHistogramFDR)
            {
                ReportMessage("Setting Mass Error Histogram FDR");
                SetMassErrorHistogramFdr();
            }
            if (m_matchParameters.ShouldCalculateShiftFDR)
            {
                ReportMessage("Calculating Shift FDR");
                var count = 0;
                for (var j = 0; j < m_matchList.Count; j++)
                {
                    m_matchList[j].InRegion(m_refinedTolerancesList[0], m_matchParameters.UseEllipsoid);
                }
                for (var j = 0; j < m_matchList.Count; j++)
                {
                    if (m_matchList[j].WithinRefinedRegion)
                        count++;
                }
                m_shiftedMatchList.AddRange(FindMatches(m_observedFeatureList, m_targetFeatureList, m_refinedTolerancesList[0], m_matchParameters.ShiftAmount));
                m_shiftFDR = (1.0 * count) / m_shiftedMatchList.Count;
                m_shiftConservativeFDR = (2.0 * count) / m_shiftedMatchList.Count;
            }

            OnProcessingComplete(new MessageEventArgs("Processing Complete"));
        }
        #endregion

        #region "Events and related functions"

        public event MessageEventHandler ErrorEvent;
        public event MessageEventHandler IterationEvent;
        public event MessageEventHandler MessageEvent;
        public event MessageEventHandler DebugEvent;
        public event MessageEventHandler ProcessingCompleteEvent;
        
        protected void StacInformationIterate(object sender, MessageEventArgs e) 
        {
            OnIterate(e);
    }

        protected void StacInformationErrorHandler(object sender, MessageEventArgs e) 
        {
            ReportError(e);
}

        protected void StacInformationMessageHandler(object sender, MessageEventArgs e) 
        {
            ReportMessage(e);
        }

        protected void StacInformationDebugHandler(object sender, MessageEventArgs e) 
        {
            OnDebugMessage(e);
        }

        /// <summary>
        /// Report an error message using OnErrorMessage
        /// </summary>
        /// <param name="message"></param>
        protected void ReportError(string message) 
        {
            OnErrorMessage(new MessageEventArgs(message));
        }

        /// <summary>
        /// Report an error message using OnErrorMessage
        /// </summary>
        /// <param name="message"></param>
        protected void ReportError(MessageEventArgs e) 
        {
            OnErrorMessage(e);
        }

        /// <summary>
        /// Report a progress message using OnMessage
        /// </summary>
        /// <param name="message"></param>
        protected void ReportMessage(string message) 
        {
            OnMessage(new MessageEventArgs(message));
        }

        /// <summary>
        /// Report a progress message using OnMessage
        /// </summary>
        /// <param name="message"></param>
        protected void ReportMessage(MessageEventArgs e) 
        {
            OnMessage(e);
        }

        private void OnDebugMessage(MessageEventArgs e) 
        {
            if (DebugEvent != null)
                DebugEvent(this, e);
        }

        private void OnErrorMessage(MessageEventArgs e) 
        {
            if (ErrorEvent != null)
                ErrorEvent(this, e);
        }

        private void OnIterate(MessageEventArgs e) 
        {
            if (IterationEvent != null)
                IterationEvent(this, e);
        }

        private void OnMessage(MessageEventArgs e) 
        {
            if (MessageEvent != null)
                MessageEvent(this, e);
        }

        private void OnProcessingComplete(MessageEventArgs e) 
        {
            if (ProcessingCompleteEvent != null)
                ProcessingCompleteEvent(this, e);
        }

        #endregion
    }

}
