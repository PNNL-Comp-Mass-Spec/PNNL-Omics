using System;
using System.Collections.Generic;

namespace PNNLOmicsIO.Utilities.Importers
{
    /// <summary>
    /// Base importer class for importing text data
    /// </summary>
    /// <typeparam name="T">the import data type... i.e UMC or MassTag </typeparam>
    public abstract class ImporterBase<T>
    {
        protected char m_delimiter = '\t';
        protected List<string> m_columnHeaders = new List<string>();

        //TODO: Gord - finish cleaning up this before harsh code review

        /// <summary>
        /// Name of this class. 
        /// </summary>
        public virtual string Name
        {
            get { return ToString(); }
            set { ;}
        }

        #region Public Methods

        public abstract T Import();
        
        #endregion

        #region Protected Methods


        /// <summary>
        /// This method retrieves a single cell of data (row, column) in the form of a string.  
        /// </summary>
        /// <param name="row">Single row of data </param>
        /// <param name="targetColumn">Column header name</param>
        /// <returns></returns>
        protected string LookupData(List<string> row, string targetColumn)
        {
            return LookupData(row, targetColumn, true);
        }

        protected string LookupData(List<string> row, string targetColumn, bool ignoreCase)
        {
            var columnIndex = GetColumnIndexForHeader(targetColumn, ignoreCase);
            if (columnIndex == -1) return "null";
            return row[columnIndex];
        }

        protected bool ParseBoolField(string inputstring)
        {
            var result = false;
            if (bool.TryParse(inputstring, out result))
                return result;
            return false;
        }

        protected short ParseShortField(string inputstring)
        {
            short result = 0;
            if (Int16.TryParse(inputstring, out result))
                return result;
            return 0;
        }

        protected double ParseDoubleField(string inputstring)
        {
            double result = 0;
            if (double.TryParse(inputstring, out result))
                return result;
            return double.NaN;
        }

        protected float ParseFloatField(string inputstring)
        {
            float result = 0;
            if (float.TryParse(inputstring, out result))
                return result;
            return float.NaN;
        }


        protected int ParseIntField(string inputstring)
        {
            var result = 0;
            if (Int32.TryParse(inputstring, out result))
                return result;
            var secondAttempt = ParseDoubleField(inputstring);
            if (secondAttempt != double.NaN)
            {
                return Convert.ToInt32(secondAttempt);
            }
            return -1;
        }

        /// <summary>
        /// Parses a single line of data into a List of strings
        /// </summary>
        /// <param name="inputLine"></param>
        /// <returns></returns>
        protected List<string> ProcessLine(string inputLine)
        {
            char[] splitter = { m_delimiter };
            var parsedLine = new List<string>();

            var arr = inputLine.Split(splitter);
            foreach (var str in arr)
            {
                parsedLine.Add(str);
            }
            return parsedLine;
        }

        protected int GetColumnIndexForHeader(string target, bool ignoreCase)
        {
            for (var i = 0; i < m_columnHeaders.Count; i++)
            {
                string columnHeader;

                if (ignoreCase)
                {
                    columnHeader = m_columnHeaders[i].ToLower();
                    target = target.ToLower();
                }
                else
                {
                    columnHeader = m_columnHeaders[i];
                }


                if (columnHeader == target)
                {
                    return i;
                }
            }
            return -1;     //didn't find header!
        }
        
        #endregion

    }
}
