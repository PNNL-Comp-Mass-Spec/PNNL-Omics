using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;
using PNNLOmics.Data;
using PNNLOmics.Extensions;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt;


namespace PNNLOmics.Algorithms.FeatureMetrics
{
    /// <summary>
    /// Scores chromatograms based on various relationships between charge state and isotopic distribution.
    /// </summary>
    public class ChromatogramMetrics
    {
        public void FitChromatograms(Chromatogram profile,
                                     BasisFunctionBase basisFunction)
        {                        
            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = basisFunction.FunctionDelegate;

            double[] coeffs = basisFunction.Coefficients;
            SolverReport report = solver.Solve(profile.Points, ref coeffs);

            double minScan      = profile.Points.Min(x => x.X);
            double maxScan      = profile.Points.Max(x => x.X);

            double scanRange    = Math.Abs(maxScan - minScan);
            double deltaScan    = scanRange / (scanRange * 4);
            double scan         = minScan;

            List<XYData> fitPoints = new List<XYData>();
            while (scan <= maxScan)
            {
                double y = basisFunction.Evaluate(coeffs, scan);                
                fitPoints.Add(new XYData(scan, y));
                scan += deltaScan;
            }
            profile.FitCoefficients = coeffs;
            profile.FitPoints       = fitPoints;
            profile.FitReport       = report;            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="basisFunction"></param>
        public void FitChromatograms(UMCLight feature,
                                    BasisFunctionBase basisFunction)
        {
            foreach (int charge in feature.ChargeStateChromatograms.Keys)
            {
                Chromatogram gram = feature.ChargeStateChromatograms[charge];                
                FitChromatograms(gram, basisFunction);                
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="features"></param>
        /// <param name="basisFunction"></param>
        public void FitChromatograms(List<UMCLight> features, BasisFunctionBase basisFunction)
        {
            features.ForEach(x => FitChromatograms(x, basisFunction));
        }
        /// <summary>
        /// Scores two chromatograms using their basis functions.
        /// </summary>
        /// <param name="profileA">chromatogram A</param>
        /// <param name="profileB">chromatogram B</param>
        /// <param name="basisFunction">Function used to interpolate</param>        
        /// <param name="intensityProfile"></param>
        /// <returns>R-squared value for a given linear regression between intensity matrix</returns>
        public double ScoreChromatogramIntensity(Chromatogram profileA,
                                                 Chromatogram profileB,
                                                 BasisFunctionBase basisFunction,
                                                 ref List<XYData>  intensityProfile)
        {            
            double minScan = profileA.FitPoints.Min(x => x.X);
            double maxScan = profileA.FitPoints.Max(x => x.X);

            minScan = Math.Min(minScan, profileB.FitPoints.Min(x => x.X));
            maxScan = Math.Max(maxScan, profileB.FitPoints.Max(x => x.X));
            
            double deltaScan    = Math.Abs(maxScan - minScan) / 100;
            double scan         = minScan;

            List<XYData> pairs = new List<XYData>();

            while (scan <= maxScan)
            {
                double x = basisFunction.Evaluate(profileA.FitCoefficients, scan);
                double y = basisFunction.Evaluate(profileB.FitCoefficients, scan);

                pairs.Add(new XYData(x, y));
                scan += deltaScan;
            }

            BasisFunctionBase linearRegression  = BasisFunctionFactory.BasisFunctionSelector(Solvers.LevenburgMarquadt.BasisFunctions.BasisFunctionsEnum.Linear);
            LevenburgMarquadtSolver solver      = new LevenburgMarquadtSolver();
            solver.BasisFunction                = linearRegression.FunctionDelegate;  

            double [] coeffs                    = linearRegression.Coefficients;
            SolverReport report                 = solver.Solve(pairs, ref coeffs);

            return report.RSquared;
        }        
    }
}
