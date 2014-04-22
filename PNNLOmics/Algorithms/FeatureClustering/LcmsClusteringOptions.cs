using PNNLOmics.Algorithms.Distance;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    public class LcmsClusteringOptions
    {
        public LcmsClusteringOptions(FeatureTolerances instrumentTolerances)
        {
            InstrumentTolerances = instrumentTolerances;
        }

        public bool                          ShouldSeparateCharge { get; set; }
        public DistanceMetric                DistanceFunction { get; set; }
        public LcmsFeatureClusteringAlgorithmType       LcmsFeatureClusteringAlgorithm { get; set; }
        public ClusterCentroidRepresentation ClusterCentroidRepresentation { get; set; }
        public FeatureTolerances             InstrumentTolerances { get; set; }

        public static FeatureClusterParameters<UMCLight> ConvertToOmics(LcmsClusteringOptions options)
        {                        
            var parameters       = new FeatureClusterParameters<UMCLight>
            {
                Tolerances                  = options.InstrumentTolerances,
                OnlyClusterSameChargeStates = (options.ShouldSeparateCharge == false),
                CentroidRepresentation      = options.ClusterCentroidRepresentation
            };

            parameters.DistanceFunction = DistanceFactory<UMCLight>.CreateDistanceFunction(options.DistanceFunction);
            return parameters;
        }
    }
}