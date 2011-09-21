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
        /// <summary>
        /// Elemental Formula for the compound
        /// </summary>
        public string ChemicalFormula { get; set; }
 
        //This naming standard was changed so that the number of elements are grouped together in Intelli Sense
        /// <summary>
        /// Number of carbon atoms in the compound
        /// </summary>
        public int NumCarbon { get; set; }

        /// <summary>
        /// Number of hydrogen atoms in the compound
        /// </summary>
        public int NumHydrogen { get; set; }

        /// <summary>
        /// Number of nitrogen atoms in the compound
        /// </summary>
        public int NumNitrogen { get; set; }

        /// <summary>
        /// Number of oxygen atoms in the compound
        /// </summary>
        public int NumOxygen { get; set; }

        /// <summary>
        /// Number of sulfur atoms in the compound
        /// </summary>
        public int NumSulfur { get; set; }

        /// <summary>
        /// Number of phosphorus atoms in the compound
        /// </summary>
        public int NumPhosphorus { get; set; }

        /// <summary>
        /// Number of potassium atoms in the compound
        /// </summary>
        public int NumPotassium { get; set; }

        /// <summary>
        /// Number of sodium atoms in the compound
        /// </summary>
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

        /// <summary>
        /// sets up the elements in the compound
        /// </summary>
        /// <param name="c">number of carbon atoms</param>
        /// <param name="h">number of hydrogen atoms</param>
        /// <param name="n">number of nitrogen atoms</param>
        /// <param name="o">number of oxygen atoms</param>
        /// <param name="s">number of sulfur atoms</param>
        /// <param name="p">number of phosphorus atoms</param>
        public void NewElements(int c, int h, int n, int o, int s, int p)
        {
            NumCarbon = c;
            NumHydrogen = h;
            NumNitrogen = n;
            NumOxygen = o;
            NumSulfur = s;
            NumPhosphorus = p;
        }
    }
}
