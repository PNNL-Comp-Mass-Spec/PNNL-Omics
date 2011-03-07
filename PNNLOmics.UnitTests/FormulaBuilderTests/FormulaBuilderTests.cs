using System.Collections.Generic;
using PNNLOmics.Data.FormulaBuilder;
using NUnit.Framework;
using System;


namespace PNNLOmics.UnitTests.FormulaBuilderTests
{
	[TestFixture]
	public class FormulaBuilderTests
	{
		[Test]
		public void TestFormulaCalculator()
		{
			AminoAcidFormulaBuilder formBuild = new AminoAcidFormulaBuilder();
			Dictionary<string, int> formula = formBuild.ConvertToMolecularFormula("ANKYLSRRH");
			Assert.AreEqual(49, formula["C"]);
			Assert.AreEqual(79, formula["H"]);
			Assert.AreEqual(19, formula["N"]);
			Assert.AreEqual(12, formula["O"]);


			formBuild.AddFormulaToPreviousFormula("HPO4", ref formula);
			Assert.AreEqual(49, formula["C"]);
			Assert.AreEqual(80, formula["H"]);
			Assert.AreEqual(19, formula["N"]);
			Assert.AreEqual(16, formula["O"]);
			Assert.AreEqual(1, formula["P"]);

			formBuild.RemoveFormulaFromPreviousFormula("H2O", ref formula);
			Assert.AreEqual(49, formula["C"]);
			Assert.AreEqual(78, formula["H"]);
			Assert.AreEqual(19, formula["N"]);
			Assert.AreEqual(15, formula["O"]);
			Assert.AreEqual(1, formula["P"]);

			double mass = formBuild.FormulaToMonoisotopicMass(formula);
			Assert.AreEqual(1204.0, Math.Round(mass));

			

		}

		[Test]
		public void Test2()
		{
			SimpleFormulaBuilder simpForm = new SimpleFormulaBuilder();
			Dictionary<string, int> SimpleFormula = simpForm.ConvertToMolecularFormula("H5C10O3");
			Assert.AreEqual(5, SimpleFormula["H"]);
			Assert.AreEqual(10, SimpleFormula["C"]);
			Assert.AreEqual(3, SimpleFormula["O"]);
		}

        [Test]
        public void TestFormulaCalculatorOligosaccharide()
        {
            OligosaccharideFormulaBuilder formBuild = new OligosaccharideFormulaBuilder();
            Dictionary<string, int> formula = formBuild.ConvertToMolecularFormula("3,2,0,0,0");
            Assert.AreEqual(34, formula["C"]);
            Assert.AreEqual(58, formula["H"]);
            Assert.AreEqual(2, formula["N"]);
            Assert.AreEqual(26, formula["O"]);
        }
	}
}