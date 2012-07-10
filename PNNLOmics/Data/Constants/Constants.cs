using System;
using System.Collections.Generic;
using PNNLOmics.Data.Constants.Libraries;

namespace PNNLOmics.Data.Constants
{
    /// <summary>
    /// This class contains the constants libraries
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Amino acid library instance.
        /// </summary>
        private static AminoAcidLibrary m_aminoAcidLibrary;
        /// <summary>
        /// Cross ring library instance.
        /// </summary>
        private static CrossRingLibrary m_crossRingLibrary;
        /// <summary>
        /// Element library instance.
        /// </summary>
        private static ElementLibrary m_elementLibrary;
        /// <summary>
        /// Miscellaneous matter library instance.
        /// </summary>
        private static MiscellaneousMatterLibrary m_miscellaneousMatterLibrary;
        /// <summary>
        /// Monosaccharide library instance.
        /// </summary>
        private static MonosaccharideLibrary m_monosaccharideLibrary;
        /// <summary>
        /// Sub atomics particle library instance.
        /// </summary>
        private static SubAtomicParticleLibrary m_subAtomicParticleLibrary;

        /// <summary>
        /// user library instance.
        /// </summary>
        private static UserUnitLibrary m_userUnitLibrary;


        #region Library Properties
        /// <summary>
        /// Gets the amino acid library.
        /// </summary>
        public static AminoAcidLibrary AminoAcids
        {
            get
            {
                if (m_aminoAcidLibrary == null)
                {
                    m_aminoAcidLibrary = new AminoAcidLibrary();
                    m_aminoAcidLibrary.LoadLibrary();
                }
                return m_aminoAcidLibrary;
            }
        }

        /// <summary>
        /// Gets the monosaccharide cross ring cleavage library.
        /// </summary>
        public static CrossRingLibrary CrossRings
        {
            get
            {
                if (m_crossRingLibrary == null)
                {
                    m_crossRingLibrary = new CrossRingLibrary();
                    m_crossRingLibrary.LoadLibrary();
                }
                return m_crossRingLibrary;
            }
        }

        /// <summary>
        /// Gets the amino acid library.
        /// </summary>
        public static ElementLibrary Elements
        {
            get
            {
                if (m_elementLibrary == null)
                {
                    m_elementLibrary = new ElementLibrary();
                    m_elementLibrary.LoadLibrary();
                }
                return m_elementLibrary;
            }
        }

        /// <summary>
        /// Gets the miscellaneous matter library.
        /// </summary>
        public static MiscellaneousMatterLibrary MiscellaneousMatter
        {
            get
            {
                if (m_miscellaneousMatterLibrary == null)
                {
                    m_miscellaneousMatterLibrary = new MiscellaneousMatterLibrary();
                    m_miscellaneousMatterLibrary.LoadLibrary();
                }
                return m_miscellaneousMatterLibrary;
            }
        }

        /// <summary>
        /// Gets the monosaccharide library.
        /// </summary>
        public static MonosaccharideLibrary Monosaccharides
        {
            get
            {
                if (m_monosaccharideLibrary == null)
                {
                    m_monosaccharideLibrary = new MonosaccharideLibrary();
                    m_monosaccharideLibrary.LoadLibrary();
                }
                return m_monosaccharideLibrary;
            }
        }

        /// <summary>
        /// Gets the sub atomic particle library.
        /// </summary>
        public static SubAtomicParticleLibrary SubAtomicParticles
        {
            get
            {
                if (m_subAtomicParticleLibrary == null)
                {
                    m_subAtomicParticleLibrary = new SubAtomicParticleLibrary();
                    m_subAtomicParticleLibrary.LoadLibrary();
                }
                return m_subAtomicParticleLibrary;
            }
        }

        /// <summary>
        /// Gets and sets the user unit library.
        /// </summary>
        public static UserUnitLibrary UserUnits
        {
            get
            {
                if (m_userUnitLibrary == null)
                {
                    m_userUnitLibrary = new UserUnitLibrary();
                    m_userUnitLibrary.LoadLibrary();
                }
                return m_userUnitLibrary;
            }
            set
            {
       
            }
        }

        public static void SetUserUnitLibrary(UserUnitLibrary libraryIn)
        {
            m_userUnitLibrary = libraryIn;
        }
        #endregion
    }
}