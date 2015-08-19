using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Statistics
{
    public interface IHypothesisTestingTwoSample
    {
        HypothesisTestingData Test(List<double> dist1, List<double> dist2); 
    }
}
