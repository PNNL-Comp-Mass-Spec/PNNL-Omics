using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    public class Ion: MSPeak
    {
        private IonType m_ionType;
        private byte m_charge;
        private Molecule m_molecule;

        public IonType IonType
        {
            get { return m_ionType; }
            set { m_ionType = value; }
        }
    }
}
