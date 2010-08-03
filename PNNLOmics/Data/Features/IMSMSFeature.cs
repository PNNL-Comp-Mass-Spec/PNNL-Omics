using System;
using System.Collections.Generic;
using PNNLOmics.Algorithms.ConformationDetection.Data;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// IMS-MS Feature class.
	/// </summary>
	public class IMSMSFeature : UMC, IComparable<IMSMSFeature>
	{
		#region AutoProperties
		/// <summary>
		/// True if the IMS-MS Feature has a gap in the IMS dimension.
		/// </summary>
		public bool HasGap { get; set; }
		/// <summary>
		/// The IMS Scan where the IMS-MS Feature begins.
		/// </summary>
		public int ScanIMSStart { get; set; }
		/// <summary>
		/// The IMS Scan where the IMS-MS Feature ends.
		/// </summary>
		public int ScanIMSEnd { get; set; }
		/// <summary>
		/// The IMS Scan of the most abundant MS Feature associated with the IMS-MS Feature.
		/// </summary>
		public int ScanIMSOfMaxAbundance { get; set; }
		/// <summary>
		/// The monoisotopic mass of the MS Feature of the max IMS Scan of the IMS-MS Feature.
		/// </summary>
		public double MassOfScanIMSMax { get; set; }
		/// <summary>
		/// The Drift Time of an IMS-MS Feature is calculated using the associated Conformation object.
		/// </summary>
		public override float DriftTime
		{
			get	{ return this.Conformation.DriftTime; }
			set	{ this.Conformation.DriftTime = value; }
		}
		/// <summary>
		/// Conformation Fit Score associated with the Conformation of this IMS-MS Feature.
		/// </summary>
		public override double ConformationFitScore
		{
			get { return this.Conformation.FitScore; }
			set { this.Conformation.FitScore = value; }
		}
		/// <summary>
		/// The Conformation object associated with this IMS-MS Feature.
		/// </summary>
		public Conformation Conformation { get; set; }
		/// <summary>
		/// A List of IMS Scan numbers.
		/// </summary>
		public List<int> ScanIMSList { get; set; }
		/// <summary>
		/// A List of IMS Scans that are gaps in the IMS dimension for the IMS-MS Feature.
		/// </summary>
		public List<int> GapList { get; set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor for IMS-MS Feature with no arguments.
		/// </summary>
		public IMSMSFeature()
		{
			Clear();
		}

		/// <summary>
		/// This constructor will create a new IMS-MS Feature that is a replica of the given IMS-MS Feature.
		/// </summary>
		/// <param name="imsmsFeatureToCopy">The IMS-MS Feature to replicate.</param>
		public IMSMSFeature(IMSMSFeature imsmsFeatureToCopy)
		{
			this.MSFeatureList = new List<MSFeature>();
			this.ScanIMSList = new List<int>();
			Clear();

			this.ScanLC = imsmsFeatureToCopy.ScanLC;
			this.ChargeState = imsmsFeatureToCopy.ChargeState;
			this.AddMSFeatureList(imsmsFeatureToCopy.MSFeatureList);
		}
		#endregion

		#region BaseData Members
		/// <summary>
		/// Clears the datatype and resets the raw values to their default values.
		/// </summary>
		public override void Clear()
		{
			this.Conformation = new Conformation();
			this.GapList = new List<int>();
			this.ScanIMSList = new List<int>();

			base.Clear();

			this.ScanIMSStart = int.MaxValue;
			this.ScanIMSEnd = 0;
			this.MassOfScanIMSMax = 0;
			this.HasGap = false;
		}
		#endregion

		#region IComparable<IMSMSFeature> Members
		/// <summary>
		/// Default Comparer used for the IMSMSFeature class. Sorts by Median Monoisotopic Mass.
		/// </summary>
		public int CompareTo(IMSMSFeature other)
		{
			return this.MassMonoisotopicMedian.CompareTo(other.MassMonoisotopicMedian);
		}
		#endregion

		#region Comparers
		/// <summary>
		/// Compares the monoisotopic mass of the Maximum IMS Scan of two IMS-MS Features
		/// </summary>
		public static Comparison<IMSMSFeature> MassOfScanIMSMaxComparison = delegate(IMSMSFeature x, IMSMSFeature y)
		{
			return x.MassOfScanIMSMax.CompareTo(y.MassOfScanIMSMax);
		};

		/// <summary>
		/// Compares the LC Scan # then the Maximum IMS Scan # of two IMS-MS Features
		/// </summary>
		public static Comparison<IMSMSFeature> ScanLCAndScanIMSEndComparison = delegate(IMSMSFeature x, IMSMSFeature y)
		{
			if (x.ScanLC != y.ScanLC)
			{
				return x.ScanLC.CompareTo(y.ScanLC);
			}
			else
			{
				return x.ScanIMSEnd.CompareTo(y.ScanIMSEnd);
			}
		};

		/// <summary>
		/// Compares the charge state then the Minimum IMS Scan # of two IMS-MS Features
		/// </summary>
		public static Comparison<IMSMSFeature> ChargeAndScanIMSStartComparison = delegate(IMSMSFeature x, IMSMSFeature y)
		{
			if (x.ChargeState != y.ChargeState)
			{
				return x.ChargeState.CompareTo(y.ChargeState);
			}
			else
			{
				return x.ScanIMSStart.CompareTo(y.ScanIMSStart);
			}
		};
		#endregion

		#region Public Utility Functions
		/// <summary>
		/// Recalculates various properties of the IMS-MS Feature based on the List of MS Features that are associated with the IMS-MS Feature.
		/// </summary>
		public void Recalculate()
		{
			this.MassMonoisotopicMaximum = 0;
			this.MassMonoisotopicMinimum = double.MaxValue;
			this.ChargeMaximum = 0;
			this.AbundanceSum = 0;

			List<double> massMonoisotopicList = new List<double>();
			int abundanceMax = 0;

			List<MSFeature> msFeatureList = this.MSFeatureList;

			msFeatureList.Sort(MSFeature.ScanIMSComparison);
			this.MassOfScanIMSMax = msFeatureList[msFeatureList.Count - 1].MassMonoisotopic;
			this.ScanIMSStart = msFeatureList[0].ScanIMS;
			this.ScanIMSEnd = msFeatureList[msFeatureList.Count - 1].ScanIMS;

			msFeatureList.Sort(new Comparison<MSFeature>(Feature.MassComparison));
			foreach (MSFeature msFeature in msFeatureList)
			{
				if (msFeature.MassMonoisotopic > this.MassMonoisotopicMaximum)
				{
					this.MassMonoisotopicMaximum = msFeature.MassMonoisotopic;
				}
				if (msFeature.MassMonoisotopic < this.MassMonoisotopicMinimum)
				{
					this.MassMonoisotopicMinimum = msFeature.MassMonoisotopic;
				}
				if (msFeature.ChargeState > this.ChargeMaximum)
				{
					this.ChargeMaximum = msFeature.ChargeState;
				}

				massMonoisotopicList.Add(msFeature.MassMonoisotopic);
				this.AbundanceSum += msFeature.Abundance;

				if (msFeature.Abundance > abundanceMax)
				{
					abundanceMax = msFeature.Abundance;
					this.ScanIMSOfMaxAbundance = msFeature.ScanIMS;
					this.MassOfMaxAbundance = msFeature.MassMonoisotopic;
					this.DriftTime = msFeature.DriftTime;
					this.MZ = msFeature.MZ;
				}
			}

			if (massMonoisotopicList.Count % 2 == 1)
			{
				this.MassMonoisotopicMedian = massMonoisotopicList[massMonoisotopicList.Count / 2];
			}
			else
			{
				this.MassMonoisotopicMedian = 0.5 * (massMonoisotopicList[massMonoisotopicList.Count / 2 - 1] + massMonoisotopicList[massMonoisotopicList.Count / 2]);
			}

			this.AbundanceMaximum = abundanceMax;

			this.GapList.Clear();

			if (this.ScanIMSList.Count > 0)
			{
				int previousScanIMS = this.ScanIMSList[0];

				for (int i = 1; i < this.ScanIMSList.Count; i++)
				{
					int currentScanIMS = this.ScanIMSList[i];
					int scanIMSDifference = currentScanIMS - previousScanIMS;

					for (int j = 1; j < scanIMSDifference; j++)
					{
						this.GapList.Add(previousScanIMS + j);
					}

					previousScanIMS = currentScanIMS;
				}
			}
		}

		/// <summary>
		/// Recalculates various properties of the IMS-MS Feature based on the List of MS Features that are associated with the IMS-MS Feature.
		/// This overload uses a single MS Feature as a reference point. Usually, this MS Feature is the most recent MS Feature that has been added to the List of MS Features.
		/// </summary>
		/// <param name="msFeature">MS Feature object used as a reference for recalculating.</param>
		public void Recalculate(MSFeature msFeature)
		{
			if (msFeature.MassMonoisotopic > this.MassMonoisotopicMaximum)
			{
				this.MassMonoisotopicMaximum = msFeature.MassMonoisotopic;
			}
			if (msFeature.MassMonoisotopic < this.MassMonoisotopicMinimum)
			{
				this.MassMonoisotopicMinimum = msFeature.MassMonoisotopic;
			}
			if (msFeature.ChargeState > this.ChargeMaximum)
			{
				this.ChargeMaximum = msFeature.ChargeState;
			}

			List<double> massMonoisotopicList = new List<double>();
			int abundanceMax = 0;

			List<MSFeature> msFeatureList = this.MSFeatureList;

			msFeatureList.Sort(MSFeature.ScanIMSComparison);
			this.MassOfScanIMSMax = msFeatureList[msFeatureList.Count - 1].MassMonoisotopic;
			this.ScanIMSStart = msFeatureList[0].ScanIMS;
			this.ScanIMSEnd = msFeatureList[msFeatureList.Count - 1].ScanIMS;

			msFeatureList.Sort(new Comparison<MSFeature>(Feature.MassComparison));
			foreach (MSFeature msFeature1 in msFeatureList)
			{
				massMonoisotopicList.Add(msFeature1.MassMonoisotopic);
				this.AbundanceSum += msFeature1.Abundance;

				if (msFeature1.Abundance > abundanceMax)
				{
					abundanceMax = msFeature1.Abundance;
					this.ScanIMSOfMaxAbundance = msFeature1.ScanIMS;
					this.MassOfMaxAbundance = msFeature1.MassMonoisotopic;
					this.DriftTime = msFeature1.DriftTime;
					this.MZ = msFeature1.MZ;
				}
			}

			if (massMonoisotopicList.Count % 2 == 1)
			{
				this.MassMonoisotopicMedian = massMonoisotopicList[massMonoisotopicList.Count / 2];
			}
			else
			{
				this.MassMonoisotopicMedian = 0.5 * (massMonoisotopicList[massMonoisotopicList.Count / 2 - 1] + massMonoisotopicList[massMonoisotopicList.Count / 2]);
			}

			this.AbundanceMaximum = abundanceMax;

			this.GapList.Clear();

			if (this.ScanIMSList.Count > 0)
			{
				int previousScanIMS = this.ScanIMSList[0];

				for (int i = 1; i < this.ScanIMSList.Count; i++)
				{
					int currentScanIMS = this.ScanIMSList[i];
					int scanIMSDifference = currentScanIMS - previousScanIMS;

					for (int j = 1; j < scanIMSDifference; j++)
					{
						this.GapList.Add(previousScanIMS + j);
					}

					previousScanIMS = currentScanIMS;
				}
			}
		}

		/// <summary>
		/// Adds an MS Feature to the List of MS Features that are associated with the IMS-MS Feature.
		/// This function makes a call to Recalculate() so that the Properties values of the IMS-MS Feature are up-to-date.
		/// </summary>
		/// <param name="msFeature">MS Feature to associate with the IMS-MS Feature.</param>
		public void AddMSFeature(MSFeature msFeature)
		{
			this.MSFeatureList.Add(msFeature);
			this.ScanIMSList.Add(msFeature.ScanIMS);
			this.ScanIMSList.Sort();
			Recalculate(msFeature);
		}

		/// <summary>
		/// Adds a List of MS Features to the List of MS Features that are associated with the IMS-MS Feature.
		/// This function makes a call to Recalculate() so that the Properties values of the IMS-MS Feature are up-to-date.
		/// </summary>
		/// <param name="msFeatureList">List of MS Features to associate with the IMS-MS Feature.</param>
		public void AddMSFeatureList(List<MSFeature> msFeatureList)
		{
			foreach (MSFeature msFeature in msFeatureList)
			{
				this.MSFeatureList.Add(msFeature);
				this.ScanIMSList.Add(msFeature.ScanIMS);
				this.ScanIMSList.Sort();
				Recalculate(msFeature);
			}

			this.MSFeatureList.Sort(MSFeature.ScanIMSComparison);
		}

		/// <summary>
		/// Adds a List of MS Features to the List of MS Features that are associated with the IMS-MS Feature.
		/// This function does not makes a call to Recalculate().
		/// </summary>
		/// <param name="msFeatureList">List of MS Features to associate with the IMS-MS Feature.</param>
		public void AddMSFeatureListWithoutUpdate(List<MSFeature> msFeatureList)
		{
			this.MSFeatureList.AddRange(msFeatureList);
		}
		#endregion
	}
}
