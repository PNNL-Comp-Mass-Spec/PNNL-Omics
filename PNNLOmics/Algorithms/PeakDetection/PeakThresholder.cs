using System;
using System.Collections.Generic;
using System.Linq;
using PNNLOmics.Data;
using PNNLOmics.Data.Peaks;

namespace PNNLOmics.Algorithms.PeakDetection
{

    /// <summary>
    /// Calculates a global threshold line to filter the data
    /// </summary>
    public class PeakThresholder
    {
        /// <summary>
        /// Gets or sets the peak centroider parameters.
        /// </summary>
        public PeakThresholderParameters Parameters { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PeakThresholder()
        {
            Parameters = new PeakThresholderParameters();
        }

        public PeakThresholder(PeakThresholderParameters parameters)
        {
            Parameters = parameters;
        }
        
        /// <summary>
        /// calculate mean of the noise (aka mean of the data) then calculate the standard deviation of the noise (aks data)
        /// if peak is above Xsigma+mean reject
        /// FWHM thresholding is not proving to be usefull yet.
        /// </summary>
        /// <param name="peakData">input peaks we want to threshold</param>
        /// <param name="peakShoulderNoise">lowest minima point intensity surrounding the peak</param>
        /// <param name="localMinimaData">index of minima on each side of point X=left, Y=right</param>
        /// <param name="parameters">Peakthreshold parameters</param>
        public List<ProcessedPeak> ApplyThreshold(List<ProcessedPeak> peakList)
        {
            var resultListThresholded = new List<ProcessedPeak>();

            if (peakList == null || peakList.Count == 0) return resultListThresholded;

            var numPoints = peakList.Count;

            double signaltoShoulder = 0;
            double signaltoBackground = 0;
            double signaltoNoise = 0;
            double thresholdIntensity = 0;

            if (!Parameters.isDataThresholded)
            {
                #region calculate average noise value and average shoulderNoiseLeve  = baseline
                double averageShoulderNoise = 0;//average of higher minima
                double averagePeakNoise = 0;//average of all data.  usefull if noise dominates
                double averageBackgroundNoise = 0;//average of lower minima = baseline
                double averageNoise = 0; //average between the the lower and higher minima.  this means that half the minima are higher and half the minima are lower.  should also work well on large numbers of points
                
                for (var i = 0; i < numPoints; i++)
                {
                    //averageShoulderNoise += peakShoulderNoise[i];
                    //averagePeakNoise += peakData[i].Y;
                    averageShoulderNoise += peakList[i].LocalHighestMinimaHeight;
                    averageBackgroundNoise += peakList[i].LocalLowestMinimaHeight;
                    averagePeakNoise += peakList[i].Height;
                    averageNoise += (peakList[i].LocalHighestMinimaHeight + peakList[i].LocalLowestMinimaHeight) / 2;//this is pretty nice
                }
                #endregion
                averageShoulderNoise /= numPoints;//worst case senario
                averageBackgroundNoise /= numPoints;//average background or baseline
                averagePeakNoise /= numPoints;//works if the noise dominates the spectra
                averageNoise /= numPoints;//good depection of the overall background of the data

                #region calculate standard deviation
                double stdevSumDeviationsSquared;
                double standardDevAllSignal;
                double MAD;
                double stdevMAD;
                CalculateDeviation(peakList, numPoints, averageNoise, out stdevSumDeviationsSquared, out standardDevAllSignal, out MAD, out stdevMAD);
                
                #endregion
                stdevMAD = MAD * 1.4826;
                standardDevAllSignal = Math.Sqrt(stdevSumDeviationsSquared / (numPoints - 1));
                
                for (var i = 0; i < numPoints; i++)
                {
                    var thresholdedPeak = new ProcessedPeak();

                    signaltoShoulder = peakList[i].Height / peakList[i].LocalHighestMinimaHeight;
                    signaltoBackground = peakList[i].Height / averageBackgroundNoise;
                    signaltoNoise = peakList[i].Height / averagePeakNoise;

                    //thresholdIntensity = Parameters.SignalToShoulderCuttoff * stdevMAD + averagePeakNoise;//average peak noise is too high
                    thresholdIntensity = Parameters.SignalToShoulderCuttoff * stdevMAD + averageNoise;//average noise is nice here

                    if (peakList[i].Height >= thresholdIntensity)
                    {
                        //include high abundant peaks
                        thresholdedPeak = peakList[i];
                        thresholdedPeak.SignalToNoiseGlobal = signaltoNoise;
                        thresholdedPeak.SignalToNoiseLocalHighestMinima = signaltoShoulder;
                        thresholdedPeak.SignalToBackground = signaltoBackground;
                       
                        resultListThresholded.Add(thresholdedPeak);// parameters.ThresholdedPeakData.Add(thresholdedPeak);
                    }
                }

                //TODO: possible FWHM filtering
                ////now that we have a global threshold, repeat filter by FWHM for the most abundant peaks so we can redraw the threshold line
                //convert to similar triangles
                //bool shouldWeThresholdByFWHM = false;
                //if (shouldWeThresholdByFWHM)
                //{
                //    #region peakfiltering by FWHM
                //    //List<XYData> sortedPeaks = new List<XYData>();
                //    parameters.ThresholdedObjectlist = parameters.ThresholdedObjectlist.OrderBy(p => p.PeakData.Y).ToList();
                //    //parameters.ThresholdedPeakData = sortedPeaks;
                //    double topPeaksFactor = 0.25;//how much of the list do we to use for our average FWHM.  0.5 means use top half of peaks above threshold
                //    double averageFWHM = 0;

                //    //calculate average
                //    for (int i = 0; i < (int)(parameters.ThresholdedObjectlist.Count * topPeaksFactor); i++)
                //    {
                //        averageFWHM += parameters.ThresholdedObjectlist[i].PeakFWHM;
                //    }
                //    averageFWHM /= parameters.ThresholdedObjectlist.Count;

                //    stdevSumDeviationsSquared = 0;
                //    //calculate standard deviation
                //    for (int i = 0; i < parameters.ThresholdedObjectlist.Count; i++)
                //    {
                //        stdevDeviations = (parameters.ThresholdedObjectlist[i].PeakFWHM - averageFWHM);
                //        stdevDeviationsSquared = stdevDeviations * stdevDeviations;
                //        stdevSumDeviationsSquared += stdevDeviationsSquared;
                //    }

                //    double standardFWHM = Math.Sqrt(stdevSumDeviationsSquared / (parameters.ThresholdedObjectlist.Count - 1));

                //    int y = 6;
                //    y = y * (int)standardFWHM;
                //    #endregion
                //}
            }
            else//add all peaks since the data is thresholded already ot setup some sort of other cuttoff
            {
                for (var i = 0; i < numPoints; i++)
                {
                    //include all peaks
                    var thresholdedPeak = new ProcessedPeak();

                    thresholdedPeak = peakList[i];
                    resultListThresholded.Add(thresholdedPeak);// parameters.ThresholdedPeakData.Add(thresholdedPeak);
                }
            }
            return resultListThresholded;
        }

        private static void CalculateDeviation(List<ProcessedPeak> peakList, int numPoints, double averagePeakNoise, out double stdevSumDeviationsSquared, out double standardDevAllSignal, out double MAD, out double stdevMAD)
        {
            double stdevDeviationsSquared = 0;
            double stdevDeviations = 0;
            stdevSumDeviationsSquared = 0;
            standardDevAllSignal = 0;
            MAD = 0; //Median Absolute Deviation
            stdevMAD = 0;//standard deviation derived from MAD

            var sortedCentroidedPeak = new List<ProcessedPeak>();
            var medanDeviationList = new List<double>();

            sortedCentroidedPeak = peakList.OrderBy(p => p.Height).ToList();

            var median = sortedCentroidedPeak[sortedCentroidedPeak.Count / 2].Height;//if it is sorted.

            double medianDeviations = 0;

            for (var i = 0; i < numPoints; i++)
            {
                stdevDeviations = (peakList[i].Height - averagePeakNoise);
                stdevDeviationsSquared = stdevDeviations * stdevDeviations;
                stdevSumDeviationsSquared += stdevDeviationsSquared;

                medianDeviations = Math.Abs(peakList[i].Height - median);
                medanDeviationList.Add(medianDeviations);
            }
            medanDeviationList.Sort();
            MAD = medanDeviationList[medanDeviationList.Count / 2];

        }
    }
}
