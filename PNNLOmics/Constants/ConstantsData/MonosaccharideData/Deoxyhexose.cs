using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    class Deoxyhexose : Monosaccharide
    {
        public Deoxyhexose()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 6));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 10));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 4));
            this.SingleLetterCode = "D";
            this.SixLetterCode = "DxyHex";
            this.ChemicalFormula = "C6H10O4";
            this.Name = "Deoxyhxode";
        }
    }
}
