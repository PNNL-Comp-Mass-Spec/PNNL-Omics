using System.Collections.Generic;
using PNNLOmics.Data;

namespace PNNLOmicsIO.IO
{
    public interface IMsMsSpectraReader
    {
        List<MSSpectra> Read(string path);
    }
}
