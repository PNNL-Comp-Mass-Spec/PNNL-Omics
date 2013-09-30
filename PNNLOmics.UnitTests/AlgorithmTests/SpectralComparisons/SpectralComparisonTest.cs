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
        //[Test(Description = "Compares two spectra against each other.")]
        //[TestCase(@"..\..\..\TestFiles\SpectralTestData\spectra-DVEHPEAGVAFR-3726.txt",
        //            @"..\..\..\TestFiles\SpectralTestData\spectra-DVEHPEAGVAFR-3541.txt",
        //            SpectralComparison.NormalizedDotProduct)]
        //[TestCase(@"..\..\..\TestFiles\SpectralTestData\spectra-DVEHPEAGVAFR-3726.txt",
        //            @"..\..\..\TestFiles\SpectralTestData\spectra-DVEHPEAGVAFR-3541.txt",
        //            SpectralComparison.DotProduct)]
        //public void TestSpectralMethods(string pathX, string pathY, SpectralComparison comparerType)
        //{
        //    Console.WriteLine("{2}, Test: {0}\tcompared to\t{1}", pathX, pathY, comparerType);
        //    MSSpectra spectrumX = GetSpectra(pathX)[0];
        //    MSSpectra spectrumY = GetSpectra(pathY)[0];

        //    ISpectralComparer comparer = SpectralComparerFactory.CreateSpectraComparer(comparerType);

        //    double value = comparer.CompareSpectra(spectrumX, spectrumY);
        //    Console.WriteLine("{0}\t{1}", comparerType, value);
        //    Console.WriteLine();
        //}        
    }
}
