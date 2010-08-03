using System;
using System.Collections.Generic;
using PNNLOmics.Algorithms.FeatureFinding;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// LC-MS Feature class.
	/// </summary>
	public class LCMSFeature : UMC, IComparable<LCMSFeature>
	{
		/// <summary>
		/// A list of the UniqueMass class.
		/// </summary>
		public List<UniqueMass> MassList { get; set; }

		#region Constructors
		/// <summary>
		/// Basic constructor. Used if no Dalton Correction is being used.
		/// </summary>
		public LCMSFeature()
		{
			Clear();
			CreateInitialMassList();
		}
		/// <summary>
		/// Constructor to be used of Dalton Correction is being used.
		/// </summary>
		/// <param name="daCorrectionMax">The maximum Dalton Correction to be applied.</param>
		public LCMSFeature(int daCorrectionMax)
		{
			this.MassList = new List<UniqueMass>();
			for (int i = 0; i < (daCorrectionMax * 2) + 1; i++)
			{
				UniqueMass uniqueMass = new UniqueMass();
				this.MassList.Add(uniqueMass);
			}

			Clear();
			this.DaltonCorrectionMax = daCorrectionMax;
			CreateInitialMassList();
		}
		#endregion

		#region BaseData Members
		/// <summary>
		/// Clears the datatype and resets the raw values to their default values.
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			foreach (UniqueMass uniqueMass in this.MassList)
			{
				uniqueMass.Clear();
			}
		}
		#endregion

		#region IComparable<LCMSFeature> Members
		/// <summary>
		/// Default Comparer used for the LCMSFeature class. Sorts by Maximum Monoisotopic Mass.
		/// </summary>
		public int CompareTo(LCMSFeature other)
		{
			return this.MassMonoisotopicMaximum.CompareTo(other.MassMonoisotopicMaximum);
		}
		#endregion

		#region Public Utility Functions
		/// <summary>
		/// Recalculates various properties of the LC-MS Feature based on the List of MS Features that are associated with the LC-MS Feature.
		/// </summary>
		public void Recalculate(MSFeature msFeature)
		{
			if (msFeature.MassMonoisotopic > this.MassMonoisotopicMaximum)
			{
				this.MassMonoisotopicMaximum = msFeature.MassMonoisotopic;
			}
			if (msFeature.ScanLC < this.ScanLCStart)
			{
				this.ScanLCStart = msFeature.ScanLC;
			}
			if (msFeature.ScanLC > this.ScanLCEnd)
			{
				this.ScanLCEnd = msFeature.ScanLC;
			}
			if (msFeature.ChargeState > this.ChargeMaximum)
			{
				this.ChargeMaximum = msFeature.ChargeState;
			}
			if (msFeature.Abundance > this.AbundanceMaximum)
			{
				this.AbundanceMaximum = msFeature.Abundance;
				this.MassOfMaxAbundance = msFeature.MassMonoisotopic;
				this.ChargeState = msFeature.ChargeState;
				this.MZ = msFeature.MZ;
				this.ScanLCOfMaxAbundance = msFeature.ScanLC;
			}
		}

		/// <summary>
		/// Adds an MS Feature to the List of MS Features that are associated with the LC-MS Feature.
		/// This function makes a call to Recalculate() so that the Properties values of the LC-MS Feature are up-to-date.
		/// </summary>
		/// <param name="msFeature">MS Feature to associate with the LC-MS Feature.</param>
		public void AddMSFeature(MSFeature msFeature)
		{
			this.MSFeatureList.Add(msFeature);
			Recalculate(msFeature);
		}

		/// <summary>
		/// Adds a List of MS Features to the List of MS Features that are associated with the LC-MS Feature.
		/// This function makes a call to Recalculate() so that the Properties values of the LC-MS Feature are up-to-date.
		/// </summary>
		/// <param name="msFeatureList">List of MS Features to associate with the LC-MS Feature.</param>
		public void AddMSFeatureList(List<MSFeature> msFeatureList)
		{
			this.MSFeatureList.AddRange(msFeatureList);
			foreach (MSFeature msFeature in msFeatureList)
			{
				Recalculate(msFeature);
			}
		}
		#endregion

		#region Private Utility Functions
		/// <summary>
		/// Creates the intial List of the UniqueMass class. The size of the List os based on the maximum Dalton Correction.
		/// </summary>
		private void CreateInitialMassList()
		{
			this.MassList = new List<UniqueMass>();

			for (int i = 0; i < (this.DaltonCorrectionMax * 2) + 1; i++)
			{
				UniqueMass uniqueMass = new UniqueMass();
				this.MassList.Add(uniqueMass);
			}
		}
		#endregion
	}
}
