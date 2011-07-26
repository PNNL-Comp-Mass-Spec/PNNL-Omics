using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Data.MassTags
{
    public class MassTagLight: FeatureLight
    {
        private int     m_conformationObservationCount;
        private int     m_qualityScore;
        //TODO: [gord] agree on this... 
        private Molecule m_molecule;
        private int m_conformationID;
        private double m_NETAverage;
        private double m_NETPredicted;
        private double m_NETStandardDeviation;
        private double m_xCorr;
        private double m_discriminantMax;
        private double m_driftTimePredicted;
        private double m_priorProbability;
        private int    m_observationCount;

        public int CleavageState
        {
            get;
            set;
        }
        public double MSGFSpecProbMax
        {
            get;
            set;
        }
        public int ModificationCount
        {
            get;
            set;
        }
        public string Modifications
        {
            get;set;
        }
        /// <summary>
        /// Gets or sets the peptide sequence. Breaking the model!
        /// </summary>
        public string PeptideSequence
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the peptide sequence. Breaking the model!
        /// </summary>
        public string PeptideSequenceEx
        {
            get;
            set;
        }
		public int ConformationID
		{
			get { return m_conformationID; }
			set { m_conformationID = value; }
		}
        public double NETAverage
        {
            get { return m_NETAverage; }
            set { m_NETAverage = value; }
        }
        public double NETPredicted
        {
            get { return m_NETPredicted; }
            set { m_NETPredicted = value; }
        }
        public double NETStandardDeviation
        {
            get { return m_NETStandardDeviation; }
            set { m_NETStandardDeviation = value; }
        }        
        public double XCorr
        {
            get { return m_xCorr; }
            set { m_xCorr = value; }
        }
        /// <summary>
        /// Gets or sets the discriminant score.  ???
        /// </summary>
        public double DiscriminantMax
        {
            get { return m_discriminantMax; }
            set { m_discriminantMax = value; }
        }
        public double DriftTimePredicted
        {
            get { return m_driftTimePredicted; }
            set { m_driftTimePredicted = value; }
        }
        /// <summary>
        /// Gets or sets the prior probability value.  This was previously
        /// peptide prophet probability, or EPIC.
        /// </summary>
        public double PriorProbability
        {
            get { return m_priorProbability; }
            set { m_priorProbability = value; }
        }
        public int ObservationCount
        {
            get { return m_observationCount; }
            set { m_observationCount = value; }
        }
		public int ConformationObservationCount
		{
			get { return m_conformationObservationCount; }
			set { m_conformationObservationCount = value; }
		}
        public int QualityScore
        {
            get { return m_qualityScore; }
            set { m_qualityScore = value; }
        }
        public Molecule Molecule
        {
            get { return m_molecule; }
            set { m_molecule = value; }
        }
        public override void Clear()
        {
            base.Clear();            
        }
    }
}
