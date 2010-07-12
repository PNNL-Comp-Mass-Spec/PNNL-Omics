using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// Class that performs LCMSWarp alignment
    /// </summary>
    public class LCMSWarp
    {
        #region Class Members
        SimilarityScore m_similarityScore;
        AlignmentScore m_alignmentScore;
        AlignmentFunction m_alignmentFunction;

        private int m_aligneeSectionCount;
        private int m_referenceSectionCount;
        private int m_expansionFactor;
        private int m_discontinuousNETSections;
        private int m_discontinuousMassSections;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aligneeSegmentCount"></param>
        /// <param name="referenceSegmentCount"></param>
        /// <param name="expansionFactor"></param>
        public LCMSWarp(int aligneeSectionCount, int referenceSectionCount, int expansionFactor,
            int discontinuousNETSections, int discontinuousMassSections)
        {
            AligneeSectionCount = aligneeSectionCount;
            ReferenceSectionCount = referenceSectionCount;
            ExpansionFactor = expansionFactor;
            
            m_discontinuousNETSections = discontinuousNETSections;
            m_discontinuousMassSections = discontinuousMassSections;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public int AligneeSectionCount
        {
            get
            {
                return m_aligneeSectionCount;
            }
            set
            {
                if (value < 1)
                {
                    m_aligneeSectionCount = 1;
                }
                else
                {
                    m_aligneeSectionCount = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReferenceSectionCount
        {
            get
            {
                return m_referenceSectionCount;
            }
            set
            {
                if (value < 1)
                {
                    m_referenceSectionCount = 1;
                }
                else
                {
                    m_referenceSectionCount = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ExpansionFactor
        {
            get
            {
                return m_expansionFactor;
            }
            set
            {
                if (value < 1)
                {
                    m_expansionFactor = 1;
                }
                else
                {
                    m_expansionFactor = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DiscontinuousNETSections
        {
            get
            {
                return m_discontinuousNETSections;
            }
            set
            {
                if (value < 1)
                {
                    m_discontinuousNETSections = 1;
                }
                else
                {
                    m_discontinuousNETSections = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DiscontinousMassSections
        {
            get
            {
                return m_discontinuousMassSections;
            }
            set
            {
                if (value < 1)
                {
                    m_discontinuousMassSections = 1;
                }
                else
                {
                    m_discontinuousMassSections = value;
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aligneeFeatures"></param>
        /// <param name="referenceFeatures"></param>
        public void Align(IList<Feature> aligneeFeatures, IList<Feature> referenceFeatures)
        {
            AlignmentDataset aligneeDataset = new AlignmentDataset(aligneeFeatures, AligneeSectionCount);
            AlignmentDataset referenceDataset = new AlignmentDataset(referenceFeatures, ReferenceSectionCount);

            // Match the sections of the datasets based on their similarity
            m_similarityScore = new SimilarityScore(aligneeDataset, referenceDataset, ExpansionFactor);
            m_similarityScore.RecalculateScore(ExpansionFactor);

            // Score the matched sections based on their alignment
            m_alignmentScore = new AlignmentScore(aligneeDataset, referenceDataset,
                ExpansionFactor, m_discontinuousNETSections);
            m_alignmentScore.CalculateAlignmentScore(m_similarityScore);

            // Generate the aligned NET's for the alignee dataset
            m_alignmentFunction = new AlignmentFunction(m_alignmentScore.AlignmentMatches);
            m_alignmentFunction.AlignDatasetNETs(aligneeDataset);
        }
        #endregion
    }
}
