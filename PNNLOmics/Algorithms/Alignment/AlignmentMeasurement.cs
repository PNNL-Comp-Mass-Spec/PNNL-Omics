﻿using System.Collections.Generic;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// Encapsulates the pre and post alignment error for a given metric
    /// </summary>
    public class AlignmentMeasurement<T>
    {
        public AlignmentMeasurement()
        {
            PreAlignment  = new List<T>();
            PostAlignment = new List<T>();
        }
        /// <summary>
        /// Gets or sets the pre alignment value
        /// </summary>
        public List<T> PreAlignment { get; set; }

        /// <summary>
        /// Gets or sets the post alignment value
        /// </summary>
        public List<T> PostAlignment { get; set; }
    }
}
