using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Valine : AminoAcid
    {
        public Valine()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 5));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 9));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 1));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 1));
            this.SingleLetterCode = "V";
            this.ChemicalFormula = "C5H9NO";
        }

    }
}
