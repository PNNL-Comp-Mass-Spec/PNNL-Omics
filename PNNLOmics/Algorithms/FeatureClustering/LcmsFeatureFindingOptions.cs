namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// LC-MS Feature Filtering Options.  These options filter LC-MS features
    /// </summary>
	[System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Clustering")]
    public class LcmsFeatureFindingOptions
    {        
        public LcmsFeatureFindingOptions(FeatureTolerances tolerances)
        {
            InstrumentTolerances = tolerances;

            MaximumScanRange = 50;
            MaximumNetRange  = .005;
        }

        public LcmsFeatureFindingOptions()
        {
            InstrumentTolerances = new FeatureTolerances();

            MaximumScanRange = 50;
            MaximumNetRange = .005;
        }

        public FeatureTolerances InstrumentTolerances { get; set; }
        public int               MaximumScanRange { get; set; }
        public double            MaximumNetRange { get; set; }     
    }    
}