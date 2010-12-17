using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.PeakDetector
{
    public class PeakCentroidParameters
    {
        public bool IsXYDataCentroided {get;set;}
        public int NumberOfPoints {get;set;} //number of points to fit a parabola at the top to 3,5,7 centered around the max point
        public double DefaultShoulderNoiseValue { get; set; }
        public LowAbundanceFWHMPeakFit LowAbundanceFWHMPeakFitType { get; set; }
        public double DefaultFWHMForCentroidedData { get; set; }//since the data comes in as sticks-to-zero data we need to add a width
        public int ScanNumber { get; set; }//so we can attach a scan number to each peak

        public PeakCentroidParameters() 
        {
            this.IsXYDataCentroided = false;
            this.NumberOfPoints = 3;//how many point to fit the parabola to
            this.DefaultShoulderNoiseValue = 1;//if the local minimum goes to 0 on both sides of the peak, return this value so signal/noise = signal
            this.LowAbundanceFWHMPeakFitType = LowAbundanceFWHMPeakFit.Lorentzian;//take log first
            this.DefaultFWHMForCentroidedData = 0.6;
            this.ScanNumber = 0;
        }
    }

    public enum LowAbundanceFWHMPeakFit
    {
        Parabola,//uses the parabola fit directly to find the FWHM
        Lorentzian//takes the log first and then uses the parabola fit to find the FWHM
    }
}
