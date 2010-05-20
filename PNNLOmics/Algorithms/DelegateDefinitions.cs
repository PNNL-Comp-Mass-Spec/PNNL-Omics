using System;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms
{

    /// <summary>
    /// Function for calculating the distance between two features.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public delegate double DistanceFunction(UMC x, UMC y);
}