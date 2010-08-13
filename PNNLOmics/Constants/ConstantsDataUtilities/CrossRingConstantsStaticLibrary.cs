using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//one line implementation
//double CRMass2 = CrossRingConstantsStaticLibrary.GetMonoisotopicMass("CRFNeu5Ac_03_X1");
//string CRFormula2 = CrossRingConstantsStaticLibrary.GetFormula("CRFNeu5Ac_03_X1");
//string CRName2 = CrossRingConstantsStaticLibrary.GetName("CRFNeu5Ac_03_X1");

namespace Constants
{
    public class CrossRingConstantsStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            Dictionary<string, CrossRingObject> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();
            return CrossRingDictionary[constantKey].MonoIsotopicMass;      
        }

        public static string GetFormula(string constantKey)
        {
            Dictionary<string, CrossRingObject> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();
            return CrossRingDictionary[constantKey].ChemicalFormula;             
        }

        public static string GetName(string constantKey)
        {
            Dictionary<string, CrossRingObject> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();
            return CrossRingDictionary[constantKey].Name;
        }
    }
}
