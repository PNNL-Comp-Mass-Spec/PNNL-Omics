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
    public class PrimsTest
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
        public void TestPrims(string path)
        {
            Console.WriteLine("Test: " + path);
            List<UMCLight> features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            UMCClusterLight cluster = new UMCClusterLight();
            cluster.ID = features[0].ID;
            features.ForEach(x => cluster.AddChildFeature(x));

            Dictionary<int, UMCCluster> maps = new Dictionary<int, UMCCluster>();

            UMCPrimsClustering<UMCLight, UMCClusterLight> prims = new UMCPrimsClustering<UMCLight, UMCClusterLight>();
            prims.Parameters = new FeatureClusterParameters<UMCLight>();
            prims.Parameters.CentroidRepresentation = ClusterCentroidRepresentation.Mean;
            prims.Parameters.Tolerances = new Algorithms.FeatureTolerances();

            List<UMCClusterLight> clusters = prims.Cluster(features);

            Console.WriteLine("Clusters = {0}", clusters.Count);
        }
        [Test(Description = "Tests clusters that should have been split.")]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-single-smallSpread.txt", .00000001, .0001)]
        [TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-ideal.txt", .00000001, .0001)]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged.txt", .09)]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged.txt", .01)]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged.txt", .00000001, .0001)]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-merged-nodelin.txt", .00000001, .0001)]
        //[TestCase(@"..\..\..\TestFiles\ClusterData\clusterData-smallMerged.txt", .00000001, .0001)]        
        public void TestPrimsWeighted(string path, double weight, double driftWeight)        
        {
            Console.WriteLine("Test: " + path);
            List<UMCLight> features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            UMCClusterLight cluster = new UMCClusterLight();
            cluster.ID = features[0].ID;
            features.ForEach(x => cluster.AddChildFeature(x));

            Dictionary<int, UMCCluster> maps = new Dictionary<int, UMCCluster>();

            UMCPrimsClustering<UMCLight, UMCClusterLight> prims = new UMCPrimsClustering<UMCLight, UMCClusterLight>();
            prims.Parameters = new FeatureClusterParameters<UMCLight>();
            prims.Parameters.CentroidRepresentation = ClusterCentroidRepresentation.Mean;
            prims.Parameters.Tolerances = new Algorithms.FeatureTolerances();


            WeightedEuclideanDistance<UMCLight> distance = new WeightedEuclideanDistance<UMCLight>();
            distance.MassWeight                 = weight;
            distance.DriftWeight                = driftWeight;            
            prims.Parameters.DistanceFunction   = distance.EuclideanDistance;
            List<UMCClusterLight> clusters      = prims.Cluster(features);

            Console.WriteLine("Clusters = {0}", clusters.Count);
            foreach (UMCClusterLight testCluster in clusters)
            {
                Console.WriteLine();
                Console.WriteLine("{0}", testCluster.ID);
                foreach (UMCLight feature in testCluster.Features)
                {
                    Console.WriteLine("{0},{1},,{2},{3},{4}",  feature.GroupID, 
                                                                feature.ID, 
                                                                feature.MassMonoisotopicAligned, 
                                                                feature.RetentionTime, 
                                                                feature.DriftTime);
                }                
            }

            Console.WriteLine("Test Done:");
            Console.WriteLine();
        }                
    }
}
