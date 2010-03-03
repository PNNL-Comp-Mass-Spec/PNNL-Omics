using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    public class Peptide: BaseData
    {
        private int m_id;
        private MassTag m_massTag;

        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }
        private string m_peptideSequence;

        public string PeptideSequence
        {
            get { return m_peptideSequence; }
            set { m_peptideSequence = value; }
        }
        private string m_peptideSequenceExtended;

        public string PeptideSequenceExtended
        {
            get { return m_peptideSequenceExtended; }
            set { m_peptideSequenceExtended = value; }
        }

        public MassTag MassTag
        {
            get { return m_massTag; }
            set { m_massTag = value; }
        }

        #region BaseData<Peptide> Members

        public override void Clear()
        {
            throw new NotImplementedException();
        }
        #endregion        
    }
}
