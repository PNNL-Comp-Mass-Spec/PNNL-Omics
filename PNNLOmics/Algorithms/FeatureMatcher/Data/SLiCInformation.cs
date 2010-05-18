using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace PNNLOmics.Algorithms.FeatureMatcher.Data
{
    public class SLiCInformation
    {
        #region Members
        private double m_massPPMStDev;
        private double m_netStDev;

        private float m_driftTimeStDev;
        #endregion

        #region Properties
        public double MassPPMStDev
        {
            get { return m_massPPMStDev; }
            set { m_massPPMStDev = value; }
        }
        public double NETStDev
        {
            get { return m_netStDev; }
            set { m_netStDev = value; }
        }

        public float DriftTimeStDev
        {
            get { return m_driftTimeStDev; }
            set { m_driftTimeStDev = value; }
        }
        #endregion

        #region Constructors
        public SLiCInformation()
        {
            Clear();
        }
        #endregion

        #region Public functions
        public void Clear()
        {
            m_massPPMStDev = 3.0;
            m_netStDev = 0.015;
            m_driftTimeStDev = 0.5f;
        }
        #endregion
    }
}
