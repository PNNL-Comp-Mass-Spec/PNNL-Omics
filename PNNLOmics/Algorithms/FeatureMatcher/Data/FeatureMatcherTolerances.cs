using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace PNNLOmics.Algorithms.FeatureMatcher.Data
{
    public class FeatureMatcherTolerances
    {
        #region Members
        private bool m_refined;

        private double m_massTolerancePPM;
        private double m_netTolerance;

        private float m_driftTimeTolerance;
        #endregion

        #region Properties
        public bool Refined
        {
            get { return m_refined; }
            set { m_refined = value; }
        }

        public double MassTolerancePPM
        {
            get { return m_massTolerancePPM; }
            set { m_massTolerancePPM = value; }
        }
        public double NETTolerance
        {
            get { return m_netTolerance; }
            set { m_netTolerance = value; }
        }

        public float DriftTimeTolerance
        {
            get { return m_driftTimeTolerance; }
            set { m_driftTimeTolerance = value; }
        }
        #endregion

        #region Constructors
        public FeatureMatcherTolerances()
        {
            Clear();
        }
        public FeatureMatcherTolerances(double massTolerancePPM, double netTolerance, float driftTimeTolerance)
        {
            m_massTolerancePPM = massTolerancePPM;
            m_netTolerance = netTolerance;
            m_driftTimeTolerance = driftTimeTolerance;
        }
        #endregion

        #region Public functions
        public void Clear()
        {
            m_massTolerancePPM = 6.0;
            m_netTolerance = 0.025;
            m_driftTimeTolerance = 1.0f;
        }

        public Matrix AsVector(bool driftTime)
        {
            Matrix tolerances;
            if (driftTime)
            {
                tolerances = new Matrix(3, 1, 0.0);
                tolerances[0, 0] = m_massTolerancePPM;
                tolerances[1, 0] = m_netTolerance;
                tolerances[2, 0] = m_driftTimeTolerance;
            }
            else
            {
                tolerances = new Matrix(2, 1, 0.0);
                tolerances[0, 0] = m_massTolerancePPM;
                tolerances[1, 0] = m_netTolerance;
            }
            return (tolerances);
        }
        #endregion
    }
}
