using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    public class MassTag: Feature
    {
        private double m_NETAverage;

        public double NETAverage
        {
            get { return m_NETAverage; }
            set { m_NETAverage = value; }
        }
        private double m_NETPredicted;

        public double NETPredicted
        {
            get { return m_NETPredicted; }
            set { m_NETPredicted = value; }
        }
        private double m_NETStandardDeviation;

        public double NETStandardDeviation
        {
            get { return m_NETStandardDeviation; }
            set { m_NETStandardDeviation = value; }
        }
        private double m_xCorr;

        public double XCorr
        {
            get { return m_xCorr; }
            set { m_xCorr = value; }
        }
        private double m_discriminantMax;

        public double DiscriminantMax
        {
            get { return m_discriminantMax; }
            set { m_discriminantMax = value; }
        }
        private double m_charge1FScore;

        public double Charge1FScore
        {
            get { return m_charge1FScore; }
            set { m_charge1FScore = value; }
        }
        private double m_charge2FScore;

        public double Charge2FScore
        {
            get { return m_charge2FScore; }
            set { m_charge2FScore = value; }
        }
        private double m_charge3FScore;

        public double Charge3FScore
        {
            get { return m_charge3FScore; }
            set { m_charge3FScore = value; }
        }
        private ushort m_cleavageState;

        public ushort CleavageState
        {
            get { return m_cleavageState; }
            set { m_cleavageState = value; }
        }
        private IList<UMCCluster> m_umcClusterList;

        public IList<UMCCluster> UmcClusterList
        {
            get { return m_umcClusterList; }
            set { m_umcClusterList = value; }
        }
        private IList<Peptide> m_peptideList;

        public IList<Peptide> PeptideList
        {
            get { return m_peptideList; }
            set { m_peptideList = value; }
        }
        /*
        private IList<Modification> m_modificationList;
        private IList<Modification> ModificationList
        {
            get { return m_modificationList; }
            set { m_modificationList = value; }
        }
         */

        private IList<Protein> m_proteinList;

        public IList<Protein> ProteinList
        {
            get { return m_proteinList; }
            set { m_proteinList = value; }
        }
        public override void Clear()
        {
            base.Clear();
            throw new NotImplementedException();
        }
    }
}
