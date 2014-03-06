using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PNNLOmics.Data.Features;
using System.IO;
using PNNLOmics.Algorithms.Distance;
using PNNLOmics.Algorithms.FeatureClustering;
using PNNLOmics.Algorithms.SpectralComparisons;
using PNNLOmics.Algorithms.SpectralProcessing;
using PNNLOmics.Data;
using PNNLOmicsIO.IO;

namespace PNNLOmics.UnitTests.AlgorithmTests.SpectralComparisons
{

    [TestFixture]
    public class SpectralComparisonTest
    {

        private List<MSSpectra> GetSpectra(string path)
        {
            MSSpectra spectrum        = new MSSpectra();
            spectrum.Peptides         = new List<Peptide>();
            IMsMsSpectraReader reader = new MgfFileReader();
            List<MSSpectra> spectra   =  reader.Read(path);         
   
            return spectra;
        }
               
    }
}
