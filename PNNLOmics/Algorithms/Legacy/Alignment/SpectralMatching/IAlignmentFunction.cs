using System;

namespace PNNLOmics.Algorithms.Alignment.SpectralMatching
{
	[Obsolete("Code moved to MultiAlignWinOmics: MultiAlignCore.Algorithms.Alignment.SpectralMatching")]
    public interface IAlignmentFunction
    {
        double AlignNet(double value);
        double AlignMass(double value);
    }
}