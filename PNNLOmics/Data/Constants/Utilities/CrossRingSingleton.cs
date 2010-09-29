using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    ///// <summary>
    ///// This class loads the cross ring fragments constants once and is accessble through the dictionary property.  Thread-safe singleton example created at first call
    ///// </summary> 
    //public sealed class CrossRingSingleton
    //{
    //    public static CrossRingSingleton Instance { get; private set; }

    //    /// <summary>
    //    /// A static constructor is automatically initialized on referenceto the class.
    //    /// </summary>
    //    static CrossRingSingleton() { Instance = new CrossRingSingleton(); }

    //    //the part of the singleton that does the work once.
    //    CrossRingSingleton()
    //    {
    //        Dictionary<string, CrossRing> crossRingDictionary = CrossRingLibrary.LoadCrossRingData();
    //        this.ConstantsDictionary = crossRingDictionary;//accessable outside by getter below

    //        int count = 0;
    //        string names = "";
    //        Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
    //        foreach (KeyValuePair<string, CrossRing> item in crossRingDictionary)
    //        {
    //            names += item.Key + ",";
    //            enumDictionary.Add(count, item.Key);
    //            count++;
    //        }
    //        names = "";
    //        for (int i = 0; i < crossRingDictionary.Count; i++)
    //        {
    //            names += ConstantsDictionary[enumDictionary[i]].Name + ",";
    //        }
    //        this.ConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
    //    }

    //    //TODO:  XML comments on properties
    //    //TODO:  add interface.  Include anyting that is required from a library.  one interface for a ImatterLirary.  each library extends the interface. 
    //    public Dictionary<string, CrossRing> ConstantsDictionary { get; set; }
    //    public Dictionary<int, string> ConstantsEnumDictionary { get; set; }
    //}
}