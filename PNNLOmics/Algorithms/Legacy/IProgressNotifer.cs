using System;

namespace PNNLOmics.Algorithms
{
    /// <summary>
    /// Interface defining signature for a progress notifier object.
    /// </summary>
    [Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms")]
    public interface IProgressNotifer
    {
        event EventHandler<ProgressNotifierArgs> Progress;
    }
}
