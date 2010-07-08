using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PNNLOmics.Data.Features;
using PNNLOmics.Data;
using PNNLOmics.Algorithms;
using PNNLOmics.Algorithms.FeatureMatcher.Data;
using PNNLOmics.Utilities.Importers;

namespace PNNLOmics.UnitTests.AlgorithmTests
{
    [TestFixture]
    public class STACTests
    {
        [Test]
        public void executeSTACOnOrbitrapDataTest1()
        {
            List<UMC> umcList = new List<UMC>();
            List<MassTag> massTagList = new List<MassTag>();


            loadOrbitrapData(ref umcList, ref massTagList);

            Assert.AreEqual(14182, umcList.Count);
            Assert.AreEqual(36549, massTagList.Count);

            FeatureMatcherParameters fmParams = new FeatureMatcherParameters();
            fmParams.ShouldCalculateHistogramFDR = false;
            fmParams.ShouldCalculateShiftFDR = false;
            fmParams.ShouldCalculateSLiC = false;
            fmParams.ShouldCalculateSTAC = false;

            PNNLOmics.Algorithms.FeatureMatcher.FeatureMatcher<UMC, MassTag> fm = new PNNLOmics.Algorithms.FeatureMatcher.FeatureMatcher<UMC, MassTag>(umcList, massTagList, fmParams);
            fm.MatchFeatures();


            Assert.AreNotEqual(0, fm.MatchList.Count);

            //Assert.AreEqual(100, fm.MatchList.Count);

            //STACInformation stac = new STACInformation(useDriftDimension);

            //stac.PerformSTAC<UMC,MassTag>(
        }




        private void loadOrbitrapData(ref List<UMC> umcList, ref List<MassTag> massTagList)
        {
            string umcFilePath = FileReferences.OrbitrapUMCFile1;
            string massTagFilePath = FileReferences.OrbitrapMassTagFile;

            UMCImporter importer = new UMCImporter(umcFilePath, '\t');
            MassTagTextFileImporter mtImporter = new MassTagTextFileImporter(massTagFilePath);
            umcList = importer.Import();
            massTagList = mtImporter.Import();
        }


    }
}
