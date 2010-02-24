using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics
{
    /// <summary>
    /// Processes data of type T, U, V
    /// </summary>
    /// <typeparam name="T">Data Type</typeparam>
    /// <typeparam name="U">Return Type</typeparam>
    /// <typeparam name="V">Options Type</typeparam>
    public abstract class Processor<T, U, V>
    {
        public abstract V Execute(T data, U options);
    }
}
