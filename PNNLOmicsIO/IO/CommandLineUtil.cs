using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PNNLOmicsIO.Utilities.ConsoleUtil
{
	public class CommandLineUtil
	{
		private const string SWITCH_START = "/";
		private const string SWITCH_START_ALT = "-";
		private const string SWITCH_PARAMETER_SEPARATOR = ":";

		private bool m_showHelp;
		private List<string> m_nonSwitchParameters;
		private Dictionary<string, string> m_ParameterValueMap;
		private Dictionary<string, string> m_ParameterValueMapAnyCase;

		/// <summary>
		/// True if the help contents should be displayed, False otherwise.
		/// </summary>
		public bool ShowHelp
		{
			get { return m_showHelp; }
		}

		/// <summary>
		/// Number of parameters with a switch
		/// </summary>
		public int ParameterCount
		{
			get { return m_ParameterValueMap.Count; }
		}

		/// <summary>
		/// Number of Parameters without a switch
		/// </summary>
		public int NonSwitchParameterCount
		{
			get { return m_nonSwitchParameters.Count; }
		}

		/// <summary>
		/// C-like argument parser
		/// </summary>
		/// <param name="commandLine">Command line string with arguments. Use Environment.CommandLine</param>
		/// <returns>The args[] array (argv)</returns>
		/// <remarks>Adapted rom http://sleepingbits.com/2010/01/command-line-arguments-with-double-quotes-in-net/</remarks>
		public static List<string> CreateArgs(string commandLine)
		{
			var argsBuilder = new StringBuilder(commandLine);
			bool inQuote = false;

			// Convert the spaces to a newline sign so we can split at newline later on
			// Only convert spaces which are outside the boundries of quoted text
			for (int i = 0; i < argsBuilder.Length; i++)
			{
				if (argsBuilder[i].Equals('"'))
				{
					inQuote = !inQuote;
				}

				if (argsBuilder[i].Equals(' ') && !inQuote)
				{
					argsBuilder[i] = '\n';
				}
			}

			// Split to args array
			List<string> args = argsBuilder.ToString().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

			// Clean the '"' signs from the args as needed.
			for (int i = 0; i < args.Count; i++)
			{
				args[i] = ClearQuotes(args[i]);
			}

			return args;
		}


		/// <summary>
		/// Cleans quotes from the arguments.<br/>
		/// All single quotes (") will be removed.<br/>
		/// Every pair of quotes ("") will transform to a single quote.<br/>
		/// </summary>
		/// <param name="stringWithQuotes">A string with quotes.</param>
		/// <returns>The same string if its without quotes, or a clean string if its with quotes.</returns>
		/// /// <remarks>From http://sleepingbits.com/2010/01/command-line-arguments-with-double-quotes-in-net/</remarks>
		private static string ClearQuotes(string stringWithQuotes)
		{
			int quoteIndex;
			if ((quoteIndex = stringWithQuotes.IndexOf('"')) == -1)
			{
				// String is without quotes..
				return stringWithQuotes;
			}

			// Linear sb scan is faster than string assignemnt if quote count is 2 or more (=always)
			var sb = new StringBuilder(stringWithQuotes);
			for (int i = quoteIndex; i < sb.Length; i++)
			{
				if (sb[i].Equals('"'))
				{
					// If we are not at the last index and the next one is '"', we need to jump one to preserve one
					if (i != sb.Length - 1 && sb[i + 1].Equals('"'))
					{
						i++;
					}

					// We remove and then set index one backwards.
					// This is because the remove itself is going to shift everything left by 1.
					sb.Remove(i--, 1);
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// Default constructor to intialize private fields.
		/// </summary>
		public CommandLineUtil()
		{
			m_showHelp = false;
			m_nonSwitchParameters = new List<string>();
			m_ParameterValueMap = new Dictionary<string, string>();
			m_ParameterValueMapAnyCase = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
		}

		/// <summary>
		/// Returns True if any command line parameters were found.
        /// Otherwise, returns False.
		/// 
        /// If /? or /help is found, then returns False and sets ShowHelp to True.
		/// </summary>
		/// <returns>true if any command line parameters, beyond /? or /help were found, false otherwise</returns>
		public bool ParseCommandLine()
		{
			string commandLine =  Environment.CommandLine;

			// Caution: do not use "parameters = Environment.GetCommandLineArgs();" since GetCommandLineArgs() can return odd results
			// For example, command line
			//   -T:msgfplus -F:"F:\Temp\AScore\Dataset_msgfdb_fht.txt" -D:"F:\Temp\AScore\Dataset_dta.txt" -O:"F:\Temp\AScore" -P:F:\Temp\AScore\DynPhos_stat_6plex_iodo_hcd.xml -L:LogFile.txt -FM:true
			// is incorrectly parsed by GetCommandLineArgs() to return just 4 arguments instead of 7
			//   -T:msgfplus
			//   -F:F:\\Temp\\AScore\\Dataset_msgfdb_fht.txt
			//   -D:F:\\Temp\\AScore\\Dataset_dta.txt
			//   -O:F:\\Temp\\AScore\" -P:F:\\Temp\\AScore\\DynPhos_stat_6plex_iodo_hcd.xml -L:LogFile.txt -FM:true

			var parameters = CreateArgs(commandLine);

			if (string.IsNullOrWhiteSpace(commandLine))
			{
				return false;
			}

			var helpSwitches = new List<string>
			{
				SWITCH_START + "?",
				SWITCH_START + "help",
				SWITCH_START_ALT + "?",
				SWITCH_START_ALT + "help"
			};

			foreach (string item in helpSwitches) 
			{
				if (commandLine.ToLower().Contains(item.ToLower()))
				{
					m_showHelp = true;
					return false;
				}
			}

			// Note that parameters[0] is the path to the executable for the calling program
			for (int i = 1; i < parameters.Count; i++)
			{
				string parameter = parameters[i];

				if (parameter.Length <= 0)
				{
					continue;
				}

				string key = parameter.Trim();
				string value = string.Empty;
					
				if (key.StartsWith(SWITCH_START) || key.StartsWith(SWITCH_START_ALT))
				{					
					int switchParameterLocation = parameter.IndexOf(SWITCH_PARAMETER_SEPARATOR);

					if (switchParameterLocation > 0)
					{
						// Parameter is of the form /I:MyParam or /I:"My Parameter" or -I:"My Parameter" or -MyParam:Setting
						value = key.Substring(switchParameterLocation + 1).Trim().Trim(new[] { '"' });
						key = key.Substring(1, switchParameterLocation - 1);
					}
					else if (i < parameters.Count - 1 && !parameters[i + 1].StartsWith(SWITCH_START) && !parameters[i + 1].StartsWith(SWITCH_START_ALT))
					{
						// Parameter is of the form /I MyParam or -I MyParam or -MyParam Setting

						string nextParameter = parameters[i + 1];
						key = key.Substring(1);
						value = nextParameter.Trim(new[] { '"' });
						i++;
					}
					else
					{
						// Parameter is of the form /S or -S
						key = key.Substring(1);
					}

					// Store the parameter in the case-sensitive dictionary
					if (!m_ParameterValueMap.ContainsKey(key))
						m_ParameterValueMap.Add(key, value);

					// Store the parameter in the case-insensitive dictionary
					if (!m_ParameterValueMapAnyCase.ContainsKey(key))
						m_ParameterValueMapAnyCase.Add(key, value);
				}
				else
				{
					m_nonSwitchParameters.Add(key.Trim(new[] { '"' }));
				}
			}

			if (m_ParameterValueMap.Count + m_nonSwitchParameters.Count > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Pauses the program for the specified number of milliseconds and displays a dot for each specified millisecond interval.
		/// </summary>
		/// <param name="millisecondsToPause">Total Number of milliseconds to pause</param>
		/// <param name="millisecondsBetweenDots">Number of milliseconds between each dot to be displayed</param>
		public void PauseAtConsole(int millisecondsToPause, int millisecondsBetweenDots)
		{
			Console.WriteLine("");
			Console.Write("Continuing in " + (millisecondsToPause / 1000.0).ToString("0") + " seconds ");

			if (millisecondsBetweenDots == 0 || millisecondsBetweenDots > millisecondsToPause)
			{
				millisecondsBetweenDots = millisecondsToPause;
			}

			var totalIterations = (int)Math.Round(millisecondsToPause / (double)millisecondsBetweenDots, 0);

			for (int i = 0; i < totalIterations; i++)
			{
				Console.Write(".");
				Thread.Sleep(millisecondsBetweenDots);
			}

			Console.WriteLine("");
		}

		/// <summary>
		/// Attempts to retrieve a non-switch parameter at the specified index
		/// </summary>
		/// <param name="parameterIndex">The index of the parameter</param>
		/// <returns>The parameter, or an empty string if the parameter does not exist</returns>
		public string RetrieveNonSwitchParameter(int parameterIndex)
		{
			if (parameterIndex >= m_nonSwitchParameters.Count)
			{
				return string.Empty;
			}

			return m_nonSwitchParameters[parameterIndex];
		}

		/// <summary>
		/// Attempts to retrieve the parameter at the specified index.
		/// </summary>
		/// <param name="parameterIndex">The index of the parameter</param>
		/// <param name="parameter">The returned parameter</param>
		/// <param name="value">The returned value</param>
		/// <returns>True if the parameter exists, False otherwise</returns>
		public bool RetrieveParameter(int parameterIndex, out string parameter, out string value)
		{
			if (parameterIndex >= m_ParameterValueMap.Count)
			{
				parameter = string.Empty;
				value = string.Empty;
				return false;
			}

			KeyValuePair<string, string> keyValuePair = m_ParameterValueMap.ElementAt(parameterIndex);

			parameter = keyValuePair.Key;
			value = keyValuePair.Value;
			return true;
		}

		/// <summary>
		/// Attempts to retrieve the value of a command line parameter.
		/// </summary>
		/// <param name="parameter">The parameter to get the value of</param>
		/// <param name="value">The returned value of the parameter (empty if no value exists for the parameter)</param>
		/// <param name="isCaseSensitive">True if parameter names are case-sensitive</param>
		/// <returns>True if the parameter exists, False otherwise</returns>
		public bool RetrieveValueForParameter(string parameter, out string value, bool isCaseSensitive=true)
		{
			if (isCaseSensitive)
				return m_ParameterValueMap.TryGetValue(parameter, out value);
			else
				return m_ParameterValueMapAnyCase.TryGetValue(parameter, out value);
		}
	}
}
