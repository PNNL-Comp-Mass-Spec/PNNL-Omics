using System;
using System.Collections.Generic;
using PNNLOmics.Data.Constants;

namespace PNNLOmics.Data.Constants.Utilities
{
	/// <summary>
	/// This abstract class is used for accessing the mapping dictionaries that connect the enums and the data
	/// </summary>
	/// <typeparam name="T">Matter Type Type</typeparam>
	/// <typeparam name="U">Enumeration Type</typeparam>
	public abstract class MatterLibrary<T, U>
	where T : Matter
	where U : struct
	
    {
		/// <summary>
		/// Maps the symbols to Matter objects.  
		/// </summary>
		protected Dictionary<string, T> m_symbolToCompoundMap;

		/// <summary>
		/// Maps the Matter enumerations to the symbols 
		/// </summary>
		protected Dictionary<U, string> m_enumToSymbolMap;

		/// <summary>
		/// This generic converts the string key into an Matter type T via the symbolToCompoundMap
		/// </summary>
		/// <param name="key">string key</param>
		/// <returns>Matter object</returns>
		public T this[string key]
		{
			get
			{
				return m_symbolToCompoundMap[key];
			}
		}

		/// <summary>
        /// This generic converts the generic enum key into an Matter type T via the symbolToCompoundMap
		/// </summary>
		/// <param name="key">Matter enumation</param>
		/// <returns>Matter object</returns>
		public T this[U key]
		{
			get
			{
				return m_symbolToCompoundMap[m_enumToSymbolMap[key]];
			}
		}

		/// <summary>
		/// This abstract generic Dictionary Loads the data from a given Matter type T 
		/// </summary>
		/// <returns>Matter library</returns>
        public abstract Dictionary<string, T> LoadLibrary();
	}
}
