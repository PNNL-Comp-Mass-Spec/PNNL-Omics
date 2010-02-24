using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics
{
    public class IMSScan:Scan
    {
        private float m_driftTime;

        public float DriftTime
        {
            get { return m_driftTime; }
            set { m_driftTime = value; }
        }
    }
}
