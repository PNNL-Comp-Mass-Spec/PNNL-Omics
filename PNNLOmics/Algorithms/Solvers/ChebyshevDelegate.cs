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
        public void SecondOrderSolve(double[] c, double[] x, ref double functionResult, object obj)
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
            functionResult = sum;
        }
    }

    /// <summary>
    /// Basis Function for the LM Algorithm using a parabola
    /// </summary>
    public class QuadraticSolver
    {
        public void QuadraticSolve(double[] c, double[] x, ref double functionResult, object obj)
        {
            double sum = 0;

            //foreach (double xValue in x)
            //{
            //    sum += (c[0] * xValue * xValue) + (c[1] * xValue) + c[2];
            //}

            sum = (c[0] * x[0] * x[0]) + (c[1] * x[0]) + c[2];
            functionResult = sum;
        }
    }

    /// <summary>
    /// Basis Function for the LM Algorithm using a Gaussian
    /// </summary>
    public class GaussianSolver
    {
        public void GaussianSolve(double[] c, double[] x, ref double functionResult, object obj)
        {
            //=a*1/(2*PI()*sigma*sigma)^0.5*EXP(-((X-Offset)^2)/2*sigma*sigma)
            //sum = height * 1 / Math.Pow((2.0 * pi * sigma * sigma), 0.5) * Math.Exp(0.5 * Math.Exp(-((x[0] - xOffset) * (x[0] - xOffset))) / (2.0 * sigma * sigma));

            functionResult = 0;

            double sigma = c[0];
            double height = c[1];
            double xOffset = c[2];

            //=a*EXP(-(X^2)/(2*sigma^2))
            functionResult = height * Math.Exp(-(Math.Pow((x[0] - xOffset), 2)) / (2.0 * sigma * sigma));
        }
    }

    /// <summary>
    /// Basis Function for the LM Algorithm using a Lorentzian or Cauchy
    /// </summary>
    public class LorentzianSolver
    {
        public void LorentziannSolve(double[] c, double[] x, ref double functionResult, object obj)
        {
            //=a*1/(2*PI()*sigma*sigma)^0.5*EXP(-((X-Offset)^2)/2*sigma*sigma)
            //sum = height * 1 / Math.Pow((2.0 * pi * sigma * sigma), 0.5) * Math.Exp(0.5 * Math.Exp(-((x[0] - xOffset) * (x[0] - xOffset))) / (2.0 * sigma * sigma));


            functionResult = 0;

            double pi = 3.14159265358979;//Math.PI;
            
            double width = c[0];
            double height = c[1];
            double xOffset = c[2];

            //=a*EXP(-(X^2)/(2*sigma^2))
            functionResult = height * 1 / pi * width / (Math.Pow(x[0] - xOffset, 2) + 0.5 * Math.Pow(width, 2));
        }
    }
}
