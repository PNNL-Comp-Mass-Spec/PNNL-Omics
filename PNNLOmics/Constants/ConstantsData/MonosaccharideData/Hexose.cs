using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    class Hexose: Monosaccharide
    {
        public Hexose()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(),6));
            this.Composition.Add(new ElementQuantity(new Hydrogen(),10));
            this.Composition.Add(new ElementQuantity(new Oxygen(),5));
            this.SingleLetterCode = "H";
            this.SixLetterCode = "Hex   ";
            this.ChemicalFormula = "C6H10O5";
            this.Name = "Hexose";
        }
    }
}
