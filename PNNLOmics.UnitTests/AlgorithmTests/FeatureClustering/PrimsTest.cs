﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PNNLOmics.Algorithms.Distance;
using PNNLOmics.Algorithms.FeatureClustering;
using PNNLOmics.Data.Features;

namespace PNNLOmics.UnitTests.AlgorithmTests.FeatureClustering
{

    [TestFixture]
    public class PrimsTest
    {

        private List<UMCLight> GetClusterData(string path)
        {
            var data = File.ReadLines(path).ToList();
            // Remove the header
            data.RemoveAt(0);

            var features = new List<UMCLight>();
            foreach(var line in data)
            {
                var lineData = line.Split(new[] {"\t"}, StringSplitOptions.RemoveEmptyEntries).ToList();

                var feature                 = new UMCLight();
                feature.ClusterID                = Convert.ToInt32(lineData[0]);                
                feature.GroupID                  = Convert.ToInt32(lineData[1]);
                feature.ID                       = Convert.ToInt32(lineData[2]);
                feature.MassMonoisotopicAligned  = Convert.ToDouble(lineData[3]);
                feature.RetentionTime            = Convert.ToDouble(lineData[4]);
                feature.DriftTime                = Convert.ToDouble(lineData[5]);
                feature.ChargeState              = Convert.ToInt32(lineData[6]);

                features.Add(feature);                    
            }
            return features;
        }
        [Test(Description = "Tests clusters that should have been split.")]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged.txt")]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-mergedSmall.txt")]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-toy.txt")]
        [TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-ideal.txt")]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged-nodelin.txt")]
        public void TestPrims(string path)
        {
            Console.WriteLine("Test: " + path);
            var features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            var cluster = new UMCClusterLight();
            cluster.ID = features[0].ID;
            features.ForEach(x => cluster.AddChildFeature(x));

            var maps = new Dictionary<int, UMCClusterLight>();

            var prims = new UMCPrimsClustering<UMCLight, UMCClusterLight>();
            prims.Parameters                        = new FeatureClusterParameters<UMCLight>();
            prims.Parameters.CentroidRepresentation = ClusterCentroidRepresentation.Mean;
            prims.Parameters.Tolerances             = new Algorithms.FeatureTolerances();

            var clusters = prims.Cluster(features);

            var counts = new Dictionary<int, Dictionary<int, int>>();
            var cid = 0;
            foreach (var clusterx in clusters)
            {
                clusterx.ID = cid++;
                foreach (var feature in clusterx.Features)
                {
                    if (!counts.ContainsKey(feature.GroupID))
                    {
                        counts.Add(feature.GroupID, new Dictionary<int, int>());
                    }
                    if (!counts[feature.GroupID].ContainsKey(feature.ID))
                    {
                        counts[feature.GroupID].Add(feature.ID, 0);
                    }

                    if (feature.ID == 51 || feature.ID == 37)
                    {
                        Console.WriteLine("Found it {0} cluster {1}", feature.ID, clusterx.ID);
                    }

                    counts[feature.GroupID][feature.ID]++;
                    Console.WriteLine("Found {0}", clusterx.ID);
                    if (counts[feature.GroupID][feature.ID] > 1)
                    {
                        Console.WriteLine("Duplicate!!!! cluster {0}  feature {1}", clusterx.ID, feature.ID);
                    }
                }
            }

            Console.WriteLine("Group\tFeature\tCount");
            foreach (var group in counts.Keys)
            {
                foreach (var id in counts[group].Keys)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", group, id, counts[group][id]);
                }
            }

            Console.WriteLine("Clusters = {0}", clusters.Count);
        }
        [Test(Description = "Tests clusters that should have been split.")]
        [TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-multiple-driftTime.txt", 4)]
        [TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-ideal.txt", 4)]
        [TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged.txt", 4)]
        [TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged-nodelin.txt", 4)]
        [TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-smallMerged.txt", 4)]
        [TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged-small.txt", 4)]
        [TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged-small.txt", 4)]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-single-1500.txt", 4)]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-single-1500-two.txt", 4)]
        public void TestPrimsWeighted(string path, double sigma)        
        {

            sigma = 1;

            Console.WriteLine();
            Console.WriteLine("Tests: " + path);
            Console.WriteLine("Sigma Cutoff: {0}", sigma);
            var features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            var cluster = new UMCClusterLight();
            cluster.ID = features[0].ID;
            features.ForEach(x => cluster.AddChildFeature(x));

            var maps = new Dictionary<int, UMCClusterLight>();


            var prims = new UMCPrimsClustering<UMCLight, UMCClusterLight>(sigma);
            prims.Parameters                        = new FeatureClusterParameters<UMCLight>();
            prims.Parameters.CentroidRepresentation = ClusterCentroidRepresentation.Mean;
            prims.Parameters.Tolerances             = new Algorithms.FeatureTolerances();
            prims.Parameters.OnlyClusterSameChargeStates = false;
            prims.Parameters.Tolerances.DriftTime       = .3;
            prims.Parameters.Tolerances.Mass            = 15;
            prims.Parameters.Tolerances.RetentionTime   = .02;
            prims.DumpLinearRelationship                = true;

            var distance = new WeightedEuclideanDistance<UMCLight>();
            prims.Parameters.DistanceFunction   = distance.EuclideanDistance;
            var clusters      = prims.Cluster(features);


            Console.WriteLine();
            Console.WriteLine("Clusters = {0}", clusters.Count);
            var id = 1;
            foreach (var testCluster in clusters)
            {
                testCluster.CalculateStatistics(ClusterCentroidRepresentation.Mean);


                var distances = new List<double>();
                testCluster.ID = id++;
                foreach (var feature in testCluster.Features)
                {
                    Console.WriteLine("{0},{1},{2},{3}",
                                                                feature.RetentionTime,                                                
                                                                feature.MassMonoisotopicAligned,
                                                                feature.DriftTime,
                                                                testCluster.ID);

                    var newDistance = distance.EuclideanDistance(feature, testCluster);
                    distances.Add(newDistance);
                }
                //Console.WriteLine();
                //Console.WriteLine("Distances");                
                //distances.ForEach(x => Console.WriteLine(x));
                //Console.WriteLine();                
            }
            Console.WriteLine();
            Console.WriteLine("Test Done:");
            Console.WriteLine();
        }                
    }
}
