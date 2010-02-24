using System;

namespace PNNLOmics.Data
{
    public class LCMSFeature: Feature, IBaseData<LCMSFeature>
    {
        public int MSFeatures
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        #region IBaseData<LCMSFeature> Members

        public void Clear()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IComparer<LCMSFeature> Members

        public int Compare(LCMSFeature x, LCMSFeature y)
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

    public class MSFeature : Feature, IBaseData<MSFeature>
    {
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

        #region IBaseData<MSFeature> Members

        public void Clear()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IComparer<MSFeature> Members

        public int Compare(MSFeature x, MSFeature y)
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
