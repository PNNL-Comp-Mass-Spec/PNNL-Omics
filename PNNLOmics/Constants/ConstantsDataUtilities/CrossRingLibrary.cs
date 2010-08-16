using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Constants.ConstantsObjectsDataLayer;

//dictionarty implementation
//Dictionary<string, CrossRingObject> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();
//double CRMass = CrossRingDictionary["crfNeu5Ac_03_X1"].MonoIsotopicMass;
//string crformula = CrossRingDictionary["crfNeu5Ac_03_X1"].ChemicalFormula;
//string CRName = CrossRingDictionary["crfNeu5Ac_03_X1"].Name;

namespace PNNLOmics.Constants.ConstantsDataUtilities
{
    public class CrossRingLibrary
    {
        public static Dictionary<string, CrossRingObject> LoadCrossRingData()
        {
            Dictionary<string, CrossRingObject> crossRingDictionary = new Dictionary<string, CrossRingObject>();

            //Deoxyhexose.NewElements(C H N O S P)

            //A ions retain the charge on the non-reducing side producing a fragment because hte core is lost
            //X ions keep the core and keep the aldehyde motif

            #region Hexose Cross Ring Fragments
            CrossRingObject crfHex_02_A2 = new CrossRingObject();//102.0316906 Fragment
            crfHex_02_A2.NewElements(4, 6, 0, 3, 0, 0);//Hex-C2H4O2
            crfHex_02_A2.Name = "CRFHex_02_A2";
            crfHex_02_A2.SixLetterCode = "H-02A2";
            crfHex_02_A2.ChemicalFormula = "C4H6O3";
            crfHex_02_A2.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfHex_02_A2);

            CrossRingObject crfHex_02_X1 = new CrossRingObject();//42.0105612 Aldehyde
            crfHex_02_X1.NewElements(2, 2, 0, 1, 0, 0);//Hex-C4H8O4
            crfHex_02_X1.Name = "CRFHex_02_X1";
            crfHex_02_X1.SixLetterCode = "H-02X1";
            crfHex_02_X1.ChemicalFormula = "C2H2O";
            crfHex_02_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfHex_02_X1);

            CrossRingObject crfHex_03_A2 = new CrossRingObject();//72.0211259 Aldehyde
            crfHex_03_A2.NewElements(3, 4, 0, 2, 0, 0);//Hex-C3H6O3
            crfHex_03_A2.Name = "CRFHex_03_A2";
            crfHex_03_A2.SixLetterCode = "H-03A2";
            crfHex_03_A2.ChemicalFormula = "C3H4O2";
            crfHex_03_A2.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfHex_03_A2);

            CrossRingObject crfHex_03_X1 = new CrossRingObject();//72.0211259 Aldehyde
            crfHex_03_X1.NewElements(3, 4, 0, 2, 0, 0);//Hex-C3H6O3
            crfHex_03_X1.Name = "CRFHex_03_X1";
            crfHex_03_X1.SixLetterCode = "H-03X1";
            crfHex_03_X1.ChemicalFormula = "C3H4O2";
            crfHex_03_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfHex_03_X1);

            CrossRingObject crfHex_24_A2 = new CrossRingObject();//42.0105612 Fragment
            crfHex_24_A2.NewElements(2, 2, 0, 1, 0, 0);//Hex-C4H8O4
            crfHex_24_A2.Name = "CRFHex_24_A2";
            crfHex_24_A2.SixLetterCode = "H-24A2";
            crfHex_24_A2.ChemicalFormula = "C2H2O";
            crfHex_24_A2.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfHex_24_A2);

            CrossRingObject crfHex_24_X1 = new CrossRingObject();//102.0316906 Aldehyde
            crfHex_24_X1.NewElements(4, 6, 0, 3, 0, 0);//Hex-C2H4O2
            crfHex_24_X1.Name = "CRFHex_24_X1";
            crfHex_24_X1.SixLetterCode = "H-24X1";
            crfHex_24_X1.ChemicalFormula = "C4H6O3";
            crfHex_24_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfHex_24_X1);
            #endregion

            #region HexNAc Cross Ring Fragments
            CrossRingObject crfHexNAc_02_A2 = new CrossRingObject();//102.031721493 Fragment
            crfHexNAc_02_A2.NewElements(4, 6, 0, 3, 0, 0);//HexNAc-C4H7NO2
            crfHexNAc_02_A2.Name = "CRFHexNAc_02_A2";
            crfHexNAc_02_A2.SixLetterCode = "N-02A2";
            crfHexNAc_02_A2.ChemicalFormula = "C4H6O3";
            crfHexNAc_02_A2.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfHexNAc_02_A2);

            CrossRingObject crfHexNAc_02_X1 = new CrossRingObject();//83.013331758 Aldehyde
            crfHexNAc_02_X1.NewElements(4, 5, 1, 1, 0, 0);//HexNAc-C4H8O4
            crfHexNAc_02_X1.Name = "CRFHexNAc_02_X1";
            crfHexNAc_02_X1.SixLetterCode = "N-02X1";
            crfHexNAc_02_X1.ChemicalFormula = "C4H5NO";
            crfHexNAc_02_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfHexNAc_02_X1);
            
            CrossRingObject crfHexNAc_03_A2 = new CrossRingObject();// Fragment
            crfHexNAc_03_A2.NewElements(3, 4, 0, 2, 0, 0);//HexNAc-C5H8NO3
            crfHexNAc_03_A2.Name = "CRFHexNAc_03_A2";
            crfHexNAc_03_A2.SixLetterCode = "N-03A2";
            crfHexNAc_03_A2.ChemicalFormula = "C3H4O2";
            crfHexNAc_03_A2.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfHexNAc_03_A2);

            CrossRingObject crfHexNAc_03_X1 = new CrossRingObject();// Aldehyde
            crfHexNAc_03_X1.NewElements(5, 6, 1, 2, 0, 0);//HexNAc-C3H6O3
            crfHexNAc_03_X1.Name = "CRFHexNAc_03_X1";
            crfHexNAc_03_X1.SixLetterCode = "N-03X1";
            crfHexNAc_03_X1.ChemicalFormula = "C5H6NO2";
            crfHexNAc_03_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfHexNAc_03_X1);
           
            CrossRingObject crfHexNAc_24_A2 = new CrossRingObject();//42.01059293 Fragment
            crfHexNAc_24_A2.NewElements(2, 2, 0, 1, 0, 0);//HexNAc-C6H11NO4
            crfHexNAc_24_A2.Name = "CRFHexNAc_24_A2";
            crfHexNAc_24_A2.SixLetterCode = "N-24A2";
            crfHexNAc_24_A2.ChemicalFormula = "C2H2O";
            crfHexNAc_24_A2.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfHexNAc_24_A2);

            CrossRingObject crfHexNAc_24_X1 = new CrossRingObject();//143.0582706 Aldehyde
            crfHexNAc_24_X1.NewElements(6, 9, 1, 3, 0, 0);//HexNAc-C2H4O2
            crfHexNAc_24_X1.Name = "CRFHexNAc_24_X1";
            crfHexNAc_24_X1.SixLetterCode = "N-24X1";
            crfHexNAc_24_X1.ChemicalFormula = "C6H9NO3";
            crfHexNAc_24_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfHexNAc_24_X1);  
            #endregion

            #region Sialic Acid Cross Ring Framgnets
            CrossRingObject crfNeu5Ac_02_X1 = new CrossRingObject();//70.0054793084 Aldehyde
            crfNeu5Ac_02_X1.NewElements(3, 2, 0, 2, 0, 0);//Neu5Ac-C8H15NO6
            crfNeu5Ac_02_X1.Name = "CRFNeu5Ac_02_X1";
            crfNeu5Ac_02_X1.SixLetterCode = "A-02X1";
            crfNeu5Ac_02_X1.ChemicalFormula = "C3H2O2";
            crfNeu5Ac_02_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfNeu5Ac_02_X1);

            CrossRingObject crfNeu5Ac_03_X1 = new CrossRingObject();// 100.01604399481499 Aldehyde
            crfNeu5Ac_03_X1.NewElements(4, 4, 0, 3, 0, 0);//Neu5Ac-C7H13NO5
            crfNeu5Ac_03_X1.Name = "CRFNeu5Ac_03_X1";
            crfNeu5Ac_03_X1.SixLetterCode = "A-03X1";
            crfNeu5Ac_03_X1.ChemicalFormula = "C4H4O3";
            crfNeu5Ac_03_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfNeu5Ac_03_X1);

            CrossRingObject crfNeu5Ac_24_X1 = new CrossRingObject();//190.0477380536 Aldehyde
            crfNeu5Ac_24_X1.NewElements(7, 10, 0, 6, 0, 0);//HexNAc-C4H7NO2
            crfNeu5Ac_24_X1.Name = "CRFNeu5Ac_24_X1";
            crfNeu5Ac_24_X1.SixLetterCode = "A-24X1";
            crfNeu5Ac_24_X1.ChemicalFormula = "C7H10O6";
            crfNeu5Ac_24_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfNeu5Ac_24_X1);

            CrossRingObject crfNeu5Ac_25_X1 = new CrossRingObject();//143.0582706 Aldehyde
            crfNeu5Ac_25_X1.NewElements(3, 2, 0, 3, 0, 0);//HexNAc-C8H15NO5
            crfNeu5Ac_25_X1.Name = "CRFNeu5Ac_25_X1";
            crfNeu5Ac_25_X1.SixLetterCode = "A-25X1";
            crfNeu5Ac_25_X1.ChemicalFormula = "C3H2O3";
            crfNeu5Ac_25_X1.MonoIsotopicMass = CrossRingObject.GetMonoisotopicMass(crfNeu5Ac_25_X1);  
            #endregion

            crossRingDictionary.Add(crfHex_02_A2.Name, crfHex_02_A2);
            crossRingDictionary.Add(crfHex_02_X1.Name, crfHex_02_X1);
            crossRingDictionary.Add(crfHex_03_A2.Name, crfHex_03_A2);
            crossRingDictionary.Add(crfHex_03_X1.Name, crfHex_03_X1);
            crossRingDictionary.Add(crfHex_24_A2.Name, crfHex_24_A2);
            crossRingDictionary.Add(crfHex_24_X1.Name, crfHex_24_X1);

            crossRingDictionary.Add(crfHexNAc_02_A2.Name, crfHexNAc_02_A2);
            crossRingDictionary.Add(crfHexNAc_02_X1.Name, crfHexNAc_02_X1);
            crossRingDictionary.Add(crfHexNAc_03_A2.Name, crfHexNAc_03_A2);
            crossRingDictionary.Add(crfHexNAc_03_X1.Name, crfHexNAc_03_X1);
            crossRingDictionary.Add(crfHexNAc_24_A2.Name, crfHexNAc_24_A2);
            crossRingDictionary.Add(crfHexNAc_24_X1.Name, crfHexNAc_24_X1);

            crossRingDictionary.Add(crfNeu5Ac_02_X1.Name, crfNeu5Ac_02_X1);
            crossRingDictionary.Add(crfNeu5Ac_03_X1.Name, crfNeu5Ac_03_X1);
            crossRingDictionary.Add(crfNeu5Ac_24_X1.Name, crfNeu5Ac_24_X1);
            crossRingDictionary.Add(crfNeu5Ac_25_X1.Name, crfNeu5Ac_25_X1);

            return crossRingDictionary;
        }

    }
}
