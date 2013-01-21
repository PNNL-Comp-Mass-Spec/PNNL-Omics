using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Distance
{
    public class PowerEuclideanDistanceMetric<T> where T: FeatureLight, new ()
    {
        /// <summary>
        /// Calculates the Euclidean distance based on drift time, aligned mass, and aligned NET.
        /// </summary>
        /// <param name="x">Feature x.</param>
        /// <param name="y">Feature y.</param>
        /// <returns>Distance calculated as </returns>
        public double EuclideanDistance(T x, T y)
        {
            double massDifference = Feature.ComputeMassPPMDifference(x.MassMonoisotopicAligned, y.MassMonoisotopicAligned);
            double netDifference  = x.RetentionTime - y.RetentionTime;

            netDifference *= 100;

            double driftDifference = x.DriftTime - y.DriftTime;
            double sum             = (massDifference * massDifference) +
                                     (netDifference * netDifference) +
                                     (driftDifference * driftDifference);

            return Math.Sqrt(sum);
        }

        /// <summary>
        /// Calculates the weighted Euclidean distance based on drift time, aligned mass, and aligned NET.
        /// </summary>
        /// <param name="x">Feature x.</param>
        /// <param name="y">Feature y.</param>
        /// <returns>Distance calculated as </returns>
        public double EuclideanDistance(T x, T y, double massWeight, double netWeight, double driftWeight)
        {
            double massDifference = Feature.ComputeMassPPMDifference(x.MassMonoisotopicAligned, y.MassMonoisotopicAligned);
            double netDifference = x.RetentionTime - y.RetentionTime;
            double driftDifference = x.DriftTime - y.DriftTime;
            double sum = (massDifference * massDifference) * massWeight +
                                     (netDifference * netDifference) * netDifference +
                                     (driftDifference * driftDifference) * driftWeight;

            return Math.Sqrt(sum);
        }
    }
}
