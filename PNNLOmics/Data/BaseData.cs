using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PNNLOmics.Data
{
    [Serializable]
    public abstract class BaseData
    {
        public int ID { get; set; }

        public abstract void Clear();        
    }
}
