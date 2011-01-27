using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    public class AMTTagDatabase
    {
        private System.Collections.Generic.IList<MassTag> m_massTagList;
        private List<Organism> m_organismList;

        public List<Organism> OrganismList
        {
            get { return m_organismList; }
            set { m_organismList = value; }
        }
        /// <summary>
        /// PSM
        /// </summary>
        private List<Molecule> m_moleculeSpectrumSourceList;

        public List<Molecule> MoleculeSpectrumSourceList
        {
            get { return m_moleculeSpectrumSourceList; }
            set { m_moleculeSpectrumSourceList = value; }
        }

        private IList<MassTag> MassTagList
        {
            get { return m_massTagList; }
            set { m_massTagList = value; }
        }
    }
}
