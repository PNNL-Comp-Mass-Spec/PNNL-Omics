using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PNNLOmics.Alignment.LCMSWarp.LCMSWarper.LCMSRegression
{
    /// <summary>
    /// Object to hold the necesary information for an LSQ regression for LCMSWarp
    /// </summary>
    public class LcmsLsqSplineRegression
    {
        readonly List<LcmsRegressionPts> m_pts;
        readonly double[] m_coeffs = new double[512];
        private const int MaxOrder = 16; // Maximum order of spline regression supported

        int m_order;
        int m_numKnots;
        double m_minX;
        double m_maxX;

        /// <summary>
        /// Cleans any remaining data from previous regression
        /// </summary>
        public void Clear()
        {
            m_pts.Clear();
        }

        /// <summary>
        /// Constructor for an LSQSplineRegressor. Initializes number of knots, order and sets up
        /// new memory space for the regression points
        /// </summary>
        public LcmsLsqSplineRegression()
        {
            m_numKnots = 0;
            m_order = 1;
            m_pts = new List<LcmsRegressionPts>();
        }

        /// <summary>
        /// Copies the number of knots to internal for LSQ regression
        /// </summary>
        /// <param name="numKnots"></param>
        public void SetOptions(int numKnots)
        {
            m_numKnots = numKnots;
        }

        /// <summary>
        /// Determines the min and max range for the regression
        /// </summary>
        /// <param name="points"></param>
        public void PreprocessCopyData(ref List<LcmsRegressionPts> points)
        {
            // find the min and max
            int numPoints = points.Count;
            m_minX = double.MaxValue;
            m_maxX = double.MinValue;

            for (int pointNum = 0; pointNum < numPoints; pointNum++)
            {
                LcmsRegressionPts point = points[pointNum];
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

        /// <summary>
        /// Computes the Regressor coefficients based on the order of the LSQ and the points to regress
        /// </summary>
        /// <param name="order"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public bool CalculateLsqRegressionCoefficients(int order, ref List<LcmsRegressionPts> points)
        {
            Clear();
            m_order = order;

            if (order > MaxOrder)
            {
                m_order = MaxOrder;
            }

            PreprocessCopyData(ref points);

            int numPoints = m_pts.Count;

            var a = new DenseMatrix(numPoints, m_order + m_numKnots + 1);
            var b = new DenseMatrix(numPoints, 1);

            for (int pointNum = 0; pointNum < numPoints; pointNum++)
            {
                LcmsRegressionPts calib = m_pts[pointNum];
                double coeff = 1;
                a[pointNum, 0] = coeff;
                for (int colNum = 1; colNum <= m_order; colNum++)
                {
                    coeff = coeff * calib.X;
                    a[pointNum, colNum] = coeff;
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
                        a[pointNum, colNum] = Math.Pow(calib.X - xIntervalStart, m_order);
                    }
                    for (int colNum = m_order + xInterval + 1; colNum <= m_order + m_numKnots; colNum++)
                    {
                        a[pointNum, colNum] = 0;
                    }
                }

                b[pointNum, 0] = calib.MassError;
            }

            var aTrans = (DenseMatrix)a.Transpose();
            var aTransA = (DenseMatrix)aTrans.Multiply(a);

            // Can't invert a matrix with a determinant of 0, if so return false
            if (Math.Abs(aTransA.Determinant()) < double.Epsilon)
            {
                return false;
            }

            var invATransA = (DenseMatrix)aTransA.Inverse();

            var c = (DenseMatrix)invATransA.Multiply(b);

            for (int colNum = 0; colNum <= m_order + m_numKnots; colNum++)
            {
                m_coeffs[colNum] = c[colNum, 0];
            }

            return true;
        }

        /// <summary>
        /// Given a value "x", returns where on the regression line the appropriate "y" would fall
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double GetPredictedValue(double x)
        {
            double powerN = 1;
            double val = m_coeffs[0];

            for (int power = 1; power <= m_order; power++)
            {
                powerN = powerN * x;
                val = val + m_coeffs[power] * powerN;
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
