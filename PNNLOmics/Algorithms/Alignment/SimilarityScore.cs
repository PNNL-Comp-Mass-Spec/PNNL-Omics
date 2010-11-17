using System;
using System.Collections.Generic;

using PNNLOmics.Data.Features;
using PNNLOmics.Data;
using PNNLOmics.Generic;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// Encapsulates the similarity scoring algorithm for scoring matches between
    /// sections of an alignee and reference dataset
    /// </summary>
    public class SimilarityScore
    {
        #region Class Members
        private AlignmentDataset m_aligneeDataset;
        private AlignmentDataset m_referenceDataset;
        private int m_expansionFactor;
        private int m_expansionFactorSquared;

        private bool m_hasBeenScored;
        private double[,,] m_similarityScores;

        private double m_standardDeviationNET;
        private double m_toleranceNET;
        private double m_standardDeviationMass;
        private double m_toleranceMass;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of SimilarityScore
        /// </summary>
        /// <param name="aligneeDataset"></param>
        /// <param name="referenceDataset"></param>
        /// <param name="matchCriteria"></param>
        public SimilarityScore(AlignmentDataset aligneeDataset, AlignmentDataset referenceDataset, int expansionFactor)
        {
            m_aligneeDataset = aligneeDataset;
            m_referenceDataset = referenceDataset;
            m_expansionFactor = expansionFactor;
            m_expansionFactorSquared = expansionFactor * expansionFactor;

            m_standardDeviationNET = NumericValues.DefaultNETStandardDeviation;
            m_toleranceNET = NumericValues.DefaultNETTolerance;
            m_standardDeviationMass = NumericValues.DefaultMassStandardDeviation;
            m_toleranceMass = NumericValues.DefaultMassTolerance;

            m_hasBeenScored = false;
            m_similarityScores = null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the standard deviation for the NET
        /// </summary>
        public double StandardDeviationNET
        {
            get { return m_standardDeviationNET; }
            set { m_standardDeviationNET = Math.Abs(value); }
        }

        /// <summary>
        /// Gets or sets the NET tolerance used when calculating section matches
        /// </summary>
        public double ToleranceNET
        {
            get { return m_toleranceNET; }
            set { m_toleranceNET = Math.Abs(value); }
        }

        /// <summary>
        /// Gets or sets the standard deviation for the mass
        /// </summary>
        public double StandardDeviationMass
        {
            get { return m_standardDeviationMass; }
            set { m_standardDeviationMass = Math.Abs(value); }
        }

        /// <summary>
        /// Gets or sets the mass tolerance used when calculating section matches
        /// </summary>
        public double ToleranceMass
        {
            get { return m_toleranceMass; }
            set { m_toleranceMass = Math.Abs(value); }
        }

        /// <summary>
        /// Gets the similarity scores between the alignee and reference datasets
        /// </summary>
        public double[,,] ScoreCube
        {
            get
            {
                if (!m_hasBeenScored)
                {
                    RecalculateScore(m_expansionFactor);
                }
                return m_similarityScores;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Recalculates the similarity score cube between the alignee and
        /// reference dataset based on a new expansion factor
        /// </summary>
        /// <param name="expansionFactor">New expansion factor</param>
        public void RecalculateScore(int expansionFactor)
        {
            m_similarityScores = new double[m_aligneeDataset.NumberOfSections,
                m_referenceDataset.NumberOfSections, m_expansionFactorSquared];

            for (int aligneeSection = 1; aligneeSection < m_aligneeDataset.NumberOfSections; ++aligneeSection)
            {
                for (int referenceSection = 1; referenceSection < m_referenceDataset.NumberOfSections; ++referenceSection)
                {
                    for (int currentExpansionFactor = 1; currentExpansionFactor <= m_expansionFactorSquared; ++currentExpansionFactor)
                    {
                        List<Pair<Feature, Feature>> sectionMatches = GenerateSectionMatches(
                            aligneeSection, referenceSection, currentExpansionFactor);

                        m_similarityScores[aligneeSection - 1, referenceSection - 1,
                            currentExpansionFactor - 1] = CalculateMatchScore(sectionMatches);
                    }
                }
            }

            m_hasBeenScored = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aligneeSection"></param>
        /// <param name="referenceSection"></param>
        /// <param name="expansionFactor"></param>
        /// <returns></returns>
        public double ValueAtLocation(int aligneeSection, int referenceSection, int expansionFactor)
        {
            if (((aligneeSection > 0) && (aligneeSection <= m_aligneeDataset.NumberOfSections)) &&
                ((referenceSection > 0) && (referenceSection <= m_referenceDataset.NumberOfSections)) &&
                ((expansionFactor > 0) && (expansionFactor <= m_expansionFactorSquared)))
            {
                return m_similarityScores[aligneeSection - 1, referenceSection - 1, expansionFactor - 1];
            }
            return 0.0;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Calculates a score for the provided matched alignment features.
        /// </summary>
        /// <param name="matchedFeatures">A list of matched features from the alignee
        /// and reference datasets</param>
        /// <returns>The score for this set of matches</returns>
        private double CalculateMatchScore(List<Pair<Feature, Feature>> matchedFeatures)
        {
            double score = 0.0;
            double logFactor = Math.Log10(m_standardDeviationNET * m_standardDeviationMass * 2 * Math.PI);
            double twoNETStandardDeviationSquared = 2 * (m_standardDeviationNET * m_standardDeviationNET);
            double twoMassStandardDeviationSquared = 2 * (m_standardDeviationMass * m_standardDeviationMass);

            foreach (Pair<Feature, Feature> match in matchedFeatures)
            {
                double deltaAligneeNET = match.First.NET - match.Second.ElutionTime;
                double deltaAligneeMass = (match.First.MassMonoisotopic -
                    match.Second.MassMonoisotopic) / match.Second.MassMonoisotopic;

                double deltaAligneeNETSquared = deltaAligneeNET * deltaAligneeNET;
                double deltaAligneeMassSquared = deltaAligneeMass * deltaAligneeMass;

                score = score - logFactor - (deltaAligneeNETSquared / twoNETStandardDeviationSquared) -
                    (deltaAligneeMassSquared - twoMassStandardDeviationSquared);

                MassTag referenceFeatureAsMassTag = match.Second as MassTag;
                if (referenceFeatureAsMassTag != null)
                {
                    score -= Math.Log10(referenceFeatureAsMassTag.ObservationCount);
                }
            }

            return score;
        }

        /// <summary>
        /// Generates a list of feature matches between a section from the alignee dataset
        /// and the reference dataset
        /// </summary>
        /// <param name="aligneeSection"></param>
        /// <param name="referenceSection"></param>
        /// <param name="expansionFactor"></param>
        /// <returns></returns>
        private List<Pair<Feature, Feature>> GenerateSectionMatches(
            int aligneeSection, int referenceSection, int expansionFactor)
        {
            List<Pair<Feature, Feature>> matches = new List<Pair<Feature, Feature>>();
            List<int> aligneeSectionFeatureIndices = m_aligneeDataset.FeatureIndicesForSection(aligneeSection - 1);
            List<int> referenceSectionFeatureIndices = m_referenceDataset.FeatureIndicesForSection(referenceSection - 1);

            foreach (int i in aligneeSectionFeatureIndices)
            {
                Feature aligneeFeature = m_aligneeDataset[i];
                Feature aligneeMatch = null;

                aligneeFeature.NET = m_aligneeDataset.EarliestElutionTime +
                    ((referenceSection - 1) * m_referenceDataset.SectionWidth);
                aligneeFeature.NET += (expansionFactor * m_referenceDataset.SectionWidth *
                    (aligneeFeature.ElutionTime - (m_aligneeDataset.EarliestElutionTime + (aligneeSection - 1) *
                    m_aligneeDataset.SectionWidth))) / m_aligneeDataset.SectionWidth;

                foreach (int j in referenceSectionFeatureIndices)
                {
                    Feature referenceFeature = m_referenceDataset[j];

                    double timeDifference = Math.Abs(referenceFeature.ElutionTime - aligneeFeature.NET);
                    double massDifference = Math.Abs(referenceFeature.MassMonoisotopic -
                        aligneeFeature.MassMonoisotopic) / referenceFeature.MassMonoisotopic;

                    // TODO: Verify that the feature matching technique below is correct

                    // Check if the time and mass differences are both under the specified tolerances
                    if ((timeDifference < ToleranceNET) && (massDifference < (ToleranceMass * NumericValues.OneMillionth)))
                    {
                        // If no match has already been made, this is the match
                        if (aligneeMatch == null)
                        {
                            aligneeMatch = referenceFeature;
                        }
                        // A match has already been made so see which one is better
                        else
                        {
                            double currentMatchTimeDifference = Math.Abs(aligneeMatch.NET - aligneeFeature.NET);
                            double currentMatchMassDifference = Math.Abs(aligneeMatch.MassMonoisotopic -
                                aligneeFeature.MassMonoisotopic) / aligneeMatch.MassMonoisotopic;

                            // Check to see if the new differences are closer than the current match
                            if ((timeDifference < currentMatchTimeDifference) &&
                                (massDifference < currentMatchMassDifference))
                            {
                                aligneeMatch = referenceFeature;
                            }
                        }
                    }
                }

                // No match was found, create a "virtual" feature
                if (aligneeMatch == null)
                {
                    aligneeMatch = (Feature)Activator.CreateInstance(m_referenceDataset[0].GetType());
                    aligneeMatch.MassMonoisotopic = aligneeFeature.MassMonoisotopic +
                        (aligneeFeature.MassMonoisotopic * ToleranceMass * NumericValues.OneMillionth);
                    aligneeMatch.ElutionTime = aligneeFeature.NET + m_toleranceNET;
                }

                matches.Add(new Pair<Feature, Feature>(aligneeFeature, aligneeMatch));

                // If both sets are LCMS features, do some additional matching
                if ((aligneeFeature is UMC) && (aligneeMatch is UMC))
                {
                    // TODO: implement additional matching for LCMS -> LCMS sets
                }
            }

            return matches;
        }
        #endregion
    }
}