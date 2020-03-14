namespace PNNLOmics.Data.Constants
{
    /// <summary>
    /// Encapsulates...
    /// </summary>
    //TODO: SCOTT - CR - Fill in XML comment.
    public class UserUnit : Matter
    {
        /// <summary>
        /// constructor for a minimal userUnit that has the required name and symbol
        /// </summary>
        /// <param name="name">name to call the user unit such as sodium</param>
        /// <param name="symbol">short name to reference it by, such as Na for sodium</param>
        public UserUnit(string name, string symbol)
        {
            Name = name;
            Symbol = symbol;
        }

        /// <summary>
        /// constructor for a full userUnit that has the all fields
        /// </summary>
        /// <param name="name">name to call the user unit such as sodium</param>
        /// <param name="symbol">short name to reference it by, such as Na for sodium</param>
        /// <param name="mass">monoisotopic mass</param>
        /// <param name="userUnitName">enum UserUnitName (user01, user02, user03)</param>
        public UserUnit(string name, string symbol, double mass, UserUnitName userUnitName)
        {
            Name = name;
            Symbol = symbol;
            MassMonoIsotopic = mass;
            UserUnitType = userUnitName;
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public UserUnit()
        {
        }

        /// <summary>
        /// Gets or sets the type of UserUnit.
        /// </summary>
        public UserUnitName UserUnitType
        {
            get;
            set;
        }
    }
}
