using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Ammonium : OtherMolecule
    {
        public Ammonium()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 4));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 1));
            this.SingleLetterCode = "M";
            this.ChemicalFormula = "NH4+";
            this.SixLetterCode = "NH4+  ";
            this.Name = "Ammonium";
        }
    }
}
