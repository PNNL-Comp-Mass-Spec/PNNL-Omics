using System.Collections.Generic;
using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace PNNLOmics.Data
{
    public class XYData: BaseData
    {                
        public XYData(double newX, double newY)
        {
            X = newX;
            Y = newY;
        }

        public double X
        {
            get;
            set;
        }
        public double Y
        {
            get;
            set;
        }
        public override void Clear()
        {
            X = 0;
            Y = 0;
        }
        public static List<XYData> Bin(List<XYData> data, double binSize)
        {            
            double lowMass       = data[0].X;
            double highMass      = data[data.Count - 1].X;
            return Bin(data, lowMass, highMass, binSize);
        }

        public  static List<XYData> Bin(List<XYData> data, double lowMass, double highMass, double binSize)
        {
            List<XYData> newData = new List<XYData>();
            int total            = Convert.ToInt32((highMass - lowMass)/binSize);

            for (int i = 0; i < total; i++)
            {
                XYData part = new XYData(lowMass + (Convert.ToDouble(i) * binSize), 0.0);
                newData.Add(part);
            }

            for (int i = 0; i < data.Count; i++)
            {
                double intensity = data[i].Y;
                int bin = Math.Min(total - 1, System.Convert.ToInt32((data[i].X - lowMass) / binSize));
                try
                {
                    newData[bin].Y += intensity;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return newData;
        }


        /// <summary>
        /// Convert XYData to arrays to interact with other functions more easily.
        /// </summary>
        /// <param name="xyList">List of XYData values to be converted.</param>
        /// <param name="xArray">Array to be populated with X values.</param>
        /// <param name="yArray">Array to be populated with Y values.</param>
        public static void XYDataListToArrays(List<XYData> xyList, double[] xArray, double[] yArray)
        {
            if (xArray.Length == xyList.Count || yArray.Length == xyList.Count)
            {
                for (int i = 0; i < xyList.Count; i++)
                {
                    xArray[i] = xyList[i].X;
                    yArray[i] = xyList[i].Y;
                }
            }
            else
            {
                throw new InvalidOperationException("X and Y arrays must be same length as XYData list in function XYDataListToArrays.");
            }
        }
    }
}