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
        public override void Clear()
        {
            throw new NotImplementedException();
        }        
        public List<Protein> ProteinList
        {
            get;
            set;
        }        
        public string Sequence
        {
            get; 
            set;
        }
        public string ExtendedSequence
        {
            get;
            set;
        }
        public int CleavageState
        {
            get;
            set;            
        }
    }
}
