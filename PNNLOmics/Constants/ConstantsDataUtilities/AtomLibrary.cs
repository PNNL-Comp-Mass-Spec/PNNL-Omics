using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Constants.ConstantsObjectsDataLayer;

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
        public static Dictionary<string, AtomObject> LoadAtomicData()
        {
            Dictionary<string, AtomObject> atomicDictionary = new Dictionary<string, AtomObject>();

            AtomObject electron = new AtomObject();
            electron.Name = "Electron";
            electron.MonoIsotopicMass = 0.00054857990943;//units of u a.k.a.Da.  NIST CODATA 2006 
            electron.Symbol = "e";

            AtomObject neutron = new AtomObject();
            neutron.Name = "Neutron";
            neutron.MonoIsotopicMass = 1.00866491597;//units of u a.k.a.Da.  NIST CODATA 2006
            neutron.Symbol = "n";

            AtomObject proton = new AtomObject();
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
