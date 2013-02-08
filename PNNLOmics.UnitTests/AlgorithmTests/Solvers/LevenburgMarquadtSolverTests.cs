using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PNNLOmics.Algorithms.Solvers;

namespace PNNLOmics.UnitTests.AlgorithmTests.Solvers
{

    [TestFixture]
    public class LevenburgMarquadtSolverTests
    {
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Sends a null list of UMC's to the clustering algorithm.")]
        public void SolveQuadratic()
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            double[] coeffs = new double[3];

            for (int i = -10; i < 10; i++)
            {
                double val = Convert.ToDouble(i);
                double xValue = val;
                double yValue = -(val * val) + 100;

                x.Add(xValue);
                y.Add(yValue);
            }

            LevenburgMarquadt solver    = new LevenburgMarquadt();
            QuadraticSolver quadSolver  = new QuadraticSolver();

            alglib.ndimensional_pfunc myDelegate = quadSolver.QuadraticSolve; 
            solver.BasisFunction        = myDelegate;
            bool worked                 = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                quadSolver.QuadraticSolve(coeffs, new double[] { xValue }, ref fitValue, null);

                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            Assert.AreEqual(-0.99999999959999375d, coeffs[0]);
            Assert.AreEqual(2.4338897338076459E-10d, coeffs[1]);
            Assert.AreEqual(99.999999976089995d, coeffs[2]); 

        }
    }     
}
