using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    class NGlycolylNeuraminicAcid : Monosaccharide
    {
        public NGlycolylNeuraminicAcid()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 11));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 17));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 1));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 8));
            this.SingleLetterCode = "G";
            this.SixLetterCode = "Neu5Gc";
            this.ChemicalFormula = "C11H17NO9";
            this.Name = "N-glycolylneuraminic acid";
        }
    }
}