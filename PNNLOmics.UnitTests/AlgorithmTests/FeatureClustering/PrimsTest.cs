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
            prims.Parameters                        = new FeatureClusterParameters<UMCLight>();
            prims.Parameters.CentroidRepresentation = ClusterCentroidRepresentation.Mean;
            prims.Parameters.Tolerances             = new Algorithms.FeatureTolerances();

            List<UMCClusterLight> clusters = prims.Cluster(features);

            Dictionary<int, Dictionary<int, int>> counts = new Dictionary<int, Dictionary<int, int>>();
            int cid = 0;
            foreach (UMCClusterLight clusterx in clusters)
            {
                clusterx.ID = cid++;
                foreach (UMCLight feature in clusterx.Features)
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
            foreach (int group in counts.Keys)
            {
                foreach (int id in counts[group].Keys)
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
            List<UMCLight> features = GetClusterData(path);

            Assert.IsNotEmpty(features);

            UMCClusterLight cluster = new UMCClusterLight();
            cluster.ID = features[0].ID;
            features.ForEach(x => cluster.AddChildFeature(x));

            Dictionary<int, UMCCluster> maps = new Dictionary<int, UMCCluster>();


            UMCPrimsClustering<UMCLight, UMCClusterLight> prims = new UMCPrimsClustering<UMCLight, UMCClusterLight>(sigma);
            prims.Parameters                        = new FeatureClusterParameters<UMCLight>();
            prims.Parameters.CentroidRepresentation = ClusterCentroidRepresentation.Mean;
            prims.Parameters.Tolerances             = new Algorithms.FeatureTolerances();
            prims.Parameters.OnlyClusterSameChargeStates = false;
            prims.Parameters.Tolerances.DriftTime       = .3;
            prims.Parameters.Tolerances.Mass            = 15;
            prims.Parameters.Tolerances.RetentionTime   = .02;
            prims.DumpLinearRelationship                = true;

            WeightedEuclideanDistance<UMCLight> distance = new WeightedEuclideanDistance<UMCLight>();
            prims.Parameters.DistanceFunction   = distance.EuclideanDistance;
            List<UMCClusterLight> clusters      = prims.Cluster(features);


            Console.WriteLine();
            Console.WriteLine("Clusters = {0}", clusters.Count);
            int id = 1;
            foreach (UMCClusterLight testCluster in clusters)
            {
                testCluster.CalculateStatistics(ClusterCentroidRepresentation.Mean);


                List<double> distances = new List<double>();
                testCluster.ID = id++;
                foreach (UMCLight feature in testCluster.Features)
                {
                    Console.WriteLine("{0},{1},{2},{3}",
                                                                feature.RetentionTime,                                                
                                                                feature.MassMonoisotopicAligned,
                                                                feature.DriftTime,
                                                                testCluster.ID);

                    double newDistance = distance.EuclideanDistance(feature, testCluster as FeatureLight);
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
