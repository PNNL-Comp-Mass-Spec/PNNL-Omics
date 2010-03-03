using System;

namespace PNNLOmics.Data
{
    public class IMSScan: Scan
    {
        private float m_driftTime;

        public float DriftTime
        {
            get { return m_driftTime; }
            set { m_driftTime = value; }
        }
    }
}
