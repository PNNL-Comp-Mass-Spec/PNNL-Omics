using System;
using System.IO;
using System.Reflection;

namespace PNNLOmics.Utilities
{
	class PathUtil
	{
		/// <summary>
		/// Returns the path to the directory containing the currently executing DLL
		/// </summary>
		/// <remarks>
		/// Use of GetExecutingAssembly().CodeBase is preferred to using GetExecutingAssembly().Location
		/// because when NUnit executes Unit Tests, .Location returns a path to a temporary folder instead of the path to the DLL
		/// </remarks>
		public static string AssemblyDirectory
		{
			get
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;
				var uri = new UriBuilder(codeBase);
				var path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

	}
}
