using System;
using System.IO;
using System.Collections.Generic;

namespace PNNLOmics.IO.FileReaders
{
	public abstract class BaseTextFileReader<T> : ITextFileReader<T>
	{
		// TODO: Change to DelimitedTextFileReader
		// TODO: Pass in delimiter?
		public BaseTextFileReader()
		{
		}

		public IEnumerable<T> ReadFile(string fileLocation) 
		{
			TextReader textReader = new StreamReader(fileLocation);
			return ReadFile(textReader);
		}

		public IEnumerable<T> ReadFile(TextReader textReader)
		{
			Dictionary<string, int> columnMapping = CreateColumnMapping(textReader);

			if (columnMapping.Count == 0)
			{
				throw new ApplicationException("Given file does not contain any valid column headers.");
			}

			IEnumerable<T> enumerable = SaveFileToEnumerable(textReader, columnMapping);
			return enumerable;
		}

		protected abstract Dictionary<string, int> CreateColumnMapping(TextReader textReader);
		protected abstract IEnumerable<T> SaveFileToEnumerable(TextReader textReader, Dictionary<string, int> columnMapping);
	}
}
