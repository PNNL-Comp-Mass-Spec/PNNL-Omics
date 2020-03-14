using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Statistics
{
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Statistics")]
    public interface INormalityTest
    {
        HypothesisTestingData Test(List<double> dist1);
    }
}
