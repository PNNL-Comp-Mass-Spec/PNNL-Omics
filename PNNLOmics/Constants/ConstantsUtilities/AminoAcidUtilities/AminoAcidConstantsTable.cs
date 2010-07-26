using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//main call
 //string chemicalFormula = AminoAcidConstantsTable.GetFormula('A');
 //double mass = AminoAcidConstantsTable.GetMass('A');

namespace Constants
{
    interface IAminoAcidConstants
       {
           void MassTable(Dictionary<char, double> DataDictionary);
           void FormulaTable(Dictionary<char, string> DataDictionary);
       }
    public class AminoAcidConstantsTable: IAminoAcidConstants 
    {

        //public Dictionary<string, AminoAcid> Data { get; set; }
        //public Dictionary<string, double> Data { get; set; }

        public static double GetMass(char IDletter)
        {
            Dictionary<char, double> MassDictionary =new Dictionary<char,double>();
            IAminoAcidConstants newTable = new AminoAcidConstantsTable();
            newTable.MassTable(MassDictionary);
            return MassDictionary[IDletter];
        }

        public static string GetFormula(char IDletter)
        {
            Dictionary<char, string> FormulaDictionary = new Dictionary<char, string>();
            IAminoAcidConstants newTable = new AminoAcidConstantsTable();
            newTable.FormulaTable(FormulaDictionary);
            return FormulaDictionary[IDletter];
        }

        #region interface functions for MassTable and FormulaTable

        void IAminoAcidConstants.MassTable(Dictionary<char, double> DataDictionary)
        {
            // this.Data = new Dictionary<string, AminoAcid>();
            //this.Data = new Dictionary<string, double>();
            //this.Data.Add("A", new Alanine());
            //this.Data.Add("R", new Arginine());
            Alanine A = new Alanine();
            Arginine R = new Arginine();
            Asparagine N = new Asparagine();
            AsparticAcid D = new AsparticAcid();
            Cysteine C = new Cysteine();
            GlutamicAcid E = new GlutamicAcid();
            Glutamine Q = new Glutamine();
            Glycine G = new Glycine();
            Histidine H = new Histidine();
            Isoleucine I = new Isoleucine();
            Leucine L = new Leucine();
            Lysine K = new Lysine();
            Methionine M = new Methionine();
            Phenylalanine F = new Phenylalanine();
            Proline P = new Proline();
            Serine S = new Serine();
            Threonine T = new Threonine();
            Tryptophan W = new Tryptophan();
            Tyrosine Y = new Tyrosine();
            Valine V = new Valine();

            DataDictionary.Add('A', A.MonoIsotopicMass);
            DataDictionary.Add('R', R.MonoIsotopicMass);
            DataDictionary.Add('N', N.MonoIsotopicMass);
            DataDictionary.Add('D', D.MonoIsotopicMass);
            DataDictionary.Add('C', C.MonoIsotopicMass);
            DataDictionary.Add('E', E.MonoIsotopicMass);
            DataDictionary.Add('Q', Q.MonoIsotopicMass);
            DataDictionary.Add('G', G.MonoIsotopicMass);
            DataDictionary.Add('H', H.MonoIsotopicMass);
            DataDictionary.Add('I', I.MonoIsotopicMass);
            DataDictionary.Add('L', L.MonoIsotopicMass);
            DataDictionary.Add('K', K.MonoIsotopicMass);
            DataDictionary.Add('M', M.MonoIsotopicMass);
            DataDictionary.Add('F', F.MonoIsotopicMass);
            DataDictionary.Add('P', P.MonoIsotopicMass);
            DataDictionary.Add('S', S.MonoIsotopicMass);
            DataDictionary.Add('T', T.MonoIsotopicMass);
            DataDictionary.Add('W', W.MonoIsotopicMass);
            DataDictionary.Add('Y', Y.MonoIsotopicMass);
            DataDictionary.Add('V', V.MonoIsotopicMass);
        }

        void IAminoAcidConstants.FormulaTable(Dictionary<char, string> DataDictionary)
        {
            // this.Data = new Dictionary<string, AminoAcid>();
            //this.Data = new Dictionary<string, double>();
            //this.Data.Add("A", new Alanine());
            //this.Data.Add("R", new Arginine());
            Alanine A = new Alanine();
            Arginine R = new Arginine();
            Asparagine N = new Asparagine();
            AsparticAcid D = new AsparticAcid();
            Cysteine C = new Cysteine();
            GlutamicAcid E = new GlutamicAcid();
            Glutamine Q = new Glutamine();
            Glycine G = new Glycine();
            Histidine H = new Histidine();
            Isoleucine I = new Isoleucine();
            Leucine L = new Leucine();
            Lysine K = new Lysine();
            Methionine M = new Methionine();
            Phenylalanine F = new Phenylalanine();
            Proline P = new Proline();
            Serine S = new Serine();
            Threonine T = new Threonine();
            Tryptophan W = new Tryptophan();
            Tyrosine Y = new Tyrosine();
            Valine V = new Valine();

            DataDictionary.Add('A', A.ChemicalFormula);
            DataDictionary.Add('R', R.ChemicalFormula);
            DataDictionary.Add('N', N.ChemicalFormula);
            DataDictionary.Add('D', D.ChemicalFormula);
            DataDictionary.Add('C', C.ChemicalFormula);
            DataDictionary.Add('E', E.ChemicalFormula);
            DataDictionary.Add('Q', Q.ChemicalFormula);
            DataDictionary.Add('G', G.ChemicalFormula);
            DataDictionary.Add('H', H.ChemicalFormula);
            DataDictionary.Add('I', I.ChemicalFormula);
            DataDictionary.Add('L', L.ChemicalFormula);
            DataDictionary.Add('K', K.ChemicalFormula);
            DataDictionary.Add('M', M.ChemicalFormula);
            DataDictionary.Add('F', F.ChemicalFormula);
            DataDictionary.Add('P', P.ChemicalFormula);
            DataDictionary.Add('S', S.ChemicalFormula);
            DataDictionary.Add('T', T.ChemicalFormula);
            DataDictionary.Add('W', W.ChemicalFormula);
            DataDictionary.Add('Y', Y.ChemicalFormula);
            DataDictionary.Add('V', V.ChemicalFormula);
        }
        #endregion
    }
}
