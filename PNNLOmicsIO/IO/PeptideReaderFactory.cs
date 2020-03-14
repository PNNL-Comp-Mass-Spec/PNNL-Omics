namespace PNNLOmicsIO.IO
{
    public class PeptideReaderFactory
    {
        public static ISequenceFileReader CreateReader(string path)
        {
            if (path == null)
                return null;

            var type = GetFileType(path);

            if (type == SequenceFileType.None)
                return null;

            return CreateReader(type);
        }
        public static ISequenceFileReader CreateReader(SequenceFileType type)
        {
            ISequenceFileReader reader = null;

            switch (type)
            {
                case SequenceFileType.SEQUESTFirstHit:
                    break;
                case SequenceFileType.MSGF:
                    reader = new MsgfReader();
                    break;
                case SequenceFileType.SkylineTransitionFile:
                    reader = new SkylineTransitionFileReader();
                    break;
            }

            return reader;
        }


        private static SequenceFileType GetFileType(string peptidePath)
        {
            var type = SequenceFileType.None;
            var lowerPath = peptidePath.ToLower();

            if (lowerPath.EndsWith("msgfplus_fht.txt") ||
                lowerPath.EndsWith("msgfplus_syn.txt") ||
                lowerPath.EndsWith("msgfdb_fht.txt") ||
                lowerPath.EndsWith("msgfdb_syn.txt"))
            {
                type = SequenceFileType.MSGF;
            }
            else if (lowerPath.EndsWith("fht_msgf.txt"))
            {
                type = SequenceFileType.MSGF;
            }
            else if (lowerPath.EndsWith("syn.txt"))
            {
                type = SequenceFileType.SEQUESTSynopsis;
            }
            else if (lowerPath.EndsWith("fht.txt"))
            {
                type = SequenceFileType.SEQUESTFirstHit;
            }
            else if (lowerPath.EndsWith("msgf.tsv"))
            {
                type = SequenceFileType.MSGFTsv;
            }
            return type;
        }
    }

    /// <summary>
    /// Types of peptide sequence fields to read.
    /// </summary>
    public enum SequenceFileType
    {
        SEQUESTFirstHit,
        SEQUESTSynopsis,
        MSGF,
        MSGFTsv,
        SkylineTransitionFile,
        XTandem,
        None
    }
}
