using System.Collections.Generic;

namespace LCMS.Regression
{
    public class CombinedRegression
    {
        private RegressionType m_regressionType;
        private bool m_LSQ_Failed;
        CentralRegression Central;
        LSQSplineRegression LSQReg;
        NaturalCubicSplineRegression CubicSpline;

        public CombinedRegression()
        {
            m_regressionType = RegressionType.Hybrid;
            m_LSQ_Failed = false;
            Central = new CentralRegression();
            LSQReg = new LSQSplineRegression();
            CubicSpline = new NaturalCubicSplineRegression();
        }

        public void SetLSQOptions(int num_knots, double outlier_zscore)
        {
            CubicSpline.SetOptions(num_knots);
            LSQReg.SetOptions(num_knots);
            Central.SetOutlierZScore(outlier_zscore);
        }

        public void SetCentralRegressionOptions(int numXBins, int numYBins, int numJumps, double regZtolerance, RegressionType regType)
        {
            Central.SetOptions(numXBins, numYBins, numJumps, regZtolerance);
            m_regressionType = regType;
        }

        public void CalculateRegressionFunction(ref List<RegressionPts> matches)
        {
            switch (m_regressionType)
            {
                case RegressionType.Central:
                    Central.CalculateRegressionFunction(ref matches);
                    //mobj_central...line 47
                    break;
                default:
                    Central.CalculateRegressionFunction(ref matches);
                    Central.RemoveRegressionOutliers();
                    List<RegressionPts> CentralPoints = Central.Points();
                    m_LSQ_Failed = !CubicSpline.CalculateLSQRegressionCoefficients(ref CentralPoints);
                    //line 50
                    break;
            }
        }

        public double GetPredictedValue(double x)
        {
            switch (m_regressionType)
            {
                case RegressionType.Central:
                    return Central.GetPredictedValue(x);
                //break;
                default:
                    if (!m_LSQ_Failed)
                    {
                        return CubicSpline.GetPredictedValue(x);
                    }
                    else
                    {
                        return Central.GetPredictedValue(x);
                    }
                //break;
            }
            //return 0;
        }

        public void PrintRegressionFunction(string file_name)
        {
            switch (m_regressionType)
            {
                case RegressionType.Central:
                    //central_regression.PrintRegressionFunction(file_name);
                    break;
                default:
                    //lsq_regression.PrintRegressionFunction(file_name);
                    break;
            }
        }

        public enum RegressionType
        {
            Central = 0,
            LSQ,
            Hybrid
        };
    }
}
