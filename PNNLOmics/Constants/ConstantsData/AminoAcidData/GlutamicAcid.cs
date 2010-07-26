using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class GlutamicAcid:AminoAcid
    {
        public GlutamicAcid()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 5));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 7));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 1));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 3));
            this.SingleLetterCode = "E";
            this.ChemicalFormula = "C5H7NO3";
        }
    }
}
