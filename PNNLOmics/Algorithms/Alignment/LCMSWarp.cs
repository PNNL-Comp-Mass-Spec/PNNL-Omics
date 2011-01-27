using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment
{


    /// <summary>
    /// Aligns two feature sets together.  Warping T onto U.
    /// </summary>
    /// <typeparam name="T">Alignee</typeparam>
    /// <typeparam name="U">Reference</typeparam>
    public class LCMSWarp<T, U>
        where T : FeatureLight, new()
        where U : FeatureLight, new()
    {
        #region Class Members
        SimilarityScore<T, U>   m_similarityScore;
        AlignmentScore<T, U>    m_alignmentScore;
        AlignmentFunction<T>    m_alignmentFunction;

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
        public LCMSWarp(int aligneeSectionCount, 
                        int referenceSectionCount, 
                        int expansionFactor,
                        int discontinuousNETSections, 
                        int discontinuousMassSections)
        {
            AligneeSectionCount         = aligneeSectionCount;
            ReferenceSectionCount       = referenceSectionCount;
            ExpansionFactor             = expansionFactor;            
            DiscontinuousNETSections    = discontinuousNETSections;
            DiscontinousMassSections    = discontinuousMassSections;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the alignee section count.
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
                    throw new Exception();
                    //throw new InvalidAlignmentParameterException("The alignee section count cannot be less than one.");
                }
                else
                {
                    m_aligneeSectionCount = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the reference section count.
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
                    throw new Exception();
                    //throw new InvalidAlignmentParameterException("The reference section count cannot be less than one.");
                }
                else
                {
                    m_referenceSectionCount = value;
                }
            }
        }
        /// <summary>
        /// Get or sets the expansion factor.
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
                    throw new Exception();
                    //throw new InvalidAlignmentParameterException("The expansion factor cannot be less than one.");
                }
                else
                {
                    m_expansionFactor = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the total number of allowed discontinous NET sections.
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
                    throw new Exception();
                    //throw new InvalidAlignmentParameterException("The discontinous NET sections cannot be less than one.");
                }
                else
                {
                    m_discontinuousNETSections = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the total number of allowed discontinous mass sections.
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
                    throw new Exception();
                    //throw new InvalidAlignmentParameterException("The discontinous mass sections cannot be less than one.");  
                }
                else
                {
                    m_discontinuousMassSections = value;
                }
            }
        }
        #endregion

        private static int MassComparison(T x, T y)
        {
            return 0;
        }
        private void GenerateCandidateMatches(List<T> aligneeFeatures, List<U> referenceFeatures)
        {            
            Comparison<T> massComparer = new Comparison<T>(MassComparison);
            aligneeFeatures.Sort(massComparer);
            //referenceFeatures.Sort(massComparer);

        }

        /// <summary>
        /// Performs NET Warp between two features.
        /// </summary>
        private void PerformNETWarp(List<T> aligneeFeatures, List<U> referenceFeatures)
        {
        }

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aligneeFeatures"></param>
        /// <param name="referenceFeatures"></param>
        public List<T> Align(List<T> aligneeFeatures, List<U> referenceFeatures)
        {
            if (aligneeFeatures.Count == 0)
                throw new Exception();
            //throw new InvalidAlignmentParameterException("There are not enough alignee features.");

            if (referenceFeatures.Count == 0)
                throw new Exception();
            //  throw new InvalidAlignmentParameterException("There are not enough reference features.");

            AlignmentDataset<T> aligneeDataset   = new AlignmentDataset<T>(aligneeFeatures, m_aligneeSectionCount);
            AlignmentDataset<U> referenceDataset = new AlignmentDataset<U>(referenceFeatures, m_referenceSectionCount);

            // Match the sections of the datasets based on their similarity
            m_similarityScore = new SimilarityScore<T, U>(aligneeDataset, referenceDataset, ExpansionFactor);
            m_similarityScore.RecalculateScore(ExpansionFactor);

            // Score the matched sections based on their alignment
            m_alignmentScore = new AlignmentScore<T, U>(aligneeDataset,
                                                    referenceDataset,
                                                    m_expansionFactor,
                                                    m_discontinuousNETSections);

            m_alignmentScore.CalculateAlignmentScore(m_similarityScore);

            // Generate the aligned NET's for the alignee dataset
            m_alignmentFunction = new AlignmentFunction<T>(m_alignmentScore.AlignmentMatches);
            return m_alignmentFunction.AlignDatasetNETs(aligneeDataset);
        }
        /// <summary>
        /// Normalizes the elution times for the list of features based on their scans.
        /// </summary>
        /// <param name="features">List of features to normalize.</param>
        public static void NormalizeElutionTimes(List<T> features) 
        {
            double max = int.MinValue;
            double min = int.MaxValue;
            foreach (T feature in features)
            {
                max = Math.Max(max, feature.RetentionTime);
                min = Math.Min(min, feature.RetentionTime);
            }
            foreach (T feature in features)
            {
                feature.NET = Convert.ToDouble(feature.RetentionTime - min) / (max - min);
            }
        }
        #endregion
    }
}
