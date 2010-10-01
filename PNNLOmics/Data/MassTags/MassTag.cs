using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PNNLOmics.Data.Features;

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
        /// <summary>
        /// Gets or sets the discriminant score.  ???
        /// </summary>
        public double DiscriminantMax
        {
            get { return m_discriminantMax; }
            set { m_discriminantMax = value; }
        }
        private double m_driftTimePredicted;

        public double DriftTimePredicted
        {
            get { return m_driftTimePredicted; }
            set { m_driftTimePredicted = value; }
        }
        private double m_priorProbability;

        /// <summary>
        /// Gets or sets the prior probability value.  This was previously
        /// peptide prophet probability, or EPIC.
        /// </summary>
        public double PriorProbability
        {
            get { return m_priorProbability; }
            set { m_priorProbability = value; }
        }
        private ushort m_observationCount;

        public ushort ObservationCount
        {
            get { return m_observationCount; }
            set { m_observationCount = value; }
        }
        private ushort m_qualityScore;

        public ushort QualityScore
        {
            get { return m_qualityScore; }
            set { m_qualityScore = value; }
        }

        //TODO: [gord] agree on this... 
        private Molecule m_molecule;

        public Molecule Molecule
        {
            get { return m_molecule; }
            set { m_molecule = value; }
        }
        /*
        private IList<Modification> m_modificationList;
        private IList<Modification> ModificationList
        {
            get { return m_modificationList; }
            set { m_modificationList = value; }
        }
         */
        public override void Clear()
        {
            base.Clear();            
        }
        public static Comparison<MassTag> MassComparison = delegate(MassTag massTag1, MassTag massTag2)
        {
            return massTag1.MassMonoisotopic.CompareTo(massTag2.MassMonoisotopic);
        };
    }
}
