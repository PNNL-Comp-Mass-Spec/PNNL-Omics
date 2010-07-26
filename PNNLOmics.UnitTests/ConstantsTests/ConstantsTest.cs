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
        public void test2()//test file iterator
        {
            //amino acid          
            string chemicalFormulaA = AminoAcidConstantsTable.GetFormula('A');
            double massA = AminoAcidConstantsTable.GetMass('A');

            List<char> outCharIndexA;
            List<double> outDoubleMassesA;
            AminoAcidUtility.AminoAcidMassList(out outCharIndexA, out outDoubleMassesA);

            //glycan
            string chemicalFormulaG = MonosaccharideConstantsTable.GetFormula("Hex");
            double massG = MonosaccharideConstantsTable.GetMass("Hex");
            string nameG = MonosaccharideConstantsTable.GetName("Hex");
            string name6G = MonosaccharideConstantsTable.GetName6("Hex");

            List<string> outStringIndexG;
            List<double> outDoubleMassesG;
            MonosaccharideUtilities.MonoSaccharideMassList(out outStringIndexG, out outDoubleMassesG);

            //elements
            string nameE = ElementsConstantTable.GetName("Na");
            double massE = ElementsConstantTable.GetExactMass("Na");

            //otheMolecules
            double massO = OtherMoleculeConstantsTable.GetMass("Water");
            string name6O = OtherMoleculeConstantsTable.GetName6("Water");
            string nameO = OtherMoleculeConstantsTable.GetName("Water");
            string chemicalFormulaO = OtherMoleculeConstantsTable.GetFormula("Water");

            //physical constants
            string nameP = PhysicalConstantsTable.GetName('e');
            double massP = PhysicalConstantsTable.GetExactMass('e');

            Assert.AreEqual(chemicalFormulaA, "C3H5NO");
            Assert.AreEqual(massA, 71.037113789543);

            Assert.AreEqual(massG, 162.05282343122502);
            Assert.AreEqual(nameG,"Hexose");
            Assert.AreEqual(name6G, "Hex   ");

            Assert.AreEqual(nameE, "Sodium");
            Assert.AreEqual(massE, 22.98976967);

            Assert.AreEqual(massO, 18.010564686245);
            Assert.AreEqual(name6O, "Water ");
            Assert.AreEqual(nameO, "Water");
            Assert.AreEqual(chemicalFormulaO, "H2O");

            Assert.AreEqual(nameP, "Electron");
            Assert.AreEqual(massP, 0.00054857990943);

        }
        
    }
}
