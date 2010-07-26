using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    class Pentose : Monosaccharide
    {
        public Pentose()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 6));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 10));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 5));
            this.SingleLetterCode = "P";
            this.SixLetterCode = "Pentos";
            this.ChemicalFormula = "C5H8O4";
            this.Name = "Pentose";
        }
    }
}