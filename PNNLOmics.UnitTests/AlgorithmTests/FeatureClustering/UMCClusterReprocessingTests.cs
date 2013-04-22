using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PNNLOmics.Data.Features;
using System.IO;
using PNNLOmics.Algorithms.Distance;
using PNNLOmics.Algorithms.FeatureClustering;

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
            List<string> data = File.ReadLines(path).ToList();
            // Remove the header
            data.RemoveAt(0);

            List<UMCLight> features = new List<UMCLight>();
            foreach(string line in data)
            {
                List<string> lineData = line.Split(new string [] {"\t"}, StringSplitOptions.RemoveEmptyEntries).ToList();

                UMCLight feature                 = new UMCLight();
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
            List<UMCLight> features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            UMCClusterLight cluster = new UMCClusterLight();
            cluster.ID              = features[0].ID;
            features.ForEach(x => cluster.AddChildFeature(x));

            Dictionary<int, UMCCluster> maps = new Dictionary<int, UMCCluster>();
            

            // Map the features
            Dictionary<int, List<UMCLight>> mapFeatures = new Dictionary<int, List<UMCLight>>();
            foreach (UMCLight feature in features)
            {
                if (!mapFeatures.ContainsKey(feature.GroupID))
                {
                    mapFeatures.Add(feature.GroupID, new List<UMCLight>());
                }
                mapFeatures[feature.GroupID].Add(feature);
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
            Console.WriteLine("{0}\t{1}\t{2}\t", cluster.ID, cluster.MassStandardDeviation, cluster.NetStandardDeviation);
            Console.WriteLine();

            EuclideanDistanceMetric<FeatureLight> distance = new EuclideanDistanceMetric<FeatureLight>();

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
            List<UMCLight> features = GetClusterData(path);
            
            Assert.IsNotEmpty(features);

            UMCClusterLight cluster = new UMCClusterLight();
            cluster.ID              = features[0].ID;
            features.ForEach(x => cluster.AddChildFeature(x));


            cluster.CalculateStatistics(ClusterCentroidRepresentation.Median);
            Console.WriteLine("Cluster\tMass\tNET");
            Console.WriteLine("{0}\t{1}\t{2}\t", cluster.ID, cluster.MassStandardDeviation, cluster.NetStandardDeviation);
            Console.WriteLine();

            EuclideanDistanceMetric<FeatureLight> distance = new EuclideanDistanceMetric<FeatureLight>();
            
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
            List<UMCLight> features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            UMCClusterLight cluster = new UMCClusterLight();
            cluster.ID = features[0].ID;
            features.ForEach(x => cluster.AddChildFeature(x));

            cluster.CalculateStatistics(ClusterCentroidRepresentation.Median);
            Console.WriteLine("Cluster\tMass\tNET");
            Console.WriteLine("{0}\t{1}\t{2}\t", cluster.ID, cluster.MassStandardDeviation, cluster.NetStandardDeviation);
            Console.WriteLine();

            IClusterReprocessor<UMCLight, UMCClusterLight> reprocessor = new MedianSplitReprocessor<UMCLight, UMCClusterLight>();

            reprocessor.ProcessClusters(new List<UMCClusterLight>() { cluster });
        }


        [Test(Description = "Tests clusters that should have been split.")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-merged.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-ideal.txt")]
        [TestCase(@"..\..\...\TestFiles\ClusterData\clusterData-merged-nodelin.txt")]
        public void TestPairwise(string path)
        {
            Console.WriteLine("Test: " + path);
            List<UMCLight> features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            UMCClusterLight cluster = new UMCClusterLight();
            cluster.ID = features[0].ID;
            features.ForEach(x => cluster.AddChildFeature(x));

            
            EuclideanDistanceMetric<FeatureLight> distance = new EuclideanDistanceMetric<FeatureLight>();

            for (int i = 0; i < features.Count; i++)
            {
                UMCLight  featureX = features[i];
                for (int j = 0; j < features.Count; j++)
                {

                    if (i != j)
                    {
                        UMCLight featureY = features[j];
                       // Console.WriteLine(distance.EuclideanDistance(featureX, featureY));
                    }
                }                
            }
        }
    }
}
