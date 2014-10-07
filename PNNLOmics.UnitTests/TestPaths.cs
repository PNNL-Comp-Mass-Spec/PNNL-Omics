using System;

namespace PNNLOmics.UnitTests
{
	static class TestPaths
	{
        static TestPaths()
        {
			// The Execution directory
			var binDirFinder = Environment.CurrentDirectory;
			// Find the bin folder...
			while (!string.IsNullOrWhiteSpace(binDirFinder) && !binDirFinder.EndsWith("bin"))
	        {
				//Console.WriteLine("Project: " + binDirFinder);
				binDirFinder = System.IO.Path.GetDirectoryName(binDirFinder);
	        }
			//Console.WriteLine("Project: " + binDirFinder);
			// The Directory for the PNNLOmics.UnitTests project
			ProjectDirectory = System.IO.Path.GetDirectoryName(binDirFinder);
			// PNNLOmics.UnitTests\TestFiles Directory
			TestFilesDirectory = System.IO.Path.Combine(ProjectDirectory, "TestFiles");
			// The Solution Directory
			SolutionDirectory = System.IO.Path.GetDirectoryName(ProjectDirectory);
			//Console.WriteLine("PNNLOmics.UnitTests: " + ProjectDirectory);
			//Console.WriteLine("PNNLOmics.UnitTests/TestFiles: " + TestFilesDirectory);
			//Console.WriteLine("SolutionDir: " + SolutionDirectory);
        }

		public static string ProjectDirectory
		{
			get;
			private set;
		}

		public static string TestFilesDirectory
		{
			get;
			private set;
		}

		public static string SolutionDirectory
		{
			get;
			private set;
		}
	}
}
