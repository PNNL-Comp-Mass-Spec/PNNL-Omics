using System;

namespace PNNLOmics.Algorithms.Solvers.LevenburgMarquadt.BasisFunctions
{
    /// <summary>
    /// Basis function for the LM Algorithm using Lorentzian Peak Shapes
    /// </summary>
    public class Lorentian : IBasisFunctionInterface
    {
        /// <summary>
        /// Evalutates the second order chebyshev polynomials
        /// </summary>
        /// <param name="c">Set of coefficients</param>
        /// <param name="x">Input variables</param>
        /// <param name="functionResult">Returned sum value of your function</param>
        /// <param name="obj">?</param>
        public void FunctionDelegate(double[] c, double[] x, ref double functionResult, object obj)
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
