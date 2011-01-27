using System;
using System.Collections.Generic;

using PNNLOmics.UnitTests.AlgorithmTests.Alignment;

namespace PNNLOmics.UnitTests
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            LCMSWarpTests tests = new LCMSWarpTests();
            tests.AlignFeatureFiles(0);            
        }
    }
}
