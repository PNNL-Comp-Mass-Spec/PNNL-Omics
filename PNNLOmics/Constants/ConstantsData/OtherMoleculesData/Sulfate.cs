using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Sulfate : OtherMolecule
    {
        public Sulfate()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Sulfur(), 2));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 4));
            this.SingleLetterCode = "S";
            this.ChemicalFormula = "S04";
            this.SixLetterCode = "Sulfat";
            this.Name = "Sulfate";
        }
    }
}
