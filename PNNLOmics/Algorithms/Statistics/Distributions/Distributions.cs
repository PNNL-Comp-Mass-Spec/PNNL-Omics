using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.Distributions;

namespace PNNLOmics.Algorithms.Statistics.Distributions
{
    public class Distributions
    {
        public static List<double> CreateNormalDistribution(int samples, double mu, double sigma)
        {            
            List<double> data = new List<double>();
            NormalDistribution dist = new NormalDistribution(mu, sigma);
            for (int i = 0; i < samples; i++)
            {
                data.Add(dist.NextDouble());
            }
            return data;
        }

        public static List<double> CreateUniform(int samples)
        {
            alglib.hqrndstate state = new alglib.hqrndstate();
            alglib.hqrndrandomize(out state);


            List<double> data = new List<double>();
            for (int i = 0; i < samples; i++)
            {
                data.Add(alglib.hqrnduniformr(state));
            }
            return data;
        }
    }    
}
