using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmicsIO.IO
{
    public class MsMsFileWriterFactory
    {
        /// <summary>
        /// Creates a spectra writer based on the file type.
        /// </summary>
        /// <param name="writerType"></param>
        /// <returns></returns>
        public static IMsMsSpectraWriter CreateSpectraWriter(MsMsWriterType writerType)
        {
            IMsMsSpectraWriter writer = null;
            switch (writerType)
            {
                case MsMsWriterType.DTA:
                    writer = new DtaFileWriter();
                    break;
                case MsMsWriterType.MGF:
                    writer = new MgfFileWriter();
                    break;
                default:
                    break;
            }
            return writer;
        }
        /// <summary>
        /// Creates a spectra writer based on the file type.
        /// </summary>
        /// <param name="writerType"></param>
        /// <returns></returns>
        public static IMsMsSpectraWriter CreateSpectraWriter(string  extension)
        {
            switch (extension)
            {
                case ".dta":
                    return CreateSpectraWriter(MsMsWriterType.DTA);
                    break;
                case ".mgf":
                    return CreateSpectraWriter(MsMsWriterType.MGF);
                    break;
            }
            return null;
        }
    }

    public enum MsMsWriterType
    {
        DTA,
        MGF
    }
}
