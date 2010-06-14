using System;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// TODO: Create class comment block for FeatureMatch
    /// </summary>
    public class FeatureMatch
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of FeatureMatch
        /// </summary>
        public FeatureMatch()
        {
            Clear();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or set the ppm mass error
        /// </summary>
        public double PPMMassError { get; set; }

        /// <summary>
        /// Gets or sets the net error
        /// </summary>
        public double NetError { get; set; }

        /// <summary>
        /// Gets or sets the first feature index
        /// </summary>
        public int FeatureIndexA { get; set; }

        /// <summary>
        /// Gets or sets the second feature index
        /// </summary>
        public int FeatureIndexB { get; set; }

        /// <summary>
        /// Gets or sets the first net
        /// </summary>
        public double NetA { get; set; }

        /// <summary>
        /// Gets or sets the second net
        /// </summary>
        public double NetB { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears all values back to their original setting
        /// </summary>
        public void Clear()
        {
            PPMMassError = 0.0;
            NetError = 0.0;
            FeatureIndexA = -1;
            FeatureIndexB = -1;
            NetA = -1.0;
            NetB = -1.0;
        }
        #endregion

        #region Comparison Classes
        /// <summary>
        /// IComparer class for sorting FeatureMatch's by net
        /// </summary>
        public class SortByNet : IComparer<FeatureMatch>
        {
            /// <summary>
            /// Compares two FeatureMatch classes based on net
            /// </summary>
            /// <param name="a">First object</param>
            /// <param name="b">Second object</param>
            /// <returns>1 if (a greater than b), 0 if (a equals b), and -1 if (a less than b)</returns>
            public int Compare(FeatureMatch a, FeatureMatch b)
            {
                return a.NetA.CompareTo(b.NetA);
            }
        }
        #endregion
    }
}