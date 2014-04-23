using System.Collections.Generic;

namespace Processor
{
    class ErrorHistogram
    {
        private double[,] m_histogram;

        public double[,] Histogram
        {
            get { return m_histogram; }
            set { m_histogram = value; }
        }

        public ErrorHistogram(List<double> bin, List<int> frequency)
        {
            m_histogram = new double[bin.Count, 2];
            for (int i = 0; i < bin.Count; i++)
            {
                m_histogram[i, 0] = bin[i];
                m_histogram[i, 1] = frequency[i];
            }
        }
    }
}
