using System.Collections.Generic;

namespace PNNLOmics.Data.FormulaBuilder
{
	public class AminoAcidFormulaBuilder : FormulaBuilderBase
	{

		/// <summary>
		/// Converts an amino acid sequence into a molecular formula.  This does not include water or any other adduct.
		/// </summary>
		/// <param name="inputSequence"></param>
		/// <param name="chargeState"></param>
		/// <returns></returns>
		public override Dictionary<string, int> ConvertToMolecularFormula(string inputSequence)
		{
			var chemicalFormula = new Dictionary<string, int>();
			foreach (var pep in inputSequence)
			{
				var tempFormula = Constants.Constants.AminoAcids["" + pep].ChemicalFormula;
				var currFormula = FormulaToDictionary(tempFormula);
				foreach (var form in currFormula)
				{
					if (!chemicalFormula.ContainsKey(form.Key))
					{
						chemicalFormula.Add(form.Key, form.Value);
					}
					else
					{
						chemicalFormula[form.Key] += form.Value;
					}
				}
			}
			return chemicalFormula;
		}


	}
}
