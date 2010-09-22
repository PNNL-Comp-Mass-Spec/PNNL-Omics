using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This is an abstract Class designed to cover the most basic parameters of matter objects.
    /// </summary>
    //TODO: Change to massMonoisotopic
    //TODO:  XML on properties
    //TODO:  Axe constantsdatalayer folder and change data utilities to Utilities
    public abstract class Matter
    {
        public string Name { get; set; }
        public double MassMonoIsotopic { get; set; }
        public string Symbol { get; set; } 

    }
}
