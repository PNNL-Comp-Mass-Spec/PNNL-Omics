namespace PNNLOmics.Data.Constants
{
    //TODO: SCOTT - CR - add XML comments
    public class Isotope
    {
        /// <summary>
        /// This is a Class designed to contain the information on the natural abundance of heavy isotopes on earth.
        /// </summary>
        public Isotope(int isotopeNumber, double mass, double naturalAbundance)
        {
            IsotopeNumber = isotopeNumber;
            Mass = mass;
            NaturalAbundance = naturalAbundance;
        }

        //TODO: SCOTT - CR - add XML comments

        public double Mass { get; set; }
        public int IsotopeNumber { get; set; }
        public double NaturalAbundance { get; set; }
    }
}
