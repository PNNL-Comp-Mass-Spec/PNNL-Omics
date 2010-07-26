using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Leucine : AminoAcid
    {
        public Leucine()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 6));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 11));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 1));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 1));
            this.SingleLetterCode = "L";
            this.ChemicalFormula = "C6H11NO";
        }
    }
}
