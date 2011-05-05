using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureMatcher.Data;
using PNNLOmics.Algorithms.FeatureMatcher;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.Alignment
{
	public static class DriftTimeAlignment<T, U>
		where T : Feature, new()
		where U : Feature, new()
	{
		// Set this to an extremely high value so that drift time difference will not be considered when matching
		private const float DRIFT_TIME_TOLERANCE = 100f;

		public static void AlignObservedEnumerable(IEnumerable<T> observedEnumerable, IEnumerable<U> targetEnumerable, double massTolerance, double netTolerance)
		{
			// Setup Tolerance for Feature Matching
			FeatureMatcherParameters featureMatcherParameters = new FeatureMatcherParameters();
			featureMatcherParameters.SetTolerances(massTolerance, netTolerance, DRIFT_TIME_TOLERANCE);
			featureMatcherParameters.UseDriftTime = true;

			// Find all matches based on defined tolerances
			FeatureMatcher<T, U> featureMatcher = new FeatureMatcher<T, U>(observedEnumerable.ToList(), targetEnumerable.ToList(), featureMatcherParameters);
			List<FeatureMatch<T, U>> matchList = featureMatcher.FindMatches(observedEnumerable.ToList(), targetEnumerable.ToList(), featureMatcherParameters.UserTolerances, 0);

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
			foreach (T observedT in observedEnumerable)
			{
				observedT.DriftTimeAligned = LinearEquationCalculator.CalculateNewValue(observedT.DriftTime, linearEquation);
			}
		}
	}
}
