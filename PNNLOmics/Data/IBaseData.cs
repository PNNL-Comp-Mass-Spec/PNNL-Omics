using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics
{
    [Serializable]
    public interface IBaseData<T>: IComparer<T>, IDisposable
    {
        void Clear();
    }
}
