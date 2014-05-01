namespace PNNLOmics.Data.Constants
{
    /// <summary>
    /// This is an abstract Class designed to cover the most basic parameters of matter objects.
    /// </summary>
    public abstract class Matter
    {
        /// <summary>
        /// name of the matter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// moniosopic mass of the matter
        /// </summary>
        public double MassMonoIsotopic { get; set; }

        /// <summary>
        /// short hand symbol for the matter
        /// </summary>
        public string Symbol { get; set; }
    }
}
