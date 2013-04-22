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
    public class UMCClusterLightTests
    {
        /// <summary>
        /// Calculates statistics for a null umc list clusters.
        /// </summary>
        [Test]
        [TestCase(ExpectedException=typeof(NullReferenceException))]
        public void CalculateStatisticsTestNullUMC()
        {
            UMCClusterLight cluster = new UMCClusterLight();
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
            UMCClusterLight cluster = new UMCClusterLight();
            cluster.UMCList         = new List<UMCLight>();
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
            UMCClusterLight cluster     = new UMCClusterLight();
            cluster.UMCList             = new List<UMCLight>();            

            UMCLight umc                    = new UMCLight();
            umc.MassMonoisotopicAligned            = umcMass;
            umc.RetentionTime               = umcNET;
            umc.DriftTime                   = umcDrifTime;
            umc.ChargeState                 = umcCharge;
            umc.Abundance                   = umcAbundance;
            cluster.UMCList.Add(umc);
            cluster.CalculateStatistics(representation);

            Assert.AreEqual(cluster.MassMonoisotopicAligned, umc.MassMonoisotopicAligned);
            Assert.AreEqual(cluster.RetentionTime,      umc.RetentionTime);
            Assert.AreEqual(cluster.DriftTime,          umc.DriftTime);
            Assert.AreEqual(cluster.ChargeState,        umc.ChargeState); 
            Assert.AreEqual(cluster.Score, 0);           
        }
        /// <summary>
        /// Calculates statistics for a empty UMC list.
        /// </summary>
        [Test]
        [TestCase(ClusterCentroidRepresentation.Median)]
        [TestCase(ClusterCentroidRepresentation.Mean)]
        public void CalculateStatisticsSame(ClusterCentroidRepresentation representation)
        {
            UMCClusterLight cluster = new UMCClusterLight();
            cluster.UMCList         = new List<UMCLight>();

            UMCLight umc            = new UMCLight();
            umc.MassMonoisotopicAligned = 100;
            umc.RetentionTime       = 100;
            umc.DriftTime           = 100;
            umc.ChargeState         = 2;
            umc.Abundance           = 100;
            cluster.UMCList.Add(umc);
            cluster.UMCList.Add(umc);
            
            cluster.CalculateStatistics(representation);
            Assert.AreEqual(cluster.Score, 0);
        }
        /// <summary>
        /// Calculates statistics for a empty UMC list.
        /// </summary>
        [Test]
        [TestCase(ClusterCentroidRepresentation.Median)]
        [TestCase(ClusterCentroidRepresentation.Mean)]
        public void CalculateStatisticsMultipleMass(ClusterCentroidRepresentation representation)
        {
            UMCClusterLight cluster = new UMCClusterLight();
            cluster.UMCList = new List<UMCLight>();

            UMCLight umc = new UMCLight();
            umc.MassMonoisotopicAligned = 100;
            umc.RetentionTime = 100;
            umc.DriftTime = 100;
            umc.ChargeState = 2;
            umc.Abundance = 100;
            cluster.UMCList.Add(umc);

            UMCLight umc2 = new UMCLight();
            umc2.MassMonoisotopicAligned = 200;
            umc2.RetentionTime = 100;
            umc2.DriftTime = 100;
            umc2.ChargeState = 2;
            umc2.Abundance = 100;
            cluster.UMCList.Add(umc2);

            cluster.CalculateStatistics(representation);
            Assert.AreEqual(cluster.MassMonoisotopic, 150);
        }
           
        /// <summary>
        /// Calculates statistics for a empty UMC list.
        /// </summary>
        [Test]
        [TestCase(ClusterCentroidRepresentation.Median)]
        [TestCase(ClusterCentroidRepresentation.Mean)]
        public void CalculateStatisticsMultipleNet(ClusterCentroidRepresentation representation)
        {
            UMCClusterLight cluster = new UMCClusterLight();
            cluster.UMCList         = new List<UMCLight>();

            UMCLight umc            = new UMCLight();
            umc.MassMonoisotopicAligned = 100;
            umc.RetentionTime       = 100;
            umc.DriftTime           = 100;
            umc.ChargeState         = 2;
            umc.Abundance           = 100;
            cluster.UMCList.Add(umc);
            
            UMCLight umc2 = new UMCLight();
            umc2.MassMonoisotopicAligned = 100;
            umc2.RetentionTime = 200;
            umc2.DriftTime = 100;
            umc2.ChargeState = 2;
            umc2.Abundance = 100;
            cluster.UMCList.Add(umc2);

            cluster.CalculateStatistics(representation);
            Assert.AreEqual(cluster.RetentionTime, 150);
        }
    }
}
