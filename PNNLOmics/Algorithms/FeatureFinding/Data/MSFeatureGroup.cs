using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureFinding.Data
{
	public class MSFeatureGroup
	{
		private List<MSFeature> m_msFeatureList;

		private int m_scanLC;

		private double m_massMonoisotopicMedian;

		public MSFeatureGroup(List<MSFeature> msFeatureList)
		{
			m_msFeatureList = msFeatureList;
			CalculateStatistics();
		}

		public List<MSFeature> MSFeatureList
		{
			get { return m_msFeatureList; }
		}

		public int ScanLC
		{
			get { return m_scanLC; }
		}

		public double MassMonoisotopicMedian
		{
			get { return m_massMonoisotopicMedian; }
		}

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
