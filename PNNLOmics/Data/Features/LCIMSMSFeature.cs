using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// LC-IMS-MS Feature class.
	/// </summary>
	public class LCIMSMSFeature : UMC, IComparable<LCIMSMSFeature>
	{
		#region Properties
		/// <summary>
		/// Drift Time of the maximum LC Scan of the LC-IMS-MS Feature.
		/// </summary>
		public float DriftTimeOfScanLCMax { get; set; }
		/// <summary>
		/// List of IMS-MS features associated with the LC-IMS-MS Feature.
		/// </summary>
		public List<IMSMSFeature> IMSMSFeatureList { get; set; }
		/// <summary>
		/// List of LC Scans associated with the LC-IMS-MS Feature.
		/// </summary>
		public List<int> ScanLCList { get; set; }
		/// <summary>
		/// List of gaos in the LC dimension.
		/// </summary>
		public List<int> GapLCList { get; set; }

		/// <summary>
		/// List of MS Features associated with the LC-IMS-MS feature.
		/// </summary>
		public override List<MSFeature> MSFeatureList
		{
			get
			{
				List<MSFeature> msFeatureList = new List<MSFeature>();

				foreach (IMSMSFeature imsmsFeature in this.IMSMSFeatureList)
				{
					msFeatureList.AddRange(imsmsFeature.MSFeatureList);
				}

				return msFeatureList;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor for LC-IMS-MS Feature with no arguments.
		/// </summary>
		public LCIMSMSFeature()
		{
			Clear();
		}
		#endregion

		#region BaseData Members
		/// <summary>
		/// Clears the datatype and resets the raw values to their default values.
		/// </summary>
		public override void Clear()
		{
			base.Clear();

			this.DriftTime = 0;
			this.IMSMSFeatureList = new List<IMSMSFeature>();
			this.ScanLCList = new List<int>();
			this.GapLCList = new List<int>();
		}
		#endregion

		#region IComparable<LCIMSMSFeature> Members
		/// <summary>
		/// Default Comparer used for the LCIMSMSFeature class. Sorts by the Monoisotopic Mass of the most abundant MSFeature.
		/// </summary>
		public int CompareTo(LCIMSMSFeature other)
		{
			return this.MassOfMaxAbundance.CompareTo(other.MassOfMaxAbundance);
		}
		#endregion

		#region Public Utility Functions
		/// <summary>
		/// Recalculates various properties of the LC-IMS-MS Feature based on the List of IMS-MS Features that are associated with the LC-IMS-MS Feature.
		/// </summary>
		public void Recalculate()
		{
			this.ChargeMaximum = 0;
			this.AbundanceMaximum = 0;
			this.ScanLCStart = int.MaxValue;
			this.ScanLCEnd = 0;
			this.AbundanceSum = 0;

			foreach (IMSMSFeature imsmsFeature in this.IMSMSFeatureList)
			{
				if (imsmsFeature.ChargeState > this.ChargeMaximum)
				{
					this.ChargeMaximum = imsmsFeature.ChargeState;
				}

				if (imsmsFeature.AbundanceMaximum > this.AbundanceMaximum)
				{
					this.AbundanceMaximum = imsmsFeature.AbundanceMaximum;
					this.ScanLCOfMaxAbundance = imsmsFeature.ScanLC;
					this.MZ = imsmsFeature.MZ;
					this.MassOfMaxAbundance = imsmsFeature.MassOfMaxAbundance;
					this.DriftTime = imsmsFeature.DriftTime;
				}

				if (imsmsFeature.ScanLC < this.ScanLCStart)
				{
					this.ScanLCStart = imsmsFeature.ScanLC;
				}
				if (imsmsFeature.ScanLC > this.ScanLCEnd)
				{
					this.ScanLCEnd = imsmsFeature.ScanLC;
					this.DriftTimeOfScanLCMax = imsmsFeature.DriftTime;
				}

				this.ScanLCList.Add(imsmsFeature.ScanLC);
				this.AbundanceSum += imsmsFeature.AbundanceSum;
			}
		}

		/// <summary>
		/// Recalculates various properties of the LC-IMS-MS Feature based on the List of IMS-MS Features that are associated with the LC-IMS-MS Feature.
		/// This overload uses a single IMS-MS Feature as a reference point. Usually, this IMS-MS Feature is the most recent IMS-MS Feature that has been added to the List of IMS-MS Features.
		/// </summary>
		/// <param name="imsmsFeature">IMS-MS Feature object used as a reference for recalculating.</param>
		public void Recalculate(IMSMSFeature imsmsFeature)
		{
			if (imsmsFeature.ChargeState > this.ChargeMaximum)
			{
				this.ChargeMaximum = imsmsFeature.ChargeState;
			}

			if (imsmsFeature.AbundanceMaximum > this.AbundanceMaximum)
			{
				this.AbundanceMaximum = imsmsFeature.AbundanceMaximum;
				this.ScanLCOfMaxAbundance = imsmsFeature.ScanLC;
				this.MZ = imsmsFeature.MZ;
				this.MassOfMaxAbundance = imsmsFeature.MassOfMaxAbundance;
				this.DriftTime = imsmsFeature.DriftTime;
			}

			if (imsmsFeature.ScanLC < this.ScanLCStart)
			{
				this.ScanLCStart = imsmsFeature.ScanLC;
			}
			if (imsmsFeature.ScanLC > this.ScanLCEnd)
			{
				this.ScanLCEnd = imsmsFeature.ScanLC;
				this.DriftTimeOfScanLCMax = imsmsFeature.DriftTime;
			}

			this.ScanLCList.Add(imsmsFeature.ScanLC);
			this.AbundanceSum += imsmsFeature.AbundanceSum;
		}

		/// <summary>
		/// Adds an IMS-MS Feature to the List of IMS-MS Features that are associated with the LC-IMS-MS Feature.
		/// This function makes a call to Recalculate() so that the Properties values of the LC-IMS-MS Feature are up-to-date.
		/// </summary>
		/// <param name="imsmsFeature">IMS-MS Feature to associate with the LC-IMS-MS Feature.</param>
		public void AddIMSMSFeature(IMSMSFeature imsmsFeature)
		{
			this.IMSMSFeatureList.Add(imsmsFeature);
			Recalculate(imsmsFeature);
		}

		/// <summary>
		/// Adds a List of MS Features to the List of MS Features that are associated with the IMS-MS Feature.
		/// This function makes a call to Recalculate() so that the Properties values of the IMS-MS Feature are up-to-date.
		/// </summary>
		/// <param name="msFeatureList">List of MS Features to associate with the IMS-MS Feature.</param>
		public void AddIMSMSFeatureList(List<IMSMSFeature> imsmsFeatureList)
		{
			foreach (IMSMSFeature imsmsFeature in imsmsFeatureList)
			{
				this.IMSMSFeatureList.Add(imsmsFeature);
				Recalculate(imsmsFeature);
			}
			this.IMSMSFeatureList.Sort(new Comparison<IMSMSFeature>(Feature.ScanLCComparison));
		}

		public void CalculateGapList()
		{
			this.ScanLCList.Sort();

			this.GapLCList.Clear();

			if (this.ScanLCList.Count > 0)
			{
				int previousScanLC = this.ScanLCList[0];

				for (int i = 1; i < this.ScanLCList.Count; i++)
				{
					int currentScanLC = this.ScanLCList[i];
					int scanLCDifference = currentScanLC - previousScanLC;

					for (int j = 1; j < scanLCDifference; j++)
					{
						this.GapLCList.Add(previousScanLC + j);
					}

					previousScanLC = currentScanLC;
				}
			}
		}
		#endregion
	}
}
