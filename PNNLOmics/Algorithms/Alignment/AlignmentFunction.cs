using System;
using System.Collections.Generic;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// Alignment data between two datasets.
    /// </summary>
    public class AlignmentFunction<T>
        where T : FeatureLight, new()
    {
        #region Class Members
        /// <summary>
        /// A list of alignment matches between two datasets.
        /// </summary>
        private List<AlignmentMatch> m_alignmentMatches;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="alignmentMatches">Matches found between two datasets.</param>
        public AlignmentFunction(List<AlignmentMatch> alignmentMatches)
        {
            m_alignmentMatches = alignmentMatches;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Transforms the features stored in the alignment dataset into new features.
        /// </summary>
        /// <param name="features">Alignment dataset containing alignee features.</param>
        /// <returns>Transformed list of features.</returns>
        public List<T> AlignDatasetNETs(AlignmentDataset<T> features)
        {
            List<T> alignedFeatures = new List<T>();
            for (int i = 0; i < features.NumberOfSections; ++i)
            {
                AlignmentMatch sectionMatch      = m_alignmentMatches[i];
                List<int> sectionFeatureIndicies = features.FeatureIndicesForSection(i);

                foreach (int featureIndex in sectionFeatureIndicies)
                {
                    T originalFeature = features[featureIndex]as T;
                    T feature         = originalFeature;//new Feature(originalFeature);
                    feature.NET       = sectionMatch.AlignFeatureNET(originalFeature.NET);
                    alignedFeatures.Add(feature);
                }
            }
            return alignedFeatures;
        }
        #endregion
    }
}