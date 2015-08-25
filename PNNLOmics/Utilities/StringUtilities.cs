using System;
using System.Collections.Concurrent;
using System.Globalization;

namespace PNNLOmics.Utilities
{
    public class StringUtilities
    {
        /// <summary>
        /// Dictionary that tracks the format string used for each digitsOfPrecision value
        /// </summary>
        private static readonly ConcurrentDictionary<byte, string> mFormatStrings = new ConcurrentDictionary<byte, string>();

        /// <summary>
        /// Dictionary that tracks the format string used for each digitsOfPrecision value displayed with scientific notation
        /// </summary>
        /// <remarks>
        /// Keys are the number of digits of precision and
        /// "false" if the format string is of the form 0.00E+00 or 
        /// "true"  if the format string is of the form 0.00E+000
        /// </remarks>
        private static readonly ConcurrentDictionary<string, string> mFormatStringsScientific = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Format the value to a string with a fixed number of decimal points
        /// </summary>
        /// <param name="value">Value to format</param>
        /// <param name="digitsAfterDecimal">Digits to show after the decimal place (0 or higher)</param>
        /// <param name="thresholdScientific">Numbers below this level will be displayed using Scientific notation</param>
        /// <returns>String representation of the value</returns>
        /// <remarks>If digitsOfPrecision is 0, will round the number to the nearest integer</remarks>
        public static string DblToString(
            double value,
            byte digitsAfterDecimal,
            double thresholdScientific)
        {
            return DblToString(value, digitsAfterDecimal, limitDecimalsForLargeValues: false, thresholdScientific: thresholdScientific, invariantCulture: true);
        }

        /// <summary>
        /// Format the value to a string with a fixed number of decimal points
        /// </summary>
        /// <param name="value">Value to format</param>
        /// <param name="digitsAfterDecimal">Digits to show after the decimal place (0 or higher)</param>
        /// <param name="limitDecimalsForLargeValues">When true, will limit the number of decimal points shown for values over 1</param>
        /// <param name="thresholdScientific">Numbers below this level will be displayed using Scientific notation</param>
        /// <param name="invariantCulture">When true (default) numbers will always use a period for the decimal point</param>
        /// <returns>String representation of the value</returns>
        /// <remarks>If digitsOfPrecision is 0, will round the number to the nearest integer</remarks>
        public static string DblToString(
            double value,
            byte digitsAfterDecimal,
            bool limitDecimalsForLargeValues = false,
            double thresholdScientific = 0.001,
            bool invariantCulture = true)
        {

            if (Math.Abs(value) < double.Epsilon)
                return "0";

            if (Math.Abs(value) >= 10 && Math.Abs(value - (int)value) < 1 / (Math.Pow(10, digitsAfterDecimal)) / 2.0)
            {
                // Value 10 or larger and it is nearly an integer value (at least with respect to digitsOfPrecision)
                // Return values like 10 or 150 instead of 10.000 or 150.000
                return value.ToString("0");
            }

            if (Math.Abs(value) < thresholdScientific)
            {
                return DblToStringScientific(value, digitsAfterDecimal, invariantCulture);
            }

            var effectiveDigitsAfterDecimal = digitsAfterDecimal;

            if (Math.Abs(value) > 1 && limitDecimalsForLargeValues)
            {
                //  digitsLeftOfDecimal  = Math.Floor(Math.Log10(value)) + 1
                //  digitsRightOfDecimal = digitsOfPrecision - (      digitsLeftOfDecimal         - 1)
                //  digitsRightOfDecimal = digitsOfPrecision - (Math.Floor(Math.Log10(value)) + 1 - 1)

                var digitsRightOfDecimal = digitsAfterDecimal - (byte)(Math.Floor(Math.Log10(value)));

                if (digitsRightOfDecimal >= 0)
                    effectiveDigitsAfterDecimal = (byte)digitsRightOfDecimal;
                else
                    effectiveDigitsAfterDecimal = 0;

            }

            if (effectiveDigitsAfterDecimal <= 0)
                return value.ToString("0");

            string formatString;
            if (mFormatStrings.TryGetValue(effectiveDigitsAfterDecimal, out formatString))
            {
                return value.ToString(formatString, invariantCulture ? NumberFormatInfo.InvariantInfo : NumberFormatInfo.CurrentInfo);
            }

            formatString = "0.0";

            if (effectiveDigitsAfterDecimal > 1)
            {
                // Update format string to be of the form "0.0#######"
                formatString += new string('#', effectiveDigitsAfterDecimal - 1);
            }

            try
            {
                mFormatStrings.TryAdd(effectiveDigitsAfterDecimal, formatString);
            }
            catch
            {
                // Ignore errors here
            }

            return value.ToString(formatString, invariantCulture ? NumberFormatInfo.InvariantInfo : NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Format the value to a string using scientific notation
        /// </summary>
        /// <param name="value">Value to format</param>
        /// <param name="digitsAfterDecimal">Digits to show after the decimal place (0 or higher)</param>
        /// <param name="invariantCulture">When true (default) numbers will always use a period for the decimal point</param>
        /// <returns>String representation of the value</returns>
        /// <remarks>If digitsOfPrecision is 0, will round the number to the nearest integer</remarks>
        public static string DblToStringScientific(
            double value,
            byte digitsAfterDecimal,
            bool invariantCulture = true)
        {
            if (Math.Abs(value) < double.Epsilon)
                return "0";

            if (digitsAfterDecimal <= 0)
                return value.ToString("0");

            var tinyNumber = Math.Log10(Math.Abs(value)) <= -99;

            string formatString;
            if (mFormatStringsScientific.TryGetValue(digitsAfterDecimal + tinyNumber.ToString(), out formatString))
            {
                return value.ToString(formatString, invariantCulture ? NumberFormatInfo.InvariantInfo : NumberFormatInfo.CurrentInfo);
            }

            formatString = "0.0";

            if (digitsAfterDecimal > 1)
            {
                // Update format string to be of the form "0.0#######"
                formatString += new string('#', digitsAfterDecimal - 1);
            }

            if (tinyNumber)
            {
                formatString += "E+000";
            }
            else
            {
                formatString += "E+00";
            }

            try
            {
                mFormatStringsScientific.TryAdd(digitsAfterDecimal + tinyNumber.ToString(), formatString);
            }
            catch
            {
                // Ignore errors here
            }

            return value.ToString(formatString, invariantCulture ? NumberFormatInfo.InvariantInfo : NumberFormatInfo.CurrentInfo);
        }

    }
}
