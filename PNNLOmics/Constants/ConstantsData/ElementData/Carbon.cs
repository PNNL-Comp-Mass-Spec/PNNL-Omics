using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public class Carbon : Element
    {
        public Isotope C12;
        public Isotope C13;
        
        public Carbon()
        {
            this.IsotopeList = new List<Isotope>();
            C12 = new Isotope(12, 12, 0.98894428);
            C13 = new Isotope(13, 13.0033548385, 0.01105628);

            this.IsotopeList.Add(C12);
            this.IsotopeList.Add(C13);
            this.Name = "Carbon";
            this.Symbol = "C";

        }
    }
}
