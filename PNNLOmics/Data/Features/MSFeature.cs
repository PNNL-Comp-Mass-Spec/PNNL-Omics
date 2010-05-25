using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Features
{
	public class MSFeature : Feature
	{
		private IList<MSPeak> m_msPeakList;

		public IList<MSPeak> MSPeakList
		{
			get { return m_msPeakList; }
			set { m_msPeakList = value; }
		}

		public int Fit
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		#region BaseData<MSFeature> Members

		public override void Clear()
		{
			throw new NotImplementedException();
		}

		#endregion


	}
}
