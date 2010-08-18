using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

//One line implementation
//double elementMonoMass = ElementConstantsStaticLibrary.GetMonoisotopicMass("C");
//string elementName = ElementConstantsStaticLibrary.GetName("C");
//string elementSymbol = ElementConstantsStaticLibrary.GetSymbol("C");

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    /// <summary>
    /// This is a Class designed to convert dictionary calls for Elements in one line static method calls.
    /// </summary>
    public class ElementStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            Dictionary<string, Element> ElementDictionary = ElementLibrary.LoadElementData();
            return ElementDictionary[constantKey].MonoIsotopicMass;      
        }

        public static string GetSymbol(string constantKey)
        {
            Dictionary<string, Element> ElementDictionary = ElementLibrary.LoadElementData();
            return ElementDictionary[constantKey].Symbol;             
        }

        public static string GetName(string constantKey)
        {
            Dictionary<string, Element> ElementDictionary = ElementLibrary.LoadElementData();
            return ElementDictionary[constantKey].Name;
        }
    }
}
