using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmicsIO.IO
{
    public interface IMsMsSpectraWriter
    {
        void Write(string path, IEnumerable<MSSpectra> msmsFeatures);
    }
}
