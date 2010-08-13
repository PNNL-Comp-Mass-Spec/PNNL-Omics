using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//dictionarty implementation
//Dictionary<string, OtherMoleculeObject> OtherMoleculeDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
//double OtherMass = OtherMoleculeDictionary["Aldehyde"].MonoIsotopicMass;
//string OtherName = OtherMoleculeDictionary["Aldehyde"].Name;
//string OtherFormula = OtherMoleculeDictionary["Aldehyde"].ChemicalFormula;

namespace Constants
{
    public class OtherMoleculeLibrary
    {
        public static Dictionary<string, OtherMoleculeObject> LoadOtherMoleculeData()
        {
            Dictionary<string, OtherMoleculeObject> OtherMoleculeDictionary = new Dictionary<string, OtherMoleculeObject>();

            //Aldehyde.NewElements(C H N O S P)

            OtherMoleculeObject Aldehyde = new OtherMoleculeObject();
            Aldehyde.NewElements(0, 2, 0, 1, 0, 0);
            Aldehyde.Name = "Aldehyde";
            Aldehyde.SixLetterCode = "Aldhyd";
            Aldehyde.ChemicalFormula = "H2O";
            Aldehyde.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(Aldehyde);

            OtherMoleculeObject Alditol = new OtherMoleculeObject();
            Alditol.NewElements(0, 4, 0, 1, 0, 0);
            Alditol.Name = "Alditol";
            Alditol.SixLetterCode = "Aldtol";
            Alditol.ChemicalFormula = "H4O";
            Alditol.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(Alditol);

            OtherMoleculeObject Ammonia = new OtherMoleculeObject();
            Ammonia.NewElements(0, 3, 1, 0, 0, 0);
            Ammonia.Name = "Ammonia";
            Ammonia.SixLetterCode = "NH3   ";
            Ammonia.ChemicalFormula = "NH3";
            Ammonia.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(Ammonia);

            OtherMoleculeObject Ammonium = new OtherMoleculeObject();
            Ammonium.NewElements(0, 4, 1, 0, 0, 0);
            Ammonium.Name = "Ammonium";
            Ammonium.SixLetterCode = "NH4+  ";
            Ammonium.ChemicalFormula = "NH4+";
            Ammonium.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(Ammonium);

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

            OtherMoleculeObject Sulfate = new OtherMoleculeObject();
            Sulfate.NewElements(0, 0, 0, 4, 2, 0);
            Sulfate.Name = "Sulfate";
            Sulfate.SixLetterCode = "SO4   ";
            Sulfate.ChemicalFormula = "S04";
            Sulfate.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(Sulfate);

            OtherMoleculeObject Water = new OtherMoleculeObject();
            Water.NewElements(0, 2, 0, 1, 0, 0);
            Water.Name = "Water";
            Water.SixLetterCode = "Water ";
            Water.ChemicalFormula = "H2O";
            Water.MonoIsotopicMass = OtherMoleculeObject.GetMonoisotopicMass(Water);

            OtherMoleculeDictionary.Add(Aldehyde.Name, Aldehyde);
            OtherMoleculeDictionary.Add(Alditol.Name, Alditol);
            OtherMoleculeDictionary.Add(Ammonia.Name, Ammonia);
            OtherMoleculeDictionary.Add(Ammonium.Name, Ammonium);
            OtherMoleculeDictionary.Add(KMinusH.Name, KMinusH);
            OtherMoleculeDictionary.Add(NaMinusH.Name, NaMinusH);
            OtherMoleculeDictionary.Add(Sulfate.Name, Sulfate);
            OtherMoleculeDictionary.Add(Water.Name, Water);
            
            return OtherMoleculeDictionary;
        }

    }
}
