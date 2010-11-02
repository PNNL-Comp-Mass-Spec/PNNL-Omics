using System;
using System.Collections.Generic;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Contains MSn data for a given parent m/z.
    /// </summary>
    public class MSSpectra: BaseData
    {
        /// <summary>
        /// The default MSn level (MS/MS).
        /// </summary>
        public const int CONST_DEFAULT_MS_LEVEL = 2;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MSSpectra ()
        {
            Clear();
        }

        #region Properties
        /// <summary>
        /// Gets or sets the retention time.
        /// </summary>
        public double RetentionTime { get; set; }
        /// <summary>
        /// Gets or sets the charge state for this spectra.
        /// </summary>
        public int ChargeState { get; set; }
        /// <summary>
        /// Gets or sets the MS Level.
        /// </summary>
        public int MSLevel
        {
            get; 
            set; 
        }
        /// <summary>
        /// Gets or sets the spectra for this MS level as x,y data points.
        /// </summary>
        public List<XYData> Peaks
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets any n + 1 level MSn child spectra.
        /// </summary>
        public List<MSSpectra> ChildSpectra
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the parent spectra if MSLevel > 1.
        /// </summary>
        public MSSpectra ParentSpectra { get; set; }
        /// <summary>
        /// Gets or sets the collision type.
        /// </summary>
        public CollisionType CollisionType
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the parent precursor M/Z for this MSn spectra.
        /// </summary>
        public double PrecursorMZ
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Resets the data to it's default state.
        /// </summary>
        public override void  Clear()
        {
            MSLevel         = CONST_DEFAULT_MS_LEVEL;
            CollisionType   = CollisionType.Other;
        }
    }
}
