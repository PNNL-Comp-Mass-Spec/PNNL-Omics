/*////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * 
 * Name:    UMC Single Linkage Cluster Parameters
 * File:    UMCSingleLinkageClustererParameters.cs
 * Author:  Brian LaMarche 
 * Purpose: Parameters for performing single linkage clustering.
 * Date:    5-19-2010
 * Revisions:
 *          5-19-2010 - BLL - Created class for single linkage clustering parameters.
 ////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
using System;
using System.Collections.Generic;

using PNNLOmics.Data;
using PNNLOmics.Algorithms;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// Parameters for the single linkage UMC Clustering Algorithm.
    /// </summary>
    public class FeatureClusterParameters<T>
        where T: FeatureLight, new()
    {        
        #region Constants
        /// <summary>
        /// Default value whether to separate UMC's based on charge state information.
        /// </summary>
        public const bool CONST_DEFAULT_ONLY_CLUSTER_SAME_CHARGE_STATES = false;
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FeatureClusterParameters()
        {
            Clear();
        }

        #region Properties
        public ClusterCentroidRepresentation CentroidRepresentation
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the tolerance values for the clustering algorithm.
        /// </summary>
        public FeatureTolerances Tolerances
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets whether to separate features based on charge state.
        /// </summary>
        public bool OnlyClusterSameChargeStates
        { 
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the distance function to use for calculating the distance between two UMC's.
        /// </summary>
        public DistanceFunction<T> DistanceFunction
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the function that determines if two features are within range of each other.
        /// </summary>
        public WithinTolerances<T> RangeFunction
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Resets the parameters to their default values.
        /// </summary>
        public virtual void Clear()
        {
            Tolerances                      = new FeatureTolerances();
            OnlyClusterSameChargeStates     = CONST_DEFAULT_ONLY_CLUSTER_SAME_CHARGE_STATES;
            DistanceFunction                = new DistanceFunction<T>(EuclideanDistance);            
            CentroidRepresentation          = ClusterCentroidRepresentation.Median;
        }

        #region Distance Functions
        /// <summary>
        /// Calculates the Euclidean distance based on drift time, aligned mass, and aligned NET.
        /// </summary>
        /// <param name="x">Feature x.</param>
        /// <param name="y">Feature y.</param>
        /// <returns>Distance calculated as </returns>
        public double EuclideanDistance(T x, T y)
        {
            double massDifference  = Feature.ComputeMassPPMDifference(x.MassMonoisotopic, y.MassMonoisotopic);
            double netDifference   = x.RetentionTime - y.RetentionTime;
            double driftDifference = x.DriftTime  - y.DriftTime;
            double sum             = (massDifference  * massDifference)  +
                                     (netDifference   * netDifference)   + 
                                     (driftDifference * driftDifference);

            return Math.Sqrt(sum);
        }
        /// <summary>
        /// Computes the mass difference between two features.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected bool WithinRange(T x, T y)
        {
			// later is more related to determining a scalar value instead.
			double massDiff         = Math.Abs(Feature.ComputeMassPPMDifference(x.MassMonoisotopic, y.MassMonoisotopic));
			double netDiff          = Math.Abs(x.RetentionTime - y.RetentionTime);
			double driftDiff        = Math.Abs(y.RetentionTime - y.DriftTime);

			// Make sure we fall within the distance range before computing...
            return (massDiff <= Tolerances.Mass && netDiff <= Tolerances .RetentionTime && driftDiff <= Tolerances.DriftTime);            
        }        
        
        /// <summary>
        /// Calculates the weighted Euclidean distance based on drift time, aligned mass, and aligned NET.
        /// </summary>
        /// <param name="x">Feature x.</param>
        /// <param name="y">Feature y.</param>
        /// <returns>Distance calculated as </returns>
		public double EuclideanDistance(T x, T y, double massWeight, double netWeight, double driftWeight)
        {
			double massDifference = Feature.ComputeMassPPMDifference(x.MassMonoisotopic, y.MassMonoisotopic);
            double netDifference = x.RetentionTime - y.RetentionTime;
            double driftDifference = x.DriftTime - y.DriftTime;
            double sum = (massDifference * massDifference)*massWeight +
                                     (netDifference * netDifference)*netDifference +
                                     (driftDifference * driftDifference) * driftWeight;

            return Math.Sqrt(sum);
        }
        #endregion
    }
}
