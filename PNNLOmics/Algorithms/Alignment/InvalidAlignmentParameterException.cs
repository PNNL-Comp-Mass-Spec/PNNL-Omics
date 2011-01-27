using System;

namespace PNNLOmics.Algorithms.Alignment
{
    public class InvalidAlignmentParameterException : Exception
    {
        public InvalidAlignmentParameterException(string message)
            : base(message)
        {
        }

        public InvalidAlignmentParameterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
