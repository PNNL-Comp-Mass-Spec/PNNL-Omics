using System.Collections.Generic;

namespace PNNLOmics.Data.FormulaBuilder
{
	public class SimpleFormulaBuilder : FormulaBuilderBase
	{
		public override Dictionary<string, int> ConvertToMolecularFormula(string inputSequence)
		{
			return FormulaToDictionary(inputSequence);
		}
	}
}
