using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//one line implementaiton
//double OSmass2 = MonosaccharideConstantsStaticLibrary.GetMonoisotopicMass("Hex");
//string OSname2 = MonosaccharideConstantsStaticLibrary.GetName("Hex");
//string OSformula2 = MonosaccharideConstantsStaticLibrary.GetFormula("Hex");

namespace Constants
{
    public class MonosaccharideConstantsStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            Dictionary<string, MonosaccharideObject> MonosacchcarideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            return MonosacchcarideDictionary[constantKey].MonoIsotopicMass;          
        }

        public static string GetFormula(string constantKey)
        {
            Dictionary<string, MonosaccharideObject> MonosacchcarideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            return MonosacchcarideDictionary[constantKey].ChemicalFormula;             
        }

        public static string GetName(string constantKey)
        {
            Dictionary<string, MonosaccharideObject> MonosacchcarideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            return MonosacchcarideDictionary[constantKey].Name;
        }

        public static string GetNameShort(string constantKey)
        {
            Dictionary<string, MonosaccharideObject> MonosacchcarideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            return MonosacchcarideDictionary[constantKey].ShortName;
        }

        public static string GetName6(string constantKey)
        {
            Dictionary<string, MonosaccharideObject> MonosacchcarideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            return MonosacchcarideDictionary[constantKey].SixLetterCode;
        }
    }
}
