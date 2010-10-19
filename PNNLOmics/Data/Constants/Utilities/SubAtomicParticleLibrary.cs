using System;
using System.Collections.Generic;
using PNNLOmics.Data.Constants.Enumerations;

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

namespace PNNLOmics.Data.Constants.Utilities
{
    /// <summary>
    /// This is a Class designed to convert tabulated data into a atom objects which are similar to physical constants.
    /// Electron, Neutron and Protons are created here and added to a Dictionarty with string keys such as "e" for electron
    /// </summary>
    public class SubAtomicParticleLibrary : MatterLibrary<SubAtomicParticle, SubAtomicParticleName>
    {
        //TODO: SCOTT - CR - Add XML comments.
        public override Dictionary<string, SubAtomicParticle> LoadLibrary()
        {
            m_symbolToCompoundMap = new Dictionary<string, SubAtomicParticle>();
            m_enumToSymbolMap = new Dictionary<SubAtomicParticleName, string>();

            //TODO: SCOTT - CR - Add particle types to each electron, proton and neutron.
            SubAtomicParticle electron = new SubAtomicParticle();
            electron.Name = "Electron";
            electron.MassMonoIsotopic = 0.00054857990943;//units of u a.k.a.Da.  NIST CODATA 2006 
            electron.Symbol = "e";

            SubAtomicParticle neutron = new SubAtomicParticle();
            neutron.Name = "Neutron";
            neutron.MassMonoIsotopic = 1.00866491597;//units of u a.k.a.Da.  NIST CODATA 2006
            neutron.Symbol = "n";
            neutron.ParticleType = SubAtomicParticleName.Neutron;

            SubAtomicParticle proton = new SubAtomicParticle();
            proton.Name = "Proton";
            proton.MassMonoIsotopic = 1.00727646677;//units of u a.k.a.Da.  NIST CODATA 2006
            proton.Symbol = "p";

            m_symbolToCompoundMap.Add(electron.Symbol, electron);
            m_symbolToCompoundMap.Add(neutron.Symbol, neutron);
            m_symbolToCompoundMap.Add(proton.Symbol, proton);

            m_enumToSymbolMap.Add(SubAtomicParticleName.Electron, electron.Symbol);
            m_enumToSymbolMap.Add(SubAtomicParticleName.Neutron, neutron.Symbol);
            m_enumToSymbolMap.Add(SubAtomicParticleName.Proton, proton.Symbol);

            return m_symbolToCompoundMap;
        }
    }
}
