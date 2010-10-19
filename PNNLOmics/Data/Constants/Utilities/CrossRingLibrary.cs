using System;
using System.Collections.Generic;
using PNNLOmics.Data.Constants;
using PNNLOmics.Data.Constants.Enumerations;

/// <example>
/// dictionarty implementation
/// Dictionary<string, Compound> CrossRingDictionary = CrossRingLibrary.LoadCrossRingData();
/// double CRMass = CrossRingDictionary["crfNeu5Ac_03_X1"].MonoIsotopicMass;
/// string crformula = CrossRingDictionary["crfNeu5Ac_03_X1"].ChemicalFormula;
/// string CRName = CrossRingDictionary["crfNeu5Ac_03_X1"].Name;
///
/// one line implementation
/// double CRMass2 = CrossRingConstantsStaticLibrary.GetMonoisotopicMass("crfNeu5Ac_03_X1");
/// string CRFormula2 = CrossRingConstantsStaticLibrary.GetFormula("crfNeu5Ac_03_X1");
/// string CRName2 = CrossRingConstantsStaticLibrary.GetName("crfNeu5Ac_03_X1");
///
/// double mass2 = CrossRingStaticLibrary.GetMonoisotopicMass(SelectCrossRing.CRFHex_02_A2);
/// </example>

namespace PNNLOmics.Data.Constants.Utilities
{
    //TODO: SCOTT - CR - add XML comments
    public class CrossRingLibrary : MatterLibrary<Compound, CrossRingName>
    {
        /// <summary>
        /// This is a Class designed to create cross ring fragments objects of monosaccharides from the elements.
        /// The cross ring fragments are added to a Dictionary searchable with string keys such as "CRFNeu5Ac_02_X1" for the 
        /// X1 cross ring fragment which breaks across the 0 and 2 ring bonds of a Neuraminic acid monosachcaride
        /// </summary>
        public override Dictionary<string, Compound> LoadLibrary()
        {
            m_symbolToCompoundMap = new Dictionary<string, Compound>();
            m_enumToSymbolMap = new Dictionary<CrossRingName, string>();

            //Deoxyhexose.NewElements(C H N O S P)

            //A ions retain the charge on the non-reducing side producing a fragment because hte core is lost
            //X ions keep the core and keep the aldehyde motif

            #region Hexose Cross Ring Fragments
            Compound crfHex_02_A2 = new Compound();//102.0316906 Fragment
            crfHex_02_A2.NewElements(4, 6, 0, 3, 0, 0);//Hex-C2H4O2//-->X.NewElements(C H N O S P) number of atoms
            crfHex_02_A2.Name = "CRFHex_02_A2";
            crfHex_02_A2.Symbol = "H-02A2";
            crfHex_02_A2.ChemicalFormula = "C4H6O3";
            crfHex_02_A2.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfHex_02_A2);

            Compound crfHex_02_X1 = new Compound();//42.0105612 Aldehyde
            crfHex_02_X1.NewElements(2, 2, 0, 1, 0, 0);//Hex-C4H8O4//-->X.NewElements(C H N O S P) number of atoms
            crfHex_02_X1.Name = "CRFHex_02_X1";
            crfHex_02_X1.Symbol = "H-02X1";
            crfHex_02_X1.ChemicalFormula = "C2H2O";
            crfHex_02_X1.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfHex_02_X1);

            Compound crfHex_03_A2 = new Compound();//72.0211259 Aldehyde
            crfHex_03_A2.NewElements(3, 4, 0, 2, 0, 0);//Hex-C3H6O3//-->X.NewElements(C H N O S P) number of atoms
            crfHex_03_A2.Name = "CRFHex_03_A2";
            crfHex_03_A2.Symbol = "H-03A2";
            crfHex_03_A2.ChemicalFormula = "C3H4O2";
            crfHex_03_A2.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfHex_03_A2);

            Compound crfHex_03_X1 = new Compound();//72.0211259 Aldehyde
            crfHex_03_X1.NewElements(3, 4, 0, 2, 0, 0);//Hex-C3H6O3//-->X.NewElements(C H N O S P) number of atoms
            crfHex_03_X1.Name = "CRFHex_03_X1";
            crfHex_03_X1.Symbol = "H-03X1";
            crfHex_03_X1.ChemicalFormula = "C3H4O2";
            crfHex_03_X1.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfHex_03_X1);

            Compound crfHex_24_A2 = new Compound();//42.0105612 Fragment
            crfHex_24_A2.NewElements(2, 2, 0, 1, 0, 0);//Hex-C4H8O4//-->X.NewElements(C H N O S P) number of atoms
            crfHex_24_A2.Name = "CRFHex_24_A2";
            crfHex_24_A2.Symbol = "H-24A2";
            crfHex_24_A2.ChemicalFormula = "C2H2O";
            crfHex_24_A2.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfHex_24_A2);

            Compound crfHex_24_X1 = new Compound();//102.0316906 Aldehyde
            crfHex_24_X1.NewElements(4, 6, 0, 3, 0, 0);//Hex-C2H4O2//-->X.NewElements(C H N O S P) number of atoms
            crfHex_24_X1.Name = "CRFHex_24_X1";
            crfHex_24_X1.Symbol = "H-24X1";
            crfHex_24_X1.ChemicalFormula = "C4H6O3";
            crfHex_24_X1.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfHex_24_X1);
            #endregion

            #region HexNAc Cross Ring Fragments
            Compound crfHexNAc_02_A2 = new Compound();//102.031721493 Fragment
            crfHexNAc_02_A2.NewElements(4, 6, 0, 3, 0, 0);//HexNAc-C4H7NO2//-->X.NewElements(C H N O S P) number of atoms
            crfHexNAc_02_A2.Name = "CRFHexNAc_02_A2";
            crfHexNAc_02_A2.Symbol = "N-02A2";
            crfHexNAc_02_A2.ChemicalFormula = "C4H6O3";
            crfHexNAc_02_A2.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfHexNAc_02_A2);

            Compound crfHexNAc_02_X1 = new Compound();//83.013331758 Aldehyde
            crfHexNAc_02_X1.NewElements(4, 5, 1, 1, 0, 0);//HexNAc-C4H8O4//-->X.NewElements(C H N O S P) number of atoms
            crfHexNAc_02_X1.Name = "CRFHexNAc_02_X1";
            crfHexNAc_02_X1.Symbol = "N-02X1";
            crfHexNAc_02_X1.ChemicalFormula = "C4H5NO";
            crfHexNAc_02_X1.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfHexNAc_02_X1);

            Compound crfHexNAc_03_A2 = new Compound();// Fragment
            crfHexNAc_03_A2.NewElements(3, 4, 0, 2, 0, 0);//HexNAc-C5H8NO3//-->X.NewElements(C H N O S P) number of atoms
            crfHexNAc_03_A2.Name = "CRFHexNAc_03_A2";
            crfHexNAc_03_A2.Symbol = "N-03A2";
            crfHexNAc_03_A2.ChemicalFormula = "C3H4O2";
            crfHexNAc_03_A2.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfHexNAc_03_A2);

            Compound crfHexNAc_03_X1 = new Compound();// Aldehyde
            crfHexNAc_03_X1.NewElements(5, 6, 1, 2, 0, 0);//HexNAc-C3H6O3//-->X.NewElements(C H N O S P) number of atoms
            crfHexNAc_03_X1.Name = "CRFHexNAc_03_X1";
            crfHexNAc_03_X1.Symbol = "N-03X1";
            crfHexNAc_03_X1.ChemicalFormula = "C5H6NO2";
            crfHexNAc_03_X1.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfHexNAc_03_X1);

            Compound crfHexNAc_24_A2 = new Compound();//42.01059293 Fragment
            crfHexNAc_24_A2.NewElements(2, 2, 0, 1, 0, 0);//HexNAc-C6H11NO4//-->X.NewElements(C H N O S P) number of atoms
            crfHexNAc_24_A2.Name = "CRFHexNAc_24_A2";
            crfHexNAc_24_A2.Symbol = "N-24A2";
            crfHexNAc_24_A2.ChemicalFormula = "C2H2O";
            crfHexNAc_24_A2.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfHexNAc_24_A2);

            Compound crfHexNAc_24_X1 = new Compound();//143.0582706 Aldehyde
            crfHexNAc_24_X1.NewElements(6, 9, 1, 3, 0, 0);//HexNAc-C2H4O2//-->X.NewElements(C H N O S P) number of atoms
            crfHexNAc_24_X1.Name = "CRFHexNAc_24_X1";
            crfHexNAc_24_X1.Symbol = "N-24X1";
            crfHexNAc_24_X1.ChemicalFormula = "C6H9NO3";
            crfHexNAc_24_X1.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfHexNAc_24_X1);
            #endregion

            //TODO: SCOTT - CR - change fragments 
            #region Sialic Acid Cross Ring Framgnets
            Compound crfNeu5Ac_02_X1 = new Compound();//70.0054793084 Aldehyde
            crfNeu5Ac_02_X1.NewElements(3, 2, 0, 2, 0, 0);//Neu5Ac-C8H15NO6//-->X.NewElements(C H N O S P) number of atoms
            crfNeu5Ac_02_X1.Name = "CRFNeu5Ac_02_X1";
            crfNeu5Ac_02_X1.Symbol = "A-02X1";
            crfNeu5Ac_02_X1.ChemicalFormula = "C3H2O2";
            crfNeu5Ac_02_X1.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfNeu5Ac_02_X1);

            Compound crfNeu5Ac_03_X1 = new Compound();// 100.01604399481499 Aldehyde
            crfNeu5Ac_03_X1.NewElements(4, 4, 0, 3, 0, 0);//Neu5Ac-C7H13NO5//-->X.NewElements(C H N O S P) number of atoms
            crfNeu5Ac_03_X1.Name = "CRFNeu5Ac_03_X1";
            crfNeu5Ac_03_X1.Symbol = "A-03X1";
            crfNeu5Ac_03_X1.ChemicalFormula = "C4H4O3";
            crfNeu5Ac_03_X1.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfNeu5Ac_03_X1);

            Compound crfNeu5Ac_24_X1 = new Compound();//190.0477380536 Aldehyde
            crfNeu5Ac_24_X1.NewElements(7, 10, 0, 6, 0, 0);//HexNAc-C4H7NO2//-->X.NewElements(C H N O S P) number of atoms
            crfNeu5Ac_24_X1.Name = "CRFNeu5Ac_24_X1";
            crfNeu5Ac_24_X1.Symbol = "A-24X1";
            crfNeu5Ac_24_X1.ChemicalFormula = "C7H10O6";
            crfNeu5Ac_24_X1.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfNeu5Ac_24_X1);

            Compound crfNeu5Ac_25_X1 = new Compound();//143.0582706 Aldehyde
            crfNeu5Ac_25_X1.NewElements(3, 2, 0, 3, 0, 0);//HexNAc-C8H15NO5//-->X.NewElements(C H N O S P) number of atoms
            crfNeu5Ac_25_X1.Name = "CRFNeu5Ac_25_X1";
            crfNeu5Ac_25_X1.Symbol = "A-25X1";
            crfNeu5Ac_25_X1.ChemicalFormula = "C3H2O3";
            crfNeu5Ac_25_X1.MassMonoIsotopic = Compound.GetMonoisotopicMass(crfNeu5Ac_25_X1);
            #endregion

            m_symbolToCompoundMap.Add(crfHex_02_A2.Symbol, crfHex_02_A2);
            m_symbolToCompoundMap.Add(crfHex_02_X1.Symbol, crfHex_02_X1);
            m_symbolToCompoundMap.Add(crfHex_03_A2.Symbol, crfHex_03_A2);
            m_symbolToCompoundMap.Add(crfHex_03_X1.Symbol, crfHex_03_X1);
            m_symbolToCompoundMap.Add(crfHex_24_A2.Symbol, crfHex_24_A2);
            m_symbolToCompoundMap.Add(crfHex_24_X1.Symbol, crfHex_24_X1);

            m_symbolToCompoundMap.Add(crfHexNAc_02_A2.Symbol, crfHexNAc_02_A2);
            m_symbolToCompoundMap.Add(crfHexNAc_02_X1.Symbol, crfHexNAc_02_X1);
            m_symbolToCompoundMap.Add(crfHexNAc_03_A2.Symbol, crfHexNAc_03_A2);
            m_symbolToCompoundMap.Add(crfHexNAc_03_X1.Symbol, crfHexNAc_03_X1);
            m_symbolToCompoundMap.Add(crfHexNAc_24_A2.Symbol, crfHexNAc_24_A2);
            m_symbolToCompoundMap.Add(crfHexNAc_24_X1.Symbol, crfHexNAc_24_X1);

            m_symbolToCompoundMap.Add(crfNeu5Ac_02_X1.Symbol, crfNeu5Ac_02_X1);
            m_symbolToCompoundMap.Add(crfNeu5Ac_03_X1.Symbol, crfNeu5Ac_03_X1);
            m_symbolToCompoundMap.Add(crfNeu5Ac_24_X1.Symbol, crfNeu5Ac_24_X1);
            m_symbolToCompoundMap.Add(crfNeu5Ac_25_X1.Symbol, crfNeu5Ac_25_X1);

            m_enumToSymbolMap.Add(CrossRingName.CRFHex_02_A2, crfHex_02_A2.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFHex_02_X1, crfHex_02_X1.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFHex_03_A2, crfHex_03_A2.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFHex_03_X1, crfHex_03_X1.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFHex_24_A2, crfHex_24_A2.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFHex_24_X1, crfHex_24_X1.Symbol);

            m_enumToSymbolMap.Add(CrossRingName.CRFHexNAc_02_A2, crfHexNAc_02_A2.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFHexNAc_02_X1, crfHexNAc_02_X1.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFHexNAc_03_A2, crfHexNAc_03_A2.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFHexNAc_03_X1, crfHexNAc_03_X1.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFHexNAc_24_A2, crfHexNAc_24_A2.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFHexNAc_24_X1, crfHexNAc_24_X1.Symbol);

            m_enumToSymbolMap.Add(CrossRingName.CRFNeu5Ac_02_X1, crfNeu5Ac_02_X1.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFNeu5Ac_03_X1, crfNeu5Ac_03_X1.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFNeu5Ac_24_X1, crfNeu5Ac_24_X1.Symbol);
            m_enumToSymbolMap.Add(CrossRingName.CRFNeu5Ac_25_X1, crfNeu5Ac_25_X1.Symbol);

            return m_symbolToCompoundMap;
        }
    }
}
