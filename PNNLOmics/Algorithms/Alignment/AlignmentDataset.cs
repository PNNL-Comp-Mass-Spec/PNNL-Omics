using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment
{
    public class AlignmentDataset : IEnumerable
    {
        #region Class Members
        private IEnumerable<Feature> m_dataset;
        private List<List<int>> m_sections;
        private int m_numberOfSections;
        private double m_sectionWidth;
        private double m_earliestFeatureElutionTime;
        private double m_lastestFeatureElutionTime;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="numberOfSections"></param>
        public AlignmentDataset(IList<Feature> dataset, int numberOfSections)
        {
            m_numberOfSections = numberOfSections;

            // Sort the dataset by ascending elution times
            m_dataset = dataset.OrderBy(f => f.NET);
            m_earliestFeatureElutionTime = m_dataset.First().NET;
            m_lastestFeatureElutionTime = m_dataset.Last().NET;

            DivideDatasetIntoSections();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the feature from the dataset at the specified index
        /// </summary>
        /// <param name="index">Index of feature to get</param>
        /// <returns>The feature at the specified index</returns>
        public Feature this[int index]
        {
            get { return m_dataset.ElementAt(index); }
        }

        /// <summary>
        /// Gets the number of Features stored in this AlignmentDataset
        /// </summary>
        public int Count
        {
            get { return m_dataset.Count(); }
        }

        /// <summary>
        /// Gets or sets the number of sections this dataset is broken into
        /// </summary>
        public int NumberOfSections
        {
            get { return m_numberOfSections; }
            set
            {
                m_numberOfSections = value;
                DivideDatasetIntoSections();
            }
        }

        /// <summary>
        /// Gets the width that each section of this dataset is broken into
        /// </summary>
        public double SectionWidth
        {
            get { return m_sectionWidth; }
        }

        /// <summary>
        /// Gets the earliest elution time that appears in the dataset
        /// </summary>
        public double EarliestElutionTime
        {
            get { return m_earliestFeatureElutionTime; }
        }

        /// <summary>
        /// Gets the latest elution time that appears in the dataset
        /// </summary>
        public double LatestElutionTime
        {
            get { return m_lastestFeatureElutionTime; }
        }
        #endregion

        #region Public Methods
        
        #region IEnumerable Methods
        /// <summary>
        /// This function allows AlignmentDataset to be used within a "foreach" loop
        /// </summary>
        /// <returns>The next Feature</returns>
        public IEnumerator GetEnumerator()
        {
            foreach (Feature feature in m_dataset)
            {
                yield return feature;
            }
        }
        #endregion

        /// <summary>
        /// Gets the indices of the features for the specified section
        /// </summary>
        /// <param name="section">The section to get</param>
        /// <returns>A list of int's representing the indices of the features
        /// belonging to the requested section from the dataset. Returns null if
        /// the section is out of bounds</returns>
        public List<int> FeatureIndicesForSection(int section)
        {
            if ((section > -1) && (section < m_numberOfSections))
            {
                return m_sections[section];
            }
            return null;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Populates the internal list of sections with indices of features from
        /// the dataset for which section they belong to
        /// </summary>
        private void DivideDatasetIntoSections()
        {
            m_sectionWidth = (m_lastestFeatureElutionTime - m_earliestFeatureElutionTime) / m_numberOfSections;

            // Cleanup the old section information
            if (m_sections != null)
            {
                m_sections.Clear();
                m_sections.Capacity = m_numberOfSections;
            }
            else
            {
                m_sections = new List<List<int>>(m_numberOfSections);
            }

            for (int i = 0; i < m_numberOfSections; ++i)
            {
                m_sections[i] = new List<int>();
            }

            // Calculate which section each feature belongs too
            for (int i = 0; i < m_dataset.Count(); ++i)
            {
                int featureSection = (int)Math.Floor(((m_dataset.ElementAt(i).NET - m_earliestFeatureElutionTime) *
                    m_numberOfSections) / (m_lastestFeatureElutionTime - m_earliestFeatureElutionTime)) + 1;
                m_sections[featureSection].Add(i);
            }
        }
        #endregion
    }
}