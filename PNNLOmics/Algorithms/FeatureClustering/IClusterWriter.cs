using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    public interface IClusterWriter<T>
    {
        void WriteCluster(T cluster);
        void Close();
    }
}
