using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Constants
{
    /// <summary>
    /// This is a Class designed to inherit from matter and detail the information for elements from the periodic table.
    /// </summary>
    public class Element : Matter
    {
        //TODO: SCOTT - CR - add XML comments
        public double MassAverage { get; set; }

        //TODO: SCOTT - CR - remove this if not needed 
        //public double MassMostAbundantIsotope { get; set; }
        public Dictionary<string, Isotope> IsotopeDictionary { get; set; }
    }
}
