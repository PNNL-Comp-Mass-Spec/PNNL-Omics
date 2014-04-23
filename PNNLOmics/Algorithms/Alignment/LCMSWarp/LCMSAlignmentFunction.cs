using System;
using System.Collections.Generic;

namespace PNNLOmics.Alignment.LCMSWarp.LCMSProcessor
{
    /// <summary>
    /// Object to house the Alignment Function from LCMS warping, including the
    /// calibration and alignment type
    /// </summary>
    public class LcmsAlignmentFunction
    {
        readonly LcmsAlignmentOptions.CalibrationType m_calibrationType;
        readonly LcmsAlignmentOptions.AlignmentType m_alignmentType;

        readonly List<double> m_netFuncTimeInput = new List<double>();
        readonly List<double> m_netFuncNetOutput = new List<double>();

        readonly List<double> m_netFuncTimeOutput = new List<double>();

        readonly List<double> m_massFuncTimeInput = new List<double>();
        readonly List<double> m_massFuncTimePpmOutput = new List<double>();

        readonly List<double> m_massFuncMzInput = new List<double>();
        readonly List<double> m_massFuncMzppmOutput = new List<double>();

        /// <summary>
        /// AutoProperty for the name of the Alignee dataset
        /// </summary>
        public string Dataset { get; set; }

        /// <summary>
        /// AutoProperty for the name of the Baseline dataset
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Constructs the Alignment function data members, copying the alignee and reference names, the
        /// calibration type and the alignment type.
        /// </summary>
        /// <param name="alignee"></param>
        /// <param name="reference"></param>
        /// <param name="calibType"></param>
        /// <param name="alignmentType"></param>
        public LcmsAlignmentFunction(string alignee, string reference,
                                 LcmsAlignmentOptions.CalibrationType calibType,
                                 LcmsAlignmentOptions.AlignmentType alignmentType)
        {
            Dataset = alignee;
            Reference = reference;
            m_calibrationType = calibType;
            m_alignmentType = alignmentType;
        }

        /// <summary>
        /// Constructs the Alignment function data members, used when there isn't a specified name for the
        /// alignee or reference names, sets up the calibration type and alignment type.
        /// </summary>
        /// <param name="calibType"></param>
        /// <param name="alignmentType"></param>
        public LcmsAlignmentFunction(LcmsAlignmentOptions.CalibrationType calibType,
                                 LcmsAlignmentOptions.AlignmentType alignmentType)
        {
            m_calibrationType = calibType;
            m_alignmentType = alignmentType;
        }

        /// <summary>
        /// If the value valX is contained within the list x, it returns the value at that index.
        /// Otherwise, BinarySearch will return the bitwise complement of the next larger index.
        /// If interpolate last is set to false, and valX is greater than the largest value in x,
        /// it returns the last value for y.
        /// In every other case, it will determine the linear interpolated value of y that valX would
        /// correspond to and returns that.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="valX"></param>
        /// <param name="interpolateLast"></param>
        /// <returns></returns>
        public double Interpolate(List<double> x, List<double> y, double valX, bool interpolateLast)
        {
            int index = x.BinarySearch(valX);
            if (index < 0)
            {
                // the next higher value
                index = ~index;
                if (index == x.Count)
                {
                    if (!interpolateLast)
                    {
                        return y[index - 1];
                    }
                    index--;
                    double xUpper = x[index];
                    double xLower = x[index - 1];
                    double yUpper = y[index];
                    double yLower = y[index - 1];

                    // Perform a linear interpolation between the two points
                    return ((valX - xLower) / (xUpper - xLower) * yUpper + (xUpper - valX) / (xUpper - xLower) * yLower);
                }
            }
            // exact match
            return y[index];
        }

        /// <summary>
        /// Sets up the NET function with relation to time
        /// Throwing exceptions if this is called without NET calibration data
        /// </summary>
        /// <param name="time"></param>
        /// <param name="net"></param>
        public void SetNetFunction(List<double> time, List<double> net)
        {
            if (time.Count == 0)
            {
                throw new ArgumentException("Input NET Calibration with time has no time data.");
            }
            if (net.Count == 0)
            {
                throw new ArgumentException("Input NET Calibration with time has no NET data.");
            }

            foreach (double value in time)
            {
                m_netFuncTimeInput.Add(value);
            }
            foreach (double value in time)
            {
                m_netFuncNetOutput.Add(value);
            }
        }

        /// <summary>
        /// Sets calibration of ppm with relation to time.
        /// Only to be used with MZ calibration and NET-MASS warping. Will throw exception if either of those is not true
        /// or if there is no data for ppms.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="ppm"></param>
        public void SetMassCalibrationFunctionwithTime(List<double> time, List<double> ppm)
        {
            if (ppm.Count == 0)
            {
                throw new ArgumentException("Input Mass Calibration Function with time has no ppm data.");
            }

            if (m_calibrationType == LcmsAlignmentOptions.CalibrationType.MZ_CALIBRATION)
            {
                throw new InvalidOperationException("Attempting to set time calibration of masses when option chosen was MZ_CALIB");
            }

            if (m_alignmentType != LcmsAlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                throw new InvalidOperationException("Recalibration of mass not enabled with NET_WARP alignment type. PPM shift cannot be retrieved. Used NET_MASS_WARP as alignment type instead");
            }
            foreach (double value in time)
            {
                m_massFuncTimeInput.Add(value);
            }
            foreach (double value in ppm)
            {
                m_massFuncTimePpmOutput.Add(value);
            }
        }

        /// <summary>
        /// Sets up the mass calibration function using ppm and mz.
        /// Throws exceptions if there is no PPM data, alignment type is not of NET-Mass warping or if calibration type is
        /// not of Scan calibration type.
        /// </summary>
        /// <param name="mz"></param>
        /// <param name="ppm"></param>
        public void SetMassCalibrationFunctionWithMz(List<double> mz, List<double> ppm)
        {
            if (ppm.Count == 0)
            {
                throw new ArgumentException("Input Mass Calibration Function with time has no ppm data.");
            }

            if (m_alignmentType != LcmsAlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                throw new InvalidOperationException("Recalibration of mass not enabled with NET_WARP alignment type. PPM shift cannot be retrieved. Used NET_MASS_WARP as alignment type instead");
            }

            if (m_calibrationType == LcmsAlignmentOptions.CalibrationType.SCAN_CALIBRATION)
            {
                throw new InvalidOperationException("Attempting to set MZ calibration of masses when option chosen was SCAN_CALIBRATION");
            }

            foreach (double value in mz)
            {
                m_massFuncMzInput.Add(value);
            }
            foreach (double value in ppm)
            {
                m_massFuncMzppmOutput.Add(value);
            }
        }

        /// <summary>
        /// Determines the appropriate value for the time passed in to the NET Calibration function.
        /// Does not interpolate the last index; returns the max value if time exceeds calibration range
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public double GetNetFromTime(double time)
        {
            return Interpolate(m_netFuncTimeInput, m_netFuncNetOutput, time, false);
        }

        /// <summary>
        /// Determines the appropriate value for the time passed in to the PPM Calibration function.
        /// Does not interpolate the last index; returns the max value if time exceeds calibration range.
        /// Throws exceptions if Alignment is not for a NET-Mass warp or if calibration is not by MZ
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public double GetPpmShiftFromTime(double time)
        {
            if (m_alignmentType != LcmsAlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                throw new InvalidOperationException("Recalibration of masses not enabled with NET_WARP alignment type. PPM shift cannot be retrieved");
            }
            if (m_calibrationType == LcmsAlignmentOptions.CalibrationType.MZ_CALIBRATION)
            {
                throw new InvalidOperationException("Attempting to get time calibration of masses when option chosen was MZ_CALIBRATION");
            }

            return Interpolate(m_massFuncTimeInput, m_massFuncTimePpmOutput, time, false);
        }

        /// <summary>
        /// Determines the appropriate value for the mz passed in to the PPM Calibration function.
        /// Does not interpolate the last index; returns the max value if time exceeds calibration range.
        /// Throws exceptions if Alignment is not for a NET-Mass warp or if calibration is not by MZ
        /// </summary>
        /// <param name="mz"></param>
        /// <returns></returns>
        public double GetPpmShiftFromMz(double mz)
        {
            if (m_alignmentType != LcmsAlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                throw new InvalidOperationException("Recalibration of masses not enabled with NET_WARP alignment type. PPM shift cannot be retrieved");
            }
            if (m_calibrationType == LcmsAlignmentOptions.CalibrationType.SCAN_CALIBRATION)
            {
                throw new InvalidOperationException("Attempting to get mz calibration of masses when option chosen was SCAN_CALIBRATION");
            }

            return Interpolate(m_massFuncMzInput, m_massFuncMzppmOutput, mz, false);
        }

        /// <summary>
        /// Calculates the appropriate shift by using both the time from the PPM shift with respect to time summed with
        /// the mz from the PPM shift with respect to Mz.
        /// Throws exceptions if alignment type isn't NET-Mass or if the calibration type is not for Hybrid calibration
        /// </summary>
        /// <param name="time"></param>
        /// <param name="mz"></param>
        /// <returns></returns>
        public double GetPpmShiftFromTimeMz(double time, double mz)
        {
            if (m_alignmentType != LcmsAlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                throw new InvalidOperationException("Recalibration of masses not enabled with NET_WARP alignment type. PPM shift cannot be retrieved");
            }
            if (m_calibrationType != LcmsAlignmentOptions.CalibrationType.HYBRID_CALIBRATION)
            {
                throw new InvalidOperationException("Attempting to get hybrid calibration of masses when option chosen was not HYBRID_CALIB");
            }

            return GetPpmShiftFromMz(mz) + GetPpmShiftFromTime(time);
        }

        /// <summary>
        /// Sets up Net function using alignee times and reference times, function requires initialized lists
        /// </summary>
        /// <param name="aligneeTimes"></param>
        /// <param name="referenceNets"></param>
        public void SetNetFunction(ref List<double> aligneeTimes, ref List<double> referenceNets)
        {
            foreach (double value in aligneeTimes)
            {
                m_netFuncTimeInput.Add(value);
            }
            foreach (double value in referenceNets)
            {
                m_netFuncNetOutput.Add(value);
            }
        }

        /// <summary>
        /// Sets up Net function using alignee times and reference times as well as the reference scans in the 
        /// time output, function requires initialized lists
        /// </summary>
        /// <param name="aligneeTimes"></param>
        /// <param name="referenceNets"></param>
        /// <param name="referenceScans"></param>
        public void SetNetFunction(ref List<double> aligneeTimes, ref List<double> referenceNets,
                            ref List<double> referenceScans)
        {
            foreach (double value in aligneeTimes)
            {
                m_netFuncTimeInput.Add(value);
            }
            foreach (double value in referenceNets)
            {
                m_netFuncNetOutput.Add(value);
            }
            foreach (double value in referenceScans)
            {
                m_netFuncTimeOutput.Add(value);
            }
        }

        /// <summary>
        /// Sets up Mass Calibration with respect to time, function requires initialized lists
        /// </summary>
        /// <param name="aligneeTimes"></param>
        /// <param name="ppmShifts"></param>
        public void SetMassCalibrationFunctionWithTime(ref List<double> aligneeTimes, ref List<double> ppmShifts)
        {
            foreach (double value in aligneeTimes)
            {
                m_massFuncTimeInput.Add(value);
            }
            foreach (double value in ppmShifts)
            {
                m_massFuncTimePpmOutput.Add(value);
            }
        }

        /// <summary>
        /// Sets up mass calibration with respect to MZ, function requires initialized lists
        /// </summary>
        /// <param name="aligneeMZs"></param>
        /// <param name="ppmShifts"></param>
        public void SetMassCalibrationFunctionWithMz(ref List<double> aligneeMZs, ref List<double> ppmShifts)
        {
            foreach (double value in aligneeMZs)
            {
                m_massFuncMzInput.Add(value);
            }
            foreach (double value in ppmShifts)
            {
                m_massFuncMzppmOutput.Add(value);
            }
        }
    }
}
