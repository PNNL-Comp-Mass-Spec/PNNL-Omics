using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PNNLOmics.Data.Constants.ConstantsDataLayer;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;


namespace PNNLOmics.UnitTests.ConstantsTests
{
    class ConstantsTest
    {
        [Test]
        public void TestCyclingThroughDictionary()
        {
            //Generating a List of keys
            LibrarySubAtomicParticle particleLibrary = Constants.GetLibrarySubAtomicParticle(SelectLibrary.SelectSubAtomicParticle);
            Dictionary<int, string> cycleConverterDictionary = particleLibrary.ConstantsEnumDictionary;
            Dictionary<string, SubAtomicParticle> atomGeneratorDictionary = particleLibrary.ConstantsDictionary;

            List<double> massList = new List<double>();
            for (int i = 0; i < cycleConverterDictionary.Count; i++)
            {
                double returnedMass = atomGeneratorDictionary[cycleConverterDictionary[i]].MassMonoIsotopic;
                massList.Add(returnedMass);
            }

            Assert.AreEqual(0.00054857990943, massList[0]);
            Assert.AreEqual(1.00866491597, massList[1]);
            Assert.AreEqual(1.00727646677, massList[2]);
        }

        [Test]
        public void TestAminoAcid()//test amino acid calls
        {
            //using a Char key with a dictionary
            string aminoAcidKey = "N";
            Dictionary<string, Compound> aminoAcidsDictionary = AminoAcidLibrary.LoadAminoAcidData();

            double aminoAcidMass = aminoAcidsDictionary[aminoAcidKey].MassMonoIsotopic;
            string aminoAcidName = aminoAcidsDictionary[aminoAcidKey].Name;
            string aminoAcidFormula = aminoAcidsDictionary[aminoAcidKey].ChemicalFormula;

            Assert.AreEqual(114.04292745124599, aminoAcidMass);
            Assert.AreEqual("Asparagine", aminoAcidName);
            Assert.AreEqual("C4H6N2O2", aminoAcidFormula);

            double aminoAcidMass2 = Constants.GetMassMonoisotopic(aminoAcidKey, SelectLibrary.SelectAminoAcid);
            string aminoAcidName2 = Constants.GetName(aminoAcidKey, SelectLibrary.SelectAminoAcid);
            string aminoAcidFormula2 = Constants.GetFormula(aminoAcidKey, SelectLibrary.SelectAminoAcid);

            Assert.AreEqual(114.04292745124599, aminoAcidMass2);
            Assert.AreEqual("Asparagine", aminoAcidName2);
            Assert.AreEqual("C4H6N2O2", aminoAcidFormula2);

            //getting the mass of a string of amino acids
            double massPeptide = 0;
            string peptideSequence = "NRTL";
            for (int y = 0; y < peptideSequence.Length; y++)
            {
                //massPeptide += AminoAcidStaticLibrary.GetMonoisotopicMass(peptideSequence[y].ToString());
                massPeptide += Constants.GetMassMonoisotopic(peptideSequence[y].ToString(), SelectLibrary.SelectAminoAcid);
            }

            Assert.AreEqual(massPeptide, 484.27578094385393);

            //using a Select Key and Enum
            double aminoAcidMass3 = Constants.GetMassMonoisotopic(SelectAminoAcid.GlutamicAcid);
            string aminoAcidName3 = Constants.GetName(SelectAminoAcid.GlutamicAcid);
            string aminoAcidFormula3 = Constants.GetFormula(SelectAminoAcid.GlutamicAcid);

            Assert.AreEqual(129.042593098113, aminoAcidMass3);
            Assert.AreEqual("GlutamicAcid", aminoAcidName3);
            Assert.AreEqual("C5H7NO3", aminoAcidFormula3);
        }

        [Test]
        public void TestMonosaccharide() //test monosacharide calls
        {
            //using a String Key with a dictionary
            string monosaccharideKey = "Hex";
            //Dictionary<string, Monosaccharide> monosacchaideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            Dictionary<string, Compound> monosacchaideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();


            double monoSaccharideMass = monosacchaideDictionary[monosaccharideKey].MassMonoIsotopic;
            string monoSaccharideName = monosacchaideDictionary[monosaccharideKey].Name;
            string monoSaccharideFormula = monosacchaideDictionary[monosaccharideKey].ChemicalFormula;

            Assert.AreEqual(162.05282343122502, monoSaccharideMass);
            Assert.AreEqual("Hexose", monoSaccharideName);
            Assert.AreEqual("C6H10O5", monoSaccharideFormula);

            //one line implementaiton
            double monoSaccharideMass2 = Constants.GetMassMonoisotopic(monosaccharideKey, SelectLibrary.SelectMonosaccharide);
            string monoSaccharideName2 = Constants.GetName(monosaccharideKey, SelectLibrary.SelectMonosaccharide);
            string monoSaccharideFormula2 = Constants.GetFormula(monosaccharideKey, SelectLibrary.SelectMonosaccharide);

            Assert.AreEqual(162.05282343122502, monoSaccharideMass2);
            Assert.AreEqual("Hexose", monoSaccharideName2);
            Assert.AreEqual("C6H10O5", monoSaccharideFormula2);

            //using a Select Key and Enum
            double monoSaccharideMass3 = Constants.GetMassMonoisotopic(SelectMonosaccharide.NeuraminicAcid);
            string monoSaccharideName3 = Constants.GetName(SelectMonosaccharide.NeuraminicAcid);
            string monoSaccharideFormula3 = Constants.GetFormula(SelectMonosaccharide.NeuraminicAcid);

            Assert.AreEqual(291.09541652933802, monoSaccharideMass3);
            Assert.AreEqual("Neuraminic Acid", monoSaccharideName3);
            Assert.AreEqual("C11H17NO8", monoSaccharideFormula3);
        }

        [Test]
        public void TestElements()
        {
            //using a String Key with a dictionary
            string elementKey = "C";
            Dictionary<string, Element> elementDictionary = ElementLibrary.LoadElementData();

            //isotopes abundance on earth
            double elementC12Mass = elementDictionary[elementKey].IsotopeDictionary["C12"].Mass;
            double elementC13Mass = elementDictionary[elementKey].IsotopeDictionary["C13"].Mass;
            double elementC12Abund = elementDictionary[elementKey].IsotopeDictionary["C12"].NaturalAbundance;
            double elementC13Abund = elementDictionary[elementKey].IsotopeDictionary["C13"].NaturalAbundance;

            Assert.AreEqual(elementC12Mass, 12.0);
            Assert.AreEqual(elementC13Mass, 13.0033548385);
            Assert.AreEqual(elementC12Abund, 0.98892228);
            Assert.AreEqual(elementC13Abund, 0.01107828);

            double elementMass = elementDictionary[elementKey].MassMonoIsotopic;
            string elementName = elementDictionary[elementKey].Name;
            string elementSymbol = elementDictionary[elementKey].Symbol;

            Assert.AreEqual(12.0, elementMass);
            Assert.AreEqual("Carbon", elementName);
            Assert.AreEqual("C", elementSymbol);

            //One line implementation
            double elementMass2 = Constants.GetMassMonoisotopic(elementKey, SelectLibrary.SelectElement);
            string elementName2 = Constants.GetName(elementKey, SelectLibrary.SelectElement);
            string elementSymbol2 = Constants.GetSymbol(elementKey, SelectLibrary.SelectElement);

            Assert.AreEqual(12.0, elementMass2);
            Assert.AreEqual("Carbon", elementName2);
            Assert.AreEqual("C", elementSymbol2);

            //using a Select Key and Enum
            double elementMass3 = Constants.GetMassMonoisotopic(SelectElement.Hydrogen);
            string elementName3 = Constants.GetName(SelectElement.Hydrogen);
            string elementSymbol3 = Constants.GetSymbol(SelectElement.Hydrogen);

            Assert.AreEqual(1.00782503196, elementMass3);
            Assert.AreEqual("Hydrogen", elementName3);
            Assert.AreEqual("H", elementSymbol3);
        }

        [Test]
        public void TestAtom()
        {
            //using a String Key with a dictionary
            string atomKey = "e";
            Dictionary<string, SubAtomicParticle> atomDictionary = SubAtomicParticleLibrary.LoadSubAtomicParticleData();

            double atomMass = atomDictionary[atomKey].MassMonoIsotopic;
            string atomName = atomDictionary[atomKey].Name;
            string atomSymbol = atomDictionary[atomKey].Symbol;

            Assert.AreEqual(0.00054857990943, atomMass);
            Assert.AreEqual("Electron", atomName);
            Assert.AreEqual("e", atomSymbol);

            //one line implementation
            double atomMass2 = Constants.GetMassMonoisotopic(atomKey, SelectLibrary.SelectSubAtomicParticle);
            string atomName2 = Constants.GetName(atomKey, SelectLibrary.SelectSubAtomicParticle);
            string atomSymbol2 = Constants.GetSymbol(atomKey, SelectLibrary.SelectSubAtomicParticle);

            Assert.AreEqual(0.00054857990943, atomMass2);
            Assert.AreEqual("Electron", atomName2);
            Assert.AreEqual("e", atomSymbol2);

            //using a Select Key and Enum
            double atomMass3 = Constants.GetMassMonoisotopic(SelectSubAtomicParticle.Proton);
            string atomName3 = Constants.GetName(SelectSubAtomicParticle.Proton);
            string atomSymbol3 = Constants.GetSymbol(SelectSubAtomicParticle.Proton);

            Assert.AreEqual(1.00727646677, atomMass3);
            Assert.AreEqual("Proton", atomName3);
            Assert.AreEqual("p", atomSymbol3);
        }

        [Test]
        public void TestMiscellaneousMatter()
        {
            //using a String Key with a dictionary
            string otherKey = "Aldehyde";
            Dictionary<string, Compound> MiscellaneousMatterDictionary = MiscellaneousMatterLibrary.LoadMiscellaneousMatterData();

            double otherMass = MiscellaneousMatterDictionary[otherKey].MassMonoIsotopic;
            string otherName = MiscellaneousMatterDictionary[otherKey].Name;
            string otherFormula = MiscellaneousMatterDictionary[otherKey].ChemicalFormula;

            Assert.AreEqual(18.010564686245, otherMass);
            Assert.AreEqual("Aldehyde", otherName);
            Assert.AreEqual("H2O", otherFormula);

            //One line implememtation
            double otherMass2 = Constants.GetMassMonoisotopic(otherKey, SelectLibrary.SelectMiscellaneousMatter);
            string otherName2 = Constants.GetName(otherKey, SelectLibrary.SelectMiscellaneousMatter);
            string otherFormula2 = Constants.GetFormula(otherKey, SelectLibrary.SelectMiscellaneousMatter);

            Assert.AreEqual(18.010564686245, otherMass2);
            Assert.AreEqual("Aldehyde", otherName2);
            Assert.AreEqual("H2O", otherFormula2);

            //using a Select Key and Enum 
            double otherMass3 = Constants.GetMassMonoisotopic(SelectMiscellaneousMatter.Ammonia);
            double otherMass4 = Constants.GetMassMonoisotopic(SelectMiscellaneousMatter.Sulfate);

            Assert.AreEqual(17.026549103298, otherMass3);
            Assert.AreEqual(127.9237999523, otherMass4);

            double otherMassAmino = otherMass - otherMass3;
            Assert.AreEqual(0.98401558294700209, otherMassAmino);

            double otherMass5 = Constants.GetMassMonoisotopic(SelectMiscellaneousMatter.Water)
                            - Constants.GetMassMonoisotopic(SelectMiscellaneousMatter.Ammonia);

            Assert.AreEqual(0.98401558294700209, otherMass5);
        }

        [Test]
        public void TestCrossRing()
        {
            //using a String Key with a dictionary
            string crossRingKey = "CRFNeu5Ac_03_X1";
            Dictionary<string, Compound> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();

            double crossRingMass = CrossRingDictionary[crossRingKey].MassMonoIsotopic;
            string crossRingFormula = CrossRingDictionary[crossRingKey].ChemicalFormula;
            string crossRingName = CrossRingDictionary[crossRingKey].Name;

            Assert.AreEqual(100.01604399481499, crossRingMass);
            Assert.AreEqual("C4H4O3", crossRingFormula);
            Assert.AreEqual("CRFNeu5Ac_03_X1", crossRingName);

            //one line implementation
            double crossRingMass2 = Constants.GetMassMonoisotopic(crossRingKey, SelectLibrary.SelectCrossRing);
            string crossRingFormula2 = Constants.GetFormula(crossRingKey, SelectLibrary.SelectCrossRing);
            string crossRingName2 = Constants.GetName(crossRingKey, SelectLibrary.SelectCrossRing);

            Assert.AreEqual(100.01604399481499, crossRingMass2);
            Assert.AreEqual("C4H4O3", crossRingFormula2);
            Assert.AreEqual("CRFNeu5Ac_03_X1", crossRingName2);

            //using a Select Key and Enum
            double crossRingMass3 = Constants.GetMassMonoisotopic(SelectCrossRing.CRFHex_02_A2);
            string crossRingFormula3 = Constants.GetFormula(SelectCrossRing.CRFHex_02_A2);
            string crossRingName3 = Constants.GetName(SelectCrossRing.CRFHex_02_A2);

            Assert.AreEqual(102.031694058735, crossRingMass3);
            Assert.AreEqual("C4H6O3", crossRingFormula3);
            Assert.AreEqual("CRFHex_02_A2", crossRingName3);
        }
    }
}
