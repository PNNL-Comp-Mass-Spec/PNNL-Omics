using System;
using System.Collections.Generic;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureFinding
{
	/// <summary>
	/// Class that represents a grouping of MSFeatures for a single Mass Range for a single LC-MS Feature.
	/// </summary>
	/// <remarks>
	/// This class is used to support Dalton Correction for LC-MS Features.
	/// </remarks>
	public class UniqueMass : BaseData
	{
		#region AutoProperties
		/// <summary>
		/// Mass value associated with this object.
		/// </summary>
		public double Mass { get; set; }
		/// <summary>
		/// List of MS Features associated with this object.
		/// </summary>
		public List<MSFeature> MSFeatureList { get; set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor for UniqueMass.
		/// </summary>
		public UniqueMass()
		{
			Clear();
		}
		#endregion

		#region BaseData Members
		public override void Clear()
		{
			this.Mass = 0;
			this.MSFeatureList = new List<MSFeature>();
		}
		#endregion
	}
}
