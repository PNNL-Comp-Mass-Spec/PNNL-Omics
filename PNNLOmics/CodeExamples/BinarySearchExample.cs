using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;
using PNNLOmics.Generic;

namespace PNNLOmics.CodeExamples
{
	public class BinarySearchExample
	{
		public static void Example()
		{
			AnonymousComparer<UMC> comparer = new AnonymousComparer<UMC>(new Comparison<UMC>(UMC.MassMaxComparison));
		}

		private void SearchAndInsert(List<UMC> umcList, UMC umc, AnonymousComparer<UMC> comparer)
		{
			int index = umcList.BinarySearch(umc, comparer);
			umcList.Insert(Math.Abs(index), umc);
		}
	}
}
