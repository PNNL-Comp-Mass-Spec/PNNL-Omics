using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.ConformationDetection.Data
{
	public class Pair<T, U>
	{
		private T m_first;
		private U m_second;

		public Pair()
		{
		}

		public Pair(T first, U second)
		{
			this.m_first = first;
			this.m_second = second;
		}

		public T First
		{
			get { return m_first; }
			set { m_first = value; }
		}

		public U Second
		{
			get { return m_second; }
			set { m_second = value; }
		}

		public static Comparison<Pair<double, int>> PairFirstComparison = delegate(Pair<double, int> pair1, Pair<double, int> pair2)
		{
			return pair1.First.CompareTo(pair2.First);
		};
	}
}
