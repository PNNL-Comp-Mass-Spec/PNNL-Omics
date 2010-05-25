using System;
using System.Collections.Generic;

using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// Interface for alignment of feature maps.
    /// </summary>
    public interface IFeatureMapAligner
    {
        /// <summary>
        /// Aligns the alignee features to the baseline features.
        /// </summary>
        /// <param name="aligneeFeatures">Features to align.</param>
        /// <param name="baselineFeatures">Features to align to.</param>
        void Align(IList<Feature> aligneeFeatures, IList<Feature> baselineFeatures);
    }
}
