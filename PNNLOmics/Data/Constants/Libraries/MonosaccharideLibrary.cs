using System;
using System.Collections.Generic;
using PNNLOmics.Data.Constants;

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

namespace PNNLOmics.Data.Constants.Libraries
{
    //TODO: SCOTT - CR - add XML comments
    public class MonosaccharideLibrary : MatterLibrary<Compound, MonosaccharideName>
    {        
        //TODO: SCOTT - CR - update XML comments, this is not accurate anymore.
        /// <summary>
        /// This is a Class designed to create Compound objects from the elements.
        /// The monosacchadies are added to a Dictionary searchable by string keys such as "DxyHex" for Deoxyhexose
        /// </summary>
        public override void LoadLibrary()
        {
            //TODO: SCOTT - CR - remove and commented code that is old 

            m_symbolToCompoundMap = new Dictionary<string, Compound>();
            m_enumToSymbolMap = new Dictionary<MonosaccharideName, string>();

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

            m_symbolToCompoundMap.Add(deoxyhexose.Symbol, deoxyhexose);
            m_symbolToCompoundMap.Add(hexose.Symbol, hexose);
            m_symbolToCompoundMap.Add(hexA.Symbol, hexA);
            m_symbolToCompoundMap.Add(KDNDeaminatedNeuraminicAcid.Symbol, KDNDeaminatedNeuraminicAcid);
            m_symbolToCompoundMap.Add(NAcetylhexosamine.Symbol, NAcetylhexosamine);
            m_symbolToCompoundMap.Add(neuraminicAcid.Symbol, neuraminicAcid);
            m_symbolToCompoundMap.Add(NGlycolylNeuraminicAcid.Symbol, NGlycolylNeuraminicAcid);
            m_symbolToCompoundMap.Add(pentose.Symbol, pentose);

            m_enumToSymbolMap.Add(MonosaccharideName.Deoxyhexose, deoxyhexose.Symbol);
            m_enumToSymbolMap.Add(MonosaccharideName.Hexose, hexose.Symbol);
            m_enumToSymbolMap.Add(MonosaccharideName.HexuronicAcid, hexA.Symbol);
            m_enumToSymbolMap.Add(MonosaccharideName.KDN, KDNDeaminatedNeuraminicAcid.Symbol);
            m_enumToSymbolMap.Add(MonosaccharideName.NAcetylhexosamine, NAcetylhexosamine.Symbol);
            m_enumToSymbolMap.Add(MonosaccharideName.NeuraminicAcid, neuraminicAcid.Symbol);
            m_enumToSymbolMap.Add(MonosaccharideName.NGlycolylneuraminicAcid, NGlycolylNeuraminicAcid.Symbol);
            m_enumToSymbolMap.Add(MonosaccharideName.Pentose, pentose.Symbol);
        }
    }
}
