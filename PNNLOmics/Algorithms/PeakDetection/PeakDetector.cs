using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;
using PNNLOmics.Algorithms.PeakDetector;

namespace PNNLOmics.Algorithms.PeakDetection
{
    public class PeakDetectorParameters
    {
        public PeakCentroidParameters CentroidParameters { get; set; }
        public PeakThresholdParameters ThresholdParameters { get; set; }
        public int ScanNumber { get; set; }//so we can attach a scan number to each peak

        public PeakDetectorParameters() 
        {
            this.CentroidParameters = new PeakCentroidParameters();
            this.ThresholdParameters = new PeakThresholdParameters();
            this.ScanNumber = 0;
        }
    }

    public class PeakDetector
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
            centroidedPeakList = PeakDetector.DiscoverPeaks(rawXYData, detectorParameters.CentroidParameters);

            List<ProcessedPeak> thresholdedData = new List<ProcessedPeak>();
            thresholdedData = PeakThreshold.ApplyThreshold(ref centroidedPeakList, detectorParameters.ThresholdParameters);

            return thresholdedData;
        }

        #region private functions
        /// <summary>
        /// Find candidate peaks in the spectra (incressing and then decreasing).  For each peak top, find centroid
        /// </summary>
        /// <param name="RawXYData">List of PNNL Omics XYData</param>
        /// <param name="parameters">parameters needed for the fit</param>
        private static List<ProcessedPeak> DiscoverPeaks(List<XYData> RawXYData, PeakCentroidParameters parameters)
        {
            List<ProcessedPeak> ResultsListCentroid = new List<ProcessedPeak>();

            int numPoints = RawXYData.Count;

            if (parameters.IsXYDataCentroided)
            {
                for (int i = 1; i < numPoints; i += 1)
                {
                    ProcessedPeak newPreCentroidedPeak = new ProcessedPeak();
                    newPreCentroidedPeak.XValue = RawXYData[i].X;
                    newPreCentroidedPeak.Height = RawXYData[i].Y;
                    newPreCentroidedPeak.Width = Convert.ToSingle(parameters.DefaultFWHMForCentroidedData);
                    newPreCentroidedPeak.ScanNumber = parameters.ScanNumber;
                    ResultsListCentroid.Add(newPreCentroidedPeak);
                }
            }
            else
            {
                List<XYData> peakTopParabolaPoints = new List<XYData>();
                for (int i = 0; i < parameters.NumberOfPoints; i++)//number of points must be 3,5,7
                {
                    XYData newPoint = new XYData(0, 0);
                    peakTopParabolaPoints.Add(newPoint);
                }

                XYData centroidedPeak = new XYData(0, 0);

                for (int i = 1; i < numPoints; i += 1)
                {
                    do
                    {
                        if (RawXYData[i].Y > RawXYData[i - 1].Y)  // is it increasing?
                        {
                            i += 1; //look ar next point
                            if (RawXYData[i].Y < RawXYData[i - 1].Y)  // Is it Decreasing?
                            {
                                //peak top data point is at location i-1
                                ProcessedPeak newcentroidPeak = new ProcessedPeak();
                                newcentroidPeak.ScanNumber = parameters.ScanNumber;

                                //1.  find local noise (or shoulder noise) by finding the average fo the local minima on each side of the peak
                                //XYData storeMinimaDataIndex = new XYData();//will contain the index of the locations where the surrounding local mnima are
                                int shoulderNoiseToLeftIndex = 0;
                                int shoulderNoiseToRightIndex = 0;

                                newcentroidPeak.LocalLowestMinimaHeight = PeakDetector.FindShoulderNoise(ref RawXYData, i - 1, parameters.DefaultShoulderNoiseValue, ref shoulderNoiseToLeftIndex, ref shoulderNoiseToRightIndex);
                                newcentroidPeak.MinimaOfLowerMassIndex = shoulderNoiseToLeftIndex;
                                newcentroidPeak.MinimaOfHigherMassIndex = shoulderNoiseToRightIndex;

                                //2.   centroid peaks via fitting a parabola
                                //TODO: decide if sending indexes is better becaus the modulariy of the parabola finder will be broken
                                //store points to go to the parabola fitter
                                for (int j = 0; j < parameters.NumberOfPoints; j += 1)
                                {
                                    int index = i - 1 - (int)((float)parameters.NumberOfPoints / (float)2 - (float)0.5) + j;//since number of points is 3,5,7 it will divide nicely
                                    peakTopParabolaPoints[j] = RawXYData[index];
                                }

                                //calculate parabola apex returning int and centroided MZ
                                centroidedPeak = PeakDetector.Parabola(peakTopParabolaPoints);
                                newcentroidPeak.XValue = centroidedPeak.X;
                                newcentroidPeak.Height = centroidedPeak.Y;

                                //3.  find FWHM
                                int centerIndex = i - 1;//this is the index in the raw data for the peak top (non centroided)
                                newcentroidPeak.Width = Convert.ToSingle(PeakDetector.FindFWHM(RawXYData, centerIndex, centroidedPeak, ref shoulderNoiseToLeftIndex, ref shoulderNoiseToRightIndex, parameters.LowAbundanceFWHMPeakFitType));

                                //4.  add centroided peak
                                ResultsListCentroid.Add(newcentroidPeak);
                            }
                        }
                    }
                    while (RawXYData[i].Y > RawXYData[i - 1].Y && i < numPoints - 1);  //Is it Still Increasing?
                }
            }

            return ResultsListCentroid;//Peak Centroid
        }

        /// <summary>
        /// find the centoid mass and apex intenxity via paraola fit to the top three points
        /// </summary>
        /// <param name="peakTopList">A list of PNNL Omics XYData</param>
        /// <returns>XYData point correspiding to the pair at the apex intensity and center of mass </returns>
        private static XYData Parabola(List<XYData> peakTopList)
        {
            double apexMass;
            double apexIntensity;

            int numberOfPoints = peakTopList.Count;

            //print "run Parabola"

            double InitialX;  //used to offset the parabola to zero
            InitialX = peakTopList[0].X;
            for (int i = 0; i < numberOfPoints; i++)
            {
                peakTopList[i].X = Convert.ToSingle(peakTopList[i].X - InitialX);
            }

            double T1 = 0, T2 = 0, T3 = 0;
            double X1 = 0, X2 = 0, X3 = 0;
            double Y1 = 0, Y2 = 0, Y3 = 0;
            double Z1 = 0, Z2 = 0, Z3 = 0;
            double Theta1, Theta2, Theta3, ThetaDenominator;
            double A = 0, B = 0, C = 0;
            double X = 0;

            //TODO unit test
            //peakTopList[0].X=1;
            //peakTopList[1].X=2;
            //peakTopList[2].X=3;
            //peakTopList[0].Y=2;
            //peakTopList[1].Y=5;
            //peakTopList[2].Y = 4;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                T1 += peakTopList[i].X * peakTopList[i].X * peakTopList[i].Y;
            }

            T1 = 2 * T1;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                T2 += peakTopList[i].X * peakTopList[i].Y;
            }

            T2 = 2 * T2;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                T3 += peakTopList[i].Y;
            }

            T3 = 2 * T3;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                X1 += peakTopList[i].X * peakTopList[i].X * peakTopList[i].X * peakTopList[i].X;
            }

            X1 = 2 * X1;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                X2 += peakTopList[i].X * peakTopList[i].X * peakTopList[i].X;
            }

            X2 = 2 * X2;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                X3 += peakTopList[i].X * peakTopList[i].X;
            }

            X3 = 2 * X3;

            Y1 = X2;
            Y2 = X3;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                Y3 += peakTopList[i].X;
            }

            Y3 = 2 * Y3;

            Z1 = X3;
            Z2 = Y3;
            Z3 = 2 * numberOfPoints;

            ThetaDenominator = X1 * Y2 * Z3 - X1 * Z2 * Y3 - Y1 * X2 * Z3 + Z1 * X2 * Y3 + Y1 * X3 * Z2 - Z1 * X3 * Y2;

            Theta1 = 0;
            Theta2 = 0;
            Theta3 = 0;
            Theta1 = (Y2 * Z3 - Z2 * Y3) * T1 / ThetaDenominator;
            Theta2 = -(Y1 * Z3 - Z1 * Y3) * T2 / ThetaDenominator;
            Theta3 = (Y1 * Z2 - Z1 * Y2) * T3 / ThetaDenominator;
            A = Theta1 + Theta2 + Theta3;

            Theta1 = 0;
            Theta2 = 0;
            Theta3 = 0;
            Theta1 = -(X2 * Z3 - Z2 * X3) * T1 / ThetaDenominator;
            Theta2 = (X1 * Z3 - Z1 * X3) * T2 / ThetaDenominator;
            Theta3 = -(X1 * Z2 - Z1 * X2) * T3 / ThetaDenominator;
            //print theta1, theta2, theta3
            B = Theta1 + Theta2 + Theta3;

            Theta1 = 0;
            Theta2 = 0;
            Theta3 = 0;
            Theta1 = (X2 * Y3 - Y2 * X3) * T1 / ThetaDenominator;
            Theta2 = -(X1 * Y3 - Y1 * X3) * T2 / ThetaDenominator;
            Theta3 = (X1 * Y2 - Y1 * X2) * T3 / ThetaDenominator;
            C = Theta1 + Theta2 + Theta3;

            X = -0.5 * B / A;


            apexMass = X + InitialX;
            apexIntensity = A * X * X + B * X + C;


            //TODO for printing we need to reset the parabola back to the correct m/z
            for (int i = 0; i < numberOfPoints; i++)
            {
                peakTopList[i].X = (float)(peakTopList[i].X + InitialX);
            }

            XYData centroidXYDataResults = new XYData(0, 0);
            centroidXYDataResults.X = (float)apexMass;
            centroidXYDataResults.Y = (float)apexIntensity;

            return centroidXYDataResults;
            //TODO unit test
            //T1=116.0000
            //T2=48.0000
            //T3=22.0000
            //X1=196.0000
            //X2=72.0000
            //X3=28.0000
            //Y1=72.0000
            //Y2=28.0000
            //Y3=12.0000
            //Z1=28.0000
            //Z2=12.0000
            //Z3=6.0000
            //A=-2
            //B=9
            //C=-5
            //apexIntensity = 5.125
        }

        /// <summary>
        /// find the coefficeints to the parabola that goes through the data points
        /// </summary>
        /// <param name="peakTopList">A list of PNNL Omics XYData</param>
        /// <returns>XYData point correspiding to the pair at the apex intensity and center of mass </returns>
        private static void ParabolaABC(List<XYData> peakSideList, ref double Aout, ref double Bout, ref double Cout)
        {
            #region copied code
            double apexMass;
            double apexIntensity;

            int numberOfPoints = peakSideList.Count;

            //switch x with Y


            //for (int i = 0; i < numberOfPoints; i++)
            //{
            //    XYData tempPoint = new XYData();
            //    tempPoint.Y = peakSideList[i].X;
            //    tempPoint.X = peakSideList[i].Y;
            //    peakSideList[i].X = tempPoint.X;
            //    peakSideList[i].Y = tempPoint.Y;
            //}

            //print "run Parabola"
            double InitialX;  //used to offset the parabola to zero
            InitialX = peakSideList[0].X;
            for (int i = 0; i < numberOfPoints; i++)
            {
                peakSideList[i].X = (float)(peakSideList[i].X - InitialX);
            }

            double T1 = 0, T2 = 0, T3 = 0;
            double X1 = 0, X2 = 0, X3 = 0;
            double Y1 = 0, Y2 = 0, Y3 = 0;
            double Z1 = 0, Z2 = 0, Z3 = 0;
            double Theta1, Theta2, Theta3, ThetaDenominator;
            double A = 0, B = 0, C = 0;
            double X = 0;

            //TODO unit test
            //peakTopList[0].X=1;
            //peakTopList[1].X=2;
            //peakTopList[2].X=3;
            //peakTopList[0].Y=2;
            //peakTopList[1].Y=5;
            //peakTopList[2].Y = 4;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                T1 += peakSideList[i].X * peakSideList[i].X * peakSideList[i].Y;
            }

            T1 = 2 * T1;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                T2 += peakSideList[i].X * peakSideList[i].Y;
            }

            T2 = 2 * T2;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                T3 += peakSideList[i].Y;
            }

            T3 = 2 * T3;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                X1 += peakSideList[i].X * peakSideList[i].X * peakSideList[i].X * peakSideList[i].X;
            }

            X1 = 2 * X1;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                X2 += peakSideList[i].X * peakSideList[i].X * peakSideList[i].X;
            }

            X2 = 2 * X2;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                X3 += peakSideList[i].X * peakSideList[i].X;
            }

            X3 = 2 * X3;

            Y1 = X2;
            Y2 = X3;

            for (int i = 0; i < numberOfPoints; i += 1)
            {
                Y3 += peakSideList[i].X;
            }

            Y3 = 2 * Y3;

            Z1 = X3;
            Z2 = Y3;
            Z3 = 2 * numberOfPoints;

            ThetaDenominator = X1 * Y2 * Z3 - X1 * Z2 * Y3 - Y1 * X2 * Z3 + Z1 * X2 * Y3 + Y1 * X3 * Z2 - Z1 * X3 * Y2;

            Theta1 = 0;
            Theta2 = 0;
            Theta3 = 0;
            Theta1 = (Y2 * Z3 - Z2 * Y3) * T1 / ThetaDenominator;
            Theta2 = -(Y1 * Z3 - Z1 * Y3) * T2 / ThetaDenominator;
            Theta3 = (Y1 * Z2 - Z1 * Y2) * T3 / ThetaDenominator;
            A = Theta1 + Theta2 + Theta3;

            Theta1 = 0;
            Theta2 = 0;
            Theta3 = 0;
            Theta1 = -(X2 * Z3 - Z2 * X3) * T1 / ThetaDenominator;
            Theta2 = (X1 * Z3 - Z1 * X3) * T2 / ThetaDenominator;
            Theta3 = -(X1 * Z2 - Z1 * X2) * T3 / ThetaDenominator;
            //print theta1, theta2, theta3
            B = Theta1 + Theta2 + Theta3;

            Theta1 = 0;
            Theta2 = 0;
            Theta3 = 0;
            Theta1 = (X2 * Y3 - Y2 * X3) * T1 / ThetaDenominator;
            Theta2 = -(X1 * Y3 - Y1 * X3) * T2 / ThetaDenominator;
            Theta3 = (X1 * Y2 - Y1 * X2) * T3 / ThetaDenominator;
            C = Theta1 + Theta2 + Theta3;

            X = -0.5 * B / A;

            apexMass = X + InitialX;
            apexIntensity = A * X * X + B * X + C;

            #endregion


            Aout = A;
            Bout = B;
            Cout = C;
            //TODO unit test
            //T1=116.0000
            //T2=48.0000
            //T3=22.0000
            //X1=196.0000
            //X2=72.0000
            //X3=28.0000
            //Y1=72.0000
            //Y2=28.0000
            //Y3=12.0000
            //Z1=28.0000
            //Z2=12.0000
            //Z3=6.0000
            //A=-2
            //B=9
            //C=-5
            //apexIntensity = 5.125
        }

        /// <summary>
        /// find the local minima on each side of the peak
        /// </summary>
        /// <param name="rawData">reference to fullspectra</param>
        /// <param name="centerIndex">index of center point at local maximum</param>
        /// <param name="defaultNoiseValue">when the data drops to 0 on either side of peak, use this value.  default =1</param>
        /// <returns>returns the Y value of the lowest shoulder</returns>
        //private static double FindShoulderNoise(ref List<XYData> rawData, int centerIndex, double defaultNoiseValue, XYData leftMinRightMin) 
        private static double FindShoulderNoise(ref List<XYData> rawData, int centerIndex, double defaultNoiseValue, ref int shoulderNoiseToLeftIndex, ref int shoulderNoiseToRightIndex)
        {
            double minIntensityLeft = 0;
            double minIntensityRight = 0;
            int length = rawData.Count;

            if (rawData[centerIndex].Y == 0)//no peak condition
            {
                return 0;
            }

            if (centerIndex <= 0 || centerIndex >= length - 1)//first and last point condition
            {
                return defaultNoiseValue;//set equal to 1 so dividing signal by the noise will produce Signal to noise = intensity
            }

            // Find the first local minimum as we go down the m/z range.
            bool found = false;
            for (int i = centerIndex; i > 0; i--)
            {
                if (rawData[i + 1].Y >= rawData[i].Y && rawData[i - 1].Y > rawData[i].Y) // Local minima here \/
                {
                    minIntensityLeft = rawData[i].Y;
                    //leftMinRightMin.X = i; //minIntensityLeft;//assign index for use in FWHM
                    shoulderNoiseToLeftIndex = i; //minIntensityLeft;//assign index for use in FWHM
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                minIntensityLeft = rawData[0].Y;
                //leftMinRightMin.X = 0; //minIntensityLeft;//assign index for use in FWHM
                shoulderNoiseToLeftIndex = 0; //minIntensityLeft;//assign index for use in FWHM
            }

            //// Find the first local minimum as we go up the m/z range.
            found = false;//reset and continue
            for (int i = centerIndex; i < length - 1; i++)
            {
                if (rawData[i + 1].Y >= rawData[i].Y && rawData[i - 1].Y > rawData[i].Y) // Local minima here \/
                {
                    minIntensityRight = rawData[i].Y;
                    //leftMinRightMin.Y = i; //minIntensityRight;//assign index for use in FWHM
                    shoulderNoiseToRightIndex = i; //minIntensityRight;//assign index for use in FWHM
                    found = true;
                    break;
                }
            }

            if (!found)//if the minIntensity to the right is still not found, we are at the end of the data so take last point
            {
                minIntensityRight = rawData[length - 1].Y;
                //leftMinRightMin.Y = length - 1; //minIntensityRight;//assign index for use in FWHM
                shoulderNoiseToRightIndex = length - 1; //minIntensityRight;//assign index for use in FWHM
            }

            //leftMinRightMin.X = minIntensityLeft;//this would bring back the actual intensity instead of the index
            //leftMinRightMin.Y = minIntensityRight;


            // assign value;
            //there are 4 cases
            //minL = 0, minR = 0-->=1 or defualt value
            //minL = 0, minR = 1-->minR
            //minL = 1, minR = 0-->minL
            //minL = 1, minR = 1--> lowest minR or minL

            //if neither minimum is found, set shoulderNoise to 1 so the ratio becomes the intensity
            if (minIntensityLeft == 0)//0,?
            {
                if (minIntensityRight == 0)//0,0
                {
                    return defaultNoiseValue;//set equal to 1 so dividing signal by the noise will produce Signal to noise = intensity
                }
                return minIntensityRight;//0,1
            }
            //else//1,?  //return the lowest when both are present
            if (minIntensityRight > 0)
            {
                if (minIntensityRight < minIntensityLeft)//1,1
                {
                    return minIntensityRight;
                }
                return minIntensityLeft;
            }
            return minIntensityLeft;//1,0
            
        }

        //! Find full width at half maximum value at position specified. 
        //remarks Looks for half height locations at left and right side, and uses twice of that value as the FWHM value. If half height 
        //locations cannot be found (because of say an overlapping neighbouring peak), we perform interpolations.
        //private static double FindFWHM(List<XYData> rawData, int centerIndex, XYData centroidPeak, XYData storeMinimaData, LowAbundanceFWHMPeakFit lowAbundanceFWHMFitType)
        private static double FindFWHM(List<XYData> rawData, int centerIndex, XYData centroidPeak, ref int shoulderNoiseToLettIndex, ref int shoulderNoiseToRightIndex, LowAbundanceFWHMPeakFit lowAbundanceFWHMFitType)
        {
            //this bounds the number of points we can used to determine FWHM
            //int MinimaLeftIndex = (int)storeMinimaData.X;//index lower in mass
            //int MinimaRightIndex = (int)storeMinimaData.Y;//index higher in mass
            int MinimaLeftIndex = shoulderNoiseToLettIndex;//index lower in mass
            int MinimaRightIndex = shoulderNoiseToRightIndex;//index higher in mass

            double deltaXRight;//distance of half-width-at-half-maximum to the right
            double deltaXLeft;//distance of half-width-at-half-maximum to the left

            //points for steppig through so we can find interpolation bounds
            double X1CurrentPoint;
            double Y1CurrentPoint;
            double X2OnePointAhead;//one less when stepping back from large X and one more when stepping from small X
            double Y2OnePointAhead;//one less when stepping back from large X and one more when stepping from small X

            //double Y2OnePointsAhead;
            //use centroided data for deltaX and halfMaximum calculations
            double Y0CenterHeight = centroidPeak.Y;
            double Y0HalfHeight = Y0CenterHeight / 2.0;
            double X0CenterMass = centroidPeak.X;

            //initialize heavily used variables
            double FWHM;//returned answer
            double A = 0;//ParabolaABC
            double B = 0;//ParabolaABC
            double C = 0;//ParabolaABC

            FWHMPointFindingOptions detectedMethodLeft = FWHMPointFindingOptions.Unassigned;
            FWHMPointFindingOptions detectedMethodRight = FWHMPointFindingOptions.Unassigned;
            int numPoints = rawData.Count();

            //if there is no data
            if (Y0CenterHeight == 0.0)
            {
                return 0.0;
            }

            //if we are on the ends of the data
            if (centerIndex <= 0 || centerIndex >= numPoints - 1)
            {
                return 0;
            }

            #region look for the Half maximum on the right side
            deltaXRight = rawData[MinimaRightIndex].X - X0CenterMass;//initialize with maximum it can be without calculations//ths may not be necesary

            if (rawData[MinimaRightIndex].Y < Y0HalfHeight)//is our minima less than half maximum
            {
                #region there are enough data points that we can find the postiion easily.
                for (int i = MinimaRightIndex; i > centerIndex; i--)//TODO check this centerindex+1? to avoid 0
                {
                    Y2OnePointAhead = rawData[i - 1].Y;//look one point ahead
                    if (Y2OnePointAhead > Y0HalfHeight)//is the Yfwhm in the range
                    {
                        X1CurrentPoint = rawData[i].X;
                        Y1CurrentPoint = rawData[i].Y;
                        X2OnePointAhead = rawData[i - 1].X;
                        //we are in range. i is below the half height and i-1 is above
                        double interpolatedX = X1CurrentPoint - (X1CurrentPoint - X2OnePointAhead) * (Y0HalfHeight - Y1CurrentPoint) / (Y2OnePointAhead - Y1CurrentPoint);//TODO check this
                        deltaXRight = interpolatedX - X0CenterMass;
                        detectedMethodRight = FWHMPointFindingOptions.Interpolated;
                        break;
                    }
                    else
                    {
                        X1CurrentPoint = 0;//break point
                        //this is not needed because we will keep iterating till the answer is found
                        //TODO is there a case for this else block or do we use the default deltaXright asigned above
                    }
                    //xcoordinateToRight = X0CenterMass + deltaXRight;//notice this is offset from the center mass not the centroid
                }
                #endregion
            }
            else//we need to interpolate beyond the data we have to find a theoretical end point
            {
                //if there are a few points
                if (MinimaRightIndex - centerIndex > 2)//three or more points
                {
                    /// 1.  take log of lorentzian data so we can fit a parabola to the peak
                    /// 2.  fit parabola
                    /// 3.  Use A,B,C from parabola to construct F(log(Y)
                    /// 4.  plug log(FWHM) into F() to get deltaX = F(log(FWHM))

                    #region load up ListXYdata with points from the center so we can fit a parabola
                    double transformedHalfHeight = 0;//this is needed incase the logarithm is taken
                    List<XYData> peakRightSideList = new List<XYData>();

                    switch (lowAbundanceFWHMFitType)//for parabola fit, don't take a log.  For lorentzian, take a log first
                    {
                        case LowAbundanceFWHMPeakFit.Parabola:
                            {
                                for (int i = MinimaLeftIndex; i <= MinimaRightIndex; i++)
                                {
                                    XYData pointTransfer = new XYData(0, 0);
                                    pointTransfer = rawData[i];
                                    peakRightSideList.Add(pointTransfer);
                                }
                                transformedHalfHeight = Y0HalfHeight;
                            }
                            break;
                        case LowAbundanceFWHMPeakFit.Lorentzian:
                            {
                                for (int i = MinimaLeftIndex; i <= MinimaRightIndex; i++)
                                {
                                    XYData pointTransfer = new XYData(0, 0);
                                    pointTransfer.X = rawData[i].X;
                                    pointTransfer.Y = (float)(Math.Log10(rawData[i].Y));
                                    peakRightSideList.Add(pointTransfer);
                                }
                                transformedHalfHeight = Math.Log10(Y0HalfHeight);
                            }
                            break;
                        default:
                            {
                                for (int i = MinimaLeftIndex; i <= MinimaRightIndex; i++)
                                {
                                    XYData pointTransfer = new XYData(0, 0);
                                    pointTransfer = rawData[i];
                                    peakRightSideList.Add(pointTransfer);
                                }
                                transformedHalfHeight = Y0HalfHeight;
                            }
                            break;
                    }
                    #endregion

                    //fit parabola to the data so we can extrapolate the missing FWHM 
                    ParabolaABC(peakRightSideList, ref A, ref B, ref C);

                    //calculate right X value for half height
                    deltaXRight = -((B / 2 + Math.Sqrt(B * B - 4 * A * C + 4 * A * transformedHalfHeight) / 2) / A);
                    detectedMethodRight = FWHMPointFindingOptions.QuadraticExtrapolation;
                    //xcoordinateToRight = rawData[MinimaLeftIndex].X + deltaXRight;//notice the different start point
                    //this is because the parabola starts at the MinLeftIndex rather than the XOCenterMass
                }
                else//there are not enough points to fit the parabola, extrapolate a line
                {
                    //calculate slope and project a line
                    double slope = (Y0CenterHeight - rawData[MinimaRightIndex].Y) / (X0CenterMass - rawData[MinimaRightIndex].X);
                    double intecept = (Y0CenterHeight - slope * X0CenterMass);
                    double regressedX = -((intecept - Y0HalfHeight) / slope);
                    deltaXRight = regressedX - X0CenterMass;
                    detectedMethodRight = FWHMPointFindingOptions.LinearExtrapolation;
                }
            }
            #endregion

            #region look for the Half maximum on the left side
            deltaXLeft = X0CenterMass - rawData[MinimaLeftIndex].X;//initialize with maximum it can be//ths may not be necesary

            if (rawData[MinimaLeftIndex].Y < Y0HalfHeight)//is our minima less than half maximum
            {
                #region there are enough data points that we can find the postiion easily
                for (int i = MinimaLeftIndex; i < centerIndex; i++)//TODO check this centerindex-1? to avoid 0
                {
                    Y2OnePointAhead = rawData[i + 1].Y;//look one point ahead
                    if (Y2OnePointAhead > Y0HalfHeight)//is the Yfwhm in the range
                    {
                        X1CurrentPoint = rawData[i].X;
                        Y1CurrentPoint = rawData[i].Y;
                        X2OnePointAhead = rawData[i + 1].X;
                        //we are in range. i is below the half height and i-1 is above
                        double interpolatedX = X1CurrentPoint - (X1CurrentPoint - X2OnePointAhead) * (Y0HalfHeight - Y1CurrentPoint) / (Y2OnePointAhead - Y1CurrentPoint);//TODO check this
                        deltaXLeft = X0CenterMass - interpolatedX;
                        detectedMethodLeft = FWHMPointFindingOptions.Interpolated;
                        break;
                    }
                    else
                    {
                        X1CurrentPoint = 0;//break point
                        //this is not needed because we will keep iterating till the answer is found
                        //TODO is there a case for this else block or do we use the default deltaXright asigned above
                    }
                }
                #endregion
            }
            else//we need to interpolate beyond the data we have to find a theoretical end point
            {
                //if there are a few points
                if (centerIndex - MinimaLeftIndex > 2)
                {
                    /// 1.  take log of lorentzian data so we can fit a parabola to the peak
                    /// 2.  fit parabola
                    /// 3.  Use A,B,C from parabola to construct F(log(Y)
                    /// 4.  plug log(FWHM) into F() to get deltaX = F(log(FWHM))

                    #region load up ListXYdata with points from the center so we can fit a parabola
                    double transformedHalfHeight = 0;//this is needed incase the logarithm is taken
                    List<XYData> peakLeftSideList = new List<XYData>();

                    switch (lowAbundanceFWHMFitType)//for parabola fit, don't take a log.  For lorentzian, take a log first
                    {
                        case LowAbundanceFWHMPeakFit.Parabola:
                            {
                                for (int i = MinimaLeftIndex; i <= MinimaRightIndex; i++)
                                {
                                    XYData pointTransfer = new XYData(0, 0);
                                    pointTransfer = rawData[i];
                                    peakLeftSideList.Add(pointTransfer);
                                }
                                transformedHalfHeight = Y0HalfHeight;
                            }
                            break;
                        case LowAbundanceFWHMPeakFit.Lorentzian:
                            {
                                for (int i = MinimaLeftIndex; i <= MinimaRightIndex; i++)
                                {
                                    XYData pointTransfer = new XYData(0, 0);
                                    pointTransfer.X = rawData[i].X;
                                    pointTransfer.Y = (float)(Math.Log10(rawData[i].Y));
                                    peakLeftSideList.Add(pointTransfer);
                                }
                                transformedHalfHeight = Math.Log10(Y0HalfHeight);
                            }
                            break;
                        default:
                            {
                                for (int i = MinimaLeftIndex; i <= MinimaRightIndex; i++)
                                {
                                    XYData pointTransfer = new XYData(0, 0);
                                    pointTransfer = rawData[i];
                                    peakLeftSideList.Add(pointTransfer);
                                }
                                transformedHalfHeight = Y0HalfHeight;
                            }
                            break;
                    }
                    #endregion

                    //fit parabola to the data so we can extrapolate the missing FWHM                  
                    ParabolaABC(peakLeftSideList, ref A, ref B, ref C);

                    //calculate right X value for half height
                    deltaXLeft = -((B / 2 + Math.Sqrt(B * B - 4 * A * C + 4 * A * transformedHalfHeight) / 2) / A);
                    detectedMethodLeft = FWHMPointFindingOptions.QuadraticExtrapolation;
                    //xcoordinateToLeft = rawData[MinimaLeftIndex].X + deltaXLeft;//notice the different start point
                    //this is because the parabola starts at the MinLeftIndex rather than the XOCenterMass
                }
                else//there are not enough points to fit the parabola, extrapolate a line
                {
                    //calculate slope and project a line
                    double slope = (Y0CenterHeight - rawData[MinimaLeftIndex].Y) / (X0CenterMass - rawData[MinimaLeftIndex].X);
                    double intecept = (Y0CenterHeight - slope * X0CenterMass);
                    double regressedX = -((intecept - Y0HalfHeight) / slope);
                    deltaXLeft = X0CenterMass - regressedX;
                    detectedMethodLeft = FWHMPointFindingOptions.LinearExtrapolation;
                }
            }
            #endregion


            if (deltaXRight == 0.0)//if we only have half the data
            {
                FWHM = 2 * deltaXLeft;
                return FWHM;
            }

            if (deltaXLeft == 0.0)
            {
                FWHM = 2 * deltaXLeft;
                return FWHM;
            }

            //If we have a weak linear extrapolation on one half of the peak and a stronger iterpolation or quadratic extrapolation
            //on the other side it is better to double the interpolation or quadratic extrapolation.
            if (detectedMethodLeft == FWHMPointFindingOptions.LinearExtrapolation)
            {
                if (detectedMethodRight == FWHMPointFindingOptions.QuadraticExtrapolation || detectedMethodRight == FWHMPointFindingOptions.Interpolated)
                {
                    deltaXLeft = deltaXRight;
                }
            }

            if (detectedMethodRight == FWHMPointFindingOptions.LinearExtrapolation)
            {
                if (detectedMethodLeft == FWHMPointFindingOptions.QuadraticExtrapolation || detectedMethodLeft == FWHMPointFindingOptions.Interpolated)
                {
                    deltaXRight = deltaXLeft;
                }
            }

            FWHM = deltaXLeft + deltaXRight;
            return FWHM;
        }

        #endregion

        /// <summary>
        /// Sets the options for finding the FWHM of the data.  Unassigned is the default
        /// Interpolated occures when there is enough data to interpolate betwen 2 detected data points.  this works well for resolved peaks
        /// Linear Extrapolation.  When there are not enough data points N=2, project a linear line to get the FWHM
        /// Quadratic Extrapolation.  When ther are more than 2 points on a side we can fit a curve.  The curve is fit to the full peak, not just the side to prevent convex curves
        /// </summary>
        enum FWHMPointFindingOptions
        {
            Unassigned,
            Interpolated,
            LinearExtrapolation,
            QuadraticExtrapolation
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
