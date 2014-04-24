using System;

namespace PNNLOmics.Algorithms.Alignment.LcmsWarp
{
    /// <summary>
    /// Holds the alignment data relevant to the LCMSWarp alignment.
    /// This includes Histograms for the Mass, Net and Drift Errors,
    /// Dataset name, Heatmap scores, the NET slope, intercept and r squared values,
    /// the mean and standard deviations for NET and Mass and the alignment function itself.
    /// </summary>
    public class LcmsWarpAlignmentData
    {
        /// <summary>
        /// Property for the dataset ID
        /// </summary>
        public int DatasetId { get; set; }
        /// <summary>
        /// Property to hold the function for the alignment
        /// </summary>
        public LcmsWarpAlignmentFunction AlignmentFunction { get; set; }
        /// <summary>
        /// Property to hold the name of the alignee dataset
        /// </summary>
        public string AligneeDataset { get; set; }
        /// <summary>
        /// Property to hold the heat scores for the alignment
        /// </summary>
        public double[,] HeatScores { get; set; }
        /// <summary>
        /// Property to hold the min scan number from the reference set
        /// </summary>
        public int MinScanBaseline { get; set; }
        /// <summary>
        /// Property to hold the max scan number from the reference set
        /// </summary>
        public int MaxScanBaseline { get; set; }
        /// <summary>
        /// Property to hold the min scan number from the alignment
        /// </summary>
        public double MinMtdbnet { get; set; }
        /// <summary>
        /// Property to hold the max scan number from the alignment
        /// </summary>
        public double MaxMtdbnet { get; set; }
        /// <summary>
        /// Property to hold the mass error histogram from the alignment
        /// </summary>
        public double[,] MassErrorHistogram { get; set; }
        /// <summary>
        /// Property to hold the net error histogram from the alignment
        /// </summary>
        public double[,] NetErrorHistogram { get; set; }
        /// <summary>
        /// Property to hold the drift error histogram from the alignment
        /// </summary>
        public double[,] DriftErrorHistogram { get; set; }
        /// <summary>
        /// Property to hold the R squared value from a linear regression of the whole alignment
        /// </summary>
        public double NetRsquared { get; set; }
        /// <summary>
        /// Property to hold the slope from a linear regression of the whole alignment
        /// </summary>
        public double NetSlope { get; set; }
        /// <summary>
        /// Property to hold the intercept value from a linear regression of the whole alignment
        /// </summary>
        public double NetIntercept { get; set; }
        /// <summary>
        /// Property to hold the mean of the mass values from the alignment
        /// </summary>
        public double MassMean { get; set; }
        /// <summary>
        /// Property to hold the mean of the normalized elution time values from the alignment
        /// </summary>
        public double NetMean { get; set; }
        /// <summary>
        /// Property to hold the Standard Deviation of the mass from the alignment
        /// </summary>
        public double MassStandardDeviation { get; set; }
        /// <summary>
        /// Property to hold the Standard Deviation of the normalized elution time from the alignment
        /// </summary>
        public double NetStandardDeviation { get; set; }

        /// <summary>
        /// Gets or sets the residual alignment data.
        /// </summary>        
        public ResidualData ResidualData { get; set; }

        /// <summary>
        /// Returns the mass based on the Kurtosis method
        /// (MassMean ^ 4) / (MassStdv ^ 4)
        /// </summary>
        public double MassKurtosis
        {
            get
            {
                return Math.Pow(MassMean, 4) / Math.Pow(MassStandardDeviation, 4);
            }
        }
        /// <summary>
        /// Returns the normalized elution time based on the Kurtosis method
        /// (NetMean ^ 4) / (NetStdv ^ 4)
        /// </summary>
        public double NetKurtosis
        {
            get
            {
                return Math.Pow(NetMean, 4) / Math.Pow(NetStandardDeviation, 4);
            }
        }

        /// <summary>
        /// Test to see if the alignment datasets are equal based on
        /// the ID number of the alignment data. Returns true if
        /// the dataset IDs are the same, false in any other case
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var factor = (LcmsWarpAlignmentData)obj;

            if (factor == null)
            {
                return false;
            }
            return DatasetId.Equals(factor.DatasetId);
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 23 + DatasetId.GetHashCode();

            return hash;
        }
    }
}
