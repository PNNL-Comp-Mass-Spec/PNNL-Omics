using System;
using System.Collections.Generic;
using PNNLOmics.Data.Constants;
using PNNLOmics.Data.Constants.Enumerations;

//TODO: SCOTT - CR - Update all dictionary example xml comments.
/// <example>
/// dictionarty implementation
/// Dictionary<string, Compound> OtherMoleculeDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
/// double OtherMass = OtherMoleculeDictionary["Aldehyde"].MonoIsotopicMass;
/// string OtherName = OtherMoleculeDictionary["Aldehyde"].Name;
/// string OtherFormula = OtherMoleculeDictionary["Aldehyde"].ChemicalFormula;
///
/// One line implememtation
/// double OtherMass2 = OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("Aldehyde");
/// string OtherName2 = OtherMoleculeConstantsStaticLibrary.GetName("Aldehyde");
/// string OtherFormula2 = OtherMoleculeConstantsStaticLibrary.GetFormula("Aldehyde");
///
/// double mass5 = OtherMoleculeStaticLibrary.GetMonoisotopicMass(SelectOtherMolecule.Ammonia);
/// </example>

namespace PNNLOmics.Data.Constants.Utilities
{
    //TODO: SCOTT - CR - add XML comments
    public class MiscellaneousMatterLibrary : MatterLibrary<Compound, MiscellaneousMatterName>
    {
        /// <summary>
        /// This is a Class designed to create other molecules from the elements.
        /// The other molecules are added to a Dictionary searchable by char keys such as "Aldehyde" for Aldehyde group
        /// </summary>
        public override Dictionary<string, Compound> LoadLibrary()
        {
            m_symbolToCompoundMap = new Dictionary<string, Compound>();
            m_enumToSymbolMap = new Dictionary<MiscellaneousMatterName, string>();

            //each integer stands for the number of atoms in the compound -->X.NewElements(C H N O S P)
            //aldehyde.NewElements(C H N O S P)

            Compound aldehyde = new Compound();
            aldehyde.NewElements(0, 2, 0, 1, 0, 0);//-->X.NewElements(C H N O S P)
            aldehyde.Name = "Aldehyde";
            aldehyde.Symbol = "Aldhyd";
            aldehyde.ChemicalFormula = "H2O";
            aldehyde.MassMonoIsotopic = Compound.GetMonoisotopicMass(aldehyde);

            Compound alditol = new Compound();
            alditol.NewElements(0, 4, 0, 1, 0, 0);//-->X.NewElements(C H N O S P)
            alditol.Name = "Alditol";
            alditol.Symbol = "Aldtol";
            alditol.ChemicalFormula = "H4O";
            alditol.MassMonoIsotopic = Compound.GetMonoisotopicMass(alditol);

            Compound ammonia = new Compound();
            ammonia.NewElements(0, 3, 1, 0, 0, 0);//-->X.NewElements(C H N O S P)
            ammonia.Name = "Ammonia";
            ammonia.Symbol = "NH3   ";
            ammonia.ChemicalFormula = "NH3";
            ammonia.MassMonoIsotopic = Compound.GetMonoisotopicMass(ammonia);

            Compound ammonium = new Compound();
            ammonium.NewElements(0, 4, 1, 0, 0, 0);//-->X.NewElements(C H N O S P)
            ammonium.Name = "Ammonium";
            ammonium.Symbol = "NH4+  ";
            ammonium.ChemicalFormula = "NH4+";
            ammonium.MassMonoIsotopic = Compound.GetMonoisotopicMass(ammonium);

            Compound KMinusH = new Compound();
            KMinusH.NewElements(0, -1, 0, 0, 0, 0);//-->X.NewElements(C H N O S P)
            KMinusH.NumPotassium = 1;
            KMinusH.Name = "KMinusH";
            KMinusH.Symbol = "KminH ";
            KMinusH.ChemicalFormula = "K-H";
            KMinusH.MassMonoIsotopic = Compound.GetMonoisotopicMass(KMinusH);

            Compound NaMinusH = new Compound();
            NaMinusH.NewElements(0, -1, 0, 0, 0, 0);//-->X.NewElements(C H N O S P)
            NaMinusH.NumSodium = 1;
            NaMinusH.Name = "NaMinusH";
            NaMinusH.Symbol = "NaminH";
            NaMinusH.ChemicalFormula = "Na-H";
            NaMinusH.MassMonoIsotopic = Compound.GetMonoisotopicMass(NaMinusH);

            Compound sulfate = new Compound();
            sulfate.NewElements(0, 0, 0, 4, 2, 0);//-->X.NewElements(C H N O S P)
            sulfate.Name = "Sulfate";
            sulfate.Symbol = "SO4   ";
            sulfate.ChemicalFormula = "S04";
            sulfate.MassMonoIsotopic = Compound.GetMonoisotopicMass(sulfate);

            Compound water = new Compound();
            water.NewElements(0, 2, 0, 1, 0, 0);//-->X.NewElements(C H N O S P)
            water.Name = "Water";
            water.Symbol = "Water ";
            water.ChemicalFormula = "H2O";
            water.MassMonoIsotopic = Compound.GetMonoisotopicMass(water);

            Compound aminoGlycan = new Compound();
            aminoGlycan.NewElements(0, 3, 1, 0, 0, 0);//-->X.NewElements(C H N O S P)
            aminoGlycan.Name = "AminoGlycan";
            aminoGlycan.Symbol = "NH3Gly ";
            aminoGlycan.ChemicalFormula = "NH3";
            aminoGlycan.MassMonoIsotopic = Compound.GetMonoisotopicMass(aminoGlycan);

            m_symbolToCompoundMap.Add(aldehyde.Symbol, aldehyde);
            m_symbolToCompoundMap.Add(alditol.Symbol, alditol);
            m_symbolToCompoundMap.Add(ammonia.Symbol, ammonia);
            m_symbolToCompoundMap.Add(ammonium.Symbol, ammonium);
            m_symbolToCompoundMap.Add(KMinusH.Symbol, KMinusH);
            m_symbolToCompoundMap.Add(NaMinusH.Symbol, NaMinusH);
            m_symbolToCompoundMap.Add(sulfate.Symbol, sulfate);
            m_symbolToCompoundMap.Add(water.Symbol, water);
            m_symbolToCompoundMap.Add(aminoGlycan.Symbol, aminoGlycan);

            m_enumToSymbolMap.Add(MiscellaneousMatterName.Aldehyde, aldehyde.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.Alditol, alditol.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.Ammonia, ammonia.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.Ammonium, ammonium.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.KMinusH, KMinusH.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.NaMinusH, NaMinusH.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.Sulfate, sulfate.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.Water, water.Symbol);
            m_enumToSymbolMap.Add(MiscellaneousMatterName.AminoGlycan, aminoGlycan.Symbol);

            return m_symbolToCompoundMap;
        }
    }
}
