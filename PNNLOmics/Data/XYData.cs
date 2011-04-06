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
    }
}