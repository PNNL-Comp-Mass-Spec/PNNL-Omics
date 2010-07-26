using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public class Sodium : Element
    {
        public Isotope Na23;
       
        public Sodium()
        {
            this.IsotopeList = new List<Isotope>();
            Na23 = new Isotope(23, 22.98976967, 1.00);//Glycolyzer
            
            this.IsotopeList.Add(Na23);

            this.Name = "Sodium";
            this.Symbol = "Na";
        }

    }
}
