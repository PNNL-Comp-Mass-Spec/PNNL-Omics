using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MS.Internal.Xml.XPath;
using NUnit.Framework;
using PNNLOmics.Algorithms.Solvers;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt.BasisFunctions;
using PNNLOmics.Data;
using System.Reflection;

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
        public void SolveQuadraticFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculatedParabola(),out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.PolynomialQuadratic;

            BasisFunctionFactory functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;
            alglib.ndimensional_pfunc myDelegate = functionSelector.GetFunction;

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
                myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            Assert.AreEqual(-0.99999999960388553d, coeffs[0]);
            Assert.AreEqual(2.410211171560969E-10d, coeffs[1]);
            Assert.AreEqual(99.999999976322613d, coeffs[2]);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Sends a null list of UMC's to the clustering algorithm.")]
        public void SolveCubicFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculatedCubic(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.PolynomialCubic;

            BasisFunctionFactory functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;
            alglib.ndimensional_pfunc myDelegate = functionSelector.GetFunction;

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
                myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            Assert.AreEqual(-0.9999999999984106d, coeffs[0]);
            Assert.AreEqual(5.0000000000444658d, coeffs[1]);
            Assert.AreEqual(99.999999999930722d, coeffs[2]);
            Assert.AreEqual(24.999999997435527d, coeffs[3]);
        }

        /// <summary>
        /// 
        /// </summary>
        //[Test]
        [Description("Sends a null list of UMC's to the clustering algorithm.")]
        public void SolveCubicWithChebyshevFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculatedParabola(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.Chebyshev;

            BasisFunctionFactory functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;
            alglib.ndimensional_pfunc myDelegate = functionSelector.GetFunction;

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
                myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            //Assert.AreEqual(-0.9999999999984106d, coeffs[0]);
            //Assert.AreEqual(5.0000000000444658d, coeffs[1]);
            //Assert.AreEqual(99.999999999930722d, coeffs[2]);
            //Assert.AreEqual(24.999999997435527d, coeffs[3]);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Sends a null list of UMC's to the clustering algorithm.")]
        public void SolveLorentzianFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualLortentzianA(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.Lorentzian;

            BasisFunctionFactory functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;
            alglib.ndimensional_pfunc myDelegate = functionSelector.GetFunction;

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
                myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            Assert.AreEqual(0.49999999817701907d, coeffs[0]);//real is 0.5. 
            Assert.AreEqual(74.99999972887592d, coeffs[1]);//real is 75
            Assert.AreEqual(0.9999999999999587d, coeffs[2]);//real is 1
        }


        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Sends a null list of UMC's to the clustering algorithm.")]
        public void SolveGaussianFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualGaussian(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.Gaussian;

            BasisFunctionFactory functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;

            coeffs[0] = 6;//sigma
            coeffs[1] = 50;//height
            coeffs[2] = -1;//xoffset

            alglib.ndimensional_pfunc myDelegate = functionSelector.GetFunction;

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
                myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            Assert.AreEqual(0.50000000014842283d, Math.Abs(coeffs[0]));//real is 0.5.  may return a negative value
            Assert.AreEqual(99.999999955476071d, coeffs[1]);//real is 100
            Assert.AreEqual(0.99999999999999967d, coeffs[2]);//real is 1
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a quadratic line shape (legacy)")]
        public void SolveQuadratic()
        {
            double[] coeffs = new double[3];

            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculatedParabola(), out x, out y);


            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            QuadraticSolver quadSolver = new QuadraticSolver();

            alglib.ndimensional_pfunc myDelegate = quadSolver.QuadraticSolve;
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

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

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            GaussianSolver gaussianSolver = new GaussianSolver();

            alglib.ndimensional_pfunc myDelegate = gaussianSolver.GaussianSolve;
            solver.BasisFunction = myDelegate;

            double[] coeffs = new double[3];

            //guess
            coeffs[0] = 6;//sigma
            coeffs[1] = 50;//height
            coeffs[2] = -1;//xoffset

            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                gaussianSolver.GaussianSolve(coeffs, new double[] { xValue }, ref fitValue, null);

                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            //sigma must be positive

            Assert.AreEqual(0.50000000014842283d, Math.Abs(coeffs[0]));//real is 0.5.  may return a negative value
            Assert.AreEqual(99.999999955476071d, coeffs[1]);//real is 100
            Assert.AreEqual(0.99999999999999967d, coeffs[2]);//real is 1

        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a lorentzian line shape (legacy)")]
        public void SolveLorentzian()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualLortentzianA(), out x, out y);

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            LorentzianSolver lorentzianSolver = new LorentzianSolver();

            alglib.ndimensional_pfunc myDelegate = lorentzianSolver.LorentziannSolve;
            solver.BasisFunction = myDelegate;

            double[] coeffs = new double[3];
            //guess
            coeffs[0] = 6;//width
            coeffs[1] = 50;//height
            coeffs[2] = -1;//xoffset

            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                lorentzianSolver.LorentziannSolve(coeffs, new double[] { xValue }, ref fitValue, null);

                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            //sigma must be positive

            Assert.AreEqual(0.49999999817701907d, coeffs[0]);//real is 0.5. 
            Assert.AreEqual(74.99999972887592d, coeffs[1]);//real is 75
            Assert.AreEqual(0.9999999999999587d, coeffs[2]);//real is 1

        }


        private static void ConvertXYDataToArrays(List<PNNLOmics.Data.XYData> data, out List<double> x, out List<double> y)
        {
            x = new List<double>();
            y = new List<double>();
            foreach (var xyData in data)
            {
                x.Add(xyData.X);
                y.Add(xyData.Y);
            }
        }
 
        private static List<PNNLOmics.Data.XYData> ManualLortentzianB()
        {
            List<PNNLOmics.Data.XYData> manualData = new List<XYData>();
            manualData.Add(new XYData(-0.0219999999999345, 9.3977766913991E-16));
            manualData.Add(new XYData(-0.02150000000006, 2.39880902501257E-14));
            manualData.Add(new XYData(-0.0209999999999582, 5.36461868040639E-13));
            manualData.Add(new XYData(-0.0205000000000837, 1.05112574057507E-11));
            manualData.Add(new XYData(-0.0199999999999818, 1.80444278425828E-10));
            manualData.Add(new XYData(-0.0195000000001073, 2.713964676405E-09));
            manualData.Add(new XYData(-0.0190000000000055, 3.57633255625202E-08));
            manualData.Add(new XYData(-0.018500000000131, 4.12899381286205E-07));
            manualData.Add(new XYData(-0.0180000000000291, 4.17660302304908E-06));
            manualData.Add(new XYData(-0.0174999999999272, 3.70147478602181E-05));
            manualData.Add(new XYData(-0.0170000000000528, 0.000287408112628826));
            manualData.Add(new XYData(-0.0164999999999509, 0.00195522112375824));
            manualData.Add(new XYData(-0.0160000000000764, 0.0116537411913572));
            manualData.Add(new XYData(-0.0154999999999745, 0.0608565723566319));
            manualData.Add(new XYData(-0.0150000000001, 0.278433981159247));
            manualData.Add(new XYData(-0.0144999999999982, 1.11611672731448));
            manualData.Add(new XYData(-0.0140000000001237, 3.91985161576496));
            manualData.Add(new XYData(-0.0135000000000218, 12.0615262993972));
            manualData.Add(new XYData(-0.01299999999992, 32.5167805045449));
            manualData.Add(new XYData(-0.0125000000000455, 76.8042833615195));
            manualData.Add(new XYData(-0.0119999999999436, 158.94101186222));
            manualData.Add(new XYData(-0.0115000000000691, 288.176880819004));
            manualData.Add(new XYData(-0.0109999999999673, 457.77801283273));
            manualData.Add(new XYData(-0.0105000000000928, 637.123102780412));
            manualData.Add(new XYData(-0.00999999999999091, 776.898723053241));
            manualData.Add(new XYData(-0.00950000000011642, 830));
            manualData.Add(new XYData(-0.00900000000001455, 776.898723053147));
            manualData.Add(new XYData(-0.00849999999991269, 637.123102780258));
            manualData.Add(new XYData(-0.0080000000000382, 457.778012832564));
            manualData.Add(new XYData(-0.00749999999993634, 288.176880818865));
            manualData.Add(new XYData(-0.00700000000006185, 158.941011862124));
            manualData.Add(new XYData(-0.00649999999995998, 76.8042833614638));
            manualData.Add(new XYData(-0.00600000000008549, 32.5167805045173));
            manualData.Add(new XYData(-0.00549999999998363, 12.0615262993856));
            manualData.Add(new XYData(-0.00500000000010914, 3.9198516157607));
            manualData.Add(new XYData(-0.00450000000000728, 1.11611672731313));
            manualData.Add(new XYData(-0.00400000000013279, 0.278433981158876));
            manualData.Add(new XYData(-0.00350000000003092, 0.0608565723565436));
            manualData.Add(new XYData(-0.00299999999992906, 0.0116537411913389));
            manualData.Add(new XYData(-0.00250000000005457, 0.00195522112375493));
            manualData.Add(new XYData(-0.00199999999995271, 0.000287408112628304));
            manualData.Add(new XYData(-0.00150000000007822, 3.70147478601467E-05));
            manualData.Add(new XYData(-0.000999999999976353, 4.17660302304052E-06));
            manualData.Add(new XYData(-0.000500000000101863, 4.12899381285309E-07));
            manualData.Add(new XYData(0, 3.57633255624383E-08));
            manualData.Add(new XYData(0.00049999999987449, 2.71396467639845E-09));
            manualData.Add(new XYData(0.000999999999976353, 1.80444278425371E-10));
            manualData.Add(new XYData(0.00150000000007822, 1.05112574057228E-11));
            manualData.Add(new XYData(0.00199999999995271, 5.36461868039152E-13));
            manualData.Add(new XYData(0.00250000000005457, 2.39880902500561E-14));
            manualData.Add(new XYData(0.00299999999992906, 9.39777669137079E-16));

            return manualData;
        }

        private static List<PNNLOmics.Data.XYData> ManualLortentzianA()
        {
            List<PNNLOmics.Data.XYData> manualData = new List<XYData>();
            manualData.Add(new XYData(-1.5, 1.87241109519877));
            manualData.Add(new XYData(-1.4, 2.02831278366901));
            manualData.Add(new XYData(-1.3, 2.20436209268553));
            manualData.Add(new XYData(-1.2, 2.40415321891081));
            manualData.Add(new XYData(-1.1, 2.63211041497071));
            manualData.Add(new XYData(-1, 2.89372623803446));
            manualData.Add(new XYData(-0.9, 3.19588239140352));
            manualData.Add(new XYData(-0.8, 3.54728699313288));
            manualData.Add(new XYData(-0.7, 3.95907818636556));
            manualData.Add(new XYData(-0.6, 4.44566880144959));
            manualData.Add(new XYData(-0.5, 5.02594557132301));
            manualData.Add(new XYData(-0.4, 5.72499795294588));
            manualData.Add(new XYData(-0.3, 6.57665054098741));
            manualData.Add(new XYData(-0.2, 7.62723369449978));
            manualData.Add(new XYData(-0.0999999999999998, 8.94128893774693));
            manualData.Add(new XYData(1.94289029309402E-16, 10.6103295394597));
            manualData.Add(new XYData(0.1, 12.7664392854462));
            manualData.Add(new XYData(0.2, 15.6034257933231));
            manualData.Add(new XYData(0.3, 19.4091394014507));
            manualData.Add(new XYData(0.4, 24.611589137922));
            manualData.Add(new XYData(0.5, 31.8309886183791));
            manualData.Add(new XYData(0.6, 41.8828797610251));
            manualData.Add(new XYData(0.7, 55.5191661948472));
            manualData.Add(new XYData(0.8, 72.3431559508615));
            manualData.Add(new XYData(0.9, 88.4194128288308));
            manualData.Add(new XYData(1, 95.4929658551372));
            manualData.Add(new XYData(1.1, 88.4194128288307));
            manualData.Add(new XYData(1.2, 72.3431559508614));
            manualData.Add(new XYData(1.3, 55.5191661948471));
            manualData.Add(new XYData(1.4, 41.882879761025));
            manualData.Add(new XYData(1.5, 31.830988618379));
            manualData.Add(new XYData(1.6, 24.6115891379219));
            manualData.Add(new XYData(1.7, 19.4091394014506));
            manualData.Add(new XYData(1.8, 15.603425793323));
            manualData.Add(new XYData(1.9, 12.7664392854461));
            manualData.Add(new XYData(2, 10.6103295394597));
            manualData.Add(new XYData(2.1, 8.94128893774691));
            manualData.Add(new XYData(2.2, 7.62723369449976));
            manualData.Add(new XYData(2.3, 6.5766505409874));
            manualData.Add(new XYData(2.4, 5.72499795294587));
            manualData.Add(new XYData(2.5, 5.025945571323));
            manualData.Add(new XYData(2.6, 4.44566880144958));
            manualData.Add(new XYData(2.7, 3.95907818636555));
            manualData.Add(new XYData(2.8, 3.54728699313288));
            manualData.Add(new XYData(2.9, 3.19588239140352));
            manualData.Add(new XYData(3, 2.89372623803446));
            manualData.Add(new XYData(3.1, 2.6321104149707));
            manualData.Add(new XYData(3.2, 2.4041532189108));
            manualData.Add(new XYData(3.3, 2.20436209268553));
            manualData.Add(new XYData(3.4, 2.02831278366901));
            manualData.Add(new XYData(3.5, 1.87241109519877));


            return manualData;
        }

        private static List<PNNLOmics.Data.XYData> ManualGaussian()
        {
            List<PNNLOmics.Data.XYData> manualData = new List<XYData>();
            manualData.Add(new XYData(-1.5, 0.000372665317207867));
            manualData.Add(new XYData(-1.4, 0.000992950430585108));
            manualData.Add(new XYData(-1.3, 0.00254193465161993));
            manualData.Add(new XYData(-1.2, 0.00625215037748204));
            manualData.Add(new XYData(-1.1, 0.0147748360232034));
            manualData.Add(new XYData(-1, 0.0335462627902513));
            manualData.Add(new XYData(-0.9, 0.0731802418880474));
            manualData.Add(new XYData(-0.8, 0.153381067932447));
            manualData.Add(new XYData(-0.7, 0.308871540823677));
            manualData.Add(new XYData(-0.6, 0.597602289500596));
            manualData.Add(new XYData(-0.5, 1.11089965382423));
            manualData.Add(new XYData(-0.4, 1.98410947443703));
            manualData.Add(new XYData(-0.3, 3.40474547345994));
            manualData.Add(new XYData(-0.2, 5.61347628341338));
            manualData.Add(new XYData(-0.0999999999999998, 8.89216174593864));
            manualData.Add(new XYData(1.94289029309402E-16, 13.5335283236613));
            manualData.Add(new XYData(0.1, 19.7898699083615));
            manualData.Add(new XYData(0.2, 27.8037300453194));
            manualData.Add(new XYData(0.3, 37.53110988514));
            manualData.Add(new XYData(0.4, 48.6752255959972));
            manualData.Add(new XYData(0.5, 60.6530659712634));
            manualData.Add(new XYData(0.6, 72.6149037073691));
            manualData.Add(new XYData(0.7, 83.5270211411272));
            manualData.Add(new XYData(0.8, 92.3116346386636));
            manualData.Add(new XYData(0.9, 98.0198673306755));
            manualData.Add(new XYData(1, 100));
            manualData.Add(new XYData(1.1, 98.0198673306755));
            manualData.Add(new XYData(1.2, 92.3116346386635));
            manualData.Add(new XYData(1.3, 83.5270211411272));
            manualData.Add(new XYData(1.4, 72.614903707369));
            manualData.Add(new XYData(1.5, 60.6530659712633));
            manualData.Add(new XYData(1.6, 48.6752255959971));
            manualData.Add(new XYData(1.7, 37.5311098851399));
            manualData.Add(new XYData(1.8, 27.8037300453193));
            manualData.Add(new XYData(1.9, 19.7898699083614));
            manualData.Add(new XYData(2, 13.5335283236612));
            manualData.Add(new XYData(2.1, 8.89216174593859));
            manualData.Add(new XYData(2.2, 5.61347628341334));
            manualData.Add(new XYData(2.3, 3.40474547345991));
            manualData.Add(new XYData(2.4, 1.98410947443701));
            manualData.Add(new XYData(2.5, 1.11089965382422));
            manualData.Add(new XYData(2.6, 0.597602289500589));
            manualData.Add(new XYData(2.7, 0.308871540823674));
            manualData.Add(new XYData(2.8, 0.153381067932445));
            manualData.Add(new XYData(2.9, 0.0731802418880463));
            manualData.Add(new XYData(3, 0.0335462627902507));
            manualData.Add(new XYData(3.1, 0.0147748360232031));
            manualData.Add(new XYData(3.2, 0.00625215037748192));
            manualData.Add(new XYData(3.3, 0.00254193465161987));
            manualData.Add(new XYData(3.4, 0.000992950430585087));
            manualData.Add(new XYData(3.5, 0.000372665317207859));


            return manualData;
        }

        private static List<PNNLOmics.Data.XYData> CalculatedParabola()
        {
            List<PNNLOmics.Data.XYData> calculatedData = new List<XYData>();

            List<double> x = new List<double>();
            List<double> y = new List<double>();
            for (int i = -10; i < 10; i++)
            {
                double val = Convert.ToDouble(i);
                double xValue = val;
                double yValue = -(val * val) + 100;

                x.Add(xValue);
                y.Add(yValue);

                calculatedData.Add(new XYData(xValue,yValue));
            }
            return calculatedData;
        }

        private static List<PNNLOmics.Data.XYData> CalculatedCubic()
        {
            List<PNNLOmics.Data.XYData> calculatedData = new List<XYData>();

            List<double> x = new List<double>();
            List<double> y = new List<double>();
            for (int i = -10; i < 10; i++)
            {
                double val = Convert.ToDouble(i);
                double xValue = val;
                double yValue = -(val * val * val) + (5*val*val) + (100*val) + 25;

                x.Add(xValue);
                y.Add(yValue);

                calculatedData.Add(new XYData(xValue, yValue));
            }
            return calculatedData;
        }
    }     


}
