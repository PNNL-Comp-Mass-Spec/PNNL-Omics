using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//http://physics.nist.gov/cuu/Constants/index.html  NIST CODATA 2006  

//dictinoary implementation
//Dictionary<string,AtomObject> AtomDictionary = AtomLibrary.LoadAtomicData();
//double AtomMass = AtomDictionary["e"].MonoIsotopicMass;
//string AtomName = AtomDictionary["e"].Name;
//string AtomSymbol = AtomDictionary["e"].Symbol;
                       
namespace Constants
{
    public class AtomLibrary
    {
        public static Dictionary<string, AtomObject> LoadAtomicData()
        {
            Dictionary<string, AtomObject> AtomiceDictionary = new Dictionary<string, AtomObject>();

            AtomObject Electron = new AtomObject();
            Electron.Name = "Electron";
            Electron.MonoIsotopicMass = 0.00054857990943;//units of u a.k.a.Da.  NIST CODATA 2006 
            Electron.Symbol = "e";

            AtomObject Neutron = new AtomObject();
            Neutron.Name = "Neutron";
            Neutron.MonoIsotopicMass = 1.00866491597;//units of u a.k.a.Da.  NIST CODATA 2006
            Neutron.Symbol = "n";

            AtomObject Proton = new AtomObject();
            Proton.Name = "Proton";
            Proton.MonoIsotopicMass = 1.00727646677;//units of u a.k.a.Da.  NIST CODATA 2006
            Proton.Symbol = "p";

            AtomiceDictionary.Add(Electron.Symbol, Electron);
            AtomiceDictionary.Add(Neutron.Symbol, Neutron);
            AtomiceDictionary.Add(Proton.Symbol, Proton);

            return AtomiceDictionary;
        }

    }
}
