using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;

namespace PNNLOmics.Algorithms.FeatureMatcher
{
    /// <summary>
    /// Matches features to a list of mass tags.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FeatureMatcherLight<T, U>
        where T : FeatureLight
        where U : FeatureLight
    {
        /// <summary>
        /// Matches a list of features to a list of mass tags.
        /// </summary>
        /// <param name="features"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public List<FeatureMatchLight<T, U>> MatchFeatures(List<T>                  features,
                                                          List<U>                   tags,
                                                          FeatureMatcherLightOptions    options)
        {
            List<FeatureMatchLight<T, U>> matches = new List<FeatureMatchLight<T, U>>();

            // Construct a large array of features so we can do searching in linear time.
            List<FeatureLight> allFeatures = new List<FeatureLight>();
            foreach (T copyFeature in features)
            {
                allFeatures.Add(copyFeature as FeatureLight);
            }
            foreach (U copyTag in tags)
            {
                allFeatures.Add(copyTag as FeatureLight);
            }

            // Sort by mass, gives us the best search time.
            allFeatures.Sort(new Comparison<FeatureLight>(FeatureLight.MassComparison));
            
            double netTolerance     = options.Tolerances.RetentionTime;
            double massTolerance    = options.Tolerances.Mass;
            double driftTolerance   = options.Tolerances.DriftTime;
            double shift            = options.DaltonShift;

            int N               = allFeatures.Count;
            int elementNumber   = 0;
 
            // This was a linear search, now O(N^2).  Need to improve.
			while(elementNumber < N)
			{
                FeatureLight feature = allFeatures[elementNumber];
                U massTag            = feature as U;
                if (massTag == null)
                {

                    double lowerNET             = feature.NET - netTolerance;
                    double higherNET            = feature.NET + netTolerance;
                    double lowerDritfTime       = feature.DriftTime - driftTolerance;
                    double higherDriftTime      = feature.DriftTime + driftTolerance;
                    double currentMassTolerance = feature.MassMonoisotopic * massTolerance / 1000000.0;
                    double lowerMass            = feature.MassMonoisotopic - currentMassTolerance;
                    double higherMass           = feature.MassMonoisotopic + currentMassTolerance;
                    int matchIndex              = elementNumber - 1;
                    while (matchIndex >= 0)
                    {
                        FeatureLight toMatchFeature = allFeatures[matchIndex];
                        if (toMatchFeature.MassMonoisotopic < lowerMass)
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
                                    FeatureMatchLight<T, U> match   = new FeatureMatchLight<T, U>();
                                    match.Observed                  = feature as T; // it has to be T if not U.
                                    match.Target                    = tag;
                                    matches.Add(match);
                                }
                            }                            
                        }
                        matchIndex--;
                    }

                    matchIndex = elementNumber + 1;
                    while(matchIndex < N)                    
                    {
                        FeatureLight toMatchFeature = allFeatures[matchIndex];
                        if (toMatchFeature.MassMonoisotopic > higherMass)
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
                                    FeatureMatchLight<T, U> match   = new FeatureMatchLight<T, U>();
                                    match.Observed                  = feature as T; // it has to be T if not U.
                                    match.Target                    = tag;
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
