using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data
{
    public abstract class Feature: BaseData
    {
        #region Properties
        public int DriftTime {get;set;}
        public int MassMonoisotopic{get;set;}       
        public int NET  {get;set;}
        public int Scan { get; set; }        
        public int Abundance{ get; set; }
        public int MZ { get; set; }
        public int ChargeState{get;set;}        
        public int NETAligned{get;set;}        
        public int MassMonoisotopicAligned{get;set;}
        public int ScanAligned {get;set; }
        #endregion

        #region BaseData Members
        public int ID{get;set;}       
        public override void  Clear()
        {
 	        throw new NotImplementedException();
        }
        #endregion
    }
}
