using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//this is not finshed yey
namespace Constants
{
    public class OtherMoleculeMass
    {
        public static double Aldehyde()
        {
            return OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("Aldehyde");
        }
        public static double Alditol()
        {
            return OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("Alditol");
        }
        public static double Ammonia()
        {
            return OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("Ammonia");
        }
        public static double Ammonium()
        {
            return OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("Ammonium");
        }
        public static double KMinusH()
        {
            return OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("KMinusH");
        }
        public static double NaMinusH()
        {
            return OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("NaMinusH");
        }
        public static double Sulfate()
        {
            return OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("Sulfate");
        }
        public static double Water()
        {
            return OtherMoleculeConstantsStaticLibrary.GetMonoisotopicMass("Water");
        }
    }
}
