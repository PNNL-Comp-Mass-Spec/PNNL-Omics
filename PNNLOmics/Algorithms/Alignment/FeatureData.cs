using System;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// TODO: Create class comment block for FeatureData
    /// </summary>
    public class FeatureData
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of FeatureData with default values
        /// </summary>
        public FeatureData()
        {
            Clear();
        }

        /// <summary>
        /// Initializes a new instance of FeatureData with provided values
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="datasetIndex">Dataset index</param>
        /// <param name="mass">Mass</param>
        /// <param name="net">Net</param>
        public FeatureData(int index, int datasetIndex, double mass, double net)
        {
            Set(index, datasetIndex, mass, net);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the dataset index
        /// </summary>
        public int DatasetIndex { get; set; }

        /// <summary>
        /// Gets or sets the mass
        /// </summary>
        public double Mass { get; set; }

        /// <summary>
        /// Gets or sets the net
        /// </summary>
        public double Net { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears all values to their original setting
        /// </summary>
        public void Clear()
        {
            Index = -1;
            DatasetIndex = -1;
            Mass = 0.0;
            Net = 0.0;
        }

        /// <summary>
        /// TODO: Comment Set
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="datasetIndex">Dataset index</param>
        /// <param name="mass">Mass</param>
        /// <param name="net">Net</param>
        public void Set(int index, int datasetIndex, double mass, double net)
        {
            Index = index;
            DatasetIndex = datasetIndex;
            Mass = mass;
            Net = net;
        }
        #endregion

        #region Comparison Classes
        /// <summary>
        /// IComparer class for sorting FeatureData's by mass
        /// </summary>
        public class SortByMass : IComparer<FeatureData>
        {
            /// <summary>
            /// Compares two FeatureData classes based on mass
            /// </summary>
            /// <param name="a">First object</param>
            /// <param name="b">Second object</param>
            /// <returns>1 if (a greater than b), 0 if (a equals b), and -1 if (a less than b)</returns>
            public int Compare(FeatureData a, FeatureData b)
            {
                return a.Mass.CompareTo(b.Mass);
            }
        }
        #endregion
    }
}