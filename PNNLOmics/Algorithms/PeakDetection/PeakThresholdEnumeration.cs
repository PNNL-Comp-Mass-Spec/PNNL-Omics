using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.PeakDetection
{
    /// <summary>
    /// Has the noise been removed yet.  Orbitrap data has the noise removed.
    /// </summary>
    public enum InstrumentDataNoiseType
    {
        Standard,
        NoiseRemoved
    }
}
