using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// MS Feature class that describes a raw or deisotoped feature.
	/// </summary>
    public class MSFeatureLight : FeatureLight, IComparable<MSFeatureLight>
	{
        //TODO: WHO?  Fix the names of the properties and the comments.  Gets or sets needs to be in each.
        //TODO: How do we drill back to the MS/MS data.  The MSPeak data does not accurately do this.  Abundance mass and mono does not capture this.

        #region AutoProperties
        /// <summary>
		/// The list of MSPeaks that make up the MSFeature.
        public List<Peak> MSPeakList { get; set; }        
		/// <summary>
		/// The UMC associated with this MS Feature.
		/// </summary>
		public UMCLight UMC { get; set; }
		#endregion

		#region BaseData Members
		/// <summary>
		/// Clears the datatype and resets the raw values to their default values.
		/// </summary>
		public override void Clear()
		{
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
	}
}
