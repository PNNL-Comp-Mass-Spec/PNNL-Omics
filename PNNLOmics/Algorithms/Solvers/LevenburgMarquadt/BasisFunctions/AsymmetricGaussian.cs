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
        public override void FunctionDelegate(double[] c, double[] x, ref double functionResult, object obj)
        {
            throw new NotImplementedException();
        }
    }
}
