using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.Solvers
{
    public class LevenburgMarquadt
    {
        public LevenburgMarquadt()
        {
            DifferentialStep = 0.0001;            
            Epsilon          = 0.000001; 
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
