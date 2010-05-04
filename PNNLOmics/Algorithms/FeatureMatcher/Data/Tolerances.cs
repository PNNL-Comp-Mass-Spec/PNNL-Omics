using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace PNNLOmics.Algorithms.FeatureMatcher.Data
{
    public class Tolerances
    {
        private bool m_refined;

        public bool Refined
        {
            get { return m_refined; }
            set { m_refined = value; }
        }

        private double m_massTolerancePPM;

        public double MassTolerancePPM
        {
            get { return m_massTolerancePPM; }
            set { m_massTolerancePPM = value; }
        }
        private double m_netTolerance;

        public double NETTolerance
        {
            get { return m_netTolerance; }
            set { m_netTolerance = value; }
        }
        private float m_driftTimeTolerance;

        public float DriftTimeTolerance
        {
            get { return m_driftTimeTolerance; }
            set { m_driftTimeTolerance = value; }
        }

        public void SetDefaults()
        {
            m_massTolerancePPM = 6.0;
            m_netTolerance = 0.03;
            m_driftTimeTolerance = 1.0f;
        }

        public Tolerances()
        {
            SetDefaults();
        }

        public Tolerances(double massTolerancePPM, double netTolerance, float driftTimeTolerance)
        {
            m_massTolerancePPM = massTolerancePPM;
            m_netTolerance = netTolerance;
            m_driftTimeTolerance = driftTimeTolerance;
        }

        public Tolerances(Matrix toleranceVector)
        {
            m_massTolerancePPM = toleranceVector[0, 0];
            m_netTolerance = toleranceVector[1, 0];
            m_driftTimeTolerance = (float)toleranceVector[2, 0];
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
    }
}
