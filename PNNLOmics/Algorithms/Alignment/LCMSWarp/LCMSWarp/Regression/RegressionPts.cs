namespace LCMS.Regression
{
    public class RegressionPts
    {
        private double m_x;
        private double m_massError;
        private double m_netError;

        public double X
        {
            get { return m_x; }
            set { m_x = value; }
        }
        public double MassError
        {
            get { return m_massError; }
            set { m_massError = value; }
        }
        public double NetError
        {
            get { return m_netError; }
            set { m_netError = value; }
        }

        public void set(double x, double mass_error, double net_error)
        {
            m_x = x;
            m_massError = mass_error;
            m_netError = net_error;
        }
    }
}
