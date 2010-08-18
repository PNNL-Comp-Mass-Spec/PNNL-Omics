using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    /// <summary>
    /// This is a Class designed to convert one line lookup calls to a monoisotopic mass object
    /// with Intelli Sense so you can select the one you want.  This way you don'y need to remember the compelex name
    /// to get the monoisotopic mass.
    /// </summary>
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
