using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    public class Isotope
    {
        /// <summary>
        /// This is a Class designed to contain the information on the natural abundance of heavy isotopes on earth.
        /// </summary>
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
