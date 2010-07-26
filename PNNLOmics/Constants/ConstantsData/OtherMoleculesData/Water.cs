using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Water : OtherMolecule
    {
        public Water()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 2));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 1));
            this.SingleLetterCode = "W";
            this.ChemicalFormula = "H2O";
            this.SixLetterCode = "Water ";
            this.Name = "Water";
        }
    }
}
