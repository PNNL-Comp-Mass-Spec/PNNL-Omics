using System;
using System.Collections.Generic;
using System.IO;

using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// Class that performs LC-MS Warp alignment
    /// </summary>
    public class LCMSWarp: IFeatureMapAligner
    {
        #region Class Members
        private List<double> m_tempFeatureBestDelta;
        private List<int> m_tempFeatureBestIndex;
        private List<int> m_sectionUniqueFeatureIndices;
        private List<int> m_numFeaturesInSections;
        private List<int> m_numFeaturesInBaselineSections;
        private Interpolation m_interpolation;
        private List<double> m_alignmentScore;
        private List<int> m_bestPreviousIndex;
        private double m_netStandardDeviation;
        private double m_minMassNetLikelihood;
        private double m_minBaselineNet;
        private double m_maxBaselineNet;
        private bool m_shouldUseMass;
        private bool m_shouldKeepPromiscuousMatches;
        private double m_minNet;
        private double m_maxNet;
        private CombinedRegression m_mzRecalibration;
        private CombinedRegression m_netRecalibration;
        private int m_massCalNumDeltaBins;
        private int m_massCalNumSlices;
        private int m_massCalNumJump;
        private double m_minScore;
        private LCMSWarpCalibrationType m_calibrationType;
        private double m_massWindow;
        private int m_maxPromiscuousUmcMatches;
        private int m_numMatchesPerSection;
        private int m_numSections;
        private int m_numBaselineSections;
        private int m_maxJump;
        private int m_percentDone;
        private double m_massTolerance;
        private double m_netTolerance;
        private double m_massStandardDeviation;
        private int m_maxSectionDistortion;
        private int m_numMatchesPerBaselineStart;
        private double m_normalizedProbability;
        private double m_u;
        private double m_muMass;
        private double m_muNet;
        private List<AlignmentMatch> m_alignmentFunction;
        private List<MassTimeFeature> m_features;
        private List<MassTimeFeature> m_baselineFeatures;
        private List<FeatureMatch> m_featureMatches;
        private List<double> m_subsectionMatchScores;
        private double m_netSlope;
        private double m_netIntercept;
        private double m_netLinearRSq;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of LCMSWarp
        /// </summary>
        public LCMSWarp()
        {

        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public double NetIntercept { get { return m_netIntercept; } }

        /// <summary>
        /// 
        /// </summary>
        public double NetSlope { get { return m_netSlope; } }

        /// <summary>
        /// 
        /// </summary>
        public double NetLinearRSq { get { return m_netLinearRSq; } }

        /// <summary>
        /// 
        /// </summary>
        public double MassCalibrationWindow { get { return m_massWindow; } }

        /// <summary>
        /// 
        /// </summary>
        public double MassTolerance
        {
            get { return m_massTolerance; }
            set { m_massTolerance = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PercentComplete
        {
            get { return m_percentDone; }
            set { m_percentDone = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public LCMSWarpCalibrationType CalibrationType
        {
            get { return m_calibrationType; }
            set { m_calibrationType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NumSections { get { return m_numSections; } }

        /// <summary>
        /// 
        /// </summary>
        public int NumBaselineSections { get { return m_numBaselineSections; } }

        /// <summary>
        /// 
        /// </summary>
        public int NumMatchesPerSection { get { return m_numMatchesPerSection; } }

        /// <summary>
        /// 
        /// </summary>
        public int NumMatchesPerBaselineStart { get { return m_numMatchesPerBaselineStart; } }

        /// <summary>
        /// 
        /// </summary>
        public int NumCandidateMatches { get { return m_featureMatches.Count; } }

        /// <summary>
        /// 
        /// </summary>
        public List<MassTimeFeature> Features
        {
            get { return m_features; }
            set { m_features = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<MassTimeFeature> ReferenceFeatures
        {
            get { return m_baselineFeatures; }
            set { m_baselineFeatures = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool UseMass
        {
            get { return m_shouldUseMass; }
            set { m_shouldUseMass = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="numMsSections"></param>
        /// <param name="contractionFactor"></param>
        /// <param name="maxJump"></param>
        /// <param name="maxPromiscuity"></param>
        public void SetNetOptions(int numMsSections, int contractionFactor,
            int maxJump, int maxPromiscuity)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="massWindow"></param>
        /// <param name="numMassDeltaBin"></param>
        /// <param name="numSlices"></param>
        /// <param name="massJump"></param>
        /// <param name="zTolerance"></param>
        /// <param name="useLsq"></param>
        public void SetMassCalOptions(double massWindow, int numMassDeltaBin,
            int numSlices, int massJump, double zTolerance, bool useLsq)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="massTolerance"></param>
        /// <param name="netTolerance"></param>
        public void SetTolerances(double massTolerance, double netTolerance)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="numKnots"></param>
        /// <param name="outlierZScore"></param>
        public void SetMassCalLSQOptions(int numKnots, double outlierZScore)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void GenerateCandidateMatches()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void CalculateAlignmentFunction()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void CalculateAlignmentMatrix()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void GetMatchProbabilities()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="massStd"></param>
        /// <param name="netStd"></param>
        /// <param name="massMu"></param>
        /// <param name="netMu"></param>
        public void GetStatistics(out double massStd, out double netStd, out double massMu, out double netMu)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="net"></param>
        /// <param name="mz"></param>
        /// <param name="linearNet"></param>
        /// <param name="customNet"></param>
        /// <param name="linearCustomNet"></param>
        /// <param name="massError"></param>
        /// <param name="massErrorCorrected"></param>
        public void GetResiduals(ref List<double> net, ref List<double> mz,
            ref List<double> linearNet, ref List<double> customNet, ref List<double> linearCustomNet,
            ref List<double> massError, ref List<double> massErrorCorrected)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="aligneeNet"></param>
        /// <param name="referenceNet"></param>
        public void GetAlignmentFunction(ref List<double> aligneeNet, ref List<double> referenceNet)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void ClearAllButData()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void ClearInputData()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void Clear()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="stream"></param>
        public void PrintSubsectionMatchScores(TextWriter stream)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="stream"></param>
        public void PrintAlignmentScores(TextWriter stream)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="fileName"></param>
        public void PrintCandidateMatches(string fileName)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="stream"></param>
        public void PrintAlignmentFunction(TextWriter stream)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="numMsSections"></param>
        /// <param name="contractionFactor"></param>
        /// <param name="maxJump"></param>
        public void PerformAlignment(int numMsSections, int contractionFactor, int maxJump)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void GetTransformedNets()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double GetTransformedNet(double value)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void CalculateAlignmentMatches()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="massBin"></param>
        /// <param name="netBin"></param>
        /// <param name="massErrorBin"></param>
        /// <param name="massErrorFrequency"></param>
        /// <param name="netErrorBin"></param>
        /// <param name="netErrorFrequency"></param>
        public void GetErrorHistograms(double massBin, double netBin, ref List<double> massErrorBin,
            ref List<int> massErrorFrequency, ref List<double> netErrorBin, ref List<int> netErrorFrequency)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="umcIndices"></param>
        /// <param name="umcCalibratedMasses"></param>
        /// <param name="umcAlignedNets"></param>
        public void GetFeatureCalibratedMassesAndAlignedNets(ref List<int> umcIndices,
            ref List<double> umcCalibratedMasses, ref List<double> umcAlignedNets)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="umcIndices"></param>
        /// <param name="umcCalibratedMasses"></param>
        /// <param name="umcAlignedNets"></param>
        /// <param name="umcAlignedScans"></param>
        /// <param name="minScan"></param>
        /// <param name="maxScan"></param>
        public void GetFeatureCalibratedMassesAndAlignedNets(ref List<int> umcIndices,
            ref List<double> umcCalibratedMasses, ref List<double> umcAlignedNets,
            ref List<int> umcAlignedScans, int minScan, int maxScan)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="ppms"></param>
        public void GetMatchDeltas(List<double> ppms)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="minBaselineNet"></param>
        /// <param name="maxBaselineNet"></param>
        /// <param name="minNet"></param>
        /// <param name="maxNet"></param>
        public void GetBounds(out double minBaselineNet, out double maxBaselineNet,
            out double minNet, out double maxNet)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void PerformMassCalibration()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void SetFeatureMassesToOriginal()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void CalculateStandardDeviations()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        public void SetDefaultStandardDeviations()
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="mz"></param>
        /// <param name="net"></param>
        /// <returns></returns>
        public double GetPPMShift(double mz, double net)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="mz"></param>
        /// <returns></returns>
        public double GetPPMShiftFromMz(double mz)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="net"></param>
        /// <returns></returns>
        public double GetPPMShiftFromNet(double net)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="slope"></param>
        /// <param name="intercept"></param>
        /// <param name="rsquare"></param>
        /// <param name="regressionPoints"></param>
        public void GetSlopeAndIntercept(out double slope, out double intercept,
            out double rsquare, ref List<RegressionPoints> regressionPoints)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="subsectionMatchScores"></param>
        /// <param name="aligneeValues"></param>
        /// <param name="refValues"></param>
        /// <param name="standardize"></param>
        public void GetSubsectionMatchScores(ref List<double> subsectionMatchScores,
            ref List<double> aligneeValues, ref List<double> refValues, bool standardize)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="massStandardDeviation"></param>
        /// <param name="netStandardDeviation"></param>
        public void SetMassAndNetStandardDeviation(double massStandardDeviation, double netStandardDeviation)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="umcId2MassTagId"></param>
        public void CalculateAlignmentMatches(ref Dictionary<int, int> umcId2MassTagId)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="nets"></param>
        public void GetTransformedNets(List<int> ids, List<double> nets)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="slope"></param>
        /// <param name="intercept"></param>
		public void GetNETSlopeAndIntercept(out double slope, out double intercept)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for 
        /// </summary>
        /// <param name="matchScans"></param>
        /// <param name="matchNets"></param>
        /// <param name="matchAlignedNet"></param>
        public void GetAlignmentMatchesScansAndNet(ref List<double> matchScans,
            List<double> matchNets, List<double> matchAlignedNet)
        {
            // TODO: Implement 
            throw new NotImplementedException();
        }

        #region IFeatureMapAligner Members
        /// <summary>
        /// Aligns the alignee feature map to the baseline feature map.
        /// </summary>
        /// <param name="aligneeFeatures"></param>
        /// <param name="baselineFeatures"></param>
        public void Align(IList<Feature> aligneeFeatures, IList<Feature> baselineFeatures)
        {
            // TODO: Implement Align
            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        /// TODO: Create comment block for ComputeSectionMatch
        /// </summary>
        /// <param name="msSection"></param>
        /// <param name="sectionMatchingFeatures"></param>
        /// <param name="minNet"></param>
        /// <param name="maxNet"></param>
        private void ComputeSectionMatch(int msSection,
            List<FeatureMatch> sectionMatchingFeatures, double minNet, double maxNet)
        {
            // TODO: Implement ComputeSectionMatch
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for PerformMZMassErrorRegression
        /// </summary>
        private void PerformMZMassErrorRegression()
        {
            // TODO: Implement PerformMZMassErrorRegression
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for PerfromScanMassErrorRegression
        /// </summary>
        private void PerformScanMassErrorRegression()
        {
            // TODO: Implement PerfromScanMassErrorRegression
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create comment block for CalculateNetSlopeAndIntercept
        /// </summary>
        private void CalculateNetSlopeAndIntercept()
        {
            // TODO: Implement CalculateNetSlopeAndIntercept
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for GetCurrentlyStoredSectionMatchScore
        /// </summary>
        /// <param name="maxMissZScore"></param>
        /// <param name="numUniqueFeatures"></param>
        /// <param name="numUnmatchedFeaturesInSection"></param>
        /// <returns></returns>
        private double GetCurrentlyStoredSectionMatchScore(double maxMissZScore,
            int numUniqueFeatures, int numUnmatchedFeaturesInSection)
        {
            // TODO: Implement GetCurrentlyStoredSectionMatchScore
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for GetMatchLikelihood
        /// </summary>
        /// <param name="massDelta"></param>
        /// <param name="netDelta"></param>
        /// <returns></returns>
        private double GetMatchLikelihood(double massDelta, double netDelta)
        {
            // Implement GetMatchLikelihood
            throw new NotImplementedException();
        }
        #endregion
    }
}
