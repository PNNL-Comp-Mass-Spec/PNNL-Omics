using System;

namespace LCMS.Alignment
{
    public class FeatureMatch : IComparable<FeatureMatch>
    {
        double m_ppmMassError;
        double m_NETError;
        double m_driftError;

        int m_feature_index;
        int m_feature_index2;
        double m_net;
        double m_net2;

        public FeatureMatch()
        {
            m_feature_index = -1;
            m_feature_index2 = -1;
            m_net = -1;
            m_net2 = -1;

        }
        ~FeatureMatch()
        {
        }

        public double PPMMassError
        {
            get { return m_ppmMassError; }
            set { m_ppmMassError = value; }
        }

        public double NETError
        {
            get { return m_NETError; }
            set { m_NETError = value; }
        }

        public double DriftError
        {
            get { return m_driftError; }
            set { m_driftError = value; }
        }

        public double Net
        {
            get { return m_net; }
            set { m_net = value; }
        }

        public double Net2
        {
            get { return m_net2; }
            set { m_net2 = value; }
        }

        public int FeatureIndex
        {
            get { return m_feature_index; }
            set { m_feature_index = value; }
        }

        public int FeatureIndex2
        {
            get { return m_feature_index2; }
            set { m_feature_index2 = value; }
        }

        public int CompareTo(FeatureMatch compareFeature)
        {
            if (compareFeature == null)
            {
                return 1;
            }
            else
            {
                return Net.CompareTo(compareFeature.Net);
            }
        }

        bool SortFeatureMatchesByNET(ref FeatureMatch left, ref FeatureMatch right)
        {
            return (left.m_net < right.m_net);
        }

    }
}
