using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    [Serializable]
    public abstract class BaseData
    {
        int ID { get; set; }
        public abstract void Clear();
    }
}
