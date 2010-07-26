using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Asparagine : AminoAcid
    {
        public Asparagine()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 4));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 6));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 2));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 2));
            this.SingleLetterCode = "N";
            this.ChemicalFormula = "C4H6N2O2";


        }
    }
}
