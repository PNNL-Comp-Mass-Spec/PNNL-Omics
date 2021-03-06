﻿using System;
using System.Collections.Generic;
using PNNLOmics.Algorithms.Alignment.SpectralMatches;

namespace PNNLOmics.Algorithms.Alignment.SpectralMatching
{
    /// <summary>
    /// Holds information about each analysis window for true and false matches.
    /// </summary>
	[Obsolete("Code moved to MultiAlignWinOmics: MultiAlignCore.Algorithms.Alignment.SpectralMatching")]
    public class SpectralAnalysis
    {        
        /// <summary>
        /// Constructor
        /// </summary>
        public SpectralAnalysis()
        {                        
            MassData    = new AlignmentMeasurement<double>();
            NetData     = new AlignmentMeasurement<double>();
            Matches = new List<SpectralAnchorPointMatch>();
            Options     = new SpectralOptions();
            DatasetNames = new List<string>();
        }

        public List<string> DatasetNames { get; set; }
        public AlignmentMeasurement<double> MassData { get; set; }
        public AlignmentMeasurement<double> NetData { get; set; }

        public SpectralOptions Options { get; set; }
        /// <summary>
        /// Gets or sets the list of matches.
        /// </summary>
        public IEnumerable<SpectralAnchorPointMatch> Matches { get; set; }

        
    }

}
