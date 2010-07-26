using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public class Potassium : Element
    {
        public Isotope K39;
        public Isotope K40;
        public Isotope K41;

        public Potassium()
        {
            this.IsotopeList = new List<Isotope>();
            K39 = new Isotope(39, 38.9637069, 0.932581);//web elements
            K40 = new Isotope(40, 39.9639992, 0.000117);//web elements
            K41 = new Isotope(41, 40.9618254, 0.067302);//web elements

            this.IsotopeList.Add(K39);
            this.IsotopeList.Add(K40);
            this.IsotopeList.Add(K41);

            this.Name = "Potassium";
            this.Symbol = "K";
        }
    }
}
