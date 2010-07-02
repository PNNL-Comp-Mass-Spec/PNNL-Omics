using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Utilities.Importers
{
    /// <summary>
    /// Base importer class for importing text data
    /// </summary>
    /// <typeparam name="T">the import data type... i.e UMC or MassTag </typeparam>
    public abstract class ImporterBase<T>
    {
        //TODO: Gord - finish cleaning up this before harsh code review


        public abstract void Import(T data);

        protected char delimiter = '\t';

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">Single row of data </param>
        /// <param name="columnHeaders">List of column headers</param>
        /// <param name="targetColumn"></param>
        /// <returns></returns>
        protected string lookupData(List<string> row, List<string> columnHeaders, string targetColumn)
        {
            int columnIndex = getIndexForHeader(columnHeaders, targetColumn, false);
            if (columnIndex == -1) return "null";
            return row[columnIndex];
        }

        protected bool parseBoolField(string inputstring)
        {
            bool result = false;
            if (bool.TryParse(inputstring, out result))
                return result;
            else return false;     
        }

        protected short parseShortField(string inputstring)
        {
            short result = 0;
            if (Int16.TryParse(inputstring, out result))
                return result;
            else return 0;
        }

        protected double parseDoubleField(string inputstring)
        {
            double result = 0;
            if (double.TryParse(inputstring, out result))
                return result;
            else
            {
                return double.NaN;
            }
        }

        protected float parseFloatField(string inputstring)
        {
            float result = 0;
            if (float.TryParse(inputstring, out result))
                return result;
            else return float.NaN;

        }


        protected int parseIntField(string inputstring)
        {
            int result = 0;
            if (Int32.TryParse(inputstring, out result))
                return result;
            else
            {
                double secondAttempt = parseDoubleField(inputstring);
                if (secondAttempt != double.NaN)
                {
                    return Convert.ToInt32(secondAttempt);
                }
                else
                {
                    return -1;
                }
            }
        }

        protected List<string> processLine(string inputLine)
        {
            char[] splitter = { delimiter };
            List<string> parsedLine = new List<string>();

            string[] arr = inputLine.Split(splitter);
            foreach (string str in arr)
            {
                parsedLine.Add(str);
            }
            return parsedLine;
        }
        protected int getIndexForHeader(List<string> tableHeaders, string target, bool ignoreCase)
        {
            for (int i = 0; i < tableHeaders.Count; i++)
            {
                string columnHeader;

                if (ignoreCase)
                {
                    columnHeader = tableHeaders[i].ToLower();
                    target = target.ToLower();
                }
                else
                {
                    columnHeader = tableHeaders[i];
                }


                if (columnHeader == target)
                {
                    return i;
                }
            }
            return -1;     //didn't find header!
        }


    }
}
