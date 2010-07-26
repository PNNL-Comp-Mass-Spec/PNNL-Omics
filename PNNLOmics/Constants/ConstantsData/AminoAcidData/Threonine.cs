using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Threonine : AminoAcid
    {
        public Threonine()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 4));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 7));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 1));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 2));
            this.SingleLetterCode = "T";
            this.ChemicalFormula = "C4H7NO2";
        }
    }
}
