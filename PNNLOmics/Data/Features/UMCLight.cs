using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// Representation of a UMC with only basic information
	/// </summary>
	public class UMCLight: FeatureLight
	{
		/// <summary>
		/// Default group ID.
		/// </summary>
		private const int DEFAULT_GROUP_ID = -1;
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		public UMCLight()
		{
			Clear();			
		}
		/// <summary>
		/// Gets or sets the group id (e.g. dataset) this feature originated from.
		/// </summary>
		public int GroupID { get; set; }
		/// <summary>
		/// Gets or sets the UMC Cluster this feature is part of.
		/// </summary>
		public UMCClusterLight UMCCluster	{ get; set; }
        /// <summary>
        /// Gets or sets the list of MS features for the given UMC.
        /// </summary>
        public List<FeatureLight> MSFeatures { get; set; }

		#region Overriden Base Methods
		/// <summary>
		/// Returns a basic string representation of the cluster.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "UMCLight Group ID " + GroupID.ToString() + " " + base.ToString();
		}
		/// <summary>
		/// Compares two objects' values to each other.
		/// </summary>
		/// <param name="obj">Object to compare to.</param>
		/// <returns>True if similar, False if not.</returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			UMCLight umc = obj as UMCLight;
			if (umc == null)
				return false;

			if (ID != umc.ID)
				return false;

			bool isBaseEqual = base.Equals(umc);
			if (!isBaseEqual)
				return false;
			
			return true;
		}
		/// <summary>
		/// Computes a hash code for the cluster.
		/// </summary>
		/// <returns>Hashcode as an integer.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		/// <summary>
		/// Clears the UMC and sets it to its default state.
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			GroupID		= DEFAULT_GROUP_ID;
			UMCCluster	= null;		
		}
		#endregion
	}
}
