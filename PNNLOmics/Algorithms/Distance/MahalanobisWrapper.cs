using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PNNLOmics.Algorithms.Distance
{
    public class MahalanobisWrapper<T> where T : FeatureLight, new()
    {
        private DenseMatrix CreateMatrix(List<T> x)
        {
            //double [,] y = new double[x.cou

            return null;
        }

        public double Mahalanobis(List<T> x, T y)
        {

            
           // DenseMatrix featureY = new DenseMatrix(


           // return MahalanobisDistanceCalculator.CalculateMahalanobisDistance(featureX, featureY);            

           return 0;
        }
    }
}
