using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    ////TODO:  XML comments on properties
    
    ///// <summary>
    ///// This class loads the monosaccharide constants once and is accessble through the dictionary property.  Thread-safe singleton example created at first call
    ///// </summary> 
    //public sealed class CompoundSingleton
    //{
    //    /// <summary>
    //    /// creates a single instance upon creation
    //    /// </summary>
    //    public static CompoundSingleton Instance { get; private set; }

    //    /// <summary>
    //    /// A static constructor is automatically initialized on referenceto the class and sets up a blank set of dictionaries.
    //    /// </summary>
    //    static CompoundSingleton()
    //    {
    //        Instance = new CompoundSingleton();
    //        Instance.IsMonosaccharideLibraryLoaded = false;
    //        Instance.IsAminoAcidLibraryLoaded = false;
    //        Instance.IsCrossRingLibraryLoaded = false;
    //        Instance.IsMiscellaneousMatterLibraryLoaded = false;
    //    }

    //    /// <summary>
    //    /// Have we loaded the Monosaccharide Library yet?
    //    /// </summary>
    //    private bool IsMonosaccharideLibraryLoaded { get; set; }

    //    /// <summary>
    //    /// Have we loaded the AminoAcid Library yet?
    //    /// </summary>
    //    private bool IsAminoAcidLibraryLoaded { get; set; }

    //    /// <summary>
    //    /// Have we loaded the CrossRing Library yet?
    //    /// </summary>
    //    private bool IsCrossRingLibraryLoaded { get; set; }

    //    /// <summary>
    //    /// Have we loaded the MiscellaneousMatter Library yet?
    //    /// </summary>
    //    private bool IsMiscellaneousMatterLibraryLoaded { get; set; }

    //    //the part of the singleton that does the work once.
    //    CompoundSingleton()
    //    {
    //    }

    //    public Dictionary<string, Compound> MonosaccharideConstantsDictionary { get; set; }
    //    public Dictionary<int, string> MonosaccharideConstantsEnumDictionary { get; set; }

    //    public Dictionary<string, Compound> AminoAcidConstantsDictionary { get; set; }
    //    public Dictionary<int, string> AminoAcidConstantsEnumDictionary { get; set; }

    //    public Dictionary<string, Compound> CrossRingConstantsDictionary { get; set; }
    //    public Dictionary<int, string> CrossRingConstantsEnumDictionary { get; set; }

    //    public Dictionary<string, Compound> MiscellaneousMatterConstantsDictionary { get; set; }
    //    public Dictionary<int, string> MiscellaneousMatterConstantsEnumDictionary { get; set; }

    //    /// <summary>
    //    /// This method loads the MonosaccharideLibrary if it has not been loaded already.
    //    /// </summary>
    //    public void InitializeMonosaccharideLibrary()
    //    {
    //        if (!IsMonosaccharideLibraryLoaded)
    //        {
    //            Dictionary<string, Compound> compoundDictionary = MonosaccharideLibrary.LoadMonosaccharideData();
    //            this.MonosaccharideConstantsDictionary = compoundDictionary;//accessable outside by getter below

    //            int count = 0;
    //            Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
    //            foreach (KeyValuePair<string, Compound> item in compoundDictionary)
    //            {
    //                enumDictionary.Add(count, item.Key);
    //                count++;
    //            }

    //            this.MonosaccharideConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
    //            this.IsMonosaccharideLibraryLoaded = true;
    //        }
    //    }

    //    /// <summary>
    //    /// This method loads the AminoAcid Library if it has not been loaded already.
    //    /// </summary>
    //    public void InitializeAminoAcidLibrary()
    //    {
    //        if (!IsAminoAcidLibraryLoaded)
    //        {
    //            Dictionary<string, Compound> compoundDictionary = AminoAcidLibrary.LoadAminoAcidData();
    //            this.AminoAcidConstantsDictionary = compoundDictionary;//accessable outside by getter below

    //            int count = 0;
    //            Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
    //            foreach (KeyValuePair<string, Compound> item in compoundDictionary)
    //            {
    //                enumDictionary.Add(count, item.Key);
    //                count++;
    //            }

    //            this.AminoAcidConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
    //            this.IsAminoAcidLibraryLoaded = true;
    //        }
    //    }

    //    /// <summary>
    //    /// This method loads the CrossRing Library if it has not been loaded already.
    //    /// </summary>
    //    public void InitializeCrossRingLibrary()
    //    {
    //        if (!IsCrossRingLibraryLoaded)
    //        {
    //            Dictionary<string, Compound> compoundDictionary = CrossRingLibrary.LoadCrossRingData();
    //            this.CrossRingConstantsDictionary = compoundDictionary;//accessable outside by getter below

    //            int count = 0;
    //            Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
    //            foreach (KeyValuePair<string, Compound> item in compoundDictionary)
    //            {
    //                enumDictionary.Add(count, item.Key);
    //                count++;
    //            }

    //            this.CrossRingConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
    //            this.IsCrossRingLibraryLoaded = true;
    //        }
    //    }

    //    /// <summary>
    //    /// This method loads the MiscellaneousMatter Library if it has not been loaded already.
    //    /// </summary>
    //    public void InitializeMiscellaneousMatterLibrary()
    //    {
    //        if (!IsMiscellaneousMatterLibraryLoaded)
    //        {
    //            Dictionary<string, Compound> compoundDictionary = MiscellaneousMatterLibrary.LoadMiscellaneousMatterData();
    //            this.MiscellaneousMatterConstantsDictionary = compoundDictionary;//accessable outside by getter below

    //            int count = 0;
    //            Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
    //            foreach (KeyValuePair<string, Compound> item in compoundDictionary)
    //            {
    //                enumDictionary.Add(count, item.Key);
    //                count++;
    //            }

    //            this.MiscellaneousMatterConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
    //            this.IsMiscellaneousMatterLibraryLoaded = true;
    //        }
    //    }
    //}

}