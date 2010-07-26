using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Ammonia : OtherMolecule
    {
        public Ammonia()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 3));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 1));
            this.SingleLetterCode = "R";
            this.ChemicalFormula = "NH3";
            this.SixLetterCode = "NH3   ";
            this.Name = "Ammonia";
        }
    }
}
