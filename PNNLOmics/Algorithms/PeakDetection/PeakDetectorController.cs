using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;
using PNNLOmics.Algorithms.PeakDetector;
using PNNLOmics.Algorithms.PeakDetectorController;

namespace PNNLOmics.Algorithms.PeakDetection
{
    public class PeakDetectorController
    {
        /// <summary>
        /// Find and identify characteristics of peaks in raw XYData.  This includes finding candidate centroid peaks and noise thresholding.
        /// </summary>
        /// <param name="rawXYData">raw data</param>
        /// <param name="detectorParameters">parameters for the candidate peak detection and threshold parameters</param>
        /// <returns>list of processed peaks and their characteristics</returns>
        public List<ProcessedPeak> DetectPeaks(List<PNNLOmics.Data.XYData> rawXYData, PeakDetectorParameters detectorParameters)
        {
            List<ProcessedPeak> centroidedPeakList = new List<ProcessedPeak>();
            centroidedPeakList = PeakCentroid.DiscoverPeaks(rawXYData, detectorParameters.CentroidParameters);

            List<ProcessedPeak> thresholdedData = new List<ProcessedPeak>();
            thresholdedData = PeakThreshold.ApplyThreshold(ref centroidedPeakList, detectorParameters.ThresholdParameters);

            return thresholdedData;
        }

        /// <summary>
        /// This quadratic formula returns the positve root or -1 for all other cases.  A*x^2 + B*x+C
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static double QuadraticFormula(double A, double B, double C)
        {
            //TODO verify that this function provides the correct values
            double root1;
            double root2;

            double discriminant;

            //this is used to check for no solutions
            discriminant = B * B - 4 * A * C;

            // Verify the discriminant.
            if (discriminant == 0)
            {
                root1 = -B / (2 * A); // If the discriminant is 0, both solutions are equal.
                root2 = root1;
            }
            else
            {
                if (discriminant < 0)
                {
                    root1 = -1;// If the discriminant is negative, there are no solutions.
                    root2 = -1;
                }
                else
                {
                    root1 = (-B - Math.Sqrt(discriminant)) / (2 * A);//In other cases the discriminant is positive, so there are two different solutions.
                    root2 = (-B + Math.Sqrt(discriminant)) / (2 * A);
                }

                //make sure we return a positive x value
                if (root1 < 0)
                {
                    root1 = root2;
                }
                if (root2 < 0)
                {
                    root2 = root1;
                }
            }
            return root1;
        }
    }
  
}
