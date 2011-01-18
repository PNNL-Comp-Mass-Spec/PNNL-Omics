using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.PeakDetection
{
    public class KronewitterPeakDetector: PeakDetector
    {
        /// <summary>
        /// Gets or sets the peak detector parameters.
        /// </summary>
        public KronewitterPeakDetectorParameters Parameters {get; set;}

        /// <summary>
        /// default constructor
        /// </summary>
        public KronewitterPeakDetector()
        {
            this.Parameters = new KronewitterPeakDetectorParameters();
        }

        /// <summary>
        /// Find and identify characteristics of peaks in raw XYData.  This includes finding candidate centroid peaks and noise thresholding.
        /// </summary>
        /// <param name="rawXYData">raw XYdata</param>
        /// <returns>Centroided peaks with noise removed</returns>
        public override Collection<Peak> DetectPeaks(Collection<XYData> collectionRawXYData)
        {
            List<XYData> rawXYData = new List<XYData>(collectionRawXYData);
            
            PeakCentroider newPeakCentroider = new PeakCentroider();
            newPeakCentroider.Parameters = Parameters.CentroidParameters;
            List<ProcessedPeak> centroidedPeakList = new List<ProcessedPeak>();
            centroidedPeakList = newPeakCentroider.DiscoverPeaks(rawXYData);

            PeakThresholder newPeakThresholder = new PeakThresholder();
            newPeakThresholder.Parameters = Parameters.ThresholdParameters;
            List<ProcessedPeak> thresholdedData = new List<ProcessedPeak>();
            thresholdedData = newPeakThresholder.ApplyThreshold(ref centroidedPeakList);

            Collection<Peak> outputPeakList = ConvertListProcessedPeaksTOStandardOutput(thresholdedData);

            return outputPeakList;
        }

        /// <summary>
        /// Peak is the standard object for the output collection and we need to convert processed peak lists.  
        /// </summary>
        /// <param name="processedPeakList">list of processed peaks</param>
        /// <returns>list of peaks</returns>
        private Collection<Peak> ConvertListProcessedPeaksTOStandardOutput(List<ProcessedPeak> processedPeakList)
        {
            Collection<Peak> outputPeakList = new Collection<Peak>();

            foreach(ProcessedPeak inPeak in processedPeakList)
            {
                Peak newPeak = new Peak();
                newPeak.Height = inPeak.Height;
                newPeak.LocalSignalToNoise = (float)inPeak.SignalToBackground;
                newPeak.Width = inPeak.Width;
                newPeak.XValue = inPeak.XValue;
                outputPeakList.Add(newPeak);
            }
            
            return outputPeakList;
        }
    } 
}
