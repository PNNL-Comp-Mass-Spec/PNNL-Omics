using System;
using System.IO;
using System.Collections.Generic;

namespace PNNLOmics.IO.FileReaders
{
	public interface ITextFileReader<T>
	{
		// TODO: Add XML Comments
		// TODO: Abstract this out from PNNLOmics entirely
		IEnumerable<T> ReadFile(string fileLocation);
		IEnumerable<T> ReadFile(TextReader textReader);
	}
}
