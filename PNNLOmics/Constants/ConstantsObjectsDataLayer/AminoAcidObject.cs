using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Constants.ConstantsObjectsDataLayer
{
    public class AminoAcidObject : AbstractCompound
    { 
        public char SingleLetterCode { get; set; }

        public override string ToString()
        {
            return this.SingleLetterCode.ToString();
        }
    }

    
}
