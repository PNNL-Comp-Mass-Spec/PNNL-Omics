using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Constants.ConstantsObjectsDataLayer;

//One line implememtation
//double OtherMass2 = OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("Aldehyde");
//string OtherName2 = OtherMoleculeConstantsStaticLibrary.GetName("Aldehyde");
//string OtherFormula2 = OtherMoleculeConstantsStaticLibrary.GetFormula("Aldehyde");

namespace PNNLOmics.Constants.ConstantsDataUtilities
{
    public class OtherMoleculeConstantsStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            Dictionary<string, OtherMoleculeObject> OtherDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
            return OtherDictionary[constantKey].MonoIsotopicMass;          
        }

        public static string GetFormula(string constantKey)
        {
            Dictionary<string, OtherMoleculeObject> OtherDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
            return OtherDictionary[constantKey].ChemicalFormula;             
        }

        public static string GetName(string constantKey)
        {
            Dictionary<string, OtherMoleculeObject> OtherDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
            return OtherDictionary[constantKey].Name;
        }
        public static string GetName6(string constantKey)
        {
            Dictionary<string, OtherMoleculeObject> OtherDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
            return OtherDictionary[constantKey].SixLetterCode;
        }
    }
}
