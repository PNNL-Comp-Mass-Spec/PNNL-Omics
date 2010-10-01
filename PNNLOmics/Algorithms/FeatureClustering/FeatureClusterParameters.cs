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
    public class FeatureClusterParameters
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
        public DistanceFunction DistanceFunction
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Resets the parameters to their default values.
        /// </summary>
        public void Clear()
        {
            Tolerances                      = new FeatureTolerances();
            OnlyClusterSameChargeStates     = CONST_DEFAULT_ONLY_CLUSTER_SAME_CHARGE_STATES;
            DistanceFunction                = new DistanceFunction(EuclideanDistance);
            CentroidRepresentation          = ClusterCentroidRepresentation.Median;
        }

        #region Distance Functions
        /// <summary>
        /// Calculates the Euclidean distance based on drift time, aligned mass, and aligned NET.
        /// </summary>
        /// <param name="x">Feature x.</param>
        /// <param name="y">Feature y.</param>
        /// <returns>Distance calculated as </returns>
        public double EuclideanDistance(UMCLight x, UMCLight y)
        {
            double massDifference  = Feature.ComputeMassPPMDifference(x.MassMonoisotopic, y.MassMonoisotopic);
            double netDifference   = x.NET - y.NET;
            double driftDifference = x.DriftTime  - y.DriftTime;
            double sum             = (massDifference  * massDifference)  +
                                     (netDifference   * netDifference)   + 
                                     (driftDifference * driftDifference);

            return Math.Sqrt(sum);
        }

        /// <summary>
        /// Calculates the weighted Euclidean distance based on drift time, aligned mass, and aligned NET.
        /// </summary>
        /// <param name="x">Feature x.</param>
        /// <param name="y">Feature y.</param>
        /// <returns>Distance calculated as </returns>
		public double EuclideanDistance(UMCLight x, UMCLight y, double massWeight, double netWeight, double driftWeight)
        {
			double massDifference = Feature.ComputeMassPPMDifference(x.MassMonoisotopic, y.MassMonoisotopic);
            double netDifference = x.NET - y.NET;
            double driftDifference = x.DriftTime - y.DriftTime;
            double sum = (massDifference * massDifference)*massWeight +
                                     (netDifference * netDifference)*netDifference +
                                     (driftDifference * driftDifference) * driftWeight;

            return Math.Sqrt(sum);
        }
        #endregion
    }
}
