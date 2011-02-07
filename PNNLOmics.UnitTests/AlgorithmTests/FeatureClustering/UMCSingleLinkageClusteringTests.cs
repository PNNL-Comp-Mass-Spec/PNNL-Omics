/*////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * 
 * Name:    UMC Single Linkage Clusterer Tests
 * File:    UMCSingleLinkageClusteringTests.cs
 * Author:  Brian LaMarche 
 * Purpose: Tests Perform clustering of UMC features across datasets into UMC Clusters.
 * Date:    8-02-2010
 * Revisions:
 *          8-02-2010 - BLL - Created mass and edge case tests.
 *          8-02-2010 - BLL - Added more NET edge case tests.
 *          9-22-2010 - BLL - Added more Mass and NET as well as singleton edge case tests for code coverage.
 *          
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
			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();
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
			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();
			List<UMCLight> list = new List<UMCLight>();
			list.Add(new UMCLight());
            list.Add(null);

            clustering.Cluster(list);
        }
        #endregion

        #region Mass Tests
        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [Category("Edge")]
        [Description("This tests to make sure that given a set of UMCs that are close, " +
                     "and one feature that is far away in mass it gets it's own cluster.")]        
        public void MassDifferenceWithTwoFeatures()
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
			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters parameters = new FeatureClusterParameters();
            parameters.OnlyClusterSameChargeStates = false;
            parameters.Tolerances = new FeatureTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.RetentionTime = .06;
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

			List<UMCLight> features = new List<UMCLight>();
            for (int i = 0; i < totalFeatures; i++)
            {
				UMCLight feature = new UMCLight();
                feature.ID          = i;
                feature.UMCCluster  = null;
                feature.RetentionTime  = startNET;
                feature.MassMonoisotopic = startMass;
                feature.DriftTime   = Convert.ToSingle(startDrift);
                feature.GroupID     = i % totalDatasets;
                features.Add(feature);

                startMass   += deltaMass;
                startDrift  += deltaDrift;
                startNET    += deltaNET;
            }

            // Now make the two UMC's that are far away from home.
            features[features.Count - 2].MassMonoisotopic = (startMass + 100.0);
            features[features.Count - 1].MassMonoisotopic = (startMass + 100.0);

            // Cluster!
            clustering.Parameters = parameters;
			List<UMCClusterLight> clusters = clustering.Cluster(features);

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
        [Category("Edge")]
        [Description("This tests to make sure that given a set of UMCs that are close, " +
                     "and one feature that is far away in mass it gets it's own cluster.")]
        public void MassDifference()
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
			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters parameters  = new FeatureClusterParameters();
            parameters.OnlyClusterSameChargeStates          = false;
            parameters.Tolerances           = new FeatureTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.RetentionTime       = .06;
            parameters.Tolerances.Mass      = 3;

            // Setup the test so that we have the same mass and drift data, but change in NET.
            int totalDatasets = 100;
            int totalFeatures = 4;
            double startMass = 4;
            double deltaMass = 0;
            double startNET = .1;
            double deltaNET = .05;
            double startDrift = 10;
            double deltaDrift = 0;

			List<UMCLight> features = new List<UMCLight>();
            for (int i = 0; i < totalFeatures; i++)
            {
				UMCLight feature = new UMCLight();
                feature.ID = i;
                feature.UMCCluster = null;
                feature.RetentionTime = startNET;
                feature.MassMonoisotopic = startMass;
                feature.DriftTime = Convert.ToSingle(startDrift);
                feature.GroupID = i % totalDatasets;
                features.Add(feature);

                startMass += deltaMass;
                startDrift += deltaDrift;
                startNET += deltaNET;
            }

            // Now make the one UMC that is far away from home.
            features[features.Count - 1].MassMonoisotopic = (startMass + 100.0);

            // Cluster!
            clustering.Parameters = parameters;
            List<UMCClusterLight> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(2, clusters.Count);
            Assert.AreEqual(1, clusters[1].UMCList.Count);
            Assert.AreEqual(totalFeatures - 1, clusters[0].UMCList.Count);
        }
        /// <summary>
        /// Produces two clusters with approximately the same mass within, and then
        /// separate nets.
        /// </summary>
        [Test]
        [Category("Edge")]
        [Description("This tests to make sure that given a set of UMCs that are close, " +
                     "and one feature that is far away in mass it gets it's own cluster.")]
        public void MassDifferenceWithMultipleFeatures()
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
			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters parameters = new FeatureClusterParameters();
            parameters.OnlyClusterSameChargeStates = false;
            parameters.Tolerances = new FeatureTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.RetentionTime = .06;
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

			List<UMCLight> features = new List<UMCLight>();
            for (int i = 0; i < totalFeatures; i++)
            {
				UMCLight feature = new UMCLight();
                feature.ID = i;
                feature.UMCCluster = null;
                feature.RetentionTime = startNET;
                feature.MassMonoisotopic = startMass;
                feature.DriftTime = Convert.ToSingle(startDrift);
                feature.GroupID = i % totalDatasets;
                features.Add(feature);

                startMass  += deltaMass;
                startDrift += deltaDrift;
                startNET   += deltaNET;
            }

            // Now make the one UMC that is far away from home.
            features[0].MassMonoisotopic = (startMass - 100.0);
            features[1].MassMonoisotopic = (features[0].MassMonoisotopic);

            // Cluster!
            clustering.Parameters = parameters;
			List<UMCClusterLight> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(2, clusters.Count);
            Assert.AreEqual(3, clusters[1].UMCList.Count);
            Assert.AreEqual(totalFeatures - 3, clusters[0].UMCList.Count);
        }
        [Test]        
        [Description("This tests to make sure that given a set of UMCs that are close, " +
                     "and one feature that is far away in mass it gets it's own cluster.")]
        public void MassDifferenceTests()
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
			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters parameters = new FeatureClusterParameters();
            parameters.OnlyClusterSameChargeStates = false;
            parameters.Tolerances = new FeatureTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.RetentionTime = .06;
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

			List<UMCLight> features = new List<UMCLight>();
            for (int i = 0; i < totalFeatures; i++)
            {
				UMCLight feature = new UMCLight();
                feature.ID = i;
                feature.UMCCluster = null;
                feature.RetentionTime = startNET;
                feature.MassMonoisotopic = startMass;
                feature.DriftTime = Convert.ToSingle(startDrift);
                feature.GroupID = i % totalDatasets;
                features.Add(feature);

                startMass += deltaMass;
                startDrift += deltaDrift;
                startNET += deltaNET;
            }

            // Now make the one UMC that is far away from home.
            features[0].MassMonoisotopic = (startMass - 100.0);

            // Cluster!
            clustering.Parameters = parameters;
			List<UMCClusterLight> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(2, clusters.Count);
            Assert.AreEqual(1, clusters[0].UMCList.Count);
            Assert.AreEqual(totalFeatures - 1, clusters[1].UMCList.Count);
        }
        #endregion

        #region NET Tests
        [Test]
        [Category("Edge")]
        [Description("")]
        public void NETTest()
        {    /*             
             *     x1 x2 ... x3 
             */
			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters parameters = new FeatureClusterParameters();
            parameters.OnlyClusterSameChargeStates = false;
            parameters.Tolerances = new FeatureTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.RetentionTime = .06;
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

			List<UMCLight> features = new List<UMCLight>();
            for (int i = 0; i < totalFeatures; i++)
            {
				UMCLight feature = new UMCLight();
                feature.ID = i;
                feature.UMCCluster = null;
                feature.RetentionTime = startNET;
                feature.MassMonoisotopic = startMass;
                feature.DriftTime = Convert.ToSingle(startDrift);
                feature.GroupID = i % totalDatasets;
                features.Add(feature);

                startMass += deltaMass;
                startDrift += deltaDrift;
                startNET += deltaNET;
            }

            // Now make the one UMC that is far away from home.
            features[features.Count - 1].RetentionTime = parameters.Tolerances.RetentionTime * 100;

            // Cluster!
            clustering.Parameters = parameters;
			List<UMCClusterLight> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(2, clusters.Count);

            // Singletons will be the first in the list as the other clusters are merged.
            // This is why we check the first cluster for a size of one, and not the last one.            
            Assert.AreEqual(1, clusters[0].UMCList.Count);
            Assert.AreEqual(totalFeatures - 1, clusters[1].UMCList.Count);
        }
        [Test]
        [Category("Edge")]
        [Description("")]
        public void NETTestReversed()
        {    /*             
             *     x1 ... x2, x3 
             */
			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters parameters = new FeatureClusterParameters();
            parameters.OnlyClusterSameChargeStates = false;
            parameters.Tolerances = new FeatureTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.RetentionTime = .06;
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

			List<UMCLight> features = new List<UMCLight>();
            for (int i = 0; i < totalFeatures; i++)
            {
				UMCLight feature = new UMCLight();
                feature.ID                      = i;
                feature.UMCCluster              = null;
                feature.RetentionTime              = startNET;
                feature.MassMonoisotopic = startMass;
                feature.DriftTime               = Convert.ToSingle(startDrift);
                feature.GroupID                 = i % totalDatasets;
                features.Add(feature);

                startMass   += deltaMass;
                startDrift  += deltaDrift;
                startNET    += deltaNET;
            }

            // Now make the one UMC that is far away from home.
            features[0].RetentionTime = parameters.Tolerances.RetentionTime * 100;

            // Cluster!
            clustering.Parameters = parameters;
			List<UMCClusterLight> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(2, clusters.Count);

            // Singletons will be the first in the list as the other clusters are merged.
            // This is why we check the first cluster for a size of one, and not the last one.            
            Assert.AreEqual(1, clusters[0].UMCList.Count);
            Assert.AreEqual(totalFeatures - 1, clusters[1].UMCList.Count);
        }
        #endregion

        #region Single Clusters
        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [TestCase(0, .05, Description = "Delta Mass = 0")]
        [TestCase(.00000001, 0, Description = "Delta NET  = 0")]
        [Description("This test makes a set of UMC's whose only varying dimension is NET.")]
        public void CreateSingleClusterTests(double deltaMass, double deltaNET)
        {
			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters parameters = new FeatureClusterParameters();
            parameters.OnlyClusterSameChargeStates  = false;
            parameters.Tolerances                   = new FeatureTolerances();
            parameters.Tolerances.DriftTime         = 1;
            parameters.Tolerances.RetentionTime               = .06;
            parameters.Tolerances.Mass              = 50;

            // Setup the test so that we have the same mass and drift data, but change in NET.
            int totalDatasets   = 6;
            int totalFeatures   = 5;
            double startMass    = 4;            
            double startNET     = 0.10;
            double startDrift   = 10;
            double deltaDrift   = 0;

			List<UMCLight> features = new List<UMCLight>();
            for (int i = 0; i < totalFeatures; i++)
            {
				UMCLight feature = new UMCLight();
                feature.ID                      = i;
                feature.UMCCluster              = null;
                feature.RetentionTime              = startNET;
                feature.MassMonoisotopic = startMass;
                feature.DriftTime               = Convert.ToSingle(startDrift);
                feature.GroupID                 = i % totalDatasets;
                features.Add(feature);

                startMass  += deltaMass;
                startDrift += deltaDrift;
                startNET   += deltaNET;
            }

            // Cluster
            clustering.Parameters = parameters;
			List<UMCClusterLight> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(1, clusters.Count);
        }
        #endregion

        #region Singleton Clusters
        /// <summary>
        /// Creates features who are clearly not clusterable based on tolerances.
        /// </summary>
        [Test]
        [Category("Edge")]
        public void CreateSingletonTests()
        {
			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters parameters = new FeatureClusterParameters();
            parameters.OnlyClusterSameChargeStates         = false;
            parameters.Tolerances           = new FeatureTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.RetentionTime       = .06;
            parameters.Tolerances.Mass      = 50;

            // Setup the test so that we have the same mass and drift data, but change in NET.
            int totalDatasets   = 100;
            int totalFeatures   = 5;
            double startMass    = 500;
            double startNET     = 0.10;
            double startDrift   = 10;

            double deltaNet   = parameters.Tolerances.RetentionTime * 2;
            double deltaDrift = parameters.Tolerances.DriftTime * 2;

			List<UMCLight> features = new List<UMCLight>();
            for (int i = 0; i < totalFeatures; i++)
            {
				UMCLight feature = new UMCLight();
                feature.ID              = i;
                feature.UMCCluster      = null;
                feature.RetentionTime      = startNET;
                feature.MassMonoisotopic = startMass;
                feature.DriftTime       = Convert.ToSingle(startDrift);
                feature.GroupID         = i % totalDatasets;
                feature.ChargeState     = i;

                feature.DriftTime               = Convert.ToSingle(startDrift);
                feature.RetentionTime              = startNET;
                feature.MassMonoisotopic = startMass;

                // Always put it just outside of the next feature.
                startMass   = Feature.ComputeDaDifferenceFromPPM(startMass, parameters.Tolerances.Mass * 1.5);
                startNET   += deltaNet;
                startDrift += deltaDrift;

                features.Add(feature);
            }
            
            clustering.Parameters     = parameters;
			List<UMCClusterLight> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(totalFeatures, clusters.Count);
        }
        #endregion

        #region Charge State
        /// <summary>
        /// Tests clustering features with like charges if IMS data.  Otherwise clusters normally.
        /// </summary>
        [Test]
        [TestCase(false)]
        [TestCase(true)]
        [Description("This test will separate clusteres based on charge.")]
        public void ChargeStatesTests(bool onlyClusterCharges)
        {
			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters parameters  = new FeatureClusterParameters();
            parameters.OnlyClusterSameChargeStates          = onlyClusterCharges;
            parameters.Tolerances                           = new FeatureTolerances();
            parameters.Tolerances.DriftTime                 = 1;
            parameters.Tolerances.RetentionTime                       = .06;
            parameters.Tolerances.Mass                      = 50;

            // Setup the test so that we have the same mass and drift data, but change in NET.
            int totalDatasets = 100;
            int totalFeatures = 5;
            double startMass  = 500;
            double startNET   = 0.10;
            double startDrift = 10;

			List<UMCLight> features = new List<UMCLight>();
            for (int i = 0; i < totalFeatures; i++)
            {
				UMCLight feature = new UMCLight();
                feature.ID          = i;
                feature.UMCCluster  = null;
                feature.RetentionTime  = startNET;
                feature.MassMonoisotopic = startMass;
                feature.DriftTime   = Convert.ToSingle(startDrift);
                feature.GroupID     = i % totalDatasets;
                feature.ChargeState = i;
                features.Add(feature);                
            }

            // Cluster
            clustering.Parameters = parameters;
			List<UMCClusterLight> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            if (onlyClusterCharges)
            {
                Assert.AreEqual(totalFeatures, clusters.Count);
            }
            else
            {
                Assert.AreNotEqual(totalFeatures, clusters.Count);
                Assert.AreEqual(1, clusters.Count);
            }
        }
        #endregion
    
        #region Dataset Overlap and Merging Tests
        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [Description("This test makes sure two dataset features are not clustered together.")]
        public void OverlapDatasetExclusionTest1()
        {
            /*
             *
             *  x1-d1 x2-d2 x3-d3 | x4-d1 x5-d2
             * 
             */

			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters parameters = new FeatureClusterParameters();
            parameters.OnlyClusterSameChargeStates = false;
            parameters.Tolerances = new FeatureTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.RetentionTime = .06;
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

			List<UMCLight> features = new List<UMCLight>();
            for (int i = 0; i < totalFeatures; i++)
            {
				UMCLight feature = new UMCLight();
                feature.ID = i;
                feature.UMCCluster = null;
                feature.RetentionTime = startNET;
                feature.MassMonoisotopic = startMass;
                feature.DriftTime = Convert.ToSingle(startDrift);
                feature.GroupID = i % totalDatasets;
                features.Add(feature);

                startMass  += deltaMass;
                startDrift += deltaDrift;
                startNET   += deltaNET;
            }

            // Cluster
            clustering.Parameters = parameters;
			List<UMCClusterLight> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(1, clusters.Count);
        }
        /// <summary>
        /// Only produces a single cluster because all features should have the same mass.
        /// </summary>
        [Test]
        [Description("This test makes sure two dataset features are not clustered together.")]
        public void OverlapDatasetExclusionTest2()
        {
            /*
             *  Cluster configuration
             * 
             *      x1-d1 x2-d2 x3-d2 x4-d1
             * 
             */
			UMCSingleLinkageClusterer<UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters parameters = new FeatureClusterParameters();
            parameters.OnlyClusterSameChargeStates = false;
            parameters.Tolerances           = new FeatureTolerances();
            parameters.Tolerances.DriftTime = 1;
            parameters.Tolerances.RetentionTime       = .06;
            parameters.Tolerances.Mass      = 3;

            // Setup the test so that we have the same mass and drift data, but change in NET.            
            int totalFeatures   = 4;
            double startMass    = 5;
            double deltaMass    = 0;
            double startNET     = .1;
            double deltaNET     = .05;
            double startDrift   = 10;
            double deltaDrift   = 0;

            // Create all features.
			List<UMCLight> features = new List<UMCLight>();
            for (int i = 0; i < totalFeatures; i++)
            {
				UMCLight feature = new UMCLight();
                feature.ID          = i;
                feature.UMCCluster  = null;
                feature.RetentionTime  = startNET;
                feature.MassMonoisotopic = startMass;
                feature.DriftTime   = Convert.ToSingle(startDrift);
                feature.GroupID     = 0;
                features.Add(feature);

                startMass   += deltaMass;
                startDrift  += deltaDrift;
                startNET    += deltaNET;
            }

            features[0].GroupID = 1;
            features[1].GroupID = 2;
            features[2].GroupID = 2;
            features[3].GroupID = 1;

            // Cluster
            clustering.Parameters     = parameters;
			List<UMCClusterLight> clusters = clustering.Cluster(features);

            // Make sure we have only one cluster.
            Assert.IsNotNull(clusters);
            Assert.AreEqual(1, clusters.Count);                                   
        }
        #endregion
    }
}