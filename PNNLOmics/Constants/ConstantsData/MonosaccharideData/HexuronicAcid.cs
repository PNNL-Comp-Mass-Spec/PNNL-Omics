using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Constants
{
    class HexuronicAcid : Monosaccharide
    {
        public HexuronicAcid()
        {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 6));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 8));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 6));
            this.SingleLetterCode = "U";
            this.SixLetterCode = "Hex A ";
            this.ChemicalFormula = "C6H8O6";
            this.Name = "Hexuronic Acid";
        }
    }
}
