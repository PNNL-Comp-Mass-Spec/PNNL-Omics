using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants;


namespace PNNLOmics.Data.FormulaBuilder
{
    public class OligosaccharideFormulaBuilder : FormulaBuilderBase
    {
        /// <summary>
        /// Converts an glycan code into a molecular formula (Hex, HexNAc, Fuc, NeuAc, Na-H). Adducts are added later
        /// If you need more monosacharides, add on the the end of the glycan code (X,X,X,X,X) and update the swtch
        /// </summary>
        /// <param name="inputCode"></param>
        /// <returns></returns>
        public override Dictionary<string, int> ConvertToMolecularFormula(string inputCode)
        {
            Dictionary<string, int> chemicalFormula = new Dictionary<string, int>();

            string[] monosacharideCount = inputCode.Split(',');
            
            for (int monosaccharideCodePosition = 0; monosaccharideCodePosition < monosacharideCount.Length; monosaccharideCodePosition++)
            {
                string tempFormula = "";
                int quantityOfMonosacharides = Convert.ToInt32(monosacharideCount[monosaccharideCodePosition]);
                if (monosaccharideCodePosition < 4)
                {
                    MonosaccharideName monosaccharide = SwitchMonosacchrideByPosition(monosaccharideCodePosition);
                    tempFormula = Constants.Constants.Monosaccharides[monosaccharide].ChemicalFormula;
                }
                else
                {
                    if (monosaccharideCodePosition == 4)
                    {
                        tempFormula = Constants.Constants.MiscellaneousMatter[MiscellaneousMatterName.NaMinusH].ChemicalFormula;
                        tempFormula = "Na";  //TODO this is forced because Na-H will not work with the FormulaToDictionary.  the result is X hydrogens to many
                    }
                    else
                    {
                        Console.WriteLine("Typed code is not correct.  The number of digits does not reflect the switch");
                        Console.ReadKey();
                    }
                }

                Dictionary<string, int> currFormula = FormulaToDictionary(tempFormula);
                for (int j = 0; j < quantityOfMonosacharides; j++)
                {
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

            }
            if(chemicalFormula.ContainsKey("Na"))
            {
                for(int Na=0;Na<chemicalFormula["Na"];Na++)
                {
                     chemicalFormula["H"]--;
                }
            }
           
            //for aldehyde
            AddFormulaToPreviousFormula("H2O", ref chemicalFormula);
            
            return chemicalFormula;
        }

        private static MonosaccharideName SwitchMonosacchrideByPosition(int codePosition)
        {
            MonosaccharideName monosaccharide= new MonosaccharideName();
            switch (codePosition)
            {
                case(0):
                    {
                        monosaccharide = MonosaccharideName.Hexose;
                    }
                    break;
                case (1):
                    {
                        monosaccharide = MonosaccharideName.NAcetylhexosamine;
                    }
                    break;
                case (2):
                    {
                        monosaccharide = MonosaccharideName.Deoxyhexose;
                    }
                    break;
                case (3):
                    {
                        monosaccharide = MonosaccharideName.NeuraminicAcid;
                    }
                    break;
                default:

                    Console.WriteLine("Typed code is not correct.  The number of digits does not reflect the switch");
                    Console.ReadKey();
                    break;
            }

            return monosaccharide;
        }

    }
}
