using System;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// TODO: Create class comment block for NaturalCubicSplineRegression
    /// </summary>
    public class NaturalCubicSplineRegression
    {
        #region Class Members
        private List<RegressionPoints> m_regressionPoints;
        private List<double> m_intervalStart;
        private double[] m_arrayCoefficients;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of NaturalCubicSplineRegression
        /// </summary>
        public NaturalCubicSplineRegression()
        {
            m_regressionPoints = new List<RegressionPoints>();
            m_intervalStart = new List<double>();
            m_arrayCoefficients = new double[512];
            Clear();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the number of knots
        /// </summary>
        public int NumKnots { get; set; }

        /// <summary>
        /// Gets or sets the minimum X
        /// </summary>
        public double MinX { get; set; }

        /// <summary>
        /// Gets or sets the maximum X
        /// </summary>
        public double MaxX { get; set; }

        /// <summary>
        /// Gets or sets the array coefficients
        /// </summary>
        public double[] ArrayCoefficients
        {
            get { return m_arrayCoefficients; }
            set { m_arrayCoefficients = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears all values to their original setting
        /// </summary>
        public void Clear()
        {
            NumKnots = 2;
            MinX = 0;
            MaxX = 0;
            m_regressionPoints.Clear();
            m_intervalStart.Clear();
            for (int i = 0; i < m_arrayCoefficients.Length; ++i)
            {
                m_arrayCoefficients[i] = 0.0;
            }
        }

        /// <summary>
        /// Sets the options for this NaturalCubicSplineRegression
        /// </summary>
        /// <param name="numKnots">Number of knots</param>
        public void SetOptions(int numKnots)
        {
            NumKnots = numKnots;
        }

        /// <summary>
        /// TODO: Create comment block for CalculateLSQRegressionCoefficients
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public bool CalculateLSQRegressionCoefficients(List<RegressionPoints> points)
        {
            // TODO: Implement CalculateLSQRegressionCoefficients
            throw new NotImplementedException();
        }

        /// <summary>
        /// // TODO: Create comment block for PrintRegressionFunction
        /// </summary>
        /// <param name="fileName"></param>
        public void PrintRegressionFunction(string fileName)
        {
            // TODO: Implement PrintRegressionFunction
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for GetPredictedValue
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double GetPredictedValue(double x)
        {
            // TODO: Implement GetPredictedValue
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for PrintPoints
        /// </summary>
        /// <param name="fileName"></param>
        public void PrintPoints(string fileName)
        {
            // TODO: Implement PrintPoints
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// TODO: Create comment block for PreprocessCopyData
        /// </summary>
        /// <param name="regressionPoints"></param>
        private void PreprocessCopyData(List<RegressionPoints> regressionPoints)
        {
            // TODO: Implement PreprocessCopyData
            throw new NotImplementedException();
        }
        #endregion
    }
}