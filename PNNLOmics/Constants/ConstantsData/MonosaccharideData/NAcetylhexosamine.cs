using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    class NAcetylhexosamine : Monosaccharide
    {
        public NAcetylhexosamine()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(),8));
            this.Composition.Add(new ElementQuantity(new Hydrogen(),13));
            this.Composition.Add(new ElementQuantity(new Nitrogen(),1));
            this.Composition.Add(new ElementQuantity(new Oxygen(),5));
            this.SingleLetterCode = "N";
            this.SixLetterCode = "HexNAc";
            this.ChemicalFormula = "C8H13NO5";
            this.Name = "N-acetylhexosamine";
        }
    }
}