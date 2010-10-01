using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// Basic feature class
	/// </summary>
	public class FeatureLight: BaseData
	{
		public FeatureLight()
		{
			Clear();
		}
	
		public long		Abundance			{ get; set; }
		public int		ID					{ get; set; }
		public double	MassMonoisotopic	{ get; set; }
		public double	NET					{ get; set; }
		public double	DriftTime			{ get; set; }		
		public int		ChargeState			{ get; set; }
		
		public override void Clear()
		{
			this.Abundance = 0;
			this.ChargeState = 0;
			this.DriftTime = 0;
			this.ID = -1;
			this.MassMonoisotopic = 0;
			this.NET = 0;			
		}

		/// <summary>
		/// Compares the aligned monoisotopic mass of two Features
		/// </summary>
		public static Comparison<FeatureLight> MassComparison = delegate(FeatureLight x, FeatureLight y)
		{
			return x.MassMonoisotopic.CompareTo(y.MassMonoisotopic);
		};

		#region Overriden Base Methods
		/// <summary>
		/// Returns a basic string representation of the cluster.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "Feature Light ID = " + ID.ToString() +
					" Mono Mass = " + MassMonoisotopic.ToString() +
					" NET = " + NET.ToString() +
					" Drift Time = " + DriftTime.ToString();
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

			FeatureLight other = obj as FeatureLight;
			if (other == null)
				return false;

			if (!this.ID.Equals(other.ID))
			{
				return false;
			}
			if (!Abundance.Equals(other.Abundance))
			{
				return false;
			}
			if (!this.ChargeState.Equals(other.ChargeState))
			{
				return false;
			}
			if (!this.DriftTime.Equals(other.DriftTime))
			{
				return false;
			}
			if (!this.MassMonoisotopic.Equals(other.MassMonoisotopic))
			{
				return false;
			}
			if (!this.NET.Equals(other.NET))
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
				Abundance.GetHashCode() ^
				ChargeState.GetHashCode() ^
				DriftTime.GetHashCode() ^
				ID.GetHashCode() ^
				NET.GetHashCode();						
			return hashCode;
		}
		#endregion
	}
}
