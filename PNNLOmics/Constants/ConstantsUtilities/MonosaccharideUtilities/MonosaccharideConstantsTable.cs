using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//main call
//string chemicalFormula = MonosaccharideConstantsTable.GetFormula("Hex");
//double mass = MonosaccharideConstantsTable.GetMass("Hex");
//string name = MonosaccharideConstantsTable.GetName("Hex");
//string name = MonosaccharideConstantsTable.GetName6("Hex");

namespace Constants
{
    interface IMonosaccharideConstants
    {
        void MassTable(Dictionary<string, double> DataDictionary);
        void FormulaTable(Dictionary<string, string> DataDictionary);
        void NameTable(Dictionary<string, string> DataDictionary);
        void SixLetterTable(Dictionary<string, string> DataDictionary);
    }
    public class MonosaccharideConstantsTable : IMonosaccharideConstants
    {

        public static double GetMass(string IDletter)
        {
            Dictionary<string, double> MassDictionary = new Dictionary<string, double>();
            IMonosaccharideConstants newTable = new MonosaccharideConstantsTable();
            newTable.MassTable(MassDictionary);
            return MassDictionary[IDletter];
        }

        public static string GetFormula(string IDletter)
        {
            Dictionary<string, string> FormulaDictionary = new Dictionary<string, string>();
            IMonosaccharideConstants newTable = new MonosaccharideConstantsTable();
            newTable.FormulaTable(FormulaDictionary);
            return FormulaDictionary[IDletter];
        }

        public static string GetName(string IDletter)
        {
            Dictionary<string, string> NameDictionary = new Dictionary<string, string>();
            IMonosaccharideConstants newTable = new MonosaccharideConstantsTable();
            newTable.NameTable(NameDictionary);
            return NameDictionary[IDletter];
        }

        public static string GetName6(string IDletter)
        {
            Dictionary<string, string> SixLetterNameDictionary = new Dictionary<string, string>();
            IMonosaccharideConstants newTable = new MonosaccharideConstantsTable();
            newTable.SixLetterTable(SixLetterNameDictionary);
            return SixLetterNameDictionary[IDletter];
        }

        #region interface functions for MassTable, FormulaTable, and SixLetterTable

        void IMonosaccharideConstants.MassTable(Dictionary<string, double> DataDictionary)
        {
            Deoxyhexose DxyHex = new Deoxyhexose();
            Hexose Hex = new Hexose();
            HexuronicAcid HexA = new HexuronicAcid();
            KDNDeaminatedNeuraminicAcid KDN = new KDNDeaminatedNeuraminicAcid();
            NAcetylhexosamine HexNAc = new NAcetylhexosamine();
            NeuraminicAcid NeuAc = new NeuraminicAcid();
            NGlycolylNeuraminicAcid NeuGc = new NGlycolylNeuraminicAcid();
            Pentose Pent = new Pentose();

            DataDictionary.Add("DxyHex", DxyHex.MonoIsotopicMass);
            DataDictionary.Add("Hex", Hex.MonoIsotopicMass);
            DataDictionary.Add("HexA", HexA.MonoIsotopicMass);
            DataDictionary.Add("KDN", KDN.MonoIsotopicMass);
            DataDictionary.Add("HexNAc", HexNAc.MonoIsotopicMass);
            DataDictionary.Add("NeuAc", NeuAc.MonoIsotopicMass);
            DataDictionary.Add("NeuGc", NeuGc.MonoIsotopicMass);
            DataDictionary.Add("Pent", Pent.MonoIsotopicMass);
            
        }

        void IMonosaccharideConstants.FormulaTable(Dictionary<string, string> DataDictionary)
        {
            Deoxyhexose DxyHex = new Deoxyhexose();
            Hexose Hex = new Hexose();
            HexuronicAcid HexA = new HexuronicAcid();
            KDNDeaminatedNeuraminicAcid KDN = new KDNDeaminatedNeuraminicAcid();
            NAcetylhexosamine HexNAc = new NAcetylhexosamine();
            NeuraminicAcid NeuAc = new NeuraminicAcid();
            NGlycolylNeuraminicAcid NeuGc = new NGlycolylNeuraminicAcid();
            Pentose Pent = new Pentose();

            DataDictionary.Add("DxyHex", DxyHex.ChemicalFormula);
            DataDictionary.Add("Hex", Hex.ChemicalFormula);
            DataDictionary.Add("HexA", HexA.ChemicalFormula);
            DataDictionary.Add("KDN", KDN.ChemicalFormula);
            DataDictionary.Add("HexNAc", HexNAc.ChemicalFormula);
            DataDictionary.Add("NeuAc", NeuAc.ChemicalFormula);
            DataDictionary.Add("NeuGc", NeuGc.ChemicalFormula);
            DataDictionary.Add("Pent", Pent.ChemicalFormula);
        }

        void IMonosaccharideConstants.NameTable(Dictionary<string, string> DataDictionary)
        {
            Deoxyhexose DxyHex = new Deoxyhexose();
            Hexose Hex = new Hexose();
            HexuronicAcid HexA = new HexuronicAcid();
            KDNDeaminatedNeuraminicAcid KDN = new KDNDeaminatedNeuraminicAcid();
            NAcetylhexosamine HexNAc = new NAcetylhexosamine();
            NeuraminicAcid NeuAc = new NeuraminicAcid();
            NGlycolylNeuraminicAcid NeuGc = new NGlycolylNeuraminicAcid();
            Pentose Pent = new Pentose();

            DataDictionary.Add("DxyHex", DxyHex.Name);
            DataDictionary.Add("Hex", Hex.Name);
            DataDictionary.Add("HexA", HexA.Name);
            DataDictionary.Add("KDN", KDN.Name);
            DataDictionary.Add("HexNAc", HexNAc.Name);
            DataDictionary.Add("NeuAc", NeuAc.Name);
            DataDictionary.Add("NeuGc", NeuGc.Name);
            DataDictionary.Add("Pent", Pent.Name);
        }

        void IMonosaccharideConstants.SixLetterTable(Dictionary<string, string> DataDictionary)
        {
            Deoxyhexose DxyHex = new Deoxyhexose();
            Hexose Hex = new Hexose();
            HexuronicAcid HexA = new HexuronicAcid();
            KDNDeaminatedNeuraminicAcid KDN = new KDNDeaminatedNeuraminicAcid();
            NAcetylhexosamine HexNAc = new NAcetylhexosamine();
            NeuraminicAcid NeuAc = new NeuraminicAcid();
            NGlycolylNeuraminicAcid NeuGc = new NGlycolylNeuraminicAcid();
            Pentose Pent = new Pentose();

            DataDictionary.Add("DxyHex", DxyHex.SixLetterCode);
            DataDictionary.Add("Hex", Hex.SixLetterCode);
            DataDictionary.Add("HexA", HexA.SixLetterCode);
            DataDictionary.Add("KDN", KDN.SixLetterCode);
            DataDictionary.Add("HexNAc", HexNAc.SixLetterCode);
            DataDictionary.Add("NeuAc", NeuAc.SixLetterCode);
            DataDictionary.Add("NeuGc", NeuGc.SixLetterCode);
            DataDictionary.Add("Pent", Pent.SixLetterCode);
        }
        #endregion
    }
}
