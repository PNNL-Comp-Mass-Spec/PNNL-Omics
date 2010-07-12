namespace PNNLOmics.Generic
{
    /// <summary>
    /// Container class that stores a generic pair of objects
    /// </summary>
    /// <typeparam name="T">Type of the first object in the Pair</typeparam>
    /// <typeparam name="U">Type of the second object in the Pair</typeparam>
    public class Pair<T, U>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of Pair with both objects set to default
        /// </summary>
        public Pair()
        {
            First = default(T);
            Second = default(U);
        }

        /// <summary>
        /// Initializes a new instance of Pair with the provided objects
        /// </summary>
        /// <param name="first">The first object of the Pair</param>
        /// <param name="second">The second object of the Pair</param>
        public Pair(T first, U second)
        {
            First = first;
            Second = second;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the first object in the Pair
        /// </summary>
        public T First { get; set; }

        /// <summary>
        /// Gets or sets the second object in the Pair
        /// </summary>
        public U Second { get; set; }
        #endregion
    }
}