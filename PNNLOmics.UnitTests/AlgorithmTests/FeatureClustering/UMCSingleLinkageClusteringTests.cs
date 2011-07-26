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
            UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();
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
            UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();
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
            UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters<UMCLight> parameters = new FeatureClusterParameters<UMCLight>();
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
			UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters<UMCLight> parameters   = new FeatureClusterParameters<UMCLight>();
            parameters.OnlyClusterSameChargeStates          = false;
            parameters.Tolerances                           = new FeatureTolerances();
            parameters.Tolerances.DriftTime                 = 1;
            parameters.Tolerances.RetentionTime             = .06;
            parameters.Tolerances.Mass                      = 3;

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
			UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters<UMCLight> parameters = new FeatureClusterParameters<UMCLight>();
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
			UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters<UMCLight> parameters = new FeatureClusterParameters<UMCLight>();
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
			UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters<UMCLight> parameters = new FeatureClusterParameters<UMCLight>();
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
			UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters<UMCLight> parameters = new FeatureClusterParameters<UMCLight>();
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
			UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters<UMCLight> parameters = new FeatureClusterParameters<UMCLight>();
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
			UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters<UMCLight> parameters = new FeatureClusterParameters<UMCLight>();
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
			UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters<UMCLight> parameters = new FeatureClusterParameters<UMCLight>();
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

			UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters<UMCLight> parameters = new FeatureClusterParameters<UMCLight>();
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
			UMCSingleLinkageClusterer<UMCLight, UMCClusterLight> clustering = new UMCSingleLinkageClusterer<UMCLight, UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters<UMCLight> parameters = new FeatureClusterParameters<UMCLight>();
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


        public UMCLight CreateFeature(double net, double drift, double mass)
        {
            UMCLight feature            = new UMCLight();
            feature.RetentionTime       = net;
            feature.DriftTime           = drift;
            feature.MassMonoisotopic    = mass;

            return feature;
        }

        [Test]
        [Category("Edge")]
        [Description("")]
        public void AverageLinkageEdgeTest30Features()
        {
            UMCCentroidClusterer<UMCLight, UMCClusterLight> clustering = new UMCCentroidClusterer<UMCLight, UMCClusterLight>();

            // Setup the parameters to work with the data.
            FeatureClusterParameters<UMCLight> parameters   = new FeatureClusterParameters<UMCLight>();
            parameters.OnlyClusterSameChargeStates          = true;
            parameters.Tolerances               = new FeatureTolerances();
            parameters.Tolerances.DriftTime     = 100;
            parameters.Tolerances.RetentionTime = .03;
            parameters.Tolerances.Mass          = 20;
            

            List<UMCLight> features = new List<UMCLight>();
            features.Add(CreateFeature(0.0003106554831  , 27.8946495056152		, 1770.9909200000000));
            features.Add(CreateFeature(0.0053106554831	, 27.8546905517578	    , 1770.9982900000000));
            features.Add(CreateFeature(0.0053226847370	, 28.2486000061035	    , 1771.0066400000000));
            features.Add(CreateFeature(0.0101613423685	, 28.1553802490234	    , 1771.0201700000000));
            features.Add(CreateFeature(0.0101613423685	, 28.2970104217529	    , 1771.0138400000000));
            features.Add(CreateFeature(0.0103226847370	, 28.2974395751953	    , 1771.0253300000000));
            features.Add(CreateFeature(0.0153226847370	, 28.2923393249512	    , 1771.0104700000000));
            features.Add(CreateFeature(0.0304840271055	, 27.9291706085205	    , 1771.0288400000000));
            features.Add(CreateFeature(0.0451613423685	, 27.8556003570557	    , 1771.0041600000000));
            features.Add(CreateFeature(0.0551613423685	, 27.7265300750732	    , 1771.0327600000000));
            features.Add(CreateFeature(0.3875411423040	, 25.7421207427979	    , 1770.9775100000000));
            features.Add(CreateFeature(0.3895708292998	, 25.5387001037598		, 1770.9600700000000));
            features.Add(CreateFeature(0.5133946434334	, 26.2309093475342	    , 1770.9480700000000));
            features.Add(CreateFeature(0.5311358502743	, 26.9620800018311		, 1771.0080600000000));
            features.Add(CreateFeature(0.5312208760485	, 26.1607608795166		, 1770.9537500000000));
            features.Add(CreateFeature(0.5315610438024	, 26.7394695281982		, 1770.9534100000000));
            features.Add(CreateFeature(0.5346014843498	, 26.6907501220703		, 1770.9723200000000));
            features.Add(CreateFeature(0.5738931913520	, 28.5330104827881		, 1771.0429400000000));
            features.Add(CreateFeature(0.5774427234592	, 28.4089603424072		, 1771.0077700000000));
            features.Add(CreateFeature(0.6417150693772	, 26.7624702453613		, 1770.9653300000000));
            features.Add(CreateFeature(0.6433970177074	, 26.1307697296143		, 1770.9206400000000));
            features.Add(CreateFeature(0.6839190061310	, 26.6364002227783		, 1771.0101900000000));
            features.Add(CreateFeature(0.6910277508874	, 27.9026203155518		, 1770.9747100000000));
            features.Add(CreateFeature(0.6939319135205	, 25.5249691009521		, 1770.9405900000000));
            features.Add(CreateFeature(0.7371619877380	, 27.7587509155273		, 1771.0300100000000));
            features.Add(CreateFeature(0.7422942884802	, 27.8166198730469		, 1771.0246700000000));
            features.Add(CreateFeature(0.7440198819509	, 27.8214893341064		, 1770.9912500000000));
            features.Add(CreateFeature(0.7452920161541	, 27.8142299652100		, 1770.9972400000000));
            features.Add(CreateFeature(0.7458518877057	, 27.9352302551270		, 1771.0228000000000));
            features.Add(CreateFeature(0.7471297192643	, 28.0252609252930		, 1771.0083100000000));
            features.Add(CreateFeature(0.7481429493385	, 27.8462505340576		, 1771.0139300000000));
            features.Add(CreateFeature(0.8735140367861	, 28.0626907348633	    , 1771.0245900000000));
            features.Add(CreateFeature(0.8736495643756	, 27.7607707977295		, 1771.0067400000000));
            features.Add(CreateFeature(0.8793352694418	, 27.9658107757568		, 1771.0138700000000));
            features.Add(CreateFeature(0.9256227815424	, 28.1710796356201		, 1771.0137300000000));
            features.Add(CreateFeature(0.9269959614787	, 27.8673591613770		, 1770.9987100000000));
            features.Add(CreateFeature(0.9343449499839	, 27.7115707397461		, 1771.0309300000000));
            features.Add(CreateFeature(0.9485382381413	, 28.1701602935791		, 1771.0094300000000));
            features.Add(CreateFeature(0.9659664407874	, 27.9675292968750		, 1771.0249900000000));
            features.Add(CreateFeature(0.9742155949053	, 27.8594703674316		, 1770.9924700000000));

            features.Clear();
            features.Add(CreateFeature(285.98113, 0, 0.0255020720433535));
            features.Add(CreateFeature(285.98081	, 0 ,	0.00956327701625757));
            features.Add(CreateFeature(285.97977	, 0 ,	0.0121134842205929));
            features.Add(CreateFeature(285.97912	, 0 ,	0.0494102645839974));
            features.Add(CreateFeature(285.97908	, 0 ,	0.0127510360216768));
            features.Add(CreateFeature(285.97997	, 0 ,	0.0535543512910424));
            features.Add(CreateFeature(285.97967	, 0 ,	0.0245457443417278));
            features.Add(CreateFeature(285.97952	, 0 ,	0.0554670066942939));
            features.Add(CreateFeature(285.9795	    , 0 ,	0.0529167994899586));
            features.Add(CreateFeature(285.97946	, 0 ,	0.0066942939113803));
            features.Add(CreateFeature(285.97923	, 0 ,	0.0650302837105515));
            features.Add(CreateFeature(285.97921	, 0 ,	0.0105196047178833));
            features.Add(CreateFeature(285.97907	, 0 ,	0.0143449155243864));
            features.Add(CreateFeature(285.979	    , 0 ,	0.0612049729040484));
            features.Add(CreateFeature(285.97886	, 0 ,	0.0624800765062161));
            features.Add(CreateFeature(285.97861	, 0 ,	0.0484539368823717));
            features.Add(CreateFeature(285.97932	, 0 ,	0.0608861970035065));
            features.Add(CreateFeature(285.97799	, 0 ,	0.00637551801083838));
            foreach (UMCLight feature in features)
            {
                feature.MassMonoisotopic = 100.1;
                feature.DriftTime        = 10.1;
            }
                //feature.ID = featureID++;
                //feature.UMCCluster = null;
                //feature.MassMonoisotopic = 100.1;
                //feature.DriftTime = 10.1;
                //feature.GroupID = 1;

                //if (i == 0) feature.RetentionTime = 0.000310655483069276;
                //else if (i == 1) feature.RetentionTime = 0.00531065548306928;
                //else if (i == 2) feature.RetentionTime = 0.00532268473701194;
                //else if (i == 3) feature.RetentionTime = 0.010161342368506;
                //else if (i == 4) feature.RetentionTime = 0.010161342368506;
                //else if (i == 5) feature.RetentionTime = 0.0103226847370119;
                //else if (i == 6) feature.RetentionTime = 0.0153226847370119;
                //else if (i == 7) feature.RetentionTime = 0.0304840271055179;
                //else if (i == 8) feature.RetentionTime = 0.045161342368506;
                //else if (i == 9) feature.RetentionTime = 0.055161342368506;
                //else if (i == 10) feature.RetentionTime = 0.531135850274282;
                //else if (i == 11) feature.RetentionTime = 0.573893191352049;
                //else if (i == 12) feature.RetentionTime = 0.57744272345918;
                //else if (i == 13) feature.RetentionTime = 0.68391900613101;
                //else if (i == 14) feature.RetentionTime = 0.73716198773798;
                //else if (i == 15) feature.RetentionTime = 0.742294288480155;
                //else if (i == 16) featurek.RetentionTime = 0.744019881950916;
                //else if (i == 17) feature.RetentionTime = 0.745292016154085;
                //else if (i == 18) feature.RetentionTime = 0.745851887705712;
                //else if (i == 19) feature.RetentionTime = 0.747129719264279;
                //else if (i == 20) feature.RetentionTime = 0.748142949338496;
                //else if (i == 21) feature.RetentionTime = 0.87351403678606;
                //else if (i == 22) feature.RetentionTime = 0.873649564375605;
                //else if (i == 23) feature.RetentionTime = 0.879335269441756;
                //else if (i == 24) feature.RetentionTime = 0.925622781542433;
                //else if (i == 25) feature.RetentionTime = 0.92699596147872;
                //else if (i == 26) feature.RetentionTime = 0.934344949983866;
                //else if (i == 27) feature.RetentionTime = 0.948538238141336;
                //else if (i == 28) feature.RetentionTime = 0.965966440787351;
                //else if (i == 29) feature.RetentionTime = 0.97421559490525;

                //features.Add(feature);
            
            // Cluster!
            parameters.CentroidRepresentation = ClusterCentroidRepresentation.Mean;
            clustering.Parameters = parameters;
            List<UMCClusterLight> clusters = clustering.Cluster(features);

            Assert.IsNotNull(clusters);
            Console.WriteLine(clusters.Count);
            foreach (UMCClusterLight cluster in clusters)
            {
                Console.WriteLine(cluster.RetentionTime);
            }
        }


    }
}