using System;
using System.Collections.Generic;

using NUnit.Framework;

using PNNLOmics.Data;
using PNNLOmics.Data.Features;

using PNNLOmics.Algorithms;
using PNNLOmics.Algorithms.FeatureClustering;

using PNNLOmics.Utilities.Importers;

namespace PNNLOmics.UnitTests.DataTests.Features
{
    [TestFixture]
    public class FeatureTests
    {
        /// <summary>
        ///  Part of a clustering test to make sure when sending a 
        ///  null list the clustering algorithm fails.
        /// </summary>
        [Test]
        public void PPMCalculations()
        {
            double massX        = 400.0;
            double massY        = 400.1;
            double ppm          = Feature.ComputeMassPPMDifference(massX, massY);
            double massYdelta   = Feature.ComputeDaDifferenceFromPPM(massX, ppm);

            Console.WriteLine("PPM {0}", ppm);

            Assert.AreEqual(massY, massYdelta);
        }       
    }
}
