using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class NaMinusH : OtherMolecule  //subtract a neutral H and add a neutral Na
    {
        public NaMinusH()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Sodium(), 1));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), -1));
            this.SingleLetterCode = "N";
            this.ChemicalFormula = "Na-H";
            this.SixLetterCode = "NaminH";
            this.Name = "NaMinusH";
        }
    }
}
