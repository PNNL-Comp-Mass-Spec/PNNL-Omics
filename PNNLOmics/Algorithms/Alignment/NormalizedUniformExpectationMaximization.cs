using System;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// TODO: Create class comment block for NormalizedUniformExpectationMaximization
    /// </summary>
    public class NormalizedUniformExpectationMaximization
    {
        #region Class Members
        private double m_mean;
        private double m_var;
        private double m_normalizedFraction;
        private int m_numIterations;
        private List<double> m_uniformProbabilites;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of NormalizedUniformExpectationMaximization
        /// </summary>
        public NormalizedUniformExpectationMaximization()
        {
            m_uniformProbabilites = new List<double>();
            Clear();
        }
        #endregion

        #region Properties
        /// <summary>
        /// TODO: Comment StandardDeviation property 
        /// </summary>
        public double StandardDeviation
        {
            get { return Math.Sqrt(m_var); }
        }

        /// <summary>
        /// Gets the mean.
        /// </summary>
        public double Mean
        {
            get { return m_mean; }
        }

        /// <summary>
        /// Gets the normalized probability.
        /// </summary>
        public double NormalizedProbability
        {
            get { return m_normalizedFraction; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears all values to their original setting.
        /// </summary>
        public void Clear()
        {
            m_mean = 0.0;
            m_var = 10.0;
            m_normalizedFraction = 0.5;
            m_numIterations = 16;
        }

        /// <summary>
        /// TODO: Create comment block for CalculateDistributions
        /// </summary>
        /// <param name="values"></param>
        public void CalculateDistributions(List<double> values)
        {
            // TODO: Implement CalculateDistributions
            throw new NotImplementedException();
        }
        #endregion
    }
}