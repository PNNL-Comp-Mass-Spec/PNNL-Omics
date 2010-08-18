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

            //Deoxyhexose.NewElements(C H N O S P)

            Monosaccharide deoxyhexose = new Monosaccharide();
            deoxyhexose.NewElements(6, 10, 0, 4, 0, 0);
            deoxyhexose.Name = "Deoxyhexose";
            deoxyhexose.ShortName = "DxyHex";
            deoxyhexose.SixLetterCode = "DxyHex";
            deoxyhexose.ChemicalFormula = "C6H10O4";
            deoxyhexose.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(deoxyhexose);

            Monosaccharide hexose = new Monosaccharide();
            hexose.NewElements(6, 10, 0, 5, 0, 0);
            hexose.Name = "Hexose";
            hexose.ShortName = "Hex";
            hexose.SixLetterCode = "Hexose";
            hexose.ChemicalFormula = "C6H10O5";
            hexose.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(hexose);

            Monosaccharide hexA = new Monosaccharide();
            hexA.NewElements(6, 8, 0, 6, 0, 0);
            hexA.Name = "Hexuronic Acid";
            hexA.ShortName = "HexA";
            hexA.SixLetterCode = "Hex A ";
            hexA.ChemicalFormula = "C6H8O6";
            hexA.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(hexA);

            //2-Keto-3-Deoxy-D-Glycero-D-Galacto-Nononic-Acid
            Monosaccharide KDNDeaminatedNeuraminicAcid = new Monosaccharide();
            KDNDeaminatedNeuraminicAcid.NewElements(9, 14, 0, 8, 0, 0);
            KDNDeaminatedNeuraminicAcid.Name = "(KDN) 2-Keto-3-Deoxy-D-Glycero-D-Galacto-Nononic-Acid";
            KDNDeaminatedNeuraminicAcid.ShortName = "KDN";
            KDNDeaminatedNeuraminicAcid.SixLetterCode = "KDN   ";
            KDNDeaminatedNeuraminicAcid.ChemicalFormula = "C9H14O8";
            KDNDeaminatedNeuraminicAcid.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(KDNDeaminatedNeuraminicAcid);

            Monosaccharide NAcetylhexosamine = new Monosaccharide();
            NAcetylhexosamine.NewElements(8, 13, 1, 5, 0, 0);
            NAcetylhexosamine.Name = "N-acetylhexosamine";
            NAcetylhexosamine.ShortName = "HexNAc";
            NAcetylhexosamine.SixLetterCode = "HexNAc";
            NAcetylhexosamine.ChemicalFormula = "C8H13NO5";
            NAcetylhexosamine.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(NAcetylhexosamine);

            Monosaccharide neuraminicAcid = new Monosaccharide();
            neuraminicAcid.NewElements(11, 17, 1, 8, 0, 0);
            neuraminicAcid.Name = "Neuraminic Acid";
            neuraminicAcid.ShortName = "NeuAc";
            neuraminicAcid.SixLetterCode = "Neu5Ac";
            neuraminicAcid.ChemicalFormula = "C11H17NO8";
            neuraminicAcid.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(neuraminicAcid);

            Monosaccharide NGlycolylNeuraminicAcid = new Monosaccharide();
            NGlycolylNeuraminicAcid.NewElements(11, 17, 1, 9, 0, 0);
            NGlycolylNeuraminicAcid.Name = "N-glycolylneuraminic Acid";
            NGlycolylNeuraminicAcid.ShortName = "NeuGc";
            NGlycolylNeuraminicAcid.SixLetterCode = "Neu5Gc";
            NGlycolylNeuraminicAcid.ChemicalFormula = "C11H17NO9";
            NGlycolylNeuraminicAcid.MonoIsotopicMass = Monosaccharide.GetMonoisotopicMass(NGlycolylNeuraminicAcid);

            Monosaccharide pentose = new Monosaccharide();
            pentose.NewElements(6, 10, 5, 0, 0, 0);
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
}
