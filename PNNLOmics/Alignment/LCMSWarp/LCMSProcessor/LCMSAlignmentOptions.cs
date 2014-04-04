using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Alignment.LCMSWarp.LCMSProcessor
{
    public class LCMSAlignmentOptions
    {
        #region Auto Properties
        /// <summary>
        /// Number of Time Sections
        /// </summary>
        public int NumTimeSections { get; set; }

        /// <summary>
        /// Contraction factor for the alignment
        /// </summary>
        public int ContractionFactor { get; set; }

        /// <summary>
        /// Max time distortion at which to filter afterwards
        /// </summary>
        public int MaxTimeDistortion { get; set; }

        /// <summary>
        /// Max number of promiscuous points to use for alignment
        /// </summary>
        public int MaxPromiscuity { get; set; }

        /// <summary>
        /// Flag for whether to even use promiscuous points or not
        /// </summary>
        public bool UsePromiscuousPoints { get; set; }

        /// <summary>
        /// Flag for whether to use LSQ during mass calibration
        /// </summary>
        public bool MassCalibUseLsq { get; set; }

        /// <summary>
        /// Window for the Mass calibration alignment
        /// </summary>
        public double MassCalibrationWindow { get; set; }

        /// <summary>
        /// Number of Mass slices for the mass calibration
        /// </summary>
        public int MassCalibNumXSlices { get; set; }

        /// <summary>
        /// Number of NET slices for the mass calibration
        /// </summary>
        public int MassCalibNumYSlices { get; set; }

        /// <summary>
        /// Number of jumps for the alignment function
        /// </summary>
        public int MassCalibMaxJump { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double MassCalibMaxZScore { get; set; }

        public double MassCalibLsqMaxZScore { get; set; }

        public int MassCalibLSQNumKnots { get; set; }

        public double MassTolerance { get; set; }

        public double NETTolerance { get; set; }

        public AlignmentType AlignType { get; set; }

        public CalibrationType CalibType { get; set; }

        public bool AlignToMassTagDatabase { get; set; }

        public string AlignmentBaselineName { get; set; }

        /// <summary>
        /// How wide the Mass histogram bins are
        /// </summary>
        public double MassBinSize { get; set; }

        /// <summary>
        /// How wide the NET histogram bins are
        /// </summary>
        public double NETBinSize { get; set; }

        /// <summary>
        /// How wide the Drift time histogram bins are
        /// </summary>
        public double DriftTimeBinSize { get; set; }

        /// <summary>
        /// Flag for whether to align split MZ boundaries or not
        /// </summary>
        public bool AlignSplitMZs { get; set; }

        /// <summary>
        /// List of the MZ boundaries for the alignment
        /// </summary>
        public List<LCMSAlignmentMzBoundary> MzBoundaries { get; set; }

        /// <summary>
        /// Abundance percentage under which to filter alignment.
        /// Set to 0 means all features are matched, set to 100 means no features are matched,
        /// set to 33 the top 67% of features sorted by abundance are matched
        /// </summary>
        public int TopFeatureAbundancePercent { get; set; }

        /// <summary>
        /// Flag for whether to store the alignment function from one alignment to another
        /// </summary>
        public bool StoreAlignmentFunction { get; set; }

        /// <summary>
        /// The type of aligner the processor uses.
        /// </summary>
        public FeatureAlignmentType AlignmentAlgorithmType { get; set; }

        #endregion

        /// <summary>
        /// Default constructor, initializes every value to commonly used values and flags
        /// </summary>
        public LCMSAlignmentOptions()
        {
            NumTimeSections = 100;
            TopFeatureAbundancePercent = 0;
            ContractionFactor = 3;
            MaxTimeDistortion = 10;
            MaxPromiscuity = 3;
            UsePromiscuousPoints = false;
            MassCalibUseLsq = false;
            MassCalibrationWindow = 6.0;
            MassCalibNumXSlices = 12;
            MassCalibNumYSlices = 50;
            MassCalibMaxJump = 20;
            MassCalibMaxZScore = 3;
            MassCalibLsqMaxZScore = 2.5;
            MassCalibLSQNumKnots = 12;
            MassTolerance = 6.0;
            NETTolerance = 0.03;

            AlignType = AlignmentType.NET_MASS_WARP;
            CalibType = CalibrationType.HYBRID_CALIBRATION;

            AlignToMassTagDatabase = false;
            AlignmentBaselineName = null;
            MassBinSize = 0.2;
            NETBinSize = 0.001;
            DriftTimeBinSize = 0.03;
            AlignSplitMZs = false;
            MzBoundaries = new List<LCMSAlignmentMzBoundary>();
            MzBoundaries.Add(new LCMSAlignmentMzBoundary(0.0, 505.7));
            MzBoundaries.Add(new LCMSAlignmentMzBoundary(505.7, 999999999.0));
            StoreAlignmentFunction = false;
            AlignmentAlgorithmType = FeatureAlignmentType.LCMSWarp;
        }

        public enum CalibrationType
        {
            MZ_CALIBRATION = 0,
            SCAN_CALIBRATION,
            HYBRID_CALIBRATION
        }

        public enum AlignmentType
        {
            NET_WARP = 0,
            NET_MASS_WARP
        }

        public enum FeatureAlignmentType
        {
            LCMSWarp,
            DirectImsInfusion
        }
    }
}
