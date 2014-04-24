using System;
using System.Collections.Generic;
using System.Linq;
using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;

namespace PNNLOmics.Algorithms.Alignment.LcmsWarp
{    
    /// <summary>
    /// Class to hold the residual data from the alignment process
    /// </summary>
    public class ResidualData
    {
        #region Public Properties
        /// <summary>
        /// AutoProperty to hold all the scans from the alignment data
        /// </summary>
        public double[] Scan { get; set; }

        /// <summary>
        /// AutoProperty to hold all the M/Z from the alignment data
        /// </summary>
        public double[] Mz { get; set; }

        /// <summary>
        /// AutoProperty to hold all the Linear NETs from the alignment data
        /// </summary>
        public double[] LinearNet { get; set; }

        /// <summary>
        /// AutoProperty to hold all the NET Errors from the alignment data
        /// </summary>
        public double[] CustomNet { get; set; }

        /// <summary>
        /// Autoproperty, holds the Aligned net minus the predicted linear net the data
        /// </summary>
        public double[] LinearCustomNet { get; set; }

        /// <summary>
        /// Autoproperty to hold all the Mass Errors from the alignment data
        /// </summary>
        public double[] MassError { get; set; }

        /// <summary>
        /// AutoProperty to hold all the corrected mass error (mass error - ppmShift) for the alignment
        /// </summary>
        public double[] MassErrorCorrected { get; set; }

        /// <summary>
        /// Autoproperty to hold all the Mass Errors from the alignment data
        /// Same as MassError
        /// </summary>
        public double[] MzMassError { get; set; }

        /// <summary>
        /// AutoProperty to hold all the corrected mass error (mass error - ppmShift) for the alignment
        /// Same as MassErrorCorrected
        /// </summary>
        public double[] MzMassErrorCorrected { get; set; }

        #endregion
    }

    /// <summary>
    /// Class which will use LCMSWarp to process alignment
    /// </summary>
    public class LcmsWarpAlignmentProcessor
    {
        // In case alignment was to ms features, this will kepe track of the minimum scan in the
        // reference features. They are needed because LCMSWarp uses NET values for reference and
        // are scaled to between 0 and 1. These will scale it back to actual scan numbers
        int m_minReferenceDatasetScan;
        int m_maxReferenceDatasetScan;

        int m_minAligneeDatasetScan;
        int m_maxAligneeDatasetScan;
        double m_minAligneeDatasetMz;
        double m_maxAligneeDatasetMz;

        // LCMSWarp instance which will do the alignment when processing
        LcmsWarp m_lcmsWarp;

        #region Public properties
        /// <summary>
        /// Get property for the NET Intercept that LCMS Warp is holding
        /// Simulates a pure linear regression
        /// </summary>
        public double NetIntercept
        {
            get { return m_lcmsWarp.NetIntercept; }
        }
        /// <summary>
        /// Get property for the NET RSquared that LCMS Warp is holding
        /// Simulates a pure linear regression
        /// </summary>
        public double NetRsquared
        {
            get { return m_lcmsWarp.NetLinearRsq; }
        }
        /// <summary>
        /// Get property for the NET Slope that LCMS Warp is holding
        /// Simulates a pure linear regression
        /// </summary>
        public double NetSlope
        {
            get { return m_lcmsWarp.NetSlope; }
        }
        /// <summary>
        /// Options for the Alignment processor
        /// </summary>
        public LcmsWarpAlignmentOptions Options { get; set; }
        /// <summary>
        /// Flag for if the Processor is aligning to a Mass Tag Database
        /// </summary>
        public bool AligningToMassTagDb { get; set; }
        /// <summary>
        /// Flag for if the Processor has loaded a MassTag Database
        /// </summary>
        public bool MassTagDbLoaded { get; set; }
        /// <summary>
        /// Flag for if the alignees are clusters
        /// </summary>
        public bool ClusterAlignee { get; set; }
        /// <summary>
        /// Property to keep track of the percent done for the Processor
        /// </summary>
        public int PercentDone { get; set; }

        #endregion

        /// <summary>
        /// Public constructor for the LCMS Alignment Processor
        /// Initializes a new LCMSWarp object using the LCMS Alignment options
        /// which were passed into the Processor
        /// </summary>
        public LcmsWarpAlignmentProcessor()
        {
            m_lcmsWarp = new LcmsWarp();
            Options = new LcmsWarpAlignmentOptions();

            ApplyAlignmentOptions();

            AligningToMassTagDb = false;
            MassTagDbLoaded = false;
            ClusterAlignee = false;
        }

        // Applies the alignment options to the LCMSWarper, setting the Mass
        // and NET Tolerances, the options for NET Alignment options for 
        // Mass calibration, the Least Squares options and the calibration type
        private void ApplyAlignmentOptions()
        {
            // Applying the Mass and NET Tolerances
            m_lcmsWarp.MassTolerance = Options.MassTolerance;
            m_lcmsWarp.NetTolerance = Options.NetTolerance;

            // Applying options for NET Calibration
            m_lcmsWarp.NumSections = Options.NumTimeSections;
            m_lcmsWarp.MaxSectionDistortion = Options.ContractionFactor;
            m_lcmsWarp.MaxJump = Options.MaxTimeDistortion;
            m_lcmsWarp.NumBaselineSections = Options.NumTimeSections * Options.ContractionFactor;
            m_lcmsWarp.NumMatchesPerBaseline = Options.ContractionFactor*Options.ContractionFactor;
            m_lcmsWarp.NumMatchesPerSection = m_lcmsWarp.NumBaselineSections * m_lcmsWarp.NumMatchesPerBaseline;
            m_lcmsWarp.MaxPromiscuousUmcMatches = Options.MaxPromiscuity;

            // Applying options for Mass Calibration
            m_lcmsWarp.MassCalibrationWindow = Options.MassCalibrationWindow;
            m_lcmsWarp.MassCalNumDeltaBins = Options.MassCalibNumYSlices;
            m_lcmsWarp.MassCalNumSlices = Options.MassCalibNumXSlices;
            m_lcmsWarp.MassCalNumJump = Options.MassCalibMaxJump;

            var regType = LcmsWarpRegressionType.Central;

            if (Options.MassCalibUseLsq)
            {
                regType = LcmsWarpRegressionType.Hybrid;
            }
            m_lcmsWarp.MzRecalibration.SetCentralRegressionOptions(m_lcmsWarp.MassCalNumSlices, m_lcmsWarp.MassCalNumDeltaBins,
                                                                   m_lcmsWarp.MassCalNumJump, Options.MassCalibMaxZScore,
                                                                   regType);
            m_lcmsWarp.NetRecalibration.SetCentralRegressionOptions(m_lcmsWarp.MassCalNumSlices, m_lcmsWarp.MassCalNumDeltaBins,
                                                                    m_lcmsWarp.MassCalNumJump, Options.MassCalibMaxZScore,
                                                                    regType);

            // Applying LSQ options
            m_lcmsWarp.MzRecalibration.SetLsqOptions(Options.MassCalibLsqNumKnots, Options.MassCalibLsqMaxZScore);
            m_lcmsWarp.NetRecalibration.SetLsqOptions(Options.MassCalibLsqNumKnots, Options.MassCalibLsqMaxZScore);

            // Setting the calibration type
            m_lcmsWarp.CalibrationType = (LcmsWarpCalibrationType)Options.CalibType;
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

            PercentDone = 0;
            var umcIndices = new List<int>();
            var umcCalibratedMasses = new List<double>();
            var umcAlignedNets = new List<double>();
            var umcAlignedScans = new List<int>();
            var umcDriftTimes = new List<double>();

            if (AligningToMassTagDb)
            {
                m_lcmsWarp.GetFeatureCalibratedMassesAndAlignedNets(ref umcIndices, ref umcCalibratedMasses,
                                                                    ref umcAlignedNets, ref umcDriftTimes);

                for (int i = 0; i < umcIndices.Count; i++)
                {
                    var point = new UMCLight
                    {
                        ID = umcIndices[i],
                        MassMonoisotopicAligned = umcCalibratedMasses[i],
                        NETAligned = umcAlignedNets[i],
                        DriftTime = umcDriftTimes[i]
                    };

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
                    var point = new UMCLight
                    {
                        ID = umcIndices[i],
                        MassMonoisotopicAligned = umcCalibratedMasses[i],
                        NETAligned = umcAlignedNets[i],
                        ScanAligned = umcAlignedScans[i],
                        DriftTime = umcDriftTimes[i]
                    };

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

            PercentDone = 0;
            var umcIndices = new List<int>();
            var umcCalibratedMasses = new List<double>();
            var umcAlignedNets = new List<double>();
            var umcAlignedScans = new List<int>();
            var umcDriftTimes = new List<double>();

            if (AligningToMassTagDb)
            {
                m_lcmsWarp.GetFeatureCalibratedMassesAndAlignedNets(ref umcIndices, ref umcCalibratedMasses,
                                                                    ref umcAlignedNets, ref umcDriftTimes);
                for (int i = 0; i < umcIndices.Count; i++)
                {
                    var point = new UMCLight
                    {
                        ID = umcIndices[i],
                        MassMonoisotopicAligned = umcCalibratedMasses[i],
                        NETAligned = umcAlignedNets[i],
                        DriftTime = umcDriftTimes[i]
                    };

                    if (i < dataList.Length)
                    {
                        dataList[i] = point;
                    }
                    else
                    {
                        var newDataList = new UMCLight[dataList.Length + 1];
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
                    var point = new UMCLight
                    {
                        ID = umcIndices[i],
                        MassMonoisotopicAligned = umcCalibratedMasses[i],
                        NETAligned = umcAlignedNets[i],
                        ScanAligned = umcAlignedScans[i],
                        DriftTime = umcDriftTimes[i]
                    };

                    if (i < dataList.Length)
                    {
                        dataList[i] = point;
                    }
                    else
                    {
                        var newDataList = new UMCLight[dataList.Length + 1];
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

            PercentDone = 0;
            var umcIndicies = new List<int>();
            var umcCalibratedMasses = new List<double>();
            var umcAlignedNets = new List<double>();
            var umcAlignedScans = new List<int>();
            var umcDriftTimes = new List<double>();

            if (AligningToMassTagDb)
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
        /// </summary>
        /// <param name="features"></param>
        public void SetAligneeDatasetFeatures(List<UMCLight> features)
        {
            PercentDone = 0;
            ClusterAlignee = false;
            var numPts = features.Count;

            var mtFeatures = new List<UMCLight> { Capacity = numPts };

            m_minAligneeDatasetScan = int.MaxValue;
            m_maxAligneeDatasetScan = int.MinValue;
            m_minAligneeDatasetMz = double.MaxValue;
            m_maxAligneeDatasetMz = double.MinValue;


            for (var index = 0; index < numPts; index++)
            {
                var mtFeature = new UMCLight();
                PercentDone = (index * 100) / numPts;

                mtFeature.MassMonoisotopic = features[index].MassMonoisotopic;
                mtFeature.MassMonoisotopicAligned = features[index].MassMonoisotopicAligned;
                //mtFeature.MonoMassOriginal = features[index].MassMonoisotopic;
                mtFeature.NET = Convert.ToDouble(features[index].Scan);

                // For if we want to split alignment at given M/Z range
                mtFeature.Mz = features[index].Mz;
                mtFeature.Abundance = features[index].Abundance;
                mtFeature.ID = features[index].ID;
                mtFeature.DriftTime = features[index].DriftTime;

                // Only allow feature to be aligned if we're splitting the alignment in MZ
                // AND if we are within the specified boundary
                //if (ValidateAlignmentBoundary(Options.AlignSplitMZs, mtFeature.Mz, boundary))
                //{
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
                //}
            }
            m_lcmsWarp.SetFeatures(ref mtFeatures);
        }


        /// <summary>
        /// For a given array of UMCLights, warps the alignee featurs to the baseline. 
        /// 
        /// Similar to ApplyNETMassFunctionToAligneeDatasetFeatures, Original c++ processor had
        /// a separate class for a list of UMCData, but UMCLight does not have a similar class. 
        /// Might be able to kill this method if it's not used
        /// </summary>
        /// <param name="features"></param>
        /// <param name="datasetIndex"></param>
        public void SetAligneeDatasetFeatures(UMCLight[] features, int datasetIndex)
        {
            PercentDone = 0;
            ClusterAlignee = false;
            var numPts = features.Length;

            var mtFeatures = new List<UMCLight> { Capacity = numPts };

            m_minAligneeDatasetScan = int.MaxValue;
            m_maxAligneeDatasetScan = int.MinValue;
            m_minAligneeDatasetMz = double.MaxValue;
            m_maxAligneeDatasetMz = double.MinValue;


            for (var index = 0; index < numPts; index++)
            {
                var mtFeature = new UMCLight();
                PercentDone = (index * 100) / numPts;
                var feature = features[index];
                mtFeature.MassMonoisotopic = feature.MassMonoisotopic;
                mtFeature.MassMonoisotopicAligned = feature.MassMonoisotopicAligned;
                mtFeature.NET = Convert.ToDouble(feature.Scan);

                // For if we want to split alignment at given M/Z range
                mtFeature.Mz = feature.Mz;
                mtFeature.Abundance = feature.Abundance;
                mtFeature.ID = feature.ID;
                mtFeature.DriftTime = feature.DriftTime;

                mtFeatures.Add(mtFeature);

                if (feature.Scan > m_maxAligneeDatasetScan)
                {
                    m_maxAligneeDatasetScan = feature.Scan;
                }
                if (feature.Scan < m_minAligneeDatasetScan)
                {
                    m_minAligneeDatasetScan = feature.Scan;
                }
                if (feature.Mz > m_maxAligneeDatasetMz)
                {
                    m_maxAligneeDatasetMz = feature.Mz;
                }
                if (feature.Mz < m_minAligneeDatasetMz)
                {
                    m_minAligneeDatasetMz = feature.Mz;
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
            AligningToMassTagDb = false;
            MassTagDbLoaded = false;
            PercentDone = 0;

            var numPts = umcData.Count;

            var mtFeatures = new List<UMCLight> { Capacity = numPts };

            m_minAligneeDatasetScan = int.MaxValue;
            m_maxAligneeDatasetScan = int.MinValue;

            for (var index = 0; index < numPts; index++)
            {
                var feature = new UMCLight();
                var data = umcData[index];
                PercentDone = (index * 100) / numPts;
                feature.MassMonoisotopic = data.MassMonoisotopic;
                feature.MassMonoisotopicAligned = data.MassMonoisotopicAligned;
                feature.NET = data.NET;
                feature.Mz = data.Mz;
                feature.Abundance = data.Abundance;
                feature.DriftTime = data.DriftTime;
                feature.ID = data.ID;

                mtFeatures.Add(feature);

                if (data.Scan > m_maxReferenceDatasetScan)
                {
                    m_maxReferenceDatasetScan = data.Scan;
                }
                if (data.Scan < m_minReferenceDatasetScan)
                {
                    m_minReferenceDatasetScan = data.Scan;
                }
                if (data.Mz > m_maxAligneeDatasetMz)
                {
                    m_maxAligneeDatasetMz = data.Mz;
                }
                if (data.Mz < m_minAligneeDatasetMz)
                {
                    m_minAligneeDatasetMz = data.Mz;
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
            AligningToMassTagDb = false;
            MassTagDbLoaded = false;
            PercentDone = 0;

            var numPts = umcData.Length;

            var mtFeatures = new List<UMCLight> { Capacity = numPts };

            m_minAligneeDatasetScan = int.MaxValue;
            m_maxAligneeDatasetScan = int.MinValue;

            for (var index = 0; index < numPts; index++)
            {
                PercentDone = (index * 100) / numPts;

                var data = umcData[index];
                var feature = new UMCLight
                {
                    MassMonoisotopic = data.MassMonoisotopic,
                    MassMonoisotopicAligned = data.MassMonoisotopicAligned,
                    NET = data.NET,
                    Mz = data.Mz,
                    Abundance = data.Abundance,
                    DriftTime = data.DriftTime,
                    ID = data.ID
                };
                mtFeatures.Add(feature);

                if (data.Scan > m_maxReferenceDatasetScan)
                {
                    m_maxReferenceDatasetScan = data.Scan;
                }
                if (data.Scan < m_minReferenceDatasetScan)
                {
                    m_minReferenceDatasetScan = data.Scan;
                }
                if (data.Mz > m_maxAligneeDatasetMz)
                {
                    m_maxAligneeDatasetMz = data.Mz;
                }
                if (data.Mz < m_minAligneeDatasetMz)
                {
                    m_minAligneeDatasetMz = data.Mz;
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
            PercentDone = 0;
            AligningToMassTagDb = true;
            var numMassTags = features.Count;

            var mtFeatures = new List<UMCLight> { Capacity = numMassTags };
            mtFeatures.AddRange(features.Select(item => new UMCLight
            {
                NETAligned = item.NETAligned, 
                MassMonoisotopic = item.MassMonoisotopic, 
                MassMonoisotopicAligned = item.MassMonoisotopicAligned, 
                Mz = item.MassMonoisotopic/item.ChargeState + (1.00782*(item.ChargeState - 1)), 
                NET = item.NET, 
                DriftTime = item.DriftTime, 
                ID = item.ID,
            }));

            m_lcmsWarp.SetReferenceFeatures(ref mtFeatures);
            MassTagDbLoaded = true;
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
            SetAligneeDatasetFeatures(umcData);

            SetReferenceDatasetFeatures(umcData);
        }

        /// <summary>
        /// Returns the Alignment Function that the processor determined
        /// </summary>
        /// <returns></returns>
        public LcmsWarpAlignmentFunction GetAlignmentFunction()
        {
            var func = new LcmsWarpAlignmentFunction(Options.CalibType, Options.AlignType);

            var aligneeNets = new List<double>();
            var referenceNets = new List<double>();
            m_lcmsWarp.AlignmentFunction(ref aligneeNets, ref referenceNets);

            if (AligningToMassTagDb)
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

            if (Options.AlignType == LcmsWarpAlignmentOptions.AlignmentType.NET_WARP)
            {
                return func;
            }

            var minAligneeNet = m_lcmsWarp.MinNet;
            var maxAligneeNet = m_lcmsWarp.MaxNet;

            if (Options.CalibType == LcmsWarpAlignmentOptions.CalibrationType.SCAN_CALIBRATION ||
                Options.CalibType == LcmsWarpAlignmentOptions.CalibrationType.HYBRID_CALIBRATION)
            {
                // Get the mass calibration function with time
                var numXKnots = Options.MassCalibNumXSlices;
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

            if (Options.CalibType == LcmsWarpAlignmentOptions.CalibrationType.MZ_CALIBRATION ||
                Options.CalibType == LcmsWarpAlignmentOptions.CalibrationType.HYBRID_CALIBRATION)
            {
                // Get the mass calibration function with time
                var numXKnots = Options.MassCalibNumXSlices;
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

        /// <summary>
        /// Method to determine which warping method to use.
        /// Throws exception if the options were not set.
        /// </summary>
        public void PerformAlignmentToMsFeatures()
        {
            if (Options == null)
            {
                throw new NullReferenceException("Alignment Options were not set in AlignmentProcessor");
            }

            if (Options.AlignType != LcmsWarpAlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                PerformNetWarp();
            }
            else
            {
                PerformNetMassWarp();
            }
        }

        /// <summary>
        /// Method to determine which warping method to use when aligning to a database.
        /// Throws exception if the options were not set.
        /// </summary>
        public void PerformAlignmentToMassTagDatabase()
        {
            if (Options == null)
            {
                throw new NullReferenceException("Alignment Options were not set in AlignmentProcessor");
            }
            if (Options.AlignType != LcmsWarpAlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                PerformNetWarp();
            }
            else
            {
                PerformNetMassWarp();
            }
        }

        /// <summary>
        /// Performs the NET Warping; Generates matches, gets the probabilities,
        /// calculates the alignment matrix and alignment function, gets the transformed NETs
        /// and then calculates the alignment matches
        /// </summary>
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

        /// <summary>
        /// Performs the NET-Mass Warping; Sets up the mass calibration settings from the options,
        /// performs NET warping, calibrates matches based on the NETWarp results, recalibrates the mass
        /// tolerance and then performs Warping again using the mass and Net scores
        /// </summary>
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

        /// <summary>
        /// Method to return the heatmap of the alignment (as a 2D array of doubles) based on
        /// the output scores taht 
        /// </summary>
        /// <param name="outputScores"></param>
        /// <param name="xIntervals"></param>
        /// <param name="yIntervals"></param>
        public void GetAlignmentHeatMap(out double[,] outputScores, out double[] xIntervals,
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
        }

        /// <summary>
        /// Method which copies the baseline nets into the parameters passed in
        /// </summary>
        /// <param name="minRefNet"></param>
        /// <param name="maxRefNet"></param>
        public void GetReferenceNetRange(out double minRefNet, out double maxRefNet)
        {
            minRefNet = m_lcmsWarp.MinBaselineNet;
            maxRefNet = m_lcmsWarp.MaxBaselineNet;
        }

        #region Public Statistic Getter properties
        /// <summary>
        /// Returns the Standard Deviation of the Mass of the data
        /// </summary>
        public double MassStd
        {
            get
            {
                double massStd, netStd, netMu, massMu;
                m_lcmsWarp.GetStatistics(out massStd, out netStd, out massMu, out netMu);
                return massStd;
            }
        }
        /// <summary>
        /// Returns the Standard Deviation of the NET of the data
        /// </summary>
        public double NetStd
        {
            get
            {
                double massStd, netStd, netMu, massMu;
                m_lcmsWarp.GetStatistics(out massStd, out netStd, out massMu, out netMu);
                return netStd;
            }
        }
        /// <summary>
        /// Returns the Mean of the Mass of the data
        /// </summary>
        public double MassMu
        {
            get
            {
                double massStd, netStd, netMu, massMu;
                m_lcmsWarp.GetStatistics(out massStd, out netStd, out massMu, out netMu);
                return massMu;
            }
        }
        /// <summary>
        /// Returns the Mean of the NET of the data
        /// </summary>
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
        /// <summary>
        /// Copies the histograms from the LCMS Warping and returns them through the Histogram parameters passed in
        /// </summary>
        /// <param name="massBin"></param>
        /// <param name="netBin"></param>
        /// <param name="driftBin"></param>
        /// <param name="massHistogram"></param>
        /// <param name="netHistogram"></param>
        /// <param name="driftHistogram"></param>
        public void GetErrorHistograms(double massBin, double netBin, double driftBin,
                        out double[,] massHistogram, out double[,] netHistogram, out double[,] driftHistogram)
        {
            var massErrorBin = new List<double>();
            var netErrorBin = new List<double>();
            var driftErrorBin = new List<double>();
            var massErrorFreq = new List<int>();
            var netErrorFreq = new List<int>();
            var driftErrorFreq = new List<int>();

            m_lcmsWarp.GetErrorHistograms(massBin, netBin, driftBin, ref massErrorBin, ref massErrorFreq, ref netErrorBin,
                                          ref netErrorFreq, ref driftErrorBin, ref driftErrorFreq);

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
