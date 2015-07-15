using System;
using System.Collections.Concurrent;

namespace PNNLOmics.Utilities
{
    public class StringUtilities
    {
        /// <summary>
        /// Dictionary that tracks the format string used for each digitsOfPrecision value
        /// </summary>
        private static readonly ConcurrentDictionary<byte, string> mFormatStrings = new ConcurrentDictionary<byte, string>();

        /// <summary>
        /// Format the value to a string with a fixed number of decimal points
        /// </summary>
        /// <param name="value">Value to format</param>
        /// <param name="digitsOfPrecision">Digits to show after the decimal place (0 or higher)</param>
        /// <returns>String representation of the value</returns>
        /// <remarks>If digitsOfPrecision is 0, will round the number to the nearest integer</remarks>
        public static string DblToString(double value, byte digitsOfPrecision)
        {
            if (Math.Abs(value) < float.Epsilon)
                return "0";

            if (digitsOfPrecision <= 0)
                return value.ToString("0");

            if (value >= 10 && Math.Abs(value - (int)value) < 1 / (Math.Pow(10, digitsOfPrecision)) / 2.0)
            {
                // Value 10 or larger and it is nearly an integer value (at least with respect to digitsOfPrecision)
                // Return values like 10 or 150 instead of 10.000 or 150.000
                return value.ToString("0");
            }

            string formatString;
            if (mFormatStrings.TryGetValue(digitsOfPrecision, out formatString))
            {
                return value.ToString(formatString);
            }

            formatString = "0.0";

            if (digitsOfPrecision > 1)
            {
                // Update format string to be of the form "0.0#######"
                formatString += new string('#', digitsOfPrecision - 1);
            }

            try
            {
                mFormatStrings.TryAdd(digitsOfPrecision, formatString);
            }
            catch
            {
                // Ignore errors here
            }
            
            return value.ToString(formatString);
        }

    }
}
