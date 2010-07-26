using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class KMinusH : OtherMolecule
    {
        public KMinusH()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Hydrogen(), -1));
            this.Composition.Add(new ElementQuantity(new Potassium(), 1));
            this.SingleLetterCode = "K";
            this.ChemicalFormula = "K-H";
            this.SixLetterCode = "KminH ";
            this.Name = "KMinusH";
        }
    }
}
