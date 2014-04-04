using System;
using PNNLOmics.Alignment.LCMSWarp.LCMSProcessor;

namespace PNNLOmics.Alignment.LCMSWarp.LCMSAligner
{
    /// <summary>
    /// Holds the alignment data relevant to the LCMSWarp alignment.
    /// This includes Histograms for the Mass, Net and Drift Errors,
    /// Dataset name, Heatmap scores, the NET slope, intercept and r squared values,
    /// the mean and standard deviations for NET and Mass and the alignment function itself.
    /// </summary>
    public class LCMSAlignmentData
    {
        private ResidualData m_residualData;

        /// <summary>
        /// Property for the dataset ID
        /// </summary>
        public int DatasetId { get; set; }

        public LCMSAlignmentFunction alignmentFunction;
        public string aligneeDataset;
        public double[,] heatScores;
        public int minScanBaseline;
        public int maxScanBaseline;
        public double minMTDBNET;
        public double maxMTDBNET;
        public double[,] massErrorHistogram;
        public double[,] netErrorHistogram;
        public double[,] driftErrorHistogram;
        public double NETRsquared { get; set; }
        public double NETSlope { get; set; }
        public double NETIntercept { get; set; }
        public double MassMean { get; set; }
        public double NETMean { get; set; }
        public double MassStandardDeviation { get; set; }
        public double NETStandardDeviation { get; set; }

        /// <summary>
        /// Gets or sets the residual alignment data.
        /// </summary>        
        public ResidualData ResidualData
        {
            get
            {
                return m_residualData;
            }
            set
            {
                m_residualData = value;
            }
        }

        public double MassKurtosis
        {
            get
            {
                return Math.Pow(MassMean, 4) / Math.Pow(MassStandardDeviation, 4);
            }
        }
        public double NETKurtosis
        {
            get
            {
                return Math.Pow(NETMean, 4) / Math.Pow(NETStandardDeviation, 4);
            }
        }



        public override bool Equals(object obj)
        {
            LCMSAlignmentData factor = (LCMSAlignmentData)obj;

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
