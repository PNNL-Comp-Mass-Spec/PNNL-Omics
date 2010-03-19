using System;
using System.Collections.Generic;

using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Clustering
{
    public class UMCClusterer
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public UMCClusterer()
        {
        }

        #region Utility Methods
        /// <summary>
        /// Calculates the distances between every point.
        /// </summary>
        /// <param name="umcs">List of raw UMC's.</param>
        /// <param name="startPointNumber">Start index in the list of the input umc's.</param>
        /// <param name="pointNumber">Stop index in the list of the input umc's to stop calculating distances at.</param>
        /// <returns>List of distances.</returns>
        protected void CalculatePairwiseDistances(List<UMC> umcs, int startPointNumber, int pointNumber)
        {
            List<double> distances = new List<double>(pointNumber - startPointNumber);

            for (int pointIndex = startPointNumber; pointIndex <= pointNumber; pointIndex++)
            {
                int featureNumber = pointIndex - startPointNumber;

            }

        }
        #endregion

        #region Cluster Methods
        /// <summary>
        /// Clusters the umc data into UMC Clusters.
        /// </summary>
        /// <param name="umcs">UMC's to cluster.</param>        
        /// <returns></returns>
        public IList<UMCCluster> Cluster(IList<UMC> umcs, UMCClusterOptions options)
        {
            /// 
            /// We want to sort based on mass first.
            /// 
            List<UMCCluster> clusters   = new List<UMCCluster>();
            List<UMC> umcList           = new List<UMC>();
            umcList.AddRange(umcs);

            /// 
            /// Sort based on mass first...
            /// 
            umcList.Sort(0, umcs.Count, new AlignedMassComparer());

            /// 
            /// Now go through the features array and using the supplied mass and net tolerance,
			/// look for the situation where the mass difference between consecutive values is 
			/// greater than mass_tolerance.
            /// 
            int startPointNumber    = 0;
            int currentPointNumber  = 0;
            int pointNumber         = 0;
            int numberOfPoints      = umcs.Count;
            
            while (pointNumber < numberOfPoints - 1)
            {
                UMC currentUMC    = umcs[currentPointNumber];
                UMC nextUMC       = umcs[currentPointNumber + 1];

                double massDifference = ((nextUMC.MassMonoisotopicAligned - currentUMC.MassMonoisotopicAligned) * 1000000) / currentUMC.MassMonoisotopicAligned;
                if (massDifference > options.MassTolerance)
                {
                    if (startPointNumber == pointNumber)
                    {
                        
                    }
                    else
                    {
                        /// 
                        /// Now that we have found something that is not greater than the mass tolerance...
                        /// And now that we have not found a new point to construct a cluster from
                        /// We have to go back through and do the HSLC on the list of UMC's.
                        /// 
                        List<double> distances = CalculatePairwiseDistances(umcs, startPointNumber, pointNumber);

                    }
                }
                else
                {
                }
                  
            }

            return clusters;
        }
        #endregion
    }
}
