using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This is a Class designed to inherit from compound and detail the information for monosaccharide residues.
    /// </summary>
    public class Monosaccharide : Compound
    {
        public string ShortName { get; set; }
        public string SixLetterCode { get; set; }
    }
}
