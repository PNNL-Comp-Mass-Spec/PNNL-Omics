using System;
using System.Collections;
using System.Collections.Generic;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// Encapsulates the alignment scoring algorithm for generating an alignment
    /// matrix between an alignee and reference dataset
    /// </summary>
    public class AlignmentScore
    {
        #region Class Members
        private AlignmentDataset m_aligneeDataset;
        private AlignmentDataset m_referenceDataset;
        private int m_expansionFactor;
        private int m_discontinuousNETSections;

        private double m_standardDeviationNET;
        private double m_toleranceNET;
        private double m_standardDeviationMass;
        private double m_toleranceMass;     

        private List<AlignmentMatch> m_alignmentMatches;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of AlignmentScore
        /// </summary>
        /// <param name="aligneeDataset"></param>
        /// <param name="referenceDataset"></param>
        public AlignmentScore(AlignmentDataset aligneeDataset, AlignmentDataset referenceDataset,
            int expansionFactor, int discontinuousNETSections)
        {
            m_aligneeDataset = aligneeDataset;
            m_referenceDataset = referenceDataset;
            m_expansionFactor = expansionFactor;
            m_discontinuousNETSections = discontinuousNETSections;

            m_standardDeviationNET = NumericValues.DefaultNETStandardDeviation;
            m_toleranceNET = NumericValues.DefaultNETTolerance;
            m_standardDeviationMass = NumericValues.DefaultMassStandardDeviation;
            m_toleranceMass = NumericValues.DefaultMassTolerance;

            m_alignmentMatches = new List<AlignmentMatch>(aligneeDataset.NumberOfSections);
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
        /// Gets the list of alignment matches generated during the scoring algorithm
        /// </summary>
        public List<AlignmentMatch> AlignmentMatches
        {
            get { return m_alignmentMatches; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Calculates the alignment matrix between the alignee and reference dataset
        /// </summary>
        /// <param name="similarityScore">The similarity score that was calculated
        /// between the two datasets as this AlignmentScore</param>
        public void CalculateAlignmentScore(SimilarityScore similarityScore)
        {
            double[,] alignmentScores = new double[m_aligneeDataset.NumberOfSections, m_referenceDataset.NumberOfSections];

            // Do a bunch of math up front
            double logFactor = Math.Log10(m_standardDeviationNET * m_standardDeviationMass * 2 * Math.PI);
            double twoNETToleranceSquaredOverTwoNETStandardDeviationSquared =
                (2 * (m_toleranceNET * m_toleranceNET)) / (2 * (m_standardDeviationNET * m_standardDeviationNET));
            double twoMassToleranceSquaredOverTwoMassStandardDeviationSquared = 
                (2 * (m_toleranceMass * m_toleranceMass)) / (2 * (m_standardDeviationMass * m_standardDeviationMass));
            
            // This is the alignment factor used to calculate the first row and
            // column of the alignment score matrix
            double alignmentFactor = logFactor - twoNETToleranceSquaredOverTwoNETStandardDeviationSquared -
                twoMassToleranceSquaredOverTwoMassStandardDeviationSquared;

            int unmatchedAligneeCount = 0;
            int unmatchedReferenceCount = 0;// m_referenceDataset.FeatureIndicesForSection(0).Count;

            // Score the first column
            for (int i = 1; i < m_aligneeDataset.NumberOfSections; ++i)
            {
                unmatchedAligneeCount += m_aligneeDataset.FeatureIndicesForSection(i - 1).Count;
                alignmentScores[i, 0] = -unmatchedAligneeCount * alignmentFactor;
            }

            // Score the first row
            for (int i = 1; i < m_referenceDataset.NumberOfSections; ++i)
            {
                unmatchedReferenceCount += m_referenceDataset.FeatureIndicesForSection(i - 1).Count;
                alignmentScores[0, i] = -unmatchedReferenceCount * alignmentFactor;
            }

            // Fill in the middle
            int expansionFactorSquared = m_expansionFactor * m_expansionFactor;
            for (int i = 2; i < m_aligneeDataset.NumberOfSections; ++i)
            {
                int bestReferenceSection = 2;
                int bestExpansionFactor = 1;

                for (int j = 2; j < m_referenceDataset.NumberOfSections; ++j)
                {
                    double bestScore = double.MinValue;
                    for (int k = 1; k < expansionFactorSquared; ++k)
                    {
                        for (int l = 0; l < m_discontinuousNETSections; ++l)
                        {
                            double possibleNewAlignmentScore = alignmentScores[i - 1, j - k - l] +
                                similarityScore.ValueAtLocation(i - 1, j - k - l, Math.Min(k, l));

                            alignmentScores[i, j] = Math.Max(
                                alignmentScores[i, j], possibleNewAlignmentScore);

                            if (alignmentScores[i, j] > bestScore)
                            {
                                bestScore = alignmentScores[i, j];
                                bestExpansionFactor = k;
                                bestReferenceSection = j;
                            }
                        }
                    }
                }

                m_alignmentMatches.Add(new AlignmentMatch(
                    (m_aligneeDataset.EarliestElutionTime + (i * (m_aligneeDataset.LatestElutionTime - m_aligneeDataset.EarliestElutionTime)) / m_aligneeDataset.NumberOfSections),
                    (m_aligneeDataset.EarliestElutionTime + ((i + 1)* (m_aligneeDataset.LatestElutionTime - m_aligneeDataset.EarliestElutionTime)) / m_aligneeDataset.NumberOfSections),
                    (m_referenceDataset.EarliestElutionTime + (bestReferenceSection * m_referenceDataset.SectionWidth)),
                    (m_referenceDataset.EarliestElutionTime + ((bestReferenceSection + 1) * m_referenceDataset.SectionWidth)),
                    alignmentScores[i, bestReferenceSection]));
            }
        }
        #endregion
    }
}