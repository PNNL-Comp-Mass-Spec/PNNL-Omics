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
    public delegate double DistanceFunction(UMCLight x, UMCLight y);
    /// <summary>
    /// Weighted distance function for comparing the distance between two UMC's.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="massWeight"></param>
    /// <param name="netWeight"></param>
    /// <param name="driftWeight"></param>
    /// <returns></returns>
    public delegate double WeightedDistanceFunction(UMCLight x, UMCLight y,double massWeight, double netWeight, double driftWeight);
}