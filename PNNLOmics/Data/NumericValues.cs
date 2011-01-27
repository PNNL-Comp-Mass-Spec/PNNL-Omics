namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// This class is simply a container for numeric values used in the alignment library
    /// </summary>
    public static class NumericValues
    {
        #region LCMSWarp Algorithm Defaults
        /// <summary>
        /// Gets the default NET standard deviation
        /// </summary>
        public static double DefaultNETStandardDeviation
        {
            get { return 0.007; }
        }

        /// <summary>
        /// Gets the default NET tolerance level
        /// </summary>
        public static double DefaultNETTolerance
        {
            get { return 0.02; }
        }

        /// <summary>
        /// Gets the default mass standard deviation
        /// </summary>
        public static double DefaultMassStandardDeviation
        {
            get { return 20.0; }
        }

        /// <summary>
        /// Gets the default mass tolerance level
        /// </summary>
        public static double DefaultMassTolerance
        {
            get { return 20.0; }
        }
        #endregion

        #region Numbers
        /// <summary>
        /// Represents (1 / 1,000,000)
        /// </summary>
        public static double OneMillionth
        {
            get { return 0.000001; }
        }
        #endregion
    }
}