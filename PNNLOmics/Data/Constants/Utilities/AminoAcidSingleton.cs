using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This class loads the amino acide constants once and is accessble through the dictionary property.  Thread-safe singleton example created at first call
    /// </summary> 
    public sealed class AminoAcidSingleton
    {
        /// <summary>
        /// Utilizes the get and set auto implemented properties.
        /// Note that set; can be any other operator as long as it's
        /// less accessible than public.
        /// </summary>
        public static AminoAcidSingleton Instance { get; private set; }

        /// <summary>
        /// A static constructor is automatically initialized on referenceto the class.
        /// </summary>
        static AminoAcidSingleton() { Instance = new AminoAcidSingleton(); }

        //the part of the singleton that does the work once.
        AminoAcidSingleton()
        {
            Dictionary<char, AminoAcid> aminoAcidDictionary = AminoAcidLibrary.LoadAminoAcidData();
            this.ConstantsDictionary = aminoAcidDictionary;//accessable outside by getter below

            int count = 0;
            string names = "";
            Dictionary<int, char> enumDictionary = new Dictionary<int, char>();
            foreach (KeyValuePair<char, AminoAcid> item in aminoAcidDictionary)
            {
                names += item.Key + ",";
                enumDictionary.Add(count, item.Key);
                count++;
            }
            names = "";
            for (int i = 0; i < aminoAcidDictionary.Count; i++)
            {
                names += ConstantsDictionary[enumDictionary[i]].Name + ",";
            }
            this.ConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
        }

        /// <summary>
        /// get/set a dictionary of <char,AminoAcid> where char is a single letter code
        /// </summary>
        public Dictionary<char, AminoAcid> ConstantsDictionary { get; set; }

        /// <summary>
        /// get/set a dictionary of <int,char> where char is a single letter code.  This dictionary can be iterated through to return char keys.
        /// </summary>
        public Dictionary<int, char> ConstantsEnumDictionary { get; set; }
    }
}