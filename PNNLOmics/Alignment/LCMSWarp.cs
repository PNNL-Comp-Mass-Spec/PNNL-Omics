using System;
using System.Collections.Generic;


namespace PNNLOmics.Alignment
{
    /// <summary>
    /// LCMSWarp Algorithm for aligning Feature Base objects.
    /// </summary>
    public class LCMSWarp: Processor<Feature, LCMSWarpOptions, Feature>
    {
        /// <summary>
        /// Executes the LCMSWarp Algorithm.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override Feature Execute(Feature data, LCMSWarpOptions options)
        {
            Options = options;

            return null;
        }

        /// <summary>
        /// Gets or sets the options for performing LCMSWarp
        /// </summary>
        public LCMSWarpOptions Options
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Enumeration describing what type of recalibration functions to use.
    /// </summary>
    public enum LCMSWarpCalibrationType
    {
        MZRegression = 0,
        ScanRegression, 
        Hybrid
    }; 
}
