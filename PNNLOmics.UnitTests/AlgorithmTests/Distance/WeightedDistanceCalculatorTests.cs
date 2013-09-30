using System;
using PNNLOmics.Algorithms.Distance;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using PNNLOmics.Data.Features;

namespace PNNLOmics.UnitTests.AlgorithmTests.Distances
{
	/// <summary>
	/// Test class for methods located in MahalanobisDistanceCalculator that contains various methods for calculating the mahalanobis distance.
	/// </summary>
	[TestFixture]
    public class WeightedDistanceCalculatorTests
	{

        private UMCClusterLight CreateCluster(double mass, double net, double drift)
        {
            UMCClusterLight cluster  = new UMCClusterLight();
            cluster.MassMonoisotopic = mass;
            cluster.NET              = net;
            cluster.RetentionTime    = net;
            cluster.DriftTime        = drift;
            return cluster;
        }

		[Test]
		public void TestDistances()
		{
            WeightedEuclideanDistance<UMCClusterLight> dist = new WeightedEuclideanDistance<UMCClusterLight>();
            

            UMCClusterLight clusterA = CreateCluster(500, .2, 27);
            UMCClusterLight clusterB = CreateCluster(500, .2, 27);

            int N                = 50;
            double stepMass      = .5;
            double stepNET       = .001;
            double stepDrift     = .01;
            

            Console.WriteLine("Walk in drift time");            
            for (int i = 0; i < N; i++)
            {
                clusterB.DriftTime += stepDrift; 
                double distance    = dist.EuclideanDistance(clusterA, clusterB);
                Console.WriteLine("{0}, {1}, {3}, {2}", clusterB.DriftTime, clusterB.DriftTime, distance, clusterB.DriftTime - clusterA.DriftTime);
            }

            Console.WriteLine();
            Console.WriteLine("Walk in net ");
            clusterB.DriftTime = 27;

            for (int i = 0; i < N; i++)
            {
                clusterB.RetentionTime += stepNET;
                double distance = dist.EuclideanDistance(clusterA, clusterB);
                Console.WriteLine("{0}, {1}, {3}, {2}", clusterB.RetentionTime, clusterB.RetentionTime, distance, clusterB.RetentionTime - clusterA.RetentionTime);                
            }


            Console.WriteLine();
            Console.WriteLine("Walk in mass ");
            clusterB.RetentionTime = .2;
            for (int i = 0; i < N; i++)
            {

                double d = Feature.ComputeDaDifferenceFromPPM(clusterA.MassMonoisotopic, stepMass * i);
                clusterB.MassMonoisotopic = d;
                double distance = dist.EuclideanDistance(clusterA, clusterB);
                Console.WriteLine("{0}, {1}, {3}, {2}", clusterB.MassMonoisotopic, 
                                                        clusterB.MassMonoisotopic, 
                                                        distance, 
                                                        Feature.ComputeMassPPMDifference(clusterA.MassMonoisotopic, clusterB.MassMonoisotopic));
            }     
		}	
	}
}
