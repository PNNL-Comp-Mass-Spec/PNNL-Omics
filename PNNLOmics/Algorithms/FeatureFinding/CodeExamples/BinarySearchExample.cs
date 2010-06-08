using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;
using PNNLOmics.Generic;

namespace PNNLOmics.Algorithms.FeatureFinding.CodeExamples
{
	public class BinarySearchExample
	{
		public static void Example()
		{
			AnonymousComparer<LCMSFeature> comparer = new AnonymousComparer<LCMSFeature>(new Comparison<LCMSFeature>(UMC.MassMaxComparison));
		}

		private void SearchAndInsert(List<LCMSFeature> lcmsFeatureList, LCMSFeature lcmsFeature, AnonymousComparer<LCMSFeature> comparer)
		{
			int index = lcmsFeatureList.BinarySearch(lcmsFeature, comparer);
			lcmsFeatureList.Insert(Math.Abs(index), lcmsFeature);
		}
	}
}
