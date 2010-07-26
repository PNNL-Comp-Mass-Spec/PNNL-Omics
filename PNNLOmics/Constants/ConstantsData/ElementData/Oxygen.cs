using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public class Oxygen : Element
    {
        public Isotope O16;
        public Isotope O17;
        public Isotope O18;

        public Oxygen()
        {
            this.IsotopeList = new List<Isotope>();
            O16 = new Isotope(16, 15.994914622325, 0.99762065);
            O17 = new Isotope(17, 16.9991315022, 0.00037909); 
            O18 = new Isotope(18, 17.99916049, 0.00200045);

            this.IsotopeList.Add(O16);
            this.IsotopeList.Add(O17);
            this.IsotopeList.Add(O18);
            this.Name = "Oxygen";
            this.Symbol = "O";
        }
    }
}
