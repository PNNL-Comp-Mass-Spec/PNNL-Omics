using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

/// <example>
/// http://physics.nist.gov/cuu/Constants/index.html  NIST CODATA 2006  
/// 
/// dictinoary implementation
/// Dictionary<string,AtomObject> AtomDictionary = AtomLibrary.LoadAtomicData();
/// double AtomMass = AtomDictionary["e"].MonoIsotopicMass;
/// string AtomName = AtomDictionary["e"].Name;
/// string AtomSymbol = AtomDictionary["e"].Symbol;

/// one line implementation
/// double AtomMass2 = AtomConstantsStaticLibrary.GetMonoisotopicMass("e");
/// string AtomName2 = AtomConstantsStaticLibrary.GetName("e");
/// string AtomSymbol2 = AtomConstantsStaticLibrary.GetSymbol("e");

/// double atomMass3 = AtomStaticLibrary.GetMonoisotopicMass(SelectAtom.Proton);
/// </example>

namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    /// <summary>
    /// This is a Class designed to convert tabulated data into a atom objects which are similar to physical constants.
    /// Electron, Neutron and Protons are created here and added to a Dictionarty with string keys such as "e" for electron
    /// </summary>
    public class SubAtomicParticleLibrary
    {
        public static Dictionary<string, SubAtomicParticle> LoadSubAtomicParticleData()
        {
            Dictionary<string, SubAtomicParticle> atomicDictionary = new Dictionary<string, SubAtomicParticle>();

            SubAtomicParticle electron = new SubAtomicParticle();
            electron.Name = "Electron";
            electron.MassMonoIsotopic = 0.00054857990943;//units of u a.k.a.Da.  NIST CODATA 2006 
            electron.Symbol = "e";

            SubAtomicParticle neutron = new SubAtomicParticle();
            neutron.Name = "Neutron";
            neutron.MassMonoIsotopic = 1.00866491597;//units of u a.k.a.Da.  NIST CODATA 2006
            neutron.Symbol = "n";

            SubAtomicParticle proton = new SubAtomicParticle();
            proton.Name = "Proton";
            proton.MassMonoIsotopic = 1.00727646677;//units of u a.k.a.Da.  NIST CODATA 2006
            proton.Symbol = "p";

            atomicDictionary.Add(electron.Symbol, electron);
            atomicDictionary.Add(neutron.Symbol, neutron);
            atomicDictionary.Add(proton.Symbol, proton);

            return atomicDictionary;
        }
    }

    #region old static library code
    ///// <summary>
    ///// This is a Class designed to convert dictionary calls for Atoms in one line static method calls.
    ///// </summary>
    //public class AtomStaticLibrary
    //{
    //    public static double GetMonoisotopicMass(string constantKey)
    //    {
    //        AtomSingleton NewSingleton = AtomSingleton.Instance;
    //        Dictionary<string, SubAtomicParticle> incommingDictionary = NewSingleton.ConstantsDictionary;
    //        return incommingDictionary[constantKey].MassMonoIsotopic;
    //    }

    //    public static string GetSymbol(string constantKey)
    //    {
    //        AtomSingleton NewSingleton = AtomSingleton.Instance;
    //        Dictionary<string, SubAtomicParticle> incommingDictionary = NewSingleton.ConstantsDictionary;
    //        return incommingDictionary[constantKey].Symbol;
    //    }

    //    public static string GetName(string constantKey)
    //    {
    //        AtomSingleton NewSingleton = AtomSingleton.Instance;
    //        Dictionary<string, SubAtomicParticle> incommingDictionary = NewSingleton.ConstantsDictionary;
    //        return incommingDictionary[constantKey].Name;
    //    }

    //    //overload to allow for SelectElement
    //    public static double GetMonoisotopicMass(SelectSubAtomicParticle selectKey)
    //    {
    //        AtomSingleton NewSingleton = AtomSingleton.Instance;
    //        Dictionary<string, SubAtomicParticle> incommingDictionary = NewSingleton.ConstantsDictionary;
    //        Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
    //        string constantKey = enumConverter[(int)selectKey];
    //        return incommingDictionary[constantKey].MassMonoIsotopic;
    //    }

    //    public static string GetSymbol(SelectSubAtomicParticle selectKey)
    //    {
    //        AtomSingleton NewSingleton = AtomSingleton.Instance;
    //        Dictionary<string, SubAtomicParticle> incommingDictionary = NewSingleton.ConstantsDictionary;
    //        Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
    //        string constantKey = enumConverter[(int)selectKey];
    //        return incommingDictionary[constantKey].Symbol;
    //    }

    //    public static string GetName(SelectSubAtomicParticle selectKey)
    //    {
    //        AtomSingleton NewSingleton = AtomSingleton.Instance;
    //        Dictionary<string, SubAtomicParticle> incommingDictionary = NewSingleton.ConstantsDictionary;
    //        Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
    //        string constantKey = enumConverter[(int)selectKey];
    //        return incommingDictionary[constantKey].Name;
    //    }
    //}
    #endregion

    public enum SelectSubAtomicParticle
    {
        Electron, Neutron, Proton
    }
}
