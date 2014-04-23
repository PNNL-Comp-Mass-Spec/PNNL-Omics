using System;
using System.Collections.Generic;

namespace Processor
{
    public class AlignmentFunction
    {
        string m_dataset;
        string m_reference;
        AlignmentOptions.CalibrationType m_calibrationType;
        AlignmentOptions.AlignmentType m_alignmentType;

        List<double> m_NETFuncTimeInput = new List<double>();
        List<double> m_NETFuncNETOutput = new List<double>();

        List<double> m_NETFuncTimeOutput = new List<double>();

        List<double> m_MassFuncTimeInput = new List<double>();
        List<double> m_MassFuncTimePPMOutput = new List<double>();

        List<double> m_MassFuncMZInput = new List<double>();
        List<double> m_MassFuncMZPPMOutput = new List<double>();

        public string Dataset
        {
            get { return m_dataset; }
            set { m_dataset = value; }
        }

        public string Reference
        {
            get { return m_reference; }
            set { m_reference = value; }
        }

        public AlignmentFunction(string alignee, string reference,
                                 AlignmentOptions.CalibrationType calibType,
                                 AlignmentOptions.AlignmentType alignmentType)
        {
            m_dataset = alignee;
            m_reference = reference;
            m_calibrationType = calibType;
            m_alignmentType = alignmentType;
        }

        public AlignmentFunction(AlignmentOptions.CalibrationType calibType,
                                 AlignmentOptions.AlignmentType alignmentType)
        {
            m_calibrationType = calibType;
            m_alignmentType = alignmentType;
        }


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
                m_NETFuncTimeInput.Add(value);
            }
            foreach (double value in time)
            {
                m_NETFuncNETOutput.Add(value);
            }
        }

        public void SetMassCalibrationFunctionwithTime(List<double> time, List<double> ppm)
        {
            if (ppm.Count == 0)
            {
                throw new ArgumentException("Input Mass Calibration Function with time has no ppm data.");
            }

            if (m_calibrationType == AlignmentOptions.CalibrationType.MZ_CALIBRATION)
            {
                throw new InvalidOperationException("Attempting ot set time calibration of masses when option chosen was MZ_CALIB");
            }

            if (m_alignmentType != AlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                throw new InvalidOperationException("Recalibration of mass not enabled with NET_WARP alignment type. PPM shift cannot be retrieved. Used NET_MASS_WARP as alignment type instead");
            }
            foreach (double value in time)
            {
                m_MassFuncTimeInput.Add(value);
            }
            foreach (double value in ppm)
            {
                m_MassFuncTimePPMOutput.Add(value);
            }
        }

        public void SetMassCalibrationFunctionWithMz(List<double> mz, List<double> ppm)
        {
            if (ppm.Count == 0)
            {
                throw new ArgumentException("Input Mass Calibration Function with time has no ppm data.");
            }

            if (m_alignmentType != AlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                throw new InvalidOperationException("Recalibration of mass not enabled with NET_WARP alignment type. PPM shift cannot be retrieved. Used NET_MASS_WARP as alignment type instead");
            }

            if (m_calibrationType == AlignmentOptions.CalibrationType.SCAN_CALIBRATION)
            {
                throw new InvalidOperationException("Attempting to set MZ calibration of masses when option chosen was SCAN_CALIBRATION");
            }

            foreach (double value in mz)
            {
                m_MassFuncMZInput.Add(value);
            }
            foreach (double value in ppm)
            {
                m_MassFuncMZPPMOutput.Add(value);
            }
        }


        public double GetNetFromTime(double time)
        {
            return Interpolate(m_NETFuncTimeInput, m_NETFuncNETOutput, time, false);
        }

        public double GetPpmShiftFromTime(double time)
        {
            if (m_alignmentType != AlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                throw new InvalidOperationException("Recalibration of masses not enabled with NET_WARP alignment type. PPM shift cannot be retrieved");
            }
            if (m_calibrationType == AlignmentOptions.CalibrationType.MZ_CALIBRATION)
            {
                throw new InvalidOperationException("Attempting to get time calibration of masses when option chosen was MZ_CALIBRATION");
            }

            return Interpolate(m_MassFuncTimeInput, m_MassFuncTimePPMOutput, time, false);
        }

        public double GetPpmShiftFromMz(double mz)
        {
            if (m_alignmentType != AlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                throw new InvalidOperationException("Recalibration of masses not enabled with NET_WARP alignment type. PPM shift cannot be retrieved");
            }
            if (m_calibrationType == AlignmentOptions.CalibrationType.SCAN_CALIBRATION)
            {
                throw new InvalidOperationException("Attempting to get mz calibration of masses when option chosen was SCAN_CALIBRATION");
            }

            return Interpolate(m_MassFuncMZInput, m_MassFuncMZPPMOutput, mz, false);
        }

        public double GetPpmShiftFromTimeMz(double time, double mz)
        {
            if (m_alignmentType != AlignmentOptions.AlignmentType.NET_MASS_WARP)
            {
                throw new InvalidOperationException("Recalibration of masses not enabled with NET_WARP alignment type. PPM shift cannot be retrieved");
            }
            if (m_calibrationType != AlignmentOptions.CalibrationType.HYBRID_CALIBRATION)
            {
                throw new InvalidOperationException("Attempting to get hybrid calibration of masses when option chosen was not HYBRID_CALIB");
            }

            return GetPpmShiftFromMz(mz) + GetPpmShiftFromTime(time);
        }

        public void SetNetFunction(ref List<double> aligneeTimes, ref List<double> referenceNets)
        {
            foreach (double value in aligneeTimes)
            {
                m_NETFuncTimeInput.Add(value);
            }
            foreach (double value in referenceNets)
            {
                m_NETFuncNETOutput.Add(value);
            }
        }

        public void SetNetFunction(ref List<double> aligneeTimes, ref List<double> referenceNets,
                            ref List<double> referenceScans)
        {
            foreach (double value in aligneeTimes)
            {
                m_NETFuncTimeInput.Add(value);
            }
            foreach (double value in referenceNets)
            {
                m_NETFuncNETOutput.Add(value);
            }
            foreach (double value in referenceScans)
            {
                m_NETFuncTimeOutput.Add(value);
            }
        }

        public void SetMassCalibrationFunctionWithTime(ref List<double> aligneeTimes, ref List<double> ppmShifts)
        {
            foreach (double value in aligneeTimes)
            {
                m_MassFuncTimeInput.Add(value);
            }
            foreach (double value in ppmShifts)
            {
                m_MassFuncTimePPMOutput.Add(value);
            }
        }

        public void SetMassCalibrationFunctionWithMz(ref List<double> aligneeMZs, ref List<double> ppmShifts)
        {
            foreach (double value in aligneeMZs)
            {
                m_MassFuncMZInput.Add(value);
            }
            foreach (double value in ppmShifts)
            {
                m_MassFuncMZPPMOutput.Add(value);
            }
        }
    }
}
