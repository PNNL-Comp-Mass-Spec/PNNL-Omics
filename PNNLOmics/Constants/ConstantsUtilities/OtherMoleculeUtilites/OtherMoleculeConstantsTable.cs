using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//main call
//string chemicalFormula = OtherMoleculeConstantsTable.GetFormula("Water");
//double mass = OtherMoleculeConstantsTable.GetMass("Water");
//string name = OtherMoleculeConstantsTable.GetName("Water");
//string name = OtherMoleculeConstantsTable.GetName6("Water");


namespace Constants
{
    interface IOtherMoleculeConstants
    {
        void MassTable(Dictionary<string, double> DataDictionary);
        void FormulaTable(Dictionary<string, string> DataDictionary);
        void NameTable(Dictionary<string, string> DataDictionary);
        void SixLetterTable(Dictionary<string, string> DataDictionary);
    }
    public class OtherMoleculeConstantsTable : IOtherMoleculeConstants
    {

        public static double GetMass(string IDletter)
        {
            Dictionary<string, double> MassDictionary = new Dictionary<string, double>();
            IOtherMoleculeConstants newTable = new OtherMoleculeConstantsTable();
            newTable.MassTable(MassDictionary);
            return MassDictionary[IDletter];
        }

        public static string GetFormula(string IDletter)
        {
            Dictionary<string, string> FormulaDictionary = new Dictionary<string, string>();
            IOtherMoleculeConstants newTable = new OtherMoleculeConstantsTable();
            newTable.FormulaTable(FormulaDictionary);
            return FormulaDictionary[IDletter];
        }

        public static string GetName(string IDletter)
        {
            Dictionary<string, string> NameDictionary = new Dictionary<string, string>();
            IOtherMoleculeConstants newTable = new OtherMoleculeConstantsTable();
            newTable.NameTable(NameDictionary);
            return NameDictionary[IDletter];
        }

        public static string GetName6(string IDletter)
        {
            Dictionary<string, string> SixLetterNameDictionary = new Dictionary<string, string>();
            IOtherMoleculeConstants newTable = new OtherMoleculeConstantsTable();
            newTable.SixLetterTable(SixLetterNameDictionary);
            return SixLetterNameDictionary[IDletter];
        }

        #region interface functions for MassTable, FormulaTable, NameTable, and SixLetterTable

        void IOtherMoleculeConstants.MassTable(Dictionary<string, double> DataDictionary)
        {
            Aldehyde mAldehyde = new Aldehyde();
            Alditol mAlditol = new Alditol();
            Ammonia mAmmonia = new Ammonia();
            Ammonium mAmmonium = new Ammonium();
            KMinusH mKMinusH = new KMinusH();
            NaMinusH mNaMinusH = new NaMinusH();
            Sulfate mSulfate = new Sulfate();
            Water mWater = new Water();
            

            DataDictionary.Add("Aldehyde", mAldehyde.MonoIsotopicMass);
            DataDictionary.Add("Alditol", mAlditol.MonoIsotopicMass);
            DataDictionary.Add("Ammonia", mAmmonia.MonoIsotopicMass);
            DataDictionary.Add("Ammonium", mAmmonium.MonoIsotopicMass);
            DataDictionary.Add("KNinusH", mKMinusH.MonoIsotopicMass);
            DataDictionary.Add("NaMinusH", mNaMinusH.MonoIsotopicMass);
            DataDictionary.Add("Sulfate", mSulfate.MonoIsotopicMass);
            DataDictionary.Add("Water", mWater.MonoIsotopicMass);           
        }

        void IOtherMoleculeConstants.FormulaTable(Dictionary<string, string> DataDictionary)
        {
            Aldehyde mAldehyde = new Aldehyde();
            Alditol mAlditol = new Alditol();
            Ammonia mAmmonia = new Ammonia();
            Ammonium mAmmonium = new Ammonium();
            KMinusH mKMinusH = new KMinusH();
            NaMinusH mNaMinusH = new NaMinusH();
            Sulfate mSulfate = new Sulfate();
            Water mWater = new Water();

            DataDictionary.Add("Aldehyde", mAldehyde.ChemicalFormula);
            DataDictionary.Add("Alditol", mAlditol.ChemicalFormula);
            DataDictionary.Add("Ammonia", mAmmonia.ChemicalFormula);
            DataDictionary.Add("Ammonium", mAmmonium.ChemicalFormula);
            DataDictionary.Add("KNinusH", mKMinusH.ChemicalFormula);
            DataDictionary.Add("NaMinusH", mNaMinusH.ChemicalFormula);
            DataDictionary.Add("Sulfate", mSulfate.ChemicalFormula);
            DataDictionary.Add("Water", mWater.ChemicalFormula);            
        }

        void IOtherMoleculeConstants.NameTable(Dictionary<string, string> DataDictionary)
        {
            Aldehyde mAldehyde = new Aldehyde();
            Alditol mAlditol = new Alditol();
            Ammonia mAmmonia = new Ammonia();
            Ammonium mAmmonium = new Ammonium();
            KMinusH mKMinusH = new KMinusH();
            NaMinusH mNaMinusH = new NaMinusH();
            Sulfate mSulfate = new Sulfate();
            Water mWater = new Water();

            DataDictionary.Add("Aldehyde", mAldehyde.Name);
            DataDictionary.Add("Alditol", mAlditol.Name);
            DataDictionary.Add("Ammonia", mAmmonia.Name);
            DataDictionary.Add("Ammonium", mAmmonium.Name);
            DataDictionary.Add("KNinusH", mKMinusH.Name);
            DataDictionary.Add("NaMinusH", mNaMinusH.Name);
            DataDictionary.Add("Sulfate", mSulfate.Name);
            DataDictionary.Add("Water", mWater.Name);
        }

        void IOtherMoleculeConstants.SixLetterTable(Dictionary<string, string> DataDictionary)
        {
            Aldehyde mAldehyde = new Aldehyde();
            Alditol mAlditol = new Alditol();
            Ammonia mAmmonia = new Ammonia();
            Ammonium mAmmonium = new Ammonium();
            KMinusH mKMinusH = new KMinusH();
            NaMinusH mNaMinusH = new NaMinusH();
            Sulfate mSulfate = new Sulfate();
            Water mWater = new Water();

            DataDictionary.Add("Aldehyde", mAldehyde.SixLetterCode);
            DataDictionary.Add("Alditol", mAlditol.SixLetterCode);
            DataDictionary.Add("Ammonia", mAmmonia.SixLetterCode);
            DataDictionary.Add("Ammonium", mAmmonium.SixLetterCode);
            DataDictionary.Add("KNinusH", mKMinusH.SixLetterCode);
            DataDictionary.Add("NaMinusH", mNaMinusH.SixLetterCode);
            DataDictionary.Add("Sulfate", mSulfate.SixLetterCode);
            DataDictionary.Add("Water", mWater.SixLetterCode);     
        }
        #endregion
    }
}
