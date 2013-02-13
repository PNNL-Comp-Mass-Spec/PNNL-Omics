using System;
using System.Collections.Generic;
using NUnit.Framework;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt.BasisFunctions;
using PNNLOmics.Data;
using PNNLOmics.Algorithms.Solvers;

namespace PNNLOmics.UnitTests.AlgorithmTests.Solvers
{
    [TestFixture]
    public class NumericalIntegrationTests : SolverTestBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a quadratic line shape.")]
        public void IntegrateLineFunction()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculateLine(), out x, out y);

            BasisFunctionBase functionSelector = BasisFunctionFactory.BasisFunctionSelector(BasisFunctionsEnum.Linear);
            double[] coeffs = EvaluateFunction(x, y, functionSelector);

            NumericalIntegrationBase integrator = new TrapezoidIntegration();
            double area = integrator.Integrate(functionSelector, coeffs, 0, 3, 500);

            Console.WriteLine("Area: {0}", area);
            Assert.AreEqual(22.5, area, .1);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a quadratic line shape.")]
        public void IntegrateLineFunctionTimeTrial()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculateLine(), out x, out y);

            BasisFunctionBase functionSelector = BasisFunctionFactory.BasisFunctionSelector(BasisFunctionsEnum.Linear);
            double[] coeffs = EvaluateFunction(x, y, functionSelector);

            NumericalIntegrationBase integrator = new TrapezoidIntegration();

            Console.WriteLine("");
            Console.WriteLine("Samples\tTime(ms)\tArea\tPercentError");

            for (int i = 0; i < 100; i++)
            {
                List<double> averageTimes   = new List<double>();
                double sum                  = 0;
                int iterations              = 1000;
                int totalSamples            = i*100;
                double area                 = 0;
                for(int j = 0; j < iterations; j++)
                {
                    DateTime start  = DateTime.Now;
                    area            = integrator.Integrate(functionSelector, coeffs, 0, 3, totalSamples);
                    DateTime end    = DateTime.Now;
                    sum += end.Subtract(start).TotalMilliseconds;
                }
                double percentError = (area - 22.5)/22.5*100;
                double averageTime = sum / Convert.ToDouble(iterations);
                Console.WriteLine("{0}\t{1}\t{2}\t{3}", totalSamples, averageTime, area, percentError);                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a Hanning line shape.")]
        public void SolveHanningFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualHanning(), out x, out y);

            BasisFunctionBase functionSelector = BasisFunctionFactory.BasisFunctionSelector(BasisFunctionsEnum.Hanning);
            double[] coeffs = functionSelector.Coefficients;

            //important guesses
            coeffs[0] = 30;//hanningI
            coeffs[1] = 5;//hanningK
            coeffs[2] = 1234.388251;//xoffset
            coeffs = EvaluateFunction(x, y, functionSelector);

            Assert.AreEqual(Math.Round(30.521054724721569d, 7), Math.Round(coeffs[0], 7), .00001);
            Assert.AreEqual(Math.Round(37.723968728457208d, 6), Math.Round(coeffs[1], 6), .00001);
            Assert.AreEqual(Math.Round(1234.4579999999935d, 7), Math.Round(coeffs[2], 7), .00001);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a quadratic line shape.")]
        public void SolveOrbitrapLorentzianFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualOrbitrap2(), out x, out y);

            BasisFunctionBase functionSelector = BasisFunctionFactory.BasisFunctionSelector(BasisFunctionsEnum.Lorentzian);

            double[] coeffs = functionSelector.Coefficients;
            //important guesses
            coeffs[0] = 5;//hanningI
            coeffs[1] = 80000;//hanningK
            coeffs[2] = 1234.388251;//xoffset

            coeffs = EvaluateFunction(x, y, functionSelector);

            Assert.AreEqual(0.014591732782157337d, coeffs[0], .001);
            Assert.AreEqual(41816.913857810927d, coeffs[1], .001);
            Assert.AreEqual(1234.4577771195013d, coeffs[2], .001);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a cubic line shape.")]
        public void SolveCubicFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculatedCubic(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.PolynomialCubic;
            BasisFunctionBase functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = EvaluateFunction(x, y, functionSelector);

            Assert.AreEqual(-0.9999999999984106d, coeffs[0], .00001);
            Assert.AreEqual(5.0000000000444658d, coeffs[1], .00001);
            Assert.AreEqual(99.999999999930722d, coeffs[2], .00001);
            Assert.AreEqual(24.999999997435527d, coeffs[3], .00001);
        }

        /// <summary>
        /// 
        /// </summary>
        //[Test]
        [Description("Tests the Levenburg Marquadt solver using Chebyshev polynomials.")]
        public void SolveCubicWithChebyshevFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculatedParabola(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.Chebyshev;

            BasisFunctionBase functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = EvaluateFunction(x, y, functionSelector);

            //Assert.AreEqual(-0.9999999999984106d, coeffs[0]);
            //Assert.AreEqual(5.0000000000444658d, coeffs[1]);
            //Assert.AreEqual(99.999999999930722d, coeffs[2]);
            //Assert.AreEqual(24.999999997435527d, coeffs[3]);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a lorentzian line shape.")]
        public void SolveLorentzianFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualLortentzianA(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.Lorentzian;

            BasisFunctionBase functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;

            coeffs[0] = 6;//width
            coeffs[1] = 50;//height
            coeffs[2] = -1;//xoffset            
            coeffs = EvaluateFunction(x, y, functionSelector);

            Assert.AreEqual(0.50000000000535016d, coeffs[0]);//real is 0.5. 
            Assert.AreEqual(150.00000000174555d, coeffs[1]);//real is 75
            Assert.AreEqual(0.99999999999999312d, coeffs[2]);//real is 1

            //using 1 instead of 0.5
            //Assert.AreEqual(0.49999999817701907d, coeffs[0]);//real is 0.5. 
            //Assert.AreEqual(74.99999972887592d, coeffs[1]);//real is 75
            //Assert.AreEqual(0.9999999999999587d, coeffs[2]);//real is 1
        }


        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a gaussian line shape.")]
        public void SolveGaussianFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualGaussian(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.Gaussian;

            BasisFunctionBase functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;

            coeffs[0] = 6;//sigma
            coeffs[1] = 50;//height
            coeffs[2] = -1;//xoffset            
            coeffs = EvaluateFunction(x, y, functionSelector);

            Assert.AreEqual(0.50000000014842283d, Math.Abs(coeffs[0]), .00001);//real is 0.5.  may return a negative value
            Assert.AreEqual(99.999999955476071d, coeffs[1], .00001);//real is 100
            Assert.AreEqual(0.99999999999999967d, coeffs[2], .00001);//real is 1
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a quadratic line shape (legacy)")]
        public void SolveQuadratic()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculatedParabola(), out x, out y);

            BasisFunctionBase functionSelector = BasisFunctionFactory.BasisFunctionSelector(BasisFunctionsEnum.PolynomialQuadratic);
            double[] coeffs = EvaluateFunction(x, y, functionSelector);

            Assert.AreEqual(-0.99999999959999375d, coeffs[0], .00001);
            Assert.AreEqual(2.4338897338076459E-10d, coeffs[1], .00001);
            Assert.AreEqual(99.999999976089995d, coeffs[2], .00001);

        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a gaussian line shape (legacy)")]
        public void SolveGaussian()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualGaussian(), out x, out y);

            BasisFunctionBase functionSelector = BasisFunctionFactory.BasisFunctionSelector(BasisFunctionsEnum.Gaussian);
            double[] coeffs = EvaluateFunction(x, y, functionSelector);

            //sigma must be positive
            Assert.AreEqual(0.50000000014842283d, Math.Abs(coeffs[0]), .00001);//real is 0.5.  may return a negative value
            Assert.AreEqual(99.999999955476071d, coeffs[1], .00001);//real is 100
            Assert.AreEqual(0.99999999999999967d, coeffs[2], .00001);//real is 1

        }
    }     
}