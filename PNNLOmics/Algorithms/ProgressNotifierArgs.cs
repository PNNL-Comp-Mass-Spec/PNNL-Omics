using System;

namespace PNNLOmics.Algorithms
{
    /// <summary>
    /// Argument object for a progress notifier.
    /// </summary>
    [Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms")]
    public sealed class ProgressNotifierArgs: EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"></param>
        public ProgressNotifierArgs(string message)
        {
            Message         = message;
            PercentComplete = 0;
        }

        public ProgressNotifierArgs(string message,
            double percentComplete)
        {
            Message         = message;
            PercentComplete = percentComplete;
        }

        /// <summary>
        /// Gets or sets the percentage complete (value between 0 and 100)
        /// </summary>
        public double PercentComplete
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message
        {
            get;
            private set;
        }
    }
}