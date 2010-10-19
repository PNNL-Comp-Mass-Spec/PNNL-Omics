using System;
using System.Collections.Generic;

namespace PNNLOmics.Data.Constants
{
    /// <summary>
    /// This is an abstract Class designed to cover the most basic parameters of matter objects.
    /// </summary>
    public abstract class Matter
    {
        public string Name { get; set; }
        public double MassMonoIsotopic { get; set; }
        public string Symbol { get; set; }
    }
    //TODO: SCOTT - CR - XML on properties
}
