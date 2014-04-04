namespace PNNLOmics.Alignment.LCMSWarp.LCMSProcessor
{
    /// <summary>
    /// Class that defines the boundary of an MZ range to align to
    /// Holds a lower bound and an upper bound for comparison
    /// </summary>
    public class LCMSAlignmentMzBoundary
    {
        /// <summary>
        /// Lower Boundary auto property
        /// </summary>
        public double BoundaryLow { get; set; }

        /// <summary>
        /// Upper Boundary auto property
        /// </summary>
        public double BoundaryHigh { get; set; }

        /// <summary>
        /// Constructor, initializing bounds to the high and low passed in
        /// </summary>
        /// <param name="low"></param>
        /// <param name="high"></param>
        public LCMSAlignmentMzBoundary(double low, double high)
        {
            BoundaryHigh = high;
            BoundaryLow = low;
        }
    }
}
