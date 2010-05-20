using System;
using System.Collections.Generic;

using PNNLOmics.Data;
using PNNLOmics.Algorithms;
using PNNLOmics.Data.Features;


namespace PNNLOmics.Algorithms.FeatureClustering.Data
{
    /// <summary>
    /// Parameters for the single linkage UMC Clustering Algorithm.
    /// </summary>
    public class UMCSingleLinkageClustererParameters
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
        public UMCSingleLinkageClustererParameters()
        {
            Clear();
        }

        #region Properties
        /// <summary>
        /// Gets or sets the tolerance values for the clustering algorithm.
        /// </summary>
        public UMCSingleLinkageClustererTolerances Tolerances
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
            Tolerances                      = new UMCSingleLinkageClustererTolerances();
            OnlyClusterSameChargeStates     = CONST_DEFAULT_ONLY_CLUSTER_SAME_CHARGE_STATES;
            DistanceFunction                = new DistanceFunction(EuclideanDistance);
        }

        #region Distance Functions
        /// <summary>
        /// Calculates the Euclidean distance based on drift time, aligned mass, and aligned NET.
        /// </summary>
        /// <param name="x">Feature x.</param>
        /// <param name="y">Feature y.</param>
        /// <returns>Distance calculated as </returns>
        public double EuclideanDistance(UMC x, UMC y)
        {
            double massDifference = UMC.ComputeMassPPMDifference(x.MassMonoisotopicAligned, y.MassMonoisotopicAligned);
            double netDifference = x.NETAligned - y.NETAligned;
            double driftDifference = x.DriftTime - y.DriftTime;
            double sum = (massDifference * massDifference) + (netDifference * netDifference) + (driftDifference * driftDifference);

            return Math.Sqrt(sum);
        }
        #endregion
    }
}
