using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This class loads the cross ring fragments constants once and is accessble through the dictionary property.  Thread-safe singleton example created at first call
    /// </summary> 
    public sealed class CrossRingSingleton
    {
        /// <summary>
        /// Utilizes the get and set auto implemented properties.
        /// Note that set; can be any other operator as long as it's
        /// less accessible than public.
        /// </summary>
        public static CrossRingSingleton Instance { get; private set; }

        /// <summary>
        /// A static constructor is automatically initialized on referenceto the class.
        /// </summary>
        static CrossRingSingleton() { Instance = new CrossRingSingleton(); }

        //the part of the singleton that does the work once.
        CrossRingSingleton()
        {
            Dictionary<string, CrossRing> crossRingDictionary = CrossRingLibrary.LoadCrossRingData();
            this.ConstantsDictionary = crossRingDictionary;//accessable outside by getter below
        }

        public Dictionary<string, CrossRing> ConstantsDictionary { get; set; }
    }
}