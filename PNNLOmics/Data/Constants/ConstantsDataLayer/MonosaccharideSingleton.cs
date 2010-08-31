using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This class loads the monosaccharide constants once and is accessble through the dictionary property.  Thread-safe singleton example created at first call
    /// </summary> 
    public sealed class MonosaccharideSingleton
    {
        /// <summary>
        /// Utilizes the get and set auto implemented properties.
        /// Note that set; can be any other operator as long as it's
        /// less accessible than public.
        /// </summary>
        public static MonosaccharideSingleton Instance { get; private set; }

        /// <summary>
        /// A static constructor is automatically initialized on referenceto the class.
        /// </summary>
        static MonosaccharideSingleton() { Instance = new MonosaccharideSingleton(); }

        //the part of the singleton that does the work once.
        MonosaccharideSingleton()
        {
            Dictionary<string, Monosaccharide> monosaccharideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
            this.ConstantsDictionary = monosaccharideDictionary;//accessable outside by getter below
        }

        public Dictionary<string, Monosaccharide> ConstantsDictionary { get; set; }
    }
}