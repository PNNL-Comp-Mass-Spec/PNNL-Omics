using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//main call
//string name = ElementsConstantTable.GetName("Na");
//double mass = ElementsConstantTable.GetExactMass("Na");

namespace Constants
{
    interface IElementConstants
       {
           void MassTable(Dictionary<string, double> DataDictionary);
           void Name(Dictionary<string, string> DataDictionary);
       }
    public class ElementsConstantTable : IElementConstants 
    {
        public static double GetExactMass(string IDletter)
        {
            Dictionary<string, double> MassDictionary =new Dictionary<string,double>();
            IElementConstants newTable = new ElementsConstantTable();
            newTable.MassTable(MassDictionary);
            return MassDictionary[IDletter];
        }

        public static string GetName(string IDletter)
        {
            Dictionary<string, string> NameDictionary = new Dictionary<string, string>();
            IElementConstants newTable = new ElementsConstantTable();
            newTable.Name(NameDictionary);
            return NameDictionary[IDletter];
        }

        #region interface functions for MassTable and NameTable

        void IElementConstants.MassTable(Dictionary<string, double> DataDictionary)
        {
            Carbon C = new Carbon();
            Hydrogen H = new Hydrogen();
            Nitrogen N = new Nitrogen();
            Oxygen O = new Oxygen();
            Phosphorus P = new Phosphorus();
            Potassium K = new Potassium();
            Sodium Na = new Sodium();
            Sulfur S = new Sulfur();
            
            DataDictionary.Add("C", C.MonoIsotopicMass);
            DataDictionary.Add("H", H.MonoIsotopicMass);
            DataDictionary.Add("N", N.MonoIsotopicMass);
            DataDictionary.Add("O", O.MonoIsotopicMass);
            DataDictionary.Add("P", P.MonoIsotopicMass);
            DataDictionary.Add("K", K.MonoIsotopicMass);
            DataDictionary.Add("Na", Na.MonoIsotopicMass);
            DataDictionary.Add("S", S.MonoIsotopicMass);
        }

        void IElementConstants.Name(Dictionary<string, string> DataDictionary)
        {
            Carbon C = new Carbon();
            Hydrogen H = new Hydrogen();
            Nitrogen N = new Nitrogen();
            Oxygen O = new Oxygen();
            Phosphorus P = new Phosphorus();
            Potassium K = new Potassium();
            Sodium Na = new Sodium();
            Sulfur S = new Sulfur();

            DataDictionary.Add("C", C.Name);
            DataDictionary.Add("H", H.Name);
            DataDictionary.Add("N", N.Name);
            DataDictionary.Add("O", O.Name);
            DataDictionary.Add("P", P.Name);
            DataDictionary.Add("K", K.Name);
            DataDictionary.Add("Na", Na.Name);
            DataDictionary.Add("S", S.Name);
        }
        #endregion
    }
}
