using System;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// TODO: Create class comment block for MassTimeFeature
    /// </summary>
    public class MassTimeFeature
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of MassTimeFeature
        /// </summary>
        public MassTimeFeature()
        {
            Clear();
        }

        /// <summary>
        /// Initializes a new instance of MassTimeFeature with the values of a
        /// provided MassTimeFeature class
        /// </summary>
        /// <param name="massTimeFeatureToCopy">Object to copy</param>
        public MassTimeFeature(MassTimeFeature massTimeFeatureToCopy)
        {
            Clear();
            MonoisotopicMass = massTimeFeatureToCopy.MonoisotopicMass;
            CalibratedMonoisotopicMass = massTimeFeatureToCopy.CalibratedMonoisotopicMass;
            OriginalMonoisotopicMass = massTimeFeatureToCopy.OriginalMonoisotopicMass;
            MOverZ = massTimeFeatureToCopy.MOverZ;
            Net = massTimeFeatureToCopy.Net;
            Abundance = massTimeFeatureToCopy.Abundance;
            AlignedNet = massTimeFeatureToCopy.AlignedNet;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the monoisotopic mass
        /// </summary>
        public double MonoisotopicMass { get; set; }

        /// <summary>
        /// Gets or sets the calibrated monoisotopic mass
        /// </summary>
        public double CalibratedMonoisotopicMass { get; set; }

        /// <summary>
        /// Gets or sets the original monoisotopic mass
        /// </summary>
        public double OriginalMonoisotopicMass { get; set; }

        /// <summary>
        /// Gets or sets the m over z value
        /// </summary>
        public double MOverZ { get; set; }

        /// <summary>
        /// Gets or set the net
        /// </summary>
        public double Net { get; set; }

        /// <summary>
        /// Gets or sets the abundance
        /// </summary>
        public double Abundance { get; set; }

        /// <summary>
        /// Gets or sets the aligned net
        /// </summary>
        public double AlignedNet { get; set; }

        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        public int ID { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears all values to their original setting
        /// </summary>
        public void Clear()
        {
            MonoisotopicMass = 0.0;
            CalibratedMonoisotopicMass = 0.0;
            OriginalMonoisotopicMass = 0.0;
            MOverZ = 0.0;
            Net = 0.0;
            Abundance = 0.0;
            AlignedNet = -1.0;
            ID = -1;
        }
        #endregion

        #region Comparison Classes
        /// <summary>
        /// IComparer class for sorting MassTimeFeature's by net
        /// </summary>
        public class SortByMass : IComparer<MassTimeFeature>
        {
            /// <summary>
            /// Compares two MassTimeFeature classes based on mass
            /// </summary>
            /// <param name="a">First object</param>
            /// <param name="b">Second object</param>
            /// <returns>1 if (a greater than b), 0 if (a equals b), and -1 if (a less than b)</returns>
            public int Compare(MassTimeFeature a, MassTimeFeature b)
            {
                return a.MonoisotopicMass.CompareTo(b.MonoisotopicMass);
            }
        }
        #endregion
    }
}