/*////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * 
 * Name:    Pairwise UMC Distance
 * File:    PairwiseUMCDistance.cs
 * Author:  Brian LaMarche 
 * Purpose: Links two UMC objects to each other by a specified distance.
 * Date:    05-19-2010
 * Revisions:
 *          05-19-2010 - BLL - Created class for clustering.
 *          08-02-2010 - BLL - Moved to PNNLOMICS.Data as a public class.
 ////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
using System;
using System.Collections.Generic;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Data
{          
    /// <summary>
    /// Holds the distance between two features and indices.
    /// </summary>
    public class PairwiseUMCDistance : IComparer<PairwiseUMCDistance>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the x feature.
        /// </summary>
        public UMC FeatureX { get; set; }
        /// <summary>
        /// Gets or sets the y feature.
        /// </summary>
        public UMC FeatureY { get; set; }
        /// <summary>
        /// Gets or sets the distance between the two features.
        /// </summary>
        public double Distance { get; set; }
        #endregion

        #region IComparer<PairwiseUMCDistance> Members
        /// <summary>
        /// Compares the distance between x and y.
        /// </summary>
        /// <param name="x">Feature x.</param>
        /// <param name="y">Feature y.</param>
        /// <returns>Returns an integer value determining if x is greater than, less than, or equal to y.</returns>
        public int Compare(PairwiseUMCDistance x, PairwiseUMCDistance y)
        {
            return x.Distance.CompareTo(y.Distance);
        }
        #endregion
    }    
}
