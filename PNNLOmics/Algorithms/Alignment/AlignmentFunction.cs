using System;
using System.Collections.Generic;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// 
    /// </summary>
    public class AlignmentFunction
    {
        #region Class Members
        private List<AlignmentMatch> m_alignmentMatches;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="alignmentMatches"></param>
        public AlignmentFunction(List<AlignmentMatch> alignmentMatches)
        {
            m_alignmentMatches = alignmentMatches;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="features"></param>
        /// <returns></returns>
        public void AlignDatasetNETs(AlignmentDataset features)
        {
            for (int i = 0; i < features.NumberOfSections; ++i)
            {
                AlignmentMatch sectionMatch = m_alignmentMatches[i];
                List<int> sectionFeatureIndicies = features.FeatureIndicesForSection(i);

                foreach (int featureIndex in sectionFeatureIndicies)
                {
                    Feature feature = features[featureIndex];
                    feature.NETAligned = sectionMatch.AlignFeatureNET(feature.NET);
                }
            }
        }
        #endregion
    }
}