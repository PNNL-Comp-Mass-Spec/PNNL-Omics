using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//this is not finshed yey
namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    public class OtherMoleculeMass
    {
        public static double Aldehyde()
        {
            return OtherMoleculeStaticLibrary.GetMonoisotopicMass("Aldehyde");
        }
        public static double Alditol()
        {
            return OtherMoleculeStaticLibrary.GetMonoisotopicMass("Alditol");
        }
        public static double Ammonia()
        {
            return OtherMoleculeStaticLibrary.GetMonoisotopicMass("Ammonia");
        }
        public static double Ammonium()
        {
            return OtherMoleculeStaticLibrary.GetMonoisotopicMass("Ammonium");
        }
        public static double KMinusH()
        {
            return OtherMoleculeStaticLibrary.GetMonoisotopicMass("KMinusH");
        }
        public static double NaMinusH()
        {
            return OtherMoleculeStaticLibrary.GetMonoisotopicMass("NaMinusH");
        }
        public static double Sulfate()
        {
            return OtherMoleculeStaticLibrary.GetMonoisotopicMass("Sulfate");
        }
        public static double Water()
        {
            return OtherMoleculeStaticLibrary.GetMonoisotopicMass("Water");
        }
    }
}
