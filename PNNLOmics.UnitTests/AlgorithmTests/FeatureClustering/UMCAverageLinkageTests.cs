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
    public class UMCAverageLinkageTest
    {

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
        [Test(Description = "Tests clusters that should have been split.")]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged.txt")]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-mergedSmall.txt")]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-toy.txt")]
        [TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-ideal.txt")]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged-nodelin.txt")]
        public void TestAverageLinkage(string path)
        {
            Console.WriteLine("Test: " + path);
            List<UMCLight> features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            UMCClusterLight cluster = new UMCClusterLight();
            cluster.ID = features[0].ID;
            features.ForEach(x => cluster.AddChildFeature(x));

            Dictionary<int, UMCCluster> maps = new Dictionary<int, UMCCluster>();

            UMCAverageLinkageClusterer<UMCLight, UMCClusterLight> average = new UMCAverageLinkageClusterer<UMCLight, UMCClusterLight>();
            average.Parameters = new FeatureClusterParameters<UMCLight>();
            average.Parameters.CentroidRepresentation = ClusterCentroidRepresentation.Mean;
            average.Parameters.Tolerances = new Algorithms.FeatureTolerances();

            List<UMCClusterLight> clusters = average.Cluster(features);            
        }

        private double WeightedDistance(UMCLight x, UMCLight y)
        {
            double test = 0;

            return test;
        }

        //[Test(Description = "Tests clusters that should have been split.")]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-single-smallSpread.txt", .09)]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-ideal.txt", .09)]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged.txt", .09)]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged.txt", .01)]
        [TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged.txt", .0000001, .0001)]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged-nodelin.txt")]
        public void TestWeightedAverageLinkage(string path, double weight, double driftWeight)
        {
            Console.WriteLine("Test: " + path);
            List<UMCLight> features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            UMCClusterLight cluster = new UMCClusterLight();
            cluster.ID = features[0].ID;
            features.ForEach(x => cluster.AddChildFeature(x));

            Dictionary<int, UMCCluster> maps = new Dictionary<int, UMCCluster>();

            UMCAverageLinkageClusterer<UMCLight, UMCClusterLight> average = new UMCAverageLinkageClusterer<UMCLight, UMCClusterLight>();
            average.Parameters                          = new FeatureClusterParameters<UMCLight>();
            average.Parameters.CentroidRepresentation   = ClusterCentroidRepresentation.Mean;
            average.Parameters.Tolerances               = new Algorithms.FeatureTolerances();
            
            WeightedEuclideanDistance<UMCLight> distance = new WeightedEuclideanDistance<UMCLight>();
            distance.MassWeight                 = weight;
            distance.DriftWeight                = driftWeight;
            average.Parameters.DistanceFunction = distance.EuclideanDistance;
            List<UMCClusterLight> clusters      = average.Cluster(features);

            Console.WriteLine("dataset\tfeature\tmass\tnet\tdrift");
            foreach (UMCClusterLight newCluster in clusters)
            {
                foreach (UMCLight feature in newCluster.Features)
                {
                    Console.WriteLine("{0},{1},{2},{3},{4}",feature.GroupID, 
                                                            feature.ID,
                                                            feature.RetentionTime, 
                                                            feature.MassMonoisotopicAligned,
                                                            feature.DriftTime);

                }
            }
        }                
    }
}
