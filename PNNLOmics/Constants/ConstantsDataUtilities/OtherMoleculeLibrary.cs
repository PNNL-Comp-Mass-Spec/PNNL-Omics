using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Constants.ConstantsObjectsDataLayer;

//dictionarty implementation
//Dictionary<string, OtherMoleculeObject> OtherMoleculeDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
//double OtherMass = OtherMoleculeDictionary["Aldehyde"].MonoIsotopicMass;
//string OtherName = OtherMoleculeDictionary["Aldehyde"].Name;
//string OtherFormula = OtherMoleculeDictionary["Aldehyde"].ChemicalFormula;

namespace PNNLOmics.Constants.ConstantsDataUtilities
{
    public class OtherMoleculeLibrary
    {
        public static Dictionary<string, OtherMoleculeObject> LoadOtherMoleculeData()
        {
            Dictionary<string, OtherMoleculeObject> otherMoleculeDictionary = new Dictionary<string, OtherMoleculeObject>();

            //Aldehyde.NewElements(C H N O S P)

            OtherMoleculeObject aldehyde = new OtherMoleculeObject();
            aldehyde.NewElements(0, 2, 0, 1, 0, 0);
            aldehyde.Name = "Aldehyde";
            aldehyde.SixLetterCode = "Aldhyd";
            aldehyde.ChemicalFormula = "H2O";
            aldehyde.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(aldehyde);

            OtherMoleculeObject alditol = new OtherMoleculeObject();
            alditol.NewElements(0, 4, 0, 1, 0, 0);
            alditol.Name = "Alditol";
            alditol.SixLetterCode = "Aldtol";
            alditol.ChemicalFormula = "H4O";
            alditol.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(alditol);

            OtherMoleculeObject ammonia = new OtherMoleculeObject();
            ammonia.NewElements(0, 3, 1, 0, 0, 0);
            ammonia.Name = "Ammonia";
            ammonia.SixLetterCode = "NH3   ";
            ammonia.ChemicalFormula = "NH3";
            ammonia.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(ammonia);

            OtherMoleculeObject ammonium = new OtherMoleculeObject();
            ammonium.NewElements(0, 4, 1, 0, 0, 0);
            ammonium.Name = "Ammonium";
            ammonium.SixLetterCode = "NH4+  ";
            ammonium.ChemicalFormula = "NH4+";
            ammonium.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(ammonium);

            OtherMoleculeObject KMinusH = new OtherMoleculeObject();
            KMinusH.NewElements(0, -1, 0, 0, 0, 0);
            KMinusH.nPotassium = 1;
            KMinusH.Name = "KMinusH";
            KMinusH.SixLetterCode = "KminH ";
            KMinusH.ChemicalFormula = "K-H";
            KMinusH.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(KMinusH);

            OtherMoleculeObject NaMinusH = new OtherMoleculeObject();
            NaMinusH.NewElements(0, -1, 0, 0, 0, 0);
            NaMinusH.nSodium = 1;
            NaMinusH.Name = "NaMinusH";
            NaMinusH.SixLetterCode = "NaminH";
            NaMinusH.ChemicalFormula = "Na-H";
            NaMinusH.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(NaMinusH);

            OtherMoleculeObject sulfate = new OtherMoleculeObject();
            sulfate.NewElements(0, 0, 0, 4, 2, 0);
            sulfate.Name = "Sulfate";
            sulfate.SixLetterCode = "SO4   ";
            sulfate.ChemicalFormula = "S04";
            sulfate.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(sulfate);

            OtherMoleculeObject water = new OtherMoleculeObject();
            water.NewElements(0, 2, 0, 1, 0, 0);
            water.Name = "Water";
            water.SixLetterCode = "Water ";
            water.ChemicalFormula = "H2O";
            water.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(water);

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
