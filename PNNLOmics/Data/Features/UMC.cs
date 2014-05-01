namespace PNNLOmics.Data.Features
{
	/// <summary>
	/// Class that represents LC-MS, IMS-MS, LC-IMS-MS, etc. type data.
	/// </summary>
	/// <remarks>UMC stands for Unique Mass Class - see Advances in Proteomics Data Analysis and Display Using An Accurate Mass and Time Tag Approach in Mass Spectrometry Reviews, 2006. Zimmer et. al.</remarks>
    //public class UMC : Feature, IComparable<UMC>
    //{
    //    /// <summary>
    //    /// Constructor.
    //    /// </summary>
    //    public UMC()
    //    {
    //        Clear();
    //    }
    //    /// <summary>
    //    /// Copy Constructor.
    //    /// </summary>
    //    /// <param name="feature">Feature to copy data from.</param>
    //    public UMC(UMC feature)
    //    {
    //        Clear();
    //        ToBeRemoved                     = feature.ToBeRemoved;
    //        HasDaltonError                  = feature.HasDaltonError;
    //        ChargeMaximum                   = feature.ChargeMaximum;
    //        ChargeMinimum                   = feature.ChargeMinimum;
    //        ChargeState                = feature.ChargeState;
    //        ConformationFitScore       = feature.ConformationFitScore;
    //        ConformationIndex          = feature.ConformationIndex;
    //        DaltonCorrectionMax        = feature.DaltonCorrectionMax;
    //        DriftTime                  = feature.DriftTime;
    //        ElutionTime                = feature.ElutionTime;
    //        FitScoreAverage            = feature.FitScoreAverage;
    //        GroupID                    = feature.GroupID;
    //        HasDaltonError             = feature.HasDaltonError;
    //        ID                         = feature.ID;
    //        IsDaltonCorrected          = feature.IsDaltonCorrected;
    //        IsSuspicious               = feature.IsSuspicious;
    //        MassMonoisotopic           = feature.MassMonoisotopic;
    //        MassMonoisotopicAligned    = feature.MassMonoisotopicAligned;
    //        MassMonoisotopicAverage    = feature.MassMonoisotopicAverage;
    //        MassMonoisotopicMaximum    = feature.MassMonoisotopicMaximum;
    //        MassMonoisotopicMedian     = feature.MassMonoisotopicMedian;
    //        MassMonoisotopicMinimum    = feature.MassMonoisotopicMinimum;
    //        MassMonoisotopicStandardDeviation  = feature.MassMonoisotopicStandardDeviation;
    //        MassOfMaxAbundance                 = feature.MassOfMaxAbundance;

    //        //TODO: Make this more than a reference copy?             
    //        MSFeatureList          = feature.MSFeatureList;
    //        MZ                     = feature.MZ;
    //        MZCorrected            = feature.MZCorrected;
    //        Net                    = feature.Net;
    //        NetAligned             = feature.NetAligned;
    //        ScanLC                 = feature.ScanLC;
    //        ScanLCAligned          = feature.ScanLCAligned;
    //        ScanLCEnd              = feature.ScanLCEnd;
    //        ScanLCOfMaxAbundance   = feature.ScanLCOfMaxAbundance;
    //        ScanLCStart            = feature.ScanLCStart;
    //        ToBeRemoved            = feature.ToBeRemoved;
    //        UmcCluster             = feature.UmcCluster;                        
    //    }

    //    #region AutoProperties and Properties
    //    /// <summary>
    //    /// The ID of the group the feature belongs to.  Where a group could be a dataset or factor.
    //    /// </summary>
    //    public int GroupID { get; set; }
    //    /// <summary>
    //    /// True if the the UMC should be removed from the working List of UMCs.
    //    /// </summary>
    //    //TODO: data is algorithm structure specific
    //    public bool ToBeRemoved { get; set; }
    //    /// <summary>
    //    /// True if the the UMC has been corrected using Dalton Error Correction.
    //    /// </summary>
    //    public bool HasDaltonError { get; set; }
    //    /// <summary>
    //    /// The maximum charge state of the UMC.
    //    /// </summary>
    //    public int ChargeMaximum { get; set; }
    //    /// <summary>
    //    /// The minimum charge state of the UMC.
    //    /// </summary>
    //    public int ChargeMinimum { get; set; }
    //    /// <summary>
    //    /// The maximum abundance of the UMC.
    //    /// </summary>
    //    public int AbundanceMaximum { get; set; }
    //    /// <summary>
    //    /// The sum of the abundance of the UMC.
    //    /// </summary>
    //    public long AbundanceSum { get; set; }
    //    /// <summary>
    //    /// The maximum Dalton Correction to be applied to the UMC.
    //    /// </summary>
    //    public int DaltonCorrectionMax { get; set; }
    //    /// <summary>
    //    /// The index of where the Conformation of the UMC occurred in respect to the rest of the Conformations.
    //    /// </summary>
    //    public int ConformationIndex { get; set; }
    //    /// <summary>
    //    /// The fit score determined by the Conformation Detection algorithm.
    //    /// </summary>
    //    public virtual double ConformationFitScore { get; set; }
    //    /// <summary>
    //    /// The LC Scan where the UMC begins.
    //    /// </summary>
    //    public int ScanLCStart { get; set; }
    //    /// <summary>
    //    /// The LC Scan where the UMC ends. 
    //    /// </summary>
    //    public int ScanLCEnd { get; set; }
    //    /// <summary>
    //    /// The LC Scan that contains the most abundant MS Feature associated with the UMC.
    //    /// </summary>
    //    public int ScanLCOfMaxAbundance { get; set; }
    //    /// <summary>
    //    /// The LC Scan that contains the most abundant MS Feature associated with the UMC.
    //    /// </summary>
    //    public override int ScanLC
    //    {
    //        get { return ScanLCOfMaxAbundance; }
    //        set { ScanLCOfMaxAbundance = value; }
    //    }
    //    /// <summary>
    //    /// The minimum monoisotopic mass of the UMC.
    //    /// </summary>
    //    public double MassMonoisotopicMinimum { get; set; }
    //    /// <summary>
    //    /// The maximum monoisotopic mass of the UMC.
    //    /// </summary>
    //    public double MassMonoisotopicMaximum { get; set; }
    //    /// <summary>
    //    /// The average monoisotopic mass of the UMC.
    //    /// </summary>
    //    public double MassMonoisotopicAverage { get; set; }
    //    /// <summary>
    //    /// The median monoisotopic mass of the UMC.
    //    /// </summary>
    //    public double MassMonoisotopicMedian { get; set; }
    //    /// <summary>
    //    /// The standard deviation of the monoisotopic mass of the UMC.
    //    /// </summary>
    //    public double MassMonoisotopicStandardDeviation { get; set; }
    //    /// <summary>
    //    /// The monoisotopic mass of the most abundant MS Feature associated with the UMC.
    //    /// </summary>
    //    public double MassOfMaxAbundance { get; set; }
    //    /// <summary>
    //    /// The List of MS Features associated with the UMC.
    //    /// </summary>
    //    public virtual List<MSFeature> MSFeatureList { get; set; }
    //    /// <summary>
    //    /// The UMC Cluster that is associated with the UMC.
    //    /// </summary>
    //    public UMCCluster UmcCluster { get; set; }

    //    //TODO: figure out whether or not to include these additional properties:
    //    public double FitScoreAverage { get; set; }
    //    #endregion

    //    #region BaseData Members
    //    /// <summary>
    //    /// Clears the datatype and resets the raw values to their default values.
    //    /// </summary>
    //    public override void Clear()
    //    {
    //        base.Clear();
    //        ToBeRemoved = false;
    //        ConformationIndex = -1;
    //        ChargeMaximum = 0;
    //        AbundanceMaximum = 0;
    //        AbundanceSum = 0;
    //        DaltonCorrectionMax = 0;
    //        MassMonoisotopicMinimum = double.MaxValue;
    //        MassMonoisotopicMaximum = 0;
    //        MassMonoisotopicAverage = 0;
    //        MassMonoisotopicMedian = 0;
    //        MassMonoisotopicStandardDeviation = 0;
    //        MassOfMaxAbundance = double.NaN;
    //        ScanLCStart = int.MaxValue;
    //        ScanLCEnd = 0;
    //        ScanLCOfMaxAbundance = 0;
    //        MSFeatureList = new List<MSFeature>();
    //        UmcCluster = null;
    //    }
    //    #endregion

    //    #region IComparable<LCFeature> Members
    //    /// <summary>
    //    /// Default Comparer used for the LCFeature class. Sorts by Monoisotopic Mass.
    //    /// </summary>
    //    public int CompareTo(UMC other)
    //    {
    //        return MassMonoisotopic.CompareTo(other.MassMonoisotopic);
    //    }

    //    #endregion

    //    #region Comparers
    //    /// <summary>
    //    /// Compares the first LC Scan of two UMCS
    //    /// </summary>
    //    public static Comparison<UMC> ScanLCStartComparison = delegate(UMC x, UMC y)
    //    {
    //        if (x == null || y == null)
    //        {
    //            throw new NullReferenceException("The UMC was null for the comparison.");
    //        }

    //        return x.ScanLCStart.CompareTo(y.ScanLCStart);
    //    };
    //    /// <summary>
    //    /// Compares the last LC Scan of two UMCS
    //    /// </summary>
    //    public static Comparison<UMC> ScanLCEndComparison = delegate(UMC x, UMC y)
    //    {
    //        if (x == null || y == null)
    //        {
    //            throw new NullReferenceException("The UMC was null for the comparison.");
    //        }
    //        return x.ScanLCEnd.CompareTo(y.ScanLCEnd);
    //    };
    //    /// <summary>
    //    /// Compares the summed abundance of two UMCS
    //    /// </summary>
    //    public static Comparison<UMC> AbundanceSumComparison = delegate(UMC x, UMC y)
    //    {
    //        if (x == null || y == null)
    //        {
    //            throw new NullReferenceException("The UMC was null for the comparison.");
    //        }
    //        return x.AbundanceSum.CompareTo(y.AbundanceSum);
    //    };
    //    /// <summary>
    //    /// Compares the maximum abundance of two UMCS
    //    /// </summary>
    //    public static Comparison<UMC> AbundanceMaximumComparison = delegate(UMC x, UMC y)
    //    {
    //        if (x == null || y == null)
    //        {
    //            throw new NullReferenceException("The UMC was null for the comparison.");
    //        }
    //        return x.AbundanceMaximum.CompareTo(y.AbundanceMaximum);
    //    };
    //    /// <summary>
    //    /// Compares the maximum charge state of two UMCS
    //    /// </summary>
    //    public static Comparison<UMC> ChargeMaximumComparison = delegate(UMC x, UMC y)
    //    {
    //        if (x == null || y == null)
    //        {
    //            throw new NullReferenceException("The UMC was null for the comparison.");
    //        }
    //        return x.ChargeMaximum.CompareTo(y.ChargeMaximum);
    //    };
    //    /// <summary>
    //    /// Compares the LC Scan that contains the most abundant MS Feature of two UMCS
    //    /// </summary>
    //    public static Comparison<UMC> ScanLCOfMaxAbundanceComparison = delegate(UMC x, UMC y)
    //    {
    //        if (x == null || y == null)
    //        {
    //            throw new NullReferenceException("The UMC was null for the comparison.");
    //        }
    //        return x.ScanLCOfMaxAbundance.CompareTo(y.ScanLCOfMaxAbundance);
    //    };
    //    /// <summary>
    //    /// Compares the maximum monoisotopic mass of two UMCS
    //    /// </summary>
    //    public static Comparison<UMC> MassMaxComparison = delegate(UMC x, UMC y)
    //    {
    //        if (x == null || y == null)
    //        {
    //            throw new NullReferenceException("The UMC was null for the comparison.");
    //        }
    //        return x.MassMonoisotopicMaximum.CompareTo(y.MassMonoisotopicMaximum);
    //    };
    //    /// <summary>
    //    /// Compares the minimum monoisotopic mass of two UMCS
    //    /// </summary>
    //    public static Comparison<UMC> MassMinComparison = delegate(UMC x, UMC y)
    //    {
    //        if (x == null || y == null)
    //        {
    //            throw new NullReferenceException("The UMC was null for the comparison.");
    //        }
    //        return x.MassMonoisotopicMinimum.CompareTo(y.MassMonoisotopicMinimum);
    //    };
    //    /// <summary>
    //    /// Compares the monoisotopic mass of the most abundant MS Features associated with two UMCS
    //    /// </summary>
    //    public static Comparison<UMC> MassofMaxAbundanceComparison = delegate(UMC x, UMC y)
    //    {
    //        if (x == null || y == null)
    //        {
    //            throw new NullReferenceException("The UMC was null for the comparison.");
    //        }
    //        return x.MassOfMaxAbundance.CompareTo(y.MassOfMaxAbundance);
    //    };
    //    /// <summary>
    //    /// Compares the first LC Scan then the monoisotopic mass of two UMCS
    //    /// </summary>
    //    public static Comparison<UMC> ScanLCStartAndMassComparison = delegate(UMC x, UMC y)
    //    {
    //        if (x == null || y == null)
    //        {
    //            throw new NullReferenceException("The UMC was null for the comparison.");
    //        }

    //        if (x.ScanLCStart != y.ScanLCStart)
    //        {
    //            return x.ScanLCStart.CompareTo(y.ScanLCStart);
    //        }
    //        return x.MassMonoisotopicMedian.CompareTo(y.MassMonoisotopicMedian);
    //    };
    //    /// <summary>
    //    /// Compares the representative LC Scan and the median monoisotopic mass of two UMCS
    //    /// </summary>
    //    public static Comparison<UMC> ScanLCAndMedianMassComparison = delegate(UMC x, UMC y)
    //    {
    //        if (x == null || y == null)
    //        {
    //            throw new NullReferenceException("The UMC was null for the comparison.");
    //        }
    //        if (x.ScanLC != y.ScanLC)
    //        {
    //            return x.ScanLC.CompareTo(y.ScanLC);
    //        }
    //        return x.MassMonoisotopicMedian.CompareTo(y.MassMonoisotopicMedian);
    //    };
    //    /// <summary>
    //    /// Compares the representative LC Scan and the monoisotopic mass of the most abundant MS Feature of two UMCS
    //    /// </summary>
    //    public static Comparison<UMC> ScanLCAndMassOfMaxAbundanceComparison = delegate(UMC x, UMC y)
    //    {
    //        if (x == null || y == null)
    //        {
    //            throw new NullReferenceException("The UMC was null for the comparison.");
    //        }
    //        if (x.ScanLC != y.ScanLC)
    //        {
    //            return x.ScanLC.CompareTo(y.ScanLC);
    //        }
    //        return x.MassOfMaxAbundance.CompareTo(y.MassOfMaxAbundance);
    //    };
    //    #endregion

    //    #region Overriden Base Methods
    //    /// <summary>
    //    /// Returns a basic string representation of the cluster.
    //    /// </summary>
    //    /// <returns></returns>
    //    public override string ToString()
    //    {
    //        return "UMC " + base.ToString();
    //    }
    //    /// <summary>
    //    /// Compares two objects' values to each other.
    //    /// </summary>
    //    /// <param name="obj">Object to compare to.</param>
    //    /// <returns>True if similar, False if not.</returns>
    //    public override bool Equals(object obj)
    //    {
    //        if (obj == null)
    //            return false;

    //        var umc = obj as UMC;
    //        if (umc == null)
    //            return false;

    //        if (ID != umc.ID)
    //            return false;

    //        var isBaseEqual = base.Equals(umc);
    //        if (!isBaseEqual)
    //            return false;

    //        //TODO: Add other data ScanLCEnd etc.

    //        return true;
    //    }
    //    /// <summary>
    //    /// Computes a hash code for the cluster.
    //    /// </summary>
    //    /// <returns>Hashcode as an integer.</returns>
    //    public override int GetHashCode()
    //    {
    //        return base.GetHashCode();
    //    }
    //    #endregion
    //}


}
