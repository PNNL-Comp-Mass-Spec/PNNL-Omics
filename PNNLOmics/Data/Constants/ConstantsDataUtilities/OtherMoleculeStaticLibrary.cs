using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

//One line implememtation
//double OtherMass2 = OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("Aldehyde");
//string OtherName2 = OtherMoleculeConstantsStaticLibrary.GetName("Aldehyde");
//string OtherFormula2 = OtherMoleculeConstantsStaticLibrary.GetFormula("Aldehyde");

//double mass5 = OtherMoleculeStaticLibrary.GetMonoisotopicMass(SelectOtherMolecule.Ammonia);

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    /// <summary>
    /// This is a Class designed to convert dictionary calls for Other Molecules in one line static method calls.
    /// </summary>
    public class OtherMoleculeStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].MonoIsotopicMass;
        }

        public static string GetFormula(string constantKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].ChemicalFormula;
        }

        public static string GetName(string constantKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].Name;
        }

        public static string GetName6(string constantKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].SixLetterCode;
        }

        //overload to allow for SelectElement
        public static double GetMonoisotopicMass(SelectOtherMolecule selectKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].MonoIsotopicMass;
        }

        public static string GetFormula(SelectOtherMolecule selectKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].ChemicalFormula;
        }

        public static string GetName(SelectOtherMolecule selectKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].Name;
        }

        public static string GetName6(SelectOtherMolecule selectKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].SixLetterCode;
        }
    }
}
