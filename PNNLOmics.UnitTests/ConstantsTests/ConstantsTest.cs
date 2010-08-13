using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Constants;

namespace LoadProgram.UnitTests
{
    class ConstantsTest
    {

        [Test]
        public void test2()//test Constants
        {
            char AAChar = 'N';
            //dictionary implementation
            Dictionary<char, AminoAcidObject> AminoAcidsDictionary = AminoAcidLibrary.LoadAminoAcidData();
            double AAMass = AminoAcidsDictionary[AAChar].MonoIsotopicMass;
            string AAName = AminoAcidsDictionary[AAChar].Name;
            string AAFormula = AminoAcidsDictionary[AAChar].ChemicalFormula;

            Assert.AreEqual(AAMass, 114.04292745124599);
            Assert.AreEqual(AAName, "Asparagine");
            Assert.AreEqual(AAFormula, "C4H6N2O2");
            
            //one line implementation
            double AAMass2 = AminoAcidConstantsStaticLibrary.GetMonoisotopicMass(AAChar);
            string AAName2 = AminoAcidConstantsStaticLibrary.GetName(AAChar);
            string AAFormula2 = AminoAcidConstantsStaticLibrary.GetFormula(AAChar);

            Assert.AreEqual(AAMass2, 114.04292745124599);
            Assert.AreEqual(AAName2, "Asparagine");
            Assert.AreEqual(AAFormula2, "C4H6N2O2");

            string OSString = "Hex";
            //dictionary implementation
            Dictionary<string, MonosaccharideObject> OligosacchaideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            double OSMass = OligosacchaideDictionary[OSString].MonoIsotopicMass;
            string OSName = OligosacchaideDictionary[OSString].Name;
            string OSFormula = OligosacchaideDictionary[OSString].ChemicalFormula;

            Assert.AreEqual(OSMass, 162.05282343122502);
            Assert.AreEqual(OSName, "Hexose");
            Assert.AreEqual(OSFormula, "C6H10O5");

            //one line implementaiton
            double OSMass2 = MonosaccharideConstantsStaticLibrary.GetMonoisotopicMass(OSString);
            string OSName2 = MonosaccharideConstantsStaticLibrary.GetName(OSString);
            string OSFormula2 = MonosaccharideConstantsStaticLibrary.GetFormula(OSString);

            Assert.AreEqual(OSMass2, 162.05282343122502);
            Assert.AreEqual(OSName2, "Hexose");
            Assert.AreEqual(OSFormula2, "C6H10O5");

            string ElementString = "C";
            //dictionary implementation

            Dictionary<string, ElementObject> ElementDictionary = ElementLibrary.LoadElementData();
            double elementC12Mass = ElementDictionary[ElementString].IsotopeDictionary["C12"].Mass;
            double elementC13Mass = ElementDictionary[ElementString].IsotopeDictionary["C13"].Mass;
            double elementC12Abund = ElementDictionary[ElementString].IsotopeDictionary["C12"].NaturalAbundance;
            double elementC13Abund = ElementDictionary[ElementString].IsotopeDictionary["C13"].NaturalAbundance;

            Assert.AreEqual(elementC12Mass, 12.0);
            Assert.AreEqual(elementC13Mass, 13.0033548385);
            Assert.AreEqual(elementC12Abund, 0.98894428);
            Assert.AreEqual(elementC13Abund, 0.01105628);

            Dictionary<string, ElementObject> elementDictionary = ElementLibrary.LoadElementData();
            double elementC12Mass2 = ElementDictionary["C"].IsotopeDictionary["C12"].Mass;
            double elementC12Abund2 = ElementDictionary["C"].IsotopeDictionary["C12"].NaturalAbundance;

            Assert.AreEqual(elementC12Mass2, 12.0);
            Assert.AreEqual(elementC12Abund2, 0.98894428);

            double ElemetMonoMass = ElementDictionary[ElementString].MonoIsotopicMass;
            string ElementName = ElementDictionary[ElementString].Name;
            string ElementSymbol = ElementDictionary[ElementString].Symbol;

            Assert.AreEqual(ElemetMonoMass, 12.0);
            Assert.AreEqual(ElementName, "Carbon");
            Assert.AreEqual(ElementSymbol, "C");

            //One line implementation

            double ElementMonoMass2 = ElementConstantsStaticLibrary.GetMonoisotopicMass(ElementString);
            string ElementName2 = ElementConstantsStaticLibrary.GetName(ElementString);
            string ElementSymbol2 = ElementConstantsStaticLibrary.GetSymbol(ElementString);

            Assert.AreEqual(ElementMonoMass2, 12.0);
            Assert.AreEqual(ElementName2, "Carbon");
            Assert.AreEqual(ElementSymbol2, "C");

            string AtomString = "e";
            //dictinoary implementation

            Dictionary<string, AtomObject> AtomDictionary = AtomLibrary.LoadAtomicData();
            double AtomMass = AtomDictionary[AtomString].MonoIsotopicMass;
            string AtomName = AtomDictionary[AtomString].Name;
            string AtomSymbol = AtomDictionary[AtomString].Symbol;

            Assert.AreEqual(AtomMass, 0.00054857990943);
            Assert.AreEqual(AtomName, "Electron");
            Assert.AreEqual(AtomSymbol, "e");

            //one line implementation
            double AtomMass2 = AtomConstantsStaticLibrary.GetMonoisotopicMass(AtomString);
            string AtomName2 = AtomConstantsStaticLibrary.GetName(AtomString);
            string AtomSymbol2 = AtomConstantsStaticLibrary.GetSymbol(AtomString);

            Assert.AreEqual(AtomMass2, 0.00054857990943);
            Assert.AreEqual(AtomName2, "Electron");
            Assert.AreEqual(AtomSymbol2, "e");

            string OtherString = "Aldehyde";
            //dictionarty implementation
            Dictionary<string, OtherMoleculeObject> OtherMoleculeDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
            double OtherMass = OtherMoleculeDictionary[OtherString].MonoIsotopicMass;
            string OtherName = OtherMoleculeDictionary[OtherString].Name;
            string OtherFormula = OtherMoleculeDictionary[OtherString].ChemicalFormula;

            Assert.AreEqual(OtherMass, 18.010564686245);
            Assert.AreEqual(OtherName, "Aldehyde");
            Assert.AreEqual(OtherFormula, "H2O");

            //One line implememtation
            double OtherMass2 = OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass(OtherString);
            string OtherName2 = OtherMoleculeConstantsStaticLibrary.GetName(OtherString);
            string OtherFormula2 = OtherMoleculeConstantsStaticLibrary.GetFormula(OtherString);

            Assert.AreEqual(OtherMass2, 18.010564686245);
            Assert.AreEqual(OtherName2, "Aldehyde");
            Assert.AreEqual(OtherFormula2, "H2O");

            string CrossRingString = "CRFNeu5Ac_03_X1";
            //dictionarty implementation
            Dictionary<string, CrossRingObject> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();
            double CRMass = CrossRingDictionary[CrossRingString].MonoIsotopicMass;
            string CRFormula = CrossRingDictionary[CrossRingString].ChemicalFormula;
            string CRName = CrossRingDictionary[CrossRingString].Name;

            Assert.AreEqual(CRMass, 100.01604399481499);
            Assert.AreEqual(CRFormula, "C4H4O3");
            Assert.AreEqual(CRName, "CRFNeu5Ac_03_X1");

            //one line implementation
            double CRMass2 = CrossRingConstantsStaticLibrary.GetMonoisotopicMass(CrossRingString);
            string CRFormula2 = CrossRingConstantsStaticLibrary.GetFormula(CrossRingString);
            string CRName2 = CrossRingConstantsStaticLibrary.GetName(CrossRingString);

            Assert.AreEqual(CRMass, 100.01604399481499);
            Assert.AreEqual(CRFormula, "C4H4O3");
            Assert.AreEqual(CRName, "CRFNeu5Ac_03_X1");

            double CRmass3 = CrossRingMass.CRFHex_02_A2();
            double CRMass4 = CrossRingMass.CRFNeu5Ac_24_X1();

            Assert.AreEqual(CRmass3, 102.031694058735);
            Assert.AreEqual(CRMass4, 190.04773805355);
   
            double OMMass3 = OtherMoleculeMass.Aldehyde();
            double OMMass4 = OtherMoleculeMass.Water();

            Assert.AreEqual(OMMass3, 18.010564686245);
            Assert.AreEqual(OMMass4, 18.010564686245);
   
            double massPeptide = 0;
            string peptideSequence = "NRTL";
            for (int y = 0; y < peptideSequence.Length; y++)
            {
                massPeptide += AminoAcidConstantsStaticLibrary.GetMonoisotopicMass(peptideSequence[y]);
            }//massPeptide = 484.27578094385393

            Assert.AreEqual(massPeptide, 484.27578094385393);         
        }

    }
}
