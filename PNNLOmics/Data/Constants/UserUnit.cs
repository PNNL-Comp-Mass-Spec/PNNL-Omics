using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data.Constants
{
    /// <summary>
    /// Encapsulates...
    /// </summary>
    //TODO: SCOTT - CR - Fill in XML comment.
    public class UserUnit : Matter
    {
        /// <summary>
        /// constructor for a simple userUnit that has the required name and symbol
        /// </summary>
        /// <param name="name"></param>
        /// <param name="symbol"></param>
        public UserUnit(string name, string symbol)
        {
            this.Name = name;
            this.Symbol = symbol;
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public UserUnit()
        {
        }
            
        /// <summary>
        /// Gets or sets the type of UserUnit.
        /// </summary>
        public UserUnitName UserUnitType
        {
            get;
            set;
        }
    }
}
