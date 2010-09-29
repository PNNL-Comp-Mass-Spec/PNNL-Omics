using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    //TODO:  XML comments on properties
    //TODO:  add interface.  Include anyting that is required from a library.  one interface for a ImatterLirary.  each library extends the interface. 


    /// <summary>
    /// This class loads the monosaccharide constants once and is accessble through the dictionary property.  Thread-safe singleton example created at first call
    /// </summary> 
    public sealed class CompoundSingleton
    {
        /// <summary>
        /// creates a single instance upon creation
        /// </summary>
        public static CompoundSingleton Instance { get; private set; }

        /// <summary>
        /// A static constructor is automatically initialized on referenceto the class and sets up a blank set of dictionaries.
        /// </summary>
        static CompoundSingleton()
        {
            Instance = new CompoundSingleton();
            Instance.IsMonosaccharideLibraryLoaded = false;
            Instance.IsAminoAcidLibraryLoaded = false;
            Instance.IsCrossRingLibraryLoaded = false;
            Instance.IsMiscellaneousMatterLibraryLoaded = false;
        }

        private bool IsMonosaccharideLibraryLoaded { get; set; }
        private bool IsAminoAcidLibraryLoaded { get; set; }
        private bool IsCrossRingLibraryLoaded { get; set; }
        private bool IsMiscellaneousMatterLibraryLoaded { get; set; }

        //the part of the singleton that does the work once.
        CompoundSingleton()
        {
        }

        public Dictionary<string, Compound> MonosaccharideConstantsDictionary { get; set; }
        public Dictionary<int, string> MonosaccharideConstantsEnumDictionary { get; set; }

        public Dictionary<string, Compound> AminoAcidConstantsDictionary { get; set; }
        public Dictionary<int, string> AminoAcidConstantsEnumDictionary { get; set; }

        public Dictionary<string, Compound> CrossRingConstantsDictionary { get; set; }
        public Dictionary<int, string> CrossRingConstantsEnumDictionary { get; set; }

        public Dictionary<string, Compound> MiscellaneousMatterConstantsDictionary { get; set; }
        public Dictionary<int, string> MiscellaneousMatterConstantsEnumDictionary { get; set; }


        public void InitializeMonosacharideLibrary()
        {
            if (!IsMonosaccharideLibraryLoaded)
            {
                Dictionary<string, Compound> compoundDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
                this.MonosaccharideConstantsDictionary = compoundDictionary;//accessable outside by getter below

                int count = 0;
                string names = "";
                Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
                foreach (KeyValuePair<string, Compound> item in compoundDictionary)
                {
                    names += item.Key + ",";
                    enumDictionary.Add(count, item.Key);
                    count++;
                }

                names = "";
                for (int i = 0; i < compoundDictionary.Count; i++)
                {
                    names += MonosaccharideConstantsDictionary[enumDictionary[i]].Name + ",";
                }
                this.MonosaccharideConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
                this.IsMonosaccharideLibraryLoaded = true;
            }
        }

        public void InitializeAminoAcidLibrary()
        {
            if (!IsAminoAcidLibraryLoaded)
            {
                Dictionary<string, Compound> compoundDictionary = AminoAcidLibrary.LoadAminoAcidData();
                this.AminoAcidConstantsDictionary = compoundDictionary;//accessable outside by getter below

                int count = 0;
                string names = "";
                Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
                foreach (KeyValuePair<string, Compound> item in compoundDictionary)
                {
                    names += item.Key + ",";
                    enumDictionary.Add(count, item.Key);
                    count++;
                }

                names = "";
                for (int i = 0; i < compoundDictionary.Count; i++)
                {
                    names += AminoAcidConstantsDictionary[enumDictionary[i]].Name + ",";
                }
                this.AminoAcidConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
                this.IsAminoAcidLibraryLoaded = true;
            }
        }

        public void InitializeCrossRingLibrary()
        {
            if (!IsCrossRingLibraryLoaded)
            {
                Dictionary<string, Compound> compoundDictionary = CrossRingLibrary.LoadCrossRingData();
                this.CrossRingConstantsDictionary = compoundDictionary;//accessable outside by getter below

                int count = 0;
                string names = "";
                Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
                foreach (KeyValuePair<string, Compound> item in compoundDictionary)
                {
                    names += item.Key + ",";
                    enumDictionary.Add(count, item.Key);
                    count++;
                }

                names = "";
                for (int i = 0; i < compoundDictionary.Count; i++)
                {
                    names += CrossRingConstantsDictionary[enumDictionary[i]].Name + ",";
                }
                this.CrossRingConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
                this.IsCrossRingLibraryLoaded = true;
            }
        }

        public void InitializeMiscellaneousMatterLibrary()
        {
            if (!IsMiscellaneousMatterLibraryLoaded)
            {
                Dictionary<string, Compound> compoundDictionary = MiscellaneousMatterLibrary.LoadMiscellaneousMatterData();
                this.MiscellaneousMatterConstantsDictionary = compoundDictionary;//accessable outside by getter below

                int count = 0;
                string names = "";
                Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
                foreach (KeyValuePair<string, Compound> item in compoundDictionary)
                {
                    names += item.Key + ",";
                    enumDictionary.Add(count, item.Key);
                    count++;
                }

                names = "";
                for (int i = 0; i < compoundDictionary.Count; i++)
                {
                    names += MiscellaneousMatterConstantsDictionary[enumDictionary[i]].Name + ",";
                }
                this.MiscellaneousMatterConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
                this.IsMiscellaneousMatterLibraryLoaded = true;
            }
        }
    }

}