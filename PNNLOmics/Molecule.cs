using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    public class Molecule: BaseData
    {
        /// <summary>
        /// Gets or sets the scan the molecule was identified in.
        /// </summary>
        public int Scan
        {
            get;
            set;
        }
        private string m_description;        
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }
        private MassTag m_massTag;

        public MassTag MassTag
        {
            get { return m_massTag; }
            set { m_massTag = value; }
        }
        private string m_name;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        private int m_chemicalFormula;

        public int ChemicalFormula
        {
            get { return m_chemicalFormula; }
            set { m_chemicalFormula = value; }
        }

        public override void Clear()
        {
            Spectrum  = null;
            m_massTag = null;
        }

        /// <summary>
        /// Gets the moleculare Weight
        /// </summary>
        public int MassMonoisotopic
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }
        /// <summary>
        /// Gets or sets the spectrum that identified the molecule.
        /// </summary>
        public MSSpectra Spectrum
        {
            get;
            set;
        }
    }
}
