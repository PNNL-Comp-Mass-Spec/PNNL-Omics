using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Constants
{
    public class Isotope
    {
        /// <summary>
        /// This is a Class designed to contain the information on the natural abundance of heavy isotopes on earth.
        /// </summary>
        public Isotope(int isotopeNumber, double mass, double naturalAbundance)
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
