namespace Processor
{
    /// <summary>
    /// Class that defines the boundary of an MZ range to align to
    /// Holds a lower bound and an upper bound for comparison
    /// </summary>
    public class AlignmentMZBoundary
    {
        double m_boundaryLow;
        double m_boundaryHigh;

        public double BoundaryLow
        {
            get { return m_boundaryLow; }
            set { m_boundaryLow = value; }
        }

        public double BoundaryHigh
        {
            get { return m_boundaryHigh; }
            set { m_boundaryHigh = value; }
        }

        public AlignmentMZBoundary(double low, double high)
        {
            m_boundaryHigh = high;
            m_boundaryLow = low;
        }
    }
}
