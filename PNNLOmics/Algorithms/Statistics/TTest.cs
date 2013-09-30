using System;
using System.Collections.Generic;
using System.Linq;

namespace PNNLOmics.Algorithms.Statistics
{
    /// <summary>
    /// Performs the Wilcoxon Signed Rank Test
    /// </summary>
    public class StudentTTest: IHypothesisTestingTwoSample
    {

        /// <summary>
        /// Computes a p-value for a given distribution.
        /// </summary>
        /// <param name="distribution">Values used to compute the p-value.</param>
        /// <returns>Level of significance (p-value)</returns>
        public HypothesisTestingData Test(List<double> dist1, List<double> dist2)
        {
            double[] y = new double[dist2.Count];
            double[] x = new double[dist1.Count];
            int n = dist1.Count;
            int m = dist2.Count;

            dist1.CopyTo(x);
            dist2.CopyTo(y);

            double twoTail      = double.MaxValue;
            double leftTail     = double.MaxValue;
            double rightTail    = double.MaxValue;

            alglib.studentttest2(x, n, y, m, out twoTail, out leftTail, out rightTail);

            HypothesisTestingData t = new HypothesisTestingData(twoTail,
                                                                 leftTail,
                                                                 rightTail);

            return t;
        }
    }
}
