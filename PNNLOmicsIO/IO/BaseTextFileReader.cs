using System;
using System.Collections.Generic;
using System.IO;
using PNNLOmics.Annotations;

namespace PNNLOmicsIO.IO
{
	public abstract class BaseTextFileReader<T> : ITextFileReader<T>
	{
        /// <summary>
        /// Default file delimeter.
        /// </summary>
        private const string DEFAULT_DELIMETER = ",";
	    protected BaseTextFileReader()
		{
            Delimeter = DEFAULT_DELIMETER;
		}

        #region Properties
        /// <summary>
        /// Gets or sets the file reading delimeter.
        /// </summary>
        [UsedImplicitly]
        public string Delimeter
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
		public IEnumerable<T> ReadFile(string fileLocation) 
		{
            IEnumerable<T> returnEnumerable;
            using (TextReader textReader = new StreamReader(fileLocation))
            {
                returnEnumerable = ReadFile(textReader);
                textReader.Close();
            }
			return returnEnumerable;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textReader"></param>
        /// <returns></returns>
		public IEnumerable<T> ReadFile(TextReader textReader)
		{
			var columnMapping = CreateColumnMapping(textReader);

			if (columnMapping.Count == 0)
			{
				throw new ApplicationException("Given file does not contain any valid column headers.");
			}

			var enumerable = SaveFileToEnumerable(textReader, columnMapping);
			return enumerable;
		}

		protected abstract Dictionary<string, int> CreateColumnMapping(TextReader textReader);
		protected abstract IEnumerable<T> SaveFileToEnumerable(TextReader textReader, Dictionary<string, int> columnMapping);
	}
}
