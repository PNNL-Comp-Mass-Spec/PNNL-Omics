using System.Collections.Generic;

namespace LCMS.Utilities
{
    class Interpolation
    {
        // used to temporarily store the spline coefficients
        private List<double> m_temp_spline = new List<double>();
        // vector to store the second derivatives at knot points of spline
        private List<double> m_Y2 = new List<double>();

        public void Spline(ref List<double> x, ref List<double> y, double yp1, double ypn)
        {
            m_temp_spline.Clear();
            int n = (int)x.Count;
            int i, k;
            double p, qn, sig, un;

            m_Y2.Capacity = n;

            if (yp1 > 0.99e30)
            {
                m_Y2[0] = 0.0;
                m_temp_spline.Add(0);
            }
            else
            {
                m_Y2[0] = -0.5f;
                m_temp_spline.Add((3.0f / (x[1] - x[0])) * ((y[1] - y[0]) / (x[1] - x[0]) - yp1));
            }
            //Generate second derivatives at internal points using recursive spline equations
            for (i = 1; i <= n - 2; i++)
            {
                sig = (x[i] - x[i - 1]) / (x[i + 1] - x[i - 1]);
                p = sig * m_Y2[i - 1] + 2.0;
                m_Y2[i] = (sig - 1.0) / p;
                m_temp_spline.Add((y[i + 1] - y[i]) / (x[i + 1] - x[i]) - (y[i] - y[i - 1]) / (x[i] - x[i - 1]));
                m_temp_spline[i] = (6.0 * m_temp_spline[i] / (x[i + 1] - x[i - 1]) - sig * m_temp_spline[i - 1]) / p;
            }
            if (ypn > 0.99e30)
            {
                qn = un = 0.0;
            }
            else
            {
                qn = 0.5;
                un = (3.0 / (x[n - 1] - x[n - 2])) * (ypn - (y[n - 1] - y[n - 2]) / (x[n - 1] - x[n - 2]));
            }
            m_Y2[n - 1] = (un - qn * m_temp_spline[n - 2]) / (qn * m_Y2[n - 2] + 1.0);
            for (k = n - 2; k >= 0; k--)
            {
                m_Y2[k] = m_Y2[k] * m_Y2[k + 1] + m_temp_spline[k];
            }
        }

        public double Splint(ref List<double> xa, ref List<double> ya, double x)
        {
            int n = xa.Count;
            int klo, khi, k;
            double h, b, a;

            klo = 0;
            khi = n - 1;

            //Binary search for khi and klo
            while (khi - klo > 1)
            {
                k = (khi + klo) >> 1;
                if (xa[k] > x)
                {
                    khi = k;
                }
                else
                {
                    klo = k;
                }
            }
            h = xa[khi] - xa[klo];
            if (h == 0.0)
            {
                return -1;
            }
            a = (xa[khi] - x) / h;
            b = (x - xa[klo]) / h;

            //cubic interpolation at x
            double y = a * ya[klo] + b * ya[khi] + ((a * a * a - a) * m_Y2[klo] + (b * b * b - b) * m_Y2[khi]) * (h * h) / 6.0;
            return y;
        }
    }
}
