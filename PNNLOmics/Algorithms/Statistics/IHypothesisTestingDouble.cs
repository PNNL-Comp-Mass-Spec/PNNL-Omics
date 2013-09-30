using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.Statistics
{
    public interface IHypothesisTestingTwoSample
    {
        HypothesisTestingData Test(List<double> dist1, List<double> dist2); 
    }
}
