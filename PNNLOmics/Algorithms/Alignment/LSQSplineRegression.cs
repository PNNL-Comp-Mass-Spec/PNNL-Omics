using System;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// TODO: Create class comment block for LSQSplineRegression
    /// </summary>
    public class LSQSplineRegression
    {
        #region Class Members
        private static readonly int MAX_ORDER = 16;
        private List<RegressionPoints> m_regressionPoints;
        private double[] m_arrayCoefficients;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of LSQSplineRegression
        /// </summary>
        public LSQSplineRegression()
        {
            m_regressionPoints = new List<RegressionPoints>();
            m_arrayCoefficients = new double[512];
            Clear();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the number of knots
        /// </summary>
        public int NumKnots { get; set; }

        /// <summary>
        /// Gets or sets the minimum x
        /// </summary>
        public double MinX { get; set; }

        /// <summary>
        /// Gets or sets the maximum x
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
            Order = 1;
            NumKnots = 0;
            MinX = 0;
            MaxX = 0;
            m_regressionPoints.Clear();
            for (int i = 0; i < m_arrayCoefficients.Length; ++i)
            {
                m_arrayCoefficients[i] = 0.0;
            }
        }

        /// <summary>
        /// Sets the options for this LSQSplineRegression
        /// </summary>
        /// <param name="numKnots">Number of knots</param>
        public void SetOptions(int numKnots)
        {
            NumKnots = numKnots;
        }

        /// <summary>
        /// TODO: Create comment block for CalculateLSQRegressionCoefficients
        /// </summary>
        /// <param name="order"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public bool CalculateLSQRegressionCoefficients(int order, List<RegressionPoints> points)
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