using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

//one line implementation
//double CRMass2 = CrossRingConstantsStaticLibrary.GetMonoisotopicMass("crfNeu5Ac_03_X1");
//string CRFormula2 = CrossRingConstantsStaticLibrary.GetFormula("crfNeu5Ac_03_X1");
//string CRName2 = CrossRingConstantsStaticLibrary.GetName("crfNeu5Ac_03_X1");

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    public class CrossRingStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            Dictionary<string, CrossRing> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();
            return CrossRingDictionary[constantKey].MonoIsotopicMass;      
        }

        public static string GetFormula(string constantKey)
        {
            Dictionary<string, CrossRing> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();
            return CrossRingDictionary[constantKey].ChemicalFormula;             
        }

        public static string GetName(string constantKey)
        {
            Dictionary<string, CrossRing> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();
            return CrossRingDictionary[constantKey].Name;
        }
    }
}
