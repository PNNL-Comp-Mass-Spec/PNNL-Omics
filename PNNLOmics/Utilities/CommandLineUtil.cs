using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PNNLOmics.Utilities
{
    public class CommandLineUtil
    {
        private const string SWITCH_START = "/";
        private const string SWITCH_PARAMETER = ":";

        private bool m_showHelp;
        private readonly List<string> m_nonSwitchParameters;
        private readonly Dictionary<string, string> m_ParameterValueMap;

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
        /// <param name="caseSensitiveSwitchNames">Set to false so that /S and /s are treated the same</param>
        public CommandLineUtil(bool caseSensitiveSwitchNames = true)
        {
            m_showHelp = false;
            m_nonSwitchParameters = new List<string>();

            if (caseSensitiveSwitchNames)
                m_ParameterValueMap = new Dictionary<string, string>(StringComparer.CurrentCulture);
            else
                m_ParameterValueMap = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

        }

        /// <summary>
        /// Check whether a parameter was present on the command line
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <returns>True if present, otherwise false</returns>
        /// <remarks>Will return True for "L" for both /L and /L:LogFileName</remarks>
        public bool IsParameterPresent(string paramName)
        {
            return m_ParameterValueMap.ContainsKey(paramName);
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
            var commandLine = Environment.CommandLine;
            var parameters = Environment.GetCommandLineArgs();

            if (string.IsNullOrWhiteSpace(commandLine))
            {
                return false;
            }

            if (commandLine.Contains(SWITCH_START + "?") || commandLine.Contains(SWITCH_START + "help"))
            {
                m_showHelp = true;
                return false;
            }

            // Note that parameters[0] is the path to the executable for the calling program
            for (var i = 1; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                if (parameter.Length <= 0)
                {
                    continue;
                }

                var key = parameter.Trim();
                var value = string.Empty;
                var switchParameterExists = key.StartsWith(SWITCH_START) || key.StartsWith("-");

                if (switchParameterExists)
                {
                    var switchParameterLocation = parameter.IndexOf(SWITCH_PARAMETER, StringComparison.Ordinal);

                    if (switchParameterLocation > 0)
                    {
                        // Parameter is of the form /I:MyParam or /I:"My Parameter" or -I:"My Parameter" or /MyParam:Setting
                        value = key.Substring(switchParameterLocation + 1).Trim().Trim('"');
                        key = key.Substring(1, switchParameterLocation - 1);
                    }
                    else if (i < parameters.Length - 1 && !parameters[i + 1].StartsWith(SWITCH_START) && !parameters[i + 1].StartsWith("-"))
                    {
                        // Parameter is of the form /I MyParam or -I MyParam
                        var nextParameter = parameters[i + 1];
                        key = key.Substring(1);
                        value = nextParameter.Trim('"');
                        i++;
                    }
                    else
                    {
                        // Parameter is of the form /S or -S
                        key = key.Substring(1);
                    }

                    if (m_ParameterValueMap.ContainsKey(key))
                    {
                        Console.WriteLine("Ignoring duplicate switch, " + parameter);
                    }
                    else
                    {
                        m_ParameterValueMap.Add(key, value);
                    }
                }
                else
                {
                    if (m_ParameterValueMap.ContainsKey(key))
                    {
                        Console.WriteLine("Ignoring duplicate switch, " + parameter);
                    }
                    else
                    {
                        m_nonSwitchParameters.Add(key.Trim('"'));
                    }

                }
            }

            return m_ParameterValueMap.Count + m_nonSwitchParameters.Count > 0;
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

            for (var i = 0; i < totalIterations; i++)
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

            var keyValuePair = m_ParameterValueMap.ElementAt(parameterIndex);

            parameter = keyValuePair.Key;
            value = keyValuePair.Value;
            return true;
        }

        /// <summary>
        /// Attempts to retrieve the value of a command line parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the value of</param>
        /// <param name="value">The returned value of the parameter</param>
        /// <param name="valueIfMissing">Value to return if the parameter is missing (default is an empty string)</param>
        /// <returns>True if the parameter exists, False otherwise</returns>
        public bool RetrieveValueForParameter(string parameter, out string value, string valueIfMissing = "")
        {
            if (m_ParameterValueMap.TryGetValue(parameter, out value))
                return true;

            value = valueIfMissing;
            return false;
        }
    }
}
