using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureMatcher.Data;
using PNNLOmics.Algorithms.FeatureMatcher;
using PNNLOmics.Data;
using System.IO;

namespace PNNLOmics.Algorithms.Alignment
{
	public static class DriftTimeAlignment<T, U>
		where T : Feature, new()
		where U : Feature, new()
	{
		// Set this to an extremely high value so that drift time difference will not be considered when matching
		private const float DRIFT_TIME_TOLERANCE = 100f;
        
        /// <summary>
        /// Aligns features to the baseline correcting for drift time.
        /// </summary>
        /// <param name="fullObservedEnumerable">All features.</param>
        /// <param name="observedEnumerable">Filtered features to use for drift time correction.</param>
        /// <param name="targetEnumerable">Expected features that should be filtered.</param>
        /// <param name="massTolerance">PPM Mass Tolerance.</param>
        /// <param name="netTolerance">Normalized Elution Time Tolerance.</param>
        public static DriftTimeAlignmentResults<T, U> AlignObservedEnumerable(IEnumerable<T> fullObservedEnumerable, IEnumerable<T> observedEnumerable, IEnumerable<U> targetEnumerable, double massTolerance, double netTolerance)
		{
            
			// Setup Tolerance for Feature Matching
			FeatureMatcherParameters featureMatcherParameters   = new FeatureMatcherParameters();
			featureMatcherParameters.SetTolerances(massTolerance, netTolerance, DRIFT_TIME_TOLERANCE);
			featureMatcherParameters.UseDriftTime               = true;

			// Find all matches based on defined tolerances
			FeatureMatcher<T, U> featureMatcher = new FeatureMatcher<T, U>(observedEnumerable.ToList(), targetEnumerable.ToList(), featureMatcherParameters);
			List<FeatureMatch<T, U>> matchList  = featureMatcher.FindMatches(observedEnumerable.ToList(), targetEnumerable.ToList(), featureMatcherParameters.UserTolerances, 0);

			// Create <ObservedDriftTime, TargetDriftTime> XYData List
			List<XYData> xyDataList = new List<XYData>();
			foreach (FeatureMatch<T, U> featureMatch in matchList)
			{
				XYData xyData = new XYData(featureMatch.ObservedFeature.DriftTime, featureMatch.TargetFeature.DriftTime);
				xyDataList.Add(xyData);
			}

			// Find the Linear Equation for the <ObservedDriftTime, TargetDriftTime> XYData List
			LinearEquation linearEquation = LinearEquationCalculator.CalculateLinearEquation(xyDataList);

			// Set the Aligned Drift Time value for each of the observed Features, even if they were not found in matching
			foreach (T observedT in fullObservedEnumerable)
			{
				observedT.DriftTimeAligned = LinearEquationCalculator.CalculateNewValue(observedT.DriftTime, linearEquation);
			}

            DriftTimeAlignmentResults<T, U> results = new DriftTimeAlignmentResults<T, U>(matchList, linearEquation);

            return results;
		}
        /// <summary>
        /// Does a zero mean drift time correction.
        /// </summary>
        /// <param name="observedEnumerable">All observed features to shift that should already be drift time aligned.</param>
        /// <param name="targetEnumerable">Expected features</param>
        /// <param name="massTolerance">PPM Mass Tolerance</param>
        /// <param name="netTolerance">Normalized Elution Time tolerance.</param>
        /// <param name="driftTimeTolerance">Drift time tolerance to use.</param>
        public static DriftTimeAlignmentResults<T,U> CorrectForOffset(IEnumerable<T> observedEnumerable, IEnumerable<U> targetEnumerable, double massTolerance, double netTolerance, double driftTimeTolerance)
		{
			// Setup Tolerance for Feature Matching
			FeatureMatcherParameters featureMatcherParameters = new FeatureMatcherParameters();
			featureMatcherParameters.SetTolerances(massTolerance, netTolerance, (float)driftTimeTolerance);
			featureMatcherParameters.UseDriftTime = true;

			// Find all matches based on defined tolerances
			FeatureMatcher<T, U> featureMatcher = new FeatureMatcher<T, U>(observedEnumerable.ToList(), targetEnumerable.ToList(), featureMatcherParameters);
			List<FeatureMatch<T, U>> matchList = featureMatcher.FindMatches(observedEnumerable.ToList(), targetEnumerable.ToList(), featureMatcherParameters.UserTolerances, 0);

			// Create List of Drift Time differences
			List<double> differenceList = new List<double>(matchList.Count);
			foreach (FeatureMatch<T, U> featureMatch in matchList)
			{
				T observedFeature = featureMatch.ObservedFeature;
				U targetFeature = featureMatch.TargetFeature;

				double observedDriftTime = 0;
				if (observedFeature.DriftTimeAligned != double.NaN && observedFeature.DriftTimeAligned > 0.0)
				{
					observedDriftTime = observedFeature.DriftTimeAligned;
				}
				else
				{
					observedDriftTime = observedFeature.DriftTime;
				}

				double targetDriftTime = 0;
				if (targetFeature.DriftTimeAligned != double.NaN && targetFeature.DriftTimeAligned > 0.0)
				{
					targetDriftTime = targetFeature.DriftTimeAligned;
				}
				else
				{
					targetDriftTime = targetFeature.DriftTime;
				}

				differenceList.Add(observedDriftTime - targetDriftTime);
			}

			// Create bins for histogram
			List<double> bins = new List<double>();
			for (double i = -driftTimeTolerance; i <= driftTimeTolerance; i += (driftTimeTolerance / 100.0))
			{
				bins.Add(i);
			}
			bins.Add(driftTimeTolerance);

			// Group drift time differences into the bins
			var groupings = differenceList.GroupBy(difference => bins.First(bin => bin >= difference));

			// Order the groupings by their count, so the group with the highest count will be first
			var orderGroupingsByCount = from singleGroup in groupings
										orderby singleGroup.Count() descending
										select singleGroup;

			// Grab the drift time from the group with the most counts
			double driftTimeOffset = orderGroupingsByCount.First().Key;
            
			// Update all of the observed features with the new drift time
			foreach (T observedFeature in observedEnumerable)
			{
				if (observedFeature.DriftTimeAligned != double.NaN && observedFeature.DriftTimeAligned > 0.0)
				{
					observedFeature.DriftTimeAligned -= driftTimeOffset;
				}
				else
				{
					observedFeature.DriftTime -= (float)driftTimeOffset;
				}
			}

            LinearEquation linearEquation           = new LinearEquation();
            linearEquation.Slope                    = 0;
            linearEquation.Intercept                = driftTimeOffset;
            DriftTimeAlignmentResults<T, U> results = new DriftTimeAlignmentResults<T, U>(matchList, linearEquation);

            return results;
		}
	}
}
