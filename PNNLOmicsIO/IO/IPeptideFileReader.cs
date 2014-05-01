using System.Collections.Generic;
using PNNLOmics.Data;

namespace PNNLOmicsIO.IO
{
    /// <summary>
    /// Interface for files that contain peptides or identifications
    /// </summary>
    public interface ISequenceFileReader
    {
        IEnumerable<Peptide> Read(string path);
    }
}
