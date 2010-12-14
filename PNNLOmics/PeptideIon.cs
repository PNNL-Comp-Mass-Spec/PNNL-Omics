using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    public class Ion: Peak
    {        
        public IonType IonType
        {
            get;
            set;
        }
        public byte Charge
        { 
            get; 
            set; 
        }
        public Molecule Molecule
        {
            get; 
            set; 
        }
    }
}
