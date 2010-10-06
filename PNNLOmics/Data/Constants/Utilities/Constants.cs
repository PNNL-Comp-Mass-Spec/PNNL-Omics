using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    public class LibraryCompound
    {
        public Dictionary<string, Compound> ConstantsDictionary { get; set; }
        public Dictionary<int, string> ConstantsEnumDictionary { get; set; }
    }

    public class LibrarySubAtomicParticle
    {
        public Dictionary<string, SubAtomicParticle> ConstantsDictionary { get; set; }
        public Dictionary<int, string> ConstantsEnumDictionary { get; set; }
    }

    public class LibraryElement
    {
        public Dictionary<string, Element> ConstantsDictionary { get; set; }
        public Dictionary<int, string> ConstantsEnumDictionary { get; set; }
    }

    public static class Constants
    {
        #region properties

        private static LibraryCompound m_AminoAcidLibrary { get; set; }
        private static LibraryCompound m_MonosaccharideLibrary { get; set; }
        private static LibraryCompound m_CrossRingLibrary { get; set; }
        private static LibraryCompound m_MiscellaneousMatterLibrary { get; set; }
        private static LibrarySubAtomicParticle m_SubAtomicParticleLibrary { get; set; }
        private static LibraryElement m_ElementLibrary { get; set; }

        private static LibraryCompound LibraryAminoAcid //m_AminoacidLibrary
        {
            get
            {
                if (m_AminoAcidLibrary == null)
                {
                    Dictionary<string, Compound> compoundDictionary = AminoAcidLibrary.LoadAminoAcidData();

                    int count = 0;
                    Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
                    foreach (KeyValuePair<string, Compound> item in compoundDictionary)
                    {
                        enumDictionary.Add(count, item.Key);
                        count++;
                    }

                    LibraryCompound compoundLibrary = new LibraryCompound();
                    compoundLibrary.ConstantsDictionary = compoundDictionary;
                    compoundLibrary.ConstantsEnumDictionary = enumDictionary;
                    m_AminoAcidLibrary = compoundLibrary;
                }
                return m_AminoAcidLibrary;
            }
        }

        private static LibraryCompound LibraryMonosaccharide //m_MonosaccharideLibrary
        {
            get
            {
                if (m_MonosaccharideLibrary == null)
                {
                    Dictionary<string, Compound> compoundDictionary = MonosaccharideLibrary.LoadMonosaccharideData();

                    int count = 0;
                    Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
                    foreach (KeyValuePair<string, Compound> item in compoundDictionary)
                    {
                        enumDictionary.Add(count, item.Key);
                        count++;
                    }

                    LibraryCompound compoundLibrary = new LibraryCompound();
                    compoundLibrary.ConstantsDictionary = compoundDictionary;
                    compoundLibrary.ConstantsEnumDictionary = enumDictionary;
                    m_MonosaccharideLibrary = compoundLibrary;
                }
                return m_MonosaccharideLibrary;
            }
        }

        private static LibraryCompound LibraryCrossRing //m_CrossRingLibrary
        {
            get
            {
                if (m_CrossRingLibrary == null)
                {
                    Dictionary<string, Compound> compoundDictionary = CrossRingLibrary.LoadCrossRingData();

                    int count = 0;
                    Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
                    foreach (KeyValuePair<string, Compound> item in compoundDictionary)
                    {
                        enumDictionary.Add(count, item.Key);
                        count++;
                    }

                    LibraryCompound compoundLibrary = new LibraryCompound();
                    compoundLibrary.ConstantsDictionary = compoundDictionary;
                    compoundLibrary.ConstantsEnumDictionary = enumDictionary;
                    m_CrossRingLibrary = compoundLibrary;
                }
                return m_CrossRingLibrary;
            }
        }

        private static LibraryCompound LibraryMiscellaneousMatter //m_MiscellaneousMatterLibrary
        {
            get
            {
                if (m_MiscellaneousMatterLibrary == null)
                {
                    Dictionary<string, Compound> compoundDictionary = MiscellaneousMatterLibrary.LoadMiscellaneousMatterData();

                    int count = 0;
                    Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
                    foreach (KeyValuePair<string, Compound> item in compoundDictionary)
                    {
                        enumDictionary.Add(count, item.Key);
                        count++;
                    }

                    LibraryCompound compoundLibrary = new LibraryCompound();
                    compoundLibrary.ConstantsDictionary = compoundDictionary;
                    compoundLibrary.ConstantsEnumDictionary = enumDictionary;
                    m_MiscellaneousMatterLibrary = compoundLibrary;
                }
                return m_MiscellaneousMatterLibrary;
            }
        }

        private static LibrarySubAtomicParticle LibrarySubAtomicParticle //m_SubAtomicParticleLibrary
        {
            get
            {
                if (m_SubAtomicParticleLibrary == null)
                {
                    Dictionary<string, SubAtomicParticle> subAtomicParticleDictionary = SubAtomicParticleLibrary.LoadSubAtomicParticleData();

                    int count = 0;
                    Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
                    foreach (KeyValuePair<string, SubAtomicParticle> item in subAtomicParticleDictionary)
                    {
                        enumDictionary.Add(count, item.Key);
                        count++;
                    }

                    LibrarySubAtomicParticle subAtomicParticleLibrary = new LibrarySubAtomicParticle();
                    subAtomicParticleLibrary.ConstantsDictionary = subAtomicParticleDictionary;
                    subAtomicParticleLibrary.ConstantsEnumDictionary = enumDictionary;
                    m_SubAtomicParticleLibrary = subAtomicParticleLibrary;
                }
                return m_SubAtomicParticleLibrary;
            }
        }

        private static LibraryElement LibraryElement //m_ElementLibrary
        {
            get
            {
                if (m_ElementLibrary == null)
                {
                    Dictionary<string, Element> elementDictionary = ElementLibrary.LoadElementData();

                    int count = 0;
                    Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
                    foreach (KeyValuePair<string, Element> item in elementDictionary)
                    {
                        enumDictionary.Add(count, item.Key);
                        count++;
                    }

                    LibraryElement elementLibrary = new LibraryElement();
                    elementLibrary.ConstantsDictionary = elementDictionary;
                    elementLibrary.ConstantsEnumDictionary = enumDictionary;
                    m_ElementLibrary = elementLibrary;
                }
                return m_ElementLibrary;
            }
        }

        #endregion

        //Matter/Compound/Element/SubAtomicParticle
        public static double GetMassMonoisotopic(System.Enum chosenKey)
        {
            Type enumDetermination = chosenKey.GetType();

            string constantKey;
            Dictionary<int, string> enumConverter;

            switch (enumDetermination.Name)
            {
                case "SelectMonosaccharide":
                    Dictionary<string, Compound> incommingDictionaryMS = Constants.LibraryMonosaccharide.ConstantsDictionary;
                    enumConverter = Constants.LibraryMonosaccharide.ConstantsEnumDictionary;

                    SelectMonosaccharide internalKeyMS = (SelectMonosaccharide)chosenKey;
                    constantKey = enumConverter[(int)internalKeyMS];
                    return incommingDictionaryMS[constantKey].MassMonoIsotopic;
                
                case "SelectAminoAcid":
                    Dictionary<string, Compound> incommingDictionaryAA = Constants.LibraryAminoAcid.ConstantsDictionary;
                    enumConverter = Constants.LibraryAminoAcid.ConstantsEnumDictionary;

                    SelectAminoAcid internalKeyAA = (SelectAminoAcid)chosenKey;
                    constantKey = enumConverter[(int)internalKeyAA];
                    return incommingDictionaryAA[constantKey].MassMonoIsotopic;

                case "SelectCrossRing":
                    Dictionary<string, Compound> incommingDictionaryCR = Constants.LibraryCrossRing.ConstantsDictionary;
                    enumConverter = Constants.LibraryCrossRing.ConstantsEnumDictionary;

                    SelectCrossRing internalKeyCR = (SelectCrossRing)chosenKey;
                    constantKey = enumConverter[(int)internalKeyCR];
                    return incommingDictionaryCR[constantKey].MassMonoIsotopic;

                case "SelectMiscellaneousMatter":
                    Dictionary<string, Compound> incommingDictionaryMM = Constants.LibraryMiscellaneousMatter.ConstantsDictionary;
                    enumConverter = Constants.LibraryMiscellaneousMatter.ConstantsEnumDictionary;

                    SelectMiscellaneousMatter internalKeyMM = (SelectMiscellaneousMatter)chosenKey;
                    constantKey = enumConverter[(int)internalKeyMM];
                    return incommingDictionaryMM[constantKey].MassMonoIsotopic;

                case "SelectSubAtomicParticle":
                    Dictionary<string, SubAtomicParticle>  incommingDictionarySAP = Constants.LibrarySubAtomicParticle.ConstantsDictionary;
                    enumConverter = Constants.LibrarySubAtomicParticle.ConstantsEnumDictionary;

                    SelectSubAtomicParticle internalKeySAP = (SelectSubAtomicParticle)chosenKey;
                    constantKey = enumConverter[(int)internalKeySAP];
                    return incommingDictionarySAP[constantKey].MassMonoIsotopic;

                case "SelectElement":
                    Dictionary<string, Element> incommingDictionaryE = Constants.LibraryElement.ConstantsDictionary;
                    enumConverter = Constants.LibraryElement.ConstantsEnumDictionary;

                    SelectElement internalKeyE = (SelectElement)chosenKey;
                    constantKey = enumConverter[(int)internalKeyE];
                    return incommingDictionaryE[constantKey].MassMonoIsotopic;

                default:
                    return -1;  
            }

           
        }

        public static double GetMassMonoisotopic(string keyIn, System.Enum chosenKey)
        {
            Dictionary<int, string> enumConverter;

            switch (chosenKey.ToString())
            {
                case "SelectMonosaccharide":
                    Dictionary<string, Compound> incommingDictionaryMS = Constants.LibraryMonosaccharide.ConstantsDictionary;
                    enumConverter = Constants.LibraryMonosaccharide.ConstantsEnumDictionary;
                    return incommingDictionaryMS[keyIn].MassMonoIsotopic;

                case "SelectAminoAcid":
                    Dictionary<string, Compound> incommingDictionaryAA = Constants.LibraryAminoAcid.ConstantsDictionary;
                    enumConverter = Constants.LibraryAminoAcid.ConstantsEnumDictionary;
                    return incommingDictionaryAA[keyIn].MassMonoIsotopic;

                case "SelectCrossRing":
                    Dictionary<string, Compound> incommingDictionaryCR = Constants.LibraryCrossRing.ConstantsDictionary;
                    enumConverter = Constants.LibraryCrossRing.ConstantsEnumDictionary;
                    return incommingDictionaryCR[keyIn].MassMonoIsotopic;

                case "SelectMiscellaneousMatter":
                    Dictionary<string, Compound> incommingDictionaryMM = Constants.LibraryMiscellaneousMatter.ConstantsDictionary;
                    enumConverter = Constants.LibraryMiscellaneousMatter.ConstantsEnumDictionary;
                    return incommingDictionaryMM[keyIn].MassMonoIsotopic;

                case "SelectSubAtomicParticle":
                    Dictionary<string, SubAtomicParticle> incommingDictionarySAP = Constants.LibrarySubAtomicParticle.ConstantsDictionary;
                    enumConverter = Constants.LibrarySubAtomicParticle.ConstantsEnumDictionary;
                    return incommingDictionarySAP[keyIn].MassMonoIsotopic;

                case "SelectElement":
                    Dictionary<string, Element> incommingDictionaryE = Constants.LibraryElement.ConstantsDictionary;
                    enumConverter = Constants.LibraryElement.ConstantsEnumDictionary;
                    return incommingDictionaryE[keyIn].MassMonoIsotopic;

                default:
                    return -1;
            }
        }

        public static string GetName(System.Enum chosenKey)
        {
            Type enumDetermination = chosenKey.GetType();

            string constantKey;
            Dictionary<int, string> enumConverter;

            switch (enumDetermination.Name)
            {
                case "SelectMonosaccharide":
                    Dictionary<string, Compound> incommingDictionaryMS = Constants.LibraryMonosaccharide.ConstantsDictionary;
                    enumConverter = Constants.LibraryMonosaccharide.ConstantsEnumDictionary;

                    SelectMonosaccharide internalKeyMS = (SelectMonosaccharide)chosenKey;
                    constantKey = enumConverter[(int)internalKeyMS];
                    return incommingDictionaryMS[constantKey].Name;

                case "SelectAminoAcid":
                    Dictionary<string, Compound> incommingDictionaryAA = Constants.LibraryAminoAcid.ConstantsDictionary;
                    enumConverter = Constants.LibraryAminoAcid.ConstantsEnumDictionary;

                    SelectAminoAcid internalKeyAA = (SelectAminoAcid)chosenKey;
                    constantKey = enumConverter[(int)internalKeyAA];
                    return incommingDictionaryAA[constantKey].Name;

                case "SelectCrossRing":
                    Dictionary<string, Compound> incommingDictionaryCR = Constants.LibraryCrossRing.ConstantsDictionary;
                    enumConverter = Constants.LibraryCrossRing.ConstantsEnumDictionary;

                    SelectCrossRing internalKeyCR = (SelectCrossRing)chosenKey;
                    constantKey = enumConverter[(int)internalKeyCR];
                    return incommingDictionaryCR[constantKey].Name;

                case "SelectMiscellaneousMatter":
                    Dictionary<string, Compound> incommingDictionaryMM = Constants.LibraryMiscellaneousMatter.ConstantsDictionary;
                    enumConverter = Constants.LibraryMiscellaneousMatter.ConstantsEnumDictionary;

                    SelectMiscellaneousMatter internalKeyMM = (SelectMiscellaneousMatter)chosenKey;
                    constantKey = enumConverter[(int)internalKeyMM];
                    return incommingDictionaryMM[constantKey].Name;

                case "SelectSubAtomicParticle":
                    Dictionary<string, SubAtomicParticle> incommingDictionarySAP = Constants.LibrarySubAtomicParticle.ConstantsDictionary;
                    enumConverter = Constants.LibrarySubAtomicParticle.ConstantsEnumDictionary;

                    SelectSubAtomicParticle internalKeySAP = (SelectSubAtomicParticle)chosenKey;
                    constantKey = enumConverter[(int)internalKeySAP];
                    return incommingDictionarySAP[constantKey].Name;

                case "SelectElement":
                    Dictionary<string, Element> incommingDictionaryE = Constants.LibraryElement.ConstantsDictionary;
                    enumConverter = Constants.LibraryElement.ConstantsEnumDictionary;

                    SelectElement internalKeyE = (SelectElement)chosenKey;
                    constantKey = enumConverter[(int)internalKeyE];
                    return incommingDictionaryE[constantKey].Name;

                default:
                    return "No Name";
            }
        }

        public static string GetName(string keyIn, System.Enum chosenKey)
        {
            Dictionary<int, string> enumConverter;

            switch (chosenKey.ToString())
            {
                case "SelectMonosaccharide":
                    Dictionary<string, Compound> incommingDictionaryMS = Constants.LibraryMonosaccharide.ConstantsDictionary;
                    enumConverter = Constants.LibraryMonosaccharide.ConstantsEnumDictionary;
                    return incommingDictionaryMS[keyIn].Name;

                case "SelectAminoAcid":
                    Dictionary<string, Compound> incommingDictionaryAA = Constants.LibraryAminoAcid.ConstantsDictionary;
                    enumConverter = Constants.LibraryAminoAcid.ConstantsEnumDictionary;
                    return incommingDictionaryAA[keyIn].Name;

                case "SelectCrossRing":
                    Dictionary<string, Compound> incommingDictionaryCR = Constants.LibraryCrossRing.ConstantsDictionary;
                    enumConverter = Constants.LibraryCrossRing.ConstantsEnumDictionary;
                    return incommingDictionaryCR[keyIn].Name;

                case "SelectMiscellaneousMatter":
                    Dictionary<string, Compound> incommingDictionaryMM = Constants.LibraryMiscellaneousMatter.ConstantsDictionary;
                    enumConverter = Constants.LibraryMiscellaneousMatter.ConstantsEnumDictionary;
                    return incommingDictionaryMM[keyIn].Name;

                case "SelectSubAtomicParticle":
                    Dictionary<string, SubAtomicParticle> incommingDictionarySAP = Constants.LibrarySubAtomicParticle.ConstantsDictionary;
                    enumConverter = Constants.LibrarySubAtomicParticle.ConstantsEnumDictionary;
                    return incommingDictionarySAP[keyIn].Name;

                case "SelectElement":
                    Dictionary<string, Element> incommingDictionaryE = Constants.LibraryElement.ConstantsDictionary;
                    enumConverter = Constants.LibraryElement.ConstantsEnumDictionary;
                    return incommingDictionaryE[keyIn].Name;

                default:
                    return "No Name";
            }
        }

        public static string GetSymbol(System.Enum chosenKey)
        {
            Type enumDetermination = chosenKey.GetType();

            string constantKey;
            Dictionary<int, string> enumConverter;

            switch (enumDetermination.Name)
            {
                case "SelectMonosaccharide":
                    Dictionary<string, Compound> incommingDictionaryMS = Constants.LibraryMonosaccharide.ConstantsDictionary;
                    enumConverter = Constants.LibraryMonosaccharide.ConstantsEnumDictionary;

                    SelectMonosaccharide internalKeyMS = (SelectMonosaccharide)chosenKey;
                    constantKey = enumConverter[(int)internalKeyMS];
                    return incommingDictionaryMS[constantKey].Symbol;

                case "SelectAminoAcid":
                    Dictionary<string, Compound> incommingDictionaryAA = Constants.LibraryAminoAcid.ConstantsDictionary;
                    enumConverter = Constants.LibraryAminoAcid.ConstantsEnumDictionary;

                    SelectAminoAcid internalKeyAA = (SelectAminoAcid)chosenKey;
                    constantKey = enumConverter[(int)internalKeyAA];
                    return incommingDictionaryAA[constantKey].Symbol;

                case "SelectCrossRing":
                    Dictionary<string, Compound> incommingDictionaryCR = Constants.LibraryCrossRing.ConstantsDictionary;
                    enumConverter = Constants.LibraryCrossRing.ConstantsEnumDictionary;

                    SelectCrossRing internalKeyCR = (SelectCrossRing)chosenKey;
                    constantKey = enumConverter[(int)internalKeyCR];
                    return incommingDictionaryCR[constantKey].Symbol;

                case "SelectMiscellaneousMatter":
                    Dictionary<string, Compound> incommingDictionaryMM = Constants.LibraryMiscellaneousMatter.ConstantsDictionary;
                    enumConverter = Constants.LibraryMiscellaneousMatter.ConstantsEnumDictionary;

                    SelectMiscellaneousMatter internalKeyMM = (SelectMiscellaneousMatter)chosenKey;
                    constantKey = enumConverter[(int)internalKeyMM];
                    return incommingDictionaryMM[constantKey].Symbol;

                case "SelectSubAtomicParticle":
                    Dictionary<string, SubAtomicParticle> incommingDictionarySAP = Constants.LibrarySubAtomicParticle.ConstantsDictionary;
                    enumConverter = Constants.LibrarySubAtomicParticle.ConstantsEnumDictionary;

                    SelectSubAtomicParticle internalKeySAP = (SelectSubAtomicParticle)chosenKey;
                    constantKey = enumConverter[(int)internalKeySAP];
                    return incommingDictionarySAP[constantKey].Symbol;

                case "SelectElement":
                    Dictionary<string, Element> incommingDictionaryE = Constants.LibraryElement.ConstantsDictionary;
                    enumConverter = Constants.LibraryElement.ConstantsEnumDictionary;

                    SelectElement internalKeyE = (SelectElement)chosenKey;
                    constantKey = enumConverter[(int)internalKeyE];
                    return incommingDictionaryE[constantKey].Symbol;

                default:
                    return "No Symbol";
            }
        }

        public static string GetSymbol(string keyIn, System.Enum chosenKey)
        {
            Dictionary<int, string> enumConverter;

            switch (chosenKey.ToString())
            {
                case "SelectMonosaccharide":
                    Dictionary<string, Compound> incommingDictionaryMS = Constants.LibraryMonosaccharide.ConstantsDictionary;
                    enumConverter = Constants.LibraryMonosaccharide.ConstantsEnumDictionary;
                    return incommingDictionaryMS[keyIn].Symbol;

                case "SelectAminoAcid":
                    Dictionary<string, Compound> incommingDictionaryAA = Constants.LibraryAminoAcid.ConstantsDictionary;
                    enumConverter = Constants.LibraryAminoAcid.ConstantsEnumDictionary;
                    return incommingDictionaryAA[keyIn].Symbol;

                case "SelectCrossRing":
                    Dictionary<string, Compound> incommingDictionaryCR = Constants.LibraryCrossRing.ConstantsDictionary;
                    enumConverter = Constants.LibraryCrossRing.ConstantsEnumDictionary;
                    return incommingDictionaryCR[keyIn].Symbol;

                case "SelectMiscellaneousMatter":
                    Dictionary<string, Compound> incommingDictionaryMM = Constants.LibraryMiscellaneousMatter.ConstantsDictionary;
                    enumConverter = Constants.LibraryMiscellaneousMatter.ConstantsEnumDictionary;
                    return incommingDictionaryMM[keyIn].Symbol;

                case "SelectSubAtomicParticle":
                    Dictionary<string, SubAtomicParticle> incommingDictionarySAP = Constants.LibrarySubAtomicParticle.ConstantsDictionary;
                    enumConverter = Constants.LibrarySubAtomicParticle.ConstantsEnumDictionary;
                    return incommingDictionarySAP[keyIn].Symbol;

                case "SelectElement":
                    Dictionary<string, Element> incommingDictionaryE = Constants.LibraryElement.ConstantsDictionary;
                    enumConverter = Constants.LibraryElement.ConstantsEnumDictionary;
                    return incommingDictionaryE[keyIn].Symbol;

                default:
                    return "No Symbol";
            }
        }


        //Compound only
        public static string GetFormula(System.Enum chosenKey)
        {
            Type enumDetermination = chosenKey.GetType();

            string constantKey;
            Dictionary<int, string> enumConverter;

            switch (enumDetermination.Name)
            {
                case "SelectMonosaccharide":
                    Dictionary<string, Compound> incommingDictionaryMS = Constants.LibraryMonosaccharide.ConstantsDictionary;
                    enumConverter = Constants.LibraryMonosaccharide.ConstantsEnumDictionary;

                    SelectMonosaccharide internalKeyMS = (SelectMonosaccharide)chosenKey;
                    constantKey = enumConverter[(int)internalKeyMS];
                    return incommingDictionaryMS[constantKey].ChemicalFormula;

                case "SelectAminoAcid":
                    Dictionary<string, Compound> incommingDictionaryAA = Constants.LibraryAminoAcid.ConstantsDictionary;
                    enumConverter = Constants.LibraryAminoAcid.ConstantsEnumDictionary;

                    SelectAminoAcid internalKeyAA = (SelectAminoAcid)chosenKey;
                    constantKey = enumConverter[(int)internalKeyAA];
                    return incommingDictionaryAA[constantKey].ChemicalFormula;

                case "SelectCrossRing":
                    Dictionary<string, Compound> incommingDictionaryCR = Constants.LibraryCrossRing.ConstantsDictionary;
                    enumConverter = Constants.LibraryCrossRing.ConstantsEnumDictionary;

                    SelectCrossRing internalKeyCR = (SelectCrossRing)chosenKey;
                    constantKey = enumConverter[(int)internalKeyCR];
                    return incommingDictionaryCR[constantKey].ChemicalFormula;

                case "SelectMiscellaneousMatter":
                    Dictionary<string, Compound> incommingDictionaryMM = Constants.LibraryMiscellaneousMatter.ConstantsDictionary;
                    enumConverter = Constants.LibraryMiscellaneousMatter.ConstantsEnumDictionary;

                    SelectMiscellaneousMatter internalKeyMM = (SelectMiscellaneousMatter)chosenKey;
                    constantKey = enumConverter[(int)internalKeyMM];
                    return incommingDictionaryMM[constantKey].ChemicalFormula;

                //case "SelectSubAtomicParticle":  NONE
                    
                //case "SelectElement":  NONE

                default:
                    return "No Formula";
            }
        }

        public static string GetFormula(string keyIn, System.Enum chosenKey)
        {
            Dictionary<int, string> enumConverter;

            switch (chosenKey.ToString())
            {
                case "SelectMonosaccharide":
                    Dictionary<string, Compound> incommingDictionaryMS = Constants.LibraryMonosaccharide.ConstantsDictionary;
                    enumConverter = Constants.LibraryMonosaccharide.ConstantsEnumDictionary;
                    return incommingDictionaryMS[keyIn].ChemicalFormula;

                case "SelectAminoAcid":
                    Dictionary<string, Compound> incommingDictionaryAA = Constants.LibraryAminoAcid.ConstantsDictionary;
                    enumConverter = Constants.LibraryAminoAcid.ConstantsEnumDictionary;
                    return incommingDictionaryAA[keyIn].ChemicalFormula;

                case "SelectCrossRing":
                    Dictionary<string, Compound> incommingDictionaryCR = Constants.LibraryCrossRing.ConstantsDictionary;
                    enumConverter = Constants.LibraryCrossRing.ConstantsEnumDictionary;
                    return incommingDictionaryCR[keyIn].ChemicalFormula;

                case "SelectMiscellaneousMatter":
                    Dictionary<string, Compound> incommingDictionaryMM = Constants.LibraryMiscellaneousMatter.ConstantsDictionary;
                    enumConverter = Constants.LibraryMiscellaneousMatter.ConstantsEnumDictionary;
                    return incommingDictionaryMM[keyIn].ChemicalFormula;

                // case "SelectSubAtomicParticle":  NONE

                //case "SelectElement": NONE

                default:
                    return "No Formula";
            }
        }

        //Element only
        public static double GetMassAverage(System.Enum chosenKey)//TODO:  not implemented yet
        {
        //this has not been implemented yet because the XML document does not have the values.  The hard coded version does.
            
            //Type enumDetermination = chosenKey.GetType();

            //string constantKey;
            //Dictionary<int, string> enumConverter;

            //switch (enumDetermination.Name)
            //{
            //    //case "SelectMonosaccharide":                   
            //    //case "SelectAminoAcid":                   
            //    //case "SelectCrossRing":                   
            //    //case "SelectMiscellaneousMatter":                  
            //    //case "SelectSubAtomicParticle":

            //    case "SelectElement":
            //        Dictionary<string, Element> incommingDictionaryE = Constants.LibraryElement.ConstantsDictionary;
            //        enumConverter = Constants.LibraryElement.ConstantsEnumDictionary;

            //        SelectElement internalKeyE = (SelectElement)chosenKey;
            //        constantKey = enumConverter[(int)internalKeyE];
            //        return incommingDictionaryE[constantKey].MassAverage;
                    
            //    default:
                    return -1;
            //}
        }

        //Element only (isotopes)
        public static double GetIsotopeMass(System.Enum chosenKey, string isotopeKey)
        {
            Type enumDetermination = chosenKey.GetType();

            string constantKey;
            Dictionary<int, string> enumConverter;

            switch (enumDetermination.Name)
            {
                //case "SelectMonosaccharide":   
                //case "SelectAminoAcid":       
                //case "SelectCrossRing":                  
                //case "SelectMiscellaneousMatter":                  
                //case "SelectSubAtomicParticle":  

                case "SelectElement": 
                    Dictionary<string, Element> incommingDictionaryE = Constants.LibraryElement.ConstantsDictionary;
                    enumConverter = Constants.LibraryElement.ConstantsEnumDictionary;

                    SelectElement internalKeyE = (SelectElement)chosenKey;
                    constantKey = enumConverter[(int)internalKeyE];

                    return incommingDictionaryE[constantKey].IsotopeDictionary[isotopeKey].Mass;

                default:
                    return -1;
            }
        }

        public static double GetIsotopeAbundance(System.Enum chosenKey, string isotopeKey)
        {
            Type enumDetermination = chosenKey.GetType();

            string constantKey;
            Dictionary<int, string> enumConverter;

            switch (enumDetermination.Name)
            {
                //case "SelectMonosaccharide":   
                //case "SelectAminoAcid":       
                //case "SelectCrossRing":                  
                //case "SelectMiscellaneousMatter":                  
                //case "SelectSubAtomicParticle":  

                case "SelectElement":
                    Dictionary<string, Element> incommingDictionaryE = Constants.LibraryElement.ConstantsDictionary;
                    enumConverter = Constants.LibraryElement.ConstantsEnumDictionary;

                    SelectElement internalKeyE = (SelectElement)chosenKey;
                    constantKey = enumConverter[(int)internalKeyE];

                    return incommingDictionaryE[constantKey].IsotopeDictionary[isotopeKey].NaturalAbundance;

                default:
                    return -1;
            }
        }

        public static int GetIsotopeNumber(System.Enum chosenKey, string isotopeKey)
        {
            Type enumDetermination = chosenKey.GetType();

            string constantKey;
            Dictionary<int, string> enumConverter;

            switch (enumDetermination.Name)
            {
                //case "SelectMonosaccharide":   
                //case "SelectAminoAcid":       
                //case "SelectCrossRing":                  
                //case "SelectMiscellaneousMatter":                  
                //case "SelectSubAtomicParticle":  

                case "SelectElement":
                    Dictionary<string, Element> incommingDictionaryE = Constants.LibraryElement.ConstantsDictionary;
                    enumConverter = Constants.LibraryElement.ConstantsEnumDictionary;

                    SelectElement internalKeyE = (SelectElement)chosenKey;
                    constantKey = enumConverter[(int)internalKeyE];

                    return incommingDictionaryE[constantKey].IsotopeDictionary[isotopeKey].IsotopeNumber;

                default:
                    return -1;
            }
        }

        public static Dictionary<string,Isotope> GetIsotopeDictionary(System.Enum chosenKey)
        {
            Type enumDetermination = chosenKey.GetType();

            string constantKey;
            Dictionary<int, string> enumConverter;

            switch (enumDetermination.Name)
            {
                //case "SelectMonosaccharide":   
                //case "SelectAminoAcid":       
                //case "SelectCrossRing":                  
                //case "SelectMiscellaneousMatter":                  
                //case "SelectSubAtomicParticle":  

                case "SelectElement":
                    Dictionary<string, Element> incommingDictionaryE = Constants.LibraryElement.ConstantsDictionary;
                    enumConverter = Constants.LibraryElement.ConstantsEnumDictionary;

                    SelectElement internalKeyE = (SelectElement)chosenKey;
                    constantKey = enumConverter[(int)internalKeyE];

                    return incommingDictionaryE[constantKey].IsotopeDictionary;

                default:
                    Dictionary<string, Isotope> voidDictionary = new Dictionary<string, Isotope>();
                    return voidDictionary;
            }
        }

        //Return full libraries
        public static LibraryCompound GetLibraryCompound(System.Enum chosenKey)
        {
            switch (chosenKey.ToString())
            {
                case "SelectMonosaccharide":
                    return Constants.LibraryMonosaccharide;

                case "SelectAminoAcid":
                    return Constants.LibraryAminoAcid;

                case "SelectCrossRing":
                    return Constants.LibraryCrossRing; ;

                case "SelectMiscellaneousMatter":
                    return Constants.LibraryMiscellaneousMatter;

                //case "SelectSubAtomicParticle":

                //case "selectElement":

                default:
                    LibraryCompound compoundLibrary = new LibraryCompound();
                    return compoundLibrary;
            }
        }

        public static LibraryElement GetLibraryElement(System.Enum chosenKey)
        {
            switch (chosenKey.ToString())
            {
                //case "SelectMonosaccharide":

                //case "SelectAminoAcid":

                //case "SelectCrossRing":

                //case "SelectMiscellaneousMatter":

                //case "SelectSubAtomicParticle":

                case "SelectElement":
                    return Constants.LibraryElement;

                default:
                    LibraryElement elementLibrary = new LibraryElement();
                    return elementLibrary;
            }
        }

        public static LibrarySubAtomicParticle GetLibrarySubAtomicParticle(System.Enum chosenKey)
        {
            switch (chosenKey.ToString())
            {
                //case "SelectMonosaccharide":
                   
                //case "SelectAminoAcid":
                    
                //case "SelectCrossRing":
                    
                //case "SelectMiscellaneousMatter":

                case "SelectSubAtomicParticle":
                    return Constants.LibrarySubAtomicParticle;

                //case "selectElement":

                default:
                    LibrarySubAtomicParticle subAtomicParticleLibrary = new LibrarySubAtomicParticle();
                    return subAtomicParticleLibrary;
            }
        }   
    }

    public enum SelectLibrary
    { 
        SelectMonosaccharide, SelectAminoAcid, SelectCrossRing, SelectMiscellaneousMatter, SelectSubAtomicParticle, SelectElement
    }
}