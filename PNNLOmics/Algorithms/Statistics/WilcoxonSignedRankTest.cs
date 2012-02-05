using System;
using System.Collections.Generic;
using System.Linq;

namespace PNNLOmics.Algorithms.Statistics
{
    /// <summary>
    /// Performs the Wilcoxon Signed Rank Test
    /// </summary>
    public class WilcoxonSignedRankTest
    {
        /// <summary>
        /// Computes a p-value for a given distribution.
        /// </summary>
        /// <param name="distribution">Values used to compute the p-value.</param>
        /// <returns>Level of significance (p-value)</returns>
        public double ComputePValue(List<double> distribution)
        {
            List<double> absoluteDifference = new List<double>();

            int totalObservations = distribution.Count;
            System.Diagnostics.Debug.Assert(distribution.Count > 1);


            for (int i = 0; i < distribution.Count; i++)
            {
                absoluteDifference.Add(Math.Abs(distribution[i]));
            }
            absoluteDifference.Sort();

            List<int> ranking = new List<int>();
            for (int i = 0; i < distribution.Count; i++)
            {
                ranking.Add(absoluteDifference.IndexOf(Math.Abs(distribution[i])) + 1);
            }
            // Compute the positive and negative rankings.
            List<int> posRank = new List<int>(totalObservations);
            List<int> negRank = new List<int>(totalObservations);
            for (int i = 0; i < totalObservations; i++)
            {
                if (distribution[i] < 0)
                {
                    negRank.Add(ranking[i]);
                }
                else
                {
                    posRank.Add(ranking[i]);
                }
            }

            int largestSignedDistributionSum = 0;
            double pValue = 0;
            if (posRank.Sum() > negRank.Sum())
            {
                largestSignedDistributionSum = posRank.Sum();
            }
            else
            {
                largestSignedDistributionSum = negRank.Sum();
            }

            pValue = ComputeLevelOfSignifance(largestSignedDistributionSum, totalObservations);
            return pValue;
        }

        /// <summary>
        /// Computes the level of significance (p-value) for the largest magnitude.
        /// http://www.fon.hum.uva.nl/Service/Statistics/Signed_Rank_Algorihms.html
        /// </summary>
        /// <param name="wInput">Largest observed w (magnitude of test)</param>
        /// <param name="numberOfObservations">Total number of observations that were made</param>
        /// <returns>p-value indicating level of significance</returns>
        private double ComputeLevelOfSignifance(double wInput, int numberOfObservations)
        {
            /* Determine Wmax, i.e., work with the largest Rank Sum */
            int maximumW = numberOfObservations * (numberOfObservations + 1) / 2;
            if (wInput < maximumW / 2)
            {
                wInput = maximumW - wInput;
            }

            int w = Convert.ToInt32(wInput);
            if (w != wInput)
            {
                ++w;
            }

            // The total number of possible outcomes is 2**N  
            long numberOfPossibilities = System.Convert.ToInt64(Math.Pow(2, numberOfObservations));
            long countLarger = 0;

            // Generate all distributions of sign over ranks as bit-patterns (i). 
            for (int i = 0; i < numberOfPossibilities; ++i)
            {
                int rankSum = 0;

                //	Shift "sign" bits out of i to determine the Sum of Ranks (j). 			
                for (int j = 0; j < numberOfObservations; ++j)
                {
                    if (((i >> j) & 1) != 0)
                    {
                        rankSum += j + 1;
                    }
                }


                // Count the number of "samples" that have a Sum of Ranks larger than 
                // or equal to the one found (i.e., >= W).			
                if (rankSum >= w)
                {
                    countLarger++;
                }
            }


            // The level of significance is the number of outcomes with a
            // sum of ranks equal to or larger than the one found (W) 
            // divided by the total number of possible outcomes. 
            // The level is doubled to get the two-tailed result.			
            double pValue = 2 * ((double)countLarger) / ((double)numberOfPossibilities);

            return pValue;
        }
    }
}
