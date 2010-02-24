using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Abstract base class for scan data shared between LC and IMS data objects.
    /// </summary>
    public abstract class Scan: IBaseData<Scan>
    {
        /// <summary>
        /// MS Scan number
        /// </summary>
        private int m_scanNumber;
        /// <summary>
        /// Highest peak found in the isotopic profile
        /// </summary>
        private MSPeak m_basePeak;

        /// <summary>
        /// Gets or sets the highest MS Peak of the isotopic profile
        /// </summary>
        public MSPeak BasePeak
        {
            get { return m_basePeak; }
            set { m_basePeak = value; }
        }
        /// <summary>
        /// Gets or sets the MS Scan Number
        /// </summary>
        public int ScanNumber
        {
            get { return m_scanNumber; }
            set { m_scanNumber = value; }
        }       
    }
}
