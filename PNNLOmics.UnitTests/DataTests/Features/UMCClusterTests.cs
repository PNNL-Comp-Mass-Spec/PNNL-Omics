using System;
using System.Collections.Generic;
using NUnit.Framework;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms;
using PNNLOmics.Algorithms.FeatureClustering;

namespace PNNLOmics.UnitTests.DataTests.Features
{
    /// <summary>
    /// Test class for the UMC Clusters.
    /// </summary>
    [TestFixture]    
    public class UMCClusterTests
    {
        /// <summary>
        /// Calculates statistics for a null umc list clusters.
        /// </summary>
        [Test]
        [TestCase(ExpectedException=typeof(NullReferenceException))]
        public void CalculateStatisticsTestNullUMC()
        {
            UMCCluster cluster      = new UMCCluster();
            cluster.UMCList         = null;
            cluster.CalculateStatistics(ClusterCentroidRepresentation.Median);
        }
        /// <summary>
        /// Calculates statistics for a empty UMC list.
        /// </summary>
        [Test]
        [TestCase(ExpectedException = typeof(Exception))]
        public void CalculateStatisticsTestEmptyUMC()
        {
            UMCCluster cluster = new UMCCluster();
            cluster.UMCList = new List<UMC>();
            cluster.CalculateStatistics(ClusterCentroidRepresentation.Median);
        }
        /// <summary>
        /// Calculates statistics for a empty UMC list.
        /// </summary>
        [Test]
        [TestCase(100, 100, 50, 2, 15000, ClusterCentroidRepresentation.Median)]
        [TestCase(100, 100, 50, 2, 15000, ClusterCentroidRepresentation.Mean)]
        public void CalculateStatisticsTestSingleUMC(   double  umcMass, 
                                                        double  umcNET,
                                                        float   umcDrifTime,
                                                        int     umcCharge, 
                                                        int     umcAbundance,
                                                        ClusterCentroidRepresentation representation)
        {
            UMCCluster cluster          = new UMCCluster();
            cluster.UMCList             = new List<UMC>();            

            UMC umc                     = new UMC();
            umc.MassMonoisotopicAligned = umcMass;
            umc.NETAligned              = umcNET;
            umc.DriftTime               = umcDrifTime;
            umc.ChargeState             = umcCharge;
            umc.Abundance               = umcAbundance;
            cluster.UMCList.Add(umc);
            cluster.CalculateStatistics(representation);

            Assert.AreEqual(cluster.MassMonoisotopic,   umc.MassMonoisotopicAligned);
            Assert.AreEqual(cluster.NET,                umc.NETAligned);
            Assert.AreEqual(cluster.DriftTime,          umc.DriftTime);
            Assert.AreEqual(cluster.ChargeState,        umc.ChargeState);            
        }


        /// <summary>
        /// Calculates statistics for a empty UMC list.
        /// </summary>
        [Test]
        [TestCase(100, 100, 50, 2, 15000, 2, 2, ClusterCentroidRepresentation.Median)]
        [TestCase(100, 100, 50, 2, 15000, 2, 2, ClusterCentroidRepresentation.Mean)]
        [TestCase(100, 100, 50, 2, 15000, 2, 3, ClusterCentroidRepresentation.Median)]
        [TestCase(100, 100, 50, 2, 15000, 2, 3, ClusterCentroidRepresentation.Mean)]
        [TestCase(100, 100, 50, 2, 15000, 2, 4, ClusterCentroidRepresentation.Median)]
        [TestCase(100, 100, 50, 2, 15000, 2, 4, ClusterCentroidRepresentation.Mean)]
        [TestCase(100, 100, 50, 2, 15000, 2, 100, ClusterCentroidRepresentation.Median)]
        [TestCase(100, 100, 50, 2, 15000, 2, 100, ClusterCentroidRepresentation.Mean)]
        public void CalculateStatisticsTestMultipleUMCs(      double  umcMass,
                                                        double  umcNET,
                                                        float   umcDrifTime,
                                                        int     umcCharge,
                                                        int     umcAbundance,
                                                        int     multiplier,
                                                        int     numUMCs,
                                                        ClusterCentroidRepresentation representation)
        {
            UMCCluster cluster  = new UMCCluster();
            cluster.UMCList     = new List<UMC>();

            int k                   = numUMCs / 2;
            double medianMass       = 0;
            double medianNET        = 0;
            float  medianDriftTime  = 0; 

            for (int i = 0; i < numUMCs; i++)
            {
                UMC umc                     = new UMC();
                umc.MassMonoisotopicAligned = umcMass      + multiplier * i;
                umc.NETAligned              = umcNET       + multiplier * i;
                umc.DriftTime               = umcDrifTime  + multiplier * i;
                umc.ChargeState             = umcCharge;
                umc.Abundance               = umcAbundance + multiplier * i;
                cluster.UMCList.Add(umc);

                if (representation == ClusterCentroidRepresentation.Mean)
                {
                    medianMass      += umc.MassMonoisotopicAligned;
                    medianNET       += umc.NETAligned;
                    medianDriftTime += umc.DriftTime;
                }
                // Odd
                else if (k == i && (numUMCs % 2 == 1))
                {
                    medianMass      = umc.MassMonoisotopicAligned;
                    medianNET       = umc.NETAligned;
                    medianDriftTime = umc.DriftTime;
                }
                // Even 
                else if ((numUMCs % 2) == 0)
                {    
                    // When we have an even number of features 
                    // We want to calculate the median as the average between
                    // the two median features (k, k + 1), where k is numUMCs / 2
                    // Remeber that we use k - 1 because i is zero indexed                    
                    if (k - 1 == i)
                    {
                        medianMass      = umc.MassMonoisotopicAligned;
                        medianNET       = umc.NETAligned;
                        medianDriftTime = umc.DriftTime;
                    }
                    else if (k == i)
                    {
                        medianMass      += umc.MassMonoisotopicAligned;
                        medianNET       += umc.NETAligned;
                        medianDriftTime += umc.DriftTime;
                        medianMass      /= 2;
                        medianNET       /= 2;
                        medianDriftTime /= 2;
                    }
                }
            }

            // We make sure that we calculate the mean correctly here.
            if (representation == ClusterCentroidRepresentation.Mean)
            {
                medianMass      /= numUMCs;
                medianNET       /= numUMCs;
                medianDriftTime /= numUMCs; 
            }

            cluster.CalculateStatistics(representation);

            Assert.AreEqual(cluster.MassMonoisotopic,   medianMass);
            Assert.AreEqual(cluster.NET,                medianNET);
            Assert.AreEqual(cluster.DriftTime,          medianDriftTime);
            Assert.AreEqual(cluster.ChargeState,        umcCharge);
        }        
    }
}
