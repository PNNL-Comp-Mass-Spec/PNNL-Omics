using System;

namespace PNNLOmics.Utilities
{
    public class StringUtilities
    {
        private static byte mFormatStringPrecision = 1;
        private static string mFormatString = "0.0";

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

            if (digitsOfPrecision == mFormatStringPrecision)
            {
                return value.ToString(mFormatString);
            }

            mFormatString = "0.0";

            if (digitsOfPrecision > 1)
            {
                // Update format string to be of the form "0.0#######"
                mFormatString += new string('#', digitsOfPrecision - 1);
            }

            mFormatStringPrecision = digitsOfPrecision;

            return value.ToString(mFormatString);
        }

    }
}
