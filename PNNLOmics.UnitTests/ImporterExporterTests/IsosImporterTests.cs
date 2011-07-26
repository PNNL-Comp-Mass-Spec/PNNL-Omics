using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PNNLOmics.Utilities.Importers;
using PNNLOmics.Data.Features;
using PNNLOmics.IO.FileReaders;

namespace PNNLOmics.UnitTests.ImporterExporterTests
{
    [TestFixture]
    public class MSFeatureImporterTest
    {
        [Test]
        public void TestRead()
        {
            string path                              = FileReferences.IsosFile;
            MSFeatureLightFileReader     reader      = new MSFeatureLightFileReader();
            IEnumerable<MSFeatureLight>  features    = reader.ReadFile(path);                       
        }
    }
}
