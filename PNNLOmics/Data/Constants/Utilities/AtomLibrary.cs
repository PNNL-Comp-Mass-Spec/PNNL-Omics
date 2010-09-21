using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataLayer;

//http://physics.nist.gov/cuu/Constants/index.html  NIST CODATA 2006  

//dictinoary implementation
//Dictionary<string,AtomObject> AtomDictionary = AtomLibrary.LoadAtomicData();
//double AtomMass = AtomDictionary["e"].MonoIsotopicMass;
//string AtomName = AtomDictionary["e"].Name;
//string AtomSymbol = AtomDictionary["e"].Symbol;

//one line implementation
//double AtomMass2 = AtomConstantsStaticLibrary.GetMonoisotopicMass("e");
//string AtomName2 = AtomConstantsStaticLibrary.GetName("e");
//string AtomSymbol2 = AtomConstantsStaticLibrary.GetSymbol("e");

//double atomMass3 = AtomStaticLibrary.GetMonoisotopicMass(SelectAtom.Proton);


namespace PNNLOmics.Data.Constants.ConstantsDataUtilities
{
    /// <summary>
    /// This is a Class designed to convert tabulated data into a atom objects which are similar to physical constants.
    /// Electron, Neutron and Protons are created here and added to a Dictionarty with string keys such as "e" for electron
    /// </summary>
    public class AtomLibrary
    {
        public static Dictionary<string, Atom> LoadAtomicData()
        {
            Dictionary<string, Atom> atomicDictionary = new Dictionary<string, Atom>();

            Atom electron = new Atom();
            electron.Name = "Electron";
            electron.MassMonoIsotopic = 0.00054857990943;//units of u a.k.a.Da.  NIST CODATA 2006 
            electron.Symbol = "e";

            Atom neutron = new Atom();
            neutron.Name = "Neutron";
            neutron.MassMonoIsotopic = 1.00866491597;//units of u a.k.a.Da.  NIST CODATA 2006
            neutron.Symbol = "n";

            Atom proton = new Atom();
            proton.Name = "Proton";
            proton.MassMonoIsotopic = 1.00727646677;//units of u a.k.a.Da.  NIST CODATA 2006
            proton.Symbol = "p";

            atomicDictionary.Add(electron.Symbol, electron);
            atomicDictionary.Add(neutron.Symbol, neutron);
            atomicDictionary.Add(proton.Symbol, proton);

            return atomicDictionary;
        }
    }

    /// <summary>
    /// This is a Class designed to convert dictionary calls for Atoms in one line static method calls.
    /// </summary>
    public class AtomStaticLibrary
    {
        public static double GetMonoisotopicMass(string constantKey)
        {
            AtomSingleton NewSingleton = AtomSingleton.Instance;
            Dictionary<string, Atom> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].MassMonoIsotopic;
        }

        public static string GetSymbol(string constantKey)
        {
            //TODO: newSingleton
            //TODO: incoming
            AtomSingleton NewSingleton = AtomSingleton.Instance;
            Dictionary<string, Atom> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].Symbol;
        }

        public static string GetName(string constantKey)
        {
            AtomSingleton NewSingleton = AtomSingleton.Instance;
            Dictionary<string, Atom> incommingDictionary = NewSingleton.ConstantsDictionary;
            return incommingDictionary[constantKey].Name;
        }

        //overload to allow for SelectElement
        public static double GetMonoisotopicMass(SelectAtom selectKey)
        {
            AtomSingleton NewSingleton = AtomSingleton.Instance;
            Dictionary<string, Atom> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].MassMonoIsotopic;
        }

        public static string GetSymbol(SelectAtom selectKey)
        {
            AtomSingleton NewSingleton = AtomSingleton.Instance;
            Dictionary<string, Atom> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].Symbol;
        }

        public static string GetName(SelectAtom selectKey)
        {
            AtomSingleton NewSingleton = AtomSingleton.Instance;
            Dictionary<string, Atom> incommingDictionary = NewSingleton.ConstantsDictionary;
            Dictionary<int, string> enumConverter = NewSingleton.ConstantsEnumDictionary;
            string constantKey = enumConverter[(int)selectKey];
            return incommingDictionary[constantKey].Name;
        }
    }

    public enum SelectAtom
    {
        Electron, Neutron, Proton
    }
}
