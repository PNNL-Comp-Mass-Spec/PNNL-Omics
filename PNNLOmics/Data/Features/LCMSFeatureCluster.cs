using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics
{
    public class LCMSFeatureCluster: Feature, IBaseData<LCMSFeatureCluster>
    {
        public List<LCMSFeature> Features
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        #region IBaseData<LCMSFeatureCluster> Members

        public void Clear()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IComparer<LCMSFeatureCluster> Members

        public int Compare(LCMSFeatureCluster x, LCMSFeatureCluster y)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
