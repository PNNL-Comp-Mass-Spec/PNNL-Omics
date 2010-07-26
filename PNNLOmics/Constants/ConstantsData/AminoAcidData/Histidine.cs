using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Histidine : AminoAcid
    {
        public Histidine()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 6));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 7));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 3));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 1));
            this.SingleLetterCode = "H";
            this.ChemicalFormula = "C6H7N3O";
        }
    }
}
