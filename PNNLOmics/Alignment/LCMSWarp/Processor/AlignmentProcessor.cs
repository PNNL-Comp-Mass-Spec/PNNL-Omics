using System;
using System.Collections.Generic;
using LCMS.Alignment;
using LCMS.Regression;
using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;

namespace Processor
{
    /// <summary>
    /// Class to hold the residual data from the alignment process
    /// </summary>
    public class ResidualData
    {
        #region Private values
        private double[] m_scans;
        private double[] m_mz;
        private double[] m_linearNet;
        private double[] m_customNet;
        private double[] m_linearCustomNet;
        private double[] m_massError;
        private double[] m_massErrorCorrected;
        private double[] m_mzMassError;
        private double[] m_mzMassErrorCorrected;
        #endregion

        #region Public Properties
        public double[] Scan
        {
            get { return m_scans; }
            set { m_scans = value; }
        }

        public double[] Mz
        {
            get { return m_mz; }
            set { m_mz = value; }
        }

        public double[] LinearNet
        {
            get { return m_linearNet; }
            set { m_linearNet = value; }
        }

        public double[] CustomNet
        {
            get { return m_customNet; }
            set { m_customNet = value; }
        }

        public double[] LinearCustomNet
        {
            get { return m_linearCustomNet; }
            set { m_linearCustomNet = value; }
        }

        public double[] MassError
        {
            get { return m_massError; }
            set { m_massError = value; }
        }

        public double[] MassErrorCorrected
        {
            get { return m_massErrorCorrected; }
            set { m_massErrorCorrected = value; }
        }

        public double[] MzMassError
        {
            get { return m_mzMassError; }
            set { m_mzMassError = value; }
        }

        public double[] MzMassErrorCorrected
        {
            get { return m_mzMassErrorCorrected; }
            set { m_mzMassErrorCorrected = value; }
        }
        #endregion
    }

    /// <summary>
    /// Class which will use LCMSWarp to process alignment
    /// </summary>
    public class AlignmentProcessor
    {
        bool m_aligningToMassTagDb;
        bool m_massTagDbLoaded;
        bool m_clusterAlignee;

        // In case alignment was to ms features, this will kepe track of the minimum scan in the
        // reference features. They are needed because LCMSWarp uses NET values for reference and
        // are scaled to between 0 and 1. These will scale it back to actual scan numbers
        int m_minReferenceDatasetScan;
        int m_maxReferenceDatasetScan;

        int m_minAligneeDatasetScan;
        int m_maxAligneeDatasetScan;
        double m_minAligneeDatasetMz;
        double m_maxAligneeDatasetMz;

        int m_percentDone;

        // LCMSWarp instance which will do the alignment when processing
        LcmsWarp m_lcmsWarp;
        AlignmentOptions m_options;

        #region Public properties

        public double NetIntercept
        {
            get { return m_lcmsWarp.NetIntercept; }
        }
        public double NetRsquared
        {
            get { return m_lcmsWarp.NetLinearRsq; }
        }
        public double NetSlope
        {
            get { return m_lcmsWarp.NetSlope; }
        }

        public AlignmentOptions Options
        {
            get { return m_options; }
            set { m_options = value; }
        }

        public bool AligningToMassTagDb
        {
            get { return m_aligningToMassTagDb; }
            set { m_aligningToMassTagDb = value; }
        }

        public bool MassTagDbLoaded
        {
            get { return m_massTagDbLoaded; }
            set { m_massTagDbLoaded = value; }
        }

        public bool ClusterAlignee
        {
            get { return m_clusterAlignee; }
            set { m_clusterAlignee = value; }
        }

        public int PercentDone
        {
            get { return m_percentDone; }
            set { m_percentDone = value; }
        }
        #endregion

        public AlignmentProcessor()
        {
            m_lcmsWarp = new LcmsWarp();
            m_options = new AlignmentOptions();

            ApplyAlignmentOptions();

            m_aligningToMassTagDb = false;
            m_massTagDbLoaded = false;
            m_clusterAlignee = false;
        }

        public void Dispose()
        {
            if (m_lcmsWarp != null)
            {
                m_lcmsWarp.Clear();
            }
        }

        // Applies the alignment options to the LCMSWarper, setting the Mass
        // and NET Tolerances, the options for NET Alignment options for 
        // Mass calibration, the Least Squares options and the calibration type
        private void ApplyAlignmentOptions()
        {
            // Applying the Mass and NET Tolerances
            m_lcmsWarp.MassTolerance = m_options.MassTolerance;
            m_lcmsWarp.NetTolerance = m_options.NETTolerance;

            // Applying options for NET Calibration
            m_lcmsWarp.NumSections = m_options.NumTimeSections;
            m_lcmsWarp.MaxSectionDistortion = m_options.ContractionFactor;
            m_lcmsWarp.MaxJump = m_options.MaxTimeDistortion;
            m_lcmsWarp.NumBaselineSections = m_options.NumTimeSections * m_options.ContractionFactor;
            m_lcmsWarp.NumMatchesPerBaselineStart = m_options.ContractionFactor * m_options.ContractionFactor;
            m_lcmsWarp.NumMatchesPerSection = m_lcmsWarp.NumBaselineSections * m_lcmsWarp.NumMatchesPerBaselineStart;
            m_lcmsWarp.MaxPromiscuousUmcMatches = m_options.MaxPromiscuity;

            // Applying options for Mass Calibration
            m_lcmsWarp.MassCalibrationWindow = m_options.MassCalibrationWindow;
            m_lcmsWarp.MassCalNumDeltaBins = m_options.MassCalibNumYSlices;
            m_lcmsWarp.MassCalNumSlices = m_options.MassCalibNumXSlices;
            m_lcmsWarp.MassCalNumJump = m_options.MassCalibMaxJump;

            CombinedRegression.RegressionType regType = CombinedRegression.RegressionType.Central;

            if (m_options.MassCalibUseLsq)
            {
                regType = CombinedRegression.RegressionType.Hybrid;
            }
            m_lcmsWarp.MzRecalibration.SetCentralRegressionOptions(m_lcmsWarp.MassCalNumSlices, m_lcmsWarp.MassCalNumDeltaBins,
                                                                   m_lcmsWarp.MassCalNumJump, m_options.MassCalibMaxZScore,
                                                                   regType);
            m_lcmsWarp.NetRecalibration.SetCentralRegressionOptions(m_lcmsWarp.MassCalNumSlices, m_lcmsWarp.MassCalNumDeltaBins,
                                                                    m_lcmsWarp.MassCalNumJump, m_options.MassCalibMaxZScore,
                                                                    regType);

            // Applying LSQ options
            m_lcmsWarp.MzRecalibration.SetLSQOptions(m_options.MassCalibLSQNumKnots, m_options.MassCalibLsqMaxZScore);
            m_lcmsWarp.NetRecalibration.SetLSQOptions(m_options.MassCalibLSQNumKnots, m_options.MassCalibLsqMaxZScore);

            // Setting the calibration type
            m_lcmsWarp.CalibrationType = (LcmsWarpCalibrationType)m_options.CalibType;
        }

        /// <summary>
        /// Takes a List of UMCLight data and applies the NET/Mass Function to the dataset and
        /// aligns it to the baseline. Updates the data in place for the calibrated Masses and
        /// Alignment. 
        /// </summary>
        /// <param name="data"></param>
        public void ApplyNetMassFunctionToAligneeDatasetFeatures(ref List<UMCLight> data)
        {
            if (m_lcmsWarp == null)
            {
                m_lcmsWarp = new LcmsWarp();
            }

            m_percentDone = 0;
            var umcIndices = new List<int>();
            var umcCalibratedMasses = new List<double>();
            var umcAlignedNets = new List<double>();
            var umcAlignedScans = new List<int>();
            var umcDriftTimes = new List<double>();

            if (m_aligningToMassTagDb)
            {
                m_lcmsWarp.GetFeatureCalibratedMassesAndAlignedNets(ref umcIndices, ref umcCalibratedMasses,
                                                                    ref umcAlignedNets, ref umcDriftTimes);

                for (int i = 0; i < umcIndices.Count; i++)
                {
                    var point = new UMCLight();
                    point.ID = umcIndices[i];
                    point.MassMonoisotopicAligned = umcCalibratedMasses[i];
                    point.NETAligned = umcAlignedNets[i];
                    point.DriftTime = umcDriftTimes[i];

                    if (i < data.Count)
                    {
                        data[i] = point;
                    }
                    else
                    {
                        data.Add(point);
                    }
                }

            }
            else
            {
                m_lcmsWarp.GetFeatureCalibratedMassesAndAlignedNets(ref umcIndices, ref umcCalibratedMasses,
                                                                    ref umcAlignedNets, ref umcAlignedScans,
                                                                    ref umcDriftTimes, m_minReferenceDatasetScan,
                                                                    m_maxReferenceDatasetScan);

                for (int i = 0; i < umcIndices.Count; i++)
                {
                    UMCLight point = new UMCLight();
                    point.ID = umcIndices[i];
                    point.MassMonoisotopicAligned = umcCalibratedMasses[i];
                    point.NETAligned = umcAlignedNets[i];
                    point.ScanAligned = umcAlignedScans[i];
                    point.DriftTime = umcDriftTimes[i];

                    if (i < data.Count)
                    {
                        data[i] = point;
                    }
                    else
                    {
                        data.Add(point);
                    }
                }
            }
        }

        /// <summary>
        /// Temporary function accepts an array of UMCLight Data and applies the NET/Mass Function
        /// to the dataset and aligns it to the baseline. Updates the data in place for the 
        /// calibrated Masses and Alignment. 
        /// 
        /// Might be able to kill this function. C++ processor had a method for UMCData pointer
        /// and another one for a list of UMC pointers. Change to UMCLight doesn't have a separate class
        /// for a list of UMCLights, so a method for this type of function is extraneous.
        /// </summary>
        /// <param name="dataList"></param>
        public void ApplyNetMassFunctionToAligneeDatasetFeatures(UMCLight[] dataList)
        {
            if (m_lcmsWarp == null)
            {
                m_lcmsWarp = new LcmsWarp();
            }

            m_percentDone = 0;
            var umcIndices = new List<int>();
            var umcCalibratedMasses = new List<double>();
            var umcAlignedNets = new List<double>();
            var umcAlignedScans = new List<int>();
            var umcDriftTimes = new List<double>();

            if (m_aligningToMassTagDb)
            {
                m_lcmsWarp.GetFeatureCalibratedMassesAndAlignedNets(ref umcIndices, ref umcCalibratedMasses,
                                                                    ref umcAlignedNets, ref umcDriftTimes);
                for (int i = 0; i < umcIndices.Count; i++)
                {
                    UMCLight point = new UMCLight();
                    point.ID = umcIndices[i];
                    point.MassMonoisotopicAligned = umcCalibratedMasses[i];
                    point.NETAligned = umcAlignedNets[i];
                    point.DriftTime = umcDriftTimes[i];

                    if (i < dataList.Length)
                    {
                        dataList[i] = point;
                    }
                    else
                    {
                        UMCLight[] newDataList = new UMCLight[dataList.Length + 1];
                        for (int j = 0; j < dataList.Length; j++)
                        {
                            newDataList[j] = dataList[j];
                        }
                        newDataList[dataList.Length] = point;
                        dataList = newDataList;
                    }
                }

            }
            else
            {
                m_lcmsWarp.GetFeatureCalibratedMassesAndAlignedNets(ref umcIndices, ref umcCalibratedMasses,
                                                                    ref umcAlignedNets, ref umcAlignedScans,
                                                                    ref umcDriftTimes, m_minReferenceDatasetScan,
                                                                    m_maxReferenceDatasetScan);

                for (int i = 0; i < umcIndices.Count; i++)
                {
                    UMCLight point = new UMCLight();
                    point.ID = umcIndices[i];
                    point.MassMonoisotopicAligned = umcCalibratedMasses[i];
                    point.NETAligned = umcAlignedNets[i];
                    point.ScanAligned = umcAlignedScans[i];
                    point.DriftTime = umcDriftTimes[i];

                    if (i < dataList.Length)
                    {
                        dataList[i] = point;
                    }
                    else
                    {
                        UMCLight[] newDataList = new UMCLight[dataList.Length + 1];
                        for (int j = 0; j < dataList.Length; j++)
                        {
                            newDataList[j] = dataList[j];
                        }
                        newDataList[dataList.Length] = point;
                        dataList = newDataList;
                    }
                }
            }

        }

        /// <summary>
        /// Temporary hack solution to align between certain MZ's. Test to see if the MZ is
        /// within a range between a lower bound and an upper bound.
        /// </summary>
        /// <param name="split"></param>
        /// <param name="mz"></param>
        /// <param name="boundary"></param>
        /// <returns> Boolean false if and only if split is true and mz is both less than 
        /// the lower boundary and greater than the upper boundary</returns>
        public bool ValidateAlignmentBoundary(bool split, double mz, AlignmentMZBoundary boundary)
        {
            if (split)
            {
                return (mz < boundary.BoundaryHigh && mz < boundary.BoundaryLow);
            }
            return true;
        }

        /// <summary>
        /// Applies a previously determined NET/Mass function to a dataset of UMCLights
        /// </summary>
        /// <param name="data"></param>
        public void ApplyNetMassFunctionToAlignee(List<UMCLight> data)
        {
            // If there isn't a LCMSWarp object, create one first.
            if (m_lcmsWarp == null)
            {
                m_lcmsWarp = new LcmsWarp();
            }

            m_percentDone = 0;
            var umcIndicies = new List<int>();
            var umcCalibratedMasses = new List<double>();
            var umcAlignedNets = new List<double>();
            var umcAlignedScans = new List<int>();
            var umcDriftTimes = new List<double>();

            if (m_aligningToMassTagDb)
            {
                m_lcmsWarp.GetFeatureCalibratedMassesAndAlignedNets(ref umcIndicies, ref umcCalibratedMasses,
                                                                    ref umcAlignedNets, ref umcDriftTimes);

                for (var i = 0; i < umcIndicies.Count; i++)
                {
                    data[i].ID = umcIndicies[i];
                    data[i].MassMonoisotopicAligned = umcCalibratedMasses[i];
                    data[i].NETAligned = umcAlignedNets[i];
                    data[i].DriftTime = umcDriftTimes[i];
                }

            }
            else
            {
                m_lcmsWarp.GetFeatureCalibratedMassesAndAlignedNets(ref umcIndicies, ref umcCalibratedMasses,
                                                                    ref umcAlignedNets, ref umcAlignedScans,
                                                                    ref umcDriftTimes, m_minReferenceDatasetScan,
                                                                    m_maxReferenceDatasetScan);

                for (var i = 0; i < umcIndicies.Count; i++)
                {
                    data[i].ID = umcIndicies[i];
                    data[i].MassMonoisotopicAligned = umcCalibratedMasses[i];
                    data[i].ScanAligned = umcAlignedScans[i];
                    data[i].NETAligned = umcAlignedNets[i];
                    data[i].DriftTime = umcDriftTimes[i];
                }
            }
        }

        /// <summary>
        /// For a given List of UMCLights, warp the alignee features to the baseline.
        /// Above boundary will determine if we should allow features above the m/z range or below
        /// if the splitMZBoundary flag is set to true in the alignment options
        /// </summary>
        /// <param name="features"></param>
        /// <param name="boundary"></param>
        public void SetAligneeDatasetFeatures(List<UMCLight> features, AlignmentMZBoundary boundary)
        {
            m_percentDone = 0;
            m_clusterAlignee = false;
            var numPts = features.Count;

            var mtFeatures = new List<MassTimeFeature> {Capacity = numPts};

            m_minAligneeDatasetScan = int.MaxValue;
            m_maxAligneeDatasetScan = int.MinValue;
            m_minAligneeDatasetMz = double.MaxValue;
            m_maxAligneeDatasetMz = -1 * double.MaxValue;


            for (var index = 0; index < numPts; index++)
            {
                var mtFeature = new MassTimeFeature();
                m_percentDone = (index * 100) / numPts;

                mtFeature.MonoMass = features[index].MassMonoisotopic;
                mtFeature.MonoMassCalibrated = features[index].MassMonoisotopicAligned;
                mtFeature.MonoMassOriginal = features[index].MassMonoisotopic;
                mtFeature.NET = Convert.ToDouble(features[index].Scan);

                // For if we want to split alignment at given M/Z range
                mtFeature.MZ = features[index].Mz;
                mtFeature.Abundance = features[index].Abundance;
                mtFeature.ID = features[index].ID;
                mtFeature.DriftTime = features[index].DriftTime;

                // Only allow feature to be aligned if we're splitting the alignment in MZ
                // AND if we are within the specified boundary
                if (ValidateAlignmentBoundary(m_options.AlignSplitMZs, mtFeature.MZ, boundary))
                {
                    mtFeatures.Add(mtFeature);

                    if (features[index].Scan > m_maxAligneeDatasetScan)
                    {
                        m_maxAligneeDatasetScan = features[index].Scan;
                    }
                    if (features[index].Scan < m_minAligneeDatasetScan)
                    {
                        m_minAligneeDatasetScan = features[index].Scan;
                    }
                    if (features[index].Mz > m_maxAligneeDatasetMz)
                    {
                        m_maxAligneeDatasetMz = features[index].Mz;
                    }
                    if (features[index].Mz < m_minAligneeDatasetMz)
                    {
                        m_minAligneeDatasetMz = features[index].Mz;
                    }
                }
            }
            m_lcmsWarp.SetFeatures(ref mtFeatures);
        }


        /// <summary>
        /// For a given array of UMCLights, warps the alignee featurs to the baseline. Above boundary
        /// will determine if we should allow features above the m/z range or below if the splitMZBoundary
        /// flag is set to true in the alignment options.
        /// 
        /// Similar to ApplyNETMassFunctionToAligneeDatasetFeatures, Original c++ processor had
        /// a separate class for a list of UMCData, but UMCLight does not have a similar class. 
        /// Might be able to kill this method if it's not used
        /// </summary>
        /// <param name="features"></param>
        /// <param name="datasetIndex"></param>
        /// <param name="boundary"></param>
        public void SetAligneeDatasetFeatures(UMCLight[] features, int datasetIndex,
                                              AlignmentMZBoundary boundary)
        {
            m_percentDone = 0;
            m_clusterAlignee = false;
            var numPts = features.Length;

            var mtFeatures = new List<MassTimeFeature> {Capacity = numPts};

            m_minAligneeDatasetScan = int.MaxValue;
            m_maxAligneeDatasetScan = int.MinValue;
            m_minAligneeDatasetMz = double.MaxValue;
            m_maxAligneeDatasetMz = -1 * double.MaxValue;


            for (var index = 0; index < numPts; index++)
            {
                var mtFeature = new MassTimeFeature();
                m_percentDone = (index * 100) / numPts;

                mtFeature.MonoMass = features[index].MassMonoisotopic;
                mtFeature.MonoMassCalibrated = features[index].MassMonoisotopicAligned;
                mtFeature.MonoMassOriginal = features[index].MassMonoisotopic;
                mtFeature.NET = Convert.ToDouble(features[index].Scan);

                // For if we want to split alignment at given M/Z range
                mtFeature.MZ = features[index].Mz;
                mtFeature.Abundance = features[index].Abundance;
                mtFeature.ID = features[index].ID;
                mtFeature.DriftTime = features[index].DriftTime;

                // Only allow feature to be aligned if we're splitting the alignment in MZ
                // AND if we are within the specified boundary
                if (ValidateAlignmentBoundary(m_options.AlignSplitMZs, mtFeature.MZ, boundary))
                {
                    mtFeatures.Add(mtFeature);

                    if (features[index].Scan > m_maxAligneeDatasetScan)
                    {
                        m_maxAligneeDatasetScan = features[index].Scan;
                    }
                    if (features[index].Scan < m_minAligneeDatasetScan)
                    {
                        m_minAligneeDatasetScan = features[index].Scan;
                    }
                    if (features[index].Mz > m_maxAligneeDatasetMz)
                    {
                        m_maxAligneeDatasetMz = features[index].Mz;
                    }
                    if (features[index].Mz < m_minAligneeDatasetMz)
                    {
                        m_minAligneeDatasetMz = features[index].Mz;
                    }
                }
            }
            m_lcmsWarp.SetFeatures(ref mtFeatures);
        }

        /// <summary>
        /// 
        /// Use the NET value of the UMCs in the List as the value to align to, the predictor variable
        /// </summary>
        /// <param name="umcData"></param>
        public void SetReferenceDatasetFeatures(List<UMCLight> umcData)
        {
            m_aligningToMassTagDb = false;
            m_massTagDbLoaded = false;
            m_percentDone = 0;

            var numPts = umcData.Count;

            var mtFeatures = new List<MassTimeFeature> {Capacity = numPts};

            m_minAligneeDatasetScan = int.MaxValue;
            m_maxAligneeDatasetScan = int.MinValue;

            for (var index = 0; index < numPts; index++)
            {
                var feature = new MassTimeFeature();
                m_percentDone = (index * 100) / numPts;
                feature.MonoMass = umcData[index].MassMonoisotopic;
                feature.MonoMassCalibrated = umcData[index].MassMonoisotopicAligned;
                feature.MonoMassOriginal = umcData[index].MassMonoisotopic;
                feature.NET = umcData[index].NET;
                feature.MZ = umcData[index].Mz;
                feature.Abundance = umcData[index].Abundance;
                feature.DriftTime = umcData[index].DriftTime;
                feature.ID = umcData[index].ID;

                mtFeatures.Add(feature);

                if (umcData[index].Scan > m_maxReferenceDatasetScan)
                {
                    m_maxReferenceDatasetScan = umcData[index].Scan;
                }
                if (umcData[index].Scan < m_minReferenceDatasetScan)
                {
                    m_minReferenceDatasetScan = umcData[index].Scan;
                }
                if (umcData[index].Mz > m_maxAligneeDatasetMz)
                {
                    m_maxAligneeDatasetMz = umcData[index].Mz;
                }
                if (umcData[index].Mz < m_minAligneeDatasetMz)
                {
                    m_minAligneeDatasetMz = umcData[index].Mz;
                }
            }
            m_lcmsWarp.SetReferenceFeatures(ref mtFeatures);
        }

        /// <summary>
        /// TEMPORARY FROM C++ PORT. UMCLight from PNNLOmics does not have a seperate collection class 
        /// that clsUMCData had.
        /// Use the NET value of the UMCs in the Array as the value to align to, the predictor variable
        /// </summary>
        /// <param name="umcData"></param>
        public void SetReferenceDatasetFeatures(UMCLight[] umcData)
        {
            m_aligningToMassTagDb = false;
            m_massTagDbLoaded = false;
            m_percentDone = 0;

            var numPts = umcData.Length;

            var mtFeatures = new List<MassTimeFeature> {Capacity = numPts};

            m_minAligneeDatasetScan = int.MaxValue;
            m_maxAligneeDatasetScan = int.MinValue;

            for (var index = 0; index < numPts; index++)
            {
                m_percentDone = (index * 100) / numPts;

                var feature = new MassTimeFeature
                {
                    MonoMass = umcData[index].MassMonoisotopic,
                    MonoMassCalibrated = umcData[index].MassMonoisotopicAligned,
                    MonoMassOriginal = umcData[index].MassMonoisotopic,
                    NET = umcData[index].NET,
                    MZ = umcData[index].Mz,
                    Abundance = umcData[index].Abundance,
                    DriftTime = umcData[index].DriftTime,
                    ID = umcData[index].ID
                };
                mtFeatures.Add(feature);

                if (umcData[index].Scan > m_maxReferenceDatasetScan)
                {
                    m_maxReferenceDatasetScan = umcData[index].Scan;
                }
                if (umcData[index].Scan < m_minReferenceDatasetScan)
                {
                    m_minReferenceDatasetScan = umcData[index].Scan;
                }
                if (umcData[index].Mz > m_maxAligneeDatasetMz)
                {
                    m_maxAligneeDatasetMz = umcData[index].Mz;
                }
                if (umcData[index].Mz < m_minAligneeDatasetMz)
                {
                    m_minAligneeDatasetMz = umcData[index].Mz;
                }
            }
            m_lcmsWarp.SetReferenceFeatures(ref mtFeatures);
        }

        /// <summary>
        /// Sets alignment features for a MSFeature dataset from a database
        /// </summary>
        /// <param name="features"></param>
        /// <param name="isDatabase"></param>
        public void SetReferenceDatasetFeatures(List<MassTagLight> features, bool isDatabase)
        {
            m_percentDone = 0;
            m_aligningToMassTagDb = true;
            var numMassTags = features.Count;

            var mtFeatures = new List<MassTimeFeature> {Capacity = numMassTags};

            foreach (var item in features)
            {
                var mtFeature = new MassTimeFeature
                {
                    AlignedNet = item.NETAligned,
                    MonoMass = item.MassMonoisotopic,
                    MonoMassCalibrated = item.MassMonoisotopicAligned,
                    MonoMassOriginal = item.MassMonoisotopic,
                    MZ = item.MassMonoisotopic/item.ChargeState + (1.00782*item.ChargeState - 1),
                    NET = item.NET,
                    DriftTime = item.DriftTime,
                    ID = item.ID,
                    ConformerID = item.GroupID
                };
                // Updated from C++ code, uses the charge from the item itself alongside the mass 
                // rather than trying to calculate it blindly. Originally always divided by 2
                mtFeatures.Add(mtFeature);
            }

            m_lcmsWarp.SetReferenceFeatures(ref mtFeatures);
            m_massTagDbLoaded = true;
        }


        /// <summary>
        /// MAY BE ABLE TO KILL THIS METHOD
        /// 
        /// Takes a list of UMCLights and initializes the reference dataset features to the features that
        /// pass the boundaries set in the options.
        /// Index Parameters not used, but included in code for compatability with C++ version
        /// </summary>
        /// <param name="umcData"></param>
        /// <param name="aligneeDatasetIndex"></param>
        /// <param name="referenceDatasetIndex"></param>
        public void SetDataForAlignmentToMsFeatures(List<UMCLight> umcData, int aligneeDatasetIndex,
                                                    int referenceDatasetIndex)
        {
            SetAligneeDatasetFeatures(umcData, m_options.MzBoundaries[0]);

            SetReferenceDatasetFeatures(umcData);
        }



        public AlignmentFunction GetAlignmentFunction()
        {
            var func = new AlignmentFunction(m_options.CalibType, m_options.AlignType);

            var aligneeNets = new List<double>();
            var referenceNets = new List<double>();
            m_lcmsWarp.AlignmentFunction(ref aligneeNets, ref referenceNets);

            if (m_aligningToMassTagDb)
            {
                func.SetNetFunction(aligneeNets, referenceNets);
            }
            else
            {
                var referenceScans = new List<double>();
                var numSections = referenceNets.Count;
                for (var sectionNum = 0; sectionNum < numSections; sectionNum++)
                {
                    referenceScans.Add(m_minReferenceDatasetScan + referenceNets[sectionNum] * (m_maxReferenceDatasetScan - m_minReferenceDatasetScan));
                }
                func.SetNetFunction(ref aligneeNets, ref referenceNets, ref referenceScans);
            }

            if (m_options.AlignType == AlignmentOptions.AlignmentType.NET_WARP)
            {
                return func;
            }

            var minAligneeNet = m_lcmsWarp.MinNet;
            var maxAligneeNet = m_lcmsWarp.MaxNet;
            //double minBaselineNET = m_LCMSWarp.MinBaselineNet;
            //double maxBaselineNET = m_LCMSWarp.MaxBaselineNet;

            if (m_options.CalibType == AlignmentOptions.CalibrationType.SCAN_CALIBRATION ||
                m_options.CalibType == AlignmentOptions.CalibrationType.HYBRID_CALIBRATION)
            {
                // Get the mass calibration function with time
                var numXKnots = m_options.MassCalibNumXSlices;
                var aligneeNetMassFunc = new List<double>();
                var aligneePpmShiftMassFunc = new List<double>();

                // get the PPM for each knot
                for (var knotNum = 0; knotNum < numXKnots; knotNum++)
                {
                    var net = minAligneeNet + ((maxAligneeNet - minAligneeNet) * knotNum) / numXKnots;
                    var ppm = m_lcmsWarp.GetPpmShiftFromNet(net);
                    aligneeNetMassFunc.Add(net);
                    aligneePpmShiftMassFunc.Add(ppm);
                }
                func.SetMassCalibrationFunctionWithTime(ref aligneeNetMassFunc, ref aligneePpmShiftMassFunc);
            }

            if (m_options.CalibType == AlignmentOptions.CalibrationType.MZ_CALIBRATION ||
                m_options.CalibType == AlignmentOptions.CalibrationType.HYBRID_CALIBRATION)
            {
                // Get the mass calibration function with time
                var numXKnots = m_options.MassCalibNumXSlices;
                var aligneeMzMassFunc = new List<double>();
                var aligneePpmShiftMassFunc = new List<double>();

                // Get the ppm for each knot
                for (var knotNum = 0; knotNum < numXKnots; knotNum++)
                {
                    var net = knotNum * 1.0 / numXKnots;
                    var mz = m_minAligneeDatasetMz + (int)((m_maxAligneeDatasetMz - m_minAligneeDatasetMz) * net);
                    var ppm = m_lcmsWarp.GetPpmShiftFromMz(mz);
                    aligneeMzMassFunc.Add(mz);
                    aligneePpmShiftMassFunc.Add(ppm);
                }
                func.SetMassCalibrationFunctionWithMz(aligneeMzMassFunc, aligneePpmShiftMassFunc);
            }
            return func;
        }

        public void PerformAlignmentToMsFeatures()
        {
            if (m_options == null)
            {
                throw new NullReferenceException("Alignment Options were not set in AlignmentProcessor");
            }

            if (m_options.AlignType != AlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                PerformNetWarp();
            }
            else
            {
                PerformNetMassWarp();
            }
        }

        public void PerformAlignmentToMassTagDatabase()
        {
            if (m_options == null)
            {
                throw new NullReferenceException("Alignment Options were not set in AlignmentProcessor");
            }
            if (m_options.AlignType != AlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                PerformNetWarp();
            }
            else
            {
                PerformNetMassWarp();
            }
        }

        public void PerformNetWarp()
        {
            m_lcmsWarp.GenerateCandidateMatches();
            if (m_lcmsWarp.NumCandidateMatches < 10)
            {
                throw new ApplicationException("Insufficient number of candidate matches by mass alone");
            }

            m_lcmsWarp.GetMatchProbabilities();
            m_lcmsWarp.CalculateAlignmentMatrix();
            m_lcmsWarp.CalculateAlignmentFunction();
            m_lcmsWarp.GetTransformedNets();
            m_lcmsWarp.CalculateAlignmentMatches();
        }

        public void PerformNetMassWarp()
        {
            // First, perform the net calibration using a mass tolerance of the same size as the mass window
            // and then perform the net calibration again using the appropriate mass tolerance
            var massTolerance = m_lcmsWarp.MassTolerance;
            m_lcmsWarp.MassTolerance = m_lcmsWarp.MassCalibrationWindow;
            m_lcmsWarp.UseMassAndNetScore(false);

            PerformNetWarp();

            m_lcmsWarp.PerformMassCalibration();
            m_lcmsWarp.CalculateStandardDeviations();
            m_lcmsWarp.MassTolerance = massTolerance;
            m_lcmsWarp.UseMassAndNetScore(true);
            PerformNetWarp();
        }


        public double[,] GetAlignmentHeatMap(out double[,] outputScores, out double[] xIntervals,
                                        out double[] yIntervals)
        {
            if (m_lcmsWarp == null)
            {
                m_lcmsWarp = new LcmsWarp();
            }

            var alignmentScores = new List<double>();
            var aligneeIntervals = new List<double>();
            var baselineIntervals = new List<double>();

            // Original .cpp code did not include the boolean standardize. Hard coding to false for the moment
            // Mike - 3/10/14
            m_lcmsWarp.GetSubsectionMatchScore(ref alignmentScores, ref aligneeIntervals, ref baselineIntervals, false);

            var numBaselineSections = baselineIntervals.Count;
            var numAligneeSections = aligneeIntervals.Count;

            outputScores = new double[numBaselineSections, numAligneeSections];

            var numTotalSections = alignmentScores.Count;
            if (numTotalSections != numBaselineSections * numAligneeSections)
            {
                throw new ApplicationException("Error in Alignment heatmap scores. Total section is not as expected");
            }

            var aligneeSection = 0;
            var baselineSection = 0;
            for (var i = 0; i < numTotalSections; i++)
            {
                outputScores[baselineSection, aligneeSection] = alignmentScores[i];
                baselineSection++;
                if (baselineSection != numBaselineSections)
                {
                    continue;
                }
                baselineSection = 0;    
                aligneeSection++;
                
            }

            xIntervals = new double[numAligneeSections];
            for (var i = 0; i < numAligneeSections; i++)
            {
                xIntervals[i] = aligneeIntervals[i];
            }

            yIntervals = new double[numBaselineSections];
            for (var i = 0; i < numBaselineSections; i++)
            {
                yIntervals[i] = baselineIntervals[i];
            }

            return outputScores;
        }


        public void GetReferenceNetRange(out double minRefNet, out double maxRefNet)
        {
            minRefNet = m_lcmsWarp.MinBaselineNet;
            maxRefNet = m_lcmsWarp.MaxBaselineNet;
        }

        #region Public Statistic Getter properties
        public double MassStd
        {
            get
            {
                double massStd, netStd, netMu, massMu;
                m_lcmsWarp.GetStatistics(out massStd, out netStd, out massMu, out netMu);
                return massStd;
            }
        }
        public double NetStd
        {
            get
            {
                double massStd, netStd, netMu, massMu;
                m_lcmsWarp.GetStatistics(out massStd, out netStd, out massMu, out netMu);
                return netStd;
            }
        }
        public double MassMu
        {
            get
            {
                double massStd, netStd, netMu, massMu;
                m_lcmsWarp.GetStatistics(out massStd, out netStd, out massMu, out netMu);
                return massMu;
            }
        }
        public double NetMu
        {
            get
            {
                double massStd, netStd, netMu, massMu;
                m_lcmsWarp.GetStatistics(out massStd, out netStd, out massMu, out netMu);
                return netMu;
            }
        }
        #endregion

        //TODO: Redesign this so that when we say "align(x,y)" we get this in an object separate from everything
        public void GetErrorHistograms(double massBin, double netBin, double driftBin,
                        out double[,] massHistogram, out double[,] netHistogram, out double[,] driftHistogram)
        {
            var massErrorBin = new List<double>();
            var netErrorBin = new List<double>();
            var driftErrorBin = new List<double>();
            var massErrorFreq = new List<int>();
            var netErrorFreq = new List<int>();
            var driftErrorFreq = new List<int>();

            //ErrorHistogram massErrorHistogram = new ErrorHistogram(massErrorBin, massErrorFreq);
            //ErrorHistogram netErrorHistogram = new ErrorHistogram(netErrorBin, netErrorFreq);
            //ErrorHistogram driftErrorHistogram = new ErrorHistogram(driftErrorBin, driftErrorFreq);

            massHistogram = new double[massErrorBin.Count, 2];
            netHistogram = new double[netErrorBin.Count, 2];
            driftHistogram = new double[driftErrorBin.Count, 2];

            for (var i = 0; i < massErrorBin.Count; i++)
            {
                massHistogram[i, 0] = massErrorBin[i];
                massHistogram[i, 1] = massErrorFreq[i];
            }
            for (var i = 0; i < netErrorBin.Count; i++)
            {
                netHistogram[i, 0] = netErrorBin[i];
                netHistogram[i, 1] = netErrorFreq[i];
            }
            for (var i = 0; i < driftErrorBin.Count; i++)
            {
                driftHistogram[i, 0] = driftErrorBin[i];
                driftHistogram[i, 1] = driftErrorFreq[i];
            }
        }

        /// <summary>
        /// Calculates all the residual data for the alignment and returns an object
        /// holding all of the residual data in the Residual Data object.
        /// </summary>
        /// <returns></returns>
        public ResidualData GetResidualData()
        {
            var net = new List<double>();
            var mz = new List<double>();
            var linearNet = new List<double>();
            var customNet = new List<double>();
            var linearCustomNet = new List<double>();
            var massError = new List<double>();
            var massErrorCorrected = new List<double>();

            m_lcmsWarp.GetResiduals(ref net, ref mz, ref linearNet, ref customNet, ref linearCustomNet,
                                    ref massError, ref massErrorCorrected);

            var count = net.Count;

            var scans = new double[count];
            var mzs = new double[count];
            var linearNets = new double[count];
            var customNets = new double[count];
            var linearCustomNets = new double[count];
            var massErrors = new double[count];
            var massErrorCorrecteds = new double[count];
            var mzMassErrors = new double[count];
            var mzMassErrorCorrecteds = new double[count];

            //List<ResidualData> dataList = new List<ResidualData>();
            for (int i = 0; i < count; i++)
            {

                scans[i] = net[i];
                mzs[i] = mz[i];
                linearNets[i] = linearNet[i];
                customNets[i] = customNet[i];
                linearCustomNets[i] = linearCustomNet[i];
                massErrors[i] = massError[i];
                massErrorCorrecteds[i] = massErrorCorrected[i];
                mzMassErrors[i] = massError[i];
                mzMassErrorCorrecteds[i] = massErrorCorrected[i];

            }

            var data = new ResidualData
            {
                Scan = scans,
                Mz = mzs,
                LinearNet = linearNets,
                CustomNet = customNets,
                LinearCustomNet = linearCustomNets,
                MassError = massErrors,
                MassErrorCorrected = massErrorCorrecteds,
                MzMassError = mzMassErrors,
                MzMassErrorCorrected = mzMassErrorCorrecteds
            };

            return data;
        }
    }
}
