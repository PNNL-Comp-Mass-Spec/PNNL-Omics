using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Features
{
    /// <summary>
    /// Compares masses 
    /// </summary>
    public class AlignedMassComparer: IComparer<Feature>, IComparer<UMC>
    {
        #region IComparer<Feature> Members
        /// <summary>
        /// Compares two features based on their aligned monoisotopic mass.
        /// </summary>
        /// <param name="x">Feature to compare</param>
        /// <param name="y">Feature to compare</param>
        /// <returns>Integer value indicating its equality value between x and y.</returns>
        public int Compare(Feature x, Feature y)
        {
            return x.MassMonoisotopicAligned.CompareTo(y.MassMonoisotopicAligned);
        }
        #endregion

        #region IComparer<UMC> Members
        /// <summary>
        /// Compares the two UMC's based on their aligned monoisotopc mass.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(UMC x, UMC y)
        {
            return x.MassMonoisotopicAligned.CompareTo(y.MassMonoisotopicAligned);
        }
        #endregion
    }
}
