using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    public class MSFeatureSingleLinkageClustering <T, U>
        where T: MSFeatureLight, new()
        where U: UMCLight, new ()
    {

        public MSFeatureSingleLinkageClustering()
        {
            Parameters = new MSFeatureClusterParameters<T>();
        }

        #region IClusterer<T,U> Members

        public MSFeatureClusterParameters<T> Parameters
        {
            get;
            set;
        }

        /// <summary>
        /// Finds LCMS Features from MS Features.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<U> Cluster(List<T> rawMSFeatures)
        {
            ClusterCentroidRepresentation centroidType  = ClusterCentroidRepresentation.Mean;
            List<U> features                            = null;            
            
            foreach (T feature in rawMSFeatures)
            {
                feature.ID = -1;
            }

            double maxDistance  = Parameters.MaxDistance;            
            int currentIndex    = 0;
            int N               = rawMSFeatures.Count;
            int numUmcsSoFar    = 0;

            Dictionary<int, List<T>> idFeatureMap = new Dictionary<int, List<T>>();
            List<T> msFeatures                    = new List<T>();
            msFeatures.AddRange(rawMSFeatures);
            msFeatures.Sort(delegate(T x, T y)
            {
                return x.MassMonoisotopic.CompareTo(y.MassMonoisotopic);
            });

            while (currentIndex < N)
            {
                T currentFeature = msFeatures[currentIndex];
                if (currentFeature.ID == -1)
                {
                    idFeatureMap.Add(numUmcsSoFar, new List<T>());
                    idFeatureMap[numUmcsSoFar].Add(currentFeature);
                    currentFeature.ID = numUmcsSoFar;
                    numUmcsSoFar++;
                }

                int matchIndex = currentIndex + 1;
                if (matchIndex == N)
                    break;

                double massTolerance = currentFeature.MassMonoisotopic * Parameters.Tolerances.Mass / 1000000;
                double maxMass       = currentFeature.MassMonoisotopic + massTolerance;

                T matchPeak = msFeatures[matchIndex];
                while (matchPeak.MassMonoisotopic < maxMass)
                {
                    if (matchPeak.ID != currentFeature.ID)
                    {
                        bool withinRange = Parameters.RangeFunction(currentFeature, matchPeak);
                        if (withinRange)
                        {
                            if (matchPeak.ID == -1)
                            {
                                idFeatureMap[currentFeature.ID].Add(matchPeak);
                                matchPeak.ID = currentFeature.ID;
                            }
                            else
                            {
                                List<T> tempFeatures = idFeatureMap[matchPeak.ID];
                                int oldID = matchPeak.ID;
                                foreach (T tempFeature in tempFeatures)
                                {
                                    tempFeature.ID = currentFeature.ID;
                                }
                                idFeatureMap[currentFeature.ID].AddRange(tempFeatures);
                                idFeatureMap.Remove(oldID);
                            }
                        }
                    }
                    matchIndex++;
                    if (matchIndex < N)
                    {
                        matchPeak = msFeatures[matchIndex];
                    }
                    else
                    {
                        break;
                    }
                }
                currentIndex++;
            }

            int id = 0;
            features = new List<U>();
            foreach (int key in idFeatureMap.Keys)
            {
                List<T> tempFeatures = idFeatureMap[key];
                U umc                = new U();
                foreach (T tempFeature in tempFeatures)
                {
                    tempFeature.ID = id++;
                    tempFeature.SetParentFeature(umc);
                    umc.AddChildFeature(tempFeature);
                }
                umc.CalculateStatistics(centroidType);
                features.Add(umc);
            }
            
            id = 0;
            foreach (U feature in features)
            {
                feature.ID = id++;
            }
            return features;
        }
        #endregion
    }
}
