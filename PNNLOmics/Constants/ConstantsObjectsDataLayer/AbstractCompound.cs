using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Constants.ConstantsObjectsDataLayer
{
    public abstract class AbstractCompound : AbstractMatter
    {
        public string ChemicalFormula { get; set; }
        
        public int nCarbonNumber { get; set; }
        public int nHydrogenNumber { get; set; }
        public int nNitrogenNumber { get; set; }
        public int nOxygenNumber { get; set; }
        public int nSulfurNumber { get; set; }
        public int nPhosphorusNumber { get; set; }
        public int nPotassium { get; set; }
        public int nSodium { get; set; }

        public static double GetMonoisotopicMass(AbstractCompound GeneralCompound)
        {
            Dictionary<string, ElementObject> ElementDictionary = ElementLibrary.LoadElementData();

            double ExactMass =
                GeneralCompound.nCarbonNumber * ElementDictionary["C"].MonoIsotopicMass +
                GeneralCompound.nHydrogenNumber * ElementDictionary["H"].MonoIsotopicMass +
                GeneralCompound.nNitrogenNumber * ElementDictionary["N"].MonoIsotopicMass +
                GeneralCompound.nOxygenNumber * ElementDictionary["O"].MonoIsotopicMass +
                GeneralCompound.nSulfurNumber * ElementDictionary["S"].MonoIsotopicMass +
                GeneralCompound.nPotassium * ElementDictionary["K"].MonoIsotopicMass +
                GeneralCompound.nSodium * ElementDictionary["Na"].MonoIsotopicMass +
                GeneralCompound.nPhosphorusNumber * ElementDictionary["P"].MonoIsotopicMass;

            return ExactMass;
        }

        public void NewElements(int C, int H, int N, int O, int S, int P)
        {
            nCarbonNumber = C;
            nHydrogenNumber = H;
            nNitrogenNumber = N;
            nOxygenNumber = O;
            nSulfurNumber = S;
            nPhosphorusNumber = P;
        }
    }


}
