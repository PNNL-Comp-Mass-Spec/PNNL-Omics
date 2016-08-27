﻿using System;

namespace PNNLOmics.Algorithms.Solvers.LevenburgMarquadt.BasisFunctions
{
    [Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Solvers.LevenburgMarquadt.BasisFunctions")]
    public class OrbitrapFunction : BasisFunctionBase
    {
        /// <summary>
        /// Evalutates the second order chebyshev polynomials
        /// </summary>
        /// <param name="c">Set of coefficients</param>
        /// <param name="x">Input variables</param>
        /// <param name="functionResult">Returned sum value of your function</param>
        /// <param name="obj">?</param>
        public override  void FunctionDelegate(double[] c, double[] x, ref double functionResult, object obj)
        {
            //Y=ax^2 + bx + c
            functionResult = 0;

            var pi = 3.14159265358979;//Math.PI;

            var hanningI = c[0];
            var hanningK = c[1];
            //double syncPct = c[2];
           
            //double lorentzianW = c[0];
            //double lorentzianI = c[1];

            var xOffset = c[2];


            //double syncPct = 0.5;

            //functionResult = hanningI*(Math.Sin(2*pi*hanningK*(x[0] - xOffset))/(2*pi*hanningK*(x[0]-xOffset)) + 0.5*Math.Sin(2*pi*hanningK*(x[0]-xOffset) - pi)/(2*pi*hanningK*(x[0]-xOffset) - pi) + 0.5*Math.Sin(2*pi*hanningK*(x[0]-xOffset) + pi)/(2*pi*hanningK*(x[0]-xOffset) + pi))*syncPct + (1 - syncPct)*lorentzianI*1/pi*0.5*lorentzianW/(Math.Pow((x[0]-xOffset), 2) + 0.5*Math.Pow(lorentzianW, 2));
            
            functionResult = hanningI*(Math.Sin(2*pi*hanningK*(x[0] - xOffset))/(2*pi*hanningK*(x[0] - xOffset)) + 0.5*Math.Sin(2*pi*hanningK*(x[0] - xOffset) - pi)/(2*pi*hanningK*(x[0] - xOffset) - pi) + 0.5*Math.Sin(2*pi*hanningK*(x[0] - xOffset) + pi)/(2*pi*hanningK*(x[0] - xOffset) + pi));

            //functionResult = lorentzianI * 1 / pi * 0.5 * lorentzianW / (Math.Pow(x[0] - xOffset, 2) + 0.5 * Math.Pow(lorentzianW, 2));

            //functionResult = hanningI * (Math.Sin(2 * pi * hanningK * (x[0] - xOffset)) / (2 * pi * hanningK * (x[0] - xOffset)) + 0.5 * Math.Sin(2 * pi * hanningK * (x[0] - xOffset) - pi) / (2 * pi * hanningK * (x[0] - xOffset) - pi) + 0.5 * Math.Sin(2 * pi * hanningK * (x[0] - xOffset) + pi) / (2 * pi * hanningK * (x[0] - xOffset) + pi)) * syncPct + (1 - syncPct) * lorentzianI * 1 / pi * 0.5 * lorentzianW / (Math.Pow(x[0] - xOffset, 2) + 0.5 * Math.Pow(lorentzianW, 2));
        }
    }
}
