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

//One line implememtation
//double OtherMass2 = OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("Aldehyde");
//string OtherName2 = OtherMoleculeConstantsStaticLibrary.GetName("Aldehyde");
//string OtherFormula2 = OtherMoleculeConstantsStaticLibrary.GetFormula("Aldehyde");

//double mass5 = OtherMoleculeStaticLibrary.GetMonoisotopicMass(SelectOtherMolecule.Ammonia);


namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    public class OtherMoleculeLibrary
    {
        /// <summary>
        /// This is a Class designed to create other molecules from the elements.
        /// The other molecules are added to a Dictionary searchable by char keys such as "Aldehyde" for Aldehyde group
        /// </summary>
        public static Dictionary<string, OtherMolecule> LoadOtherMoleculeData()
        {
            Dictionary<string, OtherMolecule> otherMoleculeDictionary = new Dictionary<string, OtherMolecule>();

            //each integer stands for the number of atoms in the compound -->X.NewElements(C H N O S P)
            //aldehyde.NewElements(C H N O S P)

            OtherMolecule aldehyde = new OtherMolecule();
            aldehyde.NewElements(0, 2, 0, 1, 0, 0);//-->X.NewElements(C H N O S P)
            aldehyde.Name = "Aldehyde";
            aldehyde.SixLetterCode = "Aldhyd";
            aldehyde.ChemicalFormula = "H2O";
            aldehyde.MassMonoIsotopic = OtherMolecule.GetMonoisotopicMass(aldehyde);

            OtherMolecule alditol = new OtherMolecule();
            alditol.NewElements(0, 4, 0, 1, 0, 0);//-->X.NewElements(C H N O S P)
            alditol.Name = "Alditol";
            alditol.SixLetterCode = "Aldtol";
            alditol.ChemicalFormula = "H4O";
            alditol.MassMonoIsotopic = OtherMolecule.GetMonoisotopicMass(alditol);

            OtherMolecule ammonia = new OtherMolecule();
            ammonia.NewElements(0, 3, 1, 0, 0, 0);//-->X.NewElements(C H N O S P)
            ammonia.Name = "Ammonia";
            ammonia.SixLetterCode = "NH3   ";
            ammonia.ChemicalFormula = "NH3";
            ammonia.MassMonoIsotopic = OtherMolecule.GetMonoisotopicMass(ammonia);

            OtherMolecule ammonium = new OtherMolecule();
            ammonium.NewElements(0, 4, 1, 0, 0, 0);//-->X.NewElements(C H N O S P)
            ammonium.Name = "Ammonium";
            ammonium.SixLetterCode = "NH4+  ";
            ammonium.ChemicalFormula = "NH4+";
            ammonium.MassMonoIsotopic = OtherMolecule.GetMonoisotopicMass(ammonium);

            OtherMolecule KMinusH = new OtherMolecule();
            KMinusH.NewElements(0, -1, 0, 0, 0, 0);//-->X.NewElements(C H N O S P)
            KMinusH.nPotassium = 1;
            KMinusH.Name = "KMinusH";
            KMinusH.SixLetterCode = "KminH ";
            KMinusH.ChemicalFormula = "K-H";
            KMinusH.MassMonoIsotopic = OtherMolecule.GetMonoisotopicMass(KMinusH);

            OtherMolecule NaMinusH = new OtherMolecule();
            NaMinusH.NewElements(0, -1, 0, 0, 0, 0);//-->X.NewElements(C H N O S P)
            NaMinusH.nSodium = 1;
            NaMinusH.Name = "NaMinusH";
            NaMinusH.SixLetterCode = "NaminH";
            NaMinusH.ChemicalFormula = "Na-H";
            NaMinusH.MassMonoIsotopic = OtherMolecule.GetMonoisotopicMass(NaMinusH);

            OtherMolecule sulfate = new OtherMolecule();
            sulfate.NewElements(0, 0, 0, 4, 2, 0);//-->X.NewElements(C H N O S P)
            sulfate.Name = "Sulfate";
            sulfate.SixLetterCode = "SO4   ";
            sulfate.ChemicalFormula = "S04";
            sulfate.MassMonoIsotopic = OtherMolecule.GetMonoisotopicMass(sulfate);

            OtherMolecule water = new OtherMolecule();
            water.NewElements(0, 2, 0, 1, 0, 0);//-->X.NewElements(C H N O S P)
            water.Name = "Water";
            water.SixLetterCode = "Water ";
            water.ChemicalFormula = "H2O";
            water.MassMonoIsotopic = OtherMolecule.GetMonoisotopicMass(water);

            OtherMolecule aminoGlycan = new OtherMolecule();
            aminoGlycan.NewElements(0, 3, 1, 0, 0, 0);//-->X.NewElements(C H N O S P)
            aminoGlycan.Name = "AminoGlycan";
            aminoGlycan.SixLetterCode = "NH3Gly ";
            aminoGlycan.ChemicalFormula = "NH3";
            aminoGlycan.MassMonoIsotopic = OtherMolecule.GetMonoisotopicMass(aminoGlycan);

            otherMoleculeDictionary.Add(aldehyde.Name, aldehyde);
            otherMoleculeDictionary.Add(alditol.Name, alditol);
            otherMoleculeDictionary.Add(ammonia.Name, ammonia);
            otherMoleculeDictionary.Add(ammonium.Name, ammonium);
            otherMoleculeDictionary.Add(KMinusH.Name, KMinusH);
            otherMoleculeDictionary.Add(NaMinusH.Name, NaMinusH);
            otherMoleculeDictionary.Add(sulfate.Name, sulfate);
            otherMoleculeDictionary.Add(water.Name, water);
            otherMoleculeDictionary.Add(aminoGlycan.Name, aminoGlycan);
            
            return otherMoleculeDictionary;
        }
    }

    /// <summary>
    /// This is a Class designed to convert dictionary calls for Other Molecules in one line static method calls.
    /// </summary>
    public class OtherMoleculeStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].MassMonoIsotopic;
        }

        public static string GetFormula(string constantKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].ChemicalFormula;
        }

        public static string GetName(string constantKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].Name;
        }

        public static string GetName6(string constantKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].SixLetterCode;
        }

        //overload to allow for SelectElement
        public static double GetMonoisotopicMass(SelectOtherMolecule selectKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].MassMonoIsotopic;
        }

        public static string GetFormula(SelectOtherMolecule selectKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].ChemicalFormula;
        }

        public static string GetName(SelectOtherMolecule selectKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].Name;
        }

        public static string GetName6(SelectOtherMolecule selectKey)
        {
            OtherMoleculeSingleton NewSingleton = OtherMoleculeSingleton.Instance;
            Dictionary<string, OtherMolecule> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].SixLetterCode;
        }
    }

    public enum SelectOtherMolecule
    {
        Aldehyde, Alditol, Ammonia, Ammonium, KMinusH, NaMinusH, Sulfate, Water, AminoGlycan
    }
}
