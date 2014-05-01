using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PNNLOmics.Data.FormulaBuilder
{
	public abstract class FormulaBuilderBase
	{
		/// <summary>
		/// Overridden to provide a method for different types of formulas characteristic to a polymer
		/// </summary>
		/// <param name="inputSequence"></param>
		/// <returns></returns>
		public abstract Dictionary<string, int> ConvertToMolecularFormula(string inputSequence);

		/// <summary>
		/// Converts a formula into a series of keyvaluepairs of element and count
		/// </summary>
		/// <param name="tempFormula"></param>
		/// <returns></returns>
		public Dictionary<string, int> FormulaToDictionary(string tempFormula)
		{
			var chemicalFormula = new Dictionary<string, int>();
			var re = new Regex(@"([A-Z][a-z]*)(\d*)");

			var mc = re.Matches(tempFormula);
			foreach (Match item in mc)
			{
				var numAtomsAreIndicated = (!item.Groups[2].Value.Equals(string.Empty));
				var numAtoms = 0;
				var elementSymbol = String.Empty;

				elementSymbol = item.Groups[1].Value;
				if (numAtomsAreIndicated)
				{
					numAtoms = Int32.Parse(item.Groups[2].Value);
				}
				else
				{
					numAtoms = 1;
				}
				if (chemicalFormula.ContainsKey(elementSymbol))
				{
					throw new FormatException("Chemical formula contains duplicate elements.");
				}
				chemicalFormula.Add(elementSymbol, numAtoms);
			}
			return chemicalFormula;
		}

		/// <summary>
		/// Add further elements to the given dictionary
		/// </summary>
		/// <param name="formulaValuesToAdd"></param>
		/// <param name="previousDict"></param>
		public void AddFormulaToPreviousFormula(string formulaValuesToAdd, ref Dictionary<string, int> previousDict)
		{

			var temp = FormulaToDictionary(formulaValuesToAdd);
			foreach (var t in temp.Keys)
			{
				if (!previousDict.ContainsKey(t))
				{
					previousDict.Add(t, temp[t]);
				}
				else
				{
					previousDict[t] += temp[t];
				}
			}

		}

		/// <summary>
		/// Removes a chemical formula from a previous formula, in cases like hydrolysis leading to removal of an H2O
		/// </summary>
		/// <param name="formulaValuesToRemove">string of elements to remove from the formula</param>
		/// <param name="previousDict">The dictionary to be changed</param>
		public void RemoveFormulaFromPreviousFormula(string formulaValuesToRemove, ref Dictionary<string, int> previousDict)
		{
			var temp = FormulaToDictionary(formulaValuesToRemove);
			foreach (var t in temp.Keys)
			{
				if (!previousDict.ContainsKey(t))
				{
					previousDict.Add(t, temp[t]);
				}
				else
				{
					if (previousDict[t] <= temp[t])
					{
						previousDict.Remove(t);
					}
					else
					{
						previousDict[t] -= temp[t];
					}
				}
			}
		}

		public double FormulaToMonoisotopicMass(Dictionary<string, int> formula)
		{
			var massMonoIsotopic = 0.0;

			foreach (var f in formula.Keys)
			{
				massMonoIsotopic += Constants.Constants.Elements[f].MassMonoIsotopic * formula[f];
			}
			return massMonoIsotopic;
		}


	}
}
