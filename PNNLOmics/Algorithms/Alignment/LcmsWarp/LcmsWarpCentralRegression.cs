﻿using System;
using System.Collections.Generic;
using System.Linq;
using PNNLOmics.Algorithms.Regression;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.Alignment.LcmsWarp
{
    /// <summary>
    /// Object which performs Central Regression for LCMSWarp
    /// </summary>
    public class LcmsWarpCentralRegression
    {
        private int m_numYBins;
        private int m_numJumps;

        // number of matches for each x section
        private int m_numSectionMatches;
        // Minimum number of points to be present in a section
        // for it to be considered in computing function
        private readonly int m_minSectionPts;

        private readonly List<double> m_matchScores;
        private readonly List<double> m_sectionMisMatchScore;
        private readonly List<double> m_sectionTolerance;
        private readonly List<double> m_alignmentScores;
        private readonly List<int> m_bestPreviousIndex;
        private readonly List<int> m_count;

        private readonly NormalUniformEm m_normUnifEm;

        private double m_minY;
        private double m_maxY;
        //the tolerance to apply
        private double m_tolerance;
        //outlier zscore
        private double m_outlierZScore;

        //Storage for standard deviations at each slice
        private readonly List<double> m_stdY;
        //Storage for alignment
        private readonly Dictionary<int, int> m_alignmentFunction;

        private double m_minX;
        private double m_maxX;

        private int m_numXBins;
        private readonly List<RegressionPoint> m_pts;

        /// <summary>
        /// Default constructor the Central regression, sets parameters to default
        /// values and allocates memory space for the lists that will be used
        /// </summary>
        public LcmsWarpCentralRegression()
        {
            m_numXBins = 100;
            m_numYBins = 100;
            m_numJumps = 30;
            m_tolerance = 0.8; // 5 standard devs
            m_outlierZScore = m_tolerance;
            m_minSectionPts = 5;
            m_matchScores = new List<double>();
            m_sectionMisMatchScore = new List<double>();
            m_sectionTolerance = new List<double>();
            m_alignmentScores = new List<double>();
            m_bestPreviousIndex = new List<int>();
            m_count = new List<int>();

            m_normUnifEm = new NormalUniformEm();

            m_stdY = new List<double>();
            m_alignmentFunction = new Dictionary<int, int>();

            m_pts = new List<RegressionPoint>();

            SetOptions(m_numXBins, m_numYBins, m_numJumps, m_outlierZScore);
        }

        /// <summary>
        /// Getter to return the list of regression points
        /// </summary>
        /// <returns></returns>
        public List<RegressionPoint> Points
        {
            get { return m_pts; }
        }

        /// <summary>
        /// Sets up all the options for the Central Regression
        /// </summary>
        /// <param name="numXBins"></param>
        /// <param name="numYBins"></param>
        /// <param name="numJumps"></param>
        /// <param name="zTolerance"></param>
        public void SetOptions(int numXBins, int numYBins, int numJumps, double zTolerance)
        {
            m_numXBins = numXBins;
            m_numYBins = numYBins;
            m_numJumps = numJumps;
            m_tolerance = zTolerance;
            m_outlierZScore = zTolerance;

            m_numSectionMatches = m_numYBins * (2 * numJumps + 1);

            m_matchScores.Clear();
            m_alignmentScores.Clear();
            m_bestPreviousIndex.Clear();
        }

        /// <summary>
        /// Method to set the outlier score for Central regression.
        /// </summary>
        /// <param name="outlierZScore"></param>
        public void SetOutlierZScore(double outlierZScore)
        {
            m_outlierZScore = outlierZScore;
        }

        /// <summary>
        /// Finds the min and max mass error and Net
        /// </summary>
        public void CalculateMinMax()
        {
            int numPts = m_pts.Count();

            m_minX = double.MaxValue;
            m_maxX = double.MinValue;

            m_minY = double.MaxValue;
            m_maxY = double.MinValue;

            for (int i = 0; i < numPts; i++)
            {
                RegressionPoint point = m_pts[i];
                if (point.X < m_minX)
                {
                    m_minX = point.X;
                }
                if (point.X > m_maxX)
                {
                    m_maxX = point.X;
                }
                if (point.MassError < m_minY)
                {
                    m_minY = point.MassError;
                }
                if (point.MassError > m_maxY)
                {
                    m_maxY = point.MassError;
                }
            }
        }

        /// <summary>
        /// Computers the Sections standard deviation and number in each match
        /// </summary>
        /// <param name="intervalNum"></param>
        private void CalculateSectionStdAndCount(int intervalNum)
        {
            var points = new List<double>();

            int numPoints = m_pts.Count();
            double xInterval = (m_maxX - m_minX) / m_numXBins;

            for (int ptNum = 0; ptNum < numPoints; ptNum++)
            {
                RegressionPoint pt = m_pts[ptNum];
                int sectionNum = Convert.ToInt32((pt.X - m_minX) / xInterval);
                if (sectionNum == m_numXBins)
                {
                    sectionNum = m_numXBins - 1;
                }
                if (intervalNum == sectionNum)
                {
                    m_count[sectionNum]++;
                    points.Add(pt.MassError);
                }
            }

            if (m_count[intervalNum] > m_minSectionPts)
            {
                m_normUnifEm.CalculateDistributions(points);
                m_stdY[intervalNum] = m_normUnifEm.StandDev;
                if (Math.Abs(m_stdY[intervalNum]) > double.Epsilon)
                {
                    double tolerance = m_stdY[intervalNum] * m_tolerance;
                    m_sectionTolerance[intervalNum] = tolerance;

                    double misMatchScore = (tolerance * tolerance) / (m_stdY[intervalNum] * m_stdY[intervalNum]);
                    m_sectionMisMatchScore[intervalNum] = misMatchScore;
                }
                else
                {
                    m_sectionMisMatchScore[intervalNum] = m_tolerance * m_tolerance;
                    m_sectionTolerance[intervalNum] = 0;
                }
            }
            else
            {
                m_stdY[intervalNum] = 0.1;
            }
        }

        /// <summary>
        /// Calculate standard deviations for all sections
        /// </summary>
        public void CalculateSectionsStd()
        {
            m_count.Capacity = m_numXBins;
            m_stdY.Capacity = m_numXBins;
            m_stdY.Clear();

            for (int interval = 0; interval < m_numXBins; interval++)
            {
                m_stdY.Add(0);
                m_count.Add(0);
                m_sectionMisMatchScore.Add(0);
                m_sectionTolerance.Add(0);
                CalculateSectionStdAndCount(interval);
            }
        }

        /// <summary>
        /// Removes all the residual data from prior regressions
        /// </summary>
        public void Clear()
        {
            m_matchScores.Clear();
            m_alignmentFunction.Clear();
            m_alignmentScores.Clear();
            m_bestPreviousIndex.Clear();
            m_count.Clear();
            m_pts.Clear();
            m_stdY.Clear();
            m_sectionMisMatchScore.Clear();
            m_sectionTolerance.Clear();
        }

        private void SetUnmatchedScoreMatrix()
        {
            // Assigns each section's score to the minimum for that section
            // for each possible matching sections, the minimum score would correspond 
            // to the situation that all points in the section lie outside the tolerance
            m_matchScores.Clear();

            // At the moment, assumes that the tolerance is in zscore units
            for (int xSection = 0; xSection < m_numXBins; xSection++)
            {
                double sectionMismatchScore = m_sectionMisMatchScore[xSection] * m_count[xSection];

                for (int sectionMatchNum = 0; sectionMatchNum < m_numSectionMatches; sectionMatchNum++)
                {
                    m_matchScores.Add(sectionMismatchScore);
                }
            }
        }

        private void CalculateScoreMatrix()
        {
            m_matchScores.Capacity = m_numXBins * m_numSectionMatches;

            // Neww to calculate the score matrix for all possible score blocks.
            // For every x section, all y secments between y_interest - m_num_jumps
            // to y_interest + m_num_jumps are scored.

            // First set the unmatched score.
            SetUnmatchedScoreMatrix();

            double yIntervalSize = (m_maxY - m_minY) / m_numYBins;
            double xIntervalSize = (m_maxX - m_minX) / m_numXBins;

            // For each point that is seen, add the supporting score to the appropriate section.
            int numPts = m_pts.Count();

            for (int pointNum = 0; pointNum < numPts; pointNum++)
            {
                RegressionPoint point = m_pts[pointNum];
                int xSection = Convert.ToInt32((point.X - m_minX) / xIntervalSize);
                if (xSection == m_numXBins)
                {
                    xSection = m_numXBins - 1;
                }

                // If the point belongs to a section where the num # of points is not met, ignore it
                if (m_count[xSection] < m_minSectionPts || Math.Abs(m_stdY[xSection]) < double.Epsilon)
                {
                    continue;
                }

                double yTolerance = m_sectionTolerance[xSection];

                int yInterval = Convert.ToInt32((0.0001 + (point.MassError - m_minY) / yIntervalSize));

                if (yInterval == m_numYBins)
                {
                    yInterval = m_numYBins - 1;
                }

                // Matches to the section that the point would contribute to.
                int minYStart = Convert.ToInt32(yInterval - yTolerance / yIntervalSize);
                int maxYStart = Convert.ToInt32(yInterval + yTolerance / yIntervalSize);

                double sectionMismatchScore = m_sectionMisMatchScore[xSection];

                double xFraction = (point.X - m_minX) / xIntervalSize - xSection;

                for (int yFrom = minYStart; yFrom <= maxYStart; yFrom++)
                {
                    if (yFrom < 0)
                    {
                        continue;
                    }
                    if (yFrom >= m_numYBins)
                    {
                        break;
                    }
                    for (int yTo = yFrom - m_numJumps; yTo <= yFrom + m_numJumps; yTo++)
                    {
                        if (yTo < 0)
                        {
                            continue;
                        }
                        if (yTo >= m_numYBins)
                        {
                            break;
                        }

                        //Assumes linear piecewise transform to calculate the estimated y
                        double yEstimated = (yFrom + (yTo - yFrom) * xFraction) * yIntervalSize + m_minY;
                        double yDelta = point.MassError - yEstimated;

                        //Make sure the point is in the linear range to effect the score
                        if (Math.Abs(yDelta) > yTolerance)
                        {
                            continue;
                        }

                        double matchScore = (yDelta * yDelta) / (m_stdY[xSection] * m_stdY[xSection]);
                        int jump = yTo - yFrom + m_numJumps;
                        int sectionIndex = xSection * m_numSectionMatches + yFrom * (2 * m_numJumps + 1) + jump;
                        double currentMatchScore = m_matchScores[sectionIndex];
                        m_matchScores[sectionIndex] = currentMatchScore - sectionMismatchScore + matchScore;
                    }
                }
            }
        }

        private void CalculateAlignmentMatrix()
        {
            m_bestPreviousIndex.Clear();
            m_alignmentScores.Clear();
            m_alignmentScores.Capacity = m_numXBins + 1 * m_numYBins;
            m_bestPreviousIndex.Capacity = m_numXBins + 1 * m_numYBins;

            for (int ySection = 0; ySection < m_numYBins; ySection++)
            {
                m_bestPreviousIndex.Add(-2);
                m_alignmentScores.Add(0);
            }

            for (int xSection = 1; xSection <= m_numXBins; xSection++)
            {
                for (int ySection = 0; ySection < m_numYBins; ySection++)
                {
                    m_bestPreviousIndex.Add(-1);
                    m_alignmentScores.Add(double.MaxValue);
                }
            }

            for (int xSection = 1; xSection <= m_numXBins; xSection++)
            {
                for (int ySection = 0; ySection < m_numYBins; ySection++)
                {
                    int index = xSection * m_numYBins + ySection;
                    double bestAlignmentScore = double.MaxValue;

                    for (int jump = m_numJumps; jump < 2 * m_numJumps + 1; jump++)
                    {
                        int ySectionFrom = ySection - jump + m_numJumps;
                        if (ySectionFrom < 0)
                        {
                            break;
                        }
                        int previousAlignmentIndex = (xSection - 1) * m_numYBins + ySectionFrom;
                        int previousMatchIndex = (xSection - 1) * m_numSectionMatches + ySectionFrom * (2 * m_numJumps + 1) + jump;
                        double previousAlignmentScore = m_alignmentScores[previousAlignmentIndex];
                        double previousMatchScore = m_matchScores[previousMatchIndex];
                        if (previousAlignmentScore + previousMatchScore < bestAlignmentScore)
                        {
                            bestAlignmentScore = previousMatchScore + previousAlignmentScore;
                            m_bestPreviousIndex[index] = previousAlignmentIndex;
                            m_alignmentScores[index] = bestAlignmentScore;
                        }
                    }

                    for (int jump = 0; jump < m_numJumps; jump++)
                    {
                        int ySectionFrom = ySection - jump + m_numJumps;
                        if (ySectionFrom < 0)
                        {
                            break;
                        }
                        int previousAlignmentIndex = (xSection - 1) * m_numYBins + ySectionFrom;
                        int previousMatchIndex = (xSection - 1) * m_numSectionMatches + ySectionFrom * (2 * m_numJumps + 1) + jump;
                        if ((previousAlignmentIndex > m_alignmentScores.Count-1) || (previousMatchIndex > m_matchScores.Count-1))
                        {
                            break;
                        }
                        double previousAlignmentScore = m_alignmentScores[previousAlignmentIndex];
                        double previousMatchScore = m_matchScores[previousMatchIndex];
                        if (previousAlignmentScore + previousMatchScore < bestAlignmentScore)
                        {
                            bestAlignmentScore = previousMatchScore + previousAlignmentScore;
                            m_bestPreviousIndex[index] = previousAlignmentIndex;
                            m_alignmentScores[index] = bestAlignmentScore;
                        }
                    }
                }
            }
        }

        private void CalculateRegressionFunction()
        {
            m_alignmentFunction.Clear();
            //Start at the last section best score and trace backwards
            double bestScore = double.MaxValue;
            int bestPreviousIndex = 0;
            int bestYShift = m_numYBins / 2;
            int xSection = m_numXBins;

            while (xSection >= 1)
            {
                if (m_count[xSection - 1] >= m_minSectionPts)
                {
                    for (int ySection = 0; ySection < m_numYBins; ySection++)
                    {
                        int index = xSection * m_numYBins + ySection;
                        double ascore = m_alignmentScores[index];
                        if (ascore < bestScore)
                        {
                            bestScore = ascore;
                            bestYShift = ySection;
                            bestPreviousIndex = m_bestPreviousIndex[index];
                        }
                    }
                    break;
                }
                xSection--;
            }

            for (int i = xSection; i <= m_numXBins; i++)
            {
                m_alignmentFunction.Add(i, bestYShift);
            }
            while (xSection > 0)
            {
                xSection--;
                int yShift = bestPreviousIndex % m_numYBins;
                m_alignmentFunction.Add(xSection, yShift);
                bestPreviousIndex = m_bestPreviousIndex[bestPreviousIndex];
            }

        }

        /// <summary>
        /// Calculates Central regression for the matches found and passed in
        /// </summary>
        /// <param name="calibMatches"></param>
        public void CalculateRegressionFunction(ref List<RegressionPoint> calibMatches)
        {
            Clear();
            foreach (RegressionPoint point in calibMatches)
            {
                m_pts.Add(point);
            }

            // First find the boundaries
            CalculateMinMax();

            // For if it's constant answer
            if (Math.Abs(m_minY - m_maxY) < double.Epsilon)
            {
                for (int xSection = 0; xSection < m_numXBins; xSection++)
                {
                    m_alignmentFunction.Add(xSection, 0);
                }
                return;
            }

            CalculateSectionsStd();
            CalculateScoreMatrix();
            CalculateAlignmentMatrix();
            CalculateRegressionFunction();
        }

        /// <summary>
        /// Goes through all the regression points and only retains the ones that are within the outlier z score
        /// </summary>
        public void RemoveRegressionOutliers()
        {
            double xIntervalSize = (m_maxX - m_minX) / m_numXBins;
            var tempPts = new List<RegressionPoint>();
            int numPts = m_pts.Count;

            for (int pointNum = 0; pointNum < numPts; pointNum++)
            {
                RegressionPoint point  = m_pts[pointNum];
                int intervalNum = Convert.ToInt32((point.X - m_minX) / xIntervalSize);
                if (intervalNum == m_numXBins)
                {
                    intervalNum = m_numXBins - 1;
                }
                double stdY = m_stdY[intervalNum];
                double val = GetPredictedValue(point.X);
                double delta = (val - point.MassError) / stdY;
                if (Math.Abs(delta) < m_outlierZScore)
                {
                    tempPts.Add(point);
                }
            }

            m_pts.Clear();

            foreach (RegressionPoint point in tempPts)
            {
                m_pts.Add(point);
            }
        }

        /// <summary>
        /// Given a value x, finds the appropriate y value that would correspond to it in the regression function
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double GetPredictedValue(double x)
        {
            double yIntervalSize = (m_maxY - m_minY) / m_numYBins;
            double xIntervalSize = (m_maxX - m_minX) / m_numXBins;

            int xSection = Convert.ToInt32((x - m_minX) / xIntervalSize);
            int ySectionFrom;
            if (xSection >= m_numXBins)
            {
                xSection = m_numXBins - 1;
            }
            if (xSection < 0)
            {
                xSection = 0;
                ySectionFrom = m_alignmentFunction.ElementAt(xSection).Value;
                return m_minY + ySectionFrom * yIntervalSize;
            }

            double xFraction = (x - m_minX) / xIntervalSize - xSection;
            ySectionFrom = m_alignmentFunction.ElementAt(xSection).Value;
            int ySectionTo = ySectionFrom;

            if (xSection < m_numXBins - 1)
            {
                ySectionTo = m_alignmentFunction.ElementAt(xSection + 1).Value;
            }

            double yPred = xFraction * yIntervalSize * (ySectionTo - ySectionFrom)
                            + ySectionFrom * yIntervalSize + m_minY;

            return yPred;
        }
    }
}