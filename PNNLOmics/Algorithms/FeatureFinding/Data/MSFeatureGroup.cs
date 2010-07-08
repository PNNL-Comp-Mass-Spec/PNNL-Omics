using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureFinding.Data
{
	/// <summary>
	/// A group of MS Features. The MS Features should all have the same LC Scan.
	/// </summary>
	public class MSFeatureGroup
	{
		private List<MSFeature> m_msFeatureList;

		private int m_scanLC;

		private double m_massMonoisotopicMedian;

		/// <summary>
		/// Constructor that takes in a List of MS Features that will make up the MSFeatureGroup.
		/// </summary>
		/// <param name="msFeatureList">List of MS Features that will make up the MSFeatureGroup</param>
		public MSFeatureGroup(List<MSFeature> msFeatureList)
		{
			m_msFeatureList = msFeatureList;
			CalculateStatistics();
		}

		/// <summary>
		/// List of MS Features in the MSFeatureGroup.
		/// </summary>
		public List<MSFeature> MSFeatureList
		{
			get { return m_msFeatureList; }
		}

		/// <summary>
		/// The LC Scan # of the MSFeatureGroup.
		/// </summary>
		public int ScanLC
		{
			get { return m_scanLC; }
		}

		/// <summary>
		/// The median monoisotopic mass of the MSFeatureGroup.
		/// </summary>
		public double MassMonoisotopicMedian
		{
			get { return m_massMonoisotopicMedian; }
		}

		/// <summary>
		/// Calculates statistics of the MSFeatureGroup.
		/// </summary>
		private void CalculateStatistics()
		{
			List<double> massMonoisotopicList = new List<double>();

			foreach (MSFeature msFeature in m_msFeatureList)
			{
				massMonoisotopicList.Add(msFeature.MassMonoisotopic);
			}

			massMonoisotopicList.Sort();
			if (m_msFeatureList.Count % 2 == 1)
			{
				m_massMonoisotopicMedian = massMonoisotopicList[m_msFeatureList.Count / 2];
			}
			else
			{
				m_massMonoisotopicMedian = 0.5 * (massMonoisotopicList[m_msFeatureList.Count / 2 - 1] + massMonoisotopicList[m_msFeatureList.Count / 2]);
			}

			if (m_msFeatureList.Count > 0)
			{
				m_scanLC = m_msFeatureList[0].ScanLC;
			}
		}
	}
}
