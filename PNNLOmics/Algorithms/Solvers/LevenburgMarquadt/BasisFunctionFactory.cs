using System;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt.BasisFunctions;

namespace PNNLOmics.Algorithms.Solvers.LevenburgMarquadt
{
    public class BasisFunctionFactory
    {
        private IBasisFunction FunctionDelegateInterface;

        public double[] Coefficients { get; set; }

        public BasisFunctionFactory(IBasisFunction strategy)
        {
            FunctionDelegateInterface = strategy;
            Coefficients = new double[0];
        }

        public void GetFunction(double[] c, double[] x, ref double functionResult, object obj)
        {
            FunctionDelegateInterface.FunctionDelegate(c, x,ref functionResult, obj);

        }

        public static BasisFunctionFactory BasisFunctionSelector(BasisFunctionsEnum functionChoise)
        {
            //default
            BasisFunctionFactory quadSolver = new BasisFunctionFactory(new Quadratic());
            
            switch (functionChoise)
            {
                case BasisFunctionsEnum.PolynomialQuadratic:
                    {
                        quadSolver = new BasisFunctionFactory(new Quadratic());
                        quadSolver.Coefficients = new double[3];
                        quadSolver.Coefficients[0] = 1;//ax^2
                        quadSolver.Coefficients[1] = 1;//bx
                        quadSolver.Coefficients[2] = 1;//c
                    }
                    break;
                case BasisFunctionsEnum.PolynomialCubic:
                    {
                        quadSolver = new BasisFunctionFactory(new Cubic());
                        quadSolver.Coefficients = new double[4];
                        quadSolver.Coefficients[0] = 1;//ax^3
                        quadSolver.Coefficients[1] = 1;//bx^2
                        quadSolver.Coefficients[2] = 1;//cx
                        quadSolver.Coefficients[3] = 1;//d
                    }
                    break;
                case BasisFunctionsEnum.Lorentzian:
                    {
                        quadSolver = new BasisFunctionFactory(new Lorentian());
                        quadSolver.Coefficients = new double[3];
                        quadSolver.Coefficients[0] = 6;//width
                        quadSolver.Coefficients[1] = 50;//height
                        quadSolver.Coefficients[2] = -1;//xoffset
                    }
                    break;
                case BasisFunctionsEnum.Gaussian:
                    {
                        quadSolver = new BasisFunctionFactory(new Gaussian());
                        quadSolver.Coefficients = new double[3];
                        quadSolver.Coefficients[0] = 6;//sigma
                        quadSolver.Coefficients[1] = 50;//height
                        quadSolver.Coefficients[2] = -1;//xoffset
                    }
                    break;
                case BasisFunctionsEnum.Chebyshev:
                    {
                        quadSolver = new BasisFunctionFactory(new Chebyshev());
                        quadSolver.Coefficients = new double[6];
                        quadSolver.Coefficients[0] = 1;//?
                        quadSolver.Coefficients[1] = 1;//?
                    }
                    break;
                case BasisFunctionsEnum.Orbitrap:
                    {
                        quadSolver = new BasisFunctionFactory(new OrbitrapFunction());
                        quadSolver.Coefficients = new double[3];
                        quadSolver.Coefficients[0] = 1;//?
                        quadSolver.Coefficients[1] = 1;//?
                        quadSolver.Coefficients[2] = 1;//?
                        //quadSolver.Coefficients[3] = 1;//?
                        //quadSolver.Coefficients[4] = 1;//?
                        //quadSolver.Coefficients[4] = 1;//?
                    }
                    break;
                case BasisFunctionsEnum.Hanning:
                    {
                        quadSolver = new BasisFunctionFactory(new Hanning());
                        quadSolver.Coefficients = new double[3];
                        quadSolver.Coefficients[0] = 1;//?
                        quadSolver.Coefficients[1] = 1;//?
                        quadSolver.Coefficients[2] = 1;//?
                    }
                    break;
                default:
                    Console.WriteLine("No Case Availible");
                    break;
            }
            return quadSolver;
        }
    }

    
}
