using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This is an abstract Class designed to cover the most basic parameters of matter objects.
    /// </summary>
    public abstract class Matter
    {
        public string Name { get; set; }
        public double MonoIsotopicMass { get; set; }
        public string Symbol { get; set; } 
    }
}
