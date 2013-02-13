using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmicsIO.IO
{
    public class PeptideReaderFactory
    {
        public static ISequenceFileReader CreateReader(SequenceFileType type)
        {
            ISequenceFileReader reader = null;

            switch (type)
            {
                case SequenceFileType.SEQUEST:
                    break;
                case SequenceFileType.MSGF:
                    reader = new MsgfReader();
                    break;
                case SequenceFileType.SkylineTransitionFile:
                    reader = new SkylineTransitionFileReader();
                    break;
                default:
                    break;
            }

            return reader;
        }
    }

    /// <summary>
    /// Types of peptide sequence fiels to read.
    /// </summary>
    public enum SequenceFileType
    {
        SEQUEST,
        MSGF,
        SkylineTransitionFile
    }
}
