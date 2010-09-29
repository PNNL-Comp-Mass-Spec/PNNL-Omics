using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

/// <example>
/// dictionary implementation
/// Dictionary<string, Compound> OligosacchaideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
/// double OSmass = OligosacchaideDictionary["Hex"].MonoIsotopicMass;
/// string OSname = OligosacchaideDictionary["Hex"].Name;
/// string OSformula = OligosacchaideDictionary["Hex"].ChemicalFormula;
///
/// one line implementaiton
/// double OSmass2 = MonosaccharideConstantsStaticLibrary.GetMonoisotopicMass("Hex");
/// string OSname2 = MonosaccharideConstantsStaticLibrary.GetName("Hex");
/// string OSformula2 = MonosaccharideConstantsStaticLibrary.GetFormula("Hex");
///
/// double mass3 = MonosaccharideStaticLibrary.GetMonoisotopicMass(SelectMonosaccharide.NeuraminicAcid);
/// </example>
   
namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    public class MonosaccharideLibrary
    {
        /// <summary>
        /// This is a Class designed to create Compound objects from the elements.
        /// The monosacchadies are added to a Dictionary searchable by string keys such as "DxyHex" for Deoxyhexose
        /// </summary>
        public static Dictionary<string, Compound> LoadMonosaccharideData()
        {
            Dictionary<string, Compound> monosachcarideDictionary = new Dictionary<string, Compound>();

            //each integer stands for the number of atoms in the compound -->X.NewElements(C H N O S P)
            //deoxyhexose.NewElements(C H N O S P)

            Compound deoxyhexose = new Compound();
            deoxyhexose.NewElements(6, 10, 0, 4, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            deoxyhexose.Name = "Deoxyhexose";
            deoxyhexose.Symbol = "DxyHex";
            //deoxyhexose.SixLetterCode = "DxyHex";
            deoxyhexose.ChemicalFormula = "C6H10O4";
            deoxyhexose.MassMonoIsotopic = Compound.GetMonoisotopicMass(deoxyhexose);

            Compound hexose = new Compound();
            hexose.NewElements(6, 10, 0, 5, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            hexose.Name = "Hexose";
            hexose.Symbol = "Hex";
            //hexose.SixLetterCode = "Hexose";
            hexose.ChemicalFormula = "C6H10O5";
            hexose.MassMonoIsotopic = Compound.GetMonoisotopicMass(hexose);

            Compound hexA = new Compound();
            hexA.NewElements(6, 8, 0, 6, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            hexA.Name = "Hexuronic Acid";
            hexA.Symbol = "HexA";
            //hexA.SixLetterCode = "Hex A ";
            hexA.ChemicalFormula = "C6H8O6";
            hexA.MassMonoIsotopic = Compound.GetMonoisotopicMass(hexA);

            //2-Keto-3-Deoxy-D-Glycero-D-Galacto-Nononic-Acid
            Compound KDNDeaminatedNeuraminicAcid = new Compound();
            KDNDeaminatedNeuraminicAcid.NewElements(9, 14, 0, 8, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            KDNDeaminatedNeuraminicAcid.Name = "(KDN) 2-Keto-3-Deoxy-D-Glycero-D-Galacto-Nononic-Acid";
            KDNDeaminatedNeuraminicAcid.Symbol = "KDN";
            //KDNDeaminatedNeuraminicAcid.SixLetterCode = "KDN   ";
            KDNDeaminatedNeuraminicAcid.ChemicalFormula = "C9H14O8";
            KDNDeaminatedNeuraminicAcid.MassMonoIsotopic = Compound.GetMonoisotopicMass(KDNDeaminatedNeuraminicAcid);

            Compound NAcetylhexosamine = new Compound();
            NAcetylhexosamine.NewElements(8, 13, 1, 5, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            NAcetylhexosamine.Name = "N-acetylhexosamine";
            NAcetylhexosamine.Symbol = "HexNAc";
            //NAcetylhexosamine.SixLetterCode = "HexNAc";
            NAcetylhexosamine.ChemicalFormula = "C8H13NO5";
            NAcetylhexosamine.MassMonoIsotopic = Compound.GetMonoisotopicMass(NAcetylhexosamine);

            Compound neuraminicAcid = new Compound();
            neuraminicAcid.NewElements(11, 17, 1, 8, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            neuraminicAcid.Name = "Neuraminic Acid";
            neuraminicAcid.Symbol = "NeuAc";
            //neuraminicAcid.SixLetterCode = "Neu5Ac";
            neuraminicAcid.ChemicalFormula = "C11H17NO8";
            neuraminicAcid.MassMonoIsotopic = Compound.GetMonoisotopicMass(neuraminicAcid);

            Compound NGlycolylNeuraminicAcid = new Compound();
            NGlycolylNeuraminicAcid.NewElements(11, 17, 1, 9, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            NGlycolylNeuraminicAcid.Name = "N-glycolylneuraminic Acid";
            NGlycolylNeuraminicAcid.Symbol = "NeuGc";
            //NGlycolylNeuraminicAcid.SixLetterCode = "Neu5Gc";
            NGlycolylNeuraminicAcid.ChemicalFormula = "C11H17NO9";
            NGlycolylNeuraminicAcid.MassMonoIsotopic = Compound.GetMonoisotopicMass(NGlycolylNeuraminicAcid);

            Compound pentose = new Compound();
            pentose.NewElements(6, 10, 5, 0, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            pentose.Name = "Pentose";
            pentose.Symbol = "Pent";
            //pentose.SixLetterCode = "Pentos";
            pentose.ChemicalFormula = "C6H10O5";
            pentose.MassMonoIsotopic = Compound.GetMonoisotopicMass(pentose);

            monosachcarideDictionary.Add(deoxyhexose.Symbol, deoxyhexose);
            monosachcarideDictionary.Add(hexose.Symbol, hexose);
            monosachcarideDictionary.Add(hexA.Symbol, hexA);
            monosachcarideDictionary.Add(KDNDeaminatedNeuraminicAcid.Symbol, KDNDeaminatedNeuraminicAcid);
            monosachcarideDictionary.Add(NAcetylhexosamine.Symbol, NAcetylhexosamine);
            monosachcarideDictionary.Add(neuraminicAcid.Symbol, neuraminicAcid);
            monosachcarideDictionary.Add(NGlycolylNeuraminicAcid.Symbol, NGlycolylNeuraminicAcid);
            monosachcarideDictionary.Add(pentose.Symbol, pentose);

            return monosachcarideDictionary;
        }
    }

    /// <summary>
    /// This is a Class designed to convert dictionary calls for Monosaccharides in one line static method calls.
    /// </summary>
    public class MonosaccharideStaticLibrary
    {
        /// <summary>
        /// This returns the monoisotopic mass that corresponds to the dictionary key
        /// </summary>
        public static double GetMonoisotopicMass(string constantKey)
        {
            CompoundSingleton NewSingleton = CompoundSingleton.Instance;
            NewSingleton.InitializeMonosaccharideLibrary();
            Dictionary<string, Compound> incommingDictionary = NewSingleton.MonosaccharideConstantsDictionary;
            return incommingDictionary[constantKey].MassMonoIsotopic;
        }

        /// <summary>
        /// This returns the Symbol that corresponds to the dictionary key
        /// </summary>
        public static string GetSymbol(string constantKey)
        {
            CompoundSingleton NewSingleton = CompoundSingleton.Instance;
            NewSingleton.InitializeMonosaccharideLibrary();
            Dictionary<string, Compound> incommingDictionary = NewSingleton.MonosaccharideConstantsDictionary;
            return incommingDictionary[constantKey].Symbol;
        }

        /// <summary>
        /// This returns the name that cooresponds to the dictionary key
        /// </summary>
        public static string GetName(string constantKey)
        {
            CompoundSingleton NewSingleton = CompoundSingleton.Instance;
            NewSingleton.InitializeMonosaccharideLibrary();
            Dictionary<string, Compound> incommingDictionary = NewSingleton.MonosaccharideConstantsDictionary;
            return incommingDictionary[constantKey].Name;
        }

        /// <summary>
        /// This returns the chemical formula that cooresponds to the dictionary key
        /// </summary>
        public static string GetFormula(string constantKey)
        {
            CompoundSingleton NewSingleton = CompoundSingleton.Instance;
            NewSingleton.InitializeMonosaccharideLibrary();
            Dictionary<string, Compound> incommingDictionary = NewSingleton.MonosaccharideConstantsDictionary;
            return incommingDictionary[constantKey].ChemicalFormula;
        }

        /// <summary>
        /// This returns the monoisotopic mass that cooresponds to the enumerated key
        /// </summary>
        public static double GetMonoisotopicMass(SelectMonosaccharide selectKey)
        {
            CompoundSingleton NewSingleton = CompoundSingleton.Instance;
            NewSingleton.InitializeMonosaccharideLibrary();
            Dictionary<string, Compound> incommingDictionary = NewSingleton.MonosaccharideConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.MonosaccharideConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].MassMonoIsotopic;
        }

        /// <summary>
        /// This returns the Symbol that cooresponds to the enumerated key
        /// </summary>
        public static string GetSymbol(SelectMonosaccharide selectKey)
        {
            CompoundSingleton NewSingleton = CompoundSingleton.Instance;
            NewSingleton.InitializeMonosaccharideLibrary();
            Dictionary<string, Compound> incommingDictionary = NewSingleton.MonosaccharideConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.MonosaccharideConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].Symbol;
        }

        /// <summary>
        /// This returns the name that cooresponds to the enumerated key
        /// </summary>
        public static string GetName(SelectMonosaccharide selectKey)
        {
            CompoundSingleton NewSingleton = CompoundSingleton.Instance;
            NewSingleton.InitializeMonosaccharideLibrary();
            Dictionary<string, Compound> incommingDictionary = NewSingleton.MonosaccharideConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.MonosaccharideConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].Name;
        }

        /// <summary>
        /// This returns the chemical formula that cooresponds to the enumerated key
        /// </summary>
        public static string GetFormula(SelectMonosaccharide selectKey)
        {
            CompoundSingleton NewSingleton = CompoundSingleton.Instance;
            NewSingleton.InitializeMonosaccharideLibrary();
            Dictionary<string, Compound> incommingDictionary = NewSingleton.MonosaccharideConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.MonosaccharideConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].ChemicalFormula;
        }
    }

    /// <summary>
    /// Enumeration of monosaccharide constants
    /// </summary>
    public enum SelectMonosaccharide
    {
        Deoxyhexose, Hexose, HexuronicAcid, KDN, NAcetylhexosamine, NeuraminicAcid, NGlycolylneuraminicAcid, Pentose
    }
}
