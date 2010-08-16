using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Constants.ConstantsObjectsDataLayer;

//one line implementaiton
//double OSmass2 = MonosaccharideConstantsStaticLibrary.GetMonoisotopicMass("Hex");
//string OSname2 = MonosaccharideConstantsStaticLibrary.GetName("Hex");
//string OSformula2 = MonosaccharideConstantsStaticLibrary.GetFormula("Hex");

namespace PNNLOmics.Constants.ConstantsDataUtilities
{
    public class MonosaccharideConstantsStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            Dictionary<string, MonosaccharideObject> monosacchcarideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            return monosacchcarideDictionary[constantKey].MonoIsotopicMass;          
        }

        public static string GetFormula(string constantKey)
        {
            Dictionary<string, MonosaccharideObject> monosacchcarideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            return monosacchcarideDictionary[constantKey].ChemicalFormula;             
        }

        public static string GetName(string constantKey)
        {
            Dictionary<string, MonosaccharideObject> monosacchcarideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            return monosacchcarideDictionary[constantKey].Name;
        }

        public static string GetNameShort(string constantKey)
        {
            Dictionary<string, MonosaccharideObject> monosacchcarideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            return monosacchcarideDictionary[constantKey].ShortName;
        }

        public static string GetName6(string constantKey)
        {
            Dictionary<string, MonosaccharideObject> monosacchcarideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            return monosacchcarideDictionary[constantKey].SixLetterCode;
        }
    }
}
