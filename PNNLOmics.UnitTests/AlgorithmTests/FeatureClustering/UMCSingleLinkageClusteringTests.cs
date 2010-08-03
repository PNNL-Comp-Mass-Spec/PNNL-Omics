using System;
using System.Collections.Generic;

using NUnit.Framework;

using PNNLOmics.Data;
using PNNLOmics.Algorithms;
using PNNLOmics.Data.Features;
using PNNLOmics.Utilities.Importers;
using PNNLOmics.Algorithms.FeatureClustering;

namespace PNNLOmics.UnitTests.AlgorithmTests.FeatureClustering
{
    [TestFixture]
    public class UMCSingleLinkageClusteringTests
    {
        /// <summary>
        ///  Part of a clustering test to make sure when sending a 
        ///  null list the clustering algorithm fails.
        /// </summary>
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        [Description("Sends a null list of UMC's to the clustering algorithm.")]
        public void ClusterNull()
        {
            // Sends a null reference to the clustering object.
            UMCSingleLinkageClusterer clustering = new UMCSingleLinkageClusterer();
            clustering.Cluster(null);
        }
        /// <summary>
        ///  Part of a clustering test to make sure when sending a 
        ///  list of valid features that it fails when encounters a null
        ///  feature.  
        /// </summary>
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        [Description("Creates a list of UMC's with one of the features being null.")]
        public void ClusterNullInList()
        {
            // Sends a null reference to the clustering object.
            UMCSingleLinkageClusterer clustering = new UMCSingleLinkageClusterer();
            List<UMC> list = new List<UMC>();
            list.Add(new UMC());
            list.Add(null);

            clustering.Cluster(list);
        }

        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [Description("This tests to make sure that given a set of UMCs that are close, " +
                     "and one feature that is far away in mass it gets it's own cluster.")]
        public void EndMassDifferenceWithTwoFeatures()
        {
            /*
             *     x4 x5
             *  
             *     ...
             * 
             * 
             *     x1 x2 x3 
             * 
             *  where x = a feature and the number is the 1-based index (ID)
             */
            UMCSingleLinkageClusterer clustering = new UMCSingleLinkageClusterer();

            // Setup the parameters to work with the data.
            UMCSingleLinkageClustererParameters parameters = new UMCSingleLinkageClustererParameters();
            parameters.OnlyClusterSameChargeStates = false;
            parameters.Tolerances = new UMCTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.NET = .06;
            parameters.Tolerances.Mass = 3;

            // Setup the test so that we have the same mass and drift data, but change in NET.
            int totalDatasets = 100;
            int totalFeatures = 5;
            double startMass = 4;
            double deltaMass = 0;
            double startNET = .1;
            double deltaNET = .05;
            double startDrift = 10;
            double deltaDrift = 0;

            List<UMC> features = new List<UMC>();
            for (int i = 0; i < totalFeatures; i++)
            {
                UMC feature = new UMC();
                feature.ID = i;
                feature.UmcCluster = null;
                feature.NETAligned = startNET;
                feature.MassMonoisotopicAligned = startMass;
                feature.DriftTime = Convert.ToSingle(startDrift);
                feature.GroupID = i % totalDatasets;
                features.Add(feature);

                startMass += deltaMass;
                startDrift += deltaDrift;
                startNET += deltaNET;
            }

            // Now make the one UMC that is far away from home.
            features[features.Count - 2].MassMonoisotopicAligned = (startMass + 100.0);
            features[features.Count - 1].MassMonoisotopicAligned = (startMass + 100.0);

            // Cluster!
            clustering.Parameters = parameters;
            List<UMCCluster> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(2, clusters.Count);
            Assert.AreEqual(2, clusters[1].UMCList.Count);
        }
        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [Description("This tests to make sure that given a set of UMCs that are close, " +
                     "and one feature that is far away in mass it gets it's own cluster.")]        
        public void EndMassDifference()
        {
            /*
             *     x4
             *  
             *     ...
             * 
             * 
             *     x1 x2 x3 
             * 
             *  where x = a feature and the number is the 1-based index (ID)
             */
            UMCSingleLinkageClusterer clustering = new UMCSingleLinkageClusterer();

            // Setup the parameters to work with the data.
            UMCSingleLinkageClustererParameters parameters = new UMCSingleLinkageClustererParameters();
            parameters.OnlyClusterSameChargeStates = false;
            parameters.Tolerances = new UMCTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.NET = .06;
            parameters.Tolerances.Mass = 3;

            // Setup the test so that we have the same mass and drift data, but change in NET.
            int totalDatasets = 100;
            int totalFeatures = 4;
            double startMass = 4;
            double deltaMass = 0;
            double startNET = .1;
            double deltaNET = .05;
            double startDrift = 10;
            double deltaDrift = 0;

            List<UMC> features = new List<UMC>();
            for (int i = 0; i < totalFeatures; i++)
            {
                UMC feature = new UMC();
                feature.ID = i;
                feature.UmcCluster = null;
                feature.NETAligned = startNET;
                feature.MassMonoisotopicAligned = startMass;
                feature.DriftTime = Convert.ToSingle(startDrift);
                feature.GroupID = i % totalDatasets;
                features.Add(feature);

                startMass += deltaMass;
                startDrift += deltaDrift;
                startNET += deltaNET;
            }

            // Now make the one UMC that is far away from home.
            features[features.Count - 1].MassMonoisotopicAligned = (startMass + 100.0);

            // Cluster!
            clustering.Parameters = parameters;
            List<UMCCluster> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);            
            Assert.AreEqual(2, clusters.Count);
            Assert.AreEqual(1, clusters[1].UMCList.Count);
        }
        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [Description("This test makes a set of UMC's whose only varying dimension is NET.")]
        public void SingleClusterTest()
        {            
            UMCSingleLinkageClusterer clustering = new UMCSingleLinkageClusterer();

            // Setup the parameters to work with the data.
            UMCSingleLinkageClustererParameters parameters  = new UMCSingleLinkageClustererParameters();
            parameters.OnlyClusterSameChargeStates          = false;
            parameters.Tolerances = new UMCTolerances();
            parameters.Tolerances.DriftTime     = 1;
            parameters.Tolerances.NET           = .06;
            parameters.Tolerances.Mass          = 3;

            // Setup the test so that we have the same mass and drift data, but change in NET.
            int totalDatasets = 6;
            int totalFeatures = 5;
            double startMass  = 4;
            double deltaMass  = 0;
            double startNET   = .1;
            double deltaNET   = .05;
            double startDrift = 10;
            double deltaDrift = 0;

            List<UMC> features = new List<UMC>();
            for (int i = 0; i < totalFeatures; i++)
            {
                UMC feature                     = new UMC();
                feature.ID                      = i;
                feature.UmcCluster              = null;
                feature.NETAligned              = startNET;
                feature.MassMonoisotopicAligned = startMass;
                feature.DriftTime               = Convert.ToSingle(startDrift);
                feature.GroupID                 = i % totalDatasets;
                features.Add(feature);

                startMass  += deltaMass;
                startDrift += deltaDrift;
                startNET   += deltaNET;
            }

            // Cluster
            clustering.Parameters = parameters;
            List<UMCCluster> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(1, clusters.Count);
        }

        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [Description("This test makes sure two dataset features are not clustered together.")]
        public void ClusterOverlapMergeTest()
        {            
            UMCSingleLinkageClusterer clustering = new UMCSingleLinkageClusterer();

            // Setup the parameters to work with the data.
            UMCSingleLinkageClustererParameters parameters = new UMCSingleLinkageClustererParameters();
            parameters.OnlyClusterSameChargeStates = false;
            parameters.Tolerances = new UMCTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.NET = .06;
            parameters.Tolerances.Mass = 3;

            // Setup the test so that we have the same mass and drift data, but change in NET.
            int totalDatasets = 3;
            int totalFeatures = 5;
            double startMass  = 4;
            double deltaMass  = 0;
            double startNET   = .1;
            double deltaNET   = .05;
            double startDrift = 10;
            double deltaDrift = 0;

            List<UMC> features = new List<UMC>();
            for (int i = 0; i < totalFeatures; i++)
            {
                UMC feature = new UMC();
                feature.ID = i;
                feature.UmcCluster = null;
                feature.NETAligned = startNET;
                feature.MassMonoisotopicAligned = startMass;
                feature.DriftTime = Convert.ToSingle(startDrift);
                feature.GroupID = i % totalDatasets;
                features.Add(feature);

                startMass += deltaMass;
                startDrift += deltaDrift;
                startNET += deltaNET;
            }

            // Cluster
            clustering.Parameters = parameters;
            List<UMCCluster> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(3, clusters.Count);
        }
    }
}
