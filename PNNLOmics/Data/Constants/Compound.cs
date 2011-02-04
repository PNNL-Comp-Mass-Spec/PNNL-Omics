using System;
using System.Collections.Generic;
using PNNLOmics.Data.Constants.Libraries;

namespace PNNLOmics.Data.Constants
{
    /// <summary>
    /// This is an abstract Class designed to cover the most basic parameters of compound objects.
    /// Compounds are made up of elements.
    /// </summary>
    public class Compound : Matter
    {
        //TODO: SCOTT - CR - add XML comments
        public string ChemicalFormula { get; set; }

        //TODO: SCOTT - CR - add XML comments
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

            double exactMass =
                GeneralCompound.NumCarbon * Constants.Elements[ElementName.Carbon].MassMonoIsotopic +
                GeneralCompound.NumHydrogen * Constants.Elements[ElementName.Hydrogen].MassMonoIsotopic +
                GeneralCompound.NumNitrogen * Constants.Elements[ElementName.Nitrogen].MassMonoIsotopic +
                GeneralCompound.NumOxygen * Constants.Elements[ElementName.Oxygen].MassMonoIsotopic +
                GeneralCompound.NumSulfur * Constants.Elements[ElementName.Sulfur].MassMonoIsotopic +
                GeneralCompound.NumPotassium * Constants.Elements[ElementName.Potassium].MassMonoIsotopic +
                GeneralCompound.NumSodium * Constants.Elements[ElementName.Sodium].MassMonoIsotopic +
                GeneralCompound.NumPhosphorus * Constants.Elements[ElementName.Phosphorous].MassMonoIsotopic;

            return exactMass;
        }

        //TODO: SCOTT - CR - add XML comments
        //TODO: SCOTT - CR - make names lower-case, and elaborate what the names are (C, H, N..etc)
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
