using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Statistics
{
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Statistics")]
    public interface IHypothesisTestingTwoSample
    {
        HypothesisTestingData Test(List<double> dist1, List<double> dist2); 
    }
}
