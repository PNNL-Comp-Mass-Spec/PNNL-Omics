using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Main Call
//List<char> outCharIndex;
//List<double> outDoubleMasses;
//AminoAcidUtility.AminoAcidMassList(out outCharIndex, out outDoubleMasses);

namespace Constants
{
    public class AminoAcidUtility
    {
        public static void AminoAcidMassList(out List<char> indexList, out List<double> newList)
        {
            int listLength=20;

            indexList = new List<char>();
            newList = new List<double>();

            indexList.Add('A');
            indexList.Add('R');
            indexList.Add('N');
            indexList.Add('D');
            indexList.Add('C');
            indexList.Add('E');
            indexList.Add('Q');
            indexList.Add('G');
            indexList.Add('H');
            indexList.Add('I');
            indexList.Add('L');
            indexList.Add('K');
            indexList.Add('M');
            indexList.Add('F');
            indexList.Add('P');
            indexList.Add('S');
            indexList.Add('T');
            indexList.Add('W');
            indexList.Add('Y');
            indexList.Add('V');

            //newlist.Add(AminoAcidConstantsTable.GetMass(indexList[2]));

            for (int i = 0; i < listLength; i++)
            {
                newList.Add(AminoAcidConstantsTable.GetMass(indexList[i]));
            }
        }
        //public Dictionary<AminoAcid, string> AminoAcidLookupTable { get; set; }
    }
}
