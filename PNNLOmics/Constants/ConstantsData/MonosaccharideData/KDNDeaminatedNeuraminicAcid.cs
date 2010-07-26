using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//2-Keto-3-Deoxy-D-Glycero-D-Galacto-Nononic-Acid

namespace Constants
{
    class KDNDeaminatedNeuraminicAcid : Monosaccharide
    {
        public KDNDeaminatedNeuraminicAcid()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 9));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 14));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 8));
            this.SingleLetterCode = "K";
            this.SixLetterCode = "KDN   ";
            this.ChemicalFormula = "C9H14O8";
            this.Name = "(KDN) 2-Keto-3-Deoxy-D-Glycero-D-Galacto-Nononic-Acid";
        }
    }
}

