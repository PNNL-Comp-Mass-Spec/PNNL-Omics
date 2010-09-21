using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This is a Class designed to inherit from compound and detail the information for cross ring cleavages of monosacharides.
    /// </summary>
    public class CrossRing : Compound
    {
        public string SixLetterCode { get; set; }
    }
}
