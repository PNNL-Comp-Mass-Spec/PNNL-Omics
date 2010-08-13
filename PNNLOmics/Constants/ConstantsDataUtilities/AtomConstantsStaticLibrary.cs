using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//one line implementation
//double AtomMass2 = AtomConstantsStaticLibrary.GetMonoisotopicMass("e");
//string AtomName2 = AtomConstantsStaticLibrary.GetName("e");
//string AtomSymbol2 = AtomConstantsStaticLibrary.GetSymbol("e");

namespace Constants
{
    public class AtomConstantsStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            Dictionary<string, AtomObject> AtomDictionary = AtomLibrary.LoadAtomicData();
            return AtomDictionary[constantKey].MonoIsotopicMass;      
        }

        public static string GetSymbol(string constantKey)
        {
            Dictionary<string, AtomObject> AtomDictionary = AtomLibrary.LoadAtomicData();
            return AtomDictionary[constantKey].Symbol;             
        }

        public static string GetName(string constantKey)
        {
            Dictionary<string, AtomObject> AtomDictionary = AtomLibrary.LoadAtomicData();
            return AtomDictionary[constantKey].Name;
        }
    }
}
