using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Proline : AminoAcid
    {
        public Proline()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 5));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 7));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 1));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 1));
            this.SingleLetterCode = "P";
            this.ChemicalFormula = "C5H7NO";
        }
    }
}
