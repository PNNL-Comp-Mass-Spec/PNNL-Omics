using System.Collections.Generic;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// Holds matches from drift time alignments.
    /// </summary>
    public class DriftTimeAlignmentResults<TTarget, TObserved>
		where TTarget   : FeatureLight, new()
        where TObserved : FeatureLight, new()
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="matches"></param>
        /// <param name="alignmentFunction"></param>
        public DriftTimeAlignmentResults(List<FeatureMatch<TTarget, TObserved>> matches, LinearEquation alignmentFunction)
        {
            Matches             = matches;
            AlignmentFunction   = alignmentFunction;
        }

        #region Properties.
        /// <summary>
        /// Gets the matches made by the drift time alignment algorithm.
        /// </summary>
        public List<FeatureMatch<TTarget, TObserved>> Matches
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the alignment function between the matches.
        /// </summary>
        public LinearEquation AlignmentFunction
        {
            get; 
            private set;
        }
        #endregion
    }
}
