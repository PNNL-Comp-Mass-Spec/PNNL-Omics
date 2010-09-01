using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

//one line implementation
//double CRMass2 = CrossRingConstantsStaticLibrary.GetMonoisotopicMass("crfNeu5Ac_03_X1");
//string CRFormula2 = CrossRingConstantsStaticLibrary.GetFormula("crfNeu5Ac_03_X1");
//string CRName2 = CrossRingConstantsStaticLibrary.GetName("crfNeu5Ac_03_X1");

//double mass2 = CrossRingStaticLibrary.GetMonoisotopicMass(SelectCrossRing.CRFHex_02_A2);

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    /// <summary>
    /// This is a Class designed to convert dictionary calls for Cross Ring Fragments in one line static method calls.
    /// </summary>
    public class CrossRingStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            CrossRingSingleton NewSingleton = CrossRingSingleton.Instance;
            Dictionary<string, CrossRing> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].MonoIsotopicMass;
        }

        public static string GetFormula(string constantKey)
        {
            CrossRingSingleton NewSingleton = CrossRingSingleton.Instance;
            Dictionary<string, CrossRing> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].ChemicalFormula;
        }

        public static string GetName(string constantKey)
        {
            CrossRingSingleton NewSingleton = CrossRingSingleton.Instance;
            Dictionary<string, CrossRing> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].Name;
        }

        //overload to allow for SelectElement
        public static double GetMonoisotopicMass(SelectCrossRing selectKey)
        {
            CrossRingSingleton NewSingleton = CrossRingSingleton.Instance;
            Dictionary<string, CrossRing> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].MonoIsotopicMass;
        }

        public static string GetFormula(SelectCrossRing selectKey)
        {
            CrossRingSingleton NewSingleton = CrossRingSingleton.Instance;
            Dictionary<string, CrossRing> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].ChemicalFormula;
        }

        public static string GetName(SelectCrossRing selectKey)
        {
            CrossRingSingleton NewSingleton = CrossRingSingleton.Instance;
            Dictionary<string, CrossRing> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].Name;
        }
    }
}
