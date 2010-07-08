using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// MS Feature class.
	/// </summary>
	public class MSFeature : Feature, IComparable<MSFeature>
	{
		#region AutoProperties
		/// <summary>
		/// The index location (row #) of the MSFeature in the input file.
		/// </summary>
		public int IndexInFile { get; set; }
		/// <summary>
		/// The monoisotopic abundance of the MSFeature.
		/// </summary>
		public int AbundanceMono { get; set; }
		/// <summary>
		/// The monoisotopic abundance plus2 of the MSFeature.
		/// </summary>
		public int AbundancePlus2 { get; set; }
		/// <summary>
		/// The IMS Scan of the MSFeature.
		/// </summary>
		public int ScanIMS { get; set; }
		/// <summary>
		/// The magnitude of Da Correction of the MSFeature.
		/// </summary>
		public int CorrectedValue { get; set; }
		/// <summary>
		/// The mass of the most abundant Peak of the MS Feature.
		/// </summary>
		public double MassMostAbundant { get; set; }
		/// <summary>
		/// The fit value of the MSFeature.
		/// </summary>
		public float Fit { get; set; }
		/// <summary>
		/// The Full-width-half-max value of the MSFeature.
		/// </summary>
		public float Fwhm { get; set; }
		/// <summary>
		/// The Signal-to-noise ratio of the MSFeature.
		/// </summary>
		public float SignalNoise { get; set; }
		/// <summary>
		/// The Original Intensity of the MSFeature.
		/// </summary>
		public float IntensityOriginal { get; set; }
		/// <summary>
		/// The Original TIA Intensity of the MSFeature.
		/// </summary>
		public float IntensityOriginalTIA { get; set; }
		/// <summary>
		/// The List of MSPeaks that make up the MSFeature.
		/// </summary>
		public List<MSPeak> MSPeakList { get; set; }
		/// <summary>
		/// The UMC associated with this MS Feature.
		/// </summary>
		public UMC UMC { get; set; }
		#endregion

		#region BaseData Members
		/// <summary>
		/// Clears the datatype and resets the raw values to their default values.
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			this.Suspicious = false;
			this.IndexInFile = -1;
			this.AbundanceMono = 0;
			this.AbundancePlus2 = 0;
			this.IntensityOriginal = 0;
			this.IntensityOriginalTIA = 0;
			this.CorrectedValue = 0;
			this.ScanIMS = CONST_DEFAULT_SCAN_VALUE;
			this.Fit = float.NaN;
			this.Fwhm = float.NaN;
			this.SignalNoise = float.NaN;
			this.MSPeakList = new List<MSPeak>();
		}
		#endregion

		#region IComparable<MSFeature> Members
		/// <summary>
		/// Default Comparer used for the MSFeature class. Sorts by Monoisotopic Mass descending.
		/// </summary>
		public int CompareTo(MSFeature other)
		{
			return other.MassMonoisotopic.CompareTo(this.MassMonoisotopic);
		}
		#endregion

		#region Comparers
		/// <summary>
		/// Compares the IMS Scan of two MS Features
		/// </summary>
		public static Comparison<MSFeature> ScanIMSComparison = delegate(MSFeature x, MSFeature y)
		{
			return x.ScanIMS.CompareTo(y.ScanIMS);
		};
		#endregion
	}
}
