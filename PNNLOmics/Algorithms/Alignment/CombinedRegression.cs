using System;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// TODO: Create class comment block for CombinedRegression
    /// </summary>
    public class CombinedRegression
    {
        #region Class Members
        private RegressionType m_regressionType;
        private bool m_didLSQFail;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of CombinedRegression
        /// </summary>
        public CombinedRegression()
        {
            Clear();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the CentralRegression
        /// </summary>
        public CentralRegression CentralRegression { get; set; }

        /// <summary>
        /// Gets or sets the LSQSplineRegression
        /// </summary>
        public LSQSplineRegression LSQSplineRegression { get; set; }

        /// <summary>
        /// Gets or sets the NaturalCubicSplineRegression
        /// </summary>
        public NaturalCubicSplineRegression NaturalCubicSplineRegression { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears all values back to their original setting
        /// </summary>
        public void Clear()
        {
            m_regressionType = RegressionType.Hybrid;
            m_didLSQFail = false;
            CentralRegression.Clear();
            LSQSplineRegression.Clear();
            NaturalCubicSplineRegression.Clear();
        }

        /// <summary>
        /// Sets the options for the central regression
        /// </summary>
        /// <param name="numXBins">Number of x bins</param>
        /// <param name="numYBins">Number of y bins</param>
        /// <param name="numJumps">Number of jumps</param>
        /// <param name="zTolerance">Z Tolerance</param>
        /// <param name="regressionType">Regression Type</param>
        public void SetCentralRegressionOptions(int numXBins, int numYBins,
            int numJumps, double zTolerance, RegressionType regressionType)
        {
            CentralRegression.SetOptions(numXBins, numYBins, numJumps, zTolerance);
            m_regressionType = regressionType;
        }

        /// <summary>
        /// Sets the LSQ options
        /// </summary>
        /// <param name="numKnots">Number of knots</param>
        /// <param name="outlierZScore">Outlier Z Score</param>
        public void SetLSQOptions(int numKnots, double outlierZScore)
        {
            NaturalCubicSplineRegression.SetOptions(numKnots);
            LSQSplineRegression.SetOptions(numKnots);
            CentralRegression.OutlierZScore = outlierZScore;
        }

        /// <summary>
        /// TODO: Create comment block for CalculateRegressionFunction
        /// </summary>
        /// <param name="calibMatches"></param>
        public void CalculateRegressionFunction(List<RegressionPoints> calibMatches)
        {
            // TODO: Implement CalculateRegressionFunction
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for PrintRegressionFunction
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
    }
}