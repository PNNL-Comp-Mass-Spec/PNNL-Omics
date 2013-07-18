// Written by Kevin Crowell, Spencer Prost and Gordon Slysz for the Department of Energy (PNNL, Richland, WA)
// Adapted by Scott Kronewitter for the Department of Energy (PNNL, Richland, WA)
// Copyright 2012, Battelle Memorial Institute
// E-mail: gordon.slysz@pnl.gov
// Website: http://panomics.pnnl.gov/software/
// -------------------------------------------------------------------------------
// 
// Licensed under the Apache License, Version 2.0; you may not use this file except
// in compliance with the License.  You may obtain a copy of the License at 
// http://www.apache.org/licenses/LICENSE-2.0


using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.SpectralProcessing
{
    public class SavitzkyGolaySmoother
    {
        DenseMatrix _smoothingFilters;

        public SavitzkyGolaySmoother(int pointsForSmoothing, int polynomialOrder, bool allowNegativeValues = true)
        {
            PointsForSmoothing = pointsForSmoothing;
            PolynomialOrder = polynomialOrder;
            AllowNegativeValues = allowNegativeValues;
        }

        public int PointsForSmoothing { get; set; }
        public int PolynomialOrder { get; set; }

        /// <summary>
        /// When smoothing, smoothed values that once were positive, may become negative. This will zero-out any negative values
        /// </summary>
        public bool AllowNegativeValues { get; set; }

        /// <summary>
        /// different values than decon tools and 4x slower
        /// </summary>
        /// <param name="xyData"></param>
        /// <returns></returns>
        public List<XYData> Smooth(List<XYData> xyData)
        {
            //if (xyData == null || xyData.Count == 0) return xyData;
            if (xyData == null || xyData.Count <3) return xyData;

            bool pureArrayTrueOrFalse = false;//defaulted to omicsList
            if (pureArrayTrueOrFalse)
            {
                return SmoothArray(xyData);//convert omcis to array first
            }
            else
            {
                return SmoothOmicsList(xyData);//direct call
            }
        }

        

        /// <summary>
        /// Performs SavitzkyGolay smoothing
        /// </summary>
        /// <param name="inputValues">Input values</param>
        /// <returns></returns>
        private List<XYData> SmoothArray(List<XYData> inputValues)
         {
             double[] yValues = inputValues.Select(x => x.Y).ToArray();

             double[] smoothedData = SmoothArray(yValues);


             for (int i = 0; i < smoothedData.Count(); i++)
             {
                 inputValues[i].Y = smoothedData[i];
             }

             return inputValues;
         }

        
        /// <summary>
        /// Performs SavitzkyGolay smoothing on a list of XYData
        /// </summary>
        /// <param name="inputValues">Input values</param>
        /// <returns></returns>
        private List<XYData> SmoothOmicsList(List<XYData> inputValues)
        {
            if (PointsForSmoothing < 3)
                throw new ArgumentOutOfRangeException("savGolayPoints must be an odd number 3 or higher");

            if (PointsForSmoothing % 2 == 0)
                throw new ArgumentOutOfRangeException("savGolayPoints must be an odd number 3 or higher");

            int m = (PointsForSmoothing - 1) / 2;
            int colCount = inputValues.Count;
            double[] yvalues = inputValues.Select(point => point.Y).ToArray();

            if (_smoothingFilters == null)
            {
                _smoothingFilters = CalculateSmoothingFilters(PolynomialOrder, PointsForSmoothing);
            }

            Matrix<double> conjTransposeMatrix = _smoothingFilters.ConjugateTranspose();

            Vector<double> conjTransposeColumn;
            double multiplicationResult;
            for (int i = 0; i <= m; i++)
            {
                conjTransposeColumn = conjTransposeMatrix.Column(i);

                multiplicationResult = 0;
                for (int z = 0; z < PointsForSmoothing; z++)
                {
                    multiplicationResult += (conjTransposeColumn[z] * yvalues[z]);
                }
                inputValues[i].Y = multiplicationResult;
            }

            Vector<double> conjTransposeColumnResult = conjTransposeMatrix.Column(m);

            for (int i = m + 1; i < colCount - m - 1; i++)
            {
                multiplicationResult = 0;
                for (int z = 0; z < PointsForSmoothing; z++)
                {
                    multiplicationResult += (conjTransposeColumnResult[z] * yvalues[i - m + z]);
                }
                inputValues[i].Y = multiplicationResult;
            }

            for (int i = 0; i <= m; i++)
            {
                conjTransposeColumn = conjTransposeMatrix.Column(m + i);

                multiplicationResult = 0;
                for (int z = 0; z < PointsForSmoothing; z++)
                {
                    multiplicationResult += (conjTransposeColumn[z] * yvalues[colCount - PointsForSmoothing + z]);
                }
                inputValues[colCount - m - 1 + i].Y = multiplicationResult;
            }

            if (!AllowNegativeValues)
            {
                foreach (XYData t in inputValues)
                {
                    if (t.Y < 0) t.Y = 0;
                }
            }

            return inputValues;
        }

        /// <summary>
        /// Performs SavitzkyGolay smoothing on an array
        /// </summary>
        /// <param name="inputValues">Input values</param>
        /// <returns></returns>
        private double[] SmoothArray(double[] inputValues)
        {
            if (PointsForSmoothing < 3)
                throw new ArgumentOutOfRangeException("savGolayPoints must be an odd number 3 or higher");

            if (PointsForSmoothing % 2 == 0)
                throw new ArgumentOutOfRangeException("savGolayPoints must be an odd number 3 or higher");

            int m = (PointsForSmoothing - 1) / 2;
            int colCount = inputValues.Length;
            double[] returnYValues = new double[colCount];

            if (_smoothingFilters == null)
            {
                _smoothingFilters = CalculateSmoothingFilters(PolynomialOrder, PointsForSmoothing);
            }

            var conjTransposeMatrix = _smoothingFilters.ConjugateTranspose();

            for (int i = 0; i <= m; i++)
            {
                var conjTransposeColumn = conjTransposeMatrix.Column(i);

                double multiplicationResult = 0;
                for (int z = 0; z < PointsForSmoothing; z++)
                {
                    multiplicationResult += (conjTransposeColumn[z] * inputValues[z]);
                }

                returnYValues[i] = multiplicationResult;
            }

            var conjTransposeColumnResult = conjTransposeMatrix.Column(m);

            for (int i = m + 1; i < colCount - m - 1; i++)
            {
                double multiplicationResult = 0;
                for (int z = 0; z < PointsForSmoothing; z++)
                {
                    multiplicationResult += (conjTransposeColumnResult[z] * inputValues[i - m + z]);
                }
                returnYValues[i] = multiplicationResult;
            }

            for (int i = 0; i <= m; i++)
            {
                var conjTransposeColumn = conjTransposeMatrix.Column(m + i);

                double multiplicationResult = 0;
                for (int z = 0; z < PointsForSmoothing; z++)
                {
                    multiplicationResult += (conjTransposeColumn[z] * inputValues[colCount - PointsForSmoothing + z]);
                }
                returnYValues[colCount - m - 1 + i] = multiplicationResult;
            }

            if (!AllowNegativeValues)
            {
                for (int i = 0; i < returnYValues.Length; i++)
                {
                    if (returnYValues[i] < 0) returnYValues[i] = 0;
                }
            }

            return returnYValues;
        }



        private DenseMatrix CalculateSmoothingFilters(int polynomialOrder, int filterLength)
        {
            int m = (filterLength - 1) / 2;
            var denseMatrix = new DenseMatrix(filterLength, polynomialOrder + 1);

            for (int i = -m; i <= m; i++)
            {
                for (int j = 0; j <= polynomialOrder; j++)
                {
                    denseMatrix[i + m, j] = Math.Pow(i, j);
                }
            }

            var sTranspose = (DenseMatrix)denseMatrix.ConjugateTranspose();
            var f = sTranspose * denseMatrix;
            var fInverse = (DenseMatrix)f.LU().Solve(DenseMatrix.Identity(f.ColumnCount));
            var smoothingFilters = denseMatrix * fInverse * sTranspose;

            return smoothingFilters;
        }

    }
}
