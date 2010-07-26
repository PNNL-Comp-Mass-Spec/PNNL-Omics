using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Cysteine : AminoAcid
    {
        public Cysteine()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 3));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 5));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 1));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 1));
            this.Composition.Add(new ElementQuantity(new Sulfur(), 1));
            this.SingleLetterCode = "C";
            this.ChemicalFormula = "C3H5NOS";
        }
    }
}
