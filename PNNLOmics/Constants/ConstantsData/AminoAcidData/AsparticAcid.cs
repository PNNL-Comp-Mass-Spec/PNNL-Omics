using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class AsparticAcid : AminoAcid
    {
        public AsparticAcid()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 4));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 5));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 1));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 3));
            this.SingleLetterCode = "D";
            this.ChemicalFormula = "C5H5N1O3";
        }


    }
}
