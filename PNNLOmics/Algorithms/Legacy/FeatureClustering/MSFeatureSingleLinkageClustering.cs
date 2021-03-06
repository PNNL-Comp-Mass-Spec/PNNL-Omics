﻿using System.Collections.Generic;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{
	[System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Clustering")]
    public class MSFeatureSingleLinkageClustering <T, U>
        where T: MSFeatureLight, new()
        where U: UMCLight, new ()
    {

        public MSFeatureSingleLinkageClustering()
        {
            Parameters = new MsFeatureClusterParameters<T>();
        }

        #region IClusterer<T,U> Members

        public MsFeatureClusterParameters<T> Parameters
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
            var centroidType  = ClusterCentroidRepresentation.Mean;
            List<U> features                            = null;
            
            var featureIDToClusterID = new Dictionary<int, int>();
            foreach (var feature in rawMSFeatures)
            {
                //feature.ID = -1;
                featureIDToClusterID.Add(feature.Id, -1);
            }

            var maxDistance  = Parameters.MaxDistance;            
            var currentIndex    = 0;
            var N               = rawMSFeatures.Count;
            var numUmcsSoFar    = 0;

            var idFeatureMap = new Dictionary<int, List<T>>();
            var msFeatures                    = new List<T>();
            msFeatures.AddRange(rawMSFeatures);
            msFeatures.Sort(delegate(T x, T y)
            {
                return x.MassMonoisotopicAligned.CompareTo(y.MassMonoisotopicAligned);
            });

            while (currentIndex < N)
            {
                var currentFeature                = msFeatures[currentIndex];
                var currentFeatureClusterID     = featureIDToClusterID[currentFeature.Id];

                if (currentFeatureClusterID == -1)
                {
                    idFeatureMap.Add(numUmcsSoFar, new List<T>());
                    idFeatureMap[numUmcsSoFar].Add(currentFeature);
                    
                    currentFeatureClusterID                 = numUmcsSoFar;
                    featureIDToClusterID[currentFeature.Id] = numUmcsSoFar++;
                }

                var matchIndex = currentIndex + 1;
                if (matchIndex == N)
                    break;

                var massTolerance = currentFeature.MassMonoisotopicAligned * Parameters.Tolerances.Mass / 1000000;
                var maxMass       = currentFeature.MassMonoisotopicAligned + massTolerance;

                var matchPeak = msFeatures[matchIndex];
                while (matchPeak.MassMonoisotopicAligned < maxMass)
                {
                    var matchClusterID =  featureIDToClusterID[matchPeak.Id];
                    
                    //this is asking if they are already clustered together.
                    if (matchClusterID != currentFeatureClusterID)
                    {
                        // This checks the distance
                        var withinRange = Parameters.RangeFunction(currentFeature, matchPeak);
                        if (withinRange)
                        {
                            // Has the match peak been matched yet?
                            if (matchClusterID == -1)
                            {
                                idFeatureMap[currentFeatureClusterID].Add(matchPeak);
                                featureIDToClusterID[matchPeak.Id] = currentFeatureClusterID;
                                //matchPeak.ID = currentFeature.ID;
                            }
                            else
                            {
                                // Otherwise, we merge the old guy.
                                var tempFeatures = idFeatureMap[matchClusterID];
                                var oldID = matchClusterID;
                                foreach (var tempFeature in tempFeatures)
                                {
                                    featureIDToClusterID[tempFeature.Id] = currentFeatureClusterID;                                    
                                }
                                idFeatureMap[currentFeatureClusterID].AddRange(tempFeatures);
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

            features = new List<U>();
            foreach (var key in idFeatureMap.Keys)
            {
                var tempFeatures = idFeatureMap[key];
                var umc                = new U();
                foreach (var tempFeature in tempFeatures)
                {
                    tempFeature.SetParentFeature(umc);
                    umc.AddChildFeature(tempFeature);
                }
                umc.CalculateStatistics(centroidType);
                features.Add(umc);
            }
            
            var id = 0;
            foreach (var feature in features)
            {
                feature.Id = id++;
            }
            return features;
        }
        #endregion
    }
}
