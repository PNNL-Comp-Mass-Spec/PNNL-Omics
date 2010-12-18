using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.PeakDetection
{
    public class PeakThresholdParameters
    {
        public bool isDataThresholded { get; set; }
        public List<XYData> ThresholdedPeakData { get; set; }//results Npoints Long
        public List<double> ThresholdedPeakFWHM { get; set; }//results NPoints Long
        public List<double> ThresholdedPeakSNShoulder { get; set; }//resutls NpointsLong
        public List<double> ThresholdedPeakSN { get; set; }//resutls NpointsLong
        public List<double> ThresholdedPeakSignalToBackground { get; set; }//results NpointsLong
        public List<ThresholdpeakObject> ThresholdedObjectlist { get; set; }//NpointsLong
        public float SignalToShoulderCuttoff { get; set; }
        public string ThresholdMethod { get; set; }
        public int ScanNumber { get; set; }

        public PeakThresholdParameters()
        {
            this.isDataThresholded = false;
            this.ThresholdedPeakData = new List<XYData>();
            this.ThresholdedPeakFWHM = new List<double>();
            this.ThresholdedPeakSNShoulder = new List<double>();
            this.ThresholdedPeakSN = new List<double>();
            this.ThresholdedPeakSignalToBackground = new List<double>();
            this.SignalToShoulderCuttoff = 3;
            this.ThresholdedObjectlist = new List<ThresholdpeakObject>();
            this.ThresholdMethod = "AveragePlusSigma";
            this.ScanNumber = 0;
        }
    }

    public class ThresholdpeakObject
    {
        public XYData PeakData { get; set; }
        public double PeakFWHM { get; set; }
        public double PeakSNShoulder { get; set; }
        public double PeakSignalToBackground { get; set; }
        public double PeakSN { get; set; }

        public ThresholdpeakObject()
        {
            this.PeakData = new XYData(0, 0);
            this.PeakFWHM = 0;
            this.PeakSNShoulder = 0;
            this.PeakSignalToBackground = 0;
            this.PeakSN = 0;
        }
    }
}
