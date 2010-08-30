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
        public void TestAminoAcid()//test amino acid calls
        {
            char aminoAcidKey = 'N';
            //dictionary implementation
            Dictionary<char, AminoAcid> aminoAcidsDictionary = AminoAcidLibrary.LoadAminoAcidData();
            double mass = aminoAcidsDictionary[aminoAcidKey].MonoIsotopicMass;
            string name = aminoAcidsDictionary[aminoAcidKey].Name;
            string formula = aminoAcidsDictionary[aminoAcidKey].ChemicalFormula;

            Assert.AreEqual(114.04292745124599, mass);
            Assert.AreEqual("Asparagine", name);
            Assert.AreEqual("C4H6N2O2", formula);
            
            //one line implementation
            double mass2 = AminoAcidStaticLibrary.GetMonoisotopicMass(aminoAcidKey);
            string name2 = AminoAcidStaticLibrary.GetName(aminoAcidKey);
            string formula2 = AminoAcidStaticLibrary.GetFormula(aminoAcidKey);

            Assert.AreEqual(114.04292745124599, mass2);
            Assert.AreEqual("Asparagine", name2);
            Assert.AreEqual("C4H6N2O2", formula2);
                
            double massPeptide = 0;
            string peptideSequence = "NRTL";
            for (int y = 0; y < peptideSequence.Length; y++)
            {
                massPeptide += AminoAcidStaticLibrary.GetMonoisotopicMass(peptideSequence[y]);
            }//massPeptide = 484.27578094385393

            Assert.AreEqual(massPeptide, 484.27578094385393);         
        }

        [Test]
        public void TestMonosaccharide() //test monosacharide calls
        {
            string monosaccharideKey = "Hex";
            //dictionary implementation
            Dictionary<string, Monosaccharide> monosacchaideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            double mass = monosacchaideDictionary[monosaccharideKey].MonoIsotopicMass;
            string name = monosacchaideDictionary[monosaccharideKey].Name;
            string formula = monosacchaideDictionary[monosaccharideKey].ChemicalFormula;

            Assert.AreEqual(162.05282343122502, mass);
            Assert.AreEqual("Hexose", name);
            Assert.AreEqual("C6H10O5", formula);

            //one line implementaiton
            double mass2 = MonosaccharideStaticLibrary.GetMonoisotopicMass(monosaccharideKey);
            string name2 = MonosaccharideStaticLibrary.GetName(monosaccharideKey);
            string formula2 = MonosaccharideStaticLibrary.GetFormula(monosaccharideKey);

            Assert.AreEqual(162.05282343122502, mass2);
            Assert.AreEqual("Hexose", name2);
            Assert.AreEqual("C6H10O5", formula2);
        }

        [Test]
        public void TestElements()
        {
            string elementKey = "C";
            //dictionary implementation

            Dictionary<string, Element> elementDictionary = ElementLibrary.LoadElementData();
            double elementC12Mass = elementDictionary[elementKey].IsotopeDictionary["C12"].Mass;
            double elementC13Mass = elementDictionary[elementKey].IsotopeDictionary["C13"].Mass;
            double elementC12Abund = elementDictionary[elementKey].IsotopeDictionary["C12"].NaturalAbundance;
            double elementC13Abund = elementDictionary[elementKey].IsotopeDictionary["C13"].NaturalAbundance;

            Assert.AreEqual(elementC12Mass, 12.0);
            Assert.AreEqual(elementC13Mass, 13.0033548385);
            Assert.AreEqual(elementC12Abund, 0.98892228);
            Assert.AreEqual(elementC13Abund, 0.01107828);

            double elementMass = elementDictionary[elementKey].MonoIsotopicMass;
            string elementName = elementDictionary[elementKey].Name;
            string elementSymbol = elementDictionary[elementKey].Symbol;

            Assert.AreEqual(12.0, elementMass);
            Assert.AreEqual("Carbon", elementName);
            Assert.AreEqual("C", elementSymbol);

            //One line implementation

            double elementMass2 = ElementStaticLibrary.GetMonoisotopicMass(elementKey);
            string elementName2 = ElementStaticLibrary.GetName(elementKey);
            string elementSymbol2 = ElementStaticLibrary.GetSymbol(elementKey);

            Assert.AreEqual(12.0, elementMass2);
            Assert.AreEqual("Carbon", elementName2);
            Assert.AreEqual("C", elementSymbol2);
        }

        [Test]
        public void TestAtom()
        {
            string atomKey = "e";
            //dictinoary implementation

            Dictionary<string, Atom> atomDictionary = AtomLibrary.LoadAtomicData();
            double atomMass = atomDictionary[atomKey].MonoIsotopicMass;
            string atomName = atomDictionary[atomKey].Name;
            string atomSymbol = atomDictionary[atomKey].Symbol;

            Assert.AreEqual(0.00054857990943, atomMass);
            Assert.AreEqual("Electron", atomName);
            Assert.AreEqual("e", atomSymbol);

            //one line implementation
            double atomMass2 = AtomStaticLibrary.GetMonoisotopicMass(atomKey);
            string atomName2 = AtomStaticLibrary.GetName(atomKey);
            string atomSymbol2 = AtomStaticLibrary.GetSymbol(atomKey);

            Assert.AreEqual(0.00054857990943, atomMass2);
            Assert.AreEqual("Electron", atomName2);
            Assert.AreEqual("e", atomSymbol2);
        }

        [Test]
        public void TestOtherMolecule()
        {
            string otherKey = "Aldehyde";
            //dictionarty implementation
            Dictionary<string, OtherMolecule> otherMoleculeDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
            double otherMass = otherMoleculeDictionary[otherKey].MonoIsotopicMass;
            string otherName = otherMoleculeDictionary[otherKey].Name;
            string otherFormula = otherMoleculeDictionary[otherKey].ChemicalFormula;

            Assert.AreEqual(18.010564686245, otherMass);
            Assert.AreEqual("Aldehyde", otherName);
            Assert.AreEqual("H2O", otherFormula);

            //One line implememtation
            double otherMass2 = OtherMoleculeStaticLibrary.GetMonoisotopicMass(otherKey);
            string otherName2 = OtherMoleculeStaticLibrary.GetName(otherKey);
            string otherFormula2 = OtherMoleculeStaticLibrary.GetFormula(otherKey);

            Assert.AreEqual(18.010564686245, otherMass2);
            Assert.AreEqual("Aldehyde", otherName2);
            Assert.AreEqual("H2O", otherFormula2);

            double mass3 = OtherMoleculeMass.Aldehyde();
            double mass4 = OtherMoleculeMass.Water();

            Assert.AreEqual(18.010564686245, mass3);
            Assert.AreEqual(18.010564686245, mass4);
        }

        [Test]
        public void TestCrossRing()
        {
            string crossRingKey = "CRFNeu5Ac_03_X1";
            //dictionarty implementation
            Dictionary<string, CrossRing> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();
            double mass = CrossRingDictionary[crossRingKey].MonoIsotopicMass;
            string formula = CrossRingDictionary[crossRingKey].ChemicalFormula;
            string name = CrossRingDictionary[crossRingKey].Name;

            Assert.AreEqual(100.01604399481499, mass);
            Assert.AreEqual("C4H4O3", formula);
            Assert.AreEqual("CRFNeu5Ac_03_X1", name);

            //one line implementation
            double mass2 = CrossRingStaticLibrary.GetMonoisotopicMass(crossRingKey);
            string formula2 = CrossRingStaticLibrary.GetFormula(crossRingKey);
            string name2 = CrossRingStaticLibrary.GetName(crossRingKey);

            Assert.AreEqual(100.01604399481499, mass);
            Assert.AreEqual("C4H4O3", formula);
            Assert.AreEqual("CRFNeu5Ac_03_X1", name);

            double mass3 = CrossRingMass.CRFHex_02_A2();
            double mass4 = CrossRingMass.CRFNeu5Ac_24_X1();

            Assert.AreEqual(102.031694058735, mass3);
            Assert.AreEqual(190.04773805355, mass4);
        }
    }
}
