using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Constants.ConstantsDataLayer;

//http://physics.nist.gov/cuu/Constants/index.html  NIST CODATA 2006  

//dictinoary implementation
//Dictionary<string,AtomObject> AtomDictionary = AtomLibrary.LoadAtomicData();
//double AtomMass = AtomDictionary["e"].MonoIsotopicMass;
//string AtomName = AtomDictionary["e"].Name;
//string AtomSymbol = AtomDictionary["e"].Symbol;

namespace PNNLOmics.Constants.ConstantsDataUtilities
{
    public class AtomLibrary
    {
        public static Dictionary<string, Atom> LoadAtomicData()
        {
            Dictionary<string, Atom> atomicDictionary = new Dictionary<string, Atom>();

            Atom electron = new Atom();
            electron.Name = "Electron";
            electron.MonoIsotopicMass = 0.00054857990943;//units of u a.k.a.Da.  NIST CODATA 2006 
            electron.Symbol = "e";

            Atom neutron = new Atom();
            neutron.Name = "Neutron";
            neutron.MonoIsotopicMass = 1.00866491597;//units of u a.k.a.Da.  NIST CODATA 2006
            neutron.Symbol = "n";

            Atom proton = new Atom();
            proton.Name = "Proton";
            proton.MonoIsotopicMass = 1.00727646677;//units of u a.k.a.Da.  NIST CODATA 2006
            proton.Symbol = "p";

            atomicDictionary.Add(electron.Symbol, electron);
            atomicDictionary.Add(neutron.Symbol, neutron);
            atomicDictionary.Add(proton.Symbol, proton);

            return atomicDictionary;
        }

    }
}
