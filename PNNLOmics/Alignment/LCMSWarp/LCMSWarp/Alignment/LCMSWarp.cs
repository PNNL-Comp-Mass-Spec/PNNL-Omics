using System;
using System.Collections.Generic;
using System.Linq;
using LCMS.Regression;
using LCMS.Utilities;

namespace LCMS.Alignment
{
    public class LcmsWarp
    {
        #region Private values
        private readonly List<double> m_tempFeatureBestDelta;
        private readonly List<int> m_tempFeatureBestIndex;

        private readonly List<int> m_sectionUniqueFeatureIndices;
        private readonly List<int> m_numFeaturesInSections;
        private readonly List<int> m_numFeaturesInBaselineSections;

        Interpolation m_interpolation;
        double[] m_alignmentScore;
        int[] m_bestPreviousIndex;

        double m_netStd;

        private const double MinMassNetLikelihood = 1e-4;
        double m_minBaselineNet;
        double m_maxBaselineNet;

        bool m_useMass;
        // Decides whether or not promiscuousmatches are kept in the scoring function for allignment
        // to a MTDD. This can be used safely because MTDB will not have split UMCs but for Ms to MS
        // alignments it is best to keep this false or all split UMCs will match to the first instance.
        readonly bool m_keepPromiscuousMatches;

        double m_maxNet;
        double m_minNet;
        // Used to control the granularity of the MSMS section size when comparing against MS Sections.
        // The number of sectiosn in the MSMS will be # of sectiosn in MS * m_maxSectionDistortion.
        // Thus each section of the MS can be compared to MSMS section wich are 1/m_maxSectionDistortion to
        // m_maxSectionDistortion times the ms section size of the chromatographic run.

        CombinedRegression m_mzRecalibration;
        CombinedRegression m_netRecalibration;

        int m_massCalNumDeltaBins;
        int m_massCalNumSlices;
        int m_massCalNumJump;

        readonly double m_minScore;

        LcmsWarpCalibrationType m_calibrationType;

        // Mass window around which the mass tolerance is applied
        double m_massWindow;
        int m_maxPromiscuousUmcMatches;

        int m_numMatchesPerSection;

        int m_numSections;
        int m_numBaselineSections;
        int m_maxJump;

        int m_percentDone;
        double m_massTolerance;
        double m_netTolerance;

        double m_massStd;
        int m_maxSectionDistotion;
        int m_numMatchesPerBaselineStart;

        double m_normalProb;
        double m_u;
        double m_muMass;
        double m_muNet;
        readonly List<AlignmentMatch> m_alignmentFunc;
        readonly List<MassTimeFeature> m_features;
        readonly List<MassTimeFeature> m_baselineFeatures;
        List<FeatureMatch> m_featureMatches;
        readonly List<double> m_subsectionMatchScores;
        double m_netSlope;
        double m_netIntercept;
        double m_netLinearRsq;
        // Slope and intercept calculated using likelihood score in m_subsectionMatchScores.
        // Range of scans used is the range which covers the start scan of the Q2 and the end
        // scan of the Q3 of the nets of the matched features.
        #endregion

        #region Public properties

        public double MinBaselineNet
        {
            get { return m_minBaselineNet; }
            set { m_minBaselineNet = value; }
        }

        public double MaxBaselineNet
        {
            get { return m_maxBaselineNet; }
            set { m_maxBaselineNet = value; }
        }

        public double MinNet
        {
            get { return m_minNet; }
            set { m_minNet = value; }
        }

        public double MaxNet
        {
            get { return m_maxNet; }
            set { m_maxNet = value; }
        }

        public double MassTolerance
        {
            get { return m_massTolerance; }
            set { m_massTolerance = value; }
        }

        public double NetTolerance
        {
            get { return m_netTolerance; }
            set { m_netTolerance = value; }
        }

        public void ResetPresentComplete()
        {
            m_percentDone = 0;
        }

        public int PercentComplete
        {
            get { return m_percentDone; }
            set { m_percentDone = value; }
        }

        public double NetIntercept
        {
            get { return m_netIntercept; }
            set { m_netIntercept = value; }
        }

        public double NetSlope
        {
            get { return m_netSlope; }
            set { m_netSlope = value; }
        }

        public double NetLinearRsq
        {
            get { return m_netLinearRsq; }
            set { m_netLinearRsq = value; }
        }

        public double MassCalibrationWindow
        {
            get { return m_massWindow; }
            set { m_massWindow = value; }
        }

        public LcmsWarpCalibrationType CalibrationType
        {
            get { return m_calibrationType; }
            set { m_calibrationType = value; }
        }

        public double MassStd
        {
            get { return m_massStd; }
            set { m_massStd = value; }
        }

        public double NetStd
        {
            get { return m_netStd; }
            set { m_netStd = value; }
        }

        public double MassMu
        {
            get { return m_muMass; }
            set { m_muMass = value; }
        }

        public double NetMu
        {
            get { return m_muNet; }
            set { m_muNet = value; }
        }

        public int NumSections
        {
            get { return m_numSections; }
            set { m_numSections = value; }
        }

        public int NumBaselineSections
        {
            get { return m_numBaselineSections; }
            set { m_numBaselineSections = value; }
        }

        public int NumMatchesPerSection
        {
            get { return m_numMatchesPerSection; }
            set { m_numMatchesPerSection = value; }
        }

        public int NumMatchesPerBaseline
        {
            get { return m_numMatchesPerBaselineStart; }
            set { m_numMatchesPerBaselineStart = value; }
        }

        public int NumCandidateMatches
        {
            get { return m_featureMatches.Count; }
        }

        public int MaxSectionDistortion
        {
            get { return m_maxSectionDistotion; }
            set { m_maxSectionDistotion = value; }
        }

        public int MaxJump
        {
            get { return m_maxJump; }
            set { m_maxJump = value; }
        }

        public int NumMatchesPerBaselineStart
        {
            get { return m_numMatchesPerBaselineStart; }
            set { m_numMatchesPerBaselineStart = value; }
        }

        public int MaxPromiscuousUmcMatches
        {
            get { return m_maxPromiscuousUmcMatches; }
            set { m_maxPromiscuousUmcMatches = value; }
        }

        public int MassCalNumDeltaBins
        {
            get { return m_massCalNumDeltaBins; }
            set { m_massCalNumDeltaBins = value; }
        }

        public int MassCalNumSlices
        {
            get { return m_massCalNumSlices; }
            set { m_massCalNumSlices = value; }
        }

        public int MassCalNumJump
        {
            get { return m_massCalNumJump; }
            set { m_massCalNumJump = value; }
        }

        public CombinedRegression MzRecalibration
        {
            get { return m_mzRecalibration; }
            set { m_mzRecalibration = value; }
        }

        public CombinedRegression NetRecalibration
        {
            get { return m_netRecalibration; }
            set { m_netRecalibration = value; }
        }
        #endregion

        // Public Constructor
        public LcmsWarp()
        {
            m_tempFeatureBestDelta = new List<double>();
            m_tempFeatureBestIndex = new List<int>();

            m_sectionUniqueFeatureIndices = new List<int>();
            m_numFeaturesInSections = new List<int>();
            m_numFeaturesInBaselineSections = new List<int>();

            m_interpolation = new Interpolation();

            m_mzRecalibration = new CombinedRegression();
            m_netRecalibration = new CombinedRegression();

            m_alignmentFunc = new List<AlignmentMatch>();
            m_features = new List<MassTimeFeature>();
            m_baselineFeatures = new List<MassTimeFeature>();
            m_featureMatches = new List<FeatureMatch>();
            m_subsectionMatchScores = new List<double>();

            m_useMass = false;
            m_massWindow = 50;
            m_massTolerance = 20;
            m_netTolerance = 0.02;
            m_maxSectionDistotion = 2;
            m_netStd = 0.007;
            m_alignmentScore = null;
            m_bestPreviousIndex = null;
            m_maxJump = 10;
            m_massStd = 20;
            m_percentDone = 0;
            m_maxPromiscuousUmcMatches = 5;
            m_keepPromiscuousMatches = false;
            m_alignmentScore = null;
            m_bestPreviousIndex = null;

            m_calibrationType = LcmsWarpCalibrationType.MZ_REGRESSION;
            m_massCalNumDeltaBins = 100;
            m_massCalNumSlices = 12;
            m_massCalNumJump = 50;

            double ztolerance = 3;
            CombinedRegression.RegressionType regType = CombinedRegression.RegressionType.Central;

            m_mzRecalibration.SetCentralRegressionOptions(m_massCalNumSlices, m_massCalNumDeltaBins, m_massCalNumJump,
                                                          ztolerance, regType);
            m_netRecalibration.SetCentralRegressionOptions(m_massCalNumSlices, m_massCalNumDeltaBins, m_massCalNumJump,
                                                          ztolerance, regType);

            double outlierZScore = 2.5;
            int numKnots = 12;
            m_mzRecalibration.SetLSQOptions(numKnots, outlierZScore);
            m_netRecalibration.SetLSQOptions(numKnots, outlierZScore);

            m_minScore = -100000;
            m_muMass = 0;
            m_muNet = 0;
            m_normalProb = 0.3;
        }

        public void CalculateNetSlopeAndIntercept()
        {
            List<double> startNets = new List<double>();
            foreach (FeatureMatch match in m_featureMatches)
            {
                startNets.Add(match.Net);
            }
            startNets.Sort();

            int numPoints = startNets.Count;
            if (numPoints == 0)
            {
                m_netSlope = 0;
                m_netIntercept = 0;
                return;
            }
            int startSection = Convert.ToInt32(((startNets[numPoints / 4] - m_minNet) * m_numSections) / (m_maxNet - m_minNet));
            int endSection = Convert.ToInt32(((startNets[(3 * numPoints) / 4] - m_minNet) * m_numSections) / (m_maxNet - m_minNet));

            if (startSection >= m_numSections)
            {
                startSection = m_numSections - 1;
            }
            if (endSection >= m_numSections)
            {
                endSection = m_numSections - 1;
            }

            double sumY = 0;
            double sumX = 0;
            double sumXy = 0;
            double sumXx = 0;
            double sumYy = 0;

            int numSumPoints = 0;
            for (int section = startSection; section <= endSection; section++)
            {
                double maxScore = -1 * double.MaxValue;
                double y = 0;

                for (int baselineSection = 0; baselineSection < m_numBaselineSections; baselineSection++)
                {
                    for (int sectionWidth = 0; sectionWidth < m_numMatchesPerBaselineStart; sectionWidth++)
                    {
                        int alignmentIndex = section * m_numMatchesPerSection + baselineSection * m_numMatchesPerBaselineStart + sectionWidth;
                        if (m_subsectionMatchScores[alignmentIndex] > maxScore)
                        {
                            maxScore = m_subsectionMatchScores[alignmentIndex];
                            y = baselineSection;
                        }
                    }
                }

                double net = (section * (m_maxNet - m_minNet)) / m_numSections + m_minNet;
                double alignedNet = (y * (m_maxBaselineNet - m_minBaselineNet)) / m_numBaselineSections + m_minBaselineNet;

                sumX = sumX + net;
                sumY = sumY + alignedNet;
                sumXy = sumXy + (net * alignedNet);
                sumXx = sumXx + (net * net);
                sumYy = sumYy + (alignedNet * alignedNet);
                numSumPoints++;
            }

            m_netSlope = (numSumPoints * sumXy - sumX * sumY) / (numSumPoints * sumXx - sumX * sumX);
            m_netIntercept = (sumY - m_netSlope * sumX) / numSumPoints;

            double temp = (numSumPoints * sumXy - sumX * sumY) / Math.Sqrt((numSumPoints * sumXx - sumX * sumX) * (numSumPoints * sumYy - sumY * sumY));
            m_netLinearRsq = temp * temp;
        }

        public double CurrentlyStoredSectionMatchScore(double maxMissZScore, ref int numUniqueFeatures,
                                                        ref int numUnmatchedFeaturesInSection)
        {
            //Compute match scores for this section: log(P(match of ms section to MSMS section))
            double matchScore = 0;

            double lg2PiStdNetSqrd = Math.Log(2 * Math.PI * m_netStd * m_netStd);
            for (int i = 0; i < numUniqueFeatures; i++)
            {
                int msFeatureIndex = m_sectionUniqueFeatureIndices[i];
                int msmsFeatureIndex = m_tempFeatureBestIndex[msFeatureIndex];
                MassTimeFeature feature = m_features[msFeatureIndex];
                MassTimeFeature baselineFeature = m_baselineFeatures[msmsFeatureIndex];

                double deltaNet = m_tempFeatureBestDelta[msFeatureIndex];

                if (m_useMass)
                {
                    double massDelta = (feature.MonoMass - baselineFeature.MonoMass) * 100000 / baselineFeature.MonoMass;
                    double likelihood = GetMatchLikelihood(massDelta, deltaNet);
                    matchScore = matchScore + Math.Log(likelihood);
                }
                else
                {
                    if (Math.Abs(deltaNet) > m_netTolerance)
                    {
                        //deltaNet = m_netTolerance;
                        numUnmatchedFeaturesInSection++;
                        matchScore = matchScore - 0.5 * (m_netTolerance / m_netStd) * (m_netTolerance / m_netStd);
                        matchScore = matchScore - 0.5 * lg2PiStdNetSqrd;
                    }
                    else
                    {
                        matchScore = matchScore - 0.5 * (deltaNet / m_netStd) * (deltaNet / m_netStd);
                        matchScore = matchScore - 0.5 * lg2PiStdNetSqrd;
                    }
                }
            }
            return matchScore;
        }

        private double GetMatchLikelihood(double massDelta, double netDelta)
        {
            double massZ = massDelta / m_massStd;
            double netZ = netDelta / m_netStd;
            double normProb = Math.Exp(-0.5 * (massZ * massZ + netZ * netZ)) / (2 * Math.PI * m_netStd * m_massStd);
            double likelihood = (normProb * m_normalProb + (1 - m_normalProb) * m_u);
            if (likelihood < MinMassNetLikelihood)
            {
                likelihood = MinMassNetLikelihood;
            }
            return likelihood;
        }

        public void SetNetOptions(int numMsSections, int contractionFactor, int maxJump, int maxPromiscuity)
        {
            m_numSections = numMsSections;
            m_maxSectionDistotion = contractionFactor;
            m_maxJump = maxJump;
            // each MS section can match MS section of size from 1 deivision to distortion ^ 2 divisions.
            m_numBaselineSections = m_numSections * m_maxSectionDistotion;
            m_numMatchesPerBaselineStart = m_maxSectionDistotion * m_maxSectionDistotion;
            m_numMatchesPerSection = m_numBaselineSections * m_numMatchesPerBaselineStart;

            m_maxPromiscuousUmcMatches = maxPromiscuity;
        }

        public void SetMassCalOptions(double massWindow, int numMassDeltaBin, int numSlices, int massJump,
                                      double zTolerance, bool useLsq)
        {
            m_massWindow = massWindow;

            m_massCalNumDeltaBins = numMassDeltaBin;
            m_massCalNumSlices = numSlices;
            m_massCalNumJump = massJump;

            CombinedRegression.RegressionType regType = CombinedRegression.RegressionType.Central;

            if (useLsq)
            {
                regType = CombinedRegression.RegressionType.Hybrid;
            }
            m_mzRecalibration.SetCentralRegressionOptions(m_massCalNumSlices, m_massCalNumDeltaBins,
                                                          m_massCalNumJump, zTolerance, regType);
            m_netRecalibration.SetCentralRegressionOptions(m_massCalNumSlices, m_massCalNumDeltaBins,
                                                          m_massCalNumJump, zTolerance, regType);
        }

        public void SetTolerances(double massTolerance, double netTolerance)
        {
            m_massTolerance = massTolerance;
            m_netTolerance = netTolerance;
        }

        public void SetMassCalLsqOptions(int numKnots, double outlierZScore)
        {
            m_mzRecalibration.SetLSQOptions(numKnots, outlierZScore);
            m_netRecalibration.SetLSQOptions(numKnots, outlierZScore);
        }

        public void GetStatistics(out double massStd, out double netStd, out double massMU, out double netMu)
        {
            massStd = MassStd;
            netStd = NetStd;
            massMU = MassMu;
            netMu = NetMu;
        }

        public double GetPpmShift(double mz, double net)
        {
            double ppmShift = 0;
            switch (m_calibrationType)
            {
                case LcmsWarpCalibrationType.MZ_REGRESSION:
                    ppmShift = m_mzRecalibration.GetPredictedValue(mz);
                    break;
                case LcmsWarpCalibrationType.SCAN_REGRESSION:
                    ppmShift = m_netRecalibration.GetPredictedValue(mz);
                    break;
                case LcmsWarpCalibrationType.BOTH:
                    ppmShift = m_mzRecalibration.GetPredictedValue(mz);
                    ppmShift = ppmShift + m_netRecalibration.GetPredictedValue(mz);
                    break;
            }
            return ppmShift;
        }

        public void GetResiduals(ref List<double> net, ref List<double> mz, ref List<double> linearNet, ref List<double> customNet,
                                 ref List<double> linearCustomNet, ref List<double> massError, ref List<double> massErrorCorrected)
        {
            int count = m_featureMatches.Count;
            net.Capacity = count;
            mz.Capacity = count;
            linearNet.Capacity = count;
            customNet.Capacity = count;
            linearCustomNet.Capacity = count;
            massError.Capacity = count;
            massErrorCorrected.Capacity = count;

            foreach (FeatureMatch match in m_featureMatches)
            {
                MassTimeFeature feature = m_features[match.FeatureIndex];
                double predictedLinear = m_netSlope * match.Net2 + m_netIntercept;
                double ppmMassError = match.PPMMassError;
                double scanNumber = match.Net;

                // NET
                net.Add(scanNumber);
                linearNet.Add(feature.AlignedNet - predictedLinear);
                customNet.Add(match.NETError);
                linearCustomNet.Add(feature.AlignedNet - predictedLinear);

                double ppmShift = 0.0;
                if (m_useMass)
                {
                    ppmShift = GetPpmShift(feature.MZ, scanNumber);
                }
                mz.Add(feature.MZ);
                massError.Add(ppmMassError);

                massErrorCorrected.Add(ppmMassError - ppmShift);
            }
        }

        public void AlignmentFunction(ref List<double> aligneeNet, ref List<double> referenceNet)
        {
            aligneeNet.Clear();
            referenceNet.Clear();
            int numPieces = m_alignmentFunc.Count;
            for (int pieceNum = 0; pieceNum < numPieces; pieceNum++)
            {              
                aligneeNet.Add(m_alignmentFunc[pieceNum].NetStart);
                referenceNet.Add(m_alignmentFunc[pieceNum].NetStart2);
            }
        }

        public void ClearAllButData()
        {
            m_sectionUniqueFeatureIndices.Clear();
            m_numFeaturesInSections.Clear();
            m_numFeaturesInBaselineSections.Clear();
            m_alignmentFunc.Clear();
            m_featureMatches.Clear();
            m_subsectionMatchScores.Clear();
            m_tempFeatureBestDelta.Clear();
            m_tempFeatureBestIndex.Clear();
        }

        public void ClearInputData()
        {
            m_features.Clear();
            m_baselineFeatures.Clear();
        }

        public void Clear()
        {
            ClearInputData();
            ClearAllButData();
        }

        public void PerformAlignment(int numMsSections, int contractionFactor, int maxJump)
        {
            m_numSections = numMsSections;
            m_maxSectionDistotion = contractionFactor;
            m_maxJump = maxJump;

            m_numBaselineSections = m_numSections * m_maxSectionDistotion;
            m_numMatchesPerBaselineStart = m_maxSectionDistotion * m_maxSectionDistotion;

            m_numMatchesPerSection = m_numBaselineSections * m_numMatchesPerBaselineStart;

            GenerateCandidateMatches();
            GetMatchProbabilities();
            CalculateAlignmentMatrix();
            CalculateAlignmentFunction();
        }

        public void GetTransformedNets()
        {
            int featureIndex;
            int numFeatures = m_features.Count;

            MassTimeFeature feature;
            int alignmentFuncLength = m_alignmentFunc.Count;
            var dicSectionToIndex = new Dictionary<int, int>();

            for (int i = 0; i < m_alignmentFunc.Count; i++)
            {
                dicSectionToIndex.Add(m_alignmentFunc[i].SectionStart, i);
            }

            double netStart;
            double netStartBaseline;
            double netEnd;
            double netEndBaseline;
            double NetTransformed;

            for (featureIndex = 0; featureIndex < numFeatures; featureIndex++)
            {
                feature = m_features[featureIndex];
                if (feature.NET < m_alignmentFunc[0].NetStart)
                {
                    netStart = m_alignmentFunc[0].NetStart;
                    netStartBaseline = m_alignmentFunc[0].NetStart2;
                    netEnd = m_alignmentFunc[0].NetEnd;
                    netEndBaseline = m_alignmentFunc[0].NetEnd2;

                    double msNetTransformed = ((feature.NET - netStart) * (netEndBaseline - netStartBaseline)) / (netEnd - netStart) + netStartBaseline;
                    m_features[featureIndex].AlignedNet = msNetTransformed;
                    continue;
                }
                if (feature.NET > m_alignmentFunc[alignmentFuncLength - 1].NetEnd)
                {
                    netStart = m_alignmentFunc[alignmentFuncLength - 1].NetStart;
                    netStartBaseline = m_alignmentFunc[alignmentFuncLength - 1].NetStart2;
                    netEnd = m_alignmentFunc[alignmentFuncLength - 1].NetEnd;
                    netEndBaseline = m_alignmentFunc[alignmentFuncLength - 1].NetEnd2;

                    NetTransformed = ((feature.NET - netStart) * (netEndBaseline - netStartBaseline)) / (netEnd - netStart) + netStartBaseline;
                    m_features[featureIndex].AlignedNet = NetTransformed;
                    continue;
                }

                int msSection1 = Convert.ToInt32(((feature.NET - m_minNet) * m_numSections) / (m_maxNet - m_minNet));
                if (msSection1 >= m_numSections)
                {
                    msSection1 = m_numSections - 1;
                }

                int msSectionIndex = dicSectionToIndex[msSection1];

                netStart = m_alignmentFunc[msSectionIndex].NetStart;
                netEnd = m_alignmentFunc[msSectionIndex].NetEnd;

                netStartBaseline = m_alignmentFunc[msSectionIndex].NetStart2;
                netEndBaseline = m_alignmentFunc[msSectionIndex].NetEnd2;

                NetTransformed = ((feature.NET - netStart) * (netEndBaseline - netStartBaseline)) / (netEnd - netStart) + netStartBaseline;
                m_features[featureIndex].AlignedNet = NetTransformed;
            }
        }

        public double GetTransformedNet(double val)
        {
            int alignmentFuncLen = m_alignmentFunc.Count;
            if (val < m_alignmentFunc[0].NetStart)
            {
                double frontStart = m_alignmentFunc[0].NetStart;
                double frontStartBaseline = m_alignmentFunc[0].NetStart2;
                double frontEnd = m_alignmentFunc[0].NetEnd;
                double frontEndBaseline = m_alignmentFunc[0].NetEnd2;

                double frontTransformed = ((val - frontStart) * (frontEndBaseline - frontStartBaseline)) / (frontEnd - frontStart) + frontStartBaseline;
                return frontTransformed;
            }
            if (val > m_alignmentFunc[alignmentFuncLen - 1].NetEnd)
            {
                double backStart = m_alignmentFunc[alignmentFuncLen - 1].NetStart;
                double backStartBaseline = m_alignmentFunc[alignmentFuncLen - 1].NetStart2;
                double backEnd = m_alignmentFunc[alignmentFuncLen - 1].NetEnd;
                double backEndBaseline = m_alignmentFunc[alignmentFuncLen - 1].NetEnd2;

                double backTransformed = ((val - backStart) * (backEndBaseline - backStartBaseline)) / (backEnd - backStart) + backStartBaseline;
                return backTransformed;
            }

            int msSectionIndex = 0;
            AlignmentMatch match = new AlignmentMatch();

            for (; msSectionIndex < alignmentFuncLen; msSectionIndex++)
            {
                match = m_alignmentFunc[msSectionIndex];
                if (val <= match.NetEnd && val >= match.NetStart)
                {
                    break;
                }
            }

            double netStart = match.NetStart;
            double netEnd = match.NetEnd;

            double netStartBaseline = match.NetStart2;
            double netEndBaseline = match.NetEnd2;

            double netTransformed = ((val - netStart) * (netEndBaseline - netStartBaseline)) / (netEnd - netStart) + netStartBaseline;
            return netTransformed;
        }

        public void CalculateAlignmentMatches()
        {
            m_features.Sort();
            m_baselineFeatures.Sort();

            int featureIndex = 0;
            int baselineFeatureIndex = 0;
            int numFeatures = m_features.Count;
            int numBaselineFeatures = m_baselineFeatures.Count;

            MassTimeFeature feature;
            FeatureMatch match = new FeatureMatch();

            m_featureMatches.Clear();

            double minMatchScore = -0.5 * (m_massTolerance * m_massTolerance) / (m_massStd * m_massStd);
            minMatchScore = minMatchScore - 0.5 * (m_massTolerance * m_massTolerance) / (m_massStd * m_massStd);

            while (featureIndex < numFeatures)
            {
                feature = m_features[featureIndex];

                double massTolerance = feature.MonoMass * m_massTolerance / 1000000;

                if (baselineFeatureIndex == numBaselineFeatures)
                {
                    baselineFeatureIndex = numBaselineFeatures - 1;
                }

                while (baselineFeatureIndex >= 0 && m_baselineFeatures[baselineFeatureIndex].MonoMass > feature.MonoMass - massTolerance)
                {
                    baselineFeatureIndex--;
                }
                baselineFeatureIndex++;

                int bestMatch = int.MaxValue;
                double bestMatchScore = minMatchScore;
                while (baselineFeatureIndex < numBaselineFeatures && m_baselineFeatures[baselineFeatureIndex].MonoMass < feature.MonoMass + massTolerance)
                {
                    if (m_baselineFeatures[baselineFeatureIndex].MonoMass > feature.MonoMass - massTolerance)
                    {
                        //Calculate the mass and net errors
                        double netDiff = m_baselineFeatures[baselineFeatureIndex].NET - feature.AlignedNet;
                        double baselineDrift = m_baselineFeatures[baselineFeatureIndex].DriftTime;
                        double driftDiff = baselineDrift - feature.DriftTime;
                        double massDiff = (m_baselineFeatures[baselineFeatureIndex].MonoMass - feature.MonoMass) * 1000000.0 / feature.MonoMass;

                        //Calculate the match score
                        double matchScore = -0.5 * (netDiff * netDiff) / (m_netStd * m_netStd);
                        matchScore = matchScore - 0.5 * (massDiff * massDiff) / (m_massStd * m_massStd);

                        //If the match score is greater than the best match score, update the holding item
                        if (matchScore > bestMatchScore)
                        {
                            bestMatch = baselineFeatureIndex;
                            bestMatchScore = matchScore;
                            match.FeatureIndex = featureIndex;
                            match.FeatureIndex2 = baselineFeatureIndex;
                            match.Net = feature.NET;
                            match.NETError = netDiff;
                            match.PPMMassError = massDiff;
                            match.DriftError = driftDiff;
                            match.Net2 = m_baselineFeatures[baselineFeatureIndex].NET;
                        }
                    }
                    baselineFeatureIndex++;
                }

                //If we found a match, add it to the list of matches
                if (bestMatch != int.MaxValue)
                {
                    m_featureMatches.Add(match);
                    match = new FeatureMatch();
                }
                featureIndex++;
            }
            CalculateNetSlopeAndIntercept();
        }

        public void CalculateAlignmentMatches(ref Dictionary<int, int> hashUmcId2MassTagId)
        {
            hashUmcId2MassTagId.Clear();
            m_features.Sort();
            m_baselineFeatures.Sort();

            int featureIndex = 0;
            int baselineFeatureIndex = 0;
            int numFeatures = m_features.Count;
            int numBaselineFeatures = m_baselineFeatures.Count;

            MassTimeFeature feature;
            FeatureMatch match = new FeatureMatch();

            m_featureMatches.Clear();

            double minMatchScore = -0.5 * (m_massTolerance * m_massTolerance) / (m_massStd * m_massStd);
            minMatchScore = minMatchScore - 0.5 * (m_massTolerance * m_massTolerance) / (m_massStd * m_massStd);

            while (featureIndex < numFeatures)
            {
                feature = m_features[featureIndex];

                double massTolerance = feature.MonoMass * m_massTolerance / 1000000;

                if (baselineFeatureIndex == numBaselineFeatures)
                {
                    baselineFeatureIndex = numBaselineFeatures - 1;
                }

                while (baselineFeatureIndex >= 0 && m_baselineFeatures[baselineFeatureIndex].MonoMass > feature.MonoMass - massTolerance)
                {
                    baselineFeatureIndex--;
                }
                baselineFeatureIndex++;

                int bestMatch = int.MaxValue;
                double bestMatchScore = minMatchScore;
                while (baselineFeatureIndex < numBaselineFeatures && m_baselineFeatures[baselineFeatureIndex].MonoMass < feature.MonoMass + massTolerance)
                {
                    if (m_baselineFeatures[baselineFeatureIndex].MonoMass > feature.MonoMass - massTolerance)
                    {
                        //Calculate the mass and net errors
                        double netDiff = m_baselineFeatures[baselineFeatureIndex].NET - feature.AlignedNet;
                        double baselineDrift = m_baselineFeatures[baselineFeatureIndex].DriftTime;
                        double driftDiff = baselineDrift - feature.DriftTime;
                        double massDiff = (m_baselineFeatures[baselineFeatureIndex].MonoMass - feature.MonoMass) * 1000000.0 / feature.MonoMass;

                        //Calculate the match score
                        double matchScore = -0.5 * (netDiff * netDiff) / (m_netStd * m_netStd);
                        matchScore = matchScore - 0.5 * (massDiff * massDiff) / (m_massStd * m_massStd);

                        //If the match score is greater than the best match score, update the holding item
                        if (matchScore > bestMatchScore)
                        {
                            bestMatch = baselineFeatureIndex;
                            bestMatchScore = matchScore;
                            match.FeatureIndex = featureIndex;
                            match.FeatureIndex2 = baselineFeatureIndex;
                            match.Net = feature.NET;
                            match.NETError = netDiff;
                            match.PPMMassError = massDiff;
                            match.DriftError = driftDiff;
                            match.Net2 = m_baselineFeatures[baselineFeatureIndex].NET;
                        }
                    }
                    baselineFeatureIndex++;
                }

                //If we found a match, add it to the list of matches
                if (bestMatch != int.MaxValue)
                {
                    m_featureMatches.Add(match);
                    hashUmcId2MassTagId[feature.ID] = m_baselineFeatures[bestMatch].ID;
                }
                featureIndex++;
            }
            CalculateNetSlopeAndIntercept();
        }

        public void GetErrorHistograms(double massBin, double netBin, double driftBin, ref List<double> massErrorBin,
                                       ref List<int> massErrorFrequency, ref List<double> netErrorBin,
                                       ref List<int> netErrorFrequency, ref List<double> driftErrorBin, ref List<int> driftErrorFrequency)
        {
            List<double> massErrors = new List<double>();
            List<double> netErrors = new List<double>();
            List<double> driftErrors = new List<double>();

            int numMatches = m_featureMatches.Count;
            massErrors.Capacity = numMatches;
            netErrors.Capacity = numMatches;
            driftErrors.Capacity = numMatches;

            foreach (FeatureMatch match in m_featureMatches)
            {
                massErrors.Add(match.PPMMassError);
                netErrors.Add(match.NETError);
                driftErrors.Add(match.DriftError);
            }

            Histogram.CreateHistogram(ref massErrors, ref massErrorBin, ref massErrorFrequency, massBin);
            Histogram.CreateHistogram(ref netErrors, ref netErrorBin, ref netErrorFrequency, netBin);
            Histogram.CreateHistogram(ref driftErrors, ref driftErrorBin, ref driftErrorFrequency, driftBin);
        }

        public void SetFeatures(ref List<MassTimeFeature> features)
        {
            m_features.Clear();
            foreach (MassTimeFeature feature in features)
            {
                m_features.Add(feature);
            }
        }

        public void GetFeatureCalibratedMassesAndAlignedNets(ref List<int> umcIndices, ref List<double> umcCalibratedMass,
                                                             ref List<double> umcAlignedNets, ref List<double> alignedDriftTimes)
        {
            foreach (MassTimeFeature feature in m_features)
            {
                umcIndices.Add(feature.ID);
                umcCalibratedMass.Add(feature.MonoMassCalibrated);
                umcAlignedNets.Add(feature.AlignedNet);
                alignedDriftTimes.Add(feature.DriftTime);
            }
        }

        public void GetFeatureCalibratedMassesAndAlignedNets(ref List<int> umcIndices, ref List<double> umcCalibratedMasses,
                                                             ref List<double> umcAlignedNets, ref List<int> umcAlignedScans,
                                                             ref List<double> umcDriftTimes, int minScan, int maxScan)
        {
            int numFeatures = m_features.Count;
            for (int featureNum = 0; featureNum < numFeatures; featureNum++)
            {
                MassTimeFeature feature = m_features[featureNum];
                umcIndices.Add(feature.ID);
                umcCalibratedMasses.Add(feature.MonoMassCalibrated);
                umcAlignedNets.Add(feature.AlignedNet);
                umcAlignedScans.Add(minScan + (int)(feature.AlignedNet * (maxScan - minScan)));
                umcDriftTimes.Add(feature.DriftTime);
            }
        }

        public void SetReferenceFeatures(ref List<MassTimeFeature> features)
        {
            m_baselineFeatures.Clear();
            foreach (MassTimeFeature feature in features)
            {
                m_baselineFeatures.Add(feature);
            }
        }

        public void AddFeature(ref MassTimeFeature feature)
        {
            m_features.Add(feature);
        }

        public void AddReferenceFeature(ref MassTimeFeature feature)
        {
            m_baselineFeatures.Add(feature);
        }
        /*
        public void OldGenerateCandidateMatches()
        {
            if (m_features.Count == 0)
            {
                return;
            }

            MassTimeFeature feature;
            m_features.Sort();
            m_baselineFeatures.Sort();

            ClearAllButData();

            int featureIndex = 0;
            int baselineFeatureIndex = 0;
            int numFeatures = m_features.Count;
            int numBaselineFeatures = m_baselineFeatures.Count;

            if (numBaselineFeatures <= 0)
            {
                return;
            }

            MassTimeFeature baselineFeature;
            

            m_percentDone = 0;

            while (featureIndex < numFeatures)
            {
                m_percentDone = (100 * featureIndex) / numFeatures;
                feature = m_features[featureIndex];

                double massTolerance = feature.MonoMass * m_massTolerance / 1000000;

                if (baselineFeatureIndex == numBaselineFeatures)
                {
                    baselineFeatureIndex = numBaselineFeatures - 1;
                }
                baselineFeature = m_baselineFeatures[baselineFeatureIndex];
                while (baselineFeatureIndex >= 0 && baselineFeature.MonoMass > feature.MonoMass - massTolerance)
                {
                    baselineFeatureIndex--;
                    if (baselineFeatureIndex >= 0)
                    {
                        baselineFeature = m_baselineFeatures[baselineFeatureIndex];
                    }
                }
                baselineFeatureIndex++;

                while (baselineFeatureIndex < numBaselineFeatures && m_baselineFeatures[baselineFeatureIndex].MonoMass < feature.MonoMass + massTolerance)
                {
                    if (m_baselineFeatures[baselineFeatureIndex].MonoMass > feature.MonoMass - massTolerance)
                    {
                        FeatureMatch matchAdd = new FeatureMatch();
                        matchAdd.FeatureIndex = featureIndex;
                        matchAdd.FeatureIndex2 = baselineFeatureIndex;
                        matchAdd.Net = feature.NET;
                        matchAdd.Net2 = m_baselineFeatures[baselineFeatureIndex].NET;

                        m_featureMatches.Add(matchAdd);
                    }
                    baselineFeatureIndex++;
                }
                featureIndex++;
            }

            // After all matches are created, go through all the matches and find a mapping of how many times a 
            // feature is matched to.
            Dictionary<int, List<int>> massTagToMatches = new Dictionary<int, List<int>>();
            Dictionary<int, int> massTagToCount = new Dictionary<int, int>();
            FeatureMatch match;
            int numMatches = m_featureMatches.Count;
            for (int matchIndex = 0; matchIndex < numMatches; matchIndex++)
            {
                match = m_featureMatches[matchIndex];
                int baselineIndex = match.FeatureIndex2;
                if (massTagToCount.ContainsKey(baselineIndex))
                {
                    massTagToCount[baselineIndex]++;
                }
                else
                {
                    massTagToCount[baselineIndex] = 1;
                }
                if (!massTagToMatches.ContainsKey(baselineIndex))
                {
                    List<int> matchIndexList = new List<int>(matchIndex);
                    massTagToMatches.Add(baselineIndex, matchIndexList);
                }
                else
                {
                    massTagToMatches[baselineIndex].Add(matchIndex);
                }
            }

            List<FeatureMatch> tempMatches = new List<FeatureMatch>();
            tempMatches.Capacity = m_featureMatches.Count;
            Dictionary<double, int> netMatchesToIndex = new Dictionary<double, int>();

            for (int index = 0; index < massTagToMatches.Count; index++)//each(KeyValuePair<int, int> entry in massTagToMatches)
            {
                var entry = massTagToMatches.ElementAt(index);
                int baselineIndex = entry.Key;
                int numHits = massTagToCount[baselineIndex];
                if (numHits <= m_maxPromiscuousUmcMatches)
                {
                    int iterator = 0;
                    for (int i = 0; i < numHits; i++)
                    {
                        int matchIndex = entry.Value[iterator];
                        match = m_featureMatches[matchIndex];
                        tempMatches.Add(match);
                        index++;
                        entry = massTagToMatches.ElementAt(index);
                        iterator++;
                    }
                    index--;
                    entry = massTagToMatches.ElementAt(index);
                    iterator--;
                }
                else if (m_keepPromiscuousMatches)
                {
                    netMatchesToIndex.Clear();
                    for (int i = 0; i < numHits; i++)
                    {
                        var matchIndex = entry.Value[index];
                        netMatchesToIndex.Add(m_featureMatches[i].Net, matchIndex);
                        //index++;
                        entry = massTagToMatches.ElementAt(index);
                        //match iterator ++
                    }
                    int netMatchPoint = 0;
                    var netMatch = netMatchesToIndex.ElementAt(netMatchPoint);

                    //scan matches iterator = net matches to index begin
                    for (int i = 0; i < m_maxPromiscuousUmcMatches; i++)
                    {
                        var matchIndex = netMatch.Value;
                        match = m_featureMatches[matchIndex];
                        tempMatches.Add(match);
                        //netMatchPoint++;
                        //netMatch = netMatchesToIndex.ElementAt(netMatchPoint);
                        //int matchIndex = scanMatchesIterator.value;
                    }
                    index--;
                    entry = massTagToMatches.ElementAt(index);
                }
            }

            m_featureMatches = tempMatches;
        }
        */


        // Function generates candidate matches between the MS and MSMS data loaded into
        // features and baseline features respectively.
        // It does so by finding all pairs of MassTimeFeature that match within a provided
        // mass tolerance window

        public void GenerateCandidateMatches()
        {
            if (m_features.Count == 0)
            {
                return;
            }

            MassTimeFeature feature;
            m_features.Sort();
            m_baselineFeatures.Sort();

            ClearAllButData();

            // Go through each MassTimeFeature and see if the next baseline MassTimeFeature matches it
            int featureIndex = 0;
            int baselineFeatureIndex = 0;
            int numFeatures = m_features.Count;
            int numBaselineFeatures = m_baselineFeatures.Count;

            if (numBaselineFeatures <= 0)
            {
                // No baseline to match it to.
                return;
            }

            MassTimeFeature baselineFeature;

            m_percentDone = 0;

            while (featureIndex < numFeatures)
            {
                m_percentDone = (100*featureIndex)/numFeatures;
                feature = m_features[featureIndex];
                double massTolerance = feature.MonoMass*(m_massTolerance/1000000);

                if (baselineFeatureIndex == numBaselineFeatures)
                {
                    baselineFeatureIndex = numBaselineFeatures - 1;
                }
                baselineFeature = m_baselineFeatures[baselineFeatureIndex];
                while (baselineFeatureIndex >= 0 && (baselineFeature.MonoMass > feature.MonoMass - massTolerance))
                {
                    baselineFeatureIndex --;
                    if (baselineFeatureIndex >= 0)
                    {
                        baselineFeature = m_baselineFeatures[baselineFeatureIndex];
                    }
                }
                baselineFeatureIndex++;

                while (baselineFeatureIndex < numBaselineFeatures &&
                       (m_baselineFeatures[baselineFeatureIndex].MonoMass < (feature.MonoMass + massTolerance)))
                {
                    if (m_baselineFeatures[baselineFeatureIndex].MonoMass > (feature.MonoMass - massTolerance))
                    {
                        FeatureMatch matchToAdd = new FeatureMatch();
                        matchToAdd.FeatureIndex = featureIndex;
                        matchToAdd.FeatureIndex2 = baselineFeatureIndex;
                        matchToAdd.Net = feature.NET;
                        matchToAdd.Net2 = m_baselineFeatures[baselineFeatureIndex].NET;

                        m_featureMatches.Add(matchToAdd);
                    }
                    baselineFeatureIndex++;
                }
                featureIndex++;
            }

            // Now that matches have been created, go through all the matches and find a mapping
            // of how many times a basline feature is matched to. Puts the matches into a map from a
            // mass tag id to a list of indexes of feature matches

            Dictionary<int, List<int>> massTagToMatches = new Dictionary<int, List<int>>();
            Dictionary<int, int>       massTagToCount   = new Dictionary<int, int>();
            int numMatches = m_featureMatches.Count();
            FeatureMatch featureMatch;
            for (int matchIndex = 0; matchIndex < numMatches; matchIndex++)
            {
                featureMatch = m_featureMatches[matchIndex];
                int baselineIndex = featureMatch.FeatureIndex2;
                if (!massTagToCount.ContainsKey(baselineIndex))
                {
                    massTagToCount[baselineIndex] = 1;
                }
                else
                {
                    massTagToCount[baselineIndex]++;
                }
                if (!massTagToMatches.ContainsKey(baselineIndex))
                {
                    var matchList = new List<int>();//matchIndex);
                    massTagToMatches.Add(baselineIndex, matchList);
                }
                massTagToMatches[baselineIndex].Add(matchIndex);
            }

            // Now go through each of the baseline features matched and for each one keep at
            // most m_maxPromiscuousUMCMatches (or non if m_keepPromiscuousMatches is false)
            // keeping only the first m_maxPromisuousUMCMatches by scan
            
            List<FeatureMatch> tempMatches = new List<FeatureMatch>();
            tempMatches.Capacity = m_featureMatches.Count;
            Dictionary<double, List<int>> netMatchesToIndex = new Dictionary<double, List<int>>();

            foreach (var matchIterator in massTagToMatches)
            {
                int baselineIndex = matchIterator.Key;
                int numHits = massTagToCount[baselineIndex];
                if (numHits <= m_maxPromiscuousUmcMatches)
                {
                    // add all of these ot the temp matches
                    for (int i = 0; i < numHits; i++)
                    {
                        int matchIndex = matchIterator.Value[i];
                        FeatureMatch match = m_featureMatches[matchIndex];
                        tempMatches.Add(match);
                    }
                }
                else if (m_keepPromiscuousMatches)
                {
                    // keep the matches that have the minimum scan numbers.
                    netMatchesToIndex.Clear();
                    for (int i = 0; i < numHits; i++)
                    {
                        int matchIndex = matchIterator.Value[i];
                        if (!netMatchesToIndex.ContainsKey(m_featureMatches[matchIndex].Net))
                        {
                            var matchList = new List<int>();
                            netMatchesToIndex.Add(m_featureMatches[matchIndex].Net, matchList);
                        }
                        netMatchesToIndex[m_featureMatches[matchIndex].Net].Add(matchIndex);
                    }
                    // Now, keep only the first m_maxPromiscuousUMCMatches in the temp list
                    //var scanMatches = netMatchesToIndex.First();
                    
                    for (int index = 0; index < m_maxPromiscuousUmcMatches; index++)
                    {
                        int matchIndex = netMatchesToIndex.ElementAt(index).Value[0];
                        FeatureMatch match = m_featureMatches[matchIndex];
                        tempMatches.Add(match);
                        //scanMatches = netMatchesToIndex
                    }
                }
            }
            m_featureMatches = tempMatches;
        }


        public void GetMatchDeltas(ref List<double> ppms)
        {
            m_percentDone = 10;
            GenerateCandidateMatches();

            m_percentDone = 20;
            int numMatches = m_featureMatches.Count;
            ppms.Clear();

            for (int matchNum = 0; matchNum < numMatches; matchNum++)
            {
                FeatureMatch match = m_featureMatches[matchNum];
                int msIndex = match.FeatureIndex;
                int baselineIndex = match.FeatureIndex2;

                double massDiff = m_features[msIndex].MonoMass - m_baselineFeatures[baselineIndex].MonoMass;
                double massPpmDiff = (massDiff * 1000.0 * 1000.0) / m_features[msIndex].MonoMass;
                ppms.Add(massPpmDiff);
            }
            m_percentDone = 100;
        }

        public void GetBounds(out double minBaselineNet, out double maxBaselineNet, out double minNet, out double maxNet)
        {
            minBaselineNet = m_minBaselineNet;
            maxBaselineNet = m_maxBaselineNet;
            minNet = m_minNet;
            maxNet = m_maxNet;
        }

        public void PerformMassCalibration()
        {
            switch (m_calibrationType)
            {
                case LcmsWarpCalibrationType.MZ_REGRESSION:
                    PerformMzMassErrorRegression();
                    break;
                case LcmsWarpCalibrationType.SCAN_REGRESSION:
                    PerformScanMassErrorRegression();
                    break;
                case LcmsWarpCalibrationType.BOTH:
                    PerformMzMassErrorRegression();
                    PerformScanMassErrorRegression();
                    break;
            }
        }

        public void SetFeatureMassesToOriginal()
        {
            int numFeatures = m_features.Count;
            for (int featureNum = 0; featureNum < numFeatures; featureNum++)
            {
                m_features[featureNum].MonoMassCalibrated = m_features[featureNum].MonoMassOriginal;
            }
        }

        public void CalculateStandardDeviations()
        {
            int numMatches = m_featureMatches.Count;
            if (numMatches > 6)
            {
                FeatureMatch match;// = new FeatureMatch();
                var massDeltas = new List<double>();
                var netDeltas = new List<double>();
                massDeltas.Capacity = numMatches;
                netDeltas.Capacity = numMatches;
                for (int matchNum = 0; matchNum < numMatches; matchNum++)
                {
                    match = m_featureMatches[matchNum];
                    MassTimeFeature feature = m_features[match.FeatureIndex];
                    MassTimeFeature baselineFeature = m_baselineFeatures[match.FeatureIndex2];
                    double currentMassDelta = ((baselineFeature.MonoMass - feature.MonoMass) * 1000000) / feature.MonoMass;
                    double currentNetDelta = baselineFeature.NET - feature.NET;

                    massDeltas.Add(currentMassDelta);
                    netDeltas.Add(currentNetDelta);
                }
                m_normalProb = 0;
                m_u = 0;
                m_muMass = 0;
                m_muNet = 0;
                MathUtils.TwoDEM(ref massDeltas, ref netDeltas, ref m_normalProb, ref m_u,
                                           ref m_muMass, ref m_muNet, ref m_massStd, ref m_netStd);
            }
        }

        public void SetDefaultStdevs()
        {
            m_netStd = 0.007;
            m_massStd = 20;
        }

        public void UseMassAndNetScore(bool use)
        {
            m_useMass = use;
        }

        public void PerformMzMassErrorRegression()
        {
            //Copy all MZs and mass errors into a list of regression points
            List<RegressionPts> calibrations = new List<RegressionPts>();
            
            int numMatches = m_featureMatches.Count;

            for (int matchNum = 0; matchNum < numMatches; matchNum++)
            {
                RegressionPts calibrationMatch = new RegressionPts();
                FeatureMatch match = m_featureMatches[matchNum];
                MassTimeFeature feature = m_features[match.FeatureIndex];
                MassTimeFeature baselineFeature = m_baselineFeatures[match.FeatureIndex2];
                double ppm = (feature.MonoMass - baselineFeature.MonoMass);
                double mz = feature.MZ;
                double netDiff = baselineFeature.NET - feature.AlignedNet;
                calibrationMatch.X = mz;
                calibrationMatch.NetError = netDiff;
                calibrationMatch.MassError = ppm;

                calibrations.Add(calibrationMatch);
            }
            m_mzRecalibration.CalculateRegressionFunction(ref calibrations);

            int numFeatures = m_features.Count;
            for (int featureNum = 0; featureNum < numFeatures; featureNum++)
            {
                double mz = m_features[featureNum].MZ;
                double mass = m_features[featureNum].MonoMassOriginal;
                double ppmShift = m_mzRecalibration.GetPredictedValue(mz);
                double newMass = mass - (mass * ppmShift) / 1000000;
                m_features[featureNum].MonoMassCalibrated = newMass;
                m_features[featureNum].MonoMass = newMass;
            }

        }

        public void PerformScanMassErrorRegression()
        {
            List<RegressionPts> calibrations = new List<RegressionPts>();
            
            int numMatches = m_featureMatches.Count;

            for (int matchNum = 0; matchNum < numMatches; matchNum++)
            {
                RegressionPts calibrationMatch = new RegressionPts();
                FeatureMatch match = m_featureMatches[matchNum];
                MassTimeFeature feature = m_features[match.FeatureIndex];
                MassTimeFeature baselineFeature = m_baselineFeatures[match.FeatureIndex2];
                double ppm = (feature.MonoMass - baselineFeature.MonoMass);
                double net = feature.NET;
                double netDiff = baselineFeature.NET - feature.AlignedNet;
                calibrationMatch.X = net;
                calibrationMatch.NetError = netDiff;
                calibrationMatch.MassError = ppm;

                calibrations.Add(calibrationMatch);
            }

            m_netRecalibration.CalculateRegressionFunction(ref calibrations);

            int numFeatures = m_features.Count;
            for (int featureNum = 0; featureNum < numFeatures; featureNum++)
            {
   double net = m_features[featureNum].NET;
                double mass = m_features[featureNum].MonoMass;
                double ppmShift = m_mzRecalibration.GetPredictedValue(net);
                double newMass = mass - (mass * ppmShift) / 1000000;
                m_features[featureNum].MonoMassCalibrated = newMass;
                m_features[featureNum].MonoMass = newMass;
            }
        }

        public void GetMatchProbabilities()
        {
            m_percentDone = 0;
            int numFeatures = m_features.Count;

            m_tempFeatureBestDelta.Clear();
            m_tempFeatureBestDelta.Capacity = numFeatures;

            m_tempFeatureBestIndex.Clear();
            m_tempFeatureBestIndex.Capacity = numFeatures;

            //Clear the old features and initialize each section to 0
            m_numFeaturesInSections.Clear();
            m_numFeaturesInSections.Capacity = m_numSections;
            for (int i = 0; i < m_numSections; i++)
            {
                m_numFeaturesInSections.Add(0);
            }

            // Do the same for baseline sections
            m_numFeaturesInBaselineSections.Clear();
            m_numFeaturesInBaselineSections.Capacity = numFeatures;
            for (int i = 0; i < m_numBaselineSections; i++)
            {
                m_numFeaturesInBaselineSections.Add(0);
            }

            m_maxNet = -1 * double.MaxValue;
            m_minNet = double.MaxValue;
            for (int i = 0; i < numFeatures; i++)
            {
                if (m_featureMatches[i].Net > m_maxNet)
                {
                    m_maxNet = m_featureMatches[i].Net;
                }
                if (m_featureMatches[i].Net < m_minNet)
                {
                    m_minNet = m_featureMatches[i].Net;
                }
                m_tempFeatureBestDelta.Add(double.MaxValue);
                m_tempFeatureBestIndex.Add(-1);
            }

            for (int i = 0; i < numFeatures; i++)
            {
                double net = m_featureMatches[i].Net;
                int sectionNum = Convert.ToInt32(((net - m_minNet) * m_numSections) / (m_maxNet - m_minNet));
                if (sectionNum >= m_numSections)
                {
                    sectionNum--;
                }
                m_numFeaturesInSections[sectionNum]++;
            }

            m_minBaselineNet = double.MaxValue;
            m_maxBaselineNet = -1 * double.MaxValue;

            int numBaselineFeatures = m_baselineFeatures.Count;
            for (int i = 0; i < numBaselineFeatures; i++)
            {
                if (m_baselineFeatures[i].NET < m_minBaselineNet)
                {
                    m_minBaselineNet = m_baselineFeatures[i].NET;
                }
                if (m_baselineFeatures[i].NET > m_maxBaselineNet)
                {
                    m_maxBaselineNet = m_baselineFeatures[i].NET;
                }
            }

            for (int i = 0; i < numBaselineFeatures; i++)
            {
                double net = m_baselineFeatures[i].NET;
                int msmsSectionNum = (int)(((net - m_minBaselineNet) * m_numBaselineSections) / (m_maxBaselineNet - m_minBaselineNet));
                if (msmsSectionNum == m_numBaselineSections)
                {
                    msmsSectionNum--;
                }
                m_numFeaturesInBaselineSections[msmsSectionNum]++;
            }

            m_featureMatches.Sort();

            int numMatches = m_featureMatches.Count;

            List<FeatureMatch> sectionFeatures = new List<FeatureMatch>();

            int numSectionMatches = m_numSections * m_numMatchesPerSection;
            m_subsectionMatchScores.Clear();
            m_subsectionMatchScores.Capacity = numSectionMatches;

            for (int i = 0; i < numSectionMatches; i++)
            {
                m_subsectionMatchScores.Add(m_minScore);
            }

            if (numMatches == 0)
            {
                return;
            }

            for (int section = 0; section < m_numSections; section++)
            {
                m_percentDone = (section * 100) / (m_numSections + 1);
                int startMatchIndex = 0;
                sectionFeatures.Clear();
                double sectionStartNet = m_minNet + (section * (m_maxNet - m_minNet)) / m_numSections;
                double sectionEndNet = m_minNet + ((section + 1) * (m_maxNet - m_minNet)) / m_numSections;

                while (startMatchIndex < numMatches && ((m_featureMatches[startMatchIndex].Net) < sectionStartNet))
                {
                    startMatchIndex++;
                }
                int endMatchIndex = startMatchIndex;
                while (endMatchIndex < numMatches && ((m_featureMatches[endMatchIndex].Net) < sectionEndNet))
                {
                    endMatchIndex++;
                }
                if (endMatchIndex != startMatchIndex)
                {
                    for (int index = startMatchIndex; index <= endMatchIndex; index++)
                    {
                        sectionFeatures.Add(m_featureMatches[index]);
                    }
                }
                ComputeSectionMatch(section, ref sectionFeatures, sectionStartNet, sectionEndNet);
            }
        }

        public void CalculateAlignmentFunction()
        {
            m_percentDone = 0;
            int section = m_numSections - 1;

            int bestPreviousAlignmentIndex = -1;
            double bestScore = -1 * double.MaxValue;
            int bestAlignedBaselineSection = -1;
            int numBaselineFeatures = m_baselineFeatures.Count;
            int numUnmatchedBaselineFeaturesSectionStart = numBaselineFeatures;
            int bestAlignmentIndex = -1;

            for (int baselineSection = 0; baselineSection < m_numBaselineSections; baselineSection++)
            {
                // Everything past this section would have remained unmatched.
                for (int sectionWidth = 0; sectionWidth < m_numMatchesPerBaselineStart; sectionWidth++)
                {
                    int numUnmatchedBaselineFeaturesSectionEnd;
                    if (baselineSection + sectionWidth >= m_numBaselineSections)
                    {
                        numUnmatchedBaselineFeaturesSectionEnd = 0;
                    }
                    else
                    {
                        numUnmatchedBaselineFeaturesSectionEnd = numUnmatchedBaselineFeaturesSectionStart - m_numFeaturesInBaselineSections[baselineSection + sectionWidth];
                    }
                    if (numUnmatchedBaselineFeaturesSectionEnd < 0)
                    {
                        //DEGAN 2/20/14 Error message that this is where it becomes less than 0
                    }
                    int alignmentIndex = section * m_numMatchesPerSection + baselineSection * m_numMatchesPerBaselineStart + sectionWidth;

                    double alignmentScore = m_alignmentScore[alignmentIndex];

                    if (alignmentScore > bestScore)
                    {
                        bestScore = alignmentScore;
                        bestPreviousAlignmentIndex = m_bestPreviousIndex[alignmentIndex];
                        bestAlignedBaselineSection = baselineSection;
                        bestAlignmentIndex = alignmentIndex;
                    }
                }
                numUnmatchedBaselineFeaturesSectionStart = numUnmatchedBaselineFeaturesSectionStart - m_numFeaturesInBaselineSections[baselineSection];
            }

            double msmsSectionWidth = (m_maxBaselineNet - m_minBaselineNet) * 1.0 / m_numBaselineSections;

            double netStart = m_minNet + (section * (m_maxNet - m_minNet)) / m_numSections;
            double netEnd = m_minNet + ((section + 1) * (m_maxNet - m_minNet)) / m_numSections;
            int baselineSectionStart = bestAlignedBaselineSection;
            int baselineSectionEnd = baselineSectionStart + bestAlignmentIndex % m_numMatchesPerBaselineStart + 1;
            double baselineStartNet = baselineSectionStart * msmsSectionWidth + m_minBaselineNet;
            double baselineEndNet = baselineSectionEnd * msmsSectionWidth + m_minBaselineNet;

            var match = new AlignmentMatch();
            match.Set(netStart, netEnd, section, m_numSections, baselineStartNet, baselineEndNet, baselineSectionStart,
                      baselineSectionEnd, bestScore, m_subsectionMatchScores[bestAlignmentIndex]);
            m_alignmentFunc.Clear();
            m_alignmentFunc.Add(match);

            while (bestPreviousAlignmentIndex >= 0)
            {
                int sectionStart = (bestPreviousAlignmentIndex / m_numMatchesPerSection); // should be current - 1
                int sectionEnd = sectionStart + 1;
                m_percentDone = 100 - (100 * sectionStart) / m_numSections;

                double nextNetStart = m_minNet + (sectionStart * (m_maxNet - m_minNet)) / m_numSections;
                double nextNetEnd = m_minNet + (sectionEnd * (m_maxNet - m_minNet)) / m_numSections;

                int nextBaselineSectionStart = (bestPreviousAlignmentIndex - (sectionStart * m_numMatchesPerSection)) / m_numMatchesPerBaselineStart;
                int nextBaselineSectionEnd = nextBaselineSectionStart + bestPreviousAlignmentIndex % m_numMatchesPerBaselineStart + 1;
                double nextBaselineStartNet = nextBaselineSectionStart * msmsSectionWidth + m_minBaselineNet;
                double nextBaselineEndNet = nextBaselineSectionEnd * msmsSectionWidth + m_minBaselineNet;
                match = new AlignmentMatch();
                match.Set(nextNetStart, nextNetEnd, sectionStart, sectionEnd, nextBaselineStartNet, nextBaselineEndNet,
                          nextBaselineSectionStart, nextBaselineSectionEnd, m_alignmentScore[bestPreviousAlignmentIndex],
                          m_subsectionMatchScores[bestPreviousAlignmentIndex]);

                bestPreviousAlignmentIndex = m_bestPreviousIndex[bestPreviousAlignmentIndex];
                m_alignmentFunc.Add(match);
            }
            m_alignmentFunc.Sort();
        }

        public void CalculateAlignmentMatrix()
        {
            m_percentDone = 0;
            int numPossibleAlignments = m_numSections * m_numBaselineSections * m_numMatchesPerBaselineStart;

            m_alignmentScore = new double[numPossibleAlignments];
            m_bestPreviousIndex = new int[numPossibleAlignments];

            // Initialize scores to - inf, best previous index to -1
            for (int i = 0; i < numPossibleAlignments; i++)
            {
                m_alignmentScore[i] = -1 * double.MaxValue;
                m_bestPreviousIndex[i] = -1;
            }
            double log2PiStdNetStdNet = Math.Log(2 * Math.PI * m_netStd * m_netStd);

            double unmatchedScore = -0.5 * log2PiStdNetStdNet;
            if (m_netTolerance < 3 * m_netStd)
            {
                unmatchedScore = unmatchedScore - 1 * 0.5 * 9.0;
            }
            else
            {
                unmatchedScore = unmatchedScore - 1.0 * 0.5 * (m_netTolerance * m_netTolerance) / (m_netStd * m_netStd);
            }
            if (m_useMass)
            {
                //Assumes that for the unmatched, the masses were also off at mass tolerance, so use the same threshold from NET
                unmatchedScore = 2 * unmatchedScore;
            }

            int numUnmatchedMsMsFeatures = 0;
            for (int baselineSection = 0; baselineSection < m_numBaselineSections; baselineSection++)
            {
                //Assume everything that was matched was past 3 standard devs in net.
                for (int sectionWidth = 0; sectionWidth < m_numMatchesPerBaselineStart; sectionWidth++)
                {
                    //no need to mulitply with msSection because its 0
                    int alignmentIndex = baselineSection * m_numMatchesPerBaselineStart + sectionWidth;
                    m_alignmentScore[alignmentIndex] = 0;
                }
                numUnmatchedMsMsFeatures = numUnmatchedMsMsFeatures + m_numFeaturesInBaselineSections[baselineSection];
            }

            int numUnmatchedMsFeatures = 0;
            for (int section = 0; section < m_numSections; section++)
            {
                for (int sectionWidth = 0; sectionWidth < m_numMatchesPerBaselineStart; sectionWidth++)
                {
                    int alignmentIndex = section * m_numMatchesPerSection + sectionWidth;
                    m_alignmentScore[alignmentIndex] = m_subsectionMatchScores[alignmentIndex] + unmatchedScore * numUnmatchedMsFeatures;
                }
                numUnmatchedMsFeatures = numUnmatchedMsFeatures + m_numFeaturesInSections[section];
            }

            for (int section = 1; section < m_numSections; section++)
            {
                m_percentDone = (100 * section) / m_numSections;
                for (int baselineSection = 1; baselineSection < m_numBaselineSections; baselineSection++)
                {
                    for (int sectionWidth = 0; sectionWidth < m_numMatchesPerBaselineStart; sectionWidth++)
                    {
                        int alignmentIndex = section * m_numMatchesPerSection + baselineSection * m_numMatchesPerBaselineStart + sectionWidth;

                        double currentBestScore = -1 * double.MaxValue;
                        int bestPreviousAlignmentIndex = 0;

                        for (int previousBaselineSection = baselineSection - 1; previousBaselineSection >= baselineSection - m_numMatchesPerBaselineStart - m_maxJump; previousBaselineSection--)
                        {
                            if (previousBaselineSection < 0)
                            {
                                break;
                            }
                            int maxWidth = baselineSection - previousBaselineSection;
                            if (maxWidth > m_numMatchesPerBaselineStart)
                            {
                                maxWidth = m_numMatchesPerBaselineStart;
                            }
                            int previousBaselineSectionWidth = maxWidth;
                            int previousAlignmentIndex = (section - 1) * m_numMatchesPerSection + previousBaselineSection * m_numMatchesPerBaselineStart + previousBaselineSectionWidth - 1;
                            if (m_alignmentScore[previousAlignmentIndex] > currentBestScore)
                            {
                                 currentBestScore = m_alignmentScore[previousAlignmentIndex];
                                bestPreviousAlignmentIndex = previousAlignmentIndex;
                            }
                        }
                        if (Math.Abs(currentBestScore - -1 * double.MaxValue) > double.Epsilon)
                        {
                            m_alignmentScore[alignmentIndex] = currentBestScore + m_subsectionMatchScores[alignmentIndex];
                            m_bestPreviousIndex[alignmentIndex] = bestPreviousAlignmentIndex;
                        }
                        else
                        {
                            m_alignmentScore[alignmentIndex] = -1 * double.MaxValue;
                        }
                    }
                }
            }
        }

        public void ComputeSectionMatch(int msSection, ref List<FeatureMatch> sectionMatchingFeatures, double minNet, double maxNet)
        {
            int numMatchingFeatures = sectionMatchingFeatures.Count;
            double baselineSectionWidth = (m_maxBaselineNet - m_minBaselineNet) * 1.0 / m_numBaselineSections;
            double maxMissZScore = 3;
            if (maxMissZScore < m_netTolerance / m_netStd)
            {
                maxMissZScore = m_netTolerance / m_netStd;
            }

            // keep track of only the unique indices of ms features because we only want the best matches for each
            m_sectionUniqueFeatureIndices.Clear();
            m_sectionUniqueFeatureIndices.Capacity = numMatchingFeatures;
            for (int i = 0; i < numMatchingFeatures; i++)
            {
                bool found = false;
                FeatureMatch match = sectionMatchingFeatures[i];
                for (int j = 0; j < i; j++)
                {
                    if (match.FeatureIndex == sectionMatchingFeatures[j].FeatureIndex)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    m_sectionUniqueFeatureIndices.Add(match.FeatureIndex);
                }
            }

            int numUniqueFeaturesStart = m_sectionUniqueFeatureIndices.Count;
            int numFeaturesInSectionStart = m_numFeaturesInSections[msSection];

            for (int baselineSectionStart = 0; baselineSectionStart < m_numBaselineSections; baselineSectionStart++)
            {
                double baselineStartNet = m_minBaselineNet + baselineSectionStart * baselineSectionWidth;
                int endSection = baselineSectionStart + m_numMatchesPerBaselineStart;
                if (endSection >= m_numBaselineSections)
                {
                    endSection = m_numBaselineSections;
                }
                int numBaselineFeaturesInSection = 0;
                for (int baselineSectionEnd = baselineSectionStart; baselineSectionEnd < endSection; baselineSectionEnd++)
                {
                    int numUniqueFeatures = numUniqueFeaturesStart;
                    int numFeaturesInSection = numFeaturesInSectionStart;
                    int numUnmatchedFeaturesInSection = numFeaturesInSection - numUniqueFeatures;

                    numBaselineFeaturesInSection = numBaselineFeaturesInSection + m_numFeaturesInBaselineSections[baselineSectionEnd];

                    int sectionIndex = msSection * m_numMatchesPerSection + baselineSectionStart * m_numMatchesPerBaselineStart
                                     + baselineSectionEnd - baselineSectionStart;
                    double baselineEndNet = m_minBaselineNet + (baselineSectionEnd + 1) * baselineSectionWidth;

                    for (int i = 0; i < numUniqueFeatures; i++)
                    {
                        int msFeatureIndex = m_sectionUniqueFeatureIndices[i];
                        m_tempFeatureBestDelta[msFeatureIndex] = double.MaxValue;
                        m_tempFeatureBestIndex[msFeatureIndex] = -1;
                    }

                    // Now that we have msmsSection and matching msSection, transform the scan numbers to nets using a
                    // transformation of the two sections, and use a temporary list to keep only the best match
                    FeatureMatch match;// = new FeatureMatch();
                    for (int i = 0; i < numMatchingFeatures; i++)
                    {
                        match = sectionMatchingFeatures[i];
                        int msFeatureIndex = match.FeatureIndex;
                        double featureNet = match.Net;

                        double transformNet = (featureNet - minNet) * (baselineEndNet - baselineStartNet);
                        transformNet = transformNet / (maxNet - minNet) + baselineStartNet;

                        double deltaMatch = transformNet - match.Net2;
                        if (Math.Abs(deltaMatch) < Math.Abs(m_tempFeatureBestDelta[msFeatureIndex]))
                        {
                            m_tempFeatureBestDelta[msFeatureIndex] = deltaMatch;
                            m_tempFeatureBestIndex[msFeatureIndex] = match.FeatureIndex2;
                        }
                    }

                    m_subsectionMatchScores[sectionIndex] = CurrentlyStoredSectionMatchScore(maxMissZScore,
                                                            ref numUniqueFeatures, ref numUnmatchedFeaturesInSection);
                }
            }
        }

        public double GetPpmShiftFromMz(double mz)
        {
            if (CalibrationType == LcmsWarpCalibrationType.MZ_REGRESSION || CalibrationType == LcmsWarpCalibrationType.BOTH)
            {
                return m_mzRecalibration.GetPredictedValue(mz);
            }
            return 0;
        }

        public double GetPpmShiftFromNet(double net)
        {
            if (CalibrationType == LcmsWarpCalibrationType.SCAN_REGRESSION || CalibrationType == LcmsWarpCalibrationType.BOTH)
            {
                return m_netRecalibration.GetPredictedValue(net);
            }
            return 0;
        }

        public void GetSlopeAndIntercept(ref double slope, ref double intercept, ref double rsquare, ref List<RegressionPts> regressionPoints)
        {
            double sumY = 0;
            double sumX = 0;
            double sumXy = 0;
            double sumXx = 0;
            double sumYy = 0;

            var numPoints = regressionPoints.Count;
            for (var index = 0; index < numPoints; index++)
            {
                var point = regressionPoints[index];

                sumX = sumX + point.X;
                sumY = sumY + point.MassError;
                sumXx = sumXx + point.X * point.X;
                sumXy = sumXy + point.X * point.MassError;
                sumYy = sumYy + point.MassError * point.MassError;
            }

            slope = (numPoints * sumXy - sumX * sumY) / (numPoints * sumXx - sumX * sumX);
            intercept = (sumY - slope * sumX) / numPoints;

            var temp = (numPoints * sumXy - sumX * sumY) / Math.Sqrt((numPoints * sumXx - sumX * sumX) * (numPoints * sumYy - sumY * sumY));
            rsquare = temp * temp;
        }

        public void GetSubsectionMatchScore(ref List<double> subsectionMatchScores, ref List<double> aligneeVals,
                                            ref List<double> refVals, bool standardize)
        {
            subsectionMatchScores.Clear();
            for (var msSection = 0; msSection < m_numSections; msSection++)
            {
                aligneeVals.Add(m_minNet + (msSection * (m_maxNet - m_minNet)) / m_numSections);
            }
            for (var msmsSection = 0; msmsSection < m_numBaselineSections; msmsSection++)
            {
                refVals.Add(m_minBaselineNet + (msmsSection * (m_maxBaselineNet - m_minBaselineNet)) / m_numBaselineSections);
            }

            for (var msSection = 0; msSection < m_numSections; msSection++)
            {
                for (var msmsSection = 0; msmsSection < m_numBaselineSections; msmsSection++)
                {
                    var maxScore = -1 * double.MaxValue;
                    for (var msmsSectionWidth = 0; msmsSectionWidth < m_numMatchesPerBaselineStart; msmsSectionWidth++)
                    {
                        if (msmsSection + msmsSectionWidth >= m_numBaselineSections)
                        {
                            continue;
                        }
                        var index = msSection * m_numMatchesPerSection + msmsSection * m_numMatchesPerBaselineStart + msmsSectionWidth;
                        if (m_subsectionMatchScores[index] > maxScore)
                        {
                            maxScore = m_subsectionMatchScores[index];
                        }
                    }
                    subsectionMatchScores.Add(maxScore);
                }
            }
            if (standardize)
            {
                var index = 0;
                for (var msSection = 0; msSection < m_numSections; msSection++)
                {
                    double sumX = 0, sumXx = 0;
                    var startIndex = index;
                    var realMinScore = double.MaxValue;
                    var numPoints = 0;
                    for (var msmsSection = 0; msmsSection < m_numBaselineSections; msmsSection++)
                    {
                        var score = subsectionMatchScores[index];
                        if (Math.Abs(score - m_minScore) > double.Epsilon)
                        {
                            if (score < realMinScore)
                            {
                                realMinScore = score;
                            }
                            sumX = sumX + score;
                            sumXx = sumXx + score * score;
                            numPoints++;
                        }
                        index++;
                    }
                    double var = 0;
                    if (numPoints > 1)
                    {
                        var = (sumXx - sumX * sumX / numPoints) / (numPoints - 1);
                    }
                    double stDev = 1;
                    double avg = 0;
                    if (numPoints >= 1)
                    {
                        avg = sumX / numPoints;
                    }
                    if (Math.Abs(var) > double.Epsilon)
                    {
                        stDev = Math.Sqrt(var);
                    }

                    index = startIndex;
                    for (int msmsSection = 0; msmsSection < m_numBaselineSections; msmsSection++)
                    {
                        double score = subsectionMatchScores[index];
                        if (Math.Abs(score - m_minScore) < double.Epsilon)
                        {
                            score = realMinScore;
                        }
                        if (numPoints > 1)
                        {
                            subsectionMatchScores[index] = ((score - avg) / stDev);
                        }
                        else
                        {
                            subsectionMatchScores[index] = 0;
                        }
                        index++;
                    }
                }
            }
        }

        public void SetMassAndNetStdev(double massStdev, double netStdev)
        {
            m_massStd = massStdev;
            m_netStd = netStdev;
        }

        public void GetTransformedNets(ref List<int> ids, ref List<double> nets)
        {
            ids.Clear();
            nets.Clear();
            var numPoints = m_features.Count;
            for (var pointNum = 0; pointNum < numPoints; pointNum++)
            {
                var feature = m_features[pointNum];
                ids.Add(feature.ID);
                nets.Add(feature.AlignedNet);
            }
        }

        public void GetNetSlopeAndIntercept(out double slope, out double intercept)
        {
            slope = m_netSlope;
            intercept = m_netIntercept;
        }

        public void GetAlignmentMatchesScansAndNet(ref List<double> matchScans, ref List<double> matchNets,
                                                   ref List<double> matchAlignedNet)
        {
            foreach (var match in m_featureMatches)
            {
                matchAlignedNet.Add(m_features[match.FeatureIndex].AlignedNet);
                matchScans.Add(match.Net);
                matchNets.Add(match.Net2);
            }
        }




    }

    public enum LcmsWarpCalibrationType
    {
        MZ_REGRESSION = 0,
        SCAN_REGRESSION,
        BOTH
    };
}
