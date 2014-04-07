using System;
using System.Collections.Generic;


namespace PNNLOmics.Alignment.LCMSWarp.LCMSWarper.LCMSUtilities
{
    class MathUtils
    {
        private const double El = 0.5772156649015329;

        public static void TwoDem(List<double> x, List<double> y, out double p, out double u, out double muX,
                           out double muY, out double stdX, out double stdY)
        {
            const int numIterations = 40;
            var numPoints = x.Count;
            var pVals = new double[2, numPoints];

            double minX = x[0], maxX = x[0];
            double minY = y[0], maxY = y[0];

            for (int pointNumber = 0; pointNumber < numPoints; pointNumber++)
            {
                if (x[pointNumber] < minX)
                {
                    minX = x[pointNumber];
                }
                if (x[pointNumber] > maxX)
                {
                    maxX = x[pointNumber];
                }
                if (y[pointNumber] < minY)
                {
                    minY = y[pointNumber];
                }
                if (y[pointNumber] > maxY)
                {
                    maxY = y[pointNumber];
                }
            }

            u = 1.0 / ((maxX - minX) * (maxY - minY));
            p = 0.5;

            CalcMeanAndStd(x, out muX, out stdX);
            stdX = stdX / 3.0;
            CalcMeanAndStd(y, out muY, out stdY);
            stdY = stdY / 3.0;

            for (int iterNum = 0; iterNum < numIterations; iterNum++)
            {
                // Calculate current probability assignments
                for (int pointNum = 0; pointNum < numPoints; pointNum++)
                {
                    double xDiff = (x[pointNum] - muX) / stdX;
                    double yDiff = (y[pointNum] - muY) / stdY;
                    pVals[0, pointNum] = p * Math.Exp(-0.5 * (xDiff * xDiff + yDiff * yDiff)) / (2 * Math.PI * stdX * stdY);
                    pVals[1, pointNum] = (1 - p) * u;
                    double sum = pVals[0, pointNum] + pVals[1, pointNum];
                    pVals[0, pointNum] = pVals[0, pointNum] / sum;
                    pVals[1, pointNum] = pVals[1, pointNum] / sum;
                }

                // Calculates new estimates from maximization step
                double pNumerator = 0;
                double muXNumerator = 0;
                double muYNumerator = 0;
                double sigmaXNumerator = 0;
                double sigmaYNumerator = 0;

                double pDenominator = 0;
                double denominator = 0;

                for (int pointNum = 0; pointNum < numPoints; pointNum++)
                {
                    pNumerator = pNumerator + pVals[0, pointNum];
                    pDenominator = pDenominator + (pVals[0, pointNum] + pVals[1, pointNum]);

                    double xDiff = (x[pointNum] - muX);
                    muXNumerator = muXNumerator + pVals[0, pointNum] * x[pointNum];
                    sigmaXNumerator = sigmaXNumerator + pVals[0, pointNum] * xDiff * xDiff;

                    double yDiff = (y[pointNum] - muY);
                    muYNumerator = muYNumerator + pVals[0, pointNum] * y[pointNum];
                    sigmaYNumerator = sigmaYNumerator + pVals[0, pointNum] * yDiff * yDiff;

                    denominator = denominator + pVals[0, pointNum];
                }

                muX = muXNumerator / denominator;
                muY = muYNumerator / denominator;
                stdX = Math.Sqrt(sigmaXNumerator / denominator);
                stdY = Math.Sqrt(sigmaYNumerator / denominator);
                p = pNumerator / pDenominator;
            }
        }

        static void CalcMeanAndStd(List<double> values, out double mean, out double stdev)
        {
            int numPoints = values.Count;
            double sumSquare = 0;
            double sum = 0;
            for (int pointNum = 0; pointNum < numPoints; pointNum++)
            {
                double val = values[pointNum];
                sum = sum + val;
                sumSquare = sumSquare + (val * val);
            }
            mean = sum / numPoints;
            stdev = Math.Sqrt((numPoints * sumSquare - sum * sum)) / (Math.Sqrt(numPoints) * Math.Sqrt(numPoints - 1));
        }

        public static double Psi(double x)
        {
            double s = 0, ps;
            int n, k;
            double[] a = {  -0.8333333333333e-01,
                             0.83333333333333333e-02,
                             -0.39682539682539683e-02,
                             0.41666666666666667e-02,
                             -0.75757575757575758e-02,
                             0.21092796092796093e-01,
                             -0.83333333333333333e-01,
                             0.4432598039215686};

            double xa = Math.Abs(x);
            if ((Math.Abs(x - (int)x) < double.Epsilon) && (x <= 0))
            {
                ps = 1e308;
                return ps;
            }
            if (Math.Abs(xa - (int)xa) < double.Epsilon)
            {
                n = Convert.ToInt32(xa);
                for (k = 1; k < n; k++)
                {
                    s += 1.0 / k;
                }
                ps = 2.0 * s - El;
            }
            else if (Math.Abs((xa + 0.5) - ((int)(xa + 0.5))) < double.Epsilon)
            {
                n = Convert.ToInt32(xa + 0.5);
                for (k = 1; k < n; k++)
                {
                    s += 1.0 / (2.0 * k - 1.0);
                }
                ps = 2.0 * s - El - 1.386294361119891;
            }
            else
            {
                if (xa < 10)
                {
                    n = 10 - (int)xa;
                    for (k = 0; k < n; k++)
                    {
                        s += 1.0 / (xa + k);
                    }
                    xa += n;
                }
                double x2 = 1.0 / (xa * xa);
                ps = Math.Log(xa) - 0.5 / xa + x2 * (((((((a[7] * x2 + a[6]) * x2 + a[5]) * x2 +
                a[4]) * x2 + a[3]) * x2 + a[2]) * x2 + a[1]) * x2 + a[0]);
                ps -= s;
            }
            if (x < 0)
            {
                ps = ps - Math.PI * Math.Cos(Math.PI * x) / (Math.Sin(Math.PI * x) - 1.0 / x);
            }
            return ps;
        }

        public static double InvPsi(double y)
        {
            //Using Algorithm from Paul Fackler
            double l = 1.0;
            double x = Math.Exp(y);
            while (l > 10e-8)
            {
                x = x + l * Math.Sign(y - Psi(x));
                l = l / 2.0;
            }
            return x;
        }
    }
}
