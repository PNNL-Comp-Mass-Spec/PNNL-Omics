using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.Statistics
{
    public class JacqueBeraNormalityTest: INormalityTest
    {
        /// <summary>
        /// Computes a p-value for a given distribution.
        /// </summary>
        /// <param name="distribution">Values used to compute the p-value.</param>
        /// <returns>Level of significance (p-value)</returns>
        public HypothesisTestingData Test(List<double> dist1)
        {
            double[] x = new double[dist1.Count];
            int n = dist1.Count;            
            dist1.CopyTo(x);

            double pValue = double.MaxValue;

            alglib.jarqueberatest(x, n, out pValue);
            HypothesisTestingData t = new HypothesisTestingData(pValue);

            return t;
        }    
    }
}
