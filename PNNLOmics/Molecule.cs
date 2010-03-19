using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    public class Molecule: BaseData
    {
        private string m_description;

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }
        private MassTag m_massTag;

        public MassTag MassTag
        {
            get { return m_massTag; }
            set { m_massTag = value; }
        }
        private string m_name;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        private int m_chemicalFormula;

        public int ChemicalFormula
        {
            get { return m_chemicalFormula; }
            set { m_chemicalFormula = value; }
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
