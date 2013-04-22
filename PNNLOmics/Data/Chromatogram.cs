using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Chromatogram for a given m/z holding X scan.
    /// </summary>
    public class Chromatogram
    {
        public Chromatogram()
        {
            Points = new List<XYData>();
        }
        /// <summary>
        /// Builds a chromatogram based on the supplied data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mz"></param>
        /// <param name="chargeState"></param>
        public Chromatogram(List<XYData> data, double mz, int chargeState)
        {
            Points      = data;
            StartScan   = Convert.ToInt32(data.Min(x => x.X));
            EndScan     = Convert.ToInt32(data.Max(x => x.X));
            Mz          = mz;
            ChargeState = chargeState;
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
        /// <summary>
        /// Gets or sets the points defining the chromatogram
        /// </summary>
        public List<XYData> Points
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the points that were fit to a chromatogram
        /// </summary>
        public List<XYData> FitPoints
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the fit values for a chromatogram.
        /// </summary>
        public SolverReport FitReport
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the area bound by the Xic
        /// </summary>
        public double Area
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the coefficients that describes the fit profile.
        /// </summary>
        public double[] FitCoefficients
        {
            get;
            set;
        }
    }
}
