using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PNNLOmics.Utilities.Importers;
using PNNLOmics.Data.Features;

namespace PNNLOmics.UnitTests.ImporterExporterTests
{
    [TestFixture]
    public class UMCImporterTests
    {
        [Test]
        public void test1()
        {
           string umcFilePath = FileReferences.OrbitrapUMCFile1;

            UMCImporter importer = new UMCImporter(umcFilePath, '\t');
            List<UMC> umcList = importer.Import();
            Assert.AreEqual(14182, umcList.Count);

            UMC testUMC = umcList[10];
            Assert.AreEqual(10,testUMC.ID);
            Assert.AreEqual(4740, testUMC.ScanLCStart);
            Assert.AreEqual(4831, testUMC.ScanLCEnd);
            Assert.AreEqual(4740, testUMC.ScanLC);
            Assert.AreEqual(0.2696, testUMC.NET);
            Assert.AreEqual(400.23241, testUMC.MassMonoisotopic);
            Assert.AreEqual(0.000211, testUMC.MassMonoisotopicStandardDeviation);
            Assert.AreEqual(400.23214262033, testUMC.MassMonoisotopicMinimum);
            Assert.AreEqual(400.232691086785, testUMC.MassMonoisotopicMaximum);
            Assert.AreEqual(4431786, testUMC.Abundance);
            Assert.AreEqual(1, testUMC.ChargeState);
            Assert.AreEqual(1, testUMC.ChargeMinimum);
            Assert.AreEqual(1, testUMC.ChargeMaximum);
            Assert.AreEqual(401.239687, testUMC.MZ);
            Assert.AreEqual(0.05, testUMC.FitScoreAverage);
        }


    }
}
