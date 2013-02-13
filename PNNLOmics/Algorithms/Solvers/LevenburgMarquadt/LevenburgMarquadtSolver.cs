using System.Collections.Generic;
using System;

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
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="coeffs">Guess at coefficients</param>
        /// <returns>True if solved, False if error</returns>
        public SolverReport Solve(List<double> x, List<double> y, ref double[] coeffs)
        {
            double epsf     = 0;
            int maxits      = 0; 
            int info        = 0;

            alglib.lsfitstate   state;
            alglib.lsfitreport  report;

            int         N   = x.Count;
            double[,]   xMatrix   = new double[N, 1];

            int         i = 0;
            foreach (double d in x)
            {
                xMatrix[i++, 0] = d;
            }

            alglib.lsfitcreatef(xMatrix,
                                y.ToArray(),
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

            //Info    -   completion code:
            //        * -7    gradient verification failed.
            //                See LSFitSetGradientCheck() for more information.
            //        *  1    relative function improvement is no more than
            //                EpsF.
            //        *  2    relative step is no more than EpsX.
            //        *  4    gradient norm is no more than EpsG
            //        *  5    MaxIts steps was taken
            //        *  7    stopping conditions are too stringent,
            //                further improvement is impossible

            bool converged = (info >= 1 && info < 5);

            // This is the flag in the ALGLIB library that says things were good for this kind of fit.
            SolverReport solverReport = new SolverReport(report, converged);

            return solverReport;
        }
    }

    /// <summary>
    /// Contains information about the Levenburg-Marquadt execution.
    /// </summary>
    public class SolverReport
    {        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="report">Report from algorithm</param>
        /// <param name="didConverge"></param>
        public SolverReport(alglib.lsfitreport report, bool didConverge)
        {
            AverageError     = report.avgerror;
            DidConverge      = didConverge;
            IterationCount   = report.iterationscount;
            MaxError         = report.maxerror;
            PerPointNoise    = report.noise;
            RmsError         = report.rmserror;
            RSquared         = report.r2;
            WeightedRmsError = report.wrmserror;
        }
        /// <summary>
        /// Gets the average error.
        /// </summary>
        public double AverageError
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the number of iterations the software took to converge.
        /// </summary>
        public int IterationCount
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the maximum error 
        /// </summary>
        public double MaxError 
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the RMS value
        /// </summary>
        public double RmsError
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the weighted RMS value 
        /// </summary>
        public double WeightedRmsError
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the noise per point.
        /// </summary>
        public double[] PerPointNoise
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the flag indicating whether the algorithm converged.
        /// </summary>
        public bool DidConverge
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the R-squared value for the fit.
        /// </summary>
        public double RSquared
        {
            get;
            private set;
        }        
    }
}
