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
        LcmsCentralRegression Central;
        LCMSLSQSplineRegression LSQReg;
        LCMSNaturalCubicSplineRegression CubicSpline;

        /// <summary>
        /// Public constructor for a Hybrid regression
        /// </summary>
        public LcmsCombinedRegression()
        {
            m_regressionType = RegressionType.HYBRID;
            m_lsqFailed = false;
            Central = new LcmsCentralRegression();
            LSQReg = new LCMSLSQSplineRegression();
            CubicSpline = new LCMSNaturalCubicSplineRegression();
        }

        /// <summary>
        /// Sets the options for all three regression types, setting up the number of knots for
        /// cubic spline and LSQ while setting the outlier z score for central regression
        /// </summary>
        /// <param name="numKnots"></param>
        /// <param name="outlierZscore"></param>
        public void SetLsqOptions(int numKnots, double outlierZscore)
        {
            CubicSpline.SetOptions(numKnots);
            LSQReg.SetOptions(numKnots);
            Central.SetOutlierZScore(outlierZscore);
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
            Central.SetOptions(numXBins, numYBins, numJumps, regZtolerance);
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
                    Central.CalculateRegressionFunction(ref matches);
                    //mobj_central...line 47
                    break;
                default:
                    Central.CalculateRegressionFunction(ref matches);
                    Central.RemoveRegressionOutliers();
                    List<LcmsRegressionPts> centralPoints = Central.Points;
                    m_lsqFailed = !CubicSpline.CalculateLSQRegressionCoefficients(ref centralPoints);
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
                    return Central.GetPredictedValue(x);
                
                default:
                    if (!m_lsqFailed)
                    {
                        return CubicSpline.GetPredictedValue(x);
                    }
                    return Central.GetPredictedValue(x);
                    
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
