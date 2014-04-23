using System.Collections.Generic;

namespace Processor
{
    public class AlignmentOptions
    {
        #region Private Values
        int m_numTimeSections;
        int m_topFeatureAbundancePercent;
        int m_contractionFactor;
        int m_maxTimeDistortion;
        int m_maxPromiscuity;

        bool m_usePromiscuousPoints;
        bool m_massCalibUseLsq;
        double m_massCalibrationWindow;
        int m_massCalibNumXSlices;
        int m_massCalibNumYSlices;
        int m_massCalibMaxJump;
        double m_massCalibMaxZScore;
        double m_massCalibLsqMaxZScore;
        int m_massCalibLsqNumKnots;

        double m_massTolerance;
        double m_netTolerance;

        AlignmentType m_alignmentType;
        CalibrationType m_calibrationType;

        bool m_alignToMassTagDatabase;
        string m_alignmentBaselineName;
        double m_massBinSize;
        double m_netBinSize;
        double m_driftTimeBinSize;
        bool m_storeAlignmentFunction;
        bool m_alignSplitMZs;

        List<AlignmentMZBoundary> m_mzBoundaries;

        FeatureAlignmentType m_alignmentAlgorithm;
        #endregion

        #region Public Properties
        public int NumTimeSections
        {
            get { return m_numTimeSections; }
            set { m_numTimeSections = value; }
        }
        public int ContractionFactor
        {
            get { return m_contractionFactor; }
            set { m_contractionFactor = value; }
        }
        public int MaxTimeDistortion
        {
            get { return m_maxTimeDistortion; }
            set { m_maxTimeDistortion = value; }
        }
        public int MaxPromiscuity
        {
            get { return m_maxPromiscuity; }
            set { m_maxPromiscuity = value; }
        }
        public bool UsePromiscuousPoints
        {
            get { return m_usePromiscuousPoints; }
            set { m_usePromiscuousPoints = value; }
        }
        public bool MassCalibUseLsq
        {
            get { return m_massCalibUseLsq; }
            set { m_massCalibUseLsq = value; }
        }
        public double MassCalibrationWindow
        {
            get { return m_massCalibrationWindow; }
            set { m_massCalibrationWindow = value; }
        }
        public int MassCalibNumXSlices
        {
            get { return m_massCalibNumXSlices; }
            set { m_massCalibNumXSlices = value; }
        }
        public int MassCalibNumYSlices
        {
            get { return m_massCalibNumYSlices; }
            set { m_massCalibNumYSlices = value; }
        }
        public int MassCalibMaxJump
        {
            get { return m_massCalibMaxJump; }
            set { m_massCalibMaxJump = value; }
        }
        public double MassCalibMaxZScore
        {
            get { return m_massCalibMaxZScore; }
            set { m_massCalibMaxZScore = value; }
        }
        public double MassCalibLsqMaxZScore
        {
            get { return m_massCalibLsqMaxZScore; }
            set { m_massCalibLsqMaxZScore = value; }
        }
        public int MassCalibLSQNumKnots
        {
            get { return m_massCalibLsqNumKnots; }
            set { m_massCalibLsqNumKnots = value; }
        }
        public double MassTolerance
        {
            get { return m_massTolerance; }
            set { m_massTolerance = value; }
        }
        public double NETTolerance
        {
            get { return m_netTolerance; }
            set { m_netTolerance = value; }
        }
        public AlignmentType AlignType
        {
            get { return m_alignmentType; }
            set { m_alignmentType = value; }
        }
        public CalibrationType CalibType
        {
            get { return m_calibrationType; }
            set { m_calibrationType = value; }
        }
        public bool AlignToMassTagDatabase
        {
            get { return m_alignToMassTagDatabase; }
            set { m_alignToMassTagDatabase = value; }
        }
        public string AlignmentBaselineName
        {
            get { return m_alignmentBaselineName; }
            set { m_alignmentBaselineName = value; }
        }
        public double MassBinSize
        {
            get { return m_massBinSize; }
            set { m_massBinSize = value; }
        }
        public double NETBinSize
        {
            get { return m_netBinSize; }
            set { m_netBinSize = value; }
        }
        public double DriftTimeBinSize
        {
            get { return m_driftTimeBinSize; }
            set { m_driftTimeBinSize = value; }
        }
        public bool AlignSplitMZs
        {
            get { return m_alignSplitMZs; }
            set { m_alignSplitMZs = value; }
        }
        public List<AlignmentMZBoundary> MzBoundaries
        {
            get { return m_mzBoundaries; }
            set { m_mzBoundaries = value; }
        }

        public int TopFeatureAbundancePercent
        {
            get { return m_topFeatureAbundancePercent; }
            set { m_topFeatureAbundancePercent = value; }
        }

        public bool StoreAlignmentFunction
        {
            get { return m_storeAlignmentFunction; }
            set { m_storeAlignmentFunction = value; }
        }

        public FeatureAlignmentType AlignmentAlgorithmType
        {
            get { return m_alignmentAlgorithm; }
            set { m_alignmentAlgorithm = value; }
        }
        #endregion


        public AlignmentOptions()
        {
            m_numTimeSections = 100;
            m_topFeatureAbundancePercent = 0;
            m_contractionFactor = 3;
            m_maxTimeDistortion = 10;
            m_maxPromiscuity = 3;
            m_usePromiscuousPoints = false;
            m_massCalibUseLsq = false;
            m_massCalibrationWindow = 6.0;
            m_massCalibNumXSlices = 12;
            m_massCalibNumYSlices = 50;
            m_massCalibMaxJump = 20;
            m_massCalibMaxZScore = 3;
            m_massCalibLsqMaxZScore = 2.5;
            m_massCalibLsqNumKnots = 12;
            m_massTolerance = 6.0;
            m_netTolerance = 0.03;

            m_alignmentType = AlignmentType.NET_MASS_WARP;
            m_calibrationType = CalibrationType.HYBRID_CALIBRATION;

            m_alignToMassTagDatabase = false;
            m_alignmentBaselineName = null;
            m_massBinSize = 0.2;
            m_netBinSize = 0.001;
            m_driftTimeBinSize = 0.03;
            m_alignSplitMZs = false;
            m_mzBoundaries = new List<AlignmentMZBoundary>();
            m_mzBoundaries.Add(new AlignmentMZBoundary(0.0, 505.7));
            m_mzBoundaries.Add(new AlignmentMZBoundary(505.7, 999999999.0));
            m_storeAlignmentFunction = false;
            m_alignmentAlgorithm = FeatureAlignmentType.LCMSWarp;
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
