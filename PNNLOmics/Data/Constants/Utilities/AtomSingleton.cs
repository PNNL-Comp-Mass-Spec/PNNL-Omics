using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This class loads the atom constants once and is accessble through the dictionary property.  Thread-safe singleton example created at first call
    /// </summary> 
    public sealed class AtomSingleton
    {
        //TODO:  this one is not needed
        /// <summary>
        /// Utilizes the get and set auto implemented properties.
        /// Note that set; can be any other operator as long as it's
        /// less accessible than public.
        /// </summary>
        public static AtomSingleton Instance { get; private set; }

        /// <summary>
        /// A static constructor is automatically initialized on referenceto the class.
        /// </summary>
        static AtomSingleton() { Instance = new AtomSingleton(); }

        //the part of the singleton that does the work once.
        AtomSingleton()
        {
            Dictionary<string, Atom> atomDictionary = AtomLibrary.LoadAtomicData();
            this.ConstantsDictionary = atomDictionary;//accessable outside by getter below

            int count = 0;
            string names = "";
            Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
            foreach (KeyValuePair<string, Atom> item in atomDictionary)
            {
                names += item.Key + ",";
                enumDictionary.Add(count, item.Key);
                count++;
            }
            names = "";
            for (int i = 0; i < atomDictionary.Count; i++)
            {
                names += ConstantsDictionary[enumDictionary[i]].Name + ",";
            }
            this.ConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
        }

        public Dictionary<string, Atom> ConstantsDictionary { get; set; }
        public Dictionary<int, string> ConstantsEnumDictionary { get; set; }
    }
}