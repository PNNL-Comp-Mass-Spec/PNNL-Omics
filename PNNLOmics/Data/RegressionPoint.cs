namespace PNNLOmics.Data
{
    /// <summary>
    /// Object used for the LCMS Warp regressions, has a Net (or MZ), Mass error and Net Error
    /// </summary>
    public class RegressionPoint
    {
        /// <summary>
        /// AutoProperty for the Net (or MZ) of a calibratin match
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// AutoProperty for the PPM of a calibration match
        /// </summary>
        public double MassError { get; set; }

        /// <summary>
        /// AutoProperty for the NET Error of a calibration match
        /// </summary>
        public double NetError { get; set; }
    }
}
