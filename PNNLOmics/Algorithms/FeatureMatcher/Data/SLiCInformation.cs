using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace PNNLOmics.Algorithms.FeatureMatcher.Data
{
    public class SLiCInformation
    {
        private double m_massPPMStDev;

        public double MassPPMStDev
        {
            get { return m_massPPMStDev; }
            set { m_massPPMStDev = value; }
        }
        private double m_netStDev;

        public double NETStDev
        {
            get { return m_netStDev; }
            set { m_netStDev = value; }
        }
        private float m_driftTimeStDev;

        public float DriftTimeStDev
        {
            get { return m_driftTimeStDev; }
            set { m_driftTimeStDev = value; }
        }

        public void SetDefaults()
        {
            m_massPPMStDev = 3.0;
            m_netStDev = 0.015;
            m_driftTimeStDev = 0.5f;
        }

        public SLiCInformation()
        {
            SetDefaults();
        }
    }
}
