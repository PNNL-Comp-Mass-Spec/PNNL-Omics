using System.Collections.Generic;

namespace PNNLOmics.Alignment.LCMSWarp.LCMSWarper.LCMSRegression
{
    /// <summary>
    /// Object that holds instances of all three regression types, as well as providing a wrapper method for all three of the
    /// regression types that LCMS could use
    /// </summary>
    public class LcmsCombinedRegression
    {
        private RegressionType m_regressionType;
        private bool m_lsqFailed;
        readonly LcmsCentralRegression m_central;
        readonly LcmsLsqSplineRegression m_lsqReg;
        readonly LcmsNaturalCubicSplineRegression m_cubicSpline;

        /// <summary>
        /// Public constructor for a Hybrid regression
        /// </summary>
        public LcmsCombinedRegression()
        {
            m_regressionType = RegressionType.HYBRID;
            m_lsqFailed = false;
            m_central = new LcmsCentralRegression();
            m_lsqReg = new LcmsLsqSplineRegression();
            m_cubicSpline = new LcmsNaturalCubicSplineRegression();
        }

        /// <summary>
        /// Sets the options for all three regression types, setting up the number of knots for
        /// cubic spline and LSQ while setting the outlier z score for central regression
        /// </summary>
        /// <param name="numKnots"></param>
        /// <param name="outlierZscore"></param>
        public void SetLsqOptions(int numKnots, double outlierZscore)
        {
            m_cubicSpline.SetOptions(numKnots);
            m_lsqReg.SetOptions(numKnots);
            m_central.SetOutlierZScore(outlierZscore);
        }

        /// <summary>
        /// Sets all the options for a central regression type
        /// </summary>
        /// <param name="numXBins"></param>
        /// <param name="numYBins"></param>
        /// <param name="numJumps"></param>
        /// <param name="regZtolerance"></param>
        /// <param name="regType"></param>
        public void SetCentralRegressionOptions(int numXBins, int numYBins, int numJumps, double regZtolerance, RegressionType regType)
        {
            m_central.SetOptions(numXBins, numYBins, numJumps, regZtolerance);
            m_regressionType = regType;
        }

        /// <summary>
        /// Sets the regression points to the appropriate values for the regression function
        /// </summary>
        /// <param name="matches"></param>
        public void CalculateRegressionFunction(ref List<LcmsRegressionPts> matches)
        {
            switch (m_regressionType)
            {
                case RegressionType.CENTRAL:
                    m_central.CalculateRegressionFunction(ref matches);
                    //mobj_central...line 47
                    break;
                default:
                    m_central.CalculateRegressionFunction(ref matches);
                    m_central.RemoveRegressionOutliers();
                    List<LcmsRegressionPts> centralPoints = m_central.Points;
                    m_lsqFailed = !m_cubicSpline.CalculateLsqRegressionCoefficients(ref centralPoints);
                    //line 50
                    break;
            }
        }
        /// <summary>
        /// Given a value x, finds the appropriate y value that would correspond to it in the regression function
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double GetPredictedValue(double x)
        {
            switch (m_regressionType)
            {
                case RegressionType.CENTRAL:
                    return m_central.GetPredictedValue(x);
                
                default:
                    if (!m_lsqFailed)
                    {
                        return m_cubicSpline.GetPredictedValue(x);
                    }
                    return m_central.GetPredictedValue(x);
                    
            }
        }

        /// <summary>
        /// Enumeration for possible regression types for LCMS
        /// </summary>
        public enum RegressionType
        {
            /// <summary>
            /// Performs piecewise linear regression for all of the sections
            /// </summary>
            CENTRAL = 0,
            /// <summary>
            /// Performs an LSQ Regression for all the sections
            /// </summary>
            LSQ,
            /// <summary>
            /// Performs a combination of both regression types (Central and LSQ)
            /// </summary>
            HYBRID
        };
    }
}
