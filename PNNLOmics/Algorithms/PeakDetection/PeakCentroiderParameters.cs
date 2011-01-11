
namespace PNNLOmics.Algorithms.PeakDetection
{
    //TODO: scott - add xml comments
    public class PeakCentroiderParameters
    {
        //TODO: scott - add xml comments
        public bool IsXYDataCentroided { get; set; }
        //TODO: scott - add xml comments
        public int NumberOfPoints { get; set; } //number of points to fit a parabola at the top to 3,5,7 centered around the max poin

        //TODO: scott - add xml comments
        public double DefaultShoulderNoiseValue { get; set; }
        //TODO: scott - add xml comments
        public LowAbundanceFWHMPeakFit LowAbundanceFWHMPeakFitType { get; set; }
        //TODO: scott - add xml comments
        public double DefaultFWHMForCentroidedData { get; set; }//since the data comes in as sticks-to-zero data we need to add a width

        //TODO: scott - add xml comments
        //TODO: Scott - remove scan number dependency.
        public int ScanNumber { get; set; }//so we can attach a scan number to each peak

        //TODO: scott - add xml comments
        public PeakCentroiderParameters()
        {
            Clear();
        }
        /// <summary>
        /// Sets the parameters to their defaults.
        /// </summary>
        public void Clear()
        {
            this.IsXYDataCentroided = false;
            this.NumberOfPoints = 3;//how many point to fit the parabola to
            this.DefaultShoulderNoiseValue = 1;//if the local minimum goes to 0 on both sides of the peak, return this value so signal/noise = signal
            this.LowAbundanceFWHMPeakFitType = LowAbundanceFWHMPeakFit.Lorentzian;//take log first
            this.DefaultFWHMForCentroidedData = 0.6;
            this.ScanNumber = 0;
        }
    }

    //TODO: scott - add xml comments
    //TODO: scott - change the name to PeakFitType 
    public enum LowAbundanceFWHMPeakFit
    {
        //TODO: scott - add xml comments
        Parabola,//uses the parabola fit directly to find the FWHM
        //TODO: scott - add xml comments
        Lorentzian//takes the log first and then uses the parabola fit to find the FWHM
    }
}
