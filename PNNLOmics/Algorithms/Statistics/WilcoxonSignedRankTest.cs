using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Statistics
{
    public class Normality : INormalityTest
    {
        #region INormalityTest Members

        public HypothesisTestingData Test(List<double> dist1)
        {

            

            return new HypothesisTestingData(0);
        }

        #endregion
    }
    /// <summary
    /// Performs the Wilcoxon Signed Rank Test
    /// </summary>
    public class WilcoxonSignedRankTest : IHypothesisTestingTwoSample
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

            alglib.unequalvariancettest(x, n, y, m, out twoTail, out leftTail, out rightTail);

            var t = new HypothesisTestingData(twoTail,
                                                                 leftTail,
                                                                 rightTail);

            return t;
        }
    }

    public class KolmogorovSmirnovTest : IHypothesisTestingTwoSample
    {

        #region IHypothesisTestingTwoSample Members

        public HypothesisTestingData Test(List<double> dist1, List<double> dist2)
        {
            var one = new double[dist1.Count];
            dist1.CopyTo(one);

            var two = new double[dist2.Count];
            dist1.CopyTo(two);

            var pValue = double.MaxValue;
            // TwoSampleKolmogorovSmirnovTest tester = new TwoSampleKolmogorovSmirnovTest(one, two, TwoSampleKolmogorovSmirnovTestHypothesis.SamplesDistributionsAreUnequal);
            // pValue = testc.PValue;

            var data = new HypothesisTestingData(pValue, pValue, pValue);
            return data;
        }

        #endregion
    }

    public class WilcoxonSignedRankAccord : IHypothesisTestingTwoSample
    {

        #region IHypothesisTestingTwoSample Members

        public HypothesisTestingData Test(List<double> dist1, List<double> dist2)
        {
            var one = new double[dist1.Count];
            dist1.CopyTo(one);

            var two = new double[dist2.Count];
            dist2.CopyTo(two);

            var pValue = double.MaxValue;
            //TwoSampleWilcoxonSignedRankTest tester = new TwoSampleWilcoxonSignedRankTest(one, two, TwoSampleHypothesis.ValuesAreDifferent);
            //HypothesisTestingData data = new HypothesisTestingData(tester.PValue, tester.PValue, tester.PValue);
            
            var data = new HypothesisTestingData(pValue, pValue, pValue);

            return data;
        }

        #endregion
    }

}
