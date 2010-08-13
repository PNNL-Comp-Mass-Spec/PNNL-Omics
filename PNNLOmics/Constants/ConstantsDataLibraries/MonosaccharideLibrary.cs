using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//dictionary implementation
//Dictionary<string, MonosaccharideObject> OligosacchaideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
//double OSmass = OligosacchaideDictionary["Hex"].MonoIsotopicMass;
//string OSname = OligosacchaideDictionary["Hex"].Name;
//string OSformula = OligosacchaideDictionary["Hex"].ChemicalFormula;
                        
namespace Constants
{
    public class MonosaccharideLibrary
    {
        public static Dictionary<string, MonosaccharideObject> LoadMonosaccharideData()
        {
            Dictionary<string, MonosaccharideObject> MonosachcarideDictionary = new Dictionary<string, MonosaccharideObject>();

            //Deoxyhexose.NewElements(C H N O S P)

            MonosaccharideObject Deoxyhexose = new MonosaccharideObject();
            Deoxyhexose.NewElements(6, 10, 0, 4, 0, 0);
            Deoxyhexose.Name = "Deoxyhexose";
            Deoxyhexose.ShortName = "DxyHex";
            Deoxyhexose.SixLetterCode = "DxyHex";
            Deoxyhexose.ChemicalFormula = "C6H10O4";
            Deoxyhexose.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(Deoxyhexose);

            MonosaccharideObject Hexose = new MonosaccharideObject();
            Hexose.NewElements(6, 10, 0, 5, 0, 0);
            Hexose.Name = "Hexose";
            Hexose.ShortName = "Hex";
            Hexose.SixLetterCode = "Hexose";
            Hexose.ChemicalFormula = "C6H10O5";
            Hexose.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(Hexose);

            MonosaccharideObject HexA = new MonosaccharideObject();
            HexA.NewElements(6, 8, 0, 6, 0, 0);
            HexA.Name = "Hexuronic Acid";
            HexA.ShortName = "HexA";
            HexA.SixLetterCode = "Hex A ";
            HexA.ChemicalFormula = "C6H8O6";
            HexA.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(HexA);

            //2-Keto-3-Deoxy-D-Glycero-D-Galacto-Nononic-Acid
            MonosaccharideObject KDNDeaminatedNeuraminicAcid = new MonosaccharideObject();
            KDNDeaminatedNeuraminicAcid.NewElements(9, 14, 0, 8, 0, 0);
            KDNDeaminatedNeuraminicAcid.Name = "(KDN) 2-Keto-3-Deoxy-D-Glycero-D-Galacto-Nononic-Acid";
            KDNDeaminatedNeuraminicAcid.ShortName = "KDN";
            KDNDeaminatedNeuraminicAcid.SixLetterCode = "KDN   ";
            KDNDeaminatedNeuraminicAcid.ChemicalFormula = "C9H14O8";
            KDNDeaminatedNeuraminicAcid.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(KDNDeaminatedNeuraminicAcid);

            MonosaccharideObject NAcetylhexosamine = new MonosaccharideObject();
            NAcetylhexosamine.NewElements(8, 13, 1, 5, 0, 0);
            NAcetylhexosamine.Name = "N-acetylhexosamine";
            NAcetylhexosamine.ShortName = "HexNAc";
            NAcetylhexosamine.SixLetterCode = "HexNAc";
            NAcetylhexosamine.ChemicalFormula = "C8H13NO5";
            NAcetylhexosamine.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(NAcetylhexosamine);

            MonosaccharideObject NeuraminicAcid = new MonosaccharideObject();
            NeuraminicAcid.NewElements(11, 17, 1, 8, 0, 0);
            NeuraminicAcid.Name = "Neuraminic Acid";
            NeuraminicAcid.ShortName = "NeuAc";
            NeuraminicAcid.SixLetterCode = "Neu5Ac";
            NeuraminicAcid.ChemicalFormula = "C11H17NO8";
            NeuraminicAcid.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(NeuraminicAcid);

            MonosaccharideObject NGlycolylNeuraminicAcid = new MonosaccharideObject();
            NGlycolylNeuraminicAcid.NewElements(11, 17, 1, 9, 0, 0);
            NGlycolylNeuraminicAcid.Name = "N-glycolylneuraminic Acid";
            NGlycolylNeuraminicAcid.ShortName = "NeuGc";
            NGlycolylNeuraminicAcid.SixLetterCode = "Neu5Gc";
            NGlycolylNeuraminicAcid.ChemicalFormula = "C11H17NO9";
            NGlycolylNeuraminicAcid.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(NGlycolylNeuraminicAcid);

            MonosaccharideObject Pentose = new MonosaccharideObject();
            Pentose.NewElements(6, 10, 5, 0, 0, 0);
            Pentose.Name = "Pentose";
            Pentose.ShortName = "Pent";
            Pentose.SixLetterCode = "Pentos";
            Pentose.ChemicalFormula = "C6H10O5";
            Pentose.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(Pentose);

            MonosachcarideDictionary.Add(Deoxyhexose.ShortName, Deoxyhexose);
            MonosachcarideDictionary.Add(Hexose.ShortName, Hexose);
            MonosachcarideDictionary.Add(HexA.ShortName, HexA);
            MonosachcarideDictionary.Add(KDNDeaminatedNeuraminicAcid.ShortName, KDNDeaminatedNeuraminicAcid);
            MonosachcarideDictionary.Add(NAcetylhexosamine.ShortName, NAcetylhexosamine);
            MonosachcarideDictionary.Add(NeuraminicAcid.ShortName, NeuraminicAcid);
            MonosachcarideDictionary.Add(NGlycolylNeuraminicAcid.ShortName, NGlycolylNeuraminicAcid);
            MonosachcarideDictionary.Add(Pentose.ShortName, Pentose);
            
            return MonosachcarideDictionary;
        }

    }
}
