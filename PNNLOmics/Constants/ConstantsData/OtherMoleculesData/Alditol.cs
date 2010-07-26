using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Alditol : OtherMolecule
    {
        public Alditol()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 4));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 1));
            this.SingleLetterCode = "O";
            this.ChemicalFormula = "H4O";
            this.SixLetterCode = "Aldtol";
            this.Name = "Alditol";
        }
    }
}
