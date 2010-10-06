using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This is an abstract Class designed to cover the most basic parameters of compound objects.
    /// Compounds are made up of elements.
    /// </summary>
    public class Compound : Matter
    {
        public string ChemicalFormula { get; set; }
        
        //This naming standard was changed so that the number of elements are grouped together in Intelli Sense
        public int NumCarbon { get; set; }
        public int NumHydrogen { get; set; }
        public int NumNitrogen { get; set; }
        public int NumOxygen { get; set; }
        public int NumSulfur { get; set; }
        public int NumPhosphorus { get; set; }
        public int NumPotassium { get; set; }
        public int NumSodium { get; set; }

        /// <summary>
        /// This static class is used to calculate the Monoisotopic mass from the element values.
        /// </summary>
        public static double GetMonoisotopicMass(Compound GeneralCompound)
        {
            Dictionary<string, Element> elementDictionary = ElementLibrary.LoadElementData();

            double ExactMass =
                GeneralCompound.NumCarbon * elementDictionary["C"].MassMonoIsotopic +
                GeneralCompound.NumHydrogen * elementDictionary["H"].MassMonoIsotopic +
                GeneralCompound.NumNitrogen * elementDictionary["N"].MassMonoIsotopic +
                GeneralCompound.NumOxygen * elementDictionary["O"].MassMonoIsotopic +
                GeneralCompound.NumSulfur * elementDictionary["S"].MassMonoIsotopic +
                GeneralCompound.NumPotassium * elementDictionary["K"].MassMonoIsotopic +
                GeneralCompound.NumSodium * elementDictionary["Na"].MassMonoIsotopic +
                GeneralCompound.NumPhosphorus * elementDictionary["P"].MassMonoIsotopic;

            return ExactMass;
        }
        
        public void NewElements(int C, int H, int N, int O, int S, int P)
        {
            NumCarbon = C;
            NumHydrogen = H;
            NumNitrogen = N;
            NumOxygen = O;
            NumSulfur = S;
            NumPhosphorus = P;
        }
    }


}
