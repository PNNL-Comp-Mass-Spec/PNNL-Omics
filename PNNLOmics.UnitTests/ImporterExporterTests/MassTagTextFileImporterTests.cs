using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PNNLOmics.Utilities.Importers;
using PNNLOmics.Data;

namespace PNNLOmics.UnitTests.ImporterExporterTests
{
    [TestFixture]
    public class MassTagTextFileImporterTests
    {
        [Test]
        public void importMassTagsFromTextFileTest1()
        {
            string massTagTextFilePath = FileReferences.OrbitrapMassTagFile;
            MassTagTextFileImporter importer = new MassTagTextFileImporter(massTagTextFilePath);

            List<MassTag> massTagList = importer.Import();
            Assert.AreEqual(36549, massTagList.Count);
            
            MassTag testMT = massTagList[4];

            Assert.AreEqual(24696, testMT.ID);
            Assert.AreEqual(3072.5514762, testMT.MassMonoisotopic);
            Assert.AreEqual(0.6527005, testMT.NET);
            Assert.AreEqual(6430, testMT.ObservationCount);
           
            //From the .txt file:
            //Mass_Tag_ID	Monoisotopic_Mass	NET	StD_GANET	Cnt_GANET	Peptide_Obs_Count_Passing_Filter	High_Normalized_Score	High_Discriminant_Score	High_Peptide_Prophet_Probability	Mod_Count	Cleavage_State_Max	ObsCount_CS1	ObsCount_CS2	ObsCount_CS3	PepProphet_FScore_Avg_CS1	PepProphet_FScore_Avg_CS2	PepProphet_FScore_Avg_CS3	PassesFilters	Peptide	ProteinReference	Ref_ID
            //Row 5 (4 if zero-based): 24696	3072.5514762	0.6527005	0.02108081	1852	6430	9.71888	0.999983	1	0	2	7995	7995	7448	4.19688	4.19688	4.322828		TYDADRDGFVISGGGGILIVEELEHALAR	SO_3072	1923

        }


    }
}
