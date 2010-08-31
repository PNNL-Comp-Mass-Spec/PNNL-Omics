using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

//one line implementaiton
//double OSmass2 = MonosaccharideConstantsStaticLibrary.GetMonoisotopicMass("Hex");
//string OSname2 = MonosaccharideConstantsStaticLibrary.GetName("Hex");
//string OSformula2 = MonosaccharideConstantsStaticLibrary.GetFormula("Hex");

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    /// <summary>
    /// This is a Class designed to convert dictionary calls for Monosaccharides in one line static method calls.
    /// </summary>
    public class MonosaccharideStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            MonosaccharideSingleton NewSingleton = MonosaccharideSingleton.Instance;
            Dictionary<string, Monosaccharide> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].MonoIsotopicMass;
        }

        public static string GetFormula(string constantKey)
        {
            MonosaccharideSingleton NewSingleton = MonosaccharideSingleton.Instance;
            Dictionary<string, Monosaccharide> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].ChemicalFormula;
        }

        public static string GetName(string constantKey)
        {
            MonosaccharideSingleton NewSingleton = MonosaccharideSingleton.Instance;
            Dictionary<string, Monosaccharide> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].Name;
        }

        public static string GetNameShort(string constantKey)
        {
            MonosaccharideSingleton NewSingleton = MonosaccharideSingleton.Instance;
            Dictionary<string, Monosaccharide> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].ShortName;
        }

        public static string GetName6(string constantKey)
        {
            MonosaccharideSingleton NewSingleton = MonosaccharideSingleton.Instance;
            Dictionary<string, Monosaccharide> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].SixLetterCode;
        }
    }
}
