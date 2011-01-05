using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using PNNLOmics.Algorithms.PeakDetector;
using PNNLOmics.Data;
using System.IO;
using PNNLOmics.Algorithms.PeakDetection;
using PNNLOmics.Algorithms.PeakDetectorController;

namespace PNNLOmics.UnitTests.AlgorithmTests.PeakDetectorTests
{
    [TestFixture]
    public class PeakDetectorTests
    {
        [Test]
        public void PeakDetectorV3_DiscoverPeaks_no_ThresholdingTest1()
        {

            float[] xvals = null;
            float[] yvals = null;

            int scanNum = 0;

            loadTestScanData(ref xvals, ref yvals);
            Assert.That(xvals != null);
            Assert.AreEqual(322040, xvals.Length);

            List<PNNLOmics.Data.XYData> testXYData = convertXYDataToOMICSXYData(xvals, yvals);

            PeakCentroidParameters parametersPeakCentroid = new PeakCentroidParameters();
            parametersPeakCentroid.ScanNumber = scanNum;

            List<ProcessedPeak> centroidedPeakList = new List<ProcessedPeak>();
            centroidedPeakList = PeakCentroid.DiscoverPeaks(testXYData, parametersPeakCentroid);

            Assert.That(centroidedPeakList.Count > 0);

            displayPeakData(centroidedPeakList);
        }

        [Test]
        public void PeakDetectorV3_DiscoverPeaksThenThresholdAsOne()
        {
            float[] xvals = null;
            float[] yvals = null;

            loadTestScanData(ref xvals, ref yvals);
            Assert.That(xvals != null);
            Assert.AreEqual(322040, xvals.Length);

            List<PNNLOmics.Data.XYData> testXYData = convertXYDataToOMICSXYData(xvals, yvals);

        
            List<ProcessedPeak> processedData = new List<ProcessedPeak>();
            PeakDetectorParameters parametersPeakDetector = new PeakDetectorParameters();
            PeakDetectorController newPeakDetector = new PeakDetectorController();
            processedData = newPeakDetector.DetectPeaks(testXYData,parametersPeakDetector);

            Assert.That(processedData.Count > 0);
            Assert.AreEqual(processedData.Count ,6);
            displayPeakData(processedData);
            Console.WriteLine();
            Console.WriteLine("Thresholded Peaks detected = " + processedData.Count);
        }

        [Test]
        public void PeakDetectorV3_DiscoverPeaksThenThreshold()
        {
            float[] xvals = null;
            float[] yvals = null;

            loadTestScanData(ref xvals, ref yvals);
            Assert.That(xvals != null);
            Assert.AreEqual(322040, xvals.Length);

            List<PNNLOmics.Data.XYData> testXYData = convertXYDataToOMICSXYData(xvals, yvals);

            PeakCentroidParameters parametersPeakCentroid = new PeakCentroidParameters();
            parametersPeakCentroid.ScanNumber = 0;

            List<ProcessedPeak> centroidedPeakList = new List<ProcessedPeak>();
            centroidedPeakList = PeakCentroid.DiscoverPeaks(testXYData, parametersPeakCentroid);

            PeakThresholdParameters parametersThreshold = new PeakThresholdParameters();
            parametersThreshold.SignalToShoulderCuttoff = 3f;

            List<ProcessedPeak> thresholdedData = new List<ProcessedPeak>();
            thresholdedData = PeakThreshold.ApplyThreshold(ref centroidedPeakList, parametersThreshold);

            Console.WriteLine("Non-thresholded Candidate Peaks detected = " + centroidedPeakList.Count);

            Assert.That(thresholdedData.Count > 0);
            Assert.AreEqual(thresholdedData.Count, 6);
            displayPeakData(thresholdedData);
            Console.WriteLine();
            Console.WriteLine("Thresholded Peaks detected = " + thresholdedData.Count);
        }

        [Test]
        public void PeakDetectorV3_DiscoverPeaksThenThresholdAsPieces()
        {
            float[] xvals = null;
            float[] yvals = null;

            loadTestScanData(ref xvals, ref yvals);
            Assert.That(xvals != null);
            Assert.AreEqual(322040, xvals.Length);


            int scanNum = 0;
            List<PNNLOmics.Data.XYData> testXYData = convertXYDataToOMICSXYData(xvals, yvals);


            List<ProcessedPeak> processedData = new List<ProcessedPeak>();

            PeakCentroidParameters parametersPeakCentroid = new PeakCentroidParameters();
            parametersPeakCentroid.ScanNumber = scanNum;

            List<ProcessedPeak> centroidedPeakList = new List<ProcessedPeak>();
            centroidedPeakList = PeakCentroid.DiscoverPeaks(testXYData, parametersPeakCentroid);

            PeakThresholdParameters parametersThreshold = new PeakThresholdParameters();
            parametersThreshold.SignalToShoulderCuttoff = 3f;

            List<ProcessedPeak> thresholdedData = new List<ProcessedPeak>();
            thresholdedData = PeakThreshold.ApplyThreshold(ref centroidedPeakList, parametersThreshold);

            Assert.That(thresholdedData.Count > 0);
            Assert.AreEqual(thresholdedData.Count, 6);
            displayPeakData(thresholdedData);
            Console.WriteLine();
            Console.WriteLine("Non-thresholded Candidate Peaks detected = " + centroidedPeakList.Count);
            Console.WriteLine("Thresholded Peaks detected = " + processedData.Count);

            
        }


        [Test]
        public void PeakDetectorV1_DiscoverPeaks_Test1()
        {
            double[] xvals = null;
            double[] yvals = null;

            loadTestScanData(ref xvals, ref yvals);
            Assert.That(xvals != null);
            Assert.AreEqual(322040, xvals.Length);

            double peakBR = 2;
            double sigNoiseThresh = 3;

            DeconTools.Backend.XYData xydata = new DeconTools.Backend.XYData();
            xydata.Xvalues = xvals;
            xydata.Yvalues = yvals;

            DeconTools.Backend.ProcessingTasks.DeconToolsPeakDetector peakdetector = new DeconTools.Backend.ProcessingTasks.DeconToolsPeakDetector(peakBR, sigNoiseThresh, DeconTools.Backend.Globals.PeakFitType.QUADRATIC, false);
            List<DeconTools.Backend.Core.IPeak> peakList = peakdetector.FindPeaks(xydata, 0, 50000);

            displayPeakData(peakList);
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
            
            PeakDetectorParameters newDetectorParameters = new PeakDetectorParameters();
            PeakDetectorController newPeakDetector = new PeakDetectorController();

            List<ProcessedPeak> finalPeakList = new List<ProcessedPeak>();

            finalPeakList = newPeakDetector.DetectPeaks(testXYData, newDetectorParameters);

            Assert.AreEqual(finalPeakList.Count, 6);
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
