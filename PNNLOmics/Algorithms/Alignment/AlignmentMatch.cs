using System;
using System.Collections.Generic;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// 
    /// </summary>
    public class AlignmentMatch
    {
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aligneeNETStart"></param>
        /// <param name="aligneeNetEnd"></param>
        /// <param name="referenceNETStart"></param>
        /// <param name="referenceNETEnd"></param>
        public AlignmentMatch(double aligneeNETStart, double aligneeNetEnd,
            double referenceNETStart, double referenceNETEnd, double score)
        {
            AligneeNETStart = aligneeNETStart;
            AligneeNETEnd = aligneeNetEnd;
            ReferenceNETStart = referenceNETStart;
            ReferenceNETEnd = referenceNETEnd;
            Score = score;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public double AligneeNETStart { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public double AligneeNETEnd { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public double ReferenceNETStart { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public double ReferenceNETEnd { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public double Score { get; private set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldNET"></param>
        /// <returns></returns>
        public double AlignFeatureNET(double oldNET)
        {
            return (((oldNET - AligneeNETStart) * (ReferenceNETEnd - ReferenceNETStart)) /
                (AligneeNETEnd - AligneeNETStart)) + ReferenceNETStart;
        }
        #endregion
    }
}