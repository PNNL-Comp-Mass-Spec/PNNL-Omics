using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using PNNLOmics.Data;
using PNNLOmics.Algorithms.PeakDetection;
using System.Collections.ObjectModel;

namespace PNNLOmics.UnitTests.AlgorithmTests.PeakDetectorTests
{
    [TestFixture]
    public class PeakDetectorTests
    {
        [Test]
        public void PeakDetectorV3_DiscoverPeaks_only_no_ThresholdingTest()
        {
            float[] xvals = null;
            float[] yvals = null;

            loadTestScanData(ref xvals, ref yvals);
            Assert.That(xvals != null);
            Assert.AreEqual(322040, xvals.Length);
            
            List<XYData> testXYData = convertXYDataToOMICSXYData(xvals, yvals);

            PeakCentroider newPeakCentroider = new PeakCentroider();
            List<ProcessedPeak> centroidedPeakList = newPeakCentroider.DiscoverPeaks(testXYData);

            displayPeakData(centroidedPeakList);

            Assert.That(centroidedPeakList.Count > 0);
            Assert.AreEqual(68756, centroidedPeakList.Count);
        }

        [Test]
        public void PeakDetectorV3_DiscoverPeaks_Then_Threshold()
        {
            float[] xvals = null;
            float[] yvals = null;

            loadTestScanData(ref xvals, ref yvals);
            Assert.That(xvals != null);
            Assert.AreEqual(322040, xvals.Length);

            List<XYData> testXYData = convertXYDataToOMICSXYData(xvals, yvals);

            PeakCentroider newPeakCentroider = new PeakCentroider();
            newPeakCentroider.Parameters.ScanNumber = 0;
            List<ProcessedPeak> centroidedPeakList = newPeakCentroider.DiscoverPeaks(testXYData);

            PeakThresholder newPeakThresholder = new PeakThresholder();
            newPeakThresholder.Parameters.SignalToShoulderCuttoff = 3f;
            List<ProcessedPeak> thresholdedData = newPeakThresholder.ApplyThreshold(ref centroidedPeakList);

            Console.WriteLine("Non-thresholded Candidate Peaks detected = " + centroidedPeakList.Count);

            Assert.That(thresholdedData.Count > 0);
            Assert.AreEqual(thresholdedData.Count, 6);
            
            displayPeakData(thresholdedData);
            Console.WriteLine();
            Console.WriteLine("Thresholded Peaks detected = " + thresholdedData.Count);
        }

        [Test]
        public void PeakDetectorTest()
        {
            float[] xvals = null;
            float[] yvals = null;

            loadTestScanData(ref xvals, ref yvals);
            Assert.That(xvals != null);
            Assert.AreEqual(322040, xvals.Length);

            List<PNNLOmics.Data.XYData> testXYData = convertXYDataToOMICSXYData(xvals, yvals);
            Collection<XYData> dataInput = new Collection<XYData>(testXYData);

            KronewitterPeakDetector newPeakDetector = new KronewitterPeakDetector();
            Collection<Peak> finalPeakList = newPeakDetector.DetectPeaks(dataInput);

            Console.WriteLine("We found " + finalPeakList.Count + " Peaks.");
            Assert.AreEqual(finalPeakList.Count, 6);
        }
        
        [Test]
        public void PeakDetectorV1_DiscoverPeaks_Test1()
        {
            float[] xvals = null;
            float[] yvals = null;

            loadTestScanData(ref xvals, ref yvals);
            Assert.That(xvals != null);
            Assert.AreEqual(322040, xvals.Length);

            double[] xvalsDouble = new double[xvals.Length]; 
            double[] yvalsDouble = new double[xvals.Length]; 

            for (int i = 0; i < xvals.Length; i++)
            {
                xvalsDouble[i] = (double)(xvals[i]);
                yvalsDouble[i] = (double)(yvals[i]);
            }
            double peakBR = 2;
            double sigNoiseThresh = 3;

            DeconTools.Backend.XYData xydata = new DeconTools.Backend.XYData();
            xydata.Xvalues = xvalsDouble;
            xydata.Yvalues = yvalsDouble;

            DeconTools.Backend.ProcessingTasks.DeconToolsPeakDetector peakdetector = new DeconTools.Backend.ProcessingTasks.DeconToolsPeakDetector(peakBR, sigNoiseThresh, DeconTools.Backend.Globals.PeakFitType.QUADRATIC, false);
            List<DeconTools.Backend.Core.IPeak> peakList = peakdetector.FindPeaks(xydata, 0, 50000);

            displayPeakData(peakList);

            Assert.AreEqual(peakList.Count, 6);
        }
 
        #region private functions
        private void displayPeakData(List<DeconTools.Backend.Core.IPeak> peakList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in peakList)
            {
                sb.Append(item.XValue);
                sb.Append('\t');
                sb.Append(item.Height);
                sb.Append('\t');

                sb.Append(item.Width);
                sb.Append(Environment.NewLine);
            }
            Console.WriteLine(sb.ToString());
        }

        private void displayPeakData(List<ProcessedPeak> centroidedPeakList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in centroidedPeakList)
            {
                sb.Append(item.XValue);
                sb.Append('\t');
                sb.Append(item.Height);
                sb.Append('\t');

                sb.Append(item.Width);
                sb.Append(Environment.NewLine);
            }
            Console.WriteLine(sb.ToString());
        }

        private List<XYData> convertXYDataToOMICSXYData(float[] xvals, float[] yvals)
        {
            List<XYData> xydataList = new List<XYData>();
            for (int i = 0; i < xvals.Length; i++)
            {
                XYData xydatapoint = new XYData(xvals[i], yvals[i]);
                xydataList.Add(xydatapoint);
            }
            return xydataList;
        }

        private void loadTestScanData(ref float[] xvals, ref float[] yvals)
        {
            double[] tempXVals = null;
            double[] tempYVals = null;

            loadTestScanData(ref tempXVals, ref tempYVals);

            xvals = tempXVals.Select<double, float>(i => (float)i).ToArray();
            yvals = tempYVals.Select<double, float>(i => (float)i).ToArray();
        }

        private void loadTestScanData(ref double[] xvals, ref double[] yvals)
        {
            string testScanFilePath = @"\\protoapps\UserData\Slysz\DeconTools_TestFiles\SyntheticMS\SyntheticMSScan00.txt";
            
            DeconTools.Backend.Runs.RunFactory runFactory = new DeconTools.Backend.Runs.RunFactory();
            DeconTools.Backend.Core.Run run = runFactory.CreateRun(testScanFilePath);

            DeconTools.Backend.Core.ScanSet scan = new DeconTools.Backend.Core.ScanSet(0);
            run.GetMassSpectrum(scan);

            //run.XYData.Display();
            xvals = run.XYData.Xvalues;
            yvals = run.XYData.Yvalues;
        }

        #endregion
    }
}
