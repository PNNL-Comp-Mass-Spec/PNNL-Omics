using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PNNLOmics.Data.Features;
using PNNLOmics.Data;
using PNNLOmics.Algorithms.FeatureMatcher.Data;
using PNNLOmics.Utilities.Importers;

namespace PNNLOmics.UnitTests.AlgorithmTests
{
    [TestFixture]
    public class STACTests
    {
        [Test]
        public void orbitrapDataStandardTest1()
        {
            List<UMC> umcList = new List<UMC>();
            List<MassTag> massTagList = new List<MassTag>();

            List<FeatureMatch<UMC, MassTag>> featureMatchList = new List<FeatureMatch<UMC, MassTag>>();
          
            loadOrbitrapData(umcList,massTagList);

            bool useDriftDimension = false;
            STACInformation stac = new STACInformation(useDriftDimension);

            //stac.PerformSTAC<UMC,MassTag>(
        }

        
        
        
        private void loadOrbitrapData(List<UMC> umcList, List<MassTag> massTagList)
        {


            //TODO: Gord - add code for importing UMC files. 
            //TODO: Gord - add code for importing MassTag data from a .txt file. 


            string umcFilePath = FileReferences.OrbitrapUMCFile1;
            string massTagFilePath = FileReferences.OrbitrapMassTagFile;

            UMCImporter importer = new UMCImporter(umcFilePath, '\t');
            umcList =  importer.Import();

        }


    }
}
