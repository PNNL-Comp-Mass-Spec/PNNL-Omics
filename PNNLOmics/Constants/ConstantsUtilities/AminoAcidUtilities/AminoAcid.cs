using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class AminoAcid:CompoundComponent
    {
        //public Dictionary<AminoAcid,string> AminoAcidLookupTable { get; set; }

        public string SingleLetterCode { get; set; }

        public string ChemicalFormula { get; set; }

        public override string ToString()
        {
            return this.SingleLetterCode;
        }
    }
}
