using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Glycine : AminoAcid
    {
        public Glycine()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 2));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 3));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 1));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 1));
            this.SingleLetterCode = "G";
            this.ChemicalFormula = "C2H3NO";
        }
    }
}
