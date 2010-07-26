using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Main Call
//List<char> outCharIndex;
//List<double> outDoubleMasses;
//MonosaccharideUtilities.MonoSaccharideMassList(out outStringIndex, out outDoubleMasses);

namespace Constants
{
    public class MonosaccharideUtilities
    {
        public static void MonoSaccharideMassList(out List<string> indexList, out List<double> newList)
        {
            int listLength = 8;

            indexList = new List<string>();
            newList = new List<double>();

            indexList.Add("DxyHex");
            indexList.Add("Hex");
            indexList.Add("HexA");
            indexList.Add("KDN");
            indexList.Add("HexNAc");
            indexList.Add("NeuAc");
            indexList.Add("NeuGc");
            indexList.Add("Pent");
            
            //newlist.Add(AminoAcidConstantsTable.GetMass(indexList[2]));

            for (int i = 0; i < listLength; i++)
            {
                newList.Add(MonosaccharideConstantsTable.GetMass(indexList[i]));
            }
        }
        //public Dictionary<AminoAcid, string> AminoAcidLookupTable { get; set; }
    }
}