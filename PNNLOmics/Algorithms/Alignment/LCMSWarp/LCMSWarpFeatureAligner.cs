﻿using System;
using System.Collections.Generic;
using System.Linq;
using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;

namespace PNNLOmics.Algorithms.Alignment.LCMSWarp
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
        /// <returns></returns>
        public LcmsWarpAlignmentData AlignFeatures(List<MassTagLight> massTags, List<UMCLight> features,
            LcmsWarpAlignmentOptions options)
        {
            var processor = new LcmsWarpAlignmentProcessor
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
        public LcmsWarpAlignmentData AlignFeatures(List<UMCLight> baseline, List<UMCLight> alignee, LcmsWarpAlignmentOptions options)
        {
            var alignmentProcessor = new LcmsWarpAlignmentProcessor
            {
                Options = options
            };

            var filteredFeatures = FilterFeaturesByAbundance(baseline, options) as List<UMCLight>;

            alignmentProcessor.SetReferenceDatasetFeatures(filteredFeatures);

            LcmsWarpAlignmentData data = AlignFeatures(alignmentProcessor, alignee, options);

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

        LcmsWarpAlignmentData AlignFeatures(LcmsWarpAlignmentProcessor processor, List<UMCLight> features, LcmsWarpAlignmentOptions options)
        {
            var alignmentFunctions = new List<LcmsWarpAlignmentFunction>();
            var netErrorHistograms = new List<double[,]>();
            var massErrorHistograms = new List<double[,]>();
            var driftErrorHistograms = new List<double[,]>();
            var heatScores = new List<double[,]>();
            var xIntervals = new List<double[]>();
            var yIntervals = new List<double[]>();

            double minMtdbNet;
            double maxMtdbNet;
            processor.GetReferenceNetRange(out minMtdbNet, out maxMtdbNet);

            var filteredFeatures = FilterFeaturesByAbundance(features, options);
            var transformedFeatures = new List<UMCLight>();
            var map = new Dictionary<int, UMCLight>();

            foreach (var feature in features)
            {
                transformedFeatures.Add(feature);
                map.Add(feature.ID, feature);
            }

            var originalFeatures = filteredFeatures.ToList();

            var minScanBaseline = int.MaxValue;
            var maxScanBaseline = int.MinValue;

            // Set features
            processor.SetAligneeDatasetFeatures(originalFeatures);
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

            var data = new LcmsWarpAlignmentData
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

            return data;

        }

        private static IEnumerable<UMCLight> FilterFeaturesByAbundance(List<UMCLight> features, LcmsWarpAlignmentOptions options)
        {
            features.Sort((x, y) => x.AbundanceSum.CompareTo(y.AbundanceSum));

            var percent = 1 - (options.TopFeatureAbundancePercent / 100);
            var total = features.Count - Convert.ToInt32(features.Count * percent);
            var threshhold = features[Math.Min(features.Count - 1, Math.Max(0, total))].AbundanceSum;

            //Filters features below the threshold
            var filteredFeatures = features.FindAll(feature => feature.AbundanceSum >= threshhold);
            return filteredFeatures;
        }
    }
}