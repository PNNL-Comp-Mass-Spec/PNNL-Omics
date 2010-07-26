using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Constants
{
    class NeuraminicAcid : Monosaccharide
    {
        public NeuraminicAcid()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 11));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 17));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 1));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 8));
            this.SingleLetterCode = "A";
            this.SixLetterCode = "Neu5Ac";
            this.ChemicalFormula = "C11H17NO8";
            this.Name = "Neuraminic acid";
        }
    }
}

