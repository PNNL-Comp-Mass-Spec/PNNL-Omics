using System;
using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// TODO: Create class comment block for Interpolation
    /// </summary>
    public class Interpolation
    {
        #region Class Members
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes an new instance of Interpolation
        /// </summary>
        public Interpolation()
        {

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// These alogrothims are from: Numerical Recipes in C by William H. Press,
        /// Brian P. Flannery, Saul A. Teukolsky, William T. Vetterling.
        /// 
        /// Given the arrays x[0..n-1] and y[0..n-1] containing the tabulated 
		///	function, i.e., yi = f(xi), with x0 less than x1 less than ... less
        ///	than xn-1, and given values yp1 and ypn for the first derivative of
        ///	the interpolating function at points 0 and n-1, respectively, this
        ///	routine returns an array y2[1..n] that contains the second derivatives
        ///	of the interpolating function at the tabulated points xi. If yp1 and
        ///	or ypn are equal to 1x10^30 or larger, the routine is signaled to set
        ///	the corresponding boundary condition for a natural spline, with zero
        ///	second derivative on that boundary.
        /// </summary>
        /// <param name="x">X vector of X values</param>
        /// <param name="y">Y vector of Y values</param>
        /// <param name="yp1">Second derivative at first point</param>
        /// <param name="ypn">Second derivative at the nth point</param>
        public void Spline(List<double> x, List<double> y, double yp1, double ypn)
        {
            // TODO: Implement Spline
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cubic Spline interpolation. This function does the actual interpolation
        /// at specified point, using provided second derivatives at the knot points. 
        /// </summary>
        /// <param name="xa">XA vector of X values</param>
        /// <param name="ya">YA vector of Y values</param>
        /// <param name="x">The value to find the interpolating Y value at</param>
        /// <returns>Interpolated Y at point X</returns>
        public double Splint(List<double> xa, List<double> ya, double x)
        {
            // TODO: Implement Splint
            throw new NotImplementedException();
        }
        #endregion
    }
}