using System.Collections.Generic;

namespace PNNLOmics.Data.Constants
{
    /// <summary>
    /// This is a Class designed to inherit from matter and detail the information for elements from the periodic table.
    /// </summary>
    public class Element : Matter
    {
        /// <summary>
        /// Average mass from IUPAC 2007.  This is not the calculated one
        /// </summary>
        public double MassAverage { get; set; }

        /// <summary>
        /// Average mass uncertainty in Da from IUPAC 2007
        /// </summary>
        public double MassAverageUncertainty { get; set; }

        /// <summary>
        /// dictionary containing all known isotopes from an element
        /// </summary>
        public Dictionary<string, Isotope> IsotopeDictionary { get; set; }
    }
}
