using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    class Sulfur : Element
    {
        public Isotope S32;
        public Isotope S33;
        public Isotope S34;
        public Isotope S36;

        public Sulfur()
        {
            this.IsotopeList = new List<Isotope>();
            S32 = new Isotope(32, 31.9720707315, 0.950395790);
            S33 = new Isotope(33, 32.9714585415, 0.007486512);
            S34 = new Isotope(34, 33.9678668714, 0.041971987);
            S36 = new Isotope(36, 35.9670808825, 0.000145921);

            this.IsotopeList.Add(S32);
            this.IsotopeList.Add(S33);
            this.IsotopeList.Add(S34);
            this.IsotopeList.Add(S36);
            this.Name = "Sulfer";
            this.Symbol = "S";

        }

    }
}
