using System;
using System.Collections.Generic;
using System.Linq;
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
			string[] parameters = Environment.GetCommandLineArgs();

			if (commandLine == null || commandLine.Equals(string.Empty))
			{
				return false;
			}

			List<string> helpSwitches = new List<string>();
			helpSwitches.Add(SWITCH_START + "?");
			helpSwitches.Add(SWITCH_START + "help");
			helpSwitches.Add(SWITCH_START_ALT + "?");
			helpSwitches.Add(SWITCH_START_ALT + "help");

			foreach (string item in helpSwitches) 
			{
				if (commandLine.ToLower().Contains(item.ToLower()))
				{
					m_showHelp = true;
					return false;
				}
			}

			// Note that parameters[0] is the path to the executable for the calling program
			for (int i = 1; i < parameters.Length; i++)
			{
				string parameter = parameters[i];

				if (parameter.Length > 0)
				{
					string key = parameter.Trim();
					string value = string.Empty;
					
					if (key.StartsWith(SWITCH_START) || key.StartsWith(SWITCH_START_ALT))
					{					
						int switchParameterLocation = parameter.IndexOf(SWITCH_PARAMETER_SEPARATOR);

						if (switchParameterLocation > 0)
						{
							// Parameter is of the form /I:MyParam or /I:"My Parameter" or -I:"My Parameter" or -MyParam:Setting
							value = key.Substring(switchParameterLocation + 1).Trim().Trim(new char[] { '"' });
							key = key.Substring(1, switchParameterLocation - 1);
						}
						else if (i < parameters.Length - 1 && !parameters[i + 1].StartsWith(SWITCH_START) && !parameters[i + 1].StartsWith(SWITCH_START_ALT))
						{
							// Parameter is of the form /I MyParam or -I MyParam or -MyParam Setting

							string nextParameter = parameters[i + 1];
							key = key.Substring(1);
							value = nextParameter.Trim(new char[] { '"' });
							i++;
						}
						else
						{
							// Parameter is of the form /S or -S
							key = key.Substring(1);
						}

						// Store the parameter in the case-sensitive dictionary
						m_ParameterValueMap.Add(key, value);

						// Store the parameter in the case-insensitive dictionary
						if (!m_ParameterValueMapAnyCase.ContainsKey(key))
							m_ParameterValueMapAnyCase.Add(key, value);
					}
					else
					{
						m_nonSwitchParameters.Add(key.Trim(new char[] { '"' }));
					}
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

			int totalIterations = (int)Math.Round((double)millisecondsToPause / (double)millisecondsBetweenDots, 0);

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
