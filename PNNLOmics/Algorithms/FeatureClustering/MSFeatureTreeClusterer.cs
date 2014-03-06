using System.IO;
using PNNLOmics.Algorithms.Chromatograms;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// Finds clusters using a tree aproach
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class MsFeatureTreeClusterer<T, U>
        where T: MSFeatureLight, new()
        where U: UMCLight, IFeatureCluster<T>, IFeatureCluster<U>, new ()
    {
        private const int CONST_SCAN_TOLERANCE = 50;

        /// <summary>
        /// Constructor
        /// </summary>
        public MsFeatureTreeClusterer()
        {
            Tolerances = new FeatureTolerances();
            ScanTolerance = CONST_SCAN_TOLERANCE;
        }

        /// <summary>
        /// Gets or sets the tolerances
        /// </summary>
        public FeatureTolerances Tolerances
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the value between 
        /// </summary>
        public int ScanTolerance { get; set; }

        /// <summary>
        /// Gets or sets the object that can be used to go back to the raw data to grab XIC's
        /// </summary>
        public ISpectraProvider SpectraProvider { get; set; }
        
        /// <summary>
        /// Clusters features based on some specified values. 
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <typeparam name="TCluster"></typeparam>
        /// <param name="features"></param>
        /// <param name="sortFunction"></param>
        /// <param name="massDiffFunction"></param>
        /// <param name="comparison"></param>
        /// <param name="massTolerance"></param>
        /// <returns></returns>
        private IEnumerable<TCluster> Cluster<TChild, TCluster>( IEnumerable<TChild>             features, 
                                                                 Comparison<TChild>              sortFunction,
                                                                 Func<TChild, TChild, double>    massDiffFunction,
                                                                 Func<TChild, TChild, int>       comparison,
                                                                 double                          massTolerance) 
                where TChild:   FeatureLight, new()
                where TCluster: FeatureLight, IFeatureCluster<TChild>, new ()
        {
            var clusters            = new List<TCluster>();            
            var currentIndex        = 0;
            var N                   = features.Count();

            // Sort the features based on m/z first
            var msFeatures = new List<TChild>();
            msFeatures.AddRange(features);
            msFeatures.Sort(sortFunction);


            // Iterate through all of the clusters
            while (currentIndex < N)
            {
                var hasGap       = false;
                var lastFeature  = msFeatures[currentIndex];
                var lastIndex    = currentIndex + 1;
                var gapFeatures  = new List<TChild>() { lastFeature };
                while (!hasGap && lastIndex < N)
                {

                    var currentFeature      = msFeatures[lastIndex];


                    var massDiff         = massDiffFunction(currentFeature, lastFeature);

                    // Time to quit
                    if (Math.Abs(massDiff) > massTolerance)
                    {
                        // Stop here...
                        hasGap       = true;
                    }
                    else
                    {
                        // Increment and save this feature
                        lastIndex++;
                        gapFeatures.Add(currentFeature);
                        lastFeature = currentFeature;
                    }
                }
                currentIndex = lastIndex;

                // Now that we have a gap...let's go a head and start building the features
                // first we build a scan dictionary 
                // sorted by scans
                var featureMap = new Dictionary<int, List<TChild>>();
                foreach (var feature in gapFeatures)
                {
                    var scan = feature.Scan;
                    if (!featureMap.ContainsKey(scan))
                        featureMap.Add(scan, new List<TChild>());
                    featureMap[scan].Add(feature);
                }

                // Now build the tree...where each node is a feature.
                var scans = featureMap.Keys.OrderBy(x => x);
                var tree  = new FeatureTree<TChild, TCluster>(comparison);

                foreach (var feature in scans.SelectMany(scan => featureMap[scan]))
                {
                    tree.Insert(feature);
                }

                var newFeatures = tree.Build();
                clusters.AddRange(newFeatures);
            }            
            return clusters;          
        }

        /// <summary>
        /// Finds LCMS Features from MS Features.
        /// </summary>
        /// <param name="rawMsFeatures"></param>
        /// <returns></returns>
        public List<U> Cluster(List<T> rawMsFeatures)
        {            
            Comparison<T> mzSort        = (x, y) => x.Mz.CompareTo(y.Mz);                               
            Comparison<U> monoSort      = (x, y) => x.MassMonoisotopic.CompareTo(y.MassMonoisotopic);
            Func<T, T, double> mzDiff   = (x, y) => Feature.ComputeMassPPMDifference(x.Mz, y.Mz);            
            Func<U, U, double> monoDiff = (x, y) => Feature.ComputeMassPPMDifference(x.MassMonoisotopic, y.MassMonoisotopic);


            var minScan = Convert.ToDouble(rawMsFeatures.Min(x => x.Scan));
            var maxScan = Convert.ToDouble(rawMsFeatures.Max(x => x.Scan));

            foreach (var msFeature in rawMsFeatures)
            {
                msFeature.RetentionTime = (Convert.ToDouble(msFeature.Scan) - minScan)/(maxScan - minScan);
            }


            // First cluster based on m/z finding the XIC's 
            var features     = Cluster<T, U>(    rawMsFeatures, 
                                                        mzSort,
                                                        mzDiff,
                                                        CompareMz,
                                                        Tolerances.Mass);
            
            // Then we group into UMC's for clustering across charge states...
            if (features == null)
                throw new InvalidDataException("No features were found from the input MS Feature list.");


            foreach (var feature in features)            
                feature.RetentionTime = Convert.ToDouble(feature.Scan - minScan)/Convert.ToDouble(maxScan - minScan);
            

            features = Cluster<U, U>(features,
                                    monoSort,
                                    monoDiff,
                                    CompareMonoisotopic,
                                    Tolerances.Mass);


            // Here we should merge the XIC data...trying to find the best possible feature
            // Note that at this point we dont have UMC's.  We only have features
            // that are separated by mass , scan , and charge 
            // so this method should interrogate each one of these....
            if (SpectraProvider != null)
            {
                var generator = new XicCreator();
                //TODO: BLL This could break down...should we just not make this a generic object?
                generator.CreateXic(features as List<UMCLight>, Tolerances.Mass, SpectraProvider);                
            }                
            var id = 0;

            var featureList = features.ToList();
            foreach (var x in featureList) x.ID = id++;
            return featureList;
        }


        #region Comparison Methods
        /// <summary>
        /// Compares a feature to the list of feature
        /// </summary>
        public int CompareMonoisotopic(U featureX, U featureY)
        {
            // If they are in mass range...
            double mzDiff = Feature.ComputeMassPPMDifference(featureX.MassMonoisotopic, featureY.MassMonoisotopic);
            if (Math.Abs(mzDiff) < Tolerances.Mass)
            {
                // otherwise make sure that our scan value is within range
                var scanDiff = featureX.RetentionTime - featureY.RetentionTime;
                //return Math.Abs(scanDiff) < Tolerances.RetentionTime ? 1 : 0;
                return Math.Abs(scanDiff) <= Tolerances.RetentionTime ? 0 : 1;
            }
            if (mzDiff < 0)
                return -1;
            return 1;
        }
        /// <summary>
        /// Compares a feature to the list of feature
        /// </summary>
        public int CompareMz(T featureX, T featureY)
        {
            // If they are in mass range...
            double mzDiff = Feature.ComputeMassPPMDifference(featureX.Mz, featureY.Mz);
            if (Math.Abs(mzDiff) < Tolerances.Mass)
            {
                // otherwise make sure that our scan value is within range
                int scanDiff = featureX.Scan - featureY.Scan;
                if (Math.Abs(scanDiff) > ScanTolerance)
                    return 1;

                return featureX.ChargeState != featureY.ChargeState ? 1 : 0;
            }
            if (mzDiff < 0)
                return -1;
            return 1;
        } 
        #endregion
    }
}
