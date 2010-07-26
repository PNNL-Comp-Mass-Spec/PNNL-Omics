using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Glutamine : AminoAcid
    {
        public Glutamine()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 5));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 8));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 2));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 2));
            this.SingleLetterCode = "Q";
            this.ChemicalFormula = "C5H8N2O2";
        }
    }
}
