﻿using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Statistics
{
    /// <summary>
    /// Performs the Wilcoxon Signed Rank Test
    /// </summary>
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Statistics")]
    public class MannWhitneyTest : IHypothesisTestingTwoSample
    {
        /// <summary>
        /// Computes a p-value for a given distribution.
        /// </summary>
        /// <param name="distribution">Values used to compute the p-value.</param>
        /// <returns>Level of significance (p-value)</returns>
        public HypothesisTestingData Test(List<double> dist1, List<double> dist2)
        {
            var y = new double[dist2.Count];
            var x = new double[dist1.Count];
            var n = dist1.Count;
            var m = dist2.Count;

            dist1.CopyTo(x);
            dist2.CopyTo(y);

            var twoTail      = double.MaxValue;
            var leftTail     = double.MaxValue;
            var rightTail    = double.MaxValue; 

            alglib.mannwhitneyutest(x, n, y, m, out twoTail, out leftTail, out rightTail);

            var t = new HypothesisTestingData(twoTail,
                                                                 leftTail,
                                                                 rightTail);

            return t;
        }
    }

}
