using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

//dictionary implementation
//Dictionary<string, MonosaccharideObject> OligosacchaideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
//double OSmass = OligosacchaideDictionary["Hex"].MonoIsotopicMass;
//string OSname = OligosacchaideDictionary["Hex"].Name;
//string OSformula = OligosacchaideDictionary["Hex"].ChemicalFormula;

//one line implementaiton
//double OSmass2 = MonosaccharideConstantsStaticLibrary.GetMonoisotopicMass("Hex");
//string OSname2 = MonosaccharideConstantsStaticLibrary.GetName("Hex");
//string OSformula2 = MonosaccharideConstantsStaticLibrary.GetFormula("Hex");

//double mass3 = MonosaccharideStaticLibrary.GetMonoisotopicMass(SelectMonosaccharide.NeuraminicAcid);
   
namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    public class MonosaccharideLibrary
    {
        /// <summary>
        /// This is a Class designed to create monosaccharide objects from the elements.
        /// The monosacchadies are added to a Dictionary searchable by string keys such as "DxyHex" for Deoxyhexose
        /// </summary>
        public static Dictionary<string, Monosaccharide> LoadMonosaccharideData()
        {
            Dictionary<string, Monosaccharide> monosachcarideDictionary = new Dictionary<string, Monosaccharide>();

            //each integer stands for the number of atoms in the compound -->X.NewElements(C H N O S P)
            //deoxyhexose.NewElements(C H N O S P)

            Monosaccharide deoxyhexose = new Monosaccharide();
            deoxyhexose.NewElements(6, 10, 0, 4, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            deoxyhexose.Name = "Deoxyhexose";
            deoxyhexose.ShortName = "DxyHex";
            deoxyhexose.SixLetterCode = "DxyHex";
            deoxyhexose.ChemicalFormula = "C6H10O4";
            deoxyhexose.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(deoxyhexose);

            Monosaccharide hexose = new Monosaccharide();
            hexose.NewElements(6, 10, 0, 5, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            hexose.Name = "Hexose";
            hexose.ShortName = "Hex";
            hexose.SixLetterCode = "Hexose";
            hexose.ChemicalFormula = "C6H10O5";
            hexose.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(hexose);

            Monosaccharide hexA = new Monosaccharide();
            hexA.NewElements(6, 8, 0, 6, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            hexA.Name = "Hexuronic Acid";
            hexA.ShortName = "HexA";
            hexA.SixLetterCode = "Hex A ";
            hexA.ChemicalFormula = "C6H8O6";
            hexA.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(hexA);

            //2-Keto-3-Deoxy-D-Glycero-D-Galacto-Nononic-Acid
            Monosaccharide KDNDeaminatedNeuraminicAcid = new Monosaccharide();
            KDNDeaminatedNeuraminicAcid.NewElements(9, 14, 0, 8, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            KDNDeaminatedNeuraminicAcid.Name = "(KDN) 2-Keto-3-Deoxy-D-Glycero-D-Galacto-Nononic-Acid";
            KDNDeaminatedNeuraminicAcid.ShortName = "KDN";
            KDNDeaminatedNeuraminicAcid.SixLetterCode = "KDN   ";
            KDNDeaminatedNeuraminicAcid.ChemicalFormula = "C9H14O8";
            KDNDeaminatedNeuraminicAcid.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(KDNDeaminatedNeuraminicAcid);

            Monosaccharide NAcetylhexosamine = new Monosaccharide();
            NAcetylhexosamine.NewElements(8, 13, 1, 5, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            NAcetylhexosamine.Name = "N-acetylhexosamine";
            NAcetylhexosamine.ShortName = "HexNAc";
            NAcetylhexosamine.SixLetterCode = "HexNAc";
            NAcetylhexosamine.ChemicalFormula = "C8H13NO5";
            NAcetylhexosamine.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(NAcetylhexosamine);

            Monosaccharide neuraminicAcid = new Monosaccharide();
            neuraminicAcid.NewElements(11, 17, 1, 8, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            neuraminicAcid.Name = "Neuraminic Acid";
            neuraminicAcid.ShortName = "NeuAc";
            neuraminicAcid.SixLetterCode = "Neu5Ac";
            neuraminicAcid.ChemicalFormula = "C11H17NO8";
            neuraminicAcid.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(neuraminicAcid);

            Monosaccharide NGlycolylNeuraminicAcid = new Monosaccharide();
            NGlycolylNeuraminicAcid.NewElements(11, 17, 1, 9, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            NGlycolylNeuraminicAcid.Name = "N-glycolylneuraminic Acid";
            NGlycolylNeuraminicAcid.ShortName = "NeuGc";
            NGlycolylNeuraminicAcid.SixLetterCode = "Neu5Gc";
            NGlycolylNeuraminicAcid.ChemicalFormula = "C11H17NO9";
            NGlycolylNeuraminicAcid.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(NGlycolylNeuraminicAcid);

            Monosaccharide pentose = new Monosaccharide();
            pentose.NewElements(6, 10, 5, 0, 0, 0);//-->X.NewElements(C H N O S P) number of atoms
            pentose.Name = "Pentose";
            pentose.ShortName = "Pent";
            pentose.SixLetterCode = "Pentos";
            pentose.ChemicalFormula = "C6H10O5";
            pentose.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(pentose);

            monosachcarideDictionary.Add(deoxyhexose.ShortName, deoxyhexose);
            monosachcarideDictionary.Add(hexose.ShortName, hexose);
            monosachcarideDictionary.Add(hexA.ShortName, hexA);
            monosachcarideDictionary.Add(KDNDeaminatedNeuraminicAcid.ShortName, KDNDeaminatedNeuraminicAcid);
            monosachcarideDictionary.Add(NAcetylhexosamine.ShortName, NAcetylhexosamine);
            monosachcarideDictionary.Add(neuraminicAcid.ShortName, neuraminicAcid);
            monosachcarideDictionary.Add(NGlycolylNeuraminicAcid.ShortName, NGlycolylNeuraminicAcid);
            monosachcarideDictionary.Add(pentose.ShortName, pentose);
            
            return monosachcarideDictionary;
        }
    }

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

        //overload to allow for SelectElement
        public static double GetMonoisotopicMass(SelectMonosaccharide selectKey)
        {
            MonosaccharideSingleton NewSingleton = MonosaccharideSingleton.Instance;
            Dictionary<string, Monosaccharide> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].MonoIsotopicMass;
        }

        public static string GetFormula(SelectMonosaccharide selectKey)
        {
            MonosaccharideSingleton NewSingleton = MonosaccharideSingleton.Instance;
            Dictionary<string, Monosaccharide> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].ChemicalFormula;
        }

        public static string GetName(SelectMonosaccharide selectKey)
        {
            MonosaccharideSingleton NewSingleton = MonosaccharideSingleton.Instance;
            Dictionary<string, Monosaccharide> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].Name;
        }

        public static string GetNameShort(SelectMonosaccharide selectKey)
        {
            MonosaccharideSingleton NewSingleton = MonosaccharideSingleton.Instance;
            Dictionary<string, Monosaccharide> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].ShortName;
        }

        public static string GetName6(SelectMonosaccharide selectKey)
        {
            MonosaccharideSingleton NewSingleton = MonosaccharideSingleton.Instance;
            Dictionary<string, Monosaccharide> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].SixLetterCode;
        }
    }

    public enum SelectMonosaccharide
    {
        Deoxyhexose, Hexose, HexuronicAcid, KDN, NAcetylhexosamine, NeuraminicAcid, NGlycolylneuraminicAcid, Pentose
    }
}
