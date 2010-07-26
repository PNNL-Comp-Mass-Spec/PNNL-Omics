using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public class Nitrogen : Element
    {
        public Isotope N14;
        public Isotope N15;

        public Nitrogen()
        {
            this.IsotopeList = new List<Isotope>();
            N14 = new Isotope(14, 14.003074007418, 0.9963374);
            N15 = new Isotope(15, 15.00010897312, 0.0036634);

            this.IsotopeList.Add(N14);
            this.IsotopeList.Add(N15);
            this.Name = "Nitrogen";
            this.Symbol = "N";




        }
    }
}
