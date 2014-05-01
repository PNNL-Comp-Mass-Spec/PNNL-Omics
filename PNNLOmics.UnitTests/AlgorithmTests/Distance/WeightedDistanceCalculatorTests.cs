﻿using System;
using NUnit.Framework;
using PNNLOmics.Algorithms.Distance;
using PNNLOmics.Data.Features;

namespace PNNLOmics.UnitTests.AlgorithmTests.Distances
{
	/// <summary>
	/// Test class for methods located in MahalanobisDistanceCalculator that contains various methods for calculating the mahalanobis distance.
	/// </summary>
	[TestFixture]
    public sealed class WeightedDistanceCalculatorTests
	{

        private UMCClusterLight CreateCluster(double mass, double net, double drift)
        {
            var cluster  = new UMCClusterLight();
            cluster.MassMonoisotopic = mass;
            cluster.Net              = net;
            cluster.RetentionTime    = net;
            cluster.DriftTime        = drift;
            return cluster;
        }

		[Test]
		public void TestDistances()
		{
            var dist = new WeightedEuclideanDistance<UMCClusterLight>();
            

            var clusterA = CreateCluster(500, .2, 27);
            var clusterB = CreateCluster(500, .2, 27);

            var N                = 50;
            var stepMass      = .5;
            var stepNET       = .001;
            var stepDrift     = .01;
            

            Console.WriteLine("Walk in drift time");            
            for (var i = 0; i < N; i++)
            {
                clusterB.DriftTime += stepDrift; 
                var distance    = dist.EuclideanDistance(clusterA, clusterB);
                Console.WriteLine("{0}, {1}, {3}, {2}", clusterB.DriftTime, clusterB.DriftTime, distance, clusterB.DriftTime - clusterA.DriftTime);
            }

            Console.WriteLine();
            Console.WriteLine("Walk in net ");
            clusterB.DriftTime = 27;

            for (var i = 0; i < N; i++)
            {
                clusterB.RetentionTime += stepNET;
                var distance = dist.EuclideanDistance(clusterA, clusterB);
                Console.WriteLine("{0}, {1}, {3}, {2}", clusterB.RetentionTime, clusterB.RetentionTime, distance, clusterB.RetentionTime - clusterA.RetentionTime);                
            }


            Console.WriteLine();
            Console.WriteLine("Walk in mass ");
            clusterB.RetentionTime = .2;
            for (var i = 0; i < N; i++)
            {

                var d = FeatureLight.ComputeDaDifferenceFromPPM(clusterA.MassMonoisotopic, stepMass * i);
                clusterB.MassMonoisotopic = d;
                var distance = dist.EuclideanDistance(clusterA, clusterB);
                Console.WriteLine("{0}, {1}, {3}, {2}", clusterB.MassMonoisotopic, 
                                                        clusterB.MassMonoisotopic, 
                                                        distance, 
                                                        FeatureLight.ComputeMassPPMDifference(clusterA.MassMonoisotopic, clusterB.MassMonoisotopic));
            }     
		}	
	}
}
