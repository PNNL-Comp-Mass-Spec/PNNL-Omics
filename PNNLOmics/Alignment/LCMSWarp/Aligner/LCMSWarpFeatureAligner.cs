﻿using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;
using Processor;
using System;
using System.Collections.Generic;

namespace Aligner
{
    public class LcmsWarpFeatureAligner
    {
        public AlignmentData AlignFeatures(List<MassTagLight> massTags, List<UMCLight> features,
            AlignmentOptions options, bool alignDriftTimes)
        {
            var processor = new AlignmentProcessor
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

            processor.Dispose();
            return data;
        }

        public AlignmentData AlignFeatures(List<UMCLight> baseline, List<UMCLight> alignee, AlignmentOptions options)
        {
            AlignmentData blah = new AlignmentData();

            var alignmentProcessor = new AlignmentProcessor
            {
                Options = options
            };

            var filteredFeatures = FilterFeaturesByAbundance(baseline, options) as List<UMCLight>;

            alignmentProcessor.SetReferenceDatasetFeatures(filteredFeatures);

            AlignmentData data = AlignFeatures(alignmentProcessor, alignee, options);

            int minScan = int.MaxValue;
            int maxScan = int.MinValue;

            foreach (var feature in baseline)
            {
                minScan = Math.Min(minScan, feature.Scan);
                maxScan = Math.Max(maxScan, feature.Scan);
            }

            data.maxMTDBNET = maxScan;
            data.minMTDBNET = minScan;

            alignmentProcessor.Dispose();

            return data;
        }

        public AlignmentData AlignFeatures(AlignmentProcessor processor, List<UMCLight> features, AlignmentOptions options)
        {
            var alignmentFunctions = new List<AlignmentFunction>();
            var netErrorHistograms = new List<double[,]>();
            var massErrorHistograms = new List<double[,]>();
            var driftErrorHistograms = new List<double[,]>();
            var alignmentData = new List<AlignmentData>();
            var heatScores = new List<double[,]>();
            var xIntervals = new List<double[]>();
            var yIntervals = new List<double[]>();

            var minMTDBNet = 0.0;
            var maxMTDBNet = 1.0;
            processor.GetReferenceNetRange(out minMTDBNet, out maxMTDBNet);

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
                var heatScore = new double[1, 1];
                var xInterval = new double[1];
                var yInterval = new double[1];

                processor.GetAlignmentHeatMap(out heatScore, out xInterval, out yInterval);

                xIntervals.Add(xInterval);
                yIntervals.Add(yInterval);
                heatScores.Add(heatScore);

                // Get the histograms
                var massErrorHistogram = new double[1, 1];
                var netErrorHistogram = new double[1, 1];
                var driftErrorHistogram = new double[1, 1];

                processor.GetErrorHistograms(options.MassBinSize, options.NETBinSize, options.DriftTimeBinSize,
                                             out massErrorHistogram, out netErrorHistogram, out driftErrorHistogram);

                massErrorHistograms.Add(massErrorHistogram);
                netErrorHistograms.Add(netErrorHistogram);
                driftErrorHistograms.Add(driftErrorHistogram);

                // Get the residual data
                var residualData = processor.GetResidualData();

                var data = new AlignmentData
                {
                    massErrorHistogram = massErrorHistogram,
                    driftErrorHistogram = driftErrorHistogram,
                    netErrorHistogram = netErrorHistogram,
                    alignmentFunction = alignmentFunction,
                    heatScores = heatScore,
                    minScanBaseline = minScanBaseline,
                    maxScanBaseline = maxScanBaseline,
                    NETIntercept = processor.NetIntercept,
                    NETRsquared = processor.NetRsquared,
                    NETSlope = processor.NetSlope,
                    ResidualData = residualData,
                    MassMean = processor.MassMu,
                    MassStandardDeviation = processor.MassStd,
                    NETMean = processor.NetMu,
                    NETStandardDeviation = processor.NetStd
                };

                if (options.AlignToMassTagDatabase)
                {
                    data.minMTDBNET = minMTDBNet;
                    data.maxMTDBNET = maxMTDBNet;
                }

                alignmentData.Add(data);
            }

            // Combine for split analysis
            var mergedData = new AlignmentData();
            var mergedAlignmentFunction = alignmentFunctions[alignmentFunctions.Count - 1];

            // Merge mass error histogram data
            var maxMassHistoLength = 0;
            var maxNetHistoLength = 0;
            var maxDriftHistoLength = 0;

            foreach (var t in alignmentData)
            {
                maxMassHistoLength = Math.Max(maxMassHistoLength, t.massErrorHistogram.GetLength(0));
                maxNetHistoLength = Math.Max(maxNetHistoLength, t.netErrorHistogram.GetLength(0));
                maxDriftHistoLength = Math.Max(maxDriftHistoLength, t.driftErrorHistogram.GetLength(0));
            }

            var massErrorHistogramData = new double[maxMassHistoLength, 2];
            MergeHistogramData(massErrorHistogramData, alignmentData[0].massErrorHistogram, false);

            // Residual Arrays are the same size, start process to count the size so we can merge everything back into one array
            var countMassResiduals = 0;
            var countNetResiduals = 0;

            for (var i = 0; i < alignmentData.Count; i++)
            {
                if (i > 0)
                {
                    MergeHistogramData(massErrorHistogramData, alignmentData[i].massErrorHistogram, true);
                }

                countMassResiduals += alignmentData[i].ResidualData.Mz.Length;
                countNetResiduals += alignmentData[i].ResidualData.Scan.Length;
            }

            // Merge net Error histogram data and Mass residual data
            var netErrorHistogramData = new double[maxNetHistoLength, 2];
            MergeHistogramData(netErrorHistogramData, alignmentData[0].netErrorHistogram, false);

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

                mergedData.NETIntercept = alignmentData[i].NETIntercept;
                mergedData.NETMean = alignmentData[i].NETMean;
                mergedData.NETStandardDeviation = alignmentData[i].NETStandardDeviation;
                mergedData.NETRsquared = alignmentData[i].NETRsquared;
                mergedData.NETSlope = alignmentData[i].NETSlope;

                if (i > 0)
                {
                    MergeHistogramData(netErrorHistogramData, alignmentData[i].netErrorHistogram, true);
                }
            }

            // Get the heat scores one last time
            mergedData.heatScores = alignmentData[alignmentData.Count - 1].heatScores;
            mergedData.massErrorHistogram = massErrorHistogramData;
            mergedData.netErrorHistogram = netErrorHistogramData;

            mergedData.driftErrorHistogram = driftErrorHistograms[0];
            mergedData.alignmentFunction = mergedAlignmentFunction;

            processor.Dispose();

            return mergedData;
        }

        private static IEnumerable<UMCLight> FilterFeaturesByAbundance(List<UMCLight> features, AlignmentOptions options)
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
