using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace LCMS.Regression
{
    class NaturalCubicSplineRegression
    {
        List<RegressionPts> m_pts;
        List<double> m_intervalStart;

        double[] m_coeffs = new double[512];

        int m_numKnots;
        double m_minX;
        double m_maxX;

        public NaturalCubicSplineRegression()
        {
            m_numKnots = 2;
            m_pts = new List<RegressionPts>();
            m_intervalStart = new List<double>();
        }

        public void Clear()
        {
            m_pts.Clear();
            m_intervalStart.Clear();
        }

        public void SetOptions(int numKnots)
        {
            m_numKnots = numKnots;
        }

        public void PreprocessCopyData(List<RegressionPts> points)
        {
            int numPts = points.Count();

            m_minX = double.MaxValue;
            m_maxX = -1 * double.MaxValue;
            foreach (RegressionPts point in points)
            {
                if (point.X < m_minX)
                {
                    m_minX = point.X;
                }
                if (point.X > m_maxX)
                {
                    m_maxX = point.X;
                }
                m_pts.Add(point);
            }

            for (int i = 0; i <= m_numKnots; i++)
            {
                double val = (i * (m_maxX - m_minX)) / (m_numKnots + 1) + m_minX;
                m_intervalStart.Add(val);
            }
        }

        // input points are [x, y], order specifies order of the regression line
        public bool CalculateLSQRegressionCoefficients(ref List<RegressionPts> Points)
        {
            Clear();
            if (m_numKnots < 2)
            {
                // Needs at least two knots for a natural cubic spline
                return false;
            }

            if (Points.Count == 0)
            {
                // Needs at least a single point for the coefficient
                return false;
            }

            PreprocessCopyData(Points);

            DenseMatrix A, B, C, BInterp;
            DenseMatrix ATrans;
            DenseMatrix ATransA, InvATransA, InvATransAATrans;

            int numPts = m_pts.Count;

            A = new DenseMatrix(numPts, m_numKnots);
            B = new DenseMatrix(numPts, 1);

            double intervalWidth = (m_maxX - m_minX) / (m_numKnots + 1);

            for (int pointNum = 0; pointNum < numPts; pointNum++)
            {
                RegressionPts point = m_pts[pointNum];
                double coeff = 1;
                A[pointNum, 0] = coeff;
                A[pointNum, 1] = point.X;

                int intervalNum = Convert.ToInt32(((point.X - m_minX) * (m_numKnots + 1)) / (m_maxX - m_minX));
                if (intervalNum > m_numKnots)
                {
                    intervalNum = m_numKnots;
                }

                double KMinus1 = 0;
                if (point.X > m_intervalStart[m_numKnots - 1])
                {
                    KMinus1 = Math.Pow(point.X - m_intervalStart[m_numKnots], 3);
                    if (point.X > m_intervalStart[m_numKnots])
                    {
                        KMinus1 = KMinus1 - Math.Pow(point.X - m_intervalStart[m_numKnots], 3);
                    }
                    KMinus1 = KMinus1 / intervalWidth;
                }

                for (int k = 1; k <= m_numKnots - 2; k++)
                {
                    double kminus1 = 0;

                    if (point.X > m_intervalStart[k])
                    {
                        kminus1 = Math.Pow(point.X - m_intervalStart[k], 3);
                        if (point.X > m_intervalStart[m_numKnots])
                        {
                            kminus1 = kminus1 - Math.Pow(point.X - m_intervalStart[m_numKnots], 3);
                        }
                        kminus1 = kminus1 / intervalWidth;
                    }

                    A[pointNum, k + 1] = kminus1 - KMinus1;
                }

                B[pointNum, 0] = point.MassError;
            }

            ATrans = (DenseMatrix)A.Transpose();
            ATransA = (DenseMatrix)A.Multiply(ATrans);

            // Can't invert a matrix with a determinant of 0.
            if (ATransA.Determinant() == 0)
            {
                return false;
            }

            InvATransA = (DenseMatrix)ATrans.Inverse();
            InvATransAATrans = (DenseMatrix)InvATransA.Multiply(ATrans);

            C = (DenseMatrix)InvATransAATrans.Multiply(B);

            BInterp = (DenseMatrix)A.Multiply(C);

            for (int col_num = 0; col_num < m_numKnots; col_num++)
            {
                m_coeffs[col_num] = C[col_num, 0];
            }

            return true;
        }

        public double GetPredictedValue(double x)
        {
            if (m_pts.Count == 0)
            {
                return 0;
            }

            if (x <= m_minX)
            {
                return m_coeffs[0] + m_coeffs[1] * m_minX;
            }

            if (x >= m_maxX)
            {
                x = m_maxX;
            }

            double val = m_coeffs[0];
            double intervalWidth = (m_maxX - m_minX) / (m_numKnots + 1);

            val = m_coeffs[0] + m_coeffs[1] * x;

            double KMinus1 = 0;
            if (x > m_intervalStart[m_numKnots - 1])
            {
                KMinus1 = Math.Pow(x - m_intervalStart[m_numKnots - 1], 3);
                if (x > m_intervalStart[m_numKnots])
                {
                    KMinus1 = KMinus1 - Math.Pow(x - m_intervalStart[m_numKnots], 3);
                }
                KMinus1 = KMinus1 / intervalWidth;
            }

            for (int k = 1; k <= m_numKnots - 2; k++)
            {
                double kminus1 = 0;
                if (x > m_intervalStart[k])
                {
                    kminus1 = Math.Pow(x - m_intervalStart[k], 3);
                    if (x > m_intervalStart[m_numKnots])
                    {
                        kminus1 = kminus1 - Math.Pow(x - m_intervalStart[m_numKnots], 3);
                    }
                }
                val = val + (kminus1 - KMinus1) * m_coeffs[k + 1];
            }

            return val;
        }

    }
}
