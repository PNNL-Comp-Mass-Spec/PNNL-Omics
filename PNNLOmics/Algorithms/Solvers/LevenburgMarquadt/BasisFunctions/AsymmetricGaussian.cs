using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.Solvers.LevenburgMarquadt.BasisFunctions
{
    class AsymmetricGaussian: BasisFunctionBase
    {
        /*
         denom = math.sqrt(2 * math.log(2)) * (hp + hm)
         a = 2 * hp * hm / denom
         b = (hp - hm) / denom
         sigma = a + b * (x - x0)
         return A * math.exp(-0.5 * ((x - x0) / sigma)**2)
         */
        public override void FunctionDelegate(double[] c, double[] xv, ref double functionResult, object obj)
        {
            double x     = xv[0];
            double x0    = c[0];          
            double A     = c[1];
            double hm    = c[2];
            double hp    = c[3];  
            double denom = Math.Sqrt(2 * Math.Log(2))  * (hp + hm);           
            double a     = 2 * hp * hm / denom;
            double b     = (hp - hm) / denom;

            double sigma    = a + b * (x - x0);
            functionResult = A * Math.Exp(-.5 * Math.Pow(((x - x0) / sigma), 2));            
        }
    }
}
