using System;
using System.Collections.Generic;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// Class that performs LC-MS Warp alignment
    /// 
    /// </summary>
    public class LCMSWarp: IFeatureMapAligner
    {


        #region IFeatureMapAligner Members

        /// <summary>
        /// Aligns the alignee feature map to the baseline feature map.
        /// </summary>
        /// <param name="aligneeFeatures"></param>
        /// <param name="baselineFeatures"></param>
        public void Align(IList<Feature> aligneeFeatures, IList<Feature> baselineFeatures)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
