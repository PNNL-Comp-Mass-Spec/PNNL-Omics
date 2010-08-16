using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Constants.ConstantsObjectsDataLayer;

//One line implementation
//double elementMonoMass = ElementConstantsStaticLibrary.GetMonoisotopicMass("C");
//string elementName = ElementConstantsStaticLibrary.GetName("C");
//string elementSymbol = ElementConstantsStaticLibrary.GetSymbol("C");

namespace PNNLOmics.Constants.ConstantsDataUtilities
{
    public class ElementConstantsStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            Dictionary<string, ElementObject> ElementDictionary = ElementLibrary.LoadElementData();
            return ElementDictionary[constantKey].MonoIsotopicMass;      
        }

        public static string GetSymbol(string constantKey)
        {
            Dictionary<string, ElementObject> ElementDictionary = ElementLibrary.LoadElementData();
            return ElementDictionary[constantKey].Symbol;             
        }

        public static string GetName(string constantKey)
        {
            Dictionary<string, ElementObject> ElementDictionary = ElementLibrary.LoadElementData();
            return ElementDictionary[constantKey].Name;
        }
    }
}
