using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Alignment.LCMSWarp.LCMSWarper.LCMSRegression
{
    public class LCMSCentralRegression
    {
        int m_num_y_bins;
        int m_num_jumps;

        // number of matches for each x section
        int m_num_section_matches;
        // Minimum number of points to be present in a section
        // for it to be considered in computing function
        int m_min_section_pts;

        List<double> m_matchScores;
        List<double> m_sectionMisMatchScore;
        List<double> m_sectionTolerance;
        List<double> m_alignmentScores;
        List<int> m_bestPreviousIndex;
        List<int> m_count;

        LCMSNormUnifEM m_normUnifEM;

        double m_minY;
        double m_maxY;
        //the tolerance to apply
        double m_tolerance;
        //outlier zscore
        double m_outlierZScore;

        //Storage for standard deviations at each slice
        List<double> m_stdY;
        //Storage for alignment
        Dictionary<int, int> m_alignmentFunction;

        double m_minX;
        double m_maxX;

        int m_num_x_bins;
        List<LCMSRegressionPts> m_pts;

        public LCMSCentralRegression()
        {
            m_num_x_bins = 0;
            m_num_y_bins = 0;
            m_num_jumps = 0;
            m_tolerance = 0.8; // 5 standard devs
            m_outlierZScore = m_tolerance;
            m_min_section_pts = 5;
            m_matchScores = new List<double>();
            m_sectionMisMatchScore = new List<double>();
            m_sectionTolerance = new List<double>();
            m_alignmentScores = new List<double>();
            m_bestPreviousIndex = new List<int>();
            m_count = new List<int>();

            m_normUnifEM = new LCMSNormUnifEM();

            m_stdY = new List<double>();
            m_alignmentFunction = new Dictionary<int, int>();

            m_pts = new List<LCMSRegressionPts>();
        }

        public List<LCMSRegressionPts> Points()
        {
            return m_pts;
        }

        public void SetOptions(int num_x_bins, int num_y_bins, int num_jumps, double zTolerance)
        {
            m_num_x_bins = num_x_bins;
            m_num_y_bins = num_y_bins;
            m_num_jumps = num_jumps;
            m_tolerance = zTolerance;
            m_outlierZScore = zTolerance;

            m_num_section_matches = m_num_y_bins * (2 * num_jumps + 1);

            m_matchScores.Clear();
            m_alignmentScores.Clear();
            m_bestPreviousIndex.Clear();
        }

        public void SetOutlierZScore(double outlierZScore)
        {
            m_outlierZScore = outlierZScore;
        }

        public void CalculateMinMax()
        {
            int numPts = m_pts.Count();

            m_minX = double.MaxValue;
            m_maxX = double.MinValue;

            m_minY = double.MaxValue;
            m_maxY = double.MinValue;

            for (int i = 0; i < numPts; i++)
            {
                LCMSRegressionPts point = m_pts[i];
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

        public void CalculateSectionStdAndCount(int interval_num)
        {
            List<double> points = new List<double>();

            int num_points = m_pts.Count();
            double x_interval = (m_maxX - m_minX) / m_num_x_bins;

            for (int pt_num = 0; pt_num < num_points; pt_num++)
            {
                LCMSRegressionPts pt = new LCMSRegressionPts();
                pt = m_pts[pt_num];
                int sectionNum = Convert.ToInt32((pt.X - m_minX) / x_interval);
                if (sectionNum == m_num_x_bins)
                {
                    sectionNum = m_num_x_bins - 1;
                }
                if (interval_num == sectionNum)
                {
                    m_count[sectionNum]++;
                    points.Add(pt.MassError);
                }
            }

            if (m_count[interval_num] > m_min_section_pts)
            {
                m_normUnifEM.CalculateDistributions(points);
                m_stdY[interval_num] = m_normUnifEM.StandDev;
                if (m_stdY[interval_num] != 0)
                {
                    double tolerance = m_stdY[interval_num] * m_tolerance;
                    m_sectionTolerance[interval_num] = tolerance;

                    double misMatch_score = (tolerance * tolerance) / (m_stdY[interval_num] * m_stdY[interval_num]);
                    m_sectionMisMatchScore[interval_num] = misMatch_score;
                }
                else
                {
                    m_sectionMisMatchScore[interval_num] = m_tolerance * m_tolerance;
                    m_sectionTolerance[interval_num] = 0;
                }
            }
            else
            {
                m_stdY[interval_num] = 0.1;
            }
        }

        public void CalculateSectionsStd()
        {
            m_count.Capacity = m_num_x_bins;
            m_stdY.Capacity = m_num_x_bins;
            m_stdY.Clear();

            for (int interval = 0; interval < m_num_x_bins; interval++)
            {
                m_stdY.Add(0);
                m_count.Add(0);
                m_sectionMisMatchScore.Add(0);
                m_sectionTolerance.Add(0);
                CalculateSectionStdAndCount(interval);
            }
        }

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

        public void SetUnmatchedScoreMatrix()
        {
            // Assigns each section's score to the minimum for that section
            // for each possible matching sections, the minimum score would correspond 
            // to the situation that all points in the section lie outside the tolerance
            m_matchScores.Clear();

            // At the moment, assumes that the tolerance is in zscore units
            for (int x_section = 0; x_section < m_num_x_bins; x_section++)
            {
                double section_mismatchScore = m_sectionMisMatchScore[x_section] * m_count[x_section];

                for (int section_match_num = 0; section_match_num < m_num_section_matches; section_match_num++)
                {
                    m_matchScores.Add(section_mismatchScore);
                }
            }
        }

        public void CalculateScoreMatrix()
        {
            m_matchScores.Capacity = m_num_x_bins * m_num_section_matches;

            // Neww to calculate the score matrix for all possible score blocks.
            // For every x section, all y secments between y_interest - m_num_jumps
            // to y_interest + m_num_jumps are scored.

            // First set the unmatched score.
            SetUnmatchedScoreMatrix();

            double y_interval_size = (m_maxY - m_minY) / m_num_y_bins;
            double x_interval_size = (m_maxX - m_minX) / m_num_x_bins;

            // For each point that is seen, add the supporting score to the appropriate section.
            int num_pts = m_pts.Count();

            for (int pointNum = 0; pointNum < num_pts; pointNum++)
            {
                LCMSRegressionPts point = m_pts[pointNum];
                int x_section = Convert.ToInt32((point.X - m_minX) / x_interval_size);
                if (x_section == m_num_x_bins)
                {
                    x_section = m_num_x_bins - 1;
                }

                // If the point belongs to a section where the num # of points is not met, ignore it
                if (m_count[x_section] < m_min_section_pts || m_stdY[x_section] == 0)
                {
                    continue;
                }

                double y_tolerance = m_sectionTolerance[x_section];

                int y_interval = Convert.ToInt32((0.0001 + (point.MassError - m_minY) / y_interval_size));

                if (y_interval == m_num_y_bins)
                {
                    y_interval = m_num_y_bins - 1;
                }

                // Matches to the section that the point would contribute to.
                int min_y_start = Convert.ToInt32(y_interval - y_tolerance / y_interval_size);
                int max_y_start = Convert.ToInt32(y_interval + y_tolerance / y_interval_size);

                double section_mismatch_score = m_sectionMisMatchScore[x_section];

                double x_fraction = (point.X - m_minX) / x_interval_size - x_section;
                double y_estimated = 0;

                for (int y_from = min_y_start; y_from <= max_y_start; y_from++)
                {
                    if (y_from < 0)
                    {
                        continue;
                    }
                    if (y_from >= m_num_y_bins)
                    {
                        break;
                    }
                    for (int y_to = y_from - m_num_jumps; y_to <= y_from + m_num_jumps; y_to++)
                    {
                        if (y_to < 0)
                        {
                            continue;
                        }
                        if (y_to >= m_num_y_bins)
                        {
                            break;
                        }

                        //Assumes linear piecewise transform to calculate the estimated y
                        y_estimated = (y_from + (y_to - y_from) * x_fraction) * y_interval_size + m_minY;
                        double y_delta = point.MassError - y_estimated;

                        //Make sure the point is in the linear range to effect the score
                        if (Math.Abs(y_delta) > y_tolerance)
                        {
                            continue;
                        }

                        double match_score = (y_delta * y_delta) / (m_stdY[x_section] * m_stdY[x_section]);
                        int jump = y_to - y_from + m_num_jumps;
                        int section_index = x_section * m_num_section_matches + y_from * (2 * m_num_jumps + 1) + jump;
                        double current_match_score = m_matchScores[section_index];
                        m_matchScores[section_index] = current_match_score - section_mismatch_score + match_score;
                    }
                }
            }
        }

        public void CalculateAlignmentMatrix()
        {
            m_bestPreviousIndex.Clear();
            m_alignmentScores.Clear();
            m_alignmentScores.Capacity = m_num_x_bins + 1 * m_num_y_bins;
            m_bestPreviousIndex.Capacity = m_num_x_bins + 1 * m_num_y_bins;

            for (int y_section = 0; y_section < m_num_y_bins; y_section++)
            {
                int index = y_section;
                m_bestPreviousIndex.Add(-2);
                m_alignmentScores.Add(0);
            }

            for (int x_section = 1; x_section <= m_num_x_bins; x_section++)
            {
                for (int y_section = 0; y_section < m_num_y_bins; y_section++)
                {
                    int index = x_section * m_num_y_bins + y_section;
                    m_bestPreviousIndex.Add(-1);
                    m_alignmentScores.Add(double.MaxValue);
                }
            }

            for (int x_section = 1; x_section <= m_num_x_bins; x_section++)
            {
                for (int y_section = 0; y_section < m_num_y_bins; y_section++)
                {
                    int index = x_section * m_num_y_bins + y_section;
                    double bestAlignmentScore = double.MaxValue;

                    for (int jump = m_num_jumps; jump < 2 * m_num_jumps + 1; jump++)
                    {
                        int y_section_from = y_section - jump + m_num_jumps;
                        if (y_section_from < 0)
                        {
                            break;
                        }
                        int previousAlignmentIndex = (x_section - 1) * m_num_y_bins + y_section_from;
                        int previousMatchIndex = (x_section - 1) * m_num_section_matches + y_section_from * (2 * m_num_jumps + 1) + jump;
                        double previousAlignmentScore = m_alignmentScores[previousAlignmentIndex];
                        double previousMatchScore = m_matchScores[previousMatchIndex];
                        if (previousAlignmentScore + previousMatchScore < bestAlignmentScore)
                        {
                            bestAlignmentScore = previousMatchScore + previousAlignmentScore;
                            m_bestPreviousIndex[index] = previousAlignmentIndex;
                            m_alignmentScores[index] = bestAlignmentScore;
                        }
                    }

                    for (int jump = 0; jump < m_num_jumps; jump++)
                    {
                        int y_section_from = y_section - jump + m_num_jumps;
                        if (y_section_from < 0)
                        {
                            break;
                        }
                        int previousAlignmentIndex = (x_section - 1) * m_num_y_bins + y_section_from;
                        int previousMatchIndex = (x_section - 1) * m_num_section_matches + y_section_from * (2 * m_num_jumps + 1) + jump;
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

        public void CalculateRegressionFunction()
        {
            m_alignmentFunction.Clear();
            //Start at the last section best score and trace backwards
            double bestScore = double.MaxValue;
            int bestPreviousIndex = 0;
            int bestYShift = m_num_y_bins / 2;
            int x_section = m_num_x_bins;

            while (x_section >= 1)
            {
                if (m_count[x_section - 1] >= m_min_section_pts)
                {
                    for (int y_section = 0; y_section < m_num_y_bins; y_section++)
                    {
                        int index = x_section * m_num_y_bins + y_section;
                        double ascore = m_alignmentScores[index];
                        if (ascore < bestScore)
                        {
                            bestScore = ascore;
                            bestYShift = y_section;
                            bestPreviousIndex = m_bestPreviousIndex[index];
                        }
                    }
                    break;
                }
                else
                {
                    x_section--;
                }
            }

            for (int i = x_section; i <= m_num_x_bins; i++)
            {
                m_alignmentFunction.Add(i, bestYShift);
            }
            while (x_section > 0)
            {
                x_section--;
                int y_shift = bestPreviousIndex % m_num_y_bins;
                m_alignmentFunction.Add(x_section, y_shift);
                bestPreviousIndex = m_bestPreviousIndex[bestPreviousIndex];
            }

        }

        public void RemoveOutliersAndCopy(ref List<LCMSRegressionPts> calibMatches)
        {
            foreach (LCMSRegressionPts point in calibMatches)
            {
                m_pts.Add(point);
            }

        }

        public void CalculateRegressionFunction(ref List<LCMSRegressionPts> calibMatches)
        {
            Clear();
            RemoveOutliersAndCopy(ref calibMatches);

            // First find the boundaries
            CalculateMinMax();

            // For if it's constant answer
            if (m_minY == m_maxY)
            {
                for (int x_section = 0; x_section < m_num_x_bins; x_section++)
                {
                    m_alignmentFunction.Add(x_section, 0);
                }
                return;
            }

            CalculateSectionsStd();
            CalculateScoreMatrix();
            CalculateAlignmentMatrix();
            CalculateRegressionFunction();
        }

        public void PrintScoreMatrix(string fileName)
        {

        }

        public void PrintAlignmentScoreMatrix(string fileName)
        {

        }

        public void PrintRegressionFunction(string fileName)
        {

        }

        public void PrintPoints(string fileName)
        {

        }

        public void RemoveRegressionOutliers()
        {
            double x_interval_size = (m_maxX - m_minX) / m_num_x_bins;
            List<LCMSRegressionPts> tempPts = new List<LCMSRegressionPts>();
            int numPts = m_pts.Count;

            for (int pointNum = 0; pointNum < numPts; pointNum++)
            {
                LCMSRegressionPts point = new LCMSRegressionPts();
                point = m_pts[pointNum];
                int intervalNum = Convert.ToInt32((point.X - m_minX) / x_interval_size);
                if (intervalNum == m_num_x_bins)
                {
                    intervalNum = m_num_x_bins - 1;
                }
                double std_y = m_stdY[intervalNum];
                double val = GetPredictedValue(point.X);
                double delta = (val - point.MassError) / std_y;
                if (Math.Abs(delta) < m_outlierZScore)
                {
                    tempPts.Add(point);
                }
                m_pts.Clear();
            }

            foreach (LCMSRegressionPts point in tempPts)
            {
                m_pts.Add(point);
            }
        }

        public double GetPredictedValue(double x)
        {
            double yIntervalSize = (m_maxY - m_minY) / m_num_y_bins;
            double xIntervalSize = (m_maxX - m_minX) / m_num_x_bins;

            int x_section = Convert.ToInt32((x - m_minX) / xIntervalSize);
            int y_section_from;
            if (x_section >= m_num_x_bins)
            {
                x_section = m_num_x_bins - 1;
            }
            if (x_section < 0)
            {
                x_section = 0;
                y_section_from = m_alignmentFunction.ElementAt(x_section).Value;
                return m_minY + y_section_from * yIntervalSize;
            }

            double x_fraction = (x - m_minX) / xIntervalSize - x_section;
            y_section_from = m_alignmentFunction.ElementAt(x_section).Value;
            int y_section_to = y_section_from;

            if (x_section < m_num_x_bins - 1)
            {
                y_section_to = m_alignmentFunction.ElementAt(x_section + 1).Value;
            }

            double y_pred = x_fraction * yIntervalSize * (y_section_to - y_section_from)
                            + y_section_from * yIntervalSize + m_minY;

            return y_pred;
        }
    }
}
