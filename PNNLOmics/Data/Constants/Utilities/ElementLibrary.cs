using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;
using System.IO;

/// <example>
/// dictionary implementation                        
/// Dictionary<string,ElementObject> ElementDictionary = ElementLibrary.LoadElementData();
/// double elementC12Mass = ElementDictionary["C"].IsotopeDictionary["C12"].Mass;
/// double elementC13Mass = ElementDictionary["C"].IsotopeDictionary["C13"].Mass;
/// double elementC12Abund = ElementDictionary["C"].IsotopeDictionary["C12"].NaturalAbundance;
/// double elementC13Abund = ElementDictionary["C"].IsotopeDictionary["C13"].NaturalAbundance;
/// double elemetMonoMass = ElementDictionary["C"].MonoIsotopicMass;
/// string elementName = ElementDictionary["C"].Name;
/// string elementSymbol = ElementDictionary["C"].Symbol;                     
///
/// One line implementation
/// double elementMonoMass = ElementConstantsStaticLibrary.GetMonoisotopicMass("C");
/// string elementName = ElementConstantsStaticLibrary.GetName("C");
/// string elementSymbol = ElementConstantsStaticLibrary.GetSymbol("C");
///
/// double elementMass3 = ElementStaticLibrary.GetMonoisotopicMass(SelectElement.Hydrogen);
/// </example>

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    //enum is at bottom of class
    public class ElementLibrary
    {
        /// <summary>
        /// This is a Class designed to convert raw values into element objects (including masses and isotope abundances)
        /// and create an element dictionary searchable by key string such as "C" for carbon.
        /// </summary>
        public static Dictionary<string, Element> LoadElementData()
        {
            Dictionary<string, Element> ElementDictionary = new Dictionary<string, Element>();

            bool loadFromFile = true;//if file is missing you can load a few elemets that are hard coded (C H N O P K Na S)

            if (loadFromFile)
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string constantsFileXMLName = currentDirectory + "\\PNNLOmicsElementData.xml";

                List<string> elementSymbolList = new List<string>();
                List<Element> elementList = new List<Element>();

                ElementLoadXML.LoadXML(constantsFileXMLName, out elementSymbolList, out elementList);

                for (int i = 0; i < elementSymbolList.Count; i++)
                {
                    ElementDictionary.Add(elementSymbolList[i], elementList[i]);
                }            
            }
            else
            {
                //Carbon
                Element carbon = new Element();
                Dictionary<string, Isotope> carbonIsotopes = new Dictionary<string, Isotope>();
                Isotope C12 = new Isotope(12, 12.0000000000, 0.98892228);//-->Isotope(Isotope Number, Mass, abundance on earth)
                Isotope C13 = new Isotope(13, 13.0033548385, 0.01107828);//-->Isotope(Isotope Number, Mass, abundance on earth)

                carbonIsotopes.Add("C12", C12);
                carbonIsotopes.Add("C13", C13);

                carbon.IsotopeDictionary = carbonIsotopes;
                carbon.MassMonoIsotopic = 12.0000000000;
                carbon.Name = "Carbon";
                carbon.Symbol = "C";
                carbon.MassAverage = 12.01078;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund

                //Hydrogen
                Element hydrogen = new Element();
                Dictionary<string, Isotope> hydrogenIsotopes = new Dictionary<string, Isotope>();
                Isotope H1 = new Isotope(1, 1.00782503196, 0.999844265);//-->Isotope(Isotope Number, Mass, abundance on earth)
                Isotope H2 = new Isotope(2, 2.01410177796, 0.000155745);//-->Isotope(Isotope Number, Mass, abundance on earth)

                hydrogenIsotopes.Add("H1", H1);
                hydrogenIsotopes.Add("H2", H2);

                hydrogen.IsotopeDictionary = hydrogenIsotopes;
                hydrogen.MassMonoIsotopic = 1.00782503196;
                hydrogen.Name = "Hydrogen";
                hydrogen.Symbol = "H";
                hydrogen.MassAverage = 1.007947;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund

                //Nitrogen
                Element nitrogen = new Element();
                Dictionary<string, Isotope> nitrogenIsotopes = new Dictionary<string, Isotope>();
                Isotope N14 = new Isotope(14, 14.003074007418, 0.9963374);//-->Isotope(Isotope Number, Mass, abundance on earth)
                Isotope N15 = new Isotope(15, 15.00010897312, 0.0036634);//-->Isotope(Isotope Number, Mass, abundance on earth)

                nitrogenIsotopes.Add("N14", N14);
                nitrogenIsotopes.Add("N15", N15);

                nitrogen.IsotopeDictionary = nitrogenIsotopes;
                nitrogen.MassMonoIsotopic = 14.003074007418;
                nitrogen.Name = "Nitrogen";
                nitrogen.Symbol = "N";
                nitrogen.MassAverage = 14.00672;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund

                //Oxygen
                Element oxygen = new Element();
                Dictionary<string, Isotope> oxygenIsotopes = new Dictionary<string, Isotope>();
                Isotope O16 = new Isotope(16, 15.994914622325, 0.99762065);//-->Isotope(Isotope Number, Mass, abundance on earth)
                Isotope O17 = new Isotope(17, 16.9991315022, 0.00037909);//-->Isotope(Isotope Number, Mass, abundance on earth)
                Isotope O18 = new Isotope(18, 17.99916049, 0.00200045);//-->Isotope(Isotope Number, Mass, abundance on earth)

                oxygenIsotopes.Add("O16", O16);
                oxygenIsotopes.Add("O17", O17);
                oxygenIsotopes.Add("O18", O18);

                oxygen.IsotopeDictionary = oxygenIsotopes;
                oxygen.MassMonoIsotopic = 15.994914622325;
                oxygen.Name = "Oxygen";
                oxygen.Symbol = "O";
                oxygen.MassAverage = 15.99943;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund

                //Phosphorus
                Element phosphorus = new Element();
                Dictionary<string, Isotope> phosphorusIsotopes = new Dictionary<string, Isotope>();
                Isotope P31 = new Isotope(31, 30.9737622, 1.0000000);//-->Isotope(Isotope Number, Mass, abundance on earth)

                phosphorusIsotopes.Add("P31", P31);

                phosphorus.IsotopeDictionary = phosphorusIsotopes;
                phosphorus.MassMonoIsotopic = 30.9737622;
                phosphorus.Name = "Phosphorus";
                phosphorus.Symbol = "P";
                phosphorus.MassAverage = 30.9737622;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund

                // Potassium
                Element potassium = new Element();
                Dictionary<string, Isotope> potassiumIsotopes = new Dictionary<string, Isotope>();
                Isotope K39 = new Isotope(39, 38.9637069, 0.932581);//web elements
                Isotope K40 = new Isotope(40, 39.9639992, 0.000117);//-->Isotope(Isotope Number, Mass, abundance on earth)
                Isotope K41 = new Isotope(41, 40.9618254, 0.067302);//-->Isotope(Isotope Number, Mass, abundance on earth)

                potassiumIsotopes.Add("K39", K39);
                potassiumIsotopes.Add("K40", K40);
                potassiumIsotopes.Add("K41", K41);

                potassium.IsotopeDictionary = potassiumIsotopes;
                potassium.MassMonoIsotopic = 38.9637069;
                potassium.Name = "Potassium";
                potassium.Symbol = "K";
                potassium.MassAverage = 39.09831;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund

                // Sodium
                Element sodium = new Element();
                Dictionary<string, Isotope> sodiumIsotopes = new Dictionary<string, Isotope>();
                Isotope Na23 = new Isotope(23, 22.9897696626, 1.000000);//-->Isotope(Isotope Number, Mass, abundance on earth)

                sodiumIsotopes.Add("Na23", Na23);

                sodium.IsotopeDictionary = sodiumIsotopes;
                sodium.MassMonoIsotopic = 22.98976967;
                sodium.Name = "Sodium";
                sodium.Symbol = "Na";
                sodium.MassAverage = 22.989769282;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund

                //Sulfur
                Element sulfur = new Element();
                Dictionary<string, Isotope> sulfurIsotopes = new Dictionary<string, Isotope>();
                Isotope S32 = new Isotope(32, 31.9720707315, 0.950395790);//-->Isotope(Isotope Number, Mass, abundance on earth)
                Isotope S33 = new Isotope(33, 32.9714585415, 0.007486512);//-->Isotope(Isotope Number, Mass, abundance on earth)
                Isotope S34 = new Isotope(34, 33.9678668714, 0.041971987);//-->Isotope(Isotope Number, Mass, abundance on earth)
                Isotope S36 = new Isotope(36, 35.9670808825, 0.000145921);//-->Isotope(Isotope Number, Mass, abundance on earth)

                sulfurIsotopes.Add("S32", S32);
                sulfurIsotopes.Add("S33", S33);
                sulfurIsotopes.Add("S34", S34);
                sulfurIsotopes.Add("S36", S36);

                sulfur.IsotopeDictionary = sulfurIsotopes;
                sulfur.MassMonoIsotopic = 31.9720707315;
                sulfur.Name = "Sulfur";
                sulfur.Symbol = "S";
                sulfur.MassAverage = 32.0655;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund

                //Dictionary
                ElementDictionary.Add("C", carbon);
                ElementDictionary.Add("H", hydrogen);
                ElementDictionary.Add("N", nitrogen);
                ElementDictionary.Add("O", oxygen);
                ElementDictionary.Add("P", phosphorus);
                ElementDictionary.Add("K", potassium);
                ElementDictionary.Add("Na", sodium);
                ElementDictionary.Add("S", sulfur); 
            }

            return ElementDictionary;
        }
    }

    /// <summary>
    /// This is a Class designed to convert dictionary calls for Elements in one line static method calls.
    /// </summary>
    public class ElementStaticLibrary
    {
        /// <summary>
        /// This returns the monoisotopic mass that corresponds to the dictionary key
        /// </summary>
        public static double GetMonoisotopicMass(string constantKey)
        {
            ElementSingleton NewSingleton = ElementSingleton.Instance;
            Dictionary<string, Element> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].MassMonoIsotopic;
        }

        /// <summary>
        /// This returns the Symbol that corresponds to the dictionary key
        /// </summary>
        public static string GetSymbol(string constantKey)
        {
            ElementSingleton NewSingleton = ElementSingleton.Instance;
            Dictionary<string, Element> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].Symbol;
        }

        /// <summary>
        /// This returns the name that cooresponds to the dictionary key
        /// </summary>
        public static string GetName(string constantKey)
        {
            ElementSingleton NewSingleton = ElementSingleton.Instance;
            Dictionary<string, Element> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].Name;
        }

        /// <summary>
        /// This returns the monoisotopic mass that cooresponds to the enumerated key
        /// </summary>
        public static double GetMonoisotopicMass(SelectElement selectKey)
        {
            ElementSingleton NewSingleton = ElementSingleton.Instance;
            Dictionary<string, Element> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].MassMonoIsotopic;
        }

        /// <summary>
        /// This returns the Symbol that cooresponds to the enumerated key
        /// </summary>
        public static string GetSymbol(SelectElement selectKey)
        {
            ElementSingleton NewSingleton = ElementSingleton.Instance;
            Dictionary<string, Element> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].Symbol;
        }

        /// <summary>
        /// This returns the name that cooresponds to the enumerated key
        /// </summary>
        public static string GetName(SelectElement selectKey)
        {
            ElementSingleton NewSingleton = ElementSingleton.Instance;
            Dictionary<string, Element> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].Name;
        }
    }

    /// <summary>
    /// Enumeration of elements constants
    /// </summary>
    public enum SelectElement
    {
        Hydrogen, Helium, Lithium, Berellium, Boron, Carbon, Nitrogen, Oxygen, Fluorine, Neon, Sodium, Magnesium,
        Almunium, Silicon, Phosphrous, Sulphur, Chlorine, Argon, Potassium, Calcium, Scandium, Titanium, Vanadium, Chromium,
        Manganese, Iron, Cobalt, Nickle, Copper, Zinc, Galium, Germanium, Arsenic, Selenium, Bromine, Krypton,
        Rubidium, Strontium, Yttrium, Zirconium, Niobium, Molybdenum, Technetium, Ruthenium, Rhodium, Palladium, Silver, Cadmium,
        Indium, Tin, Antimony, Tellerium, Iodine, Xenon, Caesium, Barium, Lanthanum, Cerium, Praseodymium, Neodynium,
        Promethium, Samarium, Europium, Gadolinium, Terbium, Dysprosium, Holmium, Erbium, Thulium, Ytterbium, Lutetium, Hafnium,
        Tantalum, Tungsten, Rhenium, Osmium, Iridium, Platinum, Gold, Mercury, Thallium, Lead, Bismuth, Polonium,
        Astatine, Radon, Francium, Radium, Actinium, Thorium, Protactinium, Uranium, Neptunium, Plutonium, Americium,
        Curium, Berkelium, Californium, Einsteinium, Fernium, Medelevium, Nobelium, Lawrencium
    }
}
