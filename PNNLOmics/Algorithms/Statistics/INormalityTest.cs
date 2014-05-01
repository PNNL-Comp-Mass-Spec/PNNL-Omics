using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Statistics
{
    public interface INormalityTest
    {
        HypothesisTestingData Test(List<double> dist1);
    }
}
