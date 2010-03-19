
using System;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Interface for filtering data types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFilter<T>
    {
        /// <summary>
        /// Determines if data passes the filter.
        /// </summary>
        /// <param name="data">Data to interrogate.</param>
        /// <returns>True if passes, false if does not pass.</returns>
        bool Passes(T data);
    }
}
