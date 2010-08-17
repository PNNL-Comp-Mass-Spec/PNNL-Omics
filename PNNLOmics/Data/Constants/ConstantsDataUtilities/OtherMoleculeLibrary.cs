using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

//dictionarty implementation
//Dictionary<string, OtherMoleculeObject> OtherMoleculeDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
//double OtherMass = OtherMoleculeDictionary["Aldehyde"].MonoIsotopicMass;
//string OtherName = OtherMoleculeDictionary["Aldehyde"].Name;
//string OtherFormula = OtherMoleculeDictionary["Aldehyde"].ChemicalFormula;

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    public class OtherMoleculeLibrary
    {
        public static Dictionary<string, OtherMolecule> LoadOtherMoleculeData()
        {
            Dictionary<string, OtherMolecule> otherMoleculeDictionary = new Dictionary<string, OtherMolecule>();

            //Aldehyde.NewElements(C H N O S P)

            OtherMolecule aldehyde = new OtherMolecule();
            aldehyde.NewElements(0, 2, 0, 1, 0, 0);
            aldehyde.Name = "Aldehyde";
            aldehyde.SixLetterCode = "Aldhyd";
            aldehyde.ChemicalFormula = "H2O";
            aldehyde.MonoIsotopicMass = OtherMolecule.GetMonoisotopicMass(aldehyde);

            OtherMolecule alditol = new OtherMolecule();
            alditol.NewElements(0, 4, 0, 1, 0, 0);
            alditol.Name = "Alditol";
            alditol.SixLetterCode = "Aldtol";
            alditol.ChemicalFormula = "H4O";
            alditol.MonoIsotopicMass = OtherMolecule.GetMonoisotopicMass(alditol);

            OtherMolecule ammonia = new OtherMolecule();
            ammonia.NewElements(0, 3, 1, 0, 0, 0);
            ammonia.Name = "Ammonia";
            ammonia.SixLetterCode = "NH3   ";
            ammonia.ChemicalFormula = "NH3";
            ammonia.MonoIsotopicMass = OtherMolecule.GetMonoisotopicMass(ammonia);

            OtherMolecule ammonium = new OtherMolecule();
            ammonium.NewElements(0, 4, 1, 0, 0, 0);
            ammonium.Name = "Ammonium";
            ammonium.SixLetterCode = "NH4+  ";
            ammonium.ChemicalFormula = "NH4+";
            ammonium.MonoIsotopicMass = OtherMolecule.GetMonoisotopicMass(ammonium);

            OtherMolecule KMinusH = new OtherMolecule();
            KMinusH.NewElements(0, -1, 0, 0, 0, 0);
            KMinusH.nPotassium = 1;
            KMinusH.Name = "KMinusH";
            KMinusH.SixLetterCode = "KminH ";
            KMinusH.ChemicalFormula = "K-H";
            KMinusH.MonoIsotopicMass = OtherMolecule.GetMonoisotopicMass(KMinusH);

            OtherMolecule NaMinusH = new OtherMolecule();
            NaMinusH.NewElements(0, -1, 0, 0, 0, 0);
            NaMinusH.nSodium = 1;
            NaMinusH.Name = "NaMinusH";
            NaMinusH.SixLetterCode = "NaminH";
            NaMinusH.ChemicalFormula = "Na-H";
            NaMinusH.MonoIsotopicMass = OtherMolecule.GetMonoisotopicMass(NaMinusH);

            OtherMolecule sulfate = new OtherMolecule();
            sulfate.NewElements(0, 0, 0, 4, 2, 0);
            sulfate.Name = "Sulfate";
            sulfate.SixLetterCode = "SO4   ";
            sulfate.ChemicalFormula = "S04";
            sulfate.MonoIsotopicMass = OtherMolecule.GetMonoisotopicMass(sulfate);

            OtherMolecule water = new OtherMolecule();
            water.NewElements(0, 2, 0, 1, 0, 0);
            water.Name = "Water";
            water.SixLetterCode = "Water ";
            water.ChemicalFormula = "H2O";
            water.MonoIsotopicMass = OtherMolecule.GetMonoisotopicMass(water);

            otherMoleculeDictionary.Add(aldehyde.Name, aldehyde);
            otherMoleculeDictionary.Add(alditol.Name, alditol);
            otherMoleculeDictionary.Add(ammonia.Name, ammonia);
            otherMoleculeDictionary.Add(ammonium.Name, ammonium);
            otherMoleculeDictionary.Add(KMinusH.Name, KMinusH);
            otherMoleculeDictionary.Add(NaMinusH.Name, NaMinusH);
            otherMoleculeDictionary.Add(sulfate.Name, sulfate);
            otherMoleculeDictionary.Add(water.Name, water);
            
            return otherMoleculeDictionary;
        }

    }
}
