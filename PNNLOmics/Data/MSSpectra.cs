using System;
using System.Collections.Generic;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Contains MSn data for a given parent m/z.
    /// </summary>
    public class MSSpectra: BaseData,IDisposable
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
        /// Gets or sets the id.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Gets or sets the retention time.
        /// </summary>
        public double RetentionTime { get; set; }
        /// <summary>
        /// Gets or sets the scan value for this spectra.
        /// </summary>
        public int Scan
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets what group (or dataset) this spectra came from.
        /// </summary>
        public int GroupID
        {
            get;
            set;
        }
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
        /// Gets or sets processed peaks asspcoated with this spectra.
        /// </summary>
        public List<ProcessedPeak> PeaksProcessed
        {
            get;
            set;
        }
        /// <summary>
        /// The level to which the peaks have been processed
        /// </summary>
        public PeakProcessingLevel PeakProcessingLevel
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
        /// Gets or sets the parent spectra if MSLevel > 2.
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
        /// Gets or sets the total ion current.
        /// </summary>
        public double TotalIonCurrent
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
        /// <summary>
        /// Gets or sets the more accurate parent precursor M/Z for this MSn spectra.
        /// </summary>
        public ProcessedPeak PrecursorPeak
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
            MSLevel             = CONST_DEFAULT_MS_LEVEL;
            CollisionType       = CollisionType.Other;
            Scan                = 0;
            TotalIonCurrent     = -1;
            PrecursorMZ         = 0;
            GroupID             = -1;
            ChargeState         = -1;
            ID                  = -1;
            Peaks               = new List<XYData>();
            PeaksProcessed      = new List<ProcessedPeak>();
            PrecursorPeak       = new ProcessedPeak();
           
            PeakProcessingLevel = Data.PeakProcessingLevel.None;
        }

        #region Overriden Base Methods
        /// <summary>
        /// Returns a basic string representation of the cluster.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("ID = {0} Scan = {1} Precursor = {2}  Charge {3} Group = {4}",
                                ID,
                                Scan,
                                PrecursorMZ,
                                ChargeState,
                                GroupID);
            
        }
        /// <summary>
        /// Compares two objects' values.
        /// </summary>
        /// <param name="obj">Other to compare with.</param>
        /// <returns>True if values are the same, false if not.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            MSSpectra other = obj as MSSpectra;
            if (other == null)
                return false;

            if (!this.GroupID.Equals(other.GroupID))
            {
                return false;
            }
            if (!MSLevel.Equals(other.MSLevel))
            {
                return false;
            }
            if (!this.ChargeState.Equals(other.ChargeState))
            {
                return false;
            }
            if (!this.PrecursorMZ.Equals(other.PrecursorMZ))
            {
                return false;
            }
            if (!this.RetentionTime.Equals(other.RetentionTime))
            {
                return false;
            }
            if (!this.Scan.Equals(other.Scan))
            {
                return false;
            }
            if (!this.TotalIonCurrent.Equals(other.TotalIonCurrent))
            {
                return false;
            }
            if (!this.CollisionType.Equals(other.CollisionType))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Generates a hash code.
        /// </summary>
        /// <returns>Hash code based on stored data.</returns>
        public override int GetHashCode()
        {
            int hashCode =
                PrecursorMZ.GetHashCode() ^
                ChargeState.GetHashCode() ^
                Scan.GetHashCode() ^
                ID.GetHashCode() ^
                GroupID.GetHashCode() ^
                TotalIonCurrent.GetHashCode() ^
                RetentionTime.GetHashCode();
            return hashCode;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Clear();
            Peaks = null;
            PeaksProcessed = null;
            PrecursorPeak = null;

            if (ParentSpectra != null)
            {
                ParentSpectra.Clear();
                ParentSpectra = null;
            }

            if (ChildSpectra != null)
            {
                foreach (MSSpectra spectra in ChildSpectra)
                {
                    spectra.Clear();
                }
                ChildSpectra = null;
            }
        }

        #endregion
    }
}
