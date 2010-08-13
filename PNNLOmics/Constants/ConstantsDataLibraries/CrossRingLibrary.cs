using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//dictionarty implementation
//Dictionary<string, CrossRingObject> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();
//double CRMass = CrossRingDictionary["CRFNeu5Ac_03_X1"].MonoIsotopicMass;
//string CRFormula = CrossRingDictionary["CRFNeu5Ac_03_X1"].ChemicalFormula;
//string CRName = CrossRingDictionary["CRFNeu5Ac_03_X1"].Name;

namespace Constants
{
    public class CrossRingLibrary
    {
        public static Dictionary<string, CrossRingObject> LoadCrossRingData()
        {
            Dictionary<string, CrossRingObject> CrossRingDictionary = new Dictionary<string, CrossRingObject>();

            //Deoxyhexose.NewElements(C H N O S P)

            //A ions retain the charge on the non-reducing side producing a fragment because hte core is lost
            //X ions keep the core and keep the aldehyde motif

            #region Hexose Cross Ring Fragments
            CrossRingObject CRFHex_02_A2 = new CrossRingObject();//102.0316906 Fragment
            CRFHex_02_A2.NewElements(4, 6, 0, 3, 0, 0);//Hex-C2H4O2
            CRFHex_02_A2.Name = "CRFHex_02_A2";
            CRFHex_02_A2.SixLetterCode = "H-02A2";
            CRFHex_02_A2.ChemicalFormula = "C4H6O3";
            CRFHex_02_A2.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFHex_02_A2);

            CrossRingObject CRFHex_02_X1 = new CrossRingObject();//42.0105612 Aldehyde
            CRFHex_02_X1.NewElements(2, 2, 0, 1, 0, 0);//Hex-C4H8O4
            CRFHex_02_X1.Name = "CRFHex_02_X1";
            CRFHex_02_X1.SixLetterCode = "H-02X1";
            CRFHex_02_X1.ChemicalFormula = "C2H2O";
            CRFHex_02_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFHex_02_X1);

            CrossRingObject CRFHex_03_A2 = new CrossRingObject();//72.0211259 Aldehyde
            CRFHex_03_A2.NewElements(3, 4, 0, 2, 0, 0);//Hex-C3H6O3
            CRFHex_03_A2.Name = "CRFHex_03_A2";
            CRFHex_03_A2.SixLetterCode = "H-03A2";
            CRFHex_03_A2.ChemicalFormula = "C3H4O2";
            CRFHex_03_A2.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFHex_03_A2);

            CrossRingObject CRFHex_03_X1 = new CrossRingObject();//72.0211259 Aldehyde
            CRFHex_03_X1.NewElements(3, 4, 0, 2, 0, 0);//Hex-C3H6O3
            CRFHex_03_X1.Name = "CRFHex_03_X1";
            CRFHex_03_X1.SixLetterCode = "H-03X1";
            CRFHex_03_X1.ChemicalFormula = "C3H4O2";
            CRFHex_03_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFHex_03_X1);

            CrossRingObject CRFHex_24_A2 = new CrossRingObject();//42.0105612 Fragment
            CRFHex_24_A2.NewElements(2, 2, 0, 1, 0, 0);//Hex-C4H8O4
            CRFHex_24_A2.Name = "CRFHex_24_A2";
            CRFHex_24_A2.SixLetterCode = "H-24A2";
            CRFHex_24_A2.ChemicalFormula = "C2H2O";
            CRFHex_24_A2.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFHex_24_A2);

            CrossRingObject CRFHex_24_X1 = new CrossRingObject();//102.0316906 Aldehyde
            CRFHex_24_X1.NewElements(4, 6, 0, 3, 0, 0);//Hex-C2H4O2
            CRFHex_24_X1.Name = "CRFHex_24_X1";
            CRFHex_24_X1.SixLetterCode = "H-24X1";
            CRFHex_24_X1.ChemicalFormula = "C4H6O3";
            CRFHex_24_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFHex_24_X1);
            #endregion

            #region HexNAc Cross Ring Fragments
            CrossRingObject CRFHexNAc_02_A2 = new CrossRingObject();//102.031721493 Fragment
            CRFHexNAc_02_A2.NewElements(4, 6, 0, 3, 0, 0);//HexNAc-C4H7NO2
            CRFHexNAc_02_A2.Name = "CRFHexNAc_02_A2";
            CRFHexNAc_02_A2.SixLetterCode = "N-02A2";
            CRFHexNAc_02_A2.ChemicalFormula = "C4H6O3";
            CRFHexNAc_02_A2.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFHexNAc_02_A2);

            CrossRingObject CRFHexNAc_02_X1 = new CrossRingObject();//83.013331758 Aldehyde
            CRFHexNAc_02_X1.NewElements(4, 5, 1, 1, 0, 0);//HexNAc-C4H8O4
            CRFHexNAc_02_X1.Name = "CRFHexNAc_02_X1";
            CRFHexNAc_02_X1.SixLetterCode = "N-02X1";
            CRFHexNAc_02_X1.ChemicalFormula = "C4H5NO";
            CRFHexNAc_02_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFHexNAc_02_X1);
            
            CrossRingObject CRFHexNAc_03_A2 = new CrossRingObject();// Fragment
            CRFHexNAc_03_A2.NewElements(3, 4, 0, 2, 0, 0);//HexNAc-C5H8NO3
            CRFHexNAc_03_A2.Name = "CRFHexNAc_03_A2";
            CRFHexNAc_03_A2.SixLetterCode = "N-03A2";
            CRFHexNAc_03_A2.ChemicalFormula = "C3H4O2";
            CRFHexNAc_03_A2.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFHexNAc_03_A2);

            CrossRingObject CRFHexNAc_03_X1 = new CrossRingObject();// Aldehyde
            CRFHexNAc_03_X1.NewElements(5, 6, 1, 2, 0, 0);//HexNAc-C3H6O3
            CRFHexNAc_03_X1.Name = "CRFHexNAc_03_X1";
            CRFHexNAc_03_X1.SixLetterCode = "N-03X1";
            CRFHexNAc_03_X1.ChemicalFormula = "C5H6NO2";
            CRFHexNAc_03_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFHexNAc_03_X1);
           
            CrossRingObject CRFHexNAc_24_A2 = new CrossRingObject();//42.01059293 Fragment
            CRFHexNAc_24_A2.NewElements(2, 2, 0, 1, 0, 0);//HexNAc-C6H11NO4
            CRFHexNAc_24_A2.Name = "CRFHexNAc_24_A2";
            CRFHexNAc_24_A2.SixLetterCode = "N-24A2";
            CRFHexNAc_24_A2.ChemicalFormula = "C2H2O";
            CRFHexNAc_24_A2.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFHexNAc_24_A2);

            CrossRingObject CRFHexNAc_24_X1 = new CrossRingObject();//143.0582706 Aldehyde
            CRFHexNAc_24_X1.NewElements(6, 9, 1, 3, 0, 0);//HexNAc-C2H4O2
            CRFHexNAc_24_X1.Name = "CRFHexNAc_24_X1";
            CRFHexNAc_24_X1.SixLetterCode = "N-24X1";
            CRFHexNAc_24_X1.ChemicalFormula = "C6H9NO3";
            CRFHexNAc_24_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFHexNAc_24_X1);  
            #endregion

            #region Sialic Acid Cross Ring Framgnets
            CrossRingObject CRFNeu5Ac_02_X1 = new CrossRingObject();//70.0054793084 Aldehyde
            CRFNeu5Ac_02_X1.NewElements(3, 2, 0, 2, 0, 0);//Neu5Ac-C8H15NO6
            CRFNeu5Ac_02_X1.Name = "CRFNeu5Ac_02_X1";
            CRFNeu5Ac_02_X1.SixLetterCode = "A-02X1";
            CRFNeu5Ac_02_X1.ChemicalFormula = "C3H2O2";
            CRFNeu5Ac_02_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFNeu5Ac_02_X1);

            CrossRingObject CRFNeu5Ac_03_X1 = new CrossRingObject();// 100.01604399481499 Aldehyde
            CRFNeu5Ac_03_X1.NewElements(4, 4, 0, 3, 0, 0);//Neu5Ac-C7H13NO5
            CRFNeu5Ac_03_X1.Name = "CRFNeu5Ac_03_X1";
            CRFNeu5Ac_03_X1.SixLetterCode = "A-03X1";
            CRFNeu5Ac_03_X1.ChemicalFormula = "C4H4O3";
            CRFNeu5Ac_03_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFNeu5Ac_03_X1);

            CrossRingObject CRFNeu5Ac_24_X1 = new CrossRingObject();//190.0477380536 Aldehyde
            CRFNeu5Ac_24_X1.NewElements(7, 10, 0, 6, 0, 0);//HexNAc-C4H7NO2
            CRFNeu5Ac_24_X1.Name = "CRFNeu5Ac_24_X1";
            CRFNeu5Ac_24_X1.SixLetterCode = "A-24X1";
            CRFNeu5Ac_24_X1.ChemicalFormula = "C7H10O6";
            CRFNeu5Ac_24_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFNeu5Ac_24_X1);

            CrossRingObject CRFNeu5Ac_25_X1 = new CrossRingObject();//143.0582706 Aldehyde
            CRFNeu5Ac_25_X1.NewElements(3, 2, 0, 3, 0, 0);//HexNAc-C8H15NO5
            CRFNeu5Ac_25_X1.Name = "CRFNeu5Ac_25_X1";
            CRFNeu5Ac_25_X1.SixLetterCode = "A-25X1";
            CRFNeu5Ac_25_X1.ChemicalFormula = "C3H2O3";
            CRFNeu5Ac_25_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(CRFNeu5Ac_25_X1);  
            #endregion

            CrossRingDictionary.Add(CRFHex_02_A2.Name, CRFHex_02_A2);
            CrossRingDictionary.Add(CRFHex_02_X1.Name, CRFHex_02_X1);
            CrossRingDictionary.Add(CRFHex_03_A2.Name, CRFHex_03_A2);
            CrossRingDictionary.Add(CRFHex_03_X1.Name, CRFHex_03_X1);
            CrossRingDictionary.Add(CRFHex_24_A2.Name, CRFHex_24_A2);
            CrossRingDictionary.Add(CRFHex_24_X1.Name, CRFHex_24_X1);

            CrossRingDictionary.Add(CRFHexNAc_02_A2.Name, CRFHexNAc_02_A2);
            CrossRingDictionary.Add(CRFHexNAc_02_X1.Name, CRFHexNAc_02_X1);
            CrossRingDictionary.Add(CRFHexNAc_03_A2.Name, CRFHexNAc_03_A2);
            CrossRingDictionary.Add(CRFHexNAc_03_X1.Name, CRFHexNAc_03_X1);
            CrossRingDictionary.Add(CRFHexNAc_24_A2.Name, CRFHexNAc_24_A2);
            CrossRingDictionary.Add(CRFHexNAc_24_X1.Name, CRFHexNAc_24_X1);

            CrossRingDictionary.Add(CRFNeu5Ac_02_X1.Name, CRFNeu5Ac_02_X1);
            CrossRingDictionary.Add(CRFNeu5Ac_03_X1.Name, CRFNeu5Ac_03_X1);
            CrossRingDictionary.Add(CRFNeu5Ac_24_X1.Name, CRFNeu5Ac_24_X1);
            CrossRingDictionary.Add(CRFNeu5Ac_25_X1.Name, CRFNeu5Ac_25_X1);

            return CrossRingDictionary;
        }

    }
}
