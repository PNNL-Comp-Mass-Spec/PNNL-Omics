using System;
using System.Collections.Generic;

namespace LCMS.Regression
{
    class NormUnifEM
    {
        private double m_mean;
        private double m_var;
        private double m_normFraction;
        private int m_numIterations;
        private List<double> m_unifProb;

        private static double MIN_VAR = 0.0001;

        public double StandDev
        {
            get { return Math.Sqrt(m_var); }
        }
        public double Mean
        {
            get { return m_mean; }
            set { m_mean = value; }
        }
        public double NormProb
        {
            get { return m_normFraction; }
            set { m_normFraction = value; }
        }

        public NormUnifEM()
        {
            m_mean = 0.0;
            m_var = 10.0;
            m_normFraction = 0.5;
            m_numIterations = 16;
            m_unifProb = new List<double>();
        }

        public void Reset()
        {
            m_mean = 0.0;
            m_var = 10.0;
            m_normFraction = 0.5;
            m_numIterations = 16;
            m_unifProb = new List<double>();
        }

        public void CalculateDistributions(ref List<double> listVals)
        {
            Reset();
            if (listVals.Count == 0)
            {
                m_mean = 0;
                m_var = 0.1;
                m_normFraction = 0;
                return;
            }

            double minVal = double.MaxValue;
            double maxVal = -1 * double.MaxValue;
            for (int i = 0; i < listVals.Count; i++)
            {
                if (listVals[i] < minVal)
                {
                    minVal = listVals[i];
                }
                if (listVals[i] > maxVal)
                {
                    maxVal = listVals[i];
                }
            }
            if (minVal == maxVal)
            {
                m_mean = maxVal;
                m_var = 0.1;
                m_normFraction = 0;
                return;
            }
            double u = 1 / (maxVal - minVal);

            int num_pts = listVals.Count;

            m_unifProb.Clear();
            m_unifProb.Capacity = num_pts;

            for (int iteration = 0; iteration < m_numIterations; iteration++)
            {
                double mean_next = 0;
                double var_next = 0;
                double norm_fraction_next = 0;
                for (int pointNum = 0; pointNum < num_pts; pointNum++)
                {
                    double val = listVals[pointNum];
                    double diff = val - m_mean;
                    double norm_prob = Math.Exp(-(0.5 * diff * diff) / m_var) / (Math.Sqrt(2 * Math.PI) * Math.Sqrt(m_var));
                    double post_norm_prob = (norm_prob * m_normFraction) / (norm_prob * m_normFraction + (1 - m_normFraction) * u);
                    m_unifProb.Add(post_norm_prob);

                    norm_fraction_next += post_norm_prob;
                    mean_next += post_norm_prob * val;
                    var_next += post_norm_prob * (val - m_mean) * (val - m_mean);
                }
                m_normFraction = norm_fraction_next / num_pts;
                m_mean = mean_next / norm_fraction_next;
                m_var = var_next / norm_fraction_next;
                if (m_var < MIN_VAR)
                {
                    break;
                }
            }
        }
    }
}
