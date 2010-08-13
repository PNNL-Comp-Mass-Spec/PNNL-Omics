using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//dictionary implementation                        
//Dictionary<string,ElementObject> ElementDictionary = ElementLibrary.LoadElementData();
//double ElementC12Mass = ElementDictionary["C"].IsotopeDictionary["C12"].Mass;
//double ElementC13Mass = ElementDictionary["C"].IsotopeDictionary["C13"].Mass;
//double ElementC12Abund = ElementDictionary["C"].IsotopeDictionary["C12"].NaturalAbundance;
//double ElementC13Abund = ElementDictionary["C"].IsotopeDictionary["C13"].NaturalAbundance;
//double ElemetMonoMass = ElementDictionary["C"].MonoIsotopicMass;
//string ElementName = ElementDictionary["C"].Name;
//string ElementSymbol = ElementDictionary["C"].Symbol;                     

namespace Constants
{
    public class ElementLibrary
    {
        public static Dictionary<string, ElementObject> LoadElementData()
        {
            Dictionary<string, ElementObject> ElementDictionary = new Dictionary<string, ElementObject>();

            #region Carbon
                ElementObject Carbon = new ElementObject();
                Dictionary<string, Isotope> CarbonIsotopes = new Dictionary<string, Isotope>();
                Isotope C12 = new Isotope(12, 12.0000000000, 0.98894428);
                Isotope C13 = new Isotope(13, 13.0033548385, 0.01105628);

                CarbonIsotopes.Add("C12", C12); 
                CarbonIsotopes.Add("C13", C13);

                Carbon.IsotopeDictionary = CarbonIsotopes;
                Carbon.MonoIsotopicMass = 12.0000000000;
                Carbon.Name = "Carbon";
                Carbon.Symbol = "C";
                Carbon.MassAverage = 12.01078;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            #endregion

            #region Hydrogen
                ElementObject Hydrogen = new ElementObject();
                Dictionary<string, Isotope> HydrogenIsotopes = new Dictionary<string, Isotope>();
                Isotope H1 = new Isotope(1, 1.00782503196, 0.999844265);
                Isotope H2 = new Isotope(2, 2.01410177796, 0.000155745);

                HydrogenIsotopes.Add("H1", H1);
                HydrogenIsotopes.Add("H2", H2);

                Hydrogen.IsotopeDictionary = HydrogenIsotopes;
                Hydrogen.MonoIsotopicMass = 1.00782503196;
                Hydrogen.Name = "Hydrogen";
                Hydrogen.Symbol = "H";
                Hydrogen.MassAverage = 1.007947;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            #endregion

            #region Nitrogen
                ElementObject Nitrogen = new ElementObject();
                Dictionary<string, Isotope> NitrogenIsotopes = new Dictionary<string, Isotope>();
                Isotope N14 = new Isotope(14, 14.003074007418, 0.9963374);
                Isotope N15 = new Isotope(15, 15.00010897312 , 0.0036634);

                NitrogenIsotopes.Add("N14", N14);
                NitrogenIsotopes.Add("N15", N15);

                Nitrogen.IsotopeDictionary = NitrogenIsotopes;
                Nitrogen.MonoIsotopicMass = 14.003074007418;
                Nitrogen.Name = "Nitrogen";
                Nitrogen.Symbol = "N";
                Nitrogen.MassAverage = 14.00672;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            #endregion

            #region Oxygen
                ElementObject Oxygen = new ElementObject();
                Dictionary<string, Isotope> OxygenIsotopes = new Dictionary<string, Isotope>();
                Isotope O16 = new Isotope(16, 15.994914622325, 0.99762065);
                Isotope O17 = new Isotope(17, 16.9991315022  , 0.00037909);
                Isotope O18 = new Isotope(18, 17.99916049    , 0.00200045);

                OxygenIsotopes.Add("O16", O16);
                OxygenIsotopes.Add("O17", O17); 
                OxygenIsotopes.Add("O18", O18);

                Oxygen.IsotopeDictionary = OxygenIsotopes;
                Oxygen.MonoIsotopicMass = 15.994914622325;
                Oxygen.Name = "Oxygen";
                Oxygen.Symbol = "O";
                Oxygen.MassAverage = 15.99943;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            #endregion

            #region Phosphorus
                ElementObject Phosphorus = new ElementObject();
                Dictionary<string, Isotope> PhosphorusIsotopes = new Dictionary<string, Isotope>();
                Isotope P31 = new Isotope(31, 30.9737622, 1.0000000);
                
                PhosphorusIsotopes.Add("P31", P31);
                
                Phosphorus.IsotopeDictionary = PhosphorusIsotopes;
                Phosphorus.MonoIsotopicMass = 30.9737622;
                Phosphorus.Name = "Phosphorus";
                Phosphorus.Symbol = "P";
                Phosphorus.MassAverage = 30.9737622;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            #endregion

            #region Potassium
                ElementObject Potassium = new ElementObject();
                Dictionary<string, Isotope> PotassiumIsotopes = new Dictionary<string, Isotope>();
                Isotope K39 = new Isotope(39, 38.9637069, 0.932581);//web elements
                Isotope K40 = new Isotope(40, 39.9639992, 0.000117);
                Isotope K41 = new Isotope(41, 40.9618254, 0.067302);

                PotassiumIsotopes.Add("K39", K39);
                PotassiumIsotopes.Add("K40", K40);
                PotassiumIsotopes.Add("K41", K41);

                Potassium.IsotopeDictionary = PotassiumIsotopes;
                Potassium.MonoIsotopicMass = 38.9637069;
                Potassium.Name = "Potassium";
                Potassium.Symbol = "K";
                Potassium.MassAverage = 39.09831;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            #endregion

            #region Sodium
                ElementObject Sodium = new ElementObject();
                Dictionary<string, Isotope> SodiumIsotopes = new Dictionary<string, Isotope>();
                Isotope Na23 = new Isotope(23, 22.98976967, 1.000000);//glycolyzer

                SodiumIsotopes.Add("Na23", Na23);

                Sodium.IsotopeDictionary = SodiumIsotopes;
                Sodium.MonoIsotopicMass = 22.98976967;
                Sodium.Name = "Sodium";
                Sodium.Symbol = "Na";
                Sodium.MassAverage = 22.989769282;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            #endregion

            #region Sulfur
                ElementObject Sulfur = new ElementObject();
                Dictionary<string, Isotope> SulfurIsotopes = new Dictionary<string, Isotope>();
                Isotope S32 = new Isotope(32, 31.9720707315, 0.950395790);
                Isotope S33 = new Isotope(33, 32.9714585415, 0.007486512);
                Isotope S34 = new Isotope(34, 33.9678668714, 0.041971987);
                Isotope S36 = new Isotope(36, 35.9670808825, 0.000145921);

                SulfurIsotopes.Add("S32", S32);
                SulfurIsotopes.Add("S33", S33);
                SulfurIsotopes.Add("S34", S34);
                SulfurIsotopes.Add("S36", S36);

                Sulfur.IsotopeDictionary = SulfurIsotopes;
                Sulfur.MonoIsotopicMass = 31.9720707315;
                Sulfur.Name = "Sulfur";
                Sulfur.Symbol = "S";
                Sulfur.MassAverage = 32.0655;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
            #endregion

            ElementDictionary.Add("C", Carbon);
            ElementDictionary.Add("H", Hydrogen);
            ElementDictionary.Add("N", Nitrogen);
            ElementDictionary.Add("O", Oxygen);
            ElementDictionary.Add("P", Phosphorus);
            ElementDictionary.Add("K", Potassium);
            ElementDictionary.Add("Na", Sodium);
            ElementDictionary.Add("S", Sulfur);

            return ElementDictionary;
        }
    }
}
