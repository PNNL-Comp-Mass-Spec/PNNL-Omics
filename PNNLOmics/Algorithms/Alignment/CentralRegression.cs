using System;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// TODO: Create class comment block for CentralRegression
    /// </summary>
    public class CentralRegression
    {
        #region Class Members
        private int m_numYBins;
        private int m_numJumps;

        // Number of matches for each X section.
        private int m_numSectionMatches;

        // Minimum number of points to be present in a section for it to be
        // considered in computing a function
        private int m_minSectionPoints;

        private List<double> m_matchScores;
        private List<double> m_sectionMismatchScore;
        private List<double> m_sectionTolerance;
        private List<double> m_alignmentScores;
        private List<int> m_bestPreviousIndex;
        private List<int> m_count;
        private NormalizedUniformExpectationMaximization m_normUnifEM;
        private double m_minY;
        private double m_maxY;

        // Applied tolerance
        private double m_tolerance;

        // Standard deviations of y in each x slice
        private List<double> m_standardDeviationsY;
        private Dictionary<int, int> m_alignmentFunction;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of CentralRegression
        /// </summary>
        public CentralRegression()
        {
            m_matchScores = new List<double>();
            m_sectionMismatchScore = new List<double>();
            m_sectionTolerance = new List<double>();
            m_alignmentScores = new List<double>();
            m_bestPreviousIndex = new List<int>();
            m_count = new List<int>();
            m_normUnifEM = new NormalizedUniformExpectationMaximization();
            m_standardDeviationsY = new List<double>();
            m_alignmentFunction = new Dictionary<int, int>();
            Clear();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the minimum x
        /// </summary>
        public double MinX { get; set; }

        /// <summary>
        /// Gets or sets the maximum x
        /// </summary>
        public double MaxX { get; set; }

        /// <summary>
        /// Gets or sets the number of x bins
        /// </summary>
        public int NumXBins { get; set; }

        /// <summary>
        /// Gets or sets the regression points
        /// </summary>
        public List<RegressionPoints> RegressionPoints { get; set; }

        /// <summary>
        /// Gets or sets the outlier z score
        /// </summary>
        public double OutlierZScore { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns all values to their original setting and clears all containers.
        /// </summary>
        public void Clear()
        {
            NumXBins = 0;
            m_numYBins = 0;
            m_numJumps = 0;
            m_tolerance = 0.8;
            OutlierZScore = 0.8;
            m_minSectionPoints = 5;
            m_normUnifEM.Clear();
            m_matchScores.Clear();
            m_alignmentScores.Clear();
            m_alignmentFunction.Clear();
            m_bestPreviousIndex.Clear();
            m_count.Clear();
            RegressionPoints.Clear();
            m_standardDeviationsY.Clear();
            m_sectionMismatchScore.Clear();
            m_sectionTolerance.Clear();
        }

        /// <summary>
        /// TODO: Create comment block for PrintScoreMatrix
        /// </summary>
        /// <param name="fileName"></param>
        public void PrintScoreMatrix(string fileName)
        {
            // TODO: Implement PrintScoreMatrix
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for PrintAlignmentScoreMatrix
        /// </summary>
        /// <param name="fileName"></param>
        public void PrintAlignmentScoreMatrix(string fileName)
        {
            // TODO: Implement PrintAlignmentScoreMatrix
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for PrintRegressionFunction
        /// </summary>
        /// <param name="fileName"></param>
        public void PrintRegressionFunction(string fileName)
        {
            // TODO: Implement PrintRegressionFunction
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for PrintPoints
        /// </summary>
        /// <param name="fileName"></param>
        public void PrintPoints(string fileName)
        {
            // TODO: Implement PrintPoints
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for SetOptions
        /// </summary>
        /// <param name="numXBins"></param>
        /// <param name="numYBins"></param>
        /// <param name="numJumps"></param>
        /// <param name="zTolerance"></param>
        public void SetOptions(int numXBins, int numYBins, int numJumps, double zTolerance)
        {
            // TODO: Implement SetOptions
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for GetPredictedValues
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double GetPredictedValue(double x)
        {
            // TODO: Implement GetPredictedValue
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for CalculateRegressionFunction
        /// </summary>
        /// <param name="calibMatches"></param>
        public void CalculateRegressionFunction(List<RegressionPoints> calibMatches)
        {
            // TODO: Implement CalculateRegressionFunction
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for RemoveRegressionOutliers
        /// </summary>
        public void RemoveRegressionOutliers()
        {
            // TODO: Implement RemoveRegressionOutliers
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// TODO: Create comment block for CalculateMinMax
        /// </summary>
        private void CalculateMinMax()
        {
            // TODO: Implement CalculateMinMax
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for CalculateSectionsStd
        /// </summary>
        private void CalculateSectionsStd()
        {
            // TODO: Implement CalculateSectionsStd
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for CalculateSectionsStdAndCount
        /// </summary>
        /// <param name="sectionNum"></param>
        private void CalculateSectionsStdAndCount(int sectionNum)
        {
            // TODO: Implement CalculateSectionsStdAndCount
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for SetUnmatchedScoreMatrix
        /// </summary>
        private void SetUnmatchedScoreMatrix()
        {
            // TODO: Implement SetUnmatchedScoreMatrix
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for CalculateScoreMatrix
        /// </summary>
        private void CalculateScoreMatrix()
        {
            // TODO: Implement CalculateScoreMatrix
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for CalculateAlignmentMatrix
        /// </summary>
        private void CalculateAlignmentMatrix()
        {
            // TODO: Implement CalculateAlignmentMatrix
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for CalculateRegressionFunction
        /// </summary>
        private void CalculateRegressionFunction()
        {
            // TODO: Implement CalculateRegressionFunction
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create comment block for RemoveOutliersAndCopy
        /// </summary>
        /// <param name="calibMatches"></param>
        private void RemoveOutliersAndCopy(List<RegressionPoints> calibMatches)
        {
            // TODO: Implement RemoveOutliersAndCopy
            throw new NotImplementedException();
        }
        #endregion
    }
}