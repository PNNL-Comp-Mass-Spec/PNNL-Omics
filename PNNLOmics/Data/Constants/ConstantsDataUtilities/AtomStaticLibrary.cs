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
    /// <summary>
    /// This is a Class designed to convert dictionary calls for Atoms in one line static method calls.
    /// </summary>
    public class AtomStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            AtomSingleton NewSingleton = AtomSingleton.Instance;
            Dictionary<string, Atom> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].MonoIsotopicMass;
        }

        public static string GetSymbol(string constantKey)
        {
            AtomSingleton NewSingleton = AtomSingleton.Instance;
            Dictionary<string, Atom> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].Symbol;
        }

        public static string GetName(string constantKey)
        {
            AtomSingleton NewSingleton = AtomSingleton.Instance;
            Dictionary<string, Atom> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].Name;
        }
    }
}
