using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

//dictionarty implementation
//Dictionary<string, CrossRingObject> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();
//double CRMass = CrossRingDictionary["crfNeu5Ac_03_X1"].MonoIsotopicMass;
//string crformula = CrossRingDictionary["crfNeu5Ac_03_X1"].ChemicalFormula;
//string CRName = CrossRingDictionary["crfNeu5Ac_03_X1"].Name;

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    public class CrossRingLibrary
    {
        /// <summary>
        /// This is a Class designed to create cross ring fragments objects of monosaccharides from the elements.
        /// The cross ring fragments are added to a Dictionary searchable with string keys such as "CRFNeu5Ac_02_X1" for the 
        /// X1 cross ring fragment which breaks across the 0 and 2 ring bonds of a Neuraminic acid monosachcaride
        /// </summary>
        public static Dictionary<string, CrossRing> LoadCrossRingData()
        {
            Dictionary<string, CrossRing> crossRingDictionary = new Dictionary<string, CrossRing>();

            //Deoxyhexose.NewElements(C H N O S P)

            //A ions retain the charge on the non-reducing side producing a fragment because hte core is lost
            //X ions keep the core and keep the aldehyde motif

            #region Hexose Cross Ring Fragments
            CrossRing crfHex_02_A2 = new CrossRing();//102.0316906 Fragment
            crfHex_02_A2.NewElements(4, 6, 0, 3, 0, 0);//Hex-C2H4O2
            crfHex_02_A2.Name = "CRFHex_02_A2";
            crfHex_02_A2.SixLetterCode = "H-02A2";
            crfHex_02_A2.ChemicalFormula = "C4H6O3";
            crfHex_02_A2.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfHex_02_A2);

            CrossRing crfHex_02_X1 = new CrossRing();//42.0105612 Aldehyde
            crfHex_02_X1.NewElements(2, 2, 0, 1, 0, 0);//Hex-C4H8O4
            crfHex_02_X1.Name = "CRFHex_02_X1";
            crfHex_02_X1.SixLetterCode = "H-02X1";
            crfHex_02_X1.ChemicalFormula = "C2H2O";
            crfHex_02_X1.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfHex_02_X1);

            CrossRing crfHex_03_A2 = new CrossRing();//72.0211259 Aldehyde
            crfHex_03_A2.NewElements(3, 4, 0, 2, 0, 0);//Hex-C3H6O3
            crfHex_03_A2.Name = "CRFHex_03_A2";
            crfHex_03_A2.SixLetterCode = "H-03A2";
            crfHex_03_A2.ChemicalFormula = "C3H4O2";
            crfHex_03_A2.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfHex_03_A2);

            CrossRing crfHex_03_X1 = new CrossRing();//72.0211259 Aldehyde
            crfHex_03_X1.NewElements(3, 4, 0, 2, 0, 0);//Hex-C3H6O3
            crfHex_03_X1.Name = "CRFHex_03_X1";
            crfHex_03_X1.SixLetterCode = "H-03X1";
            crfHex_03_X1.ChemicalFormula = "C3H4O2";
            crfHex_03_X1.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfHex_03_X1);

            CrossRing crfHex_24_A2 = new CrossRing();//42.0105612 Fragment
            crfHex_24_A2.NewElements(2, 2, 0, 1, 0, 0);//Hex-C4H8O4
            crfHex_24_A2.Name = "CRFHex_24_A2";
            crfHex_24_A2.SixLetterCode = "H-24A2";
            crfHex_24_A2.ChemicalFormula = "C2H2O";
            crfHex_24_A2.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfHex_24_A2);

            CrossRing crfHex_24_X1 = new CrossRing();//102.0316906 Aldehyde
            crfHex_24_X1.NewElements(4, 6, 0, 3, 0, 0);//Hex-C2H4O2
            crfHex_24_X1.Name = "CRFHex_24_X1";
            crfHex_24_X1.SixLetterCode = "H-24X1";
            crfHex_24_X1.ChemicalFormula = "C4H6O3";
            crfHex_24_X1.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfHex_24_X1);
            #endregion

            #region HexNAc Cross Ring Fragments
            CrossRing crfHexNAc_02_A2 = new CrossRing();//102.031721493 Fragment
            crfHexNAc_02_A2.NewElements(4, 6, 0, 3, 0, 0);//HexNAc-C4H7NO2
            crfHexNAc_02_A2.Name = "CRFHexNAc_02_A2";
            crfHexNAc_02_A2.SixLetterCode = "N-02A2";
            crfHexNAc_02_A2.ChemicalFormula = "C4H6O3";
            crfHexNAc_02_A2.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfHexNAc_02_A2);

            CrossRing crfHexNAc_02_X1 = new CrossRing();//83.013331758 Aldehyde
            crfHexNAc_02_X1.NewElements(4, 5, 1, 1, 0, 0);//HexNAc-C4H8O4
            crfHexNAc_02_X1.Name = "CRFHexNAc_02_X1";
            crfHexNAc_02_X1.SixLetterCode = "N-02X1";
            crfHexNAc_02_X1.ChemicalFormula = "C4H5NO";
            crfHexNAc_02_X1.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfHexNAc_02_X1);
            
            CrossRing crfHexNAc_03_A2 = new CrossRing();// Fragment
            crfHexNAc_03_A2.NewElements(3, 4, 0, 2, 0, 0);//HexNAc-C5H8NO3
            crfHexNAc_03_A2.Name = "CRFHexNAc_03_A2";
            crfHexNAc_03_A2.SixLetterCode = "N-03A2";
            crfHexNAc_03_A2.ChemicalFormula = "C3H4O2";
            crfHexNAc_03_A2.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfHexNAc_03_A2);

            CrossRing crfHexNAc_03_X1 = new CrossRing();// Aldehyde
            crfHexNAc_03_X1.NewElements(5, 6, 1, 2, 0, 0);//HexNAc-C3H6O3
            crfHexNAc_03_X1.Name = "CRFHexNAc_03_X1";
            crfHexNAc_03_X1.SixLetterCode = "N-03X1";
            crfHexNAc_03_X1.ChemicalFormula = "C5H6NO2";
            crfHexNAc_03_X1.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfHexNAc_03_X1);
           
            CrossRing crfHexNAc_24_A2 = new CrossRing();//42.01059293 Fragment
            crfHexNAc_24_A2.NewElements(2, 2, 0, 1, 0, 0);//HexNAc-C6H11NO4
            crfHexNAc_24_A2.Name = "CRFHexNAc_24_A2";
            crfHexNAc_24_A2.SixLetterCode = "N-24A2";
            crfHexNAc_24_A2.ChemicalFormula = "C2H2O";
            crfHexNAc_24_A2.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfHexNAc_24_A2);

            CrossRing crfHexNAc_24_X1 = new CrossRing();//143.0582706 Aldehyde
            crfHexNAc_24_X1.NewElements(6, 9, 1, 3, 0, 0);//HexNAc-C2H4O2
            crfHexNAc_24_X1.Name = "CRFHexNAc_24_X1";
            crfHexNAc_24_X1.SixLetterCode = "N-24X1";
            crfHexNAc_24_X1.ChemicalFormula = "C6H9NO3";
            crfHexNAc_24_X1.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfHexNAc_24_X1);  
            #endregion

            #region Sialic Acid Cross Ring Framgnets
            CrossRing crfNeu5Ac_02_X1 = new CrossRing();//70.0054793084 Aldehyde
            crfNeu5Ac_02_X1.NewElements(3, 2, 0, 2, 0, 0);//Neu5Ac-C8H15NO6
            crfNeu5Ac_02_X1.Name = "CRFNeu5Ac_02_X1";
            crfNeu5Ac_02_X1.SixLetterCode = "A-02X1";
            crfNeu5Ac_02_X1.ChemicalFormula = "C3H2O2";
            crfNeu5Ac_02_X1.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfNeu5Ac_02_X1);

            CrossRing crfNeu5Ac_03_X1 = new CrossRing();// 100.01604399481499 Aldehyde
            crfNeu5Ac_03_X1.NewElements(4, 4, 0, 3, 0, 0);//Neu5Ac-C7H13NO5
            crfNeu5Ac_03_X1.Name = "CRFNeu5Ac_03_X1";
            crfNeu5Ac_03_X1.SixLetterCode = "A-03X1";
            crfNeu5Ac_03_X1.ChemicalFormula = "C4H4O3";
            crfNeu5Ac_03_X1.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfNeu5Ac_03_X1);

            CrossRing crfNeu5Ac_24_X1 = new CrossRing();//190.0477380536 Aldehyde
            crfNeu5Ac_24_X1.NewElements(7, 10, 0, 6, 0, 0);//HexNAc-C4H7NO2
            crfNeu5Ac_24_X1.Name = "CRFNeu5Ac_24_X1";
            crfNeu5Ac_24_X1.SixLetterCode = "A-24X1";
            crfNeu5Ac_24_X1.ChemicalFormula = "C7H10O6";
            crfNeu5Ac_24_X1.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfNeu5Ac_24_X1);

            CrossRing crfNeu5Ac_25_X1 = new CrossRing();//143.0582706 Aldehyde
            crfNeu5Ac_25_X1.NewElements(3, 2, 0, 3, 0, 0);//HexNAc-C8H15NO5
            crfNeu5Ac_25_X1.Name = "CRFNeu5Ac_25_X1";
            crfNeu5Ac_25_X1.SixLetterCode = "A-25X1";
            crfNeu5Ac_25_X1.ChemicalFormula = "C3H2O3";
            crfNeu5Ac_25_X1.MonoIsotopicMass = CrossRing.GetMonoisotopicMass(crfNeu5Ac_25_X1);  
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
