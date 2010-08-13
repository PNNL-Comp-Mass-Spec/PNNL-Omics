using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public class Isotope
    {
        public Isotope(int isotopeNumber,double mass, double naturalAbundance)
        {
            this.IsotopeNumber = isotopeNumber;
            this.Mass = mass;
            this.NaturalAbundance = naturalAbundance;
        }
        
        public double Mass { get; set; }
        public int IsotopeNumber { get; set; }
        public double NaturalAbundance { get; set; }
    }
}
