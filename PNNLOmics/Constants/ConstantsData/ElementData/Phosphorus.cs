using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public class Phosphorus : Element
    {
        public Isotope P31;

        public Phosphorus()
        {
            this.IsotopeList = new List<Isotope>();
            P31 = new Isotope(31, 30.9737622, 1.000);

            this.IsotopeList.Add(P31);
            this.Name = "Phosphorus";
            this.Symbol = "P";
        }

    }
}
