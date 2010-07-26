using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public class Hydrogen : Element
    {
        public Isotope H1;
        public Isotope H2;

        public Hydrogen()
        {
            this.IsotopeList = new List<Isotope>();
            H1 = new Isotope(1, 1.00782503196, 0.999844265);
            H2 = new Isotope(2, 2.01410177796, 0.000155745);

            this.IsotopeList.Add(H1);
            this.IsotopeList.Add(H2);
            this.Name = "Hydrogen";
            this.Symbol = "H";



        }


    }
}
