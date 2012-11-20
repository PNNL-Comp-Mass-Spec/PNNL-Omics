using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// MS Feature class that describes a raw or deisotoped feature.
	/// </summary>
    public class MSFeatureLight : FeatureLight, IComparable<MSFeatureLight>, IChildFeature<UMCLight>
	{
    
        #region AutoProperties
        /// <summary>
		/// The list of MSPeaks that make up the MSFeature.
        public List<Peak> MSPeakList { get; set; }        
		/// <summary>
		/// The UMC associated with this MS Feature.
		/// </summary>
        public UMCLight UMC { get; set; }
        /// <summary>
        /// Gets or sets the mass to charge ratio value.
        /// </summary>
        public double Mz { get; set; }
        /// <summary>
        /// Gets or sets the scan of the feature.
        /// </summary>
        public int Scan {get;set;}
        /// <summary>
        /// Gets or sets the average monoisotopic mass. 
        /// </summary>
        public double MassMonoisotopicAverage { get; set; }
        /// <summary>
        /// Gets or sets the most abundant monoisotopic mass from the isotopic distribution.
        /// </summary>
        public double MassMonoisotopicMostAbundant { get; set; }
        /// <summary>
        /// Gets or sets the list of potential MS/MS (MSn) spectra associated with this feature.
        /// </summary>
        public List<MSSpectra> MSnSpectra { get; set; }
		#endregion

        public override double MassMonoisotopicAligned
        {
            get
            {
                return MassMonoisotopic;
            }
            set
            {
                MassMonoisotopic = value;
            }
        }

		#region BaseData Members
		/// <summary>
		/// Clears the datatype and resets the raw values to their default values.
		/// </summary>
		public override void Clear()
		{
            MSnSpectra  = new List<MSSpectra>();
            MSPeakList  = new List<Peak>();
            MassMonoisotopicMostAbundant = 0;
            UMC         = null;
			base.Clear();
		}
		#endregion

		#region IComparable<MSFeature> Members
		/// <summary>
		/// Default Comparer used for the MSFeature class. Sorts by Monoisotopic Mass descending.
		/// </summary>
		public int CompareTo(MSFeatureLight other)
		{
			return other.MassMonoisotopic.CompareTo(this.MassMonoisotopic);
		}
		#endregion

        #region Comparers
        /// <summary>
        /// Compares the IMS Scan of two MS Features
        /// </summary>
        //TODO: Move to Feature.cs?
        public static Comparison<MSFeature> ScanIMSComparison = delegate(MSFeature x, MSFeature y)
        {
            return x.ScanIMS.CompareTo(y.ScanIMS);
        };
		#endregion


        #region IChildFeature<UMCLight> Members

        public void SetParentFeature(UMCLight parentFeature)
        {
            UMC   = parentFeature;
            UMCID = UMC.ID;
        }
        public UMCLight ParentFeature
        {
            get { return UMC; }
        }
        public int UMCID
        {
            get;
            set;
        }
        #endregion

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + ID.GetHashCode();
            hash = hash * 23 + this.GroupID.GetHashCode();
            hash = hash * 23 + this.MassMonoisotopicMostAbundant.GetHashCode();
            hash = hash * 23 + this.MassMonoisotopic.GetHashCode();
            hash = hash * 23 + this.RetentionTime.GetHashCode();

            return hash;
        }
    }
}
