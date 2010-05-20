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
        #region Members
        private UMCCluster m_umcCluster;
        private ushort m_chargeMax;
        private double m_massMonoisotopicStandardDeviation;
        private ushort m_scanStart;
        private ushort m_scanEnd;
        private IList<MSFeature> m_msFeatureList;
        private int m_abundanceSum;
        private int m_abundanceMax;
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UMC()
        {
            Clear();
        }

        #region Properties
        public int AbundanceMax
        {
            get { return m_abundanceMax; }
            set { m_abundanceMax = value; }
        }
        public int AbundanceSum
        {
            get { return m_abundanceSum; }
            set { m_abundanceSum = value; }
        }
        public ushort ChargeMax
        {
            get { return m_chargeMax; }
            set { m_chargeMax = value; }
        }
        public double MassMonoisotopicStandardDeviation
        {
            get { return m_massMonoisotopicStandardDeviation; }
            set { m_massMonoisotopicStandardDeviation = value; }
        }
        public IList<MSFeature> MSFeatureList
        {
            get { return m_msFeatureList; }
            set { m_msFeatureList = value; }
        }
        public UMCCluster UmcCluster
        {
            get { return m_umcCluster; }
            set { m_umcCluster = value; }
        }
        public ushort ScanStart
        {
            get { return m_scanStart; }
            set { m_scanStart = value; }
        }        
        public ushort ScanEnd
        {
            get { return m_scanEnd; }
            set { m_scanEnd = value; }
        }
        #endregion

        #region BaseData<UMC> Members
        /// <summary>
        /// Resets the object to it's default values.
        /// </summary>
        public void Clear()
        {
            base.Clear();
            if (MSFeatureList == null)
                m_msFeatureList = new List<MSFeature>();
            else
                MSFeatureList.Clear();            
        }
        #endregion

        #region IComparer<UMC> and Comparison methods.
        /// <summary>
        /// Compares two UMC
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(UMC x, UMC y)
        {
            //TODO: How should we compare two UMC's?
            /// 
            ///  By (weighted) euclidean distance?
            ///
            throw new NotImplementedException();
        }
        /// <summary>
        /// Compares the scan end of two UMCs
        /// </summary>
        public static Comparison<UMC> ScanEndComparison = delegate(UMC x, UMC y)
        {
            return x.ScanEnd.CompareTo(y.ScanEnd);
        };
        /// <summary>
        /// Compares the scan start of two UMCs
        /// </summary>
        public static Comparison<UMC> ScanStartComparison = delegate(UMC x, UMC y)
        {
            return x.ScanStart.CompareTo(y.ScanStart);
        };
        /// <summary>
        /// Compares the summed abundance of two UMCs
        /// </summary>
        public static Comparison<UMC> AbundanceSumComparison = delegate(UMC x, UMC y)
        {
            return x.AbundanceSum.CompareTo(y.AbundanceSum);
        };
        /// <summary>
        /// Compares the max abundance of two UMCs
        /// </summary>
        public static Comparison<UMC> AbundanceMaxComparison = delegate(UMC x, UMC y)
        {
            return x.AbundanceMax.CompareTo(y.AbundanceMax);
        };
        /// <summary>
        /// Compares the max charge state of two UMCs
        /// </summary>
        public static Comparison<UMC> ChargeMaxComparison = delegate(UMC x, UMC y)
        {
            return x.ChargeMax.CompareTo(y.ChargeMax);
        };
        #endregion
    }
}
