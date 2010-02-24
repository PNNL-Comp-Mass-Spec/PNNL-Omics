using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public abstract class Task<T, U, V>
    {

        public abstract V Execute(T data, U options);        
    }
}
