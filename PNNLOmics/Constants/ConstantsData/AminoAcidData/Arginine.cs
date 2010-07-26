using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;

namespace Constants
{
    public class Arginine : AminoAcid
    {
        public Arginine ()
	    {
            this.Composition = new List<ElementQuantity>();
            this.Composition.Add(new ElementQuantity(new Carbon(), 6));
            this.Composition.Add(new ElementQuantity(new Hydrogen(), 12));
            this.Composition.Add(new ElementQuantity(new Nitrogen(), 4));
            this.Composition.Add(new ElementQuantity(new Oxygen(), 1));
            this.SingleLetterCode = "R";
            this.ChemicalFormula = "C6H12N4O";
    	}
    
            
    }
}
