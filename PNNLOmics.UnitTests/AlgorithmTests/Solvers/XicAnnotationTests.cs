using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt.BasisFunctions;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt;
using System.IO;
using PNNLOmics.Data;

namespace PNNLOmics.UnitTests.AlgorithmTests.Solvers
{

    public class XicAnnotationTests : SolverTestBase
    {
        internal class Xic
        {
            public Xic()
            {
                x = new List<double>();
                y = new List<double>();
            }
            public List<double> x;
            public List<double> y;
            public double mz;
            public int charge;
            public double score;
            public string scoreName;
            public int id;
            public Xic prevXic;
            public SolverReport report;
            public string reviewer;
        }

        private List<Xic> ReadXicDatabase(string path)
        {

            List<string> lines  = File.ReadAllLines(path).ToList();
            Xic xic             = new Xic();
            List<Xic> data = new List<Xic>();
            bool found = false;
            foreach (string line in lines)
            {
                if (line.Contains("id"))
                {
                    if (found)
                        data.Add(xic);
                    found       = true;
                    xic         = new Xic();
                    string [] splitLine = line.Split('\t');
                    xic.id      = Convert.ToInt32(splitLine[1]);                    
                    xic.charge  = Convert.ToInt32(splitLine[3]);
                    xic.x = new List<double>();
                    xic.y = new List<double>();
                }
                else if (line.Contains("score"))
                {
                    string[] splitLine = line.Split('\t');
                    xic.score = Convert.ToInt32(splitLine[1]);
                    xic.scoreName = splitLine[3];
                }
                else if (line.Contains("reviewer"))
                {
                    string[] splitLine  = line.Split('\t');
                    xic.reviewer          = splitLine[1];                    
                }
                else if (!line.Contains("mz"))
                {
                    string[] splitLine = line.Split('\t');
                    xic.x.Add(Convert.ToDouble(splitLine[1]));
                    xic.y.Add(Convert.ToDouble(splitLine[2]));
                    xic.mz = Convert.ToDouble(splitLine[0]);
                }
            }
            if (found) 
            {
                data.Add(xic);
            }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
       // [TestCase(@"..\..\..\TestFiles\annotatedXicDatabase.xic", BasisFunctionsEnum.Gaussian)]
        //[TestCase(@"..\..\..\TestFiles\annotatedXicDatabase.xic", BasisFunctionsEnum.Chebyshev)]
        [TestCase(@"..\..\..\TestFiles\annotatedXicDatabase.xic", BasisFunctionsEnum.AsymmetricGaussian)]
        [Description("Fits the XIC's for testing .")]
        public void SolveAsymmetricGaussianFactory(string path, BasisFunctionsEnum functionChoise)
        {
            List<Xic> data      = ReadXicDatabase(path);                        
            List<Xic> newData   = new List<Xic>();
            foreach (Xic xic in data)
            {

                BasisFunctionBase basisFunction = BasisFunctionFactory.BasisFunctionSelector(functionChoise);

                double[] coefficients = basisFunction.Coefficients;
                double start = xic.x.Min() - .5;
                double stop  = xic.x.Max() + .5;
                double A     = xic.y.Max();
                A += (A * .05);
                double width = Math.Abs(stop - start);

                coefficients[0] = start + ((stop - start) / 2);
                coefficients[1] = A;
                coefficients[2] = width * .5;
                coefficients[3] = coefficients[2];

                //Console.WriteLine("Xic {0}", xic.id); 
                //Console.WriteLine();

                for (int i = 0; i < xic.x.Count; i++)
                {
                   // Console.WriteLine("{0}\t{1}", xic.x[i], xic.y[i]);
                }
                SolverReport worked = null;
                Console.WriteLine();

                try
                {
                    worked = EvaluateFunction(xic.x, xic.y, basisFunction, ref coefficients);

                }
                catch (Exception)
                {
                    continue;
                }
            
                // First solve for the function
                LevenburgMarquadtSolver solver  = new LevenburgMarquadtSolver();
                
                int numberOfSamples = 100;

                // Calculate the width an spacing of each of the trapezoids.
                double delta = width / Convert.ToDouble(numberOfSamples);
                double x = start;

                Xic newXic = new Xic();
                newXic.x   = new List<double>();
                newXic.y   = new List<double>();
                newXic.charge       = xic.charge;
                newXic.scoreName    = xic.scoreName;
                newXic.score        = xic.score;
                newXic.id           = xic.id;
                newXic.prevXic      = xic;
                newXic.report       = worked;
                newXic.reviewer     = xic.reviewer;

                // We already evaluated the first point, now for each element within
                for (int i = 0; i < numberOfSamples + 1; i++)
                {
                    x += delta;
                    double y = basisFunction.Evaluate(coefficients, x);
                             
                    newXic.x.Add(x);
                    newXic.y.Add(y);
                   // Console.WriteLine("{0}\t{1}", x, y);
                } 
                //Console.WriteLine();

                newData.Add(newXic);                               
            }

            string newPath = path.Replace(".xic", functionChoise.ToString() + "_fit.xic");
            using (TextWriter writer = File.CreateText(newPath))
            {
                foreach (Xic feature in newData)
                {
                    writer.WriteLine("id\t{0}\tcharge\t{1}", feature.id, feature.charge);
                    writer.WriteLine("score\t{0}\tscoreName\t{1}", feature.score, feature.scoreName);
                    writer.WriteLine("reviewer\t{0}", feature.reviewer); 
                    writer.WriteLine("R2\t{0}", feature.report.RSquared);
                    writer.WriteLine("Rms\t{0}", feature.report.RmsError);
                    writer.WriteLine("Converged\t{0}", feature.report.DidConverge);
                    writer.WriteLine("mz\ttime\tintensity");

                    Xic prev = feature.prevXic;
                    for (int i = 0; i < feature.x.Count; i++)
                    {

                        string prevTime = "";
                        string prevInt  = "";
                        if (prev != null && i < prev.x.Count)
                        {
                            prevTime = prev.x[i].ToString();
                            prevInt = prev.y[i].ToString();
                        }

                        writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", feature.mz, feature.x[i], feature.y[i], prevTime, prevInt);
                    }
                    writer.WriteLine();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Test]
        [TestCase(@"..\..\..\TestFiles\annotatedXicDatabase.xic", BasisFunctionsEnum.Gaussian)]
        [Description("Fits the XIC's for testing .")]
        public void SolveGaussianFactory(string path, BasisFunctionsEnum functionChoise)
        {
            List<Xic> data = ReadXicDatabase(path);
            List<Xic> newData = new List<Xic>();
            foreach (Xic xic in data)
            {

                BasisFunctionBase basisFunction = BasisFunctionFactory.BasisFunctionSelector(functionChoise);

                double[] coefficients = basisFunction.Coefficients;
                double start = xic.x.Min() - .5;
                double stop = xic.x.Max() + .5;

                coefficients[2] = start + ((stop - start) / 2);
                coefficients[0] = coefficients[2] * .5;
                coefficients[1] = xic.y.Max();

                Console.WriteLine("Xic {0}", xic.id);
                Console.WriteLine();

                for (int i = 0; i < xic.x.Count; i++)
                {
                    Console.WriteLine("{0}\t{1}", xic.x[i], xic.y[i]);
                }
                SolverReport worked = null;
                Console.WriteLine();

                try
                {
                    worked = EvaluateFunction(xic.x, xic.y, basisFunction, ref coefficients);

                }
                catch (Exception)
                {
                    continue;
                }

                // First solve for the function
                LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();

                int numberOfSamples = 100;

                // Calculate the width an spacing of each of the trapezoids.
                double width = Math.Abs(stop - start);
                double delta = width / Convert.ToDouble(numberOfSamples);
                double x = start;

                Xic newXic = new Xic();
                newXic.x = new List<double>();
                newXic.y = new List<double>();
                newXic.charge = xic.charge;
                newXic.scoreName = xic.scoreName;
                newXic.score = xic.score;
                newXic.id = xic.id;
                newXic.prevXic = xic;
                newXic.report = worked;
                newXic.reviewer = xic.reviewer;

                // We already evaluated the first point, now for each element within
                for (int i = 0; i < numberOfSamples + 1; i++)
                {
                    x += delta;
                    double y = basisFunction.Evaluate(coefficients, x);

                    newXic.x.Add(x);
                    newXic.y.Add(y);
                    Console.WriteLine("{0}\t{1}", x, y);
                }
                Console.WriteLine();

                newData.Add(newXic);
            }

            string newPath = path.Replace(".xic", functionChoise.ToString() + "_fit.xic");
            using (TextWriter writer = File.CreateText(newPath))
            {
                foreach (Xic feature in newData)
                {
                    writer.WriteLine("id\t{0}\tcharge\t{1}", feature.id, feature.charge);
                    writer.WriteLine("score\t{0}\tscoreName\t{1}", feature.score, feature.scoreName);
                    writer.WriteLine("reviewer\t{0}", feature.reviewer);
                    writer.WriteLine("R2\t{0}", feature.report.RSquared);
                    writer.WriteLine("Rms\t{0}", feature.report.RmsError);
                    writer.WriteLine("Converged\t{0}", feature.report.DidConverge);
                    writer.WriteLine("mz\ttime\tintensity");

                    Xic prev = feature.prevXic;
                    for (int i = 0; i < feature.x.Count; i++)
                    {

                        string prevTime = "";
                        string prevInt = "";
                        if (prev != null && i < prev.x.Count)
                        {
                            prevTime = prev.x[i].ToString();
                            prevInt = prev.y[i].ToString();
                        }

                        writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", feature.mz, feature.x[i], feature.y[i], prevTime, prevInt);
                    }
                    writer.WriteLine();
                }
            }
        }
    }
}
