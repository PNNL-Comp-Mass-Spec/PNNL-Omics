using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms;

namespace PNNLOmics.Algorithms.Distance
{
    public static class DistanceFactory<T> where T: FeatureLight, new()
    {
        public static DistanceFunction<T> CreateDistanceFunction(DistanceMetric metric)
        {
            DistanceFunction<T> function = null;
            switch (metric)
            {                
                case DistanceMetric.Euclidean:                    
                    EuclideanDistanceMetric<T> metricFunction = new EuclideanDistanceMetric<T>();
                    function = new DistanceFunction<T>(metricFunction.EuclideanDistance);
                    break;
                case DistanceMetric.WeightedEuclidean:
                    WeightedEuclideanDistance<T> weighted = new WeightedEuclideanDistance<T>();
                    function = new DistanceFunction<T>(weighted.EuclideanDistance);
                    break;
                default:
                    break;
            }
            return function;
        }
    }

}
