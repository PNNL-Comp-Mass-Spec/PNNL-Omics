using PNNLOmics.Data.Features;

namespace PNNLOmics.Data.MassTags
{
    public class MassTagLight: FeatureLight
    {
        //TODO: [gord] agree on this... 
        private int m_conformationID;
        private double m_NETAverage;
        private double m_NETPredicted;
        private double m_NETStandardDeviation;
        private double m_xCorr;
        private double m_discriminantMax;
        private double m_driftTimePredicted;
        private double m_priorProbability;

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

        public int ObservationCount { get; set; }

        public int ConformationObservationCount { get; set; }
        public int QualityScore { get; set; }
        public Molecule Molecule { get; set; }
        
    }
}
