using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

//One line implememtation
//double OtherMass2 = OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("Aldehyde");
//string OtherName2 = OtherMoleculeConstantsStaticLibrary.GetName("Aldehyde");
//string OtherFormula2 = OtherMoleculeConstantsStaticLibrary.GetFormula("Aldehyde");

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    public class OtherMoleculeStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            Dictionary<string, OtherMolecule> OtherDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
            return OtherDictionary[constantKey].MonoIsotopicMass;          
        }

        public static string GetFormula(string constantKey)
        {
            Dictionary<string, OtherMolecule> OtherDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
            return OtherDictionary[constantKey].ChemicalFormula;             
        }

        public static string GetName(string constantKey)
        {
            Dictionary<string, OtherMolecule> OtherDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
            return OtherDictionary[constantKey].Name;
        }
        public static string GetName6(string constantKey)
        {
            Dictionary<string, OtherMolecule> OtherDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
            return OtherDictionary[constantKey].SixLetterCode;
        }
    }
}
