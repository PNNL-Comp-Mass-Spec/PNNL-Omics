using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureMatcher.Data;
using PNNLOmics.Data.MassTags;

namespace PNNLOmics.Algorithms.FeatureMatcher
{
    /// <summary>
    /// Matches features to a list of mass tags.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PeakMatcher<T, U>
        where T : Feature, new ()
        where U : Feature, new ()
    {
        /// <summary>
        /// Matches a list of features to a list of mass tags.
        /// </summary>
        /// <param name="features"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public List<FeatureMatch<T, U>> MatchFeatures(    List<T>               features,
                                                          List<U>               tags,
                                                          PeakMatcherOptions    options)
        {
            List<FeatureMatch<T, U>> matches = new List<FeatureMatch<T, U>>();

            // Construct a large array of features so we can do searching in linear time.
            List<Feature> allFeatures = new List<Feature>();
            foreach (T copyFeature in features)
            {
                allFeatures.Add(copyFeature as Feature);
            }
            foreach (U copyTag in tags)
            {
                allFeatures.Add(copyTag as Feature);
            }

            // Sort by mass, gives us the best search time.
            allFeatures.Sort(new Comparison<Feature>(Feature.MassComparison));
            
            double netTolerance     = options.Tolerances.RetentionTime;
            double massTolerance    = options.Tolerances.Mass;
            double driftTolerance   = options.Tolerances.DriftTime;
            double shift            = options.DaltonShift;

            int N               = allFeatures.Count;
            int elementNumber   = 0;
 
            // This was a linear search, now O(N^2).  Need to improve.
			while(elementNumber < N)
			{
                Feature feature = allFeatures[elementNumber];
                U massTag            = feature as U;
                if (massTag == null)
                {

                    double lowerNET             = feature.NET - netTolerance;
                    double higherNET            = feature.NET + netTolerance;
                    double lowerDritfTime       = feature.DriftTime - driftTolerance;
                    double higherDriftTime      = feature.DriftTime + driftTolerance;
                    double currentMassTolerance = feature.MassMonoisotopicAligned * massTolerance / 1000000.0;
                    double lowerMass            = feature.MassMonoisotopicAligned - currentMassTolerance;
                    double higherMass           = feature.MassMonoisotopicAligned + currentMassTolerance;
                    int matchIndex              = elementNumber - 1;
                    while (matchIndex >= 0)
                    {
                        Feature toMatchFeature = allFeatures[matchIndex];
                        if (toMatchFeature.MassMonoisotopicAligned < lowerMass)
                        {
                            break;
                        }

                        U tag = toMatchFeature as U;
                        if (tag != null)
                        {                           
                            if (lowerNET <= tag.NET && tag.NET <= higherNET)
                            {
                                if (lowerDritfTime <= tag.DriftTime && tag.DriftTime <= higherDriftTime)
                                {
                                    FeatureMatch<T, U> match   = new FeatureMatch<T, U>(feature as T, tag as U, false, false);
                                    matches.Add(match);
                                }
                            }                            
                        }
                        matchIndex--;
                    }

                    matchIndex = elementNumber + 1;
                    while(matchIndex < N)                    
                    {
                        Feature toMatchFeature = allFeatures[matchIndex];
                        if (toMatchFeature.MassMonoisotopicAligned > higherMass)
                        {
                            break;
                        }

                        U tag = toMatchFeature as U;
                        if (tag != null)
                        {
                            if (lowerNET <= tag.NET && tag.NET <= higherNET)
                            {
                                if (lowerDritfTime <= tag.DriftTime && tag.DriftTime <= higherDriftTime)
                                {
                                    FeatureMatch<T, U> match = new FeatureMatch<T, U>(feature as T, tag as U, false, false);
                                    matches.Add(match);
                                }
                            }
                        }
                        matchIndex++;
                    }
                }
                elementNumber++;																						
			}			
            return matches;
        }
    }
}
