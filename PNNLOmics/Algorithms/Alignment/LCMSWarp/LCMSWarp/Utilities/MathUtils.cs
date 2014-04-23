using System;
using System.Collections.Generic;

namespace LCMS.Utilities
{
    class MathUtils
    {
        readonly static double EL = 0.5772156649015329;

        public static void TwoDEM(ref List<double> x, ref List<double> y, ref double p, ref double u, ref double muX,
                           ref double muY, ref double stdX, ref double stdY)
        {
            const int NumIterations = 40;
            int numPoints = x.Count;
            double[,] pVals = new double[2, numPoints];

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

            CalcMeanAndStd(ref x, ref muX, ref stdX);
            stdX = stdX / 3.0;
            CalcMeanAndStd(ref y, ref muY, ref stdY);
            stdY = stdY / 3.0;

            for (int iterNum = 0; iterNum < NumIterations; iterNum++)
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

        static void CalcMeanAndStd(ref List<double> values, ref double mean, ref double stdev)
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

        static double PValGamma(double x, double alpha, double beta)
        {
            if (x < 0)
            {
                x = 0;
            }
            double gammaAlpha = gamma(alpha);
            double PvalGamma = Math.Pow(x, alpha - 1);
            double expPortion = Math.Exp((-1 * x) / beta);
            PvalGamma *= expPortion;
            double denom = (Math.Pow(beta, alpha) * gammaAlpha);
            PvalGamma /= denom;
            return PvalGamma;
        }

        private static double gamma(double x)
        {
            int i, k, m;
            double ga, gr, z;
            double r = 1.0;

            double[] g =  {1.0,
			               0.5772156649015329,
                           -0.6558780715202538,
                           -0.420026350340952e-1,
                           0.1665386113822915,
                           -0.421977345555443e-1,
                           -0.9621971527877e-2,
                           0.7218943246663e-2,
                           -0.11651675918591e-2,
                           -0.2152416741149e-3,
                           0.1280502823882e-3,
                           -0.201348547807e-4,
                           -0.12504934821e-5,
                           0.1133027232e-5,
                           -0.2056338417e-6,
                           0.6116095e-8,
                           0.50020075e-8,
                           -0.11812746e-8,
                           0.1043427e-9,
                           0.77823e-11,
                           -0.36968e-11,
                           0.51e-12,
                           -0.206e-13,
                           -0.54e-14,
                           0.14e-14};

            if (x > 171.0)
            {
                return 1e308; // Overflow flag
            }
            if (x == (int)x)
            {
                if (x > 0.0)
                {
                    ga = 1.0;
                    for (i = 2; i < x; i++)
                    {
                        ga *= i;
                    }
                }
                else
                {
                    ga = 1e308;
                }
            }
            else
            {
                if (Math.Abs(x) > 1.0)
                {
                    z = Math.Abs(x);
                    m = (int)z;
                    for (k = 1; k <= m; k++)
                    {
                        r *= (z - k);
                    }
                    z -= m;
                }
                else
                {
                    z = x;
                }
                gr = g[24];
                for (k = 23; k >= 0; k--)
                {
                    gr = gr * z + g[k];
                }
                ga = 1.0 / (gr * z);
                if (Math.Abs(x) > 1.0)
                {
                    ga *= r;
                    if (x < 0.0)
                    {
                        ga = -(Math.PI) / (x * ga * Math.Sin(Math.PI * x));
                    }
                }
            }
            return ga;
        }

        public static double psi(double x)
        {
            double s = 0, ps = 0, xa = 0, x2 = 0;
            int n = 0, k = 1;
            double[] a = {  -0.8333333333333e-01,
                             0.83333333333333333e-02,
                             -0.39682539682539683e-02,
                             0.41666666666666667e-02,
                             -0.75757575757575758e-02,
                             0.21092796092796093e-01,
                             -0.83333333333333333e-01,
                             0.4432598039215686};

            xa = Math.Abs(x);
            if ((x == (int)x) && (x <= 0))
            {
                ps = 1e308;
                return ps;
            }
            if (xa == (int)xa)
            {
                n = Convert.ToInt32(xa);
                for (k = 1; k < n; k++)
                {
                    s += 1.0 / k;
                }
                ps = 2.0 * s - EL;
            }
            else if ((xa + 0.5) == ((int)(xa + 0.5)))
            {
                n = Convert.ToInt32(xa + 0.5);
                for (k = 1; k < n; k++)
                {
                    s += 1.0 / (2.0 * k - 1.0);
                }
                ps = 2.0 * s - EL - 1.386294361119891;
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
                x2 = 1.0 / (xa * xa);
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

        public static double invPsi(double y)
        {
            //Using Algorithm from Paul Fackler
            double L = 1.0;
            double x = Math.Exp(y);
            while (L > 10e-8)
            {
                x = x + L * Math.Sign(y - psi(x));
                L = L / 2.0;
            }
            return x;
        }
    }
}
