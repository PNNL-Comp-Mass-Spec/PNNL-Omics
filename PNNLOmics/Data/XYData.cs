using System.Collections.Generic;
using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace PNNLOmics.Data
{
    public class XYData: BaseData
    {                
        public XYData(float newX, float newY)
        {
            X = newX;
            Y = newY;
        }

        public float X
        {
            get;
            set;
        }
        public float Y
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