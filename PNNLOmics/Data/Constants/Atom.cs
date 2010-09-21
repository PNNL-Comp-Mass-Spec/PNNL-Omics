using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This is an empty Class designed to inherit from matter and behave like other inherited objects from matter such as elements.
    /// </summary>
    public class Atom
    {
        public string Name { get; set; }
        public double MassMonoIsotopic { get; set; }
        public string Symbol { get; set; } 
    }
}
