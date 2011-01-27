using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// Holds a structured form of the features to be aligned or aligned against.
    /// </summary>
    public class AlignmentDataset<T>: IEnumerable
        where T : FeatureLight, new()
    {
        #region Class Members
        //private IEnumerable<T>  m_dataset;
        //private List<T>         m_dataset;
        public  readonly List<T> m_dataset;
        private List<List<int>> m_sections;
        private int             m_numberOfSections;
        private double          m_sectionWidth;
        private double          m_earliestFeatureElutionTime;
        private double          m_lastestFeatureElutionTime;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="numberOfSections"></param>
        public AlignmentDataset(ICollection<T> dataset, int numberOfSections)
        {
            if (dataset.Count < 1)
            {
                throw new Exception();
                //   throw new InvalidAlignmentParameterException("There are not enough alignment features.");
            }

            if (numberOfSections < 1)
            {
                throw new Exception();
                //throw new InvalidAlignmentParameterException("There are not enough sections.");
            }

            m_sections                   = null;            

            // Sort the dataset by ascending elution times
            //m_dataset                    = dataset.OrderBy(f => f.NET);
            IEnumerable<T> enumerableDataset = dataset.OrderBy(f => f.NET);
            m_dataset              = new List<T>(enumerableDataset);

            m_earliestFeatureElutionTime = m_dataset.First().NET;
            m_lastestFeatureElutionTime  = m_dataset.Last().NET;

            if (Math.Abs(m_earliestFeatureElutionTime - m_lastestFeatureElutionTime) <= double.Epsilon)
            {
                throw new Exception();
                //throw new InvalidAlignmentParameterException("The input features have the same NET value.");                
            }

            // Setting this property will automagically partition the data for us.
            NumberOfSections = numberOfSections;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the Feature from the dataset at the specified index
        /// </summary>
        /// <param name="index">Index of Feature to get</param>
        /// <returns>The Feature at the specified index</returns>
        public T this[int index]
        {
            get
            {
                return m_dataset[index];                
            }
        }        
        /// <summary>
        /// Gets the number of Features stored in this AlignmentDataset
        /// </summary>
        public int Count
        {
            //get { return m_dataset.Count(); }
            get { return m_dataset.Count; }
        }
        /// <summary>
        /// Gets or sets the number of sections this dataset is broken into
        /// </summary>
        public int NumberOfSections
        {
            get { return m_numberOfSections; }
            set
            {
                if (value < 1)
                {
                    throw new Exception();
                    //throw new InvalidAlignmentParameterException("The number of sections must be greater than one.");
                }
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
        /// <summary>
        /// This function allows AlignmentDataset to be used within a "foreach" loop
        /// </summary>
        /// <returns>The next Feature</returns>
        public IEnumerator GetEnumerator()
        {
            foreach (T feature in m_dataset)
            {
                yield return feature;
            }
        }
        /// <summary>
        /// Gets the indices of the Features for the specified section
        /// </summary>
        /// <param name="section">The section to get</param>
        /// <returns>A list of int's representing the indices of the Features
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
        /// Populates the internal list of sections with indices of Features from
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
                m_sections.Add(new List<int>());
            }

            // Calculate which section each Feature belongs too
            int     count       = m_dataset.Count();
            
            double elutionTimeWindow = m_lastestFeatureElutionTime - m_earliestFeatureElutionTime;
            //for (int i = 0; i < count; ++i)
            int j = 0;
            foreach(T feature in m_dataset)
            {

                //int featureSection = (int)Math.Floor(((m_dataset.ElementAt(i).NET - m_earliestFeatureElutionTime) *
                int featureSection = (int)Math.Floor(((feature.NET - m_earliestFeatureElutionTime) *
                    m_numberOfSections) / (elutionTimeWindow));

                featureSection = Math.Min(m_numberOfSections - 1, featureSection);
                featureSection = Math.Max(0, featureSection);

                m_sections[featureSection].Add(j++);
            }
        }
        #endregion
    }
}