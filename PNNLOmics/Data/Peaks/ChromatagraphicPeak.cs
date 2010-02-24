﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics
{
    public class ChromatagraphicPeak: Peak
    {
        /// <summary>
        /// Gets or sets the Scan Number this peak was found in.
        /// </summary>
        public int ScanNumber
        {
            get
            {
                return Convert.ToInt32(XValue);
            }
            set
            {
                XValue = Convert.ToDouble(value);
            }
        }

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
