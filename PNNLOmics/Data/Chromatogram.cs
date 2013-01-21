using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Chromatogram for a given m/z holding X scan.
    /// </summary>
    public class Chromatogram
    {
        public Chromatogram()
        {
            Points = new List<XYZData>();
        }
        public int ChargeState
        {
            get;
            set;
        }
        public double Mz
        {
            get;
            set;
        }
        public int StartScan
        {
            get;
            set;
        }
        public int EndScan
        {
            get;
            set;
        }
        public List<XYZData> Points
        {
            get;
            set;
        }
    }
}
