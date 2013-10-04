using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt.BasisFunctions;

namespace PNNLOmics.Algorithms.Regression
{
    public class AlglibRegression: IRegression<double>
    {
        public FitReport Fit(IEnumerable<double> x, IEnumerable<double> y, BasisFunctionsEnum basisFunction, out double[] coeffs)
        {
           
            BasisFunctionBase functionSelector = BasisFunctionFactory.BasisFunctionSelector(BasisFunctionsEnum.Linear);
            coeffs = functionSelector.Coefficients;
            SolverReport worked = EvaluateFunction(x.ToList(), y.ToList(), functionSelector, ref coeffs);

            FitReportALGLIB results = new FitReportALGLIB(worked, worked.DidConverge);
            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="initialCoefficients"></param>
        /// <param name="functionChoise"></param>
        /// <returns></returns>
        protected SolverReport EvaluateFunction(List<double> x, List<double> y, BasisFunctionBase basisFunction, ref double[] coeffs)
        {
            coeffs = basisFunction.Coefficients;
            basisFunction.Scale(x);

            alglib.ndimensional_pfunc myDelegate = basisFunction.FunctionDelegate;
            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            SolverReport worked = solver.Solve(x, y, ref coeffs);
            
            //for (int i = 0; i < x.Count; i++)
            //{

            //    // This is what we are fitting 
            //    double xValue = x[i];

            //    // This is what it should fit to
            //    double yValue = y[i];

            //    // This is the warped guy
            //    double fitValue = 0;
            //    //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
            //    myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
            //}
            //Console.WriteLine(Environment.NewLine);
            return worked;
        }
    }
}
