using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Lysine : AminoAcid
    {
        public Lysine()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 6));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 12));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 2));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 1));
            this.SingleLetterCode = "K";
            this.ChemicalFormula = "C6H12N2O";
        }
    }
}
