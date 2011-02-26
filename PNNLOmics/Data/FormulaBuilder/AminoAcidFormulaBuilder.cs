using System.Collections.Generic;
using PNNLOmics.Data.Constants;

namespace PNNLOmics.Data.FormulaBuilder
{
	public class AAFormulaBuilder : FormulaBuilderBase
	{

		/// <summary>
		/// Converts an amino acid sequence into a molecular formula
		/// </summary>
		/// <param name="inputSequence"></param>
		/// <param name="chargeState"></param>
		/// <returns></returns>
		public override Dictionary<string, int> ConvertToMolecularFormula(string inputSequence)
		{
			Dictionary<string, int> chemicalFormula = new Dictionary<string, int>();
			foreach (char pep in inputSequence)
			{
				string tempFormula = Constants.Constants.AminoAcids["" + pep].ChemicalFormula;
				Dictionary<string, int> currFormula = FormulaToDictionary(tempFormula);
				foreach (KeyValuePair<string, int> form in currFormula)
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
			chemicalFormula["H"] += 2;
			chemicalFormula["O"] += 1;
			return chemicalFormula;
		}


	}
}
