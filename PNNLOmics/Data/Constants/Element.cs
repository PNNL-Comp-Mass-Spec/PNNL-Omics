using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This is a Class designed to inherit from matter and detail the information for elements from the periodic table.
    /// </summary>
    public class Element : Matter
    {
        public double MassAverage { get; set; }
        
        //public double MassMostAbundantIsotope { get; set; }
        public Dictionary<string, Isotope> IsotopeDictionary { get; set; }
    }
}
