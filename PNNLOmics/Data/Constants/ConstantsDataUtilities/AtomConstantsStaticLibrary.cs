using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

//one line implementation
//double AtomMass2 = AtomConstantsStaticLibrary.GetMonoisotopicMass("e");
//string AtomName2 = AtomConstantsStaticLibrary.GetName("e");
//string AtomSymbol2 = AtomConstantsStaticLibrary.GetSymbol("e");

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    public class AtomConstantsStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            Dictionary<string, Atom> atomDictionary = AtomLibrary.LoadAtomicData();
            return atomDictionary[constantKey].MonoIsotopicMass;      
        }

        public static string GetSymbol(string constantKey)
        {
            Dictionary<string, Atom> atomDictionary = AtomLibrary.LoadAtomicData();
            return atomDictionary[constantKey].Symbol;             
        }

        public static string GetName(string constantKey)
        {
            Dictionary<string, Atom> atomDictionary = AtomLibrary.LoadAtomicData();
            return atomDictionary[constantKey].Name;
        }
    }
}
