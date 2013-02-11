using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Solvers.LevenburgMarquadt
{
    public class LevenburgMarquadtSolver
    {
        public LevenburgMarquadtSolver()
        {
            DifferentialStep = 0.0001;            
            Epsilon          = 0.000001; 
        }

        public LevenburgMarquadtSolver(double differentialStep, double epsilon)
        {
            DifferentialStep = differentialStep;
            Epsilon = epsilon;
        }

        #region Properties
        /// <summary>
        /// Gets or sets the amount to step when differentiating.
        /// </summary>
        public double DifferentialStep
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the basis function to call that the LM algorithm is solving for.
        /// </summary>
        public alglib.ndimensional_pfunc BasisFunction
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the tolerance to know when the Least Squares has completed.
        /// </summary>
        public double Epsilon
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Least squares solver 
        /// </summary>
        /// <param name="baseline"></param>
        /// <param name="alignee"></param>
        /// <param name="coeffs"></param>
        /// <returns></returns>
        public bool Solve(List<double> baseline, List<double> alignee, ref double[] coeffs)
        {
            double epsf     = 0;
            int maxits      = 0; 
            int info        = 0;

            alglib.lsfitstate   state;
            alglib.lsfitreport  report;

            int         N   = baseline.Count;
            double[,]   x   = new double[N, 1];

            int         i = 0;
            foreach (double d in baseline)
            {
                x[i++, 0] = d;
            }

            alglib.lsfitcreatef(x,
                                alignee.ToArray(),
                                coeffs,
                                DifferentialStep,
                                out state);

            alglib.lsfitsetcond(state,
                                epsf,
                                Epsilon,
                                maxits);

            alglib.lsfitfit(state,
                            BasisFunction,
                            null,
                            null);


            alglib.lsfitresults(state,
                                out info, 
                                out coeffs, 
                                out report);

            // This is the flag in the ALGLIB library that says things were good for this kind of fit.
            return info == 2;
        }
    }
}
