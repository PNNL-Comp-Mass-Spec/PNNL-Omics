using System;
using System.Collections.Generic;
using PNNLOmics.Data.Constants.Utilities;

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
		private static ElementLibrary   m_elementLibrary;
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
		#endregion

        #region properties

        //private static LibraryCompound m_MonosaccharideLibrary { get; set; }
        //private static LibraryCompound m_CrossRingLibrary { get; set; }
        //private static LibraryCompound m_MiscellaneousMatterLibrary { get; set; }
        //private static LibrarySubAtomicParticle m_SubAtomicParticleLibrary { get; set; }
        //private static LibraryElement m_ElementLibrary { get; set; }

		

        //private static LibraryCompound LibraryMonosaccharide //m_MonosaccharideLibrary
        //{
        //    get
        //    {
        //        if (m_MonosaccharideLibrary == null)
        //        {
        //            Dictionary<string, Compound> compoundDictionary = MonosaccharideLibrary.LoadLibrary();

        //            int count = 0;
        //            Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
        //            foreach (KeyValuePair<string, Compound> item in compoundDictionary)
        //            {
        //                enumDictionary.Add(count, item.Key);
        //                count++;
        //            }

        //            LibraryCompound compoundLibrary = new LibraryCompound();
        //            compoundLibrary.ConstantsDictionary = compoundDictionary;
        //            compoundLibrary.ConstantsEnumDictionary = enumDictionary;
        //            m_MonosaccharideLibrary = compoundLibrary;
        //        }
        //        return m_MonosaccharideLibrary;
        //    }
        //}

        //private static LibraryCompound LibraryCrossRing //m_CrossRingLibrary
        //{
        //    get
        //    {
        //        if (m_CrossRingLibrary == null)
        //        {
        //            Dictionary<string, Compound> compoundDictionary = CrossRingLibrary.LoadCrossRingData();

        //            int count = 0;
        //            Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
        //            foreach (KeyValuePair<string, Compound> item in compoundDictionary)
        //            {
        //                enumDictionary.Add(count, item.Key);
        //                count++;
        //            }

        //            LibraryCompound compoundLibrary = new LibraryCompound();
        //            compoundLibrary.ConstantsDictionary = compoundDictionary;
        //            compoundLibrary.ConstantsEnumDictionary = enumDictionary;
        //            m_CrossRingLibrary = compoundLibrary;
        //        }
        //        return m_CrossRingLibrary;
        //    }
        //}

        //private static LibraryCompound LibraryMiscellaneousMatter //m_MiscellaneousMatterLibrary
        //{
        //    get
        //    {
        //        if (m_MiscellaneousMatterLibrary == null)
        //        {
        //            Dictionary<string, Compound> compoundDictionary = MiscellaneousMatterLibrary.LoadLibrary();

        //            int count = 0;
        //            Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
        //            foreach (KeyValuePair<string, Compound> item in compoundDictionary)
        //            {
        //                enumDictionary.Add(count, item.Key);
        //                count++;
        //            }

        //            LibraryCompound compoundLibrary = new LibraryCompound();
        //            compoundLibrary.ConstantsDictionary = compoundDictionary;
        //            compoundLibrary.ConstantsEnumDictionary = enumDictionary;
        //            m_MiscellaneousMatterLibrary = compoundLibrary;
        //        }
        //        return m_MiscellaneousMatterLibrary;
        //    }
        //}

        //private static LibrarySubAtomicParticle LibrarySubAtomicParticle //m_SubAtomicParticleLibrary
        //{
        //    get
        //    {
        //        if (m_SubAtomicParticleLibrary == null)
        //        {
        //            Dictionary<string, SubAtomicParticle> subAtomicParticleDictionary = SubAtomicParticleLibrary.LoadSubAtomicParticleData();

        //            int count = 0;
        //            Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
        //            foreach (KeyValuePair<string, SubAtomicParticle> item in subAtomicParticleDictionary)
        //            {
        //                enumDictionary.Add(count, item.Key);
        //                count++;
        //            }

        //            LibrarySubAtomicParticle subAtomicParticleLibrary = new LibrarySubAtomicParticle();
        //            subAtomicParticleLibrary.ConstantsDictionary = subAtomicParticleDictionary;
        //            subAtomicParticleLibrary.ConstantsEnumDictionary = enumDictionary;
        //            m_SubAtomicParticleLibrary = subAtomicParticleLibrary;
        //        }
        //        return m_SubAtomicParticleLibrary;
        //    }
        //}

        //private static LibraryElement LibraryElement //m_ElementLibrary   
        //{
        //    get
        //    {
        //        if (m_ElementLibrary == null)
        //        {
        //            Dictionary<string, Element> elementDictionary = ElementLibrary.LoadLibrary();

        //            int count = 0;
        //            Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
        //            foreach (KeyValuePair<string, Element> item in elementDictionary)
        //            {
        //                enumDictionary.Add(count, item.Key);
        //                count++;
        //            }

        //            LibraryElement elementLibrary = new LibraryElement();
        //            elementLibrary.ConstantsDictionary = elementDictionary;
        //            elementLibrary.ConstantsEnumDictionary = enumDictionary;
        //            m_ElementLibrary = elementLibrary;
        //        }
        //        return m_ElementLibrary;
        //    }
        //}

        #endregion

       
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="chosenKey"></param>
        ///// <param name="isotopeKey"></param>
        ///// <returns></returns>
        //public static double GetIsotopeMass(ChoicesElements elementKey, string isotopeKey)
        //{
        //    Elements.GetMonoisotopicMass(PNNLOmics.Data.Constants.ChoicesElements.Actinium);
        //    double data =  Elements[PNNLOmics.Data.Constants.ChoicesElements.Actinium].MassAverage;
			
        //    Element element = Elements.GetElement(elementKey);
        //    return element.IsotopeDictionary[isotopeKey].Mass;
        //}

        //public static double GetIsotopeAbundance(ChoicesElements elementKey, string isotopeKey)
        //{
        //    Element element = Elements.GetElement(elementKey);
        //    return element.IsotopeDictionary[isotopeKey].Mass;            
        //}

        //public static int GetIsotopeNumber(ChoicesElements elementKey, string isotopeKey)
        //{
        //    Element element = Elements.GetElement(elementKey);
        //    return element.IsotopeDictionary[isotopeKey].Mass;
        //}        		
    }
}

/* 
			 * how to get the list of all masses for amino acids.
			// Array of constants.
			Array values		= Enum.GetValues(typeof(AminoAcid));
			AminoAcid[] acids	= new AminoAcid[values.Length];
			values.CopyTo(acids, 0);

			foreach (AminoAcid acid in acids)
			{
				AminoAcids.GetMonoisotopicMass(acid);
			}
		*/			         