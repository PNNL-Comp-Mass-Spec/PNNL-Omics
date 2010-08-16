using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This is a Class designed to inherit from compound and detail the information for amino acid residues.
    /// </summary>
    public class AminoAcid : Compound
    { 
        public char SingleLetterCode { get; set; }

        public override string ToString()
        {
            return this.SingleLetterCode.ToString();
        }
    }

    
}
