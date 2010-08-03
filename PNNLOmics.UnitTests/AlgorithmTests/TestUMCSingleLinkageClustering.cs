/*////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * 
 * Name:    Test UMC Single Linkage Clusterer 
 * File:    UMCSingleLinkageClustering.cs
 * Author:  Brian LaMarche 
 * Purpose: Tests for UMC Single Linkage Clustering 
 * 
 * Date:    6-9-2010
 * Revisions:
 *          6-9-2010 - BLL - Created unit tests.
 ////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
using System;
using System.Collections.Generic;

using PNNLOmics.Data.Features;
using PNNLOmics.Algorithms.FeatureClustering;

using NUnit.Framework;

namespace PNNLOmicsUnitTests
{
    /// <summary>
    /// Test Fixture for UMC Single Linkage Clustering
    /// </summary>
    [TestFixture]
    public class TestUMCSingleLinkageClustering
    {
        /// <summary>
        /// Black box test for a known UMC configuration.
        /// </summary>
        [Test]
        [Description("Black Box test to see if given a known configuration the right clusters will appear")]
        public void ClusterSimple()
        {
            /// 
            /// Create objects 
            /// 
            List<UMC> features = new List<UMC>();
            UMCSingleLinkageClusterer clusterer = new UMCSingleLinkageClusterer();
            List<UMCCluster> clusters = new List<UMCCluster>();

            /// Call clustering 
            clusterer.Cluster(features, clusters);

            /// 
            /// Examine clusters 
            /// 
        }
        /// <summary>
        /// Black box test for a known UMC configuration.
        /// </summary>
        [Test]
        [Description("No clusters should be performed when no features are loaded.")]
        public void ClusterNoData()
        {
            /// 
            /// Create objects 
            /// 
            List<UMC> features = new List<UMC>();
            UMCSingleLinkageClusterer clusterer = new UMCSingleLinkageClusterer();
            List<UMCCluster> clusters = new List<UMCCluster>();

            /// Call clustering 
            clusterer.Cluster(features, clusters);

            /// 
            Assert.IsNotNull(clusters);

            /// We should not have a 
            Assert.IsEmpty(clusters);
        }
        [Test]
        [Description("Clusters are null.")]
        public void ClusterNull()
        {
            throw new NotImplementedException("The Test has not been implemented yet.");
        }
        [Test]
        [Description("No clusters exist.")]
        public void ClusterNoClusterableData()
        {
            throw new NotImplementedException("The Test has not been implemented yet.");
        }
    }
}
