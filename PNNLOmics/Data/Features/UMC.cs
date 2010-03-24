using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Features
{
    /// <summary>
    /// Class that represents LC-MS, IMS-MS, LC-IMS-MS, etc. type data.
    /// </summary>
    /// <remarks>UMC stands for Unique Mass Class - see Advances in Proteomics Data Analysis and Display Using An Accurate Mass and Time Tag Approach in Mass Spectrometry Reviews, 2006. Zimmer et. al.</remarks>
    public class UMC: Feature
    {
        private UMCCluster m_umcCluster;

        public UMCCluster UmcCluster
        {
            get { return m_umcCluster; }
            set { m_umcCluster = value; }
        }
        private double m_massMonoisotopicStandardDeviation;

        public double MassMonoisotopicStandardDeviation
        {
            get { return m_massMonoisotopicStandardDeviation; }
            set { m_massMonoisotopicStandardDeviation = value; }
        }
        private ushort m_scanStart;

        public ushort ScanStart
        {
            get { return m_scanStart; }
            set { m_scanStart = value; }
        }
        private ushort m_scanEnd;

        public ushort ScanEnd
        {
            get { return m_scanEnd; }
            set { m_scanEnd = value; }
        }
        private ushort m_chargeMax;

        public ushort ChargeMax
        {
            get { return m_chargeMax; }
            set { m_chargeMax = value; }
        }
        private int m_abundanceMax;

        public int AbundanceMax
        {
            get { return m_abundanceMax; }
            set { m_abundanceMax = value; }
        }
        private int m_abundanceSum;

        public int AbundanceSum
        {
            get { return m_abundanceSum; }
            set { m_abundanceSum = value; }
        }
        private IList<MSFeature> m_msFeatureList;

        public IList<MSFeature> MSFeatureList
        {
            get { return m_msFeatureList; }
            set { m_msFeatureList = value; }
        }
    
        public UMC()
        {
            
        }

        #region BaseData<UMC> Members

        public void Clear()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IComparer<UMC> Members

        public int Compare(UMC x, UMC y)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
