using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace PNNLOmics.Utilities
{
    public static class PathUtilities
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
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

	}
}
