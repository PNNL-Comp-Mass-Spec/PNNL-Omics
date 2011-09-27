using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Diagnostics;
using PNNLOmics.Data.Constants;
using PNNLOmics.Data.Constants.Libraries;

namespace PNNLOmics.UnitTests.ConstantsTests
{
    class ConstantsTest
    {
        [Test]
        public void TestCyclingThroughDictionary()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Generating a List of keys
            List<double> massList = new List<double>();
            double returnedMass = 0;

            Array subAtomicParticleValues = Enum.GetValues(typeof(SubAtomicParticleName));
            foreach (SubAtomicParticleName enumSubAtomicParticle in subAtomicParticleValues)
            {
                returnedMass = Constants.SubAtomicParticles[enumSubAtomicParticle].MassMonoIsotopic;
                massList.Add(returnedMass);
            }

            Assert.AreEqual(0.00054857990943, massList[0]);
            Assert.AreEqual(1.00866491597, massList[1]);
            Assert.AreEqual(1.00727646677, massList[2]);


            stopWatch.Stop();
            Console.WriteLine("This took " + stopWatch.Elapsed + "seconds to TestCyclingThroughDictionary");
        }

        [Test]
        public void TestAminoAcid()//test amino acid calls
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //using a Char key with a dictionary
            string aminoAcidKey = "N";
            //Dictionary<string, Compound> aminoAcidsDictionary = AminoAcidLibrary.LoadAminoAcidData();

            double aminoAcidMass = Constants.AminoAcids[aminoAcidKey].MassMonoIsotopic;
            string aminoAcidName = Constants.AminoAcids[aminoAcidKey].Name;
            string aminoAcidFormula = Constants.AminoAcids[aminoAcidKey].ChemicalFormula;

            Assert.AreEqual(114.04292745124599, aminoAcidMass);
            Assert.AreEqual("Asparagine", aminoAcidName);
            Assert.AreEqual("C4H6N2O2", aminoAcidFormula);

            //using a Select Key and Enum
            double aminoAcidMass3 = Constants.AminoAcids[AminoAcidName.GlutamicAcid].MassMonoIsotopic;
            string aminoAcidName3 = Constants.AminoAcids[AminoAcidName.GlutamicAcid].Name;
            string aminoAcidFormula3 = Constants.AminoAcids[AminoAcidName.GlutamicAcid].ChemicalFormula;

            Assert.AreEqual(129.042593098113, aminoAcidMass3);
            Assert.AreEqual("GlutamicAcid", aminoAcidName3);
            Assert.AreEqual("C5H7NO3", aminoAcidFormula3);

            //getting the mass of a string of amino acids
            double massPeptide = 0;
            string peptideSequence = "NRTL";
            Array aminoacidValues = Enum.GetValues(typeof(AminoAcidName));

            for (int y = 0; y < peptideSequence.Length; y++)
            {
                massPeptide += Constants.AminoAcids[peptideSequence[y].ToString()].MassMonoIsotopic;// ,SelectLibrary.SelectAminoAcid);
            }

            Assert.AreEqual(massPeptide, 484.27578094385393);

            stopWatch.Stop();
            Console.WriteLine("This took " + stopWatch.Elapsed + "seconds to TestAminoAcid");
        }

        [Test]
        public void TestMonosaccharide() //test monosacharide calls
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //using a String Key with a dictionary
            string monosaccharideKey = "Hex";

            double monoSaccharideMass = Constants.Monosaccharides[monosaccharideKey].MassMonoIsotopic;
            string monoSaccharideName = Constants.Monosaccharides[monosaccharideKey].Name;
            string monoSaccharideFormula = Constants.Monosaccharides[monosaccharideKey].ChemicalFormula;

            Assert.AreEqual(162.05282343122502, monoSaccharideMass);
            Assert.AreEqual("Hexose", monoSaccharideName);
            Assert.AreEqual("C6H10O5", monoSaccharideFormula);

            //using a Select Key and Enum
            double monoSaccharideMass3 = Constants.Monosaccharides[MonosaccharideName.NeuraminicAcid].MassMonoIsotopic;
            string monoSaccharideName3 = Constants.Monosaccharides[MonosaccharideName.NeuraminicAcid].Name;
            string monoSaccharideFormula3 = Constants.Monosaccharides[MonosaccharideName.NeuraminicAcid].ChemicalFormula;

            Assert.AreEqual(291.09541652933802, monoSaccharideMass3);
            Assert.AreEqual("Neuraminic Acid", monoSaccharideName3);
            Assert.AreEqual("C11H17NO8", monoSaccharideFormula3);

            stopWatch.Stop();
            Console.WriteLine("This took " + stopWatch.Elapsed + "seconds to TestMonosaccharide");
        }

        [Test]
        public void TestElements()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //using a String Key with a dictionary
            string elementKey = "C";

            double elementMass = Constants.Elements[elementKey].MassMonoIsotopic;
            string elementName = Constants.Elements[elementKey].Name;
            string elementSymbol = Constants.Elements[elementKey].Symbol;

            Assert.AreEqual(12.0, elementMass);
            Assert.AreEqual("Carbon", elementName);
            Assert.AreEqual("C", elementSymbol);

            //using a Select Key and Enum
            double elementMass3 = Constants.Elements[ElementName.Hydrogen].MassMonoIsotopic;
            string elementName3 = Constants.Elements[ElementName.Hydrogen].Name;
            string elementSymbol3 = Constants.Elements[ElementName.Hydrogen].Symbol;

            Assert.AreEqual(1.00782503196, elementMass3);
            Assert.AreEqual("Hydrogen", elementName3);
            Assert.AreEqual("H", elementSymbol3);

            //isotopes abundance on earth
            double elementC12Mass = Constants.Elements[ElementName.Carbon].IsotopeDictionary["C12"].Mass;
            double elementC13Mass = Constants.Elements[ElementName.Carbon].IsotopeDictionary["C13"].Mass;

            double elementC12MassC12Abundance = Constants.Elements[ElementName.Carbon].IsotopeDictionary["C12"].NaturalAbundance;
            double elementC13MassC13Abundance = Constants.Elements[ElementName.Carbon].IsotopeDictionary["C13"].NaturalAbundance;

            double elementC12MassC12IsotopeNumber = Constants.Elements[ElementName.Carbon].IsotopeDictionary["C12"].IsotopeNumber;
            double elementC13MassC13IsotopeNumber = Constants.Elements[ElementName.Carbon].IsotopeDictionary["C13"].IsotopeNumber;

            Assert.AreEqual(12.0, elementC12Mass);
            Assert.AreEqual(13.0033548385, elementC13Mass);
            Assert.AreEqual(0.98892228, elementC12MassC12Abundance);
            Assert.AreEqual(0.01107828, elementC13MassC13Abundance);
            Assert.AreEqual(12, elementC12MassC12IsotopeNumber);
            Assert.AreEqual(13, elementC13MassC13IsotopeNumber);


            //average mass
            double averageMass = Constants.Elements[ElementName.Carbon].MassAverage;
            double averageMassUncertainty = Constants.Elements[ElementName.Carbon].MassAverageUncertainty;

            double massHigh = averageMass + averageMassUncertainty;
            double massLow = averageMass - averageMassUncertainty;

            Assert.AreEqual(12.0107d, averageMass);
            Assert.AreEqual(0.0008d, averageMassUncertainty);
            Assert.AreEqual(12.0115d, massHigh);
            Assert.AreEqual(12.0099d, massLow);

            //last element
            double elementXMass = Constants.Elements[ElementName.Generic].MassMonoIsotopic;
            double elementX999Isotope = Constants.Elements[ElementName.Generic].IsotopeDictionary["X999"].NaturalAbundance;
            Isotope newIsotope = new Isotope(1000, 1000.500, 0.75);
            Constants.Elements[ElementName.Generic].IsotopeDictionary.Add("X1000", newIsotope);
            
            double elementX1000Isotope = Constants.Elements[ElementName.Generic].IsotopeDictionary["X1000"].NaturalAbundance;

            //string elementKeyXList
            Constants.Elements[ElementName.Generic].IsotopeDictionary.Remove("X999");
            Constants.Elements[ElementName.Generic].IsotopeDictionary.Add("newName", newIsotope);

            Assert.AreEqual(999, elementXMass);
            Assert.AreEqual(0.5, elementX999Isotope);
            Assert.AreEqual(0.75, elementX1000Isotope);
            //Assert.AreEqual(elementKey, "newName");
            
            stopWatch.Stop();
            Console.WriteLine("This took " + stopWatch.Elapsed + "seconds to TestElements");
        }

        [Test]
        public void TestSubAtomicParticle()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //using a String Key with a dictionary
            string atomKey = "e";

            double atomMass = Constants.SubAtomicParticles[atomKey].MassMonoIsotopic;
            string atomName = Constants.SubAtomicParticles[atomKey].Name;
            string atomSymbol = Constants.SubAtomicParticles[atomKey].Symbol;

            Assert.AreEqual(0.00054857990943, atomMass);
            Assert.AreEqual("Electron", atomName);
            Assert.AreEqual("e", atomSymbol);

            //using a Select Key and Enum
            double atomMass3 = Constants.SubAtomicParticles[SubAtomicParticleName.Proton].MassMonoIsotopic;
            string atomName3 = Constants.SubAtomicParticles[SubAtomicParticleName.Proton].Name;
            string atomSymbol3 = Constants.SubAtomicParticles[SubAtomicParticleName.Proton].Symbol;

            Assert.AreEqual(1.00727646677, atomMass3);
            Assert.AreEqual("Proton", atomName3);
            Assert.AreEqual("p", atomSymbol3);

            stopWatch.Stop();
            Console.WriteLine("This took " + stopWatch.Elapsed + "seconds to TestSubAtomicParticle");
        }

        [Test]
        public void TestMiscellaneousMatter()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //using a String Key with a dictionary
            string otherKey = Constants.MiscellaneousMatter[MiscellaneousMatterName.Aldehyde].Symbol;//"Aldehyde";

            double otherMass = Constants.MiscellaneousMatter[otherKey].MassMonoIsotopic;
            string otherName = Constants.MiscellaneousMatter[otherKey].Name;
            string otherFormula = Constants.MiscellaneousMatter[otherKey].ChemicalFormula;

            Assert.AreEqual(18.010564686245, otherMass);
            Assert.AreEqual("Aldehyde", otherName);
            Assert.AreEqual("H2O", otherFormula);


            //using a Select Key and Enum 
            double otherMass3 = Constants.MiscellaneousMatter[MiscellaneousMatterName.Ammonia].MassMonoIsotopic;
            double otherMass4 = Constants.MiscellaneousMatter[MiscellaneousMatterName.Sulfate].MassMonoIsotopic;

            Assert.AreEqual(17.026549103298, otherMass3);
            Assert.AreEqual(127.9237999523, otherMass4);

            double otherMassAmino = otherMass - otherMass3;
            Assert.AreEqual(0.98401558294700209, otherMassAmino);

            double otherMass5 = Constants.MiscellaneousMatter[MiscellaneousMatterName.Water].MassMonoIsotopic
                            - Constants.MiscellaneousMatter[MiscellaneousMatterName.Ammonia].MassMonoIsotopic;

            Assert.AreEqual(0.98401558294700209, otherMass5);

            stopWatch.Stop();
            Console.WriteLine("This took " + stopWatch.Elapsed + "seconds to TestMiscellaneousMatter");
        }

        [Test]
        public void TestCrossRing()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //using a String Key with a dictionary
            string crossRingKey = Constants.CrossRings[CrossRingName.CRFNeu5Ac_03_X1].Symbol;//="N-03X1"//"CRFNeu5Ac_03_X1";

            double crossRingMass = Constants.CrossRings[crossRingKey].MassMonoIsotopic;
            string crossRingFormula = Constants.CrossRings[crossRingKey].ChemicalFormula;
            string crossRingName = Constants.CrossRings[crossRingKey].Name;

            Assert.AreEqual(100.01604399481499, crossRingMass);
            //Assert.AreEqual(112.03985344382801, crossRingMass);
            Assert.AreEqual("C4H4O3", crossRingFormula);
            Assert.AreEqual("CRFNeu5Ac_03_X1", crossRingName);

            //using a Select Key and Enum
            double crossRingMass3 = Constants.CrossRings[CrossRingName.CRFHex_02_A2].MassMonoIsotopic;
            string crossRingFormula3 = Constants.CrossRings[CrossRingName.CRFHex_02_A2].ChemicalFormula;
            string crossRingName3 = Constants.CrossRings[CrossRingName.CRFHex_02_A2].Name;

            Assert.AreEqual(102.031694058735, crossRingMass3);
            Assert.AreEqual("C4H6O3", crossRingFormula3);
            Assert.AreEqual("CRFHex_02_A2", crossRingName3);

            stopWatch.Stop();
            Console.WriteLine("This took " + stopWatch.Elapsed + "seconds to TestCrossRing");
        }

        [Test]
        public void TestMasses()
        {
            
            AminoAcidLibrary newlibrary1 = new AminoAcidLibrary();
            //Dictionary<string, Compound> dictionaryIn1 = newlibrary1.LoadLibrary();
            //foreach (KeyValuePair<string, Compound> matterObject in dictionaryIn1)
            //{
            //    totalMass += matterObject.Value.MassMonoIsotopic;
            //    textM = matterObject.Value.MassMonoIsotopic.ToString() + "," + matterObject.Value.Symbol.ToString() + "," + matterObject.Value.Name.ToString() + Environment.NewLine;
            //    //Console.Write(textM);
            //}

            //Assert.AreEqual(totalMass, 2475.1616262881598);

            CrossRingLibrary newlibrary2 = new CrossRingLibrary();
            //Dictionary<string, Compound> dictionaryIn2 = newlibrary2.LoadLibrary();
            //foreach (KeyValuePair<string, Compound> matterObject in dictionaryIn2)
            //{
            //    totalMass += matterObject.Value.MassMonoIsotopic;
            //    textM = matterObject.Value.MassMonoIsotopic.ToString() + "," + matterObject.Value.Symbol.ToString() + "," + matterObject.Value.Name.ToString() + Environment.NewLine;
            //    //Console.Write(textM);
            //}

            //Assert.AreEqual(totalMass, 1432.3950300356437);

            ElementLibrary newlibrary3 = new ElementLibrary();
            //Dictionary<string, Element> dictionaryIn3 = newlibrary3.LoadLibrary();
            //foreach (KeyValuePair<string, Element> matterObject in dictionaryIn3)
            //{
            //    totalMass += matterObject.Value.MassMonoIsotopic;
            //    textM = matterObject.Value.MassMonoIsotopic.ToString() + "," + matterObject.Value.Symbol.ToString() + "," + matterObject.Value.Name.ToString() + Environment.NewLine;
            //    //Console.Write(textM);
            //}

            //Assert.AreEqual(totalMass, 12899.589616552506);

            MiscellaneousMatterLibrary newlibrary4 = new MiscellaneousMatterLibrary();
            //Dictionary<string, Compound> dictionaryIn4 = newlibrary4.LoadLibrary();
            //foreach (KeyValuePair<string, Compound> matterObject in dictionaryIn4)
            //{
            //    totalMass += matterObject.Value.MassMonoIsotopic;
            //    textM = matterObject.Value.MassMonoIsotopic.ToString() + "," + matterObject.Value.Symbol.ToString() + "," + matterObject.Value.Name.ToString() + Environment.NewLine;
            //    //Console.Write(textM);
            //}

            //Assert.AreEqual(totalMass, 295.99644294548898);

            MonosaccharideLibrary newlibrary5 = new MonosaccharideLibrary();
            //Dictionary<string, Compound> dictionaryIn5 = newlibrary5.LoadLibrary();
            //foreach (KeyValuePair<string, Compound> matterObject in dictionaryIn5)
            //{
            //    totalMass += matterObject.Value.MassMonoIsotopic;
            //    textM = matterObject.Value.MassMonoIsotopic.ToString() + "," + matterObject.Value.Symbol.ToString() + "," + matterObject.Value.Name.ToString() + Environment.NewLine;
            //    //Console.Write(textM);
            //}

            //Assert.AreEqual(totalMass, 1687.5704282280092);

            SubAtomicParticleLibrary newlibrary6 = new SubAtomicParticleLibrary();
            //Dictionary<string, SubAtomicParticle> dictionaryIn6 = newlibrary6.LoadLibrary();
            //foreach (KeyValuePair<string, SubAtomicParticle> matterObject in dictionaryIn6)
            //{
            //    totalMass += matterObject.Value.MassMonoIsotopic;
            //    textM = matterObject.Value.MassMonoIsotopic.ToString() + "," + matterObject.Value.Symbol.ToString() + "," + matterObject.Value.Name.ToString() + Environment.NewLine;
            //    //Console.Write(textM);
            //}

            //Assert.AreEqual(totalMass, 2.0164899626494304);
        }
    }
}
