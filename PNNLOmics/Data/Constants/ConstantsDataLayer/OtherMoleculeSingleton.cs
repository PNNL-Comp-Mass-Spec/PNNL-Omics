using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This class loads the other molecule constants once and is accessble through the dictionary property.  Thread-safe singleton example created at first call
    /// </summary> 
    public sealed class OtherMoleculeSingleton
    {
        /// <summary>
        /// Utilizes the get and set auto implemented properties.
        /// Note that set; can be any other operator as long as it's
        /// less accessible than public.
        /// </summary>
        public static OtherMoleculeSingleton Instance { get; private set; }

        /// <summary>
        /// A static constructor is automatically initialized on referenceto the class.
        /// </summary>
        static OtherMoleculeSingleton() { Instance = new OtherMoleculeSingleton(); }

        //the part of the singleton that does the work once.
        OtherMoleculeSingleton()
        {
            Dictionary<string, OtherMolecule> otherMoleculeDictionary = OtherMoleculeLibrary.LoadOtherMoleculeData();
            this.ConstantsDictionary = otherMoleculeDictionary;//accessable outside by getter below
        }

        public Dictionary<string, OtherMolecule> ConstantsDictionary { get; set; }
    }
}