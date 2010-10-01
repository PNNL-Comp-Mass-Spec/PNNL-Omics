using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// Abstract base class that represents the most basic properties of any Feature.
	/// </summary>
	public abstract class Feature : BaseData, IComparable<Feature>
    {
		/// <summary>
		/// Default value for any scan.
		/// </summary>
        protected const int CONST_DEFAULT_SCAN_VALUE = -1;
		
		public Feature()
		{
			Clear();
		}

        #region AutoProperties
        /// <summary>
        /// The ID for a feature.
        /// </summary>
        public int ID { get; set; }
		/// <summary>
		/// True if the MSFeature was marked as Suspicious by Deconvolution algorithm.
		/// </summary>
		public bool IsSuspicious { get; set; }
		/// <summary>
		/// True if the MSFeature was marked corrected by the Da Correction algorithm.
		/// </summary>
		public bool IsDaltonCorrected { get; set; }
        /// <summary>
        /// The drift time of the feature.
        /// </summary>
        //TODO: Make this a double!
        //TODO: Why is this a virtual property.
        public virtual float DriftTime {get;set;}
        /// <summary>
        /// The elution time of the feature.
        /// </summary>
        public double ElutionTime { get; set; }
        /// <summary>
        /// The monoisotopic mass of the feature.
        /// </summary>
        public double MassMonoisotopic { get; set; }
        /// <summary>
        /// The aligned monoisotopic mass of the feature.
        /// </summary>
        public double MassMonoisotopicAligned { get; set; }     
        /// <summary>
        /// The normalized elution time (NET) of the feature.
        /// </summary>
        public double NET { get; set; }
        /// <summary>
        /// The aligned NET of the feature.
        /// </summary>
        public double NETAligned { get; set; }        
        /// <summary>
        /// The LC scan of the feature from the raw data.
        /// </summary>
        public virtual int ScanLC { get; set; }
        /// <summary>
        /// The aligned LC scan of the feature.
        /// </summary>
        public int ScanLCAligned { get; set; }   
        /// <summary>
        /// The abundance of the feature.
        /// </summary>
        public int Abundance{ get; set; }
        /// <summary>
        /// The M/Z value of the feature.
        /// </summary>
        public double MZ { get; set; }
		/// <summary>
		/// The Da corrected M/Z value of the feature.
		/// </summary>
		public double MZCorrected { get; set; }
        /// <summary>
        /// The charge state of the feature.
        /// </summary>
        public int ChargeState{get;set;}  
        #endregion

        #region BaseData Members
        /// <summary>
        /// Clears the datatype and resets the raw values to their default values.
        /// </summary>
        public override void Clear()
        {
			this.Abundance                  = 0;
			this.ChargeState                = 0;
			this.DriftTime                  = 0;            
            this.IsDaltonCorrected                  = false;
            this.ElutionTime                = 0;            
			this.ID                         = -1;
			this.MassMonoisotopic           = 0;
			this.MassMonoisotopicAligned    = 0;
			this.MZ                         = 0;
            this.MZCorrected                = 0;            
			this.NET                        = 0;
			this.NETAligned                 = 0;
			this.ScanLC                     = CONST_DEFAULT_SCAN_VALUE;
			this.ScanLCAligned              = CONST_DEFAULT_SCAN_VALUE;
            this.IsSuspicious                 = false;
            
     
            //TODO: Intialize all of the new members
        }
        #endregion

		#region IComparable<Feature> Members
		/// <summary>
		/// Default Comparer used for the Feature class. Sorts by Monoisotopic Mass.
		/// </summary>
		public int CompareTo(Feature other)
		{
			return this.MassMonoisotopic.CompareTo(other.MassMonoisotopic);
		}
		#endregion

        #region Static Comparison Methods
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
        public static Comparison<Feature> ScanLCComparison = delegate(Feature x, Feature y)
        {
            return x.ScanLC.CompareTo(y.ScanLC);
        };
        /// <summary>
        /// Compares the aligned scan of two Features
        /// </summary>
        public static Comparison<Feature> ScanAlignedComparison = delegate(Feature x, Feature y)
        {
            return x.ScanLCAligned.CompareTo(y.ScanLCAligned);
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
		/// <summary>
		/// Compares the LC Scan and then the Monoisotopic Mass of two Features
		/// </summary>
		public static Comparison<Feature> ScanLCAndMassComparison = delegate(Feature x, Feature y)
		{
			if (x.ScanLC != y.ScanLC)
			{
				return x.ScanLC.CompareTo(y.ScanLC);
			}
			else
			{
				return x.MassMonoisotopic.CompareTo(y.MassMonoisotopic);
			}
		};
		/// <summary>
		/// Compares the LC Scan and then the Charge State of two Features
		/// </summary>
		public static Comparison<Feature> ScanLCAndChargeStateComparison = delegate(Feature x, Feature y)
		{
			if (x.ScanLC != y.ScanLC)
			{
				return x.ScanLC.CompareTo(y.ScanLC);
			}
			else
			{
				return x.ChargeState.CompareTo(y.ChargeState);
			}
		};
		/// <summary>
		/// Compares the LC Scan and then the Monoisotopic Mass of two Features
		/// </summary>
		public static Comparison<Feature> ScanLCAndDriftTimeAndMassComparison = delegate(Feature x, Feature y)
		{
			if (x.ScanLC != y.ScanLC)
			{
				return x.ScanLC.CompareTo(y.ScanLC);
			}
			else if (x.DriftTime != y.DriftTime)
			{
				return x.DriftTime.CompareTo(y.DriftTime);
			}
			else
			{
				return x.MassMonoisotopic.CompareTo(y.MassMonoisotopic);
			}
		};
		#endregion

        #region Public Utility Functions
        /// <summary>
        /// Computes the mass difference in parts per million (ppm) for two given masses.
        /// </summary>
        /// <param name="massX">Mass of feature X.</param>
        /// <param name="massY">Mass of feature Y.</param>
        /// <returns>Mass difference in parts per million (ppm).</returns>
        public static double ComputeMassPPMDifference(double massX, double massY)
        {			
            return (massX - massY) * 1e6 / massX;
        }
        /// <summary>
        /// Computes the mass difference in parts per million (ppm) for two given masses.
        /// </summary>
        /// <param name="massX">Mass of feature X.</param>
        /// <param name="massY">Mass of feature Y.</param>
        /// <returns>Mass difference in parts per million (ppm).</returns>
        public static double ComputeDaDifferenceFromPPM(double massX, double ppm)
        {
            return massX - (ppm * 1e-6 * massX);            
        }
		#endregion

        #region Overriden Base Methods
        /// <summary>
        /// Returns a basic string representation of the cluster.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return  "ID = " + ID.ToString() + 
                    " Mono Mass = " + MassMonoisotopic.ToString() + 
                    " NET = " + NET.ToString() +
                    " Drift Time = " + DriftTime.ToString();
        }
        /// <summary>
        /// Compares two objects' values.
        /// </summary>
        /// <param name="obj">Other to compare with.</param>
        /// <returns>True if values are the same, false if not.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Feature other = obj as Feature;
            if (other == null)
                return false;

			if (!this.ID.Equals(other.ID))
			{
				return false;
			}
            if (!Abundance.Equals(other.Abundance))
            {
                return false;
            }
            if (!this.ChargeState.Equals(other.ChargeState))
            {
                return false;
            }
            if (!this.IsDaltonCorrected.Equals(other.IsDaltonCorrected))
            {
                return false;
            }
            if (!this.DriftTime.Equals(other.DriftTime))
            {
                return false;
            }
            if (!this.ElutionTime.Equals(other.ElutionTime))
            {
                return false;
            }
            if (!this.MassMonoisotopic.Equals(other.MassMonoisotopic))
            {
                return false;
            }
            if (!this.MassMonoisotopicAligned.Equals(other.MassMonoisotopicAligned))
            {
                return false;
            }
            if (!this.MZ.Equals(other.MZ))
            {
                return false;
            }
            if (!this.MZCorrected.Equals(other.MZCorrected))
            {
                return false;
            }
            if (!this.NET.Equals(other.NET))
            {
                return false;
            }
            if (!this.NETAligned.Equals(other.NETAligned))
            {
                return false;
            }
            if (!this.ScanLC.Equals(other.ScanLC))
            {
                return false;
            }
            if (!this.ScanLCAligned.Equals(other.ScanLCAligned))
            {
                return false;
            }
            if (!this.IsSuspicious.Equals(other.IsSuspicious))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Generates a hash code.
        /// </summary>
        /// <returns>Hash code based on stored data.</returns>
        public override int GetHashCode()
        {
            int hashCode =
                Abundance.GetHashCode() ^
                ChargeState.GetHashCode() ^
                IsDaltonCorrected.GetHashCode() ^
                DriftTime.GetHashCode() ^
                ElutionTime.GetHashCode() ^                
                ID.GetHashCode() ^
                MZ.GetHashCode() ^
                MZCorrected.GetHashCode() ^
                NETAligned.GetHashCode() ^
                NET.GetHashCode() ^
                ScanLC.GetHashCode() ^
                ScanLCAligned.GetHashCode() ^
                IsSuspicious.GetHashCode();
			
            return hashCode;
        }
        #endregion
    }
}
