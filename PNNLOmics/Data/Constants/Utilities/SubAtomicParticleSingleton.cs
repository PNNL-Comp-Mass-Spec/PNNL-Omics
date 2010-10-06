using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    ///// <summary>
    ///// This class loads the atom constants once and is accessble through the dictionary property.  Thread-safe singleton example created at first call
    ///// </summary> 
    //public sealed class SubatomicParticleSingleton
    //{
    //    public static SubatomicParticleSingleton Instance { get; private set; }

    //    /// <summary>
    //    /// A static constructor is automatically initialized on referenceto the class.
    //    /// </summary>
    //    static SubatomicParticleSingleton() { Instance = new SubatomicParticleSingleton(); }

    //    //the part of the singleton that does the work once.
    //    SubatomicParticleSingleton()
    //    {
    //        Dictionary<string, SubatomicParticle> atomDictionary = SubatomicParticleLibrary.LoadAtomicData();
    //        this.ConstantsDictionary = atomDictionary;//accessable outside by getter below

    //        int count = 0;
    //        string names = "";
    //        Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
    //        foreach (KeyValuePair<string, SubatomicParticle> item in atomDictionary)
    //        {
    //            names += item.Key + ",";
    //            enumDictionary.Add(count, item.Key);
    //            count++;
    //        }
    //        names = "";
    //        for (int i = 0; i < atomDictionary.Count; i++)
    //        {
    //            names += ConstantsDictionary[enumDictionary[i]].Name + ",";
    //        }
    //        this.ConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
    //    }

    //    public Dictionary<string, SubatomicParticle> ConstantsDictionary { get; set; }
    //    public Dictionary<int, string> ConstantsEnumDictionary { get; set; }
    //}
}