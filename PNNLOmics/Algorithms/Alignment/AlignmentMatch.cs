using System;

namespace PNNLOmics.Algorithms.Alignment
{
    /// <summary>
    /// TODO: Create class comment block for AlignmentMatch
    /// </summary>
    public class AlignmentMatch
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of AlignmentMatch.
        /// </summary>
        public AlignmentMatch()
        {
            Clear();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the first net start
        /// </summary>
        public double NetStartA { get; set; }

        /// <summary>
        /// Gets or sets the first net end
        /// </summary>
        public double NetEndA { get; set; }

        /// <summary>
        /// Gets or sets the first section start
        /// </summary>
        public int SectionStartA { get; set; }

        /// <summary>
        /// Gets or sets the first section end
        /// </summary>
        public int SectionEndA { get; set; }

        /// <summary>
        /// Gets or sets the second net start
        /// </summary>
        public double NetStartB { get; set; }

        /// <summary>
        /// Gets or sets the second net end
        /// </summary>
        public double NetEndB { get; set; }

        /// <summary>
        /// Gets or sets the second section start
        /// </summary>
        public int SectionStartB { get; set; }

        /// <summary>
        /// Gets or sets the second section end
        /// </summary>
        public int SectionEndB { get; set; }

        /// <summary>
        /// The score of the alignments between the two maps up to and including
        /// this section match.
        /// </summary>
        public double AlignmentScore { get; set; }

        /// <summary>
        /// The score of just the match between the two maps and their sections.
        /// </summary>
        public double MatchScore { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears all values back to zero.
        /// </summary>
        public void Clear()
        {
            NetStartA = 0.0;
            NetEndA = 0.0;
            SectionStartA = 0;
            SectionEndA = 0;
            NetStartB = 0.0;
            NetEndB = 0.0;
            SectionStartB = 0;
            SectionEndB = 0;
            AlignmentScore = 0.0;
            MatchScore = 0.0;
        }

        /// <summary>
        /// TODO: Create comment block for Set
        /// </summary>
        /// <param name="netStartA"></param>
        /// <param name="netEndA"></param>
        /// <param name="sectionStartA"></param>
        /// <param name="sectionEndA"></param>
        /// <param name="netStartB"></param>
        /// <param name="netEndB"></param>
        /// <param name="sectionStartB"></param>
        /// <param name="sectionEndB"></param>
        /// <param name="alignmentScore"></param>
        /// <param name="matchScore"></param>
        public void Set(double netStartA, double netEndA, int sectionStartA,
            int sectionEndA, double netStartB, double netEndB, int sectionStartB,
            int sectionEndB, double alignmentScore, double matchScore)
        {
            NetStartA = netStartA;
            NetEndA = netEndA;
            SectionStartA = sectionStartA;
            SectionEndA = sectionEndA;
            NetStartB = netStartB;
            NetEndB = netEndB;
            SectionStartB = sectionStartB;
            SectionEndB = sectionEndB;
            AlignmentScore = alignmentScore;
            MatchScore = matchScore;
        }
        #endregion
    }
}