using System;
using System.Collections.Generic;
using PNNLOmics.Alignment.LCMSWarp.LCMSProcessor;
using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;

namespace PNNLOmics.Alignment.LCMSWarp.LCMSAligner
{
    /// <summary>
    /// Wrapper object for LCMSWarp Feature Aligning
    /// </summary>
    public class LcmsWarpFeatureAligner
    {
        /// <summary>
        /// Uses a list of MassTagLights as a baseline for aligning a list of UMCLights to it, using
        /// the passed in options for processor options as well as a boolean for whether to align based on
        /// Drift Times as well.
        /// Creates an LCMS Alignment processor to do the alignment, does the alignment, and then passes
        /// back the alignment data to the caller.
        /// </summary>
        /// <param name="massTags"></param>
        /// <param name="features"></param>
        /// <param name="options"></param>
        /// <param name="alignDriftTimes"></param>
        /// <returns></returns>
        public LcmsAlignmentData AlignFeatures(List<MassTagLight> massTags, List<UMCLight> features,
            LcmsAlignmentOptions options, bool alignDriftTimes)
        {
            var processor = new LcmsAlignmentProcessor
            {
                Options = options
            };

            var featureTest = features.Find(x => x.DriftTime > 0);
            var massTagTest = massTags.Find(x => x.DriftTime > 0);

            if (featureTest != null && massTagTest == null)
            {
                // Warming! Data has drift time info, but the mass tags do not.
            }

            processor.SetReferenceDatasetFeatures(massTags, true);

            var data = AlignFeatures(processor, features, options);

            return data;
        }

        /// <summary>
        /// Uses a list of UMCLights as a baseline for aligning a second list of UMCLights to it, using
        /// the passed in options for processor options
        /// Creates an LCMS Alignment processor to do the alignment, does the alignment, and then passes
        /// back the alignment data to the caller.
        /// </summary>
        /// <param name="baseline"></param>
        /// <param name="alignee"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public LcmsAlignmentData AlignFeatures(List<UMCLight> baseline, List<UMCLight> alignee, LcmsAlignmentOptions options)
        {
            var alignmentProcessor = new LcmsAlignmentProcessor
            {
                Options = options
            };

            var filteredFeatures = FilterFeaturesByAbundance(baseline, options) as List<UMCLight>;

            alignmentProcessor.SetReferenceDatasetFeatures(filteredFeatures);

            LcmsAlignmentData data = AlignFeatures(alignmentProcessor, alignee, options);

            int minScan = int.MaxValue;
            int maxScan = int.MinValue;

            foreach (var feature in baseline)
            {
                minScan = Math.Min(minScan, feature.Scan);
                maxScan = Math.Max(maxScan, feature.Scan);
            }

            data.MaxMtdbnet = maxScan;
            data.MinMtdbnet = minScan;

            return data;
        }

        LcmsAlignmentData AlignFeatures(LcmsAlignmentProcessor processor, List<UMCLight> features, LcmsAlignmentOptions options)
        {
            var alignmentFunctions = new List<LCMSAlignmentFunction>();
            var netErrorHistograms = new List<double[,]>();
            var massErrorHistograms = new List<double[,]>();
            var driftErrorHistograms = new List<double[,]>();
            var alignmentData = new List<LcmsAlignmentData>();
            var heatScores = new List<double[,]>();
            var xIntervals = new List<double[]>();
            var yIntervals = new List<double[]>();

            double minMtdbNet;
            double maxMtdbNet;
            processor.GetReferenceNetRange(out minMtdbNet, out maxMtdbNet);

            var filteredFeatures = FilterFeaturesByAbundance(features, options);
            var originalFeatures = new List<UMCLight>();
            var transformedFeatures = new List<UMCLight>();
            var map = new Dictionary<int, UMCLight>();

            foreach (var feature in features)
            {
                transformedFeatures.Add(feature);
                map.Add(feature.ID, feature);
            }

            foreach (var filtered in filteredFeatures)
            {
                originalFeatures.Add(filtered);
            }

            var minScanBaseline = int.MaxValue;
            var maxScanBaseline = int.MinValue;

            var totalBoundaries = 1;
            if (options.AlignSplitMZs)
            {
                totalBoundaries = 2;
            }

            for (var i = 0; i < totalBoundaries; i++)
            {
                // Set features
                processor.SetAligneeDatasetFeatures(originalFeatures, options.MzBoundaries[i]);
                // Find alignment
                processor.PerformAlignmentToMsFeatures();
                // Extract alignment function
                var alignmentFunction = processor.GetAlignmentFunction();
                alignmentFunctions.Add(alignmentFunction);
                // Correct the features
                processor.ApplyNetMassFunctionToAligneeDatasetFeatures(ref transformedFeatures);
                // Find the min and max scan for the data
                var tempMinScan = int.MaxValue;
                var tempMaxScan = int.MinValue;

                foreach (var feature in transformedFeatures)
                {
                    tempMaxScan = Math.Max(tempMaxScan, feature.Scan);
                    tempMinScan = Math.Min(tempMinScan, feature.Scan);

                    var featureId = feature.ID;
                    var isInMap = map.ContainsKey(featureId);
                    if (!isInMap) continue;

                    map[featureId].MassMonoisotopicAligned = feature.MassMonoisotopicAligned;
                    map[featureId].NETAligned = feature.NETAligned;
                    map[featureId].RetentionTime = feature.NETAligned;
                    map[featureId].ScanAligned = feature.ScanAligned;
                }
                minScanBaseline = Math.Min(minScanBaseline, tempMinScan);
                maxScanBaseline = Math.Max(maxScanBaseline, tempMaxScan);

                // Get the heat maps
                double[,] heatScore;
                double[] xInterval;
                double[] yInterval;

                processor.GetAlignmentHeatMap(out heatScore, out xInterval, out yInterval);

                xIntervals.Add(xInterval);
                yIntervals.Add(yInterval);
                heatScores.Add(heatScore);

                // Get the histograms
                double[,] massErrorHistogram;
                double[,] netErrorHistogram;
                double[,] driftErrorHistogram;

                processor.GetErrorHistograms(options.MassBinSize, options.NetBinSize, options.DriftTimeBinSize,
                                             out massErrorHistogram, out netErrorHistogram, out driftErrorHistogram);

                massErrorHistograms.Add(massErrorHistogram);
                netErrorHistograms.Add(netErrorHistogram);
                driftErrorHistograms.Add(driftErrorHistogram);

                // Get the residual data
                var residualData = processor.GetResidualData();

                var data = new LcmsAlignmentData
                {
                    MassErrorHistogram = massErrorHistogram,
                    DriftErrorHistogram = driftErrorHistogram,
                    NetErrorHistogram = netErrorHistogram,
                    AlignmentFunction = alignmentFunction,
                    HeatScores = heatScore,
                    MinScanBaseline = minScanBaseline,
                    MaxScanBaseline = maxScanBaseline,
                    NetIntercept = processor.NetIntercept,
                    NetRsquared = processor.NetRsquared,
                    NetSlope = processor.NetSlope,
                    ResidualData = residualData,
                    MassMean = processor.MassMu,
                    MassStandardDeviation = processor.MassStd,
                    NetMean = processor.NetMu,
                    NetStandardDeviation = processor.NetStd
                };

                if (options.AlignToMassTagDatabase)
                {
                    data.MinMtdbnet = minMtdbNet;
                    data.MaxMtdbnet = maxMtdbNet;
                }

                alignmentData.Add(data);
            }

            // Combine for split analysis
            var mergedData = new LcmsAlignmentData();
            var mergedAlignmentFunction = alignmentFunctions[alignmentFunctions.Count - 1];

            // Merge mass error histogram data
            var maxMassHistoLength = 0;
            var maxNetHistoLength = 0;
            var maxDriftHistoLength = 0;

            foreach (var t in alignmentData)
            {
                maxMassHistoLength = Math.Max(maxMassHistoLength, t.MassErrorHistogram.GetLength(0));
                maxNetHistoLength = Math.Max(maxNetHistoLength, t.NetErrorHistogram.GetLength(0));
                maxDriftHistoLength = Math.Max(maxDriftHistoLength, t.DriftErrorHistogram.GetLength(0));
            }

            var massErrorHistogramData = new double[maxMassHistoLength, 2];
            MergeHistogramData(massErrorHistogramData, alignmentData[0].MassErrorHistogram, false);

            // Residual Arrays are the same size, start process to count the size so we can merge everything back into one array
            var countMassResiduals = 0;
            var countNetResiduals = 0;

            for (var i = 0; i < alignmentData.Count; i++)
            {
                if (i > 0)
                {
                    MergeHistogramData(massErrorHistogramData, alignmentData[i].MassErrorHistogram, true);
                }

                countMassResiduals += alignmentData[i].ResidualData.Mz.Length;
                countNetResiduals += alignmentData[i].ResidualData.Scan.Length;
            }

            // Merge net Error histogram data and Mass residual data
            var netErrorHistogramData = new double[maxNetHistoLength, 2];
            MergeHistogramData(netErrorHistogramData, alignmentData[0].NetErrorHistogram, false);

            mergedData.ResidualData = new ResidualData
            {
                CustomNet = new double[countNetResiduals],
                LinearCustomNet = new double[countNetResiduals],
                LinearNet = new double[countNetResiduals],
                Scan = new double[countNetResiduals],
                MassError = new double[countMassResiduals],
                MassErrorCorrected = new double[countMassResiduals],
                Mz = new double[countMassResiduals],
                MzMassError = new double[countMassResiduals],
                MzMassErrorCorrected = new double[countMassResiduals]
            };

            var copyNetBlocks = 0;
            var copyMassBlocks = 0;

            for (int i = 0; i < alignmentData.Count; i++)
            {
                // Merge the residual data
                alignmentData[i].ResidualData.CustomNet.CopyTo(mergedData.ResidualData.CustomNet, copyNetBlocks);
                alignmentData[i].ResidualData.LinearCustomNet.CopyTo(mergedData.ResidualData.LinearCustomNet, copyNetBlocks);
                alignmentData[i].ResidualData.LinearNet.CopyTo(mergedData.ResidualData.LinearNet, copyNetBlocks);
                alignmentData[i].ResidualData.Scan.CopyTo(mergedData.ResidualData.Scan, copyNetBlocks);

                alignmentData[i].ResidualData.MassError.CopyTo(mergedData.ResidualData.MassError, copyMassBlocks);
                alignmentData[i].ResidualData.MassErrorCorrected.CopyTo(mergedData.ResidualData.MassErrorCorrected, copyMassBlocks);
                alignmentData[i].ResidualData.MzMassError.CopyTo(mergedData.ResidualData.MzMassError, copyMassBlocks);
                alignmentData[i].ResidualData.MzMassErrorCorrected.CopyTo(mergedData.ResidualData.MzMassErrorCorrected, copyMassBlocks);
                alignmentData[i].ResidualData.Mz.CopyTo(mergedData.ResidualData.Mz, copyMassBlocks);

                copyNetBlocks += alignmentData[i].ResidualData.Scan.Length;
                copyMassBlocks += alignmentData[i].ResidualData.Mz.Length;

                mergedData.MassMean = alignmentData[i].MassMean;
                mergedData.MassStandardDeviation = alignmentData[i].MassStandardDeviation;

                mergedData.NetIntercept = alignmentData[i].NetIntercept;
                mergedData.NetMean = alignmentData[i].NetMean;
                mergedData.NetStandardDeviation = alignmentData[i].NetStandardDeviation;
                mergedData.NetRsquared = alignmentData[i].NetRsquared;
                mergedData.NetSlope = alignmentData[i].NetSlope;

                if (i > 0)
                {
                    MergeHistogramData(netErrorHistogramData, alignmentData[i].NetErrorHistogram, true);
                }
            }

            // Get the heat scores one last time
            mergedData.HeatScores = alignmentData[alignmentData.Count - 1].HeatScores;
            mergedData.MassErrorHistogram = massErrorHistogramData;
            mergedData.NetErrorHistogram = netErrorHistogramData;

            mergedData.DriftErrorHistogram = driftErrorHistograms[0];
            mergedData.AlignmentFunction = mergedAlignmentFunction;

            return mergedData;
        }

        private static IEnumerable<UMCLight> FilterFeaturesByAbundance(List<UMCLight> features, LcmsAlignmentOptions options)
        {
            features.Sort((x, y) => x.AbundanceSum.CompareTo(y.AbundanceSum));

            var percent = 1 - (options.TopFeatureAbundancePercent / 100);
            var total = features.Count - Convert.ToInt32(features.Count * percent);
            var threshhold = features[Math.Min(features.Count - 1, Math.Max(0, total))].AbundanceSum;

            //Filters features below the threshold
            var filteredFeatures = features.FindAll(feature => feature.AbundanceSum >= threshhold);
            return filteredFeatures;
        }

        private static void MergeHistogramData(double[,] histogramDest,
                                        double[,] histogramSource,
                                        bool checkClosestBin)
        {
            for (var i = 0; i < histogramSource.GetLength(0) && i < histogramDest.GetLength(0); i++)
            {
                var bestIndex = 0;
                var massDiff = double.MaxValue;

                if (checkClosestBin == false)
                {
                    bestIndex = i;
                    histogramDest[i, 0] = histogramSource[bestIndex, 0];
                }
                else
                {
                    var length = Math.Min(histogramDest.GetLength(0), histogramSource.GetLength(0));

                    // Find the best mass item if the previous mass items are skewed or changed                    
                    for (int j = 0; j < length; j++)
                    {
                        double diff = Math.Abs(histogramDest[j, 0] - histogramSource[j, 0]);
                        if (diff < massDiff)
                        {
                            bestIndex = j;
                            massDiff = diff;
                        }
                    }
                }
                histogramDest[i, 1] += histogramSource[bestIndex, 1];
            }
        }
    }
}
