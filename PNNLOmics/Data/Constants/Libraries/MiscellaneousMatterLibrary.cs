﻿using System.Collections.Generic;

//TODO: SCOTT - CR - Update all dictionary example xml comments.
// <example>
// Dictionary implementation
// Dictionary<string, Compound> OtherMoleculeDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
// double OtherMass = OtherMoleculeDictionary["Aldehyde"].MonoIsotopicMass;
// string OtherName = OtherMoleculeDictionary["Aldehyde"].Name;
// string OtherFormula = OtherMoleculeDictionary["Aldehyde"].ChemicalFormula;
//
// One line implementation
// double OtherMass2 = OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("Aldehyde");
// string OtherName2 = OtherMoleculeConstantsStaticLibrary.GetName("Aldehyde");
// string OtherFormula2 = OtherMoleculeConstantsStaticLibrary.GetFormula("Aldehyde");
//
// double mass5 = OtherMoleculeStaticLibrary.GetMonoisotopicMass(SelectOtherMolecule.Ammonia);
// </example>

namespace PNNLOmics.Data.Constants.Libraries
{
    /// <summary>
    /// Sets up the miscellaneous matter compound library and calculates its monoisotopic masses from its elemental composition
    /// </summary>
    public class MiscellaneousMatterLibrary : MatterLibrary<Compound, MiscellaneousMatterName>
    {
        /// <summary>
        /// This is a Class designed to create other molecules from the elements.
        /// The other molecules are added to a Dictionary searchable by char keys such as "Aldehyde" for Aldehyde group
        /// </summary>
        public override void LoadLibrary()
        {
            m_symbolToCompoundMap = new Dictionary<string, Compound>();
            m_enumToSymbolMap = new Dictionary<MiscellaneousMatterName, string>();

            //each integer stands for the number of atoms in the compound -->X.NewElements(C H N O S P)
            //aldehyde.NewElements(C H N O S P)

            var aldehyde = new Compound();
            aldehyde.NewElements(0, 2, 0, 1, 0, 0);//-->X.NewElements(C H N O S P)
            aldehyde.Name = "Aldehyde";
            aldehyde.Symbol = "Aldhyde";
            aldehyde.ChemicalFormula = "H2O";
            aldehyde.MassMonoIsotopic = Compound.GetMonoisotopicMass(aldehyde);

            var alditol = new Compound();
            alditol.NewElements(0, 4, 0, 1, 0, 0);//-->X.NewElements(C H N O S P)
            alditol.Name = "Alditol";
            alditol.Symbol = "Aldtol";
            alditol.ChemicalFormula = "H4O";
            alditol.MassMonoIsotopic = Compound.GetMonoisotopicMass(alditol);

            var ammonia = new Compound();
            ammonia.NewElements(0, 3, 1, 0, 0, 0);//-->X.NewElements(C H N O S P)
            ammonia.Name = "Ammonia";
            ammonia.Symbol = "NH3   ";
            ammonia.ChemicalFormula = "NH3";
            ammonia.MassMonoIsotopic = Compound.GetMonoisotopicMass(ammonia);

            var ammonium = new Compound();
            ammonium.NewElements(0, 4, 1, 0, 0, 0);//-->X.NewElements(C H N O S P)
            ammonium.Name = "Ammonium";
            ammonium.Symbol = "NH4+  ";
            ammonium.ChemicalFormula = "NH4+";
            ammonium.MassMonoIsotopic = Compound.GetMonoisotopicMass(ammonium);

            var KMinusH = new Compound();
            KMinusH.NewElements(0, -1, 0, 0, 0, 0);//-->X.NewElements(C H N O S P)
            KMinusH.NumPotassium = 1;
            KMinusH.Name = "KMinusH";
            KMinusH.Symbol = "KminH ";
            KMinusH.ChemicalFormula = "K-H";
            KMinusH.MassMonoIsotopic = Compound.GetMonoisotopicMass(KMinusH);

            var methyl = new Compound();
            methyl.NewElements(1, 3, 0, 0, 0, 0);//-->X.NewElements(C H N O S P)
            methyl.Name = "Methyl";
            methyl.Symbol = "CH3   ";
            methyl.ChemicalFormula = "CH3";
            methyl.MassMonoIsotopic = Compound.GetMonoisotopicMass(methyl);

            var NaMinusH = new Compound();
            NaMinusH.NewElements(0, -1, 0, 0, 0, 0);//-->X.NewElements(C H N O S P)
            NaMinusH.NumSodium = 1;
            NaMinusH.Name = "NaMinusH";
            NaMinusH.Symbol = "NaminH";
            NaMinusH.ChemicalFormula = "Na-H";
            NaMinusH.MassMonoIsotopic = Compound.GetMonoisotopicMass(NaMinusH);

            var oAcetyl = new Compound();
            oAcetyl.NewElements(2, 2, 0, 1, 0, 0);//-->X.NewElements(C H N O S P)
            oAcetyl.Name = "O-acetyl";
            oAcetyl.Symbol = "OAc";
            oAcetyl.ChemicalFormula = "C2H2O";
            oAcetyl.MassMonoIsotopic = Compound.GetMonoisotopicMass(oAcetyl);

            var sulfate = new Compound();
            sulfate.NewElements(0, 0, 0, 4, 2, 0);//-->X.NewElements(C H N O S P)
            sulfate.Name = "Sulfate";
            sulfate.Symbol = "SO4   ";
            sulfate.ChemicalFormula = "S04";
            sulfate.MassMonoIsotopic = Compound.GetMonoisotopicMass(sulfate);

            var water = new Compound();
            water.NewElements(0, 2, 0, 1, 0, 0);//-->X.NewElements(C H N O S P)
            water.Name = "Water";
            water.Symbol = "Water ";
            water.ChemicalFormula = "H2O";
            water.MassMonoIsotopic = Compound.GetMonoisotopicMass(water);

            var aminoGlycan = new Compound();
            aminoGlycan.NewElements(0, 3, 1, 0, 0, 0);//-->X.NewElements(C H N O S P)
            aminoGlycan.Name = "AminoGlycan";
            aminoGlycan.Symbol = "NH3Gly ";
            aminoGlycan.ChemicalFormula = "NH3";
            aminoGlycan.MassMonoIsotopic = Compound.GetMonoisotopicMass(aminoGlycan);

            var fragment = new Compound();
            fragment.NewElements(0, 0, 0, 0, 0, 0);//-->X.NewElements(C H N O S P)
            fragment.Name = "Fragment";
            fragment.Symbol = "Fragmt";
            fragment.ChemicalFormula = " - ";
            fragment.MassMonoIsotopic = 0;

            m_symbolToCompoundMap.Add(aldehyde.Symbol, aldehyde);
            m_symbolToCompoundMap.Add(alditol.Symbol, alditol);
            m_symbolToCompoundMap.Add(ammonia.Symbol, ammonia);
            m_symbolToCompoundMap.Add(ammonium.Symbol, ammonium);
            m_symbolToCompoundMap.Add(KMinusH.Symbol, KMinusH);
            m_symbolToCompoundMap.Add(methyl.Symbol, methyl);
            m_symbolToCompoundMap.Add(NaMinusH.Symbol, NaMinusH);
            m_symbolToCompoundMap.Add(oAcetyl.Symbol, oAcetyl);
            m_symbolToCompoundMap.Add(sulfate.Symbol, sulfate);
            m_symbolToCompoundMap.Add(water.Symbol, water);
            m_symbolToCompoundMap.Add(aminoGlycan.Symbol, aminoGlycan);
            m_symbolToCompoundMap.Add(fragment.Symbol, fragment);


            m_enumToSymbolMap.Add(MiscellaneousMatterName.Aldehyde, aldehyde.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.Alditol, alditol.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.Ammonia, ammonia.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.Ammonium, ammonium.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.KMinusH, KMinusH.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.Methyl, methyl.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.NaMinusH, NaMinusH.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.OAcetyl, oAcetyl.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.Sulfate, sulfate.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.Water, water.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.AminoGlycan, aminoGlycan.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.Fragment, fragment.Symbol);
        }
    }
}
