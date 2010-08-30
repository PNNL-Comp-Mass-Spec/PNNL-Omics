/*////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * 
 * Name:    UMC Single Linkage Clusterer 
 * File:    UMCSingleLinkageClustering.cs
 * Author:  Brian LaMarche 
 * Purpose: Perform clustering of UMC features across datasets into UMC Clusters.
 * Date:    8-02-2010
 * Revisions:
 *          8-02-2010 - BLL - Created mass and edge case tests.
 *          8-02-2010 - BLL - Added more NET edge case tests.
 ////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
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
        #region Null Reference Data
        /// <summary>
        ///  Part of a clustering test to make sure when sending a 
        ///  null list the clustering algorithm fails.
        /// </summary>
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        [Description("Sends a null list of UMC's to the clustering algorithm.")]
        public void BadDataClusterNull()
        {
            // Sends a null reference to the clustering object.
            UMCSingleLinkageClusterer<UMCCluster> clustering = new UMCSingleLinkageClusterer<UMCCluster>();
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
        public void BadDataClusterNullInList()
        {
            // Sends a null reference to the clustering object.
            UMCSingleLinkageClusterer<UMCCluster> clustering = new UMCSingleLinkageClusterer<UMCCluster>();
            List<UMC> list = new List<UMC>();
            list.Add(new UMC());
            list.Add(null);

            clustering.Cluster(list);
        }
        #endregion

        #region Mass Difference Edge Cases
        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [Description("This tests to make sure that given a set of UMCs that are close, " +
                     "and one feature that is far away in mass it gets it's own cluster.")]
        public void EdgeEndMassDifferenceWithTwoFeatures()
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
            UMCSingleLinkageClusterer<UMCCluster> clustering = new UMCSingleLinkageClusterer<UMCCluster>();

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
            Assert.AreEqual(totalFeatures - 2, clusters[0].UMCList.Count);
        }
        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [Description("This tests to make sure that given a set of UMCs that are close, " +
                     "and one feature that is far away in mass it gets it's own cluster.")]
        public void EdgeEndMassDifference()
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
            UMCSingleLinkageClusterer<UMCCluster> clustering = new UMCSingleLinkageClusterer<UMCCluster>();

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
            Assert.AreEqual(totalFeatures - 1, clusters[0].UMCList.Count);
        }
        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [Description("This tests to make sure that given a set of UMCs that are close, " +
                     "and one feature that is far away in mass it gets it's own cluster.")]
        public void EdgeStartMassDifferenceWithTwoFeatures()
        {
            /*
             *     x3 x4 x5
             *  
             *     ...
             * 
             * 
             *     x1 x2  
             * 
             *  where x = a feature and the number is the 1-based index (ID)
             */
            UMCSingleLinkageClusterer<UMCCluster> clustering = new UMCSingleLinkageClusterer<UMCCluster>();

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
            double startMass = 400;
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

                startMass  += deltaMass;
                startDrift += deltaDrift;
                startNET   += deltaNET;
            }

            // Now make the one UMC that is far away from home.
            features[0].MassMonoisotopicAligned = (startMass - 100.0);
            features[1].MassMonoisotopicAligned = (features[0].MassMonoisotopicAligned);

            // Cluster!
            clustering.Parameters = parameters;
            List<UMCCluster> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(2, clusters.Count);
            Assert.AreEqual(3, clusters[1].UMCList.Count);
            Assert.AreEqual(totalFeatures - 3, clusters[0].UMCList.Count);
        }
        [Test]
        [Description("This tests to make sure that given a set of UMCs that are close, " +
                     "and one feature that is far away in mass it gets it's own cluster.")]
        public void EdgeStartMassDifference()
        {
            /*
             *     x2 x3 x4
             *  
             *     ...
             *              
             *     x1  
             * 
             *  where x = a feature and the number is the 1-based index (ID)
             */
            UMCSingleLinkageClusterer<UMCCluster> clustering = new UMCSingleLinkageClusterer<UMCCluster>();

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
            double startMass = 400;
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
            features[0].MassMonoisotopicAligned = (startMass - 100.0);

            // Cluster!
            clustering.Parameters = parameters;
            List<UMCCluster> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(2, clusters.Count);
            Assert.AreEqual(1, clusters[0].UMCList.Count);
            Assert.AreEqual(totalFeatures - 1, clusters[1].UMCList.Count);
        }
        #endregion

        #region NET Difference edge cases
        [Test]
        [Description("")]
        public void EdgeNETTest()
        {    /*             
             *     x1 x2 ... x3 
             */
            UMCSingleLinkageClusterer<UMCCluster> clustering = new UMCSingleLinkageClusterer<UMCCluster>();

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
            double startNET = 0.1;
            double deltaNET = 0.05;
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
            features[features.Count - 1].NETAligned = parameters.Tolerances.NET * 100;

            // Cluster!
            clustering.Parameters = parameters;
            List<UMCCluster> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(2, clusters.Count);

            // Singletons will be the first in the list as the other clusters are merged.
            // This is why we check the first cluster for a size of one, and not the last one.            
            Assert.AreEqual(1, clusters[0].UMCList.Count);
            Assert.AreEqual(totalFeatures - 1, clusters[1].UMCList.Count);
        }
        [Test]
        [Description("")]
        public void EdgeNETTestReversed()
        {    /*             
             *     x1 ... x2, x3 
             */
            UMCSingleLinkageClusterer<UMCCluster> clustering = new UMCSingleLinkageClusterer<UMCCluster>();

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
            double startNET = 0.1;
            double deltaNET = 0.05;
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

                startMass   += deltaMass;
                startDrift  += deltaDrift;
                startNET    += deltaNET;
            }

            // Now make the one UMC that is far away from home.
            features[0].NETAligned = parameters.Tolerances.NET * 100;

            // Cluster!
            clustering.Parameters = parameters;
            List<UMCCluster> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(2, clusters.Count);

            // Singletons will be the first in the list as the other clusters are merged.
            // This is why we check the first cluster for a size of one, and not the last one.            
            Assert.AreEqual(1, clusters[1].UMCList.Count);
            Assert.AreEqual(totalFeatures - 1, clusters[0].UMCList.Count);
        }
        #endregion

        #region Valid Cluster Tests
        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [Description("This test makes a set of UMC's whose only varying dimension is NET.")]
        public void EdgeSingleClusterTest()
        {
            UMCSingleLinkageClusterer<UMCCluster> clustering = new UMCSingleLinkageClusterer<UMCCluster>();

            // Setup the parameters to work with the data.
            UMCSingleLinkageClustererParameters parameters = new UMCSingleLinkageClustererParameters();
            parameters.OnlyClusterSameChargeStates = false;
            parameters.Tolerances = new UMCTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.NET = .06;
            parameters.Tolerances.Mass = 3;

            // Setup the test so that we have the same mass and drift data, but change in NET.
            int totalDatasets   = 6;
            int totalFeatures   = 5;
            double startMass    = 4;
            double deltaMass    = 0;
            double startNET     = 0.10;
            double deltaNET     = 0.05;
            double startDrift   = 10;
            double deltaDrift   = 0;

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
        [Description("Cluster a single feature.")]
        public void EdgeSingleFeatureTest()
        {
            UMCSingleLinkageClusterer<UMCCluster> clustering = new UMCSingleLinkageClusterer<UMCCluster>();

            // Setup the parameters to work with the data.
            UMCSingleLinkageClustererParameters parameters = new UMCSingleLinkageClustererParameters();
            parameters.OnlyClusterSameChargeStates  = false;
            parameters.Tolerances                   = new UMCTolerances();
            parameters.Tolerances.DriftTime         = 1;
            parameters.Tolerances.NET               = .06;
            parameters.Tolerances.Mass              = 3;

            // Setup the test so that we have the same mass and drift data, but change in NET.
            int totalDatasets = 6;
            int totalFeatures = 1;
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
            Assert.AreEqual(1, clusters.Count);
            Assert.AreEqual(1, clusters[0].UMCList.Count);
        }
        #endregion
    
        #region Overlap Tests
        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [Description("This test makes sure two dataset features are not clustered together.")]
        public void ClusterOverlapMergeTest()
        {
            UMCSingleLinkageClusterer<UMCCluster> clustering = new UMCSingleLinkageClusterer<UMCCluster>();

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
        #endregion
    }
}
