using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.Solvers
{

    /// <summary>
    /// Basis function for the LM Algorithm using First Order Chebyshev
    /// </summary>
    public class ChebyshevSolver
    {
        /// <summary>
        /// Evalutates the second order chebyshev polynomials
        /// </summary>
        /// <param name="c">Set of coefficients</param>
        /// <param name="x">Input variables</param>
        /// <param name="func">Returned sum value of your function</param>
        /// <param name="obj">?</param>
        public void SecondOrderSolve(double[] c, double[] x, ref double func, object obj)
        {
            double sum = 0;
            double t0 = c[0];
            double t1 = c[1] * x[0];
            double prev = t0;
            for (int i = 0; i < c.Length - 1; i++)
            {
                double value = 2 * x[0] * c[i] - prev;
                prev = value;
                sum += value;
            }
            func = sum;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QuadraticSolver
    {

        public void QuadraticSolve(double[] c, double[] x, ref double func, object obj)
        {
            double sum = 0;

            foreach (double xValue in x)
            {
                sum += (c[0] * xValue * xValue) + (c[1] * xValue) + c[2];
            }

            func = sum;
        }
    }
}
