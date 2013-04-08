using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using PNNLOmics.Data;
using PNNLOmics.Algorithms.PeakDetection;
using System.Collections.ObjectModel;
using TestSpectra;

namespace PNNLOmics.UnitTests.AlgorithmTests.PeakDetectorTests
{
    [TestFixture]
    public class PeakDetectorTests
    {
        /// <summary>
        /// This tests only the XYData loader and the peak detection
        /// </summary>
        [Test]
        public void PeakDetectorV3_DiscoverPeaks_only_no_ThresholdingTest_AOnly()
        {
            float[] xvals = null;
            float[] yvals = null;

            loadTestScanData(ref xvals, ref yvals);
            Assert.That(xvals != null);
            Assert.AreEqual(122032, xvals.Length);

            List<XYData> testXYData = convertXYDataToOMICSXYData(xvals, yvals);

            PeakCentroider newPeakCentroider = new PeakCentroider();
            List<ProcessedPeak> centroidedPeakList = newPeakCentroider.DiscoverPeaks(testXYData);

            displayPeakData(centroidedPeakList);

            Assert.That(centroidedPeakList.Count > 0);
            Assert.AreEqual(15255, centroidedPeakList.Count);
        }

        /// <summary>
        /// this tests the peak detection and the thresholding
        /// </summary>
        [Test]
        public void PeakDetectorV3_DiscoverPeaks_Then_Threshold_A_and_B()
        {
            float[] xvals = null;
            float[] yvals = null;

            loadTestScanData(ref xvals, ref yvals);
            Assert.That(xvals != null);
            Assert.AreEqual(122032, xvals.Length);
            Console.WriteLine("Passed Load" + Environment.NewLine);

            List<XYData> testXYData = convertXYDataToOMICSXYData(xvals, yvals);

            PeakCentroider newPeakCentroider = new PeakCentroider();
            List<ProcessedPeak> centroidedPeakList = newPeakCentroider.DiscoverPeaks(testXYData);

            Assert.AreEqual(15255, centroidedPeakList.Count);
            Console.WriteLine("Passed Peak Detection" + Environment.NewLine);

            PeakThresholder newPeakThresholder = new PeakThresholder();
            //newPeakThresholder.Parameters.SignalToShoulderCuttoff = 3f;//3 sigma
            newPeakThresholder.Parameters.SignalToShoulderCuttoff = 4f;//4 sigma// this is very nice
            List<ProcessedPeak> thresholdedData = newPeakThresholder.ApplyThreshold(centroidedPeakList);

            Console.WriteLine("Non-thresholded Candidate Peaks detected = " + centroidedPeakList.Count);

            Assert.That(thresholdedData.Count > 0);
            //Assert.AreEqual(thresholdedData.Count, 53);
            Assert.AreEqual(thresholdedData.Count, 414);

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
            Assert.AreEqual(122032, xvals.Length);

            List<PNNLOmics.Data.XYData> testXYData = convertXYDataToOMICSXYData(xvals, yvals);
            Collection<XYData> dataInput = new Collection<XYData>(testXYData);

            KronewitterPeakDetector newPeakDetector = new KronewitterPeakDetector();
            Collection<Peak> finalPeakList = newPeakDetector.DetectPeaks(dataInput);

            Console.WriteLine("We found " + finalPeakList.Count + " Peaks.");
            //Assert.AreEqual(finalPeakList.Count, 53);
            Assert.AreEqual(finalPeakList.Count, 3134);
        }


        #region private functions

        private void displayPeakData(List<ProcessedPeak> centroidedPeakList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("m/z" + '\t' + "Height" + '\t' + "Width");
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
            HardCodedSpectraDouble newSpectra = new HardCodedSpectraDouble();
            newSpectra.GenerateSpectraDouble();
            Console.WriteLine(newSpectra.XValues.Length);
            xvals = newSpectra.XValues;
            yvals = newSpectra.YValues;
        }

        #endregion
    }
}


