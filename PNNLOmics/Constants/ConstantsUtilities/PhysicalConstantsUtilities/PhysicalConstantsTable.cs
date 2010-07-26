using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//main call
//string name = PhysicalConstantsTable.GetName('e');
//double mass = PhysicalConstantsTable.GetExactMass('e');

namespace Constants
{
    interface IPhysicalConstants
       {
           void MassTable(Dictionary<char, double> DataDictionary);
           void Name(Dictionary<char, string> DataDictionary);
       }
    public class PhysicalConstantsTable : IPhysicalConstants 
    {
        public static double GetExactMass(char IDletter)
        {
            Dictionary<char, double> MassDictionary =new Dictionary<char,double>();
            IPhysicalConstants newTable = new PhysicalConstantsTable();
            newTable.MassTable(MassDictionary);
            return MassDictionary[IDletter];
        }

        public static string GetName(char IDletter)
        {
            Dictionary<char, string> NameDictionary = new Dictionary<char, string>();
            IPhysicalConstants newTable = new PhysicalConstantsTable();
            newTable.Name(NameDictionary);
            return NameDictionary[IDletter];
        }

        #region interface functions for MassTable and NameTable

        void IPhysicalConstants.MassTable(Dictionary<char, double> DataDictionary)
        {
            Electron e = new Electron();
            Neutron n = new Neutron();
            Proton p = new Proton();
            
            DataDictionary.Add('e', e.ExactMass);
            DataDictionary.Add('n', n.ExactMass);
            DataDictionary.Add('p', p.ExactMass);
        }

        void IPhysicalConstants.Name(Dictionary<char, string> DataDictionary)
        {
            Electron e = new Electron();
            Neutron n = new Neutron();
            Proton p = new Proton();

            DataDictionary.Add('e', e.Name);
            DataDictionary.Add('n', n.Name);
            DataDictionary.Add('p', p.Name);
        }
        #endregion
    }
}
