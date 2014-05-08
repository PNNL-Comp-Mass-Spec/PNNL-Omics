/*
 * 
 * Reviewed 
 *  1-18-2011
 * 
 */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using PNNLOmics.Data;
using PNNLOmics.Data.Peaks;

namespace PNNLOmics.Algorithms.PeakDetection
{
    //TODO: Fill in this comment.
    /// <summary>
    /// 
    /// </summary>
    public class KronewitterPeakDetector: PeakDetector
    {
        /// <summary>
        /// Parameters used for centroiding algorithms
        /// </summary>
        public PeakCentroiderParameters CentroidParameters { get; set; }

        /// <summary>
        /// Parameters used for thresholding algorithms
        /// </summary>
        public PeakThresholderParameters ThresholdParameters { get; set; }
        

        /// <summary>
        /// default constructor
        /// </summary>
        public KronewitterPeakDetector()
        {
            CentroidParameters  = new PeakCentroiderParameters();
            ThresholdParameters = new PeakThresholderParameters();
        }

        /// <summary>
        /// Find and identify characteristics of peaks in raw XYData.  This includes finding candidate centroid peaks and noise thresholding.
        /// </summary>
        /// <param name="rawXYData">raw XYdata</param>
        /// <returns>Centroided peaks with noise removed</returns>
        public override Collection<Peak> DetectPeaks(Collection<XYData> collectionRawXYData)
        {
            var rawXYData = new List<XYData>(collectionRawXYData);

            //TODO: Scott Create constructor that accepts parameters.
            var newPeakCentroider = new PeakCentroider();
            newPeakCentroider.Parameters = CentroidParameters;

            // Find peaks in profile.
            var centroidedPeakList = new List<ProcessedPeak>();
            centroidedPeakList = newPeakCentroider.DiscoverPeaks(rawXYData);

            //TODO: Scott Create constructor that accepts parameters.
            var newPeakThresholder = new PeakThresholder();
            newPeakThresholder.Parameters = ThresholdParameters;
            
            // Separate signal from noise.
            List<ProcessedPeak> thresholdedData = null; 
            thresholdedData = newPeakThresholder.ApplyThreshold(centroidedPeakList);

            // Find peaks.
            var outputPeakList = ProcessedPeak.ToPeaks(thresholdedData); 

            return outputPeakList;
        }
    } 
}
