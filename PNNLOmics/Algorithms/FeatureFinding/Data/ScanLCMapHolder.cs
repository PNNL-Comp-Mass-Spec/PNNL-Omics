using System;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.FeatureFinding.Data
{
	/// <summary>
	/// This class holds a mapping between the adjusted LC Scan # and the actual LC Scan #.
	/// The adjusted LC Scan # is designed to be a consecutive # starting at 1. When an MS/MS is ran as part of an experiment,
	/// the Isos file will have gaps in the LC Scan #. The adjusted LC Scan # closes these gaps so that the program can easily
	/// calculate the actual distance between 2 LC Scan #s.
	/// </summary>
	public static class ScanLCMapHolder
	{
		/// <summary>
		/// The next adjusted LC Scan # to be used.
		/// </summary>
		public static int ScanLCIndex { get; set; }
		/// <summary>
		/// The Dictionary object that contains the mapping betweent the adjusted LC Scan # and the actual LC Scan #.
		/// The keys of the Dictionary are the adjusted LC Scan #s and the values are the actual LC Scan #s.
		/// </summary>
		public static Dictionary<int, int> ScanLCMap { get; set; }

		/// <summary>
		/// Static constructor for intial creation of the object.
		/// </summary>
		static ScanLCMapHolder()
		{
			Reset();
		}

		/// <summary>
		/// Resets the ScanLCIndex to 1 and empties the DIctionary object.
		/// </summary>
		public static void Reset()
		{
			ScanLCIndex = 1;
			ScanLCMap = new Dictionary<int, int>();
		}
	}
}
