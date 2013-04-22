using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    public class Molecule: BaseData
    {
        /// <summary>
        /// Gets or sets the scan the molecule was identified in.
        /// </summary>
        public int Scan
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public int ChargeState
        {
            get;
            set;
        }
        public MassTag MassTag
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string ChemicalFormula
        {
            get;
            set;
        }
        public override void Clear()
        {
            Spectrum  = null;
            MassTag   = null;
        }
        /// <summary>
        /// Gets the moleculare Weight
        /// </summary>
        public double MassMonoisotopic
        {
            get;
            set;
        }
        public double Mz
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the spectrum that identified the molecule.
        /// </summary>
        public MSSpectra Spectrum
        {
            get;
            set;
        }
    }
}
