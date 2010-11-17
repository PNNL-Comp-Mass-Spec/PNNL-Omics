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
        /// <summary>
        /// Gets or sets the abundance.
        /// </summary>
		public long		Abundance			{ get; set; }
        /// <summary>
        /// Gets or sets the identification number of the feature.
        /// </summary>
		public int		ID					{ get; set; }
        /// <summary>
        /// Gets or sets the monoisotopic mass of the feature.
        /// </summary>
		public double	MassMonoisotopic	{ get; set; }        
        /// <summary>
        /// Gets or sets the retention time of a feature.
        /// </summary>
        //TODO:  How can we convert the scan LC to time?  This should happen in Decon2ls
        public double RetentionTime { get; set; }
        /// <summary>
        /// Gets or sets the drift time of a feature.
        /// </summary>
		public double	DriftTime			{ get; set; }		
        /// <summary>
        /// Gets or sets the charge state of a feature.
        /// </summary>
		public int		ChargeState			{ get; set; }
        /// <summary>
        /// Gets or sets the score value for this feature.
        /// </summary>
        public float Score { get; set; }

        /// <summary>
        /// Resets the data structure back to its default state.
        /// </summary>
		public override void Clear()
		{
			this.Abundance          = 0;
			this.ChargeState        = 0;
			this.DriftTime          = 0;
			this.ID                 = -1;
			this.MassMonoisotopic   = 0;
            this.RetentionTime      = 0;
		}
		/// <summary>
		/// Compares the aligned monoisotopic mass of two Features
		/// </summary>
		public static Comparison<FeatureLight> MassComparison = delegate(FeatureLight x, FeatureLight y)
		{
			return x.MassMonoisotopic.CompareTo(y.MassMonoisotopic);
		};

        //TODO: Where to put this?
        public static double CalculateMZFromMonoisotopicMass(double massMonoisotopic, int chargeState)
        {
            throw new NotImplementedException();            
        }

		#region Overriden Base Methods
		/// <summary>
		/// Returns a basic string representation of the cluster.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "Feature Light ID = " + ID.ToString() +
					" Mono Mass = " + MassMonoisotopic.ToString() +
					" Retention Time = " + RetentionTime.ToString() +
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
			if (!this.RetentionTime.Equals(other.RetentionTime))
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
				RetentionTime.GetHashCode();						
			return hashCode;
		}
		#endregion
	}
}
