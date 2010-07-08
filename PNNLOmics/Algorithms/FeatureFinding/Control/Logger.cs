using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace PNNLOmics.Algorithms.FeatureFinding.Control
{
	/// <summary>
	/// The Logger class controls the logging of the Feature Finding output.
	/// </summary>
	public class Logger
	{
		private TextWriter m_textWriter;

		/// <summary>
		/// This constructor creates a log file based on the output directory and the input file name in the given Settings file.
		/// The log file has AutoFlush set to true so that it may be peeked during Feature Finding.
		/// </summary>
		/// <param name="settings">Settings object</param>
		public Logger(Settings settings)
		{
			String baseFileName = Regex.Split(settings.InputFileName, "_isos")[0];

			StreamWriter streamWriter = new StreamWriter(settings.OutputDirectory + baseFileName + "_FeatureFinder_Log.txt");
			streamWriter.AutoFlush = true;
			m_textWriter = streamWriter;
		}

		/// <summary>
		/// Logs a string of text with the current timestamp.
		/// </summary>
		/// <param name="textToLog">The string to be logged.</param>
		public void Log(String textToLog)
		{
			DateTime currentTime = DateTime.Now;
			String logText = String.Format("{0:MM/dd/yyyy HH:mm:ss}", currentTime) + "\t" + textToLog;
			m_textWriter.WriteLine(logText);
			Console.WriteLine(logText);
		}

		/// <summary>
		/// Closes the log file.
		/// </summary>
		public void CloseLog()
		{
			m_textWriter.Close();
		}
	}
}
