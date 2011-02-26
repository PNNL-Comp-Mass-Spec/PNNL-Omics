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
			AAFormulaBuilder formBuild = new AAFormulaBuilder();
			Dictionary<string, int> formula = formBuild.ConvertToMolecularFormula("ANKYLSRRH");
			Assert.AreEqual(49, formula["C"]);
			Assert.AreEqual(81, formula["H"]);
			Assert.AreEqual(19, formula["N"]);
			Assert.AreEqual(13, formula["O"]);


			formBuild.AddFormulaToPreviousFormula("HPO4", ref formula);
			Assert.AreEqual(49, formula["C"]);
			Assert.AreEqual(82, formula["H"]);
			Assert.AreEqual(19, formula["N"]);
			Assert.AreEqual(17, formula["O"]);
			Assert.AreEqual(1, formula["P"]);

			formBuild.RemoveFormulaFromPreviousFormula("H2O", ref formula);
			Assert.AreEqual(49, formula["C"]);
			Assert.AreEqual(80, formula["H"]);
			Assert.AreEqual(19, formula["N"]);
			Assert.AreEqual(16, formula["O"]);
			Assert.AreEqual(1, formula["P"]);

			double mass = formBuild.FormulaToMonoisotopicMass(formula);
			Assert.AreEqual(1222.0, Math.Round(mass));

			

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
	}
}