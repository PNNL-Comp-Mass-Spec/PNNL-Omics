using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// Class that represents LC-MS, IMS-MS, LC-IMS-MS, etc. type data.
	/// </summary>
	/// <remarks>UMC stands for Unique Mass Class - see Advances in Proteomics Data Analysis and Display Using An Accurate Mass and Time Tag Approach in Mass Spectrometry Reviews, 2006. Zimmer et. al.</remarks>
	public class UMC : Feature, IComparable<UMC>
	{
		#region AutoProperties and Properties
		/// <summary>
		/// True if the the UMC should be removed from the working List of UMCs.
		/// </summary>
		public bool Remove { get; set; }
		/// <summary>
		/// True if the the UMC has been corrected using Dalton Error Correction.
		/// </summary>
		public bool DaError { get; set; }
		/// <summary>
		/// The maximum charge state of the UMC.
		/// </summary>
        public int ChargeMaximum { get; set; }
        /// <summary>
        /// The minimum charge state of the UMC.
        /// </summary>
        public int ChargeMinimum { get; set; }
        /// <summary>
		/// The maximum abundance of the UMC.
		/// </summary>
		public int AbundanceMaximum { get; set; }
		/// <summary>
		/// The sum of the abundance of the UMC.
		/// </summary>
		public int AbundanceSum { get; set; }
		/// <summary>
		/// The maximum Dalton Correction to be applied to the UMC.
		/// </summary>
		public int DaltonCorrectionMax { get; set; }
		/// <summary>
		/// The index of where the Conformation of the UMC occurred in respect to the rest of the Conformations.
		/// </summary>
		public int ConformationIndex { get; set; }
		/// <summary>
		/// The fit score determined by the Conformation Detection algorithm.
		/// </summary>
		public virtual double ConformationFitScore { get; set; }
		/// <summary>
		/// The LC Scan where the UMC begins.
		/// </summary>
		public int ScanLCStart { get; set; }
		/// <summary>
		/// The LC Scan where the UMC ends. 
		/// </summary>
		public int ScanLCEnd { get; set; }
		/// <summary>
		/// The LC Scan that contains the most abundant MS Feature associated with the UMC.
		/// </summary>
		public int ScanLCOfMaxAbundance { get; set; }
		/// <summary>
		/// The LC Scan that contains the most abundant MS Feature associated with the UMC.
		/// </summary>
		public override int ScanLC
		{
			get { return this.ScanLCOfMaxAbundance; }
			set { this.ScanLCOfMaxAbundance = value; }
		}
		/// <summary>
		/// The minimum monoisotopic mass of the UMC.
		/// </summary>
		public double MassMonoisotopicMinimum { get; set; }
		/// <summary>
		/// The maximum monoisotopic mass of the UMC.
		/// </summary>
		public double MassMonoisotopicMaximum { get; set; }
		/// <summary>
		/// The average monoisotopic mass of the UMC.
		/// </summary>
		public double MassMonoisotopicAverage { get; set; }
		/// <summary>
		/// The median monoisotopic mass of the UMC.
		/// </summary>
		public double MassMonoisotopicMedian { get; set; }
		/// <summary>
		/// The standard deviation of the monoisotopic mass of the UMC.
		/// </summary>
		public double MassMonoisotopicStandardDeviation { get; set; }
		/// <summary>
		/// The monoisotopic mass of the most abundant MS Feature associated with the UMC.
		/// </summary>
		public double MassOfMaxAbundance { get; set; }
		/// <summary>
		/// The List of MS Features associated with the UMC.
		/// </summary>
		public virtual List<MSFeature> MSFeatureList { get; set; }
		/// <summary>
		/// The UMC Cluster that is associated with the UMC.
		/// </summary>
		public UMCCluster UmcCluster { get; set; }

        //TODO: figure out whether or not to include these additional properties:
        public double FitScoreAverage { get; set; }
		#endregion

		#region BaseData Members
		/// <summary>
		/// Clears the datatype and resets the raw values to their default values.
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			this.Remove = false;
			this.ConformationIndex = -1;
			this.ChargeMaximum = 0;
			this.AbundanceMaximum = 0;
			this.AbundanceSum = 0;
			this.DaltonCorrectionMax = 0;
			this.MassMonoisotopicMinimum = double.MaxValue;
			this.MassMonoisotopicMaximum = 0;
			this.MassMonoisotopicAverage = 0;
			this.MassMonoisotopicMedian = 0;
			this.MassMonoisotopicStandardDeviation = 0;
			this.MassOfMaxAbundance = double.NaN;
			this.ScanLCStart = int.MaxValue;
			this.ScanLCEnd = 0;
			this.ScanLCOfMaxAbundance = 0;
			this.MSFeatureList = new List<MSFeature>();
			this.UmcCluster = null;
		}
		#endregion

		#region IComparable<LCFeature> Members
		/// <summary>
		/// Default Comparer used for the LCFeature class. Sorts by Monoisotopic Mass.
		/// </summary>
		public int CompareTo(UMC other)
		{
			return this.MassMonoisotopic.CompareTo(other.MassMonoisotopic);
		}

		#endregion

		#region Comparers
		/// <summary>
		/// Compares the first LC Scan of two UMCS
		/// </summary>
		public static Comparison<UMC> ScanLCStartComparison = delegate(UMC x, UMC y)
		{
            if (x == null || y == null)
            {
                throw new NullReferenceException("The UMC was null for the comparison.");
            }

			return x.ScanLCStart.CompareTo(y.ScanLCStart);
		};
		/// <summary>
		/// Compares the last LC Scan of two UMCS
		/// </summary>
		public static Comparison<UMC> ScanLCEndComparison = delegate(UMC x, UMC y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("The UMC was null for the comparison.");
            }
			return x.ScanLCEnd.CompareTo(y.ScanLCEnd);
		};
		/// <summary>
		/// Compares the summed abundance of two UMCS
		/// </summary>
		public static Comparison<UMC> AbundanceSumComparison = delegate(UMC x, UMC y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("The UMC was null for the comparison.");
            }
			return x.AbundanceSum.CompareTo(y.AbundanceSum);
		};
		/// <summary>
		/// Compares the maximum abundance of two UMCS
		/// </summary>
		public static Comparison<UMC> AbundanceMaximumComparison = delegate(UMC x, UMC y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("The UMC was null for the comparison.");
            }
			return x.AbundanceMaximum.CompareTo(y.AbundanceMaximum);
		};
		/// <summary>
		/// Compares the maximum charge state of two UMCS
		/// </summary>
		public static Comparison<UMC> ChargeMaximumComparison = delegate(UMC x, UMC y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("The UMC was null for the comparison.");
            }
			return x.ChargeMaximum.CompareTo(y.ChargeMaximum);
		};
		/// <summary>
		/// Compares the LC Scan that contains the most abundant MS Feature of two UMCS
		/// </summary>
		public static Comparison<UMC> ScanLCOfMaxAbundanceComparison = delegate(UMC x, UMC y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("The UMC was null for the comparison.");
            }
			return x.ScanLCOfMaxAbundance.CompareTo(y.ScanLCOfMaxAbundance);
		};
		/// <summary>
		/// Compares the maximum monoisotopic mass of two UMCS
		/// </summary>
		public static Comparison<UMC> MassMaxComparison = delegate(UMC x, UMC y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("The UMC was null for the comparison.");
            }
			return x.MassMonoisotopicMaximum.CompareTo(y.MassMonoisotopicMaximum);
		};
		/// <summary>
		/// Compares the minimum monoisotopic mass of two UMCS
		/// </summary>
		public static Comparison<UMC> MassMinComparison = delegate(UMC x, UMC y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("The UMC was null for the comparison.");
            }
			return x.MassMonoisotopicMinimum.CompareTo(y.MassMonoisotopicMinimum);
		};
		/// <summary>
		/// Compares the monoisotopic mass of the most abundant MS Features associated with two UMCS
		/// </summary>
		public static Comparison<UMC> MassofMaxAbundanceComparison = delegate(UMC x, UMC y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("The UMC was null for the comparison.");
            }
			return x.MassOfMaxAbundance.CompareTo(y.MassOfMaxAbundance);
		};
		/// <summary>
		/// Compares the first LC Scan then the monoisotopic mass of two UMCS
		/// </summary>
		public static Comparison<UMC> ScanLCStartAndMassComparison = delegate(UMC x, UMC y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("The UMC was null for the comparison.");
            }

			if (x.ScanLCStart != y.ScanLCStart)
			{
				return x.ScanLCStart.CompareTo(y.ScanLCStart);
			}
			else
			{
				return x.MassMonoisotopicMedian.CompareTo(y.MassMonoisotopicMedian);
			}
		};
		/// <summary>
		/// Compares the representative LC Scan and the median monoisotopic mass of two UMCS
		/// </summary>
		public static Comparison<UMC> ScanLCAndMedianMassComparison = delegate(UMC x, UMC y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("The UMC was null for the comparison.");
            }
			if (x.ScanLC != y.ScanLC)
			{
				return x.ScanLC.CompareTo(y.ScanLC);
			}
			else
			{
				return x.MassMonoisotopicMedian.CompareTo(y.MassMonoisotopicMedian);
			}
		};
		/// <summary>
		/// Compares the representative LC Scan and the monoisotopic mass of the most abundant MS Feature of two UMCS
		/// </summary>
		public static Comparison<UMC> ScanLCAndMassOfMaxAbundanceComparison = delegate(UMC x, UMC y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("The UMC was null for the comparison.");
            }
			if (x.ScanLC != y.ScanLC)
			{
				return x.ScanLC.CompareTo(y.ScanLC);
			}
			else
			{
				return x.MassOfMaxAbundance.CompareTo(y.MassOfMaxAbundance);
			}
		};
		#endregion

        /// <summary>
        /// Returns a string representation of this object based on its aligned
        /// data values and feature ID.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("ID: {0} Mono-Mass: {1} NET {2} Drift {3}",
                                    ID, 
                                    MassMonoisotopicAligned,
                                    NETAligned, 
                                    DriftTime);
        }
	}
}
