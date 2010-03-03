using System;
using System.Collections.Generic;

namespace PNNLOmics.Data
{
    public class Protein: BaseData
    {
        private string m_proteinSequence;
        public string ProteinSequence
        {
            get { return m_proteinSequence; }
            set { m_proteinSequence = value; }
        }
        private IList<MassTag> m_massTagList;

        public IList<MassTag> MassTagList
        {
            get { return m_massTagList; }
            set { m_massTagList = value; }
        }


      
        public override void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
