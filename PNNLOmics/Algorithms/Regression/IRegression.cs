using System.Collections.Generic;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt.BasisFunctions;

namespace PNNLOmics.Algorithms.Regression
{
    /// <summary>
    /// interface for regression packages
    /// </summary>
    /// <typeparam name="T">data type for X and Y data</typeparam>
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Regression")]
    public interface IRegression<T>
    {
        /// <summary>
        /// basic fit interface
        /// </summary>
        /// <param name="x">experimental intensity array</param>
        /// <param name="basisFunction">type of curve to fit</param>
        /// <param name="coeffs">Guess (in) and Fit values (out)</param>
        /// <returns></returns>
        FitReport Fit(IEnumerable<T> x, IEnumerable<T> y, BasisFunctionsEnum basisFunction, ref T[] coeffs);
    }
}
