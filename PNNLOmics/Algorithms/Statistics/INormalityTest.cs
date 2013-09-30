using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.Statistics
{
    public interface INormalityTest
    {
        HypothesisTestingData Test(List<double> dist1);
    }
}
