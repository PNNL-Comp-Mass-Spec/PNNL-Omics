//using System;
//using System.Collections.Generic;

//namespace PNNLOmics.Data.Features
//{
//    /// <summary>
//    /// MS Feature class that describes a raw or deisotoped feature.
//    /// </summary>
//    //TODO: We need to think about what actually belongs in this class and what belongs elsewhere 
//    public class MSFeature : MSFeatureLight, IComparable<MSFeature>
//    {
//        //TODO: WHO?  Fix the names of the properties and the comments.  Gets or sets needs to be in each.
//        //TODO: How do we drill back to the MS/MS data.  The MSPeak data does not accurately do this.  Abundance mass and mono does not capture this.

//        #region AutoProperties
//        /// <summary>
//        /// The index location (row #) of the MSFeature in the input file.
//        /// </summary>
//        //TODO: What is this? Refactor this somewhere else.
//        public int IndexInFile { get; set; }
//        /// <summary>
//        /// The abundance of the monisotopic peak of the MSFeature.
//        /// </summary>
//        public int AbundanceMono { get; set; }
//        /// <summary>
//        /// The monoisotopic abundance plus2 of the MSFeature.
//        /// </summary>        
//        //TODO: What is this ? For O16/O18 labeled data. Not sure how to design this yet.
//        //TODO: Also update comment to be more descriptive
//        public int AbundancePlus2 { get; set; }
//        /// <summary>
//        /// The IMS Scan of the MSFeature.
//        /// </summary>
//        //TODO: Move to Feature.cs?
//        public int ScanIMS { get; set; }
//        /// <summary>
//        /// The magnitude of Mass Correction of the MSFeature measured in Daltons.
//        /// </summary>
//        public int MassOffset { get; set; }
//        /// <summary>
//        /// The mass of the most abundant Peak of the MS Feature.
//        /// </summary>
//        public double MassMostAbundant { get; set; }
//        /// <summary>
//        /// The fit value of the MSFeature.
//        /// </summary>
//        public float Fit { get; set; }
//        /// <summary>
//        /// The Full-width-half-max value of the MSFeature.
//        /// </summary>
//        public float Fwhm { get; set; }
//        /// <summary>
//        /// The Signal-to-noise ratio of the most abundant peak of the MSFeature.
//        /// </summary>
//        public float SignalToNoiseRatio { get; set; }
//        /// <summary>
//        /// The Original Intensity of the MSFeature.
//        /// </summary>
//        //TODO: Anyone need this? (most likely should be removed completely)
//        public float IntensityOriginal { get; set; }
//        /// <summary>
//        /// The Original TIA Intensity of the MSFeature.
//        /// </summary>      
//        //TODO: Anyone need this? (most likely should be removed completely)
//        public float IntensityOriginalTIA { get; set; }		
//        /// <summary>
//        /// Gets or sets the child fragmentation spectra if it was acquired.
//        /// </summary>
//        public MSSpectra MSMSFragmentation
//        { get; set; }
//        /// <summary>
//        /// Gets or sets the mass to charge ratio.
//        /// </summary>
//        public double MZ
//        {
//            get;
//            set;
//        }
//        #endregion

//        #region BaseData Members
//        /// <summary>
//        /// Clears the datatype and resets the raw values to their default values.
//        /// </summary>
//        public override void Clear()
//        {
//            base.Clear();
//            //TODO: Was this removed?  Does this affect LCMS Feature Finder.
//            IndexInFile            = -1;
//            AbundanceMono          = 0;
//            AbundancePlus2         = 0;
//            IntensityOriginal      = 0;
//            IntensityOriginalTIA   = 0;
//            MassOffset             = 0;
//            ScanIMS                = 0;
//            Fit                    = float.NaN;
//            Fwhm                   = float.NaN;
//            SignalToNoiseRatio     = float.NaN;
//            MsPeakList             = new List<Peak>();
//            MSMSFragmentation      = null;
//        }
//        #endregion

//        #region IComparable<MSFeature> Members
//        /// <summary>
//        /// Default Comparer used for the MSFeature class. Sorts by Monoisotopic Mass descending.
//        /// </summary>
//        public int CompareTo(MSFeature other)
//        {
//            return other.MassMonoisotopic.CompareTo(MassMonoisotopic);
//        }
//        #endregion		
//    }
//}
