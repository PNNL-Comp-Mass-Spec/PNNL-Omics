using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

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

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    public class MiscellaneousMatterLibrary
    {
        /// <summary>
        /// This is a Class designed to create other molecules from the elements.
        /// The other molecules are added to a Dictionary searchable by char keys such as "Aldehyde" for Aldehyde group
        /// </summary>
        public static Dictionary<string, Compound> LoadMiscellaneousMatterData()
        {
            Dictionary<string, Compound> otherMoleculeDictionary = new Dictionary<string, Compound>();

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
    #region old static library code
    ///// <summary>
    ///// This is a Class designed to convert dictionary calls for Other Molecules in one line static method calls.
    ///// </summary>
    //public class MiscellaneousMatterStaticLibrary
    //{
    //    public static double GetMonoisotopicMass(string constantKey)
    //    {
    //        CompoundSingleton NewSingleton = CompoundSingleton.Instance;
    //        NewSingleton.InitializeMiscellaneousMatterLibrary();
    //        Dictionary<string, Compound> incommingDictionary = NewSingleton.MiscellaneousMatterConstantsDictionary;
    //        return incommingDictionary[constantKey].MassMonoIsotopic;
    //    }

    //    public static string GetFormula(string constantKey)
    //    {
    //        CompoundSingleton NewSingleton = CompoundSingleton.Instance;
    //        Dictionary<string, Compound> incommingDictionary = NewSingleton.MiscellaneousMatterConstantsDictionary;
    //        return incommingDictionary[constantKey].ChemicalFormula;
    //    }

    //    public static string GetName(string constantKey)
    //    {
    //        CompoundSingleton NewSingleton = CompoundSingleton.Instance;
    //        Dictionary<string, Compound> incommingDictionary = NewSingleton.MiscellaneousMatterConstantsDictionary;
    //        return incommingDictionary[constantKey].Name;
    //    }

    //    public static string GetSymbol(string constantKey)
    //    {
    //        CompoundSingleton NewSingleton = CompoundSingleton.Instance;
    //        Dictionary<string, Compound> incommingDictionary = NewSingleton.MiscellaneousMatterConstantsDictionary;
    //        return incommingDictionary[constantKey].Symbol;
    //    }

    //    //overload to allow for SelectElement
    //    public static double GetMonoisotopicMass(SelectMiscellaneousMatter selectKey)
    //    {
    //        CompoundSingleton NewSingleton = CompoundSingleton.Instance;
    //        Dictionary<string, Compound> incommingDictionary = NewSingleton.MiscellaneousMatterConstantsDictionary;
    //        Dictionary<int, string> enumConverter = NewSingleton.MiscellaneousMatterConstantsEnumDictionary;
    //        string constantKey = enumConverter[(int)selectKey];
    //        return incommingDictionary[constantKey].MassMonoIsotopic;
    //    }

    //    public static string GetFormula(SelectMiscellaneousMatter selectKey)
    //    {
    //        CompoundSingleton NewSingleton = CompoundSingleton.Instance;
    //        Dictionary<string, Compound> incommingDictionary = NewSingleton.MiscellaneousMatterConstantsDictionary;
    //        Dictionary<int, string> enumConverter = NewSingleton.MiscellaneousMatterConstantsEnumDictionary;
    //        string constantKey = enumConverter[(int)selectKey];
    //        return incommingDictionary[constantKey].ChemicalFormula;
    //    }

    //    public static string GetName(SelectMiscellaneousMatter selectKey)
    //    {
    //        CompoundSingleton NewSingleton = CompoundSingleton.Instance;
    //        Dictionary<string, Compound> incommingDictionary = NewSingleton.MiscellaneousMatterConstantsDictionary;
    //        Dictionary<int, string> enumConverter = NewSingleton.MiscellaneousMatterConstantsEnumDictionary;
    //        string constantKey = enumConverter[(int)selectKey];
    //        return incommingDictionary[constantKey].Name;
    //    }

    //    public static string GetSymbol(SelectMiscellaneousMatter selectKey)
    //    {
    //        CompoundSingleton NewSingleton = CompoundSingleton.Instance;
    //        Dictionary<string, Compound> incommingDictionary = NewSingleton.MiscellaneousMatterConstantsDictionary;
    //        Dictionary<int, string> enumConverter = NewSingleton.MiscellaneousMatterConstantsEnumDictionary;
    //        string constantKey = enumConverter[(int)selectKey];
    //        return incommingDictionary[constantKey].Symbol;
    //    }
    //}
    #endregion

    public enum SelectMiscellaneousMatter
    {
        Aldehyde, Alditol, Ammonia, Ammonium, KMinusH, NaMinusH, Sulfate, Water, AminoGlycan
    }
}
