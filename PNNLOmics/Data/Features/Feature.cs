using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data.Features
{
    public abstract class Feature: BaseData
    {
        private const int CONST_DEFAULT_SCAN_VALUE = -1;

        #region Properties
        /// <summary>
        /// Gets or sets the ID of the group the feature belongs to.  Where a group could be a dataset or factor.
        /// </summary>
        public int GroupID { get; set; }
        /// <summary>
        /// Gets or sets the ID for a feature.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Gets or sets the drift time of the feature.
        /// </summary>
        public float DriftTime {get;set;}
        /// <summary>
        /// Gets or sets the monoisotopic mass of the feature.
        /// </summary>
        public double MassMonoisotopic { get; set; }
        /// <summary>
        /// Gets or sets the monoisotopic mass (aligned) of the feature.
        /// </summary>
        public double MassMonoisotopicAligned { get; set; }     
        /// <summary>
        /// Gets or sets the normalized elution time (NET) of the feature.
        /// </summary>
        public double NET { get; set; }
        /// <summary>
        /// Gets or sets the aligned NET of the feature.
        /// </summary>
        public double NETAligned { get; set; }        
        /// <summary>
        /// Gets or sets the scan of the feature from the raw data.
        /// </summary>
        public int Scan { get; set; }
        /// <summary>
        /// Gets or sets the aligned scan of the feature.
        /// </summary>
        public int ScanAligned { get; set; }   
        /// <summary>
        /// Gets or sets the abundance of the feature.
        /// </summary>
        public int Abundance{ get; set; }
        /// <summary>
        /// Gets or sets the M/Z value of the feature.
        /// </summary>
        public double MZ { get; set; }
        /// <summary>
        /// Gets or sets the charge state of the feature.
        /// </summary>
        public int ChargeState{get;set;}  
        #endregion

        #region BaseData Members
        /// <summary>
        /// Clears the datatype and resets the raw values to their default values.
        /// </summary>
        public override void  Clear()
        {
            Abundance                   = 0;
            ChargeState                 = 0;
            DriftTime                   = 0;
            ID                          = -1;
            MassMonoisotopic            = double.NaN;
            MassMonoisotopicAligned     = double.NaN;
            MZ                          = double.NaN;
            NET                         = double.NaN;
            NETAligned                  = double.NaN;
            Scan                        = CONST_DEFAULT_SCAN_VALUE;
            ScanAligned                 = CONST_DEFAULT_SCAN_VALUE;           
        }
        #endregion

        #region Comparison Methods
        /// <summary>
        /// Compares the monoisotopic mass of two Features
        /// </summary>
        public static Comparison<Feature> MassComparison = delegate(Feature x, Feature y)
        {
            return x.MassMonoisotopic.CompareTo(y.MassMonoisotopic);
        };
        /// <summary>
        /// Compares the aligned monoisotopic mass of two Features
        /// </summary>
        public static Comparison<Feature> MassAlignedComparison = delegate(Feature x, Feature y)
        {
            return x.MassMonoisotopicAligned.CompareTo(y.MassMonoisotopicAligned);
        };
        /// <summary>
        /// Compares the scan of two Features
        /// </summary>
        public static Comparison<Feature> ScanComparison = delegate(Feature x, Feature y)
        {
            return x.Scan.CompareTo(y.Scan);
        };
        /// <summary>
        /// Compares the aligned scan of two Features
        /// </summary>
        public static Comparison<Feature> ScanAlignedComparison = delegate(Feature x, Feature y)
        {
            return x.ScanAligned.CompareTo(y.ScanAligned);
        };
        /// <summary>
        /// Compares the NET of two Features
        /// </summary>
        public static Comparison<Feature> NETComparison = delegate(Feature x, Feature y)
        {
            return x.NET.CompareTo(y.NET);
        };
        /// <summary>
        /// Compares the NET aligned of two Features
        /// </summary>
        public static Comparison<Feature> NETAlignedComparison = delegate(Feature x, Feature y)
        {
            return x.NETAligned.CompareTo(y.NETAligned);
        };
        /// <summary>
        /// Compares the mass to charge ratio (m/z) of two Features
        /// </summary>
        public static Comparison<Feature> MZComparison = delegate(Feature x, Feature y)
        {
            return x.MZ.CompareTo(y.MZ);
        };
        /// <summary>
        /// Compares the ID values of two Features
        /// </summary>
        public static Comparison<Feature> IDComparison = delegate(Feature x, Feature y)
        {
            return x.ID.CompareTo(y.ID);
        };
        /// <summary>
        /// Compares the drift time of two Features
        /// </summary>
        public static Comparison<Feature> DriftTimeComparison = delegate(Feature x, Feature y)
        {
            return x.DriftTime.CompareTo(y.DriftTime);
        };
        /// <summary>
        /// Compares the charge state of two Features
        /// </summary>
        public static Comparison<Feature> ChargeStateComparison = delegate(Feature x, Feature y)
        {
            return x.ChargeState.CompareTo(y.ChargeState);
        };
        /// <summary>
        /// Compares the abundance of two Features
        /// </summary>
        public static Comparison<Feature> AbundanceComparison = delegate(Feature x, Feature y)
        {
            return x.Abundance.CompareTo(y.Abundance);
        };
        #endregion

        /// <summary>
        /// Computes the mass difference in parts per million (ppm) for two given masses.
        /// </summary>
        /// <param name="massX">Mass of feature X.</param>
        /// <param name="massY">Mass of feature Y.</param>
        /// <returns>Mass difference in parts per million (ppm).</returns>
        public static double ComputeMassPPMDifference(double massX, double massY)
        {
            return (massX - massY) * 1000000.0/ massX;
        }
    }
}
