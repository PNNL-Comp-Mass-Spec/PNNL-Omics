using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    ///// <summary>
    ///// This class loads the monosaccharide constants once and is accessble through the dictionary property.  Thread-safe singleton example created at first call
    ///// </summary> 
    //public sealed class MonosaccharideSingleton
    //{
    //    /// <summary>
    //    /// creates a single instance upon creation
    //    /// </summary>
    //    public static MonosaccharideSingleton Instance { get; private set; }

    //    /// <summary>
    //    /// A static constructor is automatically initialized on referenceto the class.
    //    /// </summary>
    //    static MonosaccharideSingleton()
    //    {
    //        Instance = new MonosaccharideSingleton();
    //    }

    //    //the part of the singleton that does the work once.
    //    MonosaccharideSingleton()
    //    {
    //        Dictionary<string, Monosaccharide> monosaccharideDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
    //        this.ConstantsDictionary = monosaccharideDictionary;//accessable outside by getter below

    //        int count = 0;
    //        string names = "";
    //        Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
    //        foreach (KeyValuePair<string, Monosaccharide> item in monosaccharideDictionary)
    //        {
    //            names += item.Key + ",";
    //            enumDictionary.Add(count, item.Key);
    //            count++;
    //        }
    //        names = "";
    //        for (int i = 0; i < monosaccharideDictionary.Count; i++)
    //        {
    //            names += ConstantsDictionary[enumDictionary[i]].Name + ",";
    //        }
    //        this.ConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
    //    }

    //    public Dictionary<string, Monosaccharide> ConstantsDictionary { get; set; }
    //    public Dictionary<int, string> ConstantsEnumDictionary { get; set; }
    //}
}