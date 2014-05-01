using System.Collections.Generic;

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

namespace PNNLOmics.Data.Constants.Libraries
{
    /// <summary>
    /// This is a Class designed to convert tabulated data into a atom objects which are similar to physical constants.
    /// Electron, Neutron and Protons are created here and added to a Dictionarty with string keys such as "e" for electron
    /// </summary>
    public class SubAtomicParticleLibrary : MatterLibrary<SubAtomicParticle, SubAtomicParticleName>
    {
        public const double MASS_PROTON     = 1.00727646677;
        public const double MASS_NUETRON    = 1.00866491597;
        public const double MASS_ELECTRON   = 0.00054857990943;

        /// <summary>
        /// Loads the information from the const section into a sub atomics particle library
        /// </summary>
        public override void LoadLibrary()
        {
            m_symbolToCompoundMap = new Dictionary<string, SubAtomicParticle>();
            m_enumToSymbolMap = new Dictionary<SubAtomicParticleName, string>();

            var electron = new SubAtomicParticle();
            electron.Name = "Electron";
            electron.MassMonoIsotopic = MASS_ELECTRON;//units of u a.k.a.Da.  NIST CODATA 2006 
            electron.Symbol = "e";
            electron.ParticleType = SubAtomicParticleName.Electron;

            var neutron = new SubAtomicParticle();
            neutron.Name = "Neutron";
            neutron.MassMonoIsotopic = MASS_NUETRON;//units of u a.k.a.Da.  NIST CODATA 2006
            neutron.Symbol = "n";
            neutron.ParticleType = SubAtomicParticleName.Neutron;

            var proton = new SubAtomicParticle();
            proton.Name = "Proton";
            proton.MassMonoIsotopic = MASS_PROTON;//units of u a.k.a.Da.  NIST CODATA 2006
            proton.Symbol = "p";
            proton.ParticleType = SubAtomicParticleName.Proton;

            m_symbolToCompoundMap.Add(electron.Symbol, electron);
            m_symbolToCompoundMap.Add(neutron.Symbol, neutron);
            m_symbolToCompoundMap.Add(proton.Symbol, proton);

            m_enumToSymbolMap.Add(SubAtomicParticleName.Electron, electron.Symbol);
            m_enumToSymbolMap.Add(SubAtomicParticleName.Neutron, neutron.Symbol);
            m_enumToSymbolMap.Add(SubAtomicParticleName.Proton, proton.Symbol);
        }
    }
}
