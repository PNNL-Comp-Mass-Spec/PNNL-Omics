using System.Collections.Generic;
using System.Linq;
using PNNLOmics.Data;
using System;

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
            this.Parameters = new PeakThresholderParameters();
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
            List<ProcessedPeak> ResultListThresholded = new List<ProcessedPeak>();
            //int numPoints = peakData.Count;
            int numPoints = peakList.Count;

            double signaltoShoulder = 0;
            double signaltoBackground = 0;
            double signaltoNoise = 0;
            double thresholdIntensity = 0;

            if (!Parameters.isDataThresholded)
            {
                #region calculate average noise value and average shoulderNoiseLeve  = baseline
                double averageShoulderNoise = 0;
                double averagePeakNoise = 0;
                
                for (int i = 0; i < numPoints; i++)
                {
                    //averageShoulderNoise += peakShoulderNoise[i];
                    //averagePeakNoise += peakData[i].Y;
                    averageShoulderNoise += peakList[i].LocalLowestMinimaHeight;
                    averagePeakNoise += peakList[i].Height;
                }
                #endregion
                averageShoulderNoise /= numPoints;//baseline
                averagePeakNoise /= numPoints;//works if the noise dominates the spectra

                #region calculate standard deviation
                double stdevDeviationsSquared = 0;
                double stdevDeviations = 0;
                double stdevSumDeviationsSquared = 0;
                double standardDevAllSignal =0;
                double MAD = 0; //Median Absolute Deviation
                double stdevMAD = 0;//standard deviation derived from MAD

                List<ProcessedPeak> sortedCentroidedPeak = new List<ProcessedPeak>();
                List<double> medanDeviationList = new List<double>();

                sortedCentroidedPeak = peakList.OrderBy(p => p.Height).ToList();

                double median = sortedCentroidedPeak[(int)(sortedCentroidedPeak.Count / 2)].Height;//if it is sorted.

                double medianDeviations = 0;

                for (int i = 0; i < numPoints; i++)
                {
                    stdevDeviations = (peakList[i].Height - averagePeakNoise);
                    stdevDeviationsSquared = stdevDeviations * stdevDeviations;
                    stdevSumDeviationsSquared += stdevDeviationsSquared;

                    medianDeviations = Math.Abs(peakList[i].Height - median);
                    medanDeviationList.Add(medianDeviations);
                }
                medanDeviationList.Sort();
                MAD = medanDeviationList[(int)(medanDeviationList.Count / 2)];
                
                
                #endregion
                stdevMAD = MAD * 1.4826;
                standardDevAllSignal = Math.Sqrt(stdevSumDeviationsSquared / (numPoints - 1));
                
                for (int i = 0; i < numPoints; i++)
                {
                    ProcessedPeak thresholdedPeak = new ProcessedPeak();

                    signaltoShoulder = peakList[i].Height / peakList[i].LocalLowestMinimaHeight;
                    signaltoBackground = peakList[i].Height / averageShoulderNoise;
                    signaltoNoise = peakList[i].Height / averagePeakNoise;

                    thresholdIntensity = Parameters.SignalToShoulderCuttoff * stdevMAD + averagePeakNoise;

                    if (peakList[i].Height >= thresholdIntensity)
                    {
                        //include high abundant peaks
                        thresholdedPeak = peakList[i];
                        thresholdedPeak.SignalToNoiseGlobal = signaltoNoise;
                        thresholdedPeak.SignalToNoiseLocalMinima = signaltoShoulder;
                        thresholdedPeak.SignalToBackground = signaltoBackground;
                       
                        ResultListThresholded.Add(thresholdedPeak);// parameters.ThresholdedPeakData.Add(thresholdedPeak);
                    }
                    else
                    {
                        //TODO: deal with low abundant peaks base on FWHM or some other criteria.  Perhaps all data should be filtered on FWHM
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
                for (int i = 0; i < numPoints; i++)
                {
                    //include all peaks
                    ProcessedPeak thresholdedPeak = new ProcessedPeak();

                    thresholdedPeak = peakList[i];
                    ResultListThresholded.Add(thresholdedPeak);// parameters.ThresholdedPeakData.Add(thresholdedPeak);
                }
            }
            return ResultListThresholded;
        }
    }
}
