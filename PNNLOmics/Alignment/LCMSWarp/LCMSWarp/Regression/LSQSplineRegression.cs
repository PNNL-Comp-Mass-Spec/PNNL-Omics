using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace LCMS.Regression
{
    class LSQSplineRegression
    {
        List<RegressionPts> m_pts;
        double[] m_coeffs = new double[512];
        static int m_maxOrder = 16; // Maximum order of spline regression supported

        int m_order;
        int m_numKnots;
        double m_minX;
        double m_maxX;

        public void Clear()
        {
            m_pts.Clear();
        }

        public LSQSplineRegression()
        {
            m_numKnots = 0;
            m_order = 1;
            m_pts = new List<RegressionPts>();
        }

        public void SetOptions(int numKnots)
        {
            m_numKnots = numKnots;
        }

        public void PreprocessCopyData(ref List<RegressionPts> Points)
        {
            // find the min and max
            int numPoints = Points.Count;
            m_minX = double.MaxValue;
            m_maxX = -1 * double.MaxValue;

            for (int pointNum = 0; pointNum < numPoints; pointNum++)
            {
                RegressionPts point = Points[pointNum];
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
        }

        public bool CalculateLSQRegressionCoefficients(int order, ref List<RegressionPts> Points)
        {
            Clear();
            m_order = order;

            if (order > m_maxOrder)
            {
                order = m_maxOrder;
                m_order = m_maxOrder;
            }

            PreprocessCopyData(ref Points);

            DenseMatrix A, B, C, BInterp;
            DenseMatrix ATrans;
            DenseMatrix ATransA, InvATransA, InvATransAATrans;

            int numPoints = m_pts.Count;

            A = new DenseMatrix(numPoints, m_order + m_numKnots + 1);
            B = new DenseMatrix(numPoints, 1);

            for (int pointNum = 0; pointNum < numPoints; pointNum++)
            {
                RegressionPts calib = m_pts[pointNum];
                double coeff = 1;
                A[pointNum, 0] = coeff;
                for (int colNum = 1; colNum <= m_order; colNum++)
                {
                    coeff = coeff * calib.X;
                    A[pointNum, colNum] = coeff;
                }

                if (m_numKnots > 0 && m_order > 0)
                {
                    int xInterval = Convert.ToInt32(((m_numKnots + 1) * (calib.X - m_minX)) / (m_maxX - m_minX));
                    if (xInterval >= m_numKnots + 1)
                    {
                        xInterval = m_numKnots;
                    }

                    for (int colNum = m_order + 1; colNum <= m_order + xInterval; colNum++)
                    {
                        double xIntervalStart = m_minX + ((colNum - m_order) * (m_maxX - m_minX)) / (m_numKnots + 1);
                        A[pointNum, colNum] = Math.Pow(calib.X - xIntervalStart, m_order);
                    }
                    for (int colNum = m_order + xInterval + 1; colNum <= m_order + m_numKnots; colNum++)
                    {
                        A[pointNum, colNum] = 0;
                    }
                }

                B[pointNum, 0] = calib.MassError;
            }

            ATrans = (DenseMatrix)A.Transpose();
            ATransA = (DenseMatrix)ATrans.Multiply(A);

            // Can't invert a matrix with a determinant of 0, if so return false
            if (ATransA.Determinant() == 0)
            {
                return false;
            }

            InvATransA = (DenseMatrix)ATransA.Inverse();
            InvATransAATrans = (DenseMatrix)InvATransA.Multiply(ATrans);

            C = (DenseMatrix)InvATransA.Multiply(B);
            BInterp = (DenseMatrix)A.Multiply(C);

            for (int colNum = 0; colNum <= m_order + m_numKnots; colNum++)
            {
                m_coeffs[colNum] = C[colNum, 0];
            }

            return true;
        }

        public double GetPredictedValue(double x)
        {
            double power_n = 1;
            double val = m_coeffs[0];

            for (int power = 1; power <= m_order; power++)
            {
                power_n = power_n * x;
                val = val + m_coeffs[power] * power_n;
            }

            if (m_numKnots > 0 && m_order > 0)
            {
                int xInterval = Convert.ToInt32(((m_numKnots + 1) * (x - m_minX)) / (m_maxX - m_minX));
                if (xInterval >= m_numKnots + 1)
                {
                    xInterval = m_numKnots;
                }

                for (int colNum = m_order + 1; colNum <= m_order + xInterval; colNum++)
                {
                    double xIntervalStart = m_minX + ((colNum - m_order) * (m_maxX - m_minX)) / (m_numKnots + 1);
                    val = val + Math.Pow(x - xIntervalStart, m_order) * m_coeffs[colNum];
                }
            }

            return val;
        }

    }
}
