using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    public class MSPeak: Peak
    {
        private int m_scanNumber;

        public int ScanNumber
        {
            get { return m_scanNumber; }
            set { m_scanNumber = value; }
        }
        /// <summary>
        /// Gets or sets the mass to charge ratio (m/z).
        /// </summary>
        public double MZ
        {
            get
            {
                return XValue;
            }
            set
            {
                XValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the intensity of the peak
        /// </summary>
        public int Intensity
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        public override void Clear()
        {
            base.Clear();
        }
    }
}
