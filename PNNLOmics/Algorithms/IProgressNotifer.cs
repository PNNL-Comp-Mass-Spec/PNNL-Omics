using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms
{
    /// <summary>
    /// Interface defining signature for a progress notifier object.
    /// </summary>
    public interface IProgressNotifer
    {
        event EventHandler<ProgressNotifierArgs> Progress;
    }

    /// <summary>
    /// Argument object for a progress notifier.
    /// </summary>
    public class ProgressNotifierArgs: EventArgs
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
        /// Gets or sets the percentage complete.
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
