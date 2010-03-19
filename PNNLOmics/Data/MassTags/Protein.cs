using System;
using System.Collections.Generic;

using PNNLOmics;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Class that holds information about a protein.
    /// </summary>
    public class Protein: Molecule
    {
        private int m_sequence;

        public int Sequence
        {
            get { return m_sequence; }
            set { m_sequence = value; }
        }

        /// <summary>
        /// Clears the 
        /// </summary>
        public void Clear()
        {            
        }
    }

    /// <summary>
    /// Class that holds information about a protein.
    /// </summary>
    public class Metabolite : Molecule
    {

        /// <summary>
        /// Clears the 
        /// </summary>
        public void Clear()
        {
        }
    }
}
