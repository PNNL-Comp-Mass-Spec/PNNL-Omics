using System.Collections.Generic;
using NUnit.Framework;
using PNNLOmics.Data;
using PNNLOmicsIO.IO;

namespace PNNLOmics.UnitTests.AlgorithmTests.SpectralComparisons
{

    [TestFixture]
    public class SpectralComparisonTest
    {

        private List<MSSpectra> GetSpectra(string path)
        {
            var spectrum        = new MSSpectra();
            spectrum.Peptides         = new List<Peptide>();
            IMsMsSpectraReader reader = new MgfFileReader();
            var spectra   =  reader.Read(path);         
   
            return spectra;
        }
               
    }
}
