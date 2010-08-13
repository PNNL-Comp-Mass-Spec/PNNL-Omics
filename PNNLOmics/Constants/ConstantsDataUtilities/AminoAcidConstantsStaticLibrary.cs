using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



//one line implementation
//double AAmass2 = AminoAcidConstantsStaticLibrary.GetMonoisotopicMass('A');
//string AAName2 = AminoAcidConstantsStaticLibrary.GetName('A');
//string AAFormula2 = AminoAcidConstantsStaticLibrary.GetFormula('A');

//how to calculate the mass of a peptide
//double massPeptide=0;
//string peptideSequence = "NRTL";
//for (int y = 0; y < peptideSequence.Length; y++)
//{
//    massPeptide += AminoAcidConstantsStaticLibrary.GetMonoisotopicMass(peptideSequence[y]);
//}//massPeptide = 484.27578094385393

namespace Constants
{
    public class AminoAcidConstantsStaticLibrary
    {
        public static double GetMonoisotopicMass(char constantKey)
        {
            Dictionary<char, AminoAcidObject> AminoAcidsDictionary = AminoAcidLibrary.LoadAminoAcidData();
            return AminoAcidsDictionary[constantKey].MonoIsotopicMass;      
        }

        public static string GetFormula(char constantKey)
        {
            Dictionary<char, AminoAcidObject> AminoAcidsDictionary = AminoAcidLibrary.LoadAminoAcidData();
            return AminoAcidsDictionary[constantKey].ChemicalFormula;             
        }

        public static string GetName(char constantKey)
        {
            Dictionary<char, AminoAcidObject> AminoAcidsDictionary = AminoAcidLibrary.LoadAminoAcidData();
            return AminoAcidsDictionary[constantKey].Name;
        }
    }
}
