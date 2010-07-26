using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Aldehyde : OtherMolecule
    {
        public Aldehyde()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 2));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 1));
            this.SingleLetterCode = "A";
            this.ChemicalFormula = "H2O";
            this.SixLetterCode = "Aldhyd";
            this.Name = "Aldehyde";
        }
    }
}
