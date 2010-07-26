using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public class OtherMolecule : CompoundComponent
    {
        public string SingleLetterCode { get; set; }

        public string SixLetterCode { get; set; }

        public string Name { get; set; }

        public string ChemicalFormula { get; set; }

        public override string ToString()
        {
            return this.SingleLetterCode;
        }
    }
}
