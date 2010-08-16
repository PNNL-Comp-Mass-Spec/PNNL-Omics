using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This is a Class designed to inherit from compound and detail the information for other molecues including end groups and modifications.
    /// </summary>
    public class OtherMolecule : Compound
    {
        public string SixLetterCode { get; set; }
    }
}
