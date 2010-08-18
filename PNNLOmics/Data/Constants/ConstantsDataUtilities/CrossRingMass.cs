using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//There are more cross ring fragments possible than this list
namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    /// <summary>
    /// This is a Class designed to convert one line lookup calls to a monoisotopic mass object
    /// with Intelli Sense so you can select the one you want.  This way you don'y need to remember the compelex name
    /// to get the monoisotopic mass.
    /// </summary>
    public class CrossRingMass
    {
        public static double CRFHex_02_A2()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFHex_02_A2");
        }
        public static double CRFHex_02_X1()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFHex_02_X1");
        }
        public static double CRFHex_03_A2()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFHex_03_A2");
        }
        public static double CRFHex_03_X1()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFHex_03_X1");
        }
        public static double CRFHex_24_A2()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFHex_24_A2");
        }
        public static double CRFHex_24_X1()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFHex_24_X1");
        }


        public static double CRFHexNAc_02_A2()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFHexNAc_02_A2");
        }
        public static double CRFHexNAc_02_X1()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFHexNAc_02_X1");
        }
        public static double CRFHexNAc_03_A2()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFHexNAc_03_A2");
        }
        public static double CRFHexNAc_03_X1()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFHexNAc_03_X1");
        }
        public static double CRFHexNAc_24_A2()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFHexNAc_24_A2");
        }
        public static double CRFHexNAc_24_X1()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFHexNAc_24_X1");
        }


        public static double CRFNeu5Ac_02_X1()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFNeu5Ac_02_X1");
        }
        public static double CRFNeu5Ac_03_X1()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFNeu5Ac_03_X1");
        }
        public static double CRFNeu5Ac_24_X1()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFNeu5Ac_24_X1");
        }
        public static double CRFNeu5Ac_25_X1()
        {
            return CrossRingStaticLibrary.GetMonoisotopicMass("CRFNeu5Ac_25_X1");
        }
    
    }
}
