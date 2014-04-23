using PNNLOmics.Algorithms.Regression;
using System.Collections.Generic;
using System.Linq;

namespace PNNLOmics.Algorithms.Alignment.SpectralMatching
{
    public class SpectralAnchorPointAligner: IAlignmentFunction
    {
        private LoessInterpolator m_netInterpolator;
        private LoessInterpolator m_massInterpolator;

        public void CreateAlignmentFunctions(IEnumerable<SpectralAnchorPointMatch> matches)
        {
            var netXvalues  = new List<double>();
            var netYvalues  = new List<double>();
            var massXvalues = new List<double>();
            var massYvalues = new List<double>();

            matches = matches.ToList().OrderBy(x => x.AnchorPointX.Net);

            // 1. Find the best matches
            // 2. Find only matches that have been made once.
            var bestMatches = new Dictionary<int, SpectralAnchorPointMatch>();
            foreach (var match in matches)
            {
                var scan = match.AnchorPointX.Scan;
                if (bestMatches.ContainsKey(scan))
                {
                    if (bestMatches[scan].SimilarityScore < match.SimilarityScore)
                    {
                        bestMatches[scan] = match;
                    }
                }
                else
                {
                    bestMatches.Add(scan, match);
                }
            }

            // 2. Find only those matched once
            var all = new Dictionary<int, SpectralAnchorPointMatch>();
            foreach (var match in bestMatches.Values)
            {
                var scan = match.AnchorPointY.Scan;
                if (all.ContainsKey(scan))
                {
                    if (all[scan].SimilarityScore < match.SimilarityScore)
                    {
                        all[scan] = match;
                    }
                }
                else
                {
                    all.Add(scan, match);
                }
            }

            // Then generate the NET Alignment using R1
            var anchorPoints = all.Values.OrderBy(x => x.AnchorPointX.Net).ToList();

            foreach (var match in anchorPoints)
            {
                netXvalues.Add(match.AnchorPointX.Net);
                netYvalues.Add(match.AnchorPointY.Net);                
            }

            var netInterpolator = new LoessInterpolator();
            netInterpolator.Smooth(  netXvalues,
                                                           netYvalues,
                                                           FitFunctionFactory.Create(FitFunctionTypes.TriCubic));
            
            // Then generate the Mass Alignment using R1
            // We also have to resort the matches based on mass now too
            anchorPoints = all.Values.OrderBy(x => x.AnchorPointX.Mz).ToList();
            foreach (var match in anchorPoints)
            {
                massXvalues.Add(match.AnchorPointX.Mz);
                massYvalues.Add(match.AnchorPointY.Mz);
            }

            var massInterpolator = new LoessInterpolator();
            massInterpolator.Smooth(massXvalues,
                                    massYvalues,
                                    FitFunctionFactory.Create(FitFunctionTypes.TriCubic));

            m_netInterpolator  = netInterpolator;
            m_massInterpolator = massInterpolator;


            foreach (var match in anchorPoints)
            {
                match.AnchorPointY.NetAligned = netInterpolator.Predict(match.AnchorPointY.Net);
                match.AnchorPointY.MzAligned  = massInterpolator.Predict(match.AnchorPointY.Mz);
            }
        }

        public double AlignMass(double mass)
        {
            return m_massInterpolator.Predict(mass);
        }
        public double AlignNet(double net)
        {
            return m_netInterpolator.Predict(net);
        }
    }
}
