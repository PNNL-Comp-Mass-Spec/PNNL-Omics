using System;
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
    public class UMCClusterReprocessingTests
    {

        /// <summary>
        /// Reads cluster data from the path provided.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
                feature.ClusterId                = Convert.ToInt32(lineData[0]);                
                feature.GroupId                  = Convert.ToInt32(lineData[1]);
                feature.Id                       = Convert.ToInt32(lineData[2]);
                feature.MassMonoisotopicAligned  = Convert.ToDouble(lineData[3]);
                feature.RetentionTime            = Convert.ToDouble(lineData[4]);
                feature.DriftTime                = Convert.ToDouble(lineData[5]);
                feature.ChargeState              = Convert.ToInt32(lineData[6]);

                features.Add(feature);                    
            }
            return features;
        }        
        /// <summary>
        /// 
        /// </summary>
        [Test(Description = "Tests clusters that should have been split.")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-merged.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-ideal.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-merged-nodelin.txt")]
        public void TestDatasets(string path)
        {
            Console.WriteLine("Test: " + path);
            var features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            var cluster = new UMCClusterLight();
            cluster.Id              = features[0].Id;
            features.ForEach(x => cluster.AddChildFeature(x));

            var maps = new Dictionary<int, UMCClusterLight>();
            

            // Map the features
            var mapFeatures = new Dictionary<int, List<UMCLight>>();
            foreach (var feature in features)
            {
                if (!mapFeatures.ContainsKey(feature.GroupId))
                {
                    mapFeatures.Add(feature.GroupId, new List<UMCLight>());
                }
                mapFeatures[feature.GroupId].Add(feature);
            }
            //// Then sort
            //foreach (int x in mapFeatures.Keys)
            //{
            //    mapFeatures[x].Sort(delegate(UMCLight x, UMCLight y)
            //    {
            //        return x.RetentionTime.CompareTo(y.RetentionTime);
            //    });

            //    int count = 0;
                
            //}
            Console.WriteLine("Cluster\tMass\tNET");
            Console.WriteLine("{0}\t{1}\t{2}\t", cluster.Id, cluster.MassStandardDeviation, cluster.NetStandardDeviation);
            Console.WriteLine();

            var distance = new EuclideanDistanceMetric<FeatureLight>();

            //features.ForEach(x => Console.WriteLine(distance.EuclideanDistance(x, cluster)));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test(Description = "Tests clusters that should have been split.")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-merged.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-ideal.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-merged-nodelin.txt")]
        public void TestTwoClusters(string path)
        {
            Console.WriteLine("Test: " + path);
            var features = GetClusterData(path);
            
            Assert.IsNotEmpty(features);

            var cluster = new UMCClusterLight();
            cluster.Id              = features[0].Id;
            features.ForEach(x => cluster.AddChildFeature(x));


            cluster.CalculateStatistics(ClusterCentroidRepresentation.Median);
            Console.WriteLine("Cluster\tMass\tNET");
            Console.WriteLine("{0}\t{1}\t{2}\t", cluster.Id, cluster.MassStandardDeviation, cluster.NetStandardDeviation);
            Console.WriteLine();

            var distance = new EuclideanDistanceMetric<FeatureLight>();
            
            features.ForEach(x => Console.WriteLine(distance.EuclideanDistance(x, cluster)));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test(Description = "Tests clusters that should have been split.")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-merged.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-ideal.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-single-smallSpread.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-single-large.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-single-large2.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-merged-nodelin.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-merged-Net.txt")]
        public void TestReprocessing(string path)
        {
            Console.WriteLine("Test: " + path);
            var features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            var cluster = new UMCClusterLight();
            cluster.Id = features[0].Id;
            features.ForEach(x => cluster.AddChildFeature(x));

            cluster.CalculateStatistics(ClusterCentroidRepresentation.Median);
            Console.WriteLine("Cluster\tMass\tNET");
            Console.WriteLine("{0}\t{1}\t{2}\t", cluster.Id, cluster.MassStandardDeviation, cluster.NetStandardDeviation);
            Console.WriteLine();

            IClusterReprocessor<UMCLight, UMCClusterLight> reprocessor = new MedianSplitReprocessor<UMCLight, UMCClusterLight>();

            reprocessor.ProcessClusters(new List<UMCClusterLight> { cluster });
        }


        [Test(Description = "Tests clusters that should have been split.")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-merged.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-ideal.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-merged-nodelin.txt")]
        public void TestPairwise(string path)
        {
            Console.WriteLine("Test: " + path);
            var features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            var cluster = new UMCClusterLight();
            cluster.Id = features[0].Id;
            features.ForEach(x => cluster.AddChildFeature(x));

            
            var distance = new EuclideanDistanceMetric<FeatureLight>();

            for (var i = 0; i < features.Count; i++)
            {
                var  featureX = features[i];
                for (var j = 0; j < features.Count; j++)
                {

                    if (i != j)
                    {
                        var featureY = features[j];
                       // Console.WriteLine(distance.EuclideanDistance(featureX, featureY));
                    }
                }                
            }
        }
    }
}
