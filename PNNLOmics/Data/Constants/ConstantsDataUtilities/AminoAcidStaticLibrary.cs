using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

//one line implementation
//double AAmass2 = AminoAcidConstantsStaticLibrary.GetMonoisotopicMass('A');
//string AAName2 = AminoAcidConstantsStaticLibrary.GetName('A');
//string AAFormula2 = AminoAcidConstantsStaticLibrary.GetFormula('A');

//double mass3 = AminoAcidStaticLibrary.GetMonoisotopicMass(SelectAminoAcid.GlutamicAcid);

//how to calculate the mass of a peptide
//double massPeptide=0;
//string peptideSequence = "NRTL";
//for (int y = 0; y < peptideSequence.Length; y++)
//{
//    massPeptide += AminoAcidConstantsStaticLibrary.GetMonoisotopicMass(peptideSequence[y]);
//}//massPeptide = 484.27578094385393

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    /// <summary>
    /// This is a Class designed to convert dictionary calls for amino acids in one line static method calls.
    /// </summary>
    public class AminoAcidStaticLibrary
    {
        public static double GetMonoisotopicMass(char constantKey)
        {
            AminoAcidSingleton NewSingleton = AminoAcidSingleton.Instance;
            Dictionary<char, AminoAcid> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].MonoIsotopicMass;
        }

        public static string GetFormula(char constantKey)
        {
            AminoAcidSingleton NewSingleton = AminoAcidSingleton.Instance;
            Dictionary<char, AminoAcid> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].ChemicalFormula;
        }

        public static string GetName(char constantKey)
        {
            AminoAcidSingleton NewSingleton = AminoAcidSingleton.Instance;
            Dictionary<char, AminoAcid> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].Name;
        }

        //overload to allow for SelectElement
        public static double GetMonoisotopicMass(SelectAminoAcid selectKey)
        {
            AminoAcidSingleton NewSingleton = AminoAcidSingleton.Instance;
            Dictionary<char, AminoAcid> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, char> enumConverter = NewSingleton.ConstantsEnumDictionary;
            char constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].MonoIsotopicMass;
        }

        public static string GetFormula(SelectAminoAcid selectKey)
        {
            AminoAcidSingleton NewSingleton = AminoAcidSingleton.Instance;
            Dictionary<char, AminoAcid> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, char> enumConverter = NewSingleton.ConstantsEnumDictionary;
            char constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].ChemicalFormula;
        }

        public static string GetName(SelectAminoAcid selectKey)
        {
            AminoAcidSingleton NewSingleton = AminoAcidSingleton.Instance;
            Dictionary<char, AminoAcid> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, char> enumConverter = NewSingleton.ConstantsEnumDictionary;
            char constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].Name;
        }
    }
}
