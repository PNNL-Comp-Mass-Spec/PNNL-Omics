using System;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// TODO: Create class comment block for RegressionPoints
    /// </summary>
    public class RegressionPoints
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of RegressionPoints
        /// </summary>
        public RegressionPoints()
        {
            Clear();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets X
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the mass error
        /// </summary>
        public double MassError { get; set; }

        /// <summary>
        /// Gets or sets the net error
        /// </summary>
        public double NetError { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears all values to their original setting
        /// </summary>
        private void Clear()
        {
            X = 0.0;
            MassError = 0.0;
            NetError = 0.0;
        }

        /// <summary>
        /// Sets all values to the provided setting
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="massError">Mass error</param>
        /// <param name="netError">Net error</param>
        public void Set(double x, double massError, double netError)
        {
            X = x;
            MassError = massError;
            NetError = netError;
        }
        #endregion
    }
}