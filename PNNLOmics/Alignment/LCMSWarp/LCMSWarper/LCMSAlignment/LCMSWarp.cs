using PNNLOmics.Alignment.LCMSWarp.LCMSWarper.LCMSRegression;
using PNNLOmics.Alignment.LCMSWarp.LCMSWarper.LCMSUtilities;
using PNNLOmics.Data.Features;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PNNLOmics.Alignment.LCMSWarp.LCMSWarper.LCMSAlignment
{
    /// <summary>
    /// Object which performs the LCMS Warping functionality
    /// </summary>
    public class LcmsWarp
    {
        #region Private values
        private readonly List<double> m_tempFeatureBestDelta;
        private readonly List<int> m_tempFeatureBestIndex;

        private readonly List<int> m_sectionUniqueFeatureIndices;
        private readonly List<int> m_numFeaturesInSections;
        private readonly List<int> m_numFeaturesInBaselineSections;

        //Interpolation m_interpolation;
        private double[] m_alignmentScore;
        private int[] m_bestPreviousIndex;

        private double m_netStd;

        private const double MinMassNetLikelihood = 1e-4;

        private bool m_useMass;
        // Decides whether or not promiscuousmatches are kept in the scoring function for allignment
        // to a MTDD. This can be used safely because MTDB will not have split UMCs but for Ms to MS
        // alignments it is best to keep this false or all split UMCs will match to the first instance.
        private readonly bool m_keepPromiscuousMatches;

        // Used to control the granularity of the MSMS section size when comparing against MS Sections.
        // The number of sectiosn in the MSMS will be # of sectiosn in MS * m_maxSectionDistortion.
        // Thus each section of the MS can be compared to MSMS section wich are 1/m_maxSectionDistortion to
        // m_maxSectionDistortion times the ms section size of the chromatographic run.

        private readonly double m_minScore;

        // Mass window around which the mass tolerance is applied

        private double m_massStd;

        private double m_normalProb;
        private double m_u;
        private double m_muMass;
        private double m_muNet;
        private readonly List<LcmsAlignmentMatch> m_alignmentFunc;
        private readonly List<UMCLight> m_features;
        private readonly List<UMCLight> m_baselineFeatures;
        private List<LcmsFeatureMatch> m_featureMatches;
        private readonly List<double> m_subsectionMatchScores;
        // Slope and intercept calculated using likelihood score in m_subsectionMatchScores.
        // Range of scans used is the range which covers the start scan of the Q2 and the end
        // scan of the Q3 of the nets of the matched features.
        #endregion

        #region Public properties

        /// <summary>
        /// AutoProperty for MinNet for the LCMS Baseline features
        /// </summary>
        public double MinBaselineNet { get; set; }

        /// <summary>
        /// AutoProperty for MaxNet for the LCMS Baseline features
        /// </summary>
        public double MaxBaselineNet { get; set; }

        /// <summary>
        /// AutoProperty for MinNet for the LCMS alignee features
        /// </summary>
        public double MinNet { get; set; }

        /// <summary>
        /// AutoProperty for MaxNet for the LCMS alignee features
        /// </summary>
        public double MaxNet { get; set; }

        /// <summary>
        /// AutoProperty for LCMS Mass Tolerance
        /// </summary>
        public double MassTolerance { get; set; }

        /// <summary>
        /// AutoProperty for LCMS Net Tolerance
        /// </summary>
        public double NetTolerance { get; set; }

        /// <summary>
        /// AutoProperty for Percent Complete
        /// </summary>
        public int PercentComplete { get; set; }

        /// <summary>
        /// AutoProperty for the Net Intercept of what would be a linear regression of the sets
        /// </summary>
        public double NetIntercept { get; set; }

        /// <summary>
        /// AutoProperty for the Net Slope of what would be a linear regression of the sets
        /// </summary>
        public double NetSlope { get; set; }

        /// <summary>
        /// AutoProperty for the R-Squared of the linear regression of the sets
        /// </summary>
        public double NetLinearRsq { get; set; }

        /// <summary>
        /// AutoProperty for the Mass Calibration Window of the LCMS warper
        /// </summary>
        public double MassCalibrationWindow { get; set; }

        /// <summary>
        /// AutoProperty for the LCMSWarp calibration type
        /// </summary>
        public LcmsWarpCalibrationType CalibrationType { get; set; }

        /// <summary>
        /// Property to get and set the mass standard deviation
        /// </summary>
        public double MassStd
        {
            get { return m_massStd; }
            set { m_massStd = value; }
        }

        /// <summary>
        /// Property to get and set the net standard deviation
        /// </summary>
        public double NetStd
        {
            get { return m_netStd; }
            set { m_netStd = value; }
        }

        /// <summary>
        /// Property to get and set the mass mean value
        /// </summary>
        public double MassMu
        {
            get { return m_muMass; }
            set { m_muMass = value; }
        }

        /// <summary>
        /// Property to get and set the net mean value
        /// </summary>
        public double NetMu
        {
            get { return m_muNet; }
            set { m_muNet = value; }
        }

        /// <summary>
        /// AutoProperty for the number of sections that the alignee features is split into
        /// </summary>
        public int NumSections { get; set; }

        /// <summary>
        /// AutoProperty for the number of sections that the baseline is split into
        /// </summary>
        public int NumBaselineSections { get; set; }

        /// <summary>
        /// AutoProperty for the number of matches in an alignee section
        /// </summary>
        public int NumMatchesPerSection { get; set; }

        /// <summary>
        /// AutoProperty for the number of matches in a baseline section
        /// </summary>
        public int NumMatchesPerBaseline { get; set; }

        /// <summary>
        /// Property to get the number of feature matches found
        /// </summary>
        public int NumCandidateMatches
        {
            get { return m_featureMatches.Count; }
        }

        /// <summary>
        /// AutoProperty for the contraction factor of the warper
        /// </summary>
        public int MaxSectionDistortion { get; set; }

        /// <summary>
        /// AutoProperty for the Max Time distortion of the warper
        /// </summary>
        public int MaxJump { get; set; }
        
        /// <summary>
        /// AutoProperty for the number of promiscuous matches above which to filter UMC matches
        /// </summary>
        public int MaxPromiscuousUmcMatches { get; set; }

        /// <summary>
        /// AutoProperty for the number of "y" bins during regression
        /// </summary>
        public int MassCalNumDeltaBins { get; set; }

        /// <summary>
        /// AutoProperty for the number of "x" bins during regression
        /// </summary>
        public int MassCalNumSlices { get; set; }

        /// <summary>
        /// AutoProperty for the Max calibration jump during regression
        /// </summary>
        public int MassCalNumJump { get; set; }

        /// <summary>
        /// AutoProperty to hold the MzCalibration object
        /// </summary>
        public LcmsCombinedRegression MzRecalibration { get; set; }

        /// <summary>
        /// AutoProperty to hold the NetCalibration object
        /// </summary>
        public LcmsCombinedRegression NetRecalibration { get; set; }

        #endregion

        /// <summary>
        /// Public Constructor, doesn't take arguements, initializes memory space and sets it
        /// to default values
        /// </summary>
 
        public LcmsWarp()
        {
            m_tempFeatureBestDelta = new List<double>();
            m_tempFeatureBestIndex = new List<int>();

            m_sectionUniqueFeatureIndices = new List<int>();
            m_numFeaturesInSections = new List<int>();
            m_numFeaturesInBaselineSections = new List<int>();

            //m_interpolation = new Interpolation();

            MzRecalibration = new LcmsCombinedRegression();
            NetRecalibration = new LcmsCombinedRegression();

            m_alignmentFunc = new List<LcmsAlignmentMatch>();
            m_features = new List<UMCLight>();
            m_baselineFeatures = new List<UMCLight>();
            m_featureMatches = new List<LcmsFeatureMatch>();
            m_subsectionMatchScores = new List<double>();

            m_useMass = false;
            MassCalibrationWindow = 50;
            MassTolerance = 20;
            NetTolerance = 0.02;
            MaxSectionDistortion = 2;
            m_netStd = 0.007;
            m_alignmentScore = null;
            m_bestPreviousIndex = null;
            MaxJump = 10;
            m_massStd = 20;
            PercentComplete = 0;
            MaxPromiscuousUmcMatches = 5;
            m_keepPromiscuousMatches = false;
            m_alignmentScore = null;
            m_bestPreviousIndex = null;

            CalibrationType = LcmsWarpCalibrationType.MZ_REGRESSION;
            MassCalNumDeltaBins = 100;
            MassCalNumSlices = 12;
            MassCalNumJump = 50;

            var ztolerance = 3;
            var regType = LcmsCombinedRegression.RegressionType.CENTRAL;

            MzRecalibration.SetCentralRegressionOptions(MassCalNumSlices, MassCalNumDeltaBins, MassCalNumJump,
                                                          ztolerance, regType);
            NetRecalibration.SetCentralRegressionOptions(MassCalNumSlices, MassCalNumDeltaBins, MassCalNumJump,
                                                          ztolerance, regType);

            var outlierZScore = 2.5;
            var numKnots = 12;
            MzRecalibration.SetLsqOptions(numKnots, outlierZScore);
            NetRecalibration.SetLsqOptions(numKnots, outlierZScore);

            m_minScore = -100000;
            m_muMass = 0;
            m_muNet = 0;
            m_normalProb = 0.3;
        }

        /// <summary>
        /// Method which calculates the Net slope, intercept and r squared as if
        /// a linear regression was performed
        /// </summary>
        public void CalculateNetSlopeAndIntercept()
        {
            var startNets = m_featureMatches.Select(match => match.Net).ToList();
            startNets.Sort();

            int numPoints = startNets.Count;
            if (numPoints == 0)
            {
                NetSlope = 0;
                NetIntercept = 0;
                return;
            }
            int startSection = Convert.ToInt32(((startNets[numPoints / 4] - MinNet) * NumSections) / (MaxNet - MinNet));
            int endSection = Convert.ToInt32(((startNets[(3 * numPoints) / 4] - MinNet) * NumSections) / (MaxNet - MinNet));

            if (startSection >= NumSections)
            {
                startSection = NumSections - 1;
            }
            if (endSection >= NumSections)
            {
                endSection = NumSections - 1;
            }

            double sumY = 0;
            double sumX = 0;
            double sumXy = 0;
            double sumXx = 0;
            double sumYy = 0;

            int numSumPoints = 0;
            for (int section = startSection; section <= endSection; section++)
            {
                double maxScore = double.MinValue;
                double y = 0;

                for (int baselineSection = 0; baselineSection < NumBaselineSections; baselineSection++)
                {
                    for (int sectionWidth = 0; sectionWidth < NumMatchesPerBaseline; sectionWidth++)
                    {
                        int alignmentIndex = (section * NumMatchesPerSection) + (baselineSection * NumMatchesPerBaseline) + sectionWidth;
                        if (m_subsectionMatchScores[alignmentIndex] > maxScore)
                        {
                            maxScore = m_subsectionMatchScores[alignmentIndex];
                            y = baselineSection;
                        }
                    }
                }

                double net = ((section * (MaxNet - MinNet)) / NumSections) + MinNet;
                double alignedNet = ((y * (MaxBaselineNet - MinBaselineNet)) / NumBaselineSections) + MinBaselineNet;

                sumX = sumX + net;
                sumY = sumY + alignedNet;
                sumXy = sumXy + (net * alignedNet);
                sumXx = sumXx + (net * net);
                sumYy = sumYy + (alignedNet * alignedNet);
                numSumPoints++;
            }

            NetSlope = ((numSumPoints * sumXy) - (sumX * sumY)) / ((numSumPoints * sumXx) - (sumX * sumX));
            NetIntercept = (sumY - NetSlope * sumX) / numSumPoints;

            double temp = ((numSumPoints * sumXy) - (sumX * sumY)) / Math.Sqrt(((numSumPoints * sumXx) - (sumX * sumX)) * ((numSumPoints * sumYy) - (sumY * sumY)));
            NetLinearRsq = temp * temp;
        }

        /// <summary>
        /// Compute match scores for this section: log(P(match of ms section to MSMS section))
        /// Does this within the Net Tolerance of the LCMSWarper
        /// </summary>
        /// <param name="numUniqueFeatures"></param>
        /// <param name="numUnmatchedFeaturesInSection"></param>
        /// <returns></returns>
        public double CurrentlyStoredSectionMatchScore(int numUniqueFeatures,
                                                       int numUnmatchedFeaturesInSection)
        {
            //Compute match scores for this section: log(P(match of ms section to MSMS section))
            double matchScore = 0;

            double lg2PiStdNetSqrd = Math.Log(2 * Math.PI * m_netStd * m_netStd);
            for (int i = 0; i < numUniqueFeatures; i++)
            {
                int msFeatureIndex = m_sectionUniqueFeatureIndices[i];
                int msmsFeatureIndex = m_tempFeatureBestIndex[msFeatureIndex];
                UMCLight feature = m_features[msFeatureIndex];
                UMCLight baselineFeature = m_baselineFeatures[msmsFeatureIndex];

                double deltaNet = m_tempFeatureBestDelta[msFeatureIndex];

                if (m_useMass)
                {
                    double massDelta = (feature.MassMonoisotopic - baselineFeature.MassMonoisotopic) * 100000 / baselineFeature.MassMonoisotopic;
                    double likelihood = GetMatchLikelihood(massDelta, deltaNet);
                    matchScore = matchScore + Math.Log(likelihood);
                }
                else
                {
                    if (Math.Abs(deltaNet) > NetTolerance)
                    {
                        numUnmatchedFeaturesInSection++;
                        matchScore = matchScore - 0.5 * (NetTolerance / m_netStd) * (NetTolerance / m_netStd);
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
            double normProb = Math.Exp(-0.5 * ((massZ * massZ) + (netZ * netZ))) / (2 * Math.PI * m_netStd * m_massStd);
            double likelihood = (normProb * m_normalProb + ((1 - m_normalProb) * m_u));
            if (likelihood < MinMassNetLikelihood)
            {
                likelihood = MinMassNetLikelihood;
            }
            return likelihood;
        }

        /// <summary>
        /// Sets up the options for warping regarding the Normalized Elution Time
        /// Each MS section can match MS section of size from 1 deivision to distortion ^ 2 divisions.
        /// </summary>
        /// <param name="numMsSections"></param>
        /// <param name="contractionFactor"></param>
        /// <param name="maxJump"></param>
        /// <param name="maxPromiscuity"></param>
        public void SetNetOptions(int numMsSections, int contractionFactor, int maxJump, int maxPromiscuity)
        {
            NumSections = numMsSections;
            MaxSectionDistortion = contractionFactor;
            MaxJump = maxJump;
            // each MS section can match MS section of size from 1 deivision to distortion ^ 2 divisions.
            NumBaselineSections = NumSections * MaxSectionDistortion;
            NumMatchesPerBaseline = MaxSectionDistortion * MaxSectionDistortion;
            NumMatchesPerSection = NumBaselineSections * NumMatchesPerBaseline;

            MaxPromiscuousUmcMatches = maxPromiscuity;
        }

        /// <summary>
        /// Method to set the Mass Calibration options for LCMS Warp
        /// </summary>
        /// <param name="massWindow"></param>
        /// <param name="numMassDeltaBin"></param>
        /// <param name="numSlices"></param>
        /// <param name="massJump"></param>
        /// <param name="zTolerance"></param>
        /// <param name="useLsq"></param>
        public void SetMassCalOptions(double massWindow, int numMassDeltaBin, int numSlices, int massJump,
                                      double zTolerance, bool useLsq)
        {
            MassCalibrationWindow = massWindow;

            MassCalNumDeltaBins = numMassDeltaBin;
            MassCalNumSlices = numSlices;
            MassCalNumJump = massJump;

            var regType = LcmsCombinedRegression.RegressionType.CENTRAL;

            if (useLsq)
            {
                regType = LcmsCombinedRegression.RegressionType.HYBRID;
            }
            MzRecalibration.SetCentralRegressionOptions(MassCalNumSlices, MassCalNumDeltaBins,
                                                          MassCalNumJump, zTolerance, regType);
            NetRecalibration.SetCentralRegressionOptions(MassCalNumSlices, MassCalNumDeltaBins,
                                                          MassCalNumJump, zTolerance, regType);
        }

        /// <summary>
        /// Method to set both the Net and Mass tolerance at once
        /// </summary>
        /// <param name="massTolerance"></param>
        /// <param name="netTolerance"></param>
        public void SetTolerances(double massTolerance, double netTolerance)
        {
            MassTolerance = massTolerance;
            NetTolerance = netTolerance;
        }

        /// <summary>
        /// Sets LSQ options for both Mz recalibration and Net recalibration
        /// </summary>
        /// <param name="numKnots"></param>
        /// <param name="outlierZScore"></param>
        public void SetMassCalLsqOptions(int numKnots, double outlierZScore)
        {
            MzRecalibration.SetLsqOptions(numKnots, outlierZScore);
            NetRecalibration.SetLsqOptions(numKnots, outlierZScore);
        }

        /// <summary>
        /// Method to grab the Mass and NET statistical data from the LCMS Warper
        /// </summary>
        /// <param name="massStd"></param>
        /// <param name="netStd"></param>
        /// <param name="massMu"></param>
        /// <param name="netMu"></param>
        public void GetStatistics(out double massStd, out double netStd, out double massMu, out double netMu)
        {
            massStd = MassStd;
            netStd = NetStd;
            massMu = MassMu;
            netMu = NetMu;
        }

        /// <summary>
        /// Method to, given a mz and a net, get the ppm shift based on the regression method
        /// for the LCMS warper
        /// </summary>
        /// <param name="mz"></param>
        /// <param name="net"></param>
        /// <returns></returns>
        public double GetPpmShift(double mz, double net)
        {
            double ppmShift = 0;
            switch (CalibrationType)
            {
                case LcmsWarpCalibrationType.MZ_REGRESSION:
                    ppmShift = MzRecalibration.GetPredictedValue(mz);
                    break;
                case LcmsWarpCalibrationType.SCAN_REGRESSION:
                    ppmShift = NetRecalibration.GetPredictedValue(net);
                    break;
                case LcmsWarpCalibrationType.BOTH:
                    ppmShift = MzRecalibration.GetPredictedValue(mz);
                    ppmShift = ppmShift + NetRecalibration.GetPredictedValue(net);
                    break;
            }
            return ppmShift;
        }

        /// <summary>
        /// Updates the lists passed in with the residual data from the Warper
        /// </summary>
        /// <param name="net"></param>
        /// <param name="mz"></param>
        /// <param name="linearNet"></param>
        /// <param name="customNet"></param>
        /// <param name="linearCustomNet"></param>
        /// <param name="massError"></param>
        /// <param name="massErrorCorrected"></param>
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

            foreach (LcmsFeatureMatch match in m_featureMatches)
            {
                UMCLight feature = m_features[match.FeatureIndex];
                double predictedLinear = (NetSlope * match.Net2) + NetIntercept;
                double ppmMassError = match.PpmMassError;
                double scanNumber = match.Net;

                // NET
                net.Add(scanNumber);
                linearNet.Add(feature.NETAligned - predictedLinear);
                customNet.Add(match.NetError);
                linearCustomNet.Add(feature.NETAligned - predictedLinear);

                double ppmShift = 0.0;
                if (m_useMass)
                {
                    ppmShift = GetPpmShift(feature.Mz, scanNumber);
                }
                mz.Add(feature.Mz);
                massError.Add(ppmMassError);

                massErrorCorrected.Add(ppmMassError - ppmShift);
            }
        }

        /// <summary>
        /// Updates the Alignee and Reference net lists with the data from the alignment
        /// function determined by the warper with the Net start data
        /// </summary>
        /// <param name="aligneeNet"></param>
        /// <param name="referenceNet"></param>
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

        /// <summary>
        /// Determines the transformed NETs for the LCMSWarp function
        /// </summary>
        public void GetTransformedNets()
        {
            int featureIndex;
            int numFeatures = m_features.Count;

            int alignmentFuncLength = m_alignmentFunc.Count;
            var dicSectionToIndex = new Dictionary<int, int>();

            for (int i = 0; i < m_alignmentFunc.Count; i++)
            {
                dicSectionToIndex.Add(m_alignmentFunc[i].SectionStart, i);
            }

            for (featureIndex = 0; featureIndex < numFeatures; featureIndex++)
            {
                UMCLight feature = m_features[featureIndex];
                double netStart;
                double netEndBaseline;
                double netEnd;
                double netStartBaseline;
                if (feature.NET < m_alignmentFunc[0].NetStart)
                {
                    netStart = m_alignmentFunc[0].NetStart;
                    netStartBaseline = m_alignmentFunc[0].NetStart2;
                    netEnd = m_alignmentFunc[0].NetEnd;
                    netEndBaseline = m_alignmentFunc[0].NetEnd2;

                    double msNetTransformed = ((feature.NET - netStart) * (netEndBaseline - netStartBaseline)) / (netEnd - netStart) + netStartBaseline;
                    m_features[featureIndex].NETAligned = msNetTransformed;
                    continue;
                }
                double netTransformed;
                if (feature.NET > m_alignmentFunc[alignmentFuncLength - 1].NetEnd)
                {
                    netStart = m_alignmentFunc[alignmentFuncLength - 1].NetStart;
                    netStartBaseline = m_alignmentFunc[alignmentFuncLength - 1].NetStart2;
                    netEnd = m_alignmentFunc[alignmentFuncLength - 1].NetEnd;
                    netEndBaseline = m_alignmentFunc[alignmentFuncLength - 1].NetEnd2;

                    netTransformed = ((feature.NET - netStart) * (netEndBaseline - netStartBaseline)) / (netEnd - netStart) + netStartBaseline;
                    m_features[featureIndex].NETAligned = netTransformed;
                    continue;
                }

                int msSection1 = Convert.ToInt32(((feature.NET - MinNet) * NumSections) / (MaxNet - MinNet));
                if (msSection1 >= NumSections)
                {
                    msSection1 = NumSections - 1;
                }

                int msSectionIndex = dicSectionToIndex[msSection1];

                netStart = m_alignmentFunc[msSectionIndex].NetStart;
                netEnd = m_alignmentFunc[msSectionIndex].NetEnd;

                netStartBaseline = m_alignmentFunc[msSectionIndex].NetStart2;
                netEndBaseline = m_alignmentFunc[msSectionIndex].NetEnd2;

                netTransformed = ((feature.NET - netStart) * (netEndBaseline - netStartBaseline)) / (netEnd - netStart) + netStartBaseline;
                m_features[featureIndex].NETAligned = netTransformed;
            }
        }

        /// <summary>
        /// Initially, clears any residual feature matches to ensure no carryover of prior runs,
        /// then goes through and calculates the match score for each feature with relation to
        /// the baselines, holding onto the "best match" for each one
        /// </summary>
        public void CalculateAlignmentMatches()
        {
            m_features.Sort();
            m_baselineFeatures.Sort();

            int featureIndex = 0;
            int baselineFeatureIndex = 0;
            int numFeatures = m_features.Count;
            int numBaselineFeatures = m_baselineFeatures.Count;

            var match = new LcmsFeatureMatch();

            m_featureMatches.Clear();

            double minMatchScore = -0.5 * (MassTolerance * MassTolerance) / (m_massStd * m_massStd);
            minMatchScore = minMatchScore - 0.5 * (MassTolerance * MassTolerance) / (m_massStd * m_massStd);

            while (featureIndex < numFeatures)
            {
                UMCLight feature = m_features[featureIndex];

                double massTolerance = feature.MassMonoisotopic * MassTolerance / 1000000;

                if (baselineFeatureIndex == numBaselineFeatures)
                {
                    baselineFeatureIndex = numBaselineFeatures - 1;
                }

                while (baselineFeatureIndex >= 0 && m_baselineFeatures[baselineFeatureIndex].MassMonoisotopic > feature.MassMonoisotopic - massTolerance)
                {
                    baselineFeatureIndex--;
                }
                baselineFeatureIndex++;

                int bestMatch = int.MaxValue;
                double bestMatchScore = minMatchScore;
                while (baselineFeatureIndex < numBaselineFeatures && m_baselineFeatures[baselineFeatureIndex].MassMonoisotopic < feature.MassMonoisotopic + massTolerance)
                {
                    if (m_baselineFeatures[baselineFeatureIndex].MassMonoisotopic > feature.MassMonoisotopic - massTolerance)
                    {
                        //Calculate the mass and net errors
                        double netDiff = m_baselineFeatures[baselineFeatureIndex].NET - feature.NETAligned;
                        double baselineDrift = m_baselineFeatures[baselineFeatureIndex].DriftTime;
                        double driftDiff = baselineDrift - feature.DriftTime;
                        double massDiff = (m_baselineFeatures[baselineFeatureIndex].MassMonoisotopic - feature.MassMonoisotopic) * 1000000.0 / feature.MassMonoisotopic;

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
                            match.NetError = netDiff;
                            match.PpmMassError = massDiff;
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
                    match = new LcmsFeatureMatch();
                }
                featureIndex++;
            }
            CalculateNetSlopeAndIntercept();
        }

        /// <summary>
        /// Initializes the features in the LCMSWarp object to the list of UMCLights
        /// </summary>
        /// <param name="features"></param>
        public void SetFeatures(ref List<UMCLight> features)
        {
            m_features.Clear();
            foreach (var feature in features)
            {
                m_features.Add(feature);
            }
        }

        /// <summary>
        /// Updates the data passed in with the appropriate calibrated mass, aligned NET and drift times
        /// </summary>
        /// <param name="umcIndices"></param>
        /// <param name="umcCalibratedMass"></param>
        /// <param name="umcAlignedNets"></param>
        /// <param name="alignedDriftTimes"></param>
        public void GetFeatureCalibratedMassesAndAlignedNets(ref List<int> umcIndices, ref List<double> umcCalibratedMass,
                                                             ref List<double> umcAlignedNets, ref List<double> alignedDriftTimes)
        {
            foreach (var feature in m_features)
            {
                umcIndices.Add(feature.ID);
                umcCalibratedMass.Add(feature.MassMonoisotopicAligned);
                umcAlignedNets.Add(feature.NETAligned);
                alignedDriftTimes.Add(feature.DriftTime);
            }
        }

        /// <summary>
        /// Updates the data passed in with the appropriate calibrated mass, aligned NET, drift times,
        /// aligned scans based on the min and max scan numbers
        /// </summary>
        /// <param name="umcIndices"></param>
        /// <param name="umcCalibratedMasses"></param>
        /// <param name="umcAlignedNets"></param>
        /// <param name="umcAlignedScans"></param>
        /// <param name="umcDriftTimes"></param>
        /// <param name="minScan"></param>
        /// <param name="maxScan"></param>
        public void GetFeatureCalibratedMassesAndAlignedNets(ref List<int> umcIndices, ref List<double> umcCalibratedMasses,
                                                             ref List<double> umcAlignedNets, ref List<int> umcAlignedScans,
                                                             ref List<double> umcDriftTimes, int minScan, int maxScan)
        {
            var numFeatures = m_features.Count;
            for (var featureNum = 0; featureNum < numFeatures; featureNum++)
            {
                var feature = m_features[featureNum];
                umcIndices.Add(feature.ID);
                umcCalibratedMasses.Add(feature.MassMonoisotopicAligned);
                umcAlignedNets.Add(feature.NETAligned);
                umcAlignedScans.Add(minScan + (int)(feature.NETAligned * (maxScan - minScan)));
                umcDriftTimes.Add(feature.DriftTime);
            }
        }

        /// <summary>
        /// Initializes the reference features to the features entered in
        /// </summary>
        /// <param name="features"></param>
        public void SetReferenceFeatures(ref List<UMCLight> features)
        {
            m_baselineFeatures.Clear();
            foreach (var feature in features)
            {
                m_baselineFeatures.Add(feature);
            }
        }

        /// <summary>
        /// Adds a feature to the list of alignee features
        /// </summary>
        /// <param name="feature"></param>
        public void AddFeature(ref UMCLight feature)
        {
            m_features.Add(feature);
        }

        /// <summary>
        /// Adds a feature to the list of reference features
        /// </summary>
        /// <param name="feature"></param>
        public void AddReferenceFeature(ref UMCLight feature)
        {
            m_baselineFeatures.Add(feature);
        }

        /// <summary>
        /// Function generates candidate matches between the MS and MSMS data loaded into
        /// features and baseline features respectively.
        /// It does so by finding all pairs of MassTimeFeature that match within a provided
        /// mass tolerance window
        /// </summary>
        public void GenerateCandidateMatches()
        {
            if (m_features.Count == 0)
            {
                return;
            }

            m_features.Sort();
            m_baselineFeatures.Sort();

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

            PercentComplete = 0;

            while (featureIndex < numFeatures)
            {
                PercentComplete = (100*featureIndex)/numFeatures;
                var feature = m_features[featureIndex];
                var massTolerance = feature.MassMonoisotopic*(MassTolerance/1000000);

                if (baselineFeatureIndex == numBaselineFeatures)
                {
                    baselineFeatureIndex = numBaselineFeatures - 1;
                }
                var baselineFeature = m_baselineFeatures[baselineFeatureIndex];
                while (baselineFeatureIndex >= 0 && (baselineFeature.MassMonoisotopic > feature.MassMonoisotopic - massTolerance))
                {
                    baselineFeatureIndex --;
                    if (baselineFeatureIndex >= 0)
                    {
                        baselineFeature = m_baselineFeatures[baselineFeatureIndex];
                    }
                }
                baselineFeatureIndex++;

                while (baselineFeatureIndex < numBaselineFeatures &&
                       (m_baselineFeatures[baselineFeatureIndex].MassMonoisotopic < (feature.MassMonoisotopic + massTolerance)))
                {
                    if (m_baselineFeatures[baselineFeatureIndex].MassMonoisotopic > (feature.MassMonoisotopic - massTolerance))
                    {
                        var matchToAdd = new LcmsFeatureMatch
                        {
                            FeatureIndex = featureIndex,
                            FeatureIndex2 = baselineFeatureIndex,
                            Net = feature.NET,
                            Net2 = m_baselineFeatures[baselineFeatureIndex].NET
                        };

                        m_featureMatches.Add(matchToAdd);
                    }
                    baselineFeatureIndex++;
                }
                featureIndex++;
            }

            // Now that matches have been created, go through all the matches and find a mapping
            // of how many times a basline feature is matched to. Puts the matches into a map from a
            // mass tag id to a list of indexes of feature matches

            var massTagToMatches = new Dictionary<int, List<int>>();
            int numMatches = m_featureMatches.Count();
            for (int matchIndex = 0; matchIndex < numMatches; matchIndex++)
            {
                LcmsFeatureMatch featureMatch = m_featureMatches[matchIndex];
                int baselineIndex = featureMatch.FeatureIndex2;
                if (!massTagToMatches.ContainsKey(baselineIndex))
                {
                    var matchList = new List<int>();
                    massTagToMatches.Add(baselineIndex, matchList);
                }
                massTagToMatches[baselineIndex].Add(matchIndex);
            }

            // Now go through each of the baseline features matched and for each one keep at
            // most m_maxPromiscuousUMCMatches (or non if m_keepPromiscuousMatches is false)
            // keeping only the first m_maxPromisuousUMCMatches by scan

            var tempMatches = new List<LcmsFeatureMatch> {Capacity = m_featureMatches.Count};
            var netMatchesToIndex = new Dictionary<double, List<int>>();

            foreach (var matchIterator in massTagToMatches)
            {
                int baselineIndex = matchIterator.Key;
                int numHits = massTagToMatches[baselineIndex].Count;
                if (numHits <= MaxPromiscuousUmcMatches)
                {
                    // add all of these ot the temp matches
                    for (int i = 0; i < numHits; i++)
                    {
                        int matchIndex = matchIterator.Value[i];
                        LcmsFeatureMatch match = m_featureMatches[matchIndex];
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
                    
                    for (int index = 0; index < MaxPromiscuousUmcMatches; index++)
                    {
                        int matchIndex = netMatchesToIndex.ElementAt(index).Value[0];
                        LcmsFeatureMatch match = m_featureMatches[matchIndex];
                        tempMatches.Add(match);
                    }
                }
            }
            m_featureMatches = tempMatches;
        }

        /// <summary>
        /// Performs Mass calibrtion, depending on calibration type, utilizing MZ
        /// regression, scan regression, or both (with the MZ regression preceeding
        /// the scan regression
        /// </summary>
        public void PerformMassCalibration()
        {
            switch (CalibrationType)
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
        
        /// <summary>
        /// Calculates the Standard deviations of the matches.
        /// Note: method requires more than 6 matches to produce meaningful
        /// results.
        /// </summary>
        public void CalculateStandardDeviations()
        {
            int numMatches = m_featureMatches.Count;
            if (numMatches > 6)
            {
                var massDeltas = new List<double>();
                var netDeltas = new List<double>();
                massDeltas.Capacity = numMatches;
                netDeltas.Capacity = numMatches;
                for (int matchNum = 0; matchNum < numMatches; matchNum++)
                {
                    LcmsFeatureMatch match = m_featureMatches[matchNum];
                    UMCLight feature = m_features[match.FeatureIndex];
                    UMCLight baselineFeature = m_baselineFeatures[match.FeatureIndex2];
                    double currentMassDelta = ((baselineFeature.MassMonoisotopic - feature.MassMonoisotopic) * 1000000) / feature.MassMonoisotopic;
                    double currentNetDelta = baselineFeature.NET - feature.NET;

                    massDeltas.Add(currentMassDelta);
                    netDeltas.Add(currentNetDelta);
                }
                m_normalProb = 0;
                m_u = 0;
                m_muMass = 0;
                m_muNet = 0;
                MathUtils.TwoDem(massDeltas, netDeltas, out m_normalProb, out m_u,
                                 out m_muMass, out m_muNet, out m_massStd, out m_netStd);
            }
        }
        
        /// <summary>
        /// Method to set the flag for whether to use the Mass and the Net
        /// match scores with the LCMSWarper
        /// </summary>
        /// <param name="use"></param>
        public void UseMassAndNetScore(bool use)
        {
            m_useMass = use;
        }

        /// <summary>
        /// Performs Mass error regression based on MZ of the match
        /// </summary>
        public void PerformMzMassErrorRegression()
        {
            //Copy all MZs and mass errors into a list of regression points
            var calibrations = new List<LcmsRegressionPts>();
            
            int numMatches = m_featureMatches.Count;

            for (int matchNum = 0; matchNum < numMatches; matchNum++)
            {
                var calibrationMatch = new LcmsRegressionPts();
                LcmsFeatureMatch match = m_featureMatches[matchNum];
                var feature = m_features[match.FeatureIndex];
                var baselineFeature = m_baselineFeatures[match.FeatureIndex2];
                double ppm = (feature.MassMonoisotopic - baselineFeature.MassMonoisotopic);
                double mz = feature.Mz;
                double netDiff = baselineFeature.NET - feature.NETAligned;
                calibrationMatch.X = mz;
                calibrationMatch.NetError = netDiff;
                calibrationMatch.MassError = ppm;

                calibrations.Add(calibrationMatch);
            }
            MzRecalibration.CalculateRegressionFunction(ref calibrations);

            int numFeatures = m_features.Count;
            for (int featureNum = 0; featureNum < numFeatures; featureNum++)
            {
                var feature = m_features[featureNum];
                double mz = feature.Mz;
                double mass = feature.MassMonoisotopic;
                double ppmShift = MzRecalibration.GetPredictedValue(mz);
                double newMass = mass - (mass * ppmShift) / 1000000;
                feature.MassMonoisotopicAligned = newMass;
                feature.MassMonoisotopic = newMass;
            }
        }

        /// <summary>
        /// Performs Mass error regression based on Scan of the match
        /// </summary>
        public void PerformScanMassErrorRegression()
        {
            var calibrations = new List<LcmsRegressionPts>();
            
            int numMatches = m_featureMatches.Count;

            for (int matchNum = 0; matchNum < numMatches; matchNum++)
            {
                var calibrationMatch = new LcmsRegressionPts();
                LcmsFeatureMatch match = m_featureMatches[matchNum];
                UMCLight feature = m_features[match.FeatureIndex];
                UMCLight baselineFeature = m_baselineFeatures[match.FeatureIndex2];
                double ppm = (feature.MassMonoisotopic - baselineFeature.MassMonoisotopic);
                double net = feature.NET;
                double netDiff = baselineFeature.NET - feature.NETAligned;
                calibrationMatch.X = net;
                calibrationMatch.NetError = netDiff;
                calibrationMatch.MassError = ppm;

                calibrations.Add(calibrationMatch);
            }

            NetRecalibration.CalculateRegressionFunction(ref calibrations);

            int numFeatures = m_features.Count;
            for (int featureNum = 0; featureNum < numFeatures; featureNum++)
            {
                var feature = m_features[featureNum];
                double net = feature.NET;
                double mass = feature.MassMonoisotopic;
                double ppmShift = MzRecalibration.GetPredictedValue(net);
                double newMass = mass - (mass * ppmShift) / 1000000;
                feature.MassMonoisotopicAligned = newMass;
                feature.MassMonoisotopic = newMass;
            }
        }

        /// <summary>
        /// Goes through the matched features and determines the probability
        /// of each that the match is correct
        /// </summary>
        public void GetMatchProbabilities()
        {
            PercentComplete = 0;
            int numFeatures = m_features.Count;

            m_tempFeatureBestDelta.Clear();
            m_tempFeatureBestDelta.Capacity = numFeatures;

            m_tempFeatureBestIndex.Clear();
            m_tempFeatureBestIndex.Capacity = numFeatures;

            //Clear the old features and initialize each section to 0
            m_numFeaturesInSections.Clear();
            m_numFeaturesInSections.Capacity = NumSections;
            for (int i = 0; i < NumSections; i++)
            {
                m_numFeaturesInSections.Add(0);
            }

            // Do the same for baseline sections
            m_numFeaturesInBaselineSections.Clear();
            m_numFeaturesInBaselineSections.Capacity = numFeatures;
            for (int i = 0; i < NumBaselineSections; i++)
            {
                m_numFeaturesInBaselineSections.Add(0);
            }

            MaxNet = double.MinValue;
            MinNet = double.MaxValue;
            for (int i = 0; i < numFeatures; i++)
            {
                if (m_featureMatches[i].Net > MaxNet)
                {
                    MaxNet = m_featureMatches[i].Net;
                }
                if (m_featureMatches[i].Net < MinNet)
                {
                    MinNet = m_featureMatches[i].Net;
                }
                m_tempFeatureBestDelta.Add(double.MaxValue);
                m_tempFeatureBestIndex.Add(-1);
            }

            for (int i = 0; i < numFeatures; i++)
            {
                double net = m_featureMatches[i].Net;
                int sectionNum = Convert.ToInt32(((net - MinNet) * NumSections) / (MaxNet - MinNet));
                if (sectionNum >= NumSections)
                {
                    sectionNum--;
                }
                m_numFeaturesInSections[sectionNum]++;
            }

            MinBaselineNet = double.MaxValue;
            MaxBaselineNet = double.MinValue;

            int numBaselineFeatures = m_baselineFeatures.Count;
            for (int i = 0; i < numBaselineFeatures; i++)
            {
                if (m_baselineFeatures[i].NET < MinBaselineNet)
                {
                    MinBaselineNet = m_baselineFeatures[i].NET;
                }
                if (m_baselineFeatures[i].NET > MaxBaselineNet)
                {
                    MaxBaselineNet = m_baselineFeatures[i].NET;
                }
            }

            for (int i = 0; i < numBaselineFeatures; i++)
            {
                double net = m_baselineFeatures[i].NET;
                var msmsSectionNum = (int)(((net - MinBaselineNet) * NumBaselineSections) / (MaxBaselineNet - MinBaselineNet));
                if (msmsSectionNum == NumBaselineSections)
                {
                    msmsSectionNum--;
                }
                m_numFeaturesInBaselineSections[msmsSectionNum]++;
            }

            m_featureMatches.Sort();

            int numMatches = m_featureMatches.Count;

            var sectionFeatures = new List<LcmsFeatureMatch>();

            int numSectionMatches = NumSections * NumMatchesPerSection;
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

            for (int section = 0; section < NumSections; section++)
            {
                PercentComplete = (section * 100) / (NumSections + 1);
                int startMatchIndex = 0;
                sectionFeatures.Clear();
                double sectionStartNet = MinNet + (section * (MaxNet - MinNet)) / NumSections;
                double sectionEndNet = MinNet + ((section + 1) * (MaxNet - MinNet)) / NumSections;

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

        /// <summary>
        /// Calculates the alignment function for each of the sections, based
        /// on the match scores for every feature in the subsection
        /// </summary>
        public void CalculateAlignmentFunction()
        {
            PercentComplete = 0;
            int section = NumSections - 1;

            int bestPreviousAlignmentIndex = -1;
            double bestScore = double.MinValue;
            int bestAlignedBaselineSection = -1;
            int numBaselineFeatures = m_baselineFeatures.Count;
            int numUnmatchedBaselineFeaturesSectionStart = numBaselineFeatures;
            int bestAlignmentIndex = -1;

            for (int baselineSection = 0; baselineSection < NumBaselineSections; baselineSection++)
            {
                // Everything past this section would have remained unmatched.
                for (int sectionWidth = 0; sectionWidth < NumMatchesPerBaseline; sectionWidth++)
                {
                    int numUnmatchedBaselineFeaturesSectionEnd;
                    if (baselineSection + sectionWidth >= NumBaselineSections)
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
                    int alignmentIndex = (section * NumMatchesPerSection) + (baselineSection * NumMatchesPerBaseline) + sectionWidth;

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

            double msmsSectionWidth = (MaxBaselineNet - MinBaselineNet) / NumBaselineSections;

            double netStart = MinNet + (section * (MaxNet - MinNet)) / NumSections;
            double netEnd = MinNet + ((section + 1) * (MaxNet - MinNet)) / NumSections;
            int baselineSectionStart = bestAlignedBaselineSection;
            int baselineSectionEnd = baselineSectionStart + bestAlignmentIndex % NumMatchesPerBaseline + 1;
            double baselineStartNet = baselineSectionStart * msmsSectionWidth + MinBaselineNet;
            double baselineEndNet = baselineSectionEnd * msmsSectionWidth + MinBaselineNet;

            var match = new LcmsAlignmentMatch();
            match.Set(netStart, netEnd, section, NumSections, baselineStartNet, baselineEndNet, baselineSectionStart,
                      baselineSectionEnd, bestScore, m_subsectionMatchScores[bestAlignmentIndex]);
            m_alignmentFunc.Clear();
            m_alignmentFunc.Add(match);

            while (bestPreviousAlignmentIndex >= 0)
            {
                int sectionStart = (bestPreviousAlignmentIndex / NumMatchesPerSection); // should be current - 1
                int sectionEnd = sectionStart + 1;
                PercentComplete = 100 - (100 * sectionStart) / NumSections;

                double nextNetStart = MinNet + (sectionStart * (MaxNet - MinNet)) / NumSections;
                double nextNetEnd = MinNet + (sectionEnd * (MaxNet - MinNet)) / NumSections;

                int nextBaselineSectionStart = (bestPreviousAlignmentIndex - (sectionStart * NumMatchesPerSection)) / NumMatchesPerBaseline;
                int nextBaselineSectionEnd = nextBaselineSectionStart + (bestPreviousAlignmentIndex % NumMatchesPerBaseline) + 1;
                double nextBaselineStartNet = (nextBaselineSectionStart * msmsSectionWidth) + MinBaselineNet;
                double nextBaselineEndNet = nextBaselineSectionEnd * msmsSectionWidth + MinBaselineNet;
                match = new LcmsAlignmentMatch();
                match.Set(nextNetStart, nextNetEnd, sectionStart, sectionEnd, nextBaselineStartNet, nextBaselineEndNet,
                          nextBaselineSectionStart, nextBaselineSectionEnd, m_alignmentScore[bestPreviousAlignmentIndex],
                          m_subsectionMatchScores[bestPreviousAlignmentIndex]);

                bestPreviousAlignmentIndex = m_bestPreviousIndex[bestPreviousAlignmentIndex];
                m_alignmentFunc.Add(match);
            }
            m_alignmentFunc.Sort();
        }

        /// <summary>
        /// Computes the alignment matrix for all possible alignments, holding
        /// onto each and every possible alignment score and then linking the feature
        /// to the best previous index of possible alignments
        /// </summary>
        public void CalculateAlignmentMatrix()
        {
            PercentComplete = 0;
            int numPossibleAlignments = NumSections * NumBaselineSections * NumMatchesPerBaseline;

            m_alignmentScore = new double[numPossibleAlignments];
            m_bestPreviousIndex = new int[numPossibleAlignments];

            // Initialize scores to - inf, best previous index to -1
            for (int i = 0; i < numPossibleAlignments; i++)
            {
                m_alignmentScore[i] = double.MinValue;
                m_bestPreviousIndex[i] = -1;
            }
            double log2PiStdNetStdNet = Math.Log(2 * Math.PI * m_netStd * m_netStd);

            double unmatchedScore = -0.5 * log2PiStdNetStdNet;
            if (NetTolerance < 3 * m_netStd)
            {
                unmatchedScore = unmatchedScore - (0.5 * 9.0);
            }
            else
            {
                unmatchedScore = unmatchedScore - (0.5 * (NetTolerance * NetTolerance) / (m_netStd * m_netStd));
            }
            if (m_useMass)
            {
                //Assumes that for the unmatched, the masses were also off at mass tolerance, so use the same threshold from NET
                unmatchedScore = 2 * unmatchedScore;
            }

            int numUnmatchedMsMsFeatures = 0;
            for (int baselineSection = 0; baselineSection < NumBaselineSections; baselineSection++)
            {
                //Assume everything that was matched was past 3 standard devs in net.
                for (int sectionWidth = 0; sectionWidth < NumMatchesPerBaseline; sectionWidth++)
                {
                    //no need to mulitply with msSection because its 0
                    int alignmentIndex = baselineSection * NumMatchesPerBaseline + sectionWidth;
                    m_alignmentScore[alignmentIndex] = 0;
                }
                numUnmatchedMsMsFeatures = numUnmatchedMsMsFeatures + m_numFeaturesInBaselineSections[baselineSection];
            }

            int numUnmatchedMsFeatures = 0;
            for (int section = 0; section < NumSections; section++)
            {
                for (int sectionWidth = 0; sectionWidth < NumMatchesPerBaseline; sectionWidth++)
                {
                    int alignmentIndex = section * NumMatchesPerSection + sectionWidth;
                    m_alignmentScore[alignmentIndex] = m_subsectionMatchScores[alignmentIndex] + unmatchedScore * numUnmatchedMsFeatures;
                }
                numUnmatchedMsFeatures = numUnmatchedMsFeatures + m_numFeaturesInSections[section];
            }

            for (int section = 1; section < NumSections; section++)
            {
                PercentComplete = (100 * section) / NumSections;
                for (int baselineSection = 1; baselineSection < NumBaselineSections; baselineSection++)
                {
                    for (int sectionWidth = 0; sectionWidth < NumMatchesPerBaseline; sectionWidth++)
                    {
                        int alignmentIndex = section * NumMatchesPerSection + baselineSection * NumMatchesPerBaseline + sectionWidth;

                        double currentBestScore = double.MinValue;
                        int bestPreviousAlignmentIndex = 0;

                        for (int previousBaselineSection = baselineSection - 1; previousBaselineSection >= baselineSection - NumMatchesPerBaseline - MaxJump; previousBaselineSection--)
                        {
                            if (previousBaselineSection < 0)
                            {
                                break;
                            }
                            int maxWidth = baselineSection - previousBaselineSection;
                            if (maxWidth > NumMatchesPerBaseline)
                            {
                                maxWidth = NumMatchesPerBaseline;
                            }
                            int previousBaselineSectionWidth = maxWidth;
                            int previousAlignmentIndex = (section - 1) * NumMatchesPerSection + previousBaselineSection * NumMatchesPerBaseline + previousBaselineSectionWidth - 1;
                            if (m_alignmentScore[previousAlignmentIndex] > currentBestScore)
                            {
                                 currentBestScore = m_alignmentScore[previousAlignmentIndex];
                                bestPreviousAlignmentIndex = previousAlignmentIndex;
                            }
                        }
                        if (Math.Abs(currentBestScore - double.MinValue) > double.Epsilon)
                        {
                            m_alignmentScore[alignmentIndex] = currentBestScore + m_subsectionMatchScores[alignmentIndex];
                            m_bestPreviousIndex[alignmentIndex] = bestPreviousAlignmentIndex;
                        }
                        else
                        {
                            m_alignmentScore[alignmentIndex] = double.MinValue;
                        }
                    }
                }
            }
        }

        private void ComputeSectionMatch(int msSection, ref List<LcmsFeatureMatch> sectionMatchingFeatures, double minNet, double maxNet)
        {
            int numMatchingFeatures = sectionMatchingFeatures.Count;
            double baselineSectionWidth = (MaxBaselineNet - MinBaselineNet) / NumBaselineSections;

            // keep track of only the unique indices of ms features because we only want the best matches for each
            m_sectionUniqueFeatureIndices.Clear();
            m_sectionUniqueFeatureIndices.Capacity = numMatchingFeatures;
            for (int i = 0; i < numMatchingFeatures; i++)
            {
                bool found = false;
                LcmsFeatureMatch match = sectionMatchingFeatures[i];
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

            for (int baselineSectionStart = 0; baselineSectionStart < NumBaselineSections; baselineSectionStart++)
            {
                double baselineStartNet = MinBaselineNet + baselineSectionStart * baselineSectionWidth;
                int endSection = baselineSectionStart + NumMatchesPerBaseline;
                if (endSection >= NumBaselineSections)
                {
                    endSection = NumBaselineSections;
                }
                int numBaselineFeaturesInSection = 0;
                for (int baselineSectionEnd = baselineSectionStart; baselineSectionEnd < endSection; baselineSectionEnd++)
                {
                    int numUniqueFeatures = numUniqueFeaturesStart;
                    int numFeaturesInSection = numFeaturesInSectionStart;
                    int numUnmatchedFeaturesInSection = numFeaturesInSection - numUniqueFeatures;

                    numBaselineFeaturesInSection = numBaselineFeaturesInSection + m_numFeaturesInBaselineSections[baselineSectionEnd];

                    int sectionIndex = msSection * NumMatchesPerSection + baselineSectionStart * NumMatchesPerBaseline
                                     + baselineSectionEnd - baselineSectionStart;
                    double baselineEndNet = MinBaselineNet + (baselineSectionEnd + 1) * baselineSectionWidth;

                    for (int i = 0; i < numUniqueFeatures; i++)
                    {
                        int msFeatureIndex = m_sectionUniqueFeatureIndices[i];
                        m_tempFeatureBestDelta[msFeatureIndex] = double.MaxValue;
                        m_tempFeatureBestIndex[msFeatureIndex] = -1;
                    }

                    // Now that we have msmsSection and matching msSection, transform the scan numbers to nets using a
                    // transformation of the two sections, and use a temporary list to keep only the best match
                    for (int i = 0; i < numMatchingFeatures; i++)
                    {
                        LcmsFeatureMatch match = sectionMatchingFeatures[i];
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

                    m_subsectionMatchScores[sectionIndex] = CurrentlyStoredSectionMatchScore(numUniqueFeatures,numUnmatchedFeaturesInSection);
                }
            }
        }

        /// <summary>
        /// Given an MZ value, return the appropriate ppm shift
        /// Note: Requires LCMSWarp to be calibrating based on MZ Regression or
        /// Hybrid regression to process.
        /// </summary>
        /// <param name="mz"></param>
        /// <returns></returns>
        public double GetPpmShiftFromMz(double mz)
        {
            if (CalibrationType == LcmsWarpCalibrationType.MZ_REGRESSION || CalibrationType == LcmsWarpCalibrationType.BOTH)
            {
                return MzRecalibration.GetPredictedValue(mz);
            }
            return 0;
        }

        /// <summary>
        /// Given an NET value, return the appropriate ppm shift
        /// Note: Requires LCMSWarp to be calibrating based on Scan Regression or
        /// Hybrid regression to process.
        /// </summary>
        /// <param name="net"></param>
        /// <returns></returns>
        public double GetPpmShiftFromNet(double net)
        {
            if (CalibrationType == LcmsWarpCalibrationType.SCAN_REGRESSION || CalibrationType == LcmsWarpCalibrationType.BOTH)
            {
                return NetRecalibration.GetPredictedValue(net);
            }
            return 0;
        }

        /// <summary>
        /// Calculates the match score of the subsection, saving
        /// the match scores to the first parameter and the alignee and
        /// reference values to the second and third parameters.
        /// </summary>
        /// <param name="subsectionMatchScores"></param>
        /// <param name="aligneeVals"></param>
        /// <param name="refVals"></param>
        /// <param name="standardize"></param>
        public void GetSubsectionMatchScore(ref List<double> subsectionMatchScores, ref List<double> aligneeVals,
                                            ref List<double> refVals, bool standardize)
        {
            subsectionMatchScores.Clear();
            for (var msSection = 0; msSection < NumSections; msSection++)
            {
                aligneeVals.Add(MinNet + (msSection * (MaxNet - MinNet)) / NumSections);
            }
            for (var msmsSection = 0; msmsSection < NumBaselineSections; msmsSection++)
            {
                refVals.Add(MinBaselineNet + (msmsSection * (MaxBaselineNet - MinBaselineNet)) / NumBaselineSections);
            }

            for (var msSection = 0; msSection < NumSections; msSection++)
            {
                for (var msmsSection = 0; msmsSection < NumBaselineSections; msmsSection++)
                {
                    var maxScore = double.MinValue;
                    for (var msmsSectionWidth = 0; msmsSectionWidth < NumMatchesPerBaseline; msmsSectionWidth++)
                    {
                        if (msmsSection + msmsSectionWidth >= NumBaselineSections)
                        {
                            continue;
                        }
                        var index = (msSection * NumMatchesPerSection) + (msmsSection * NumMatchesPerBaseline) + msmsSectionWidth;
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
                for (var msSection = 0; msSection < NumSections; msSection++)
                {
                    double sumX = 0, sumXx = 0;
                    var startIndex = index;
                    var realMinScore = double.MaxValue;
                    var numPoints = 0;
                    for (var msmsSection = 0; msmsSection < NumBaselineSections; msmsSection++)
                    {
                        var score = subsectionMatchScores[index];
                        if (Math.Abs(score - m_minScore) > double.Epsilon)
                        {
                            if (score < realMinScore)
                            {
                                realMinScore = score;
                            }
                            sumX = sumX + score;
                            sumXx = sumXx + (score * score);
                            numPoints++;
                        }
                        index++;
                    }
                    double var = 0;
                    if (numPoints > 1)
                    {
                        var = (sumXx - ((sumX * sumX) / numPoints)) / (numPoints - 1);
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
                    for (int msmsSection = 0; msmsSection < NumBaselineSections; msmsSection++)
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
    }

    /// <summary>
    /// Enumeration of calibration types for LCMSWarping
    /// </summary>
    public enum LcmsWarpCalibrationType
    {
        /// <summary>
        /// Performs Regression based on MZ of the features
        /// </summary>
        MZ_REGRESSION = 0,
        /// <summary>
        /// Performs Regression based on the Scan of the features
        /// </summary>
        SCAN_REGRESSION,
        /// <summary>
        /// Performs a hybrid regression, performing MZ regression and then
        /// subsequently performing Scan regression
        /// </summary>
        BOTH
    };
}
