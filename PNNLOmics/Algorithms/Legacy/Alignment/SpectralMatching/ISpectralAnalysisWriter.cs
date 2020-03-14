using System;

namespace PNNLOmics.Algorithms.Alignment.SpectralMatching
{
	[Obsolete("Code moved to MultiAlignWinOmics: MultiAlignCore.Algorithms.Alignment.SpectralMatching")]
    public interface ISpectralAnalysisWriter
    {
        void Write(SpectralAnalysis analysis);
        void WriteLine(string value);
        void Close();
    }
}
