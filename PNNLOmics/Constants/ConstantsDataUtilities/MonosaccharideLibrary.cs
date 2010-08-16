using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Constants.ConstantsObjectsDataLayer;

//dictionary implementation
//Dictionary<string, MonosaccharideObject> OligosacchaideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
//double OSmass = OligosacchaideDictionary["Hex"].MonoIsotopicMass;
//string OSname = OligosacchaideDictionary["Hex"].Name;
//string OSformula = OligosacchaideDictionary["Hex"].ChemicalFormula;

namespace PNNLOmics.Constants.ConstantsDataUtilities
{
    public class MonosaccharideLibrary
    {
        public static Dictionary<string, MonosaccharideObject> LoadMonosaccharideData()
        {
            Dictionary<string, MonosaccharideObject> monosachcarideDictionary = new Dictionary<string, MonosaccharideObject>();

            //Deoxyhexose.NewElements(C H N O S P)

            MonosaccharideObject deoxyhexose = new MonosaccharideObject();
            deoxyhexose.NewElements(6, 10, 0, 4, 0, 0);
            deoxyhexose.Name = "Deoxyhexose";
            deoxyhexose.ShortName = "DxyHex";
            deoxyhexose.SixLetterCode = "DxyHex";
            deoxyhexose.ChemicalFormula = "C6H10O4";
            deoxyhexose.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(deoxyhexose);

            MonosaccharideObject hexose = new MonosaccharideObject();
            hexose.NewElements(6, 10, 0, 5, 0, 0);
            hexose.Name = "Hexose";
            hexose.ShortName = "Hex";
            hexose.SixLetterCode = "Hexose";
            hexose.ChemicalFormula = "C6H10O5";
            hexose.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(hexose);

            MonosaccharideObject hexA = new MonosaccharideObject();
            hexA.NewElements(6, 8, 0, 6, 0, 0);
            hexA.Name = "Hexuronic Acid";
            hexA.ShortName = "HexA";
            hexA.SixLetterCode = "Hex A ";
            hexA.ChemicalFormula = "C6H8O6";
            hexA.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(hexA);

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

            MonosaccharideObject neuraminicAcid = new MonosaccharideObject();
            neuraminicAcid.NewElements(11, 17, 1, 8, 0, 0);
            neuraminicAcid.Name = "Neuraminic Acid";
            neuraminicAcid.ShortName = "NeuAc";
            neuraminicAcid.SixLetterCode = "Neu5Ac";
            neuraminicAcid.ChemicalFormula = "C11H17NO8";
            neuraminicAcid.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(neuraminicAcid);

            MonosaccharideObject NGlycolylNeuraminicAcid = new MonosaccharideObject();
            NGlycolylNeuraminicAcid.NewElements(11, 17, 1, 9, 0, 0);
            NGlycolylNeuraminicAcid.Name = "N-glycolylneuraminic Acid";
            NGlycolylNeuraminicAcid.ShortName = "NeuGc";
            NGlycolylNeuraminicAcid.SixLetterCode = "Neu5Gc";
            NGlycolylNeuraminicAcid.ChemicalFormula = "C11H17NO9";
            NGlycolylNeuraminicAcid.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(NGlycolylNeuraminicAcid);

            MonosaccharideObject pentose = new MonosaccharideObject();
            pentose.NewElements(6, 10, 5, 0, 0, 0);
            pentose.Name = "Pentose";
            pentose.ShortName = "Pent";
            pentose.SixLetterCode = "Pentos";
            pentose.ChemicalFormula = "C6H10O5";
            pentose.MonoIsotopicMass = MonosaccharideObject.GetMonoisotopicMass(pentose);

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
}
