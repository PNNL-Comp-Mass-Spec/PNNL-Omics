using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics
{
    public class MSPeak: Peak
    {
        /// <summary>
        /// Gets
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
    }
}
