using System;
using System.Collections.Generic;

namespace PNNLOmics.Data
{
    /// <summary>
    /// This class encapsulates peptide level information.
    /// </summary>
    public class Peptide: Molecule
    {
        private int m_id;

        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        #region BaseData<Peptide> Members

        public void Clear()
        {
            throw new NotImplementedException();
        }
        #endregion        

        private List<Protein> m_proteinList;

        public List<Protein> ProteinList
        {
            get { return m_proteinList; }
            set { m_proteinList = value; }
        }

        private int m_sequence;

        public int Sequence
        {
            get { return m_sequence; }
            set { m_sequence = value; }
        }

        private string m_extendedSequence;
    }
}
